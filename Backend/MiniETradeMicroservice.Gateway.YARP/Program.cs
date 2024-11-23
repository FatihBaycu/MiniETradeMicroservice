using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MiniETradeMicroservice.Gateway.YARP.Context;
using MiniETradeMicroservice.Gateway.YARP.Models;
using MiniETradeMicroservice.Gateway.YARP.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var yarpConfiguration = builder.Configuration.GetSection("ReverseProxy");
builder.Services.Configure<JWTTokenOptionModel>(builder.Configuration.GetSection("JWTTokenOptions"));

JWTTokenOptionModel jwtTokenOptions = builder.Configuration.GetSection("JWTTokenOptions").Get<JWTTokenOptionModel>();

builder.Services.AddSingleton<IJWTService, JWTManager>();
builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlServer"));
});
builder.Services.AddReverseProxy().LoadFromConfig(yarpConfiguration);

#region Jwt
if (jwtTokenOptions == null)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("JWT token configuration not exist.");
    Console.ForegroundColor = ConsoleColor.Black;
}
builder.Services.AddAuthentication().AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtTokenOptions?.Issuer,
        ValidAudience = jwtTokenOptions?.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenOptions?.SecretKey)),
        ValidateLifetime = true,
    };
});

#endregion

var app = builder.Build();

app.UseCors(p => p.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
app.MapControllers();
app.MapGet("/", () => "Hello from MiniETradeMicroservice.Gateway.YARP");

using (var scope = app.Services.CreateScope())
{
    try
    {
        var srv = scope.ServiceProvider;
        var context = srv.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(ex.Message);
        Console.ForegroundColor = ConsoleColor.White;
    }
}

try
{
    app.MapReverseProxy();
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(ex.Message);
    Console.ForegroundColor = ConsoleColor.White;
}

app.UseAuthentication();
app.UseAuthorization();
app.Run();
