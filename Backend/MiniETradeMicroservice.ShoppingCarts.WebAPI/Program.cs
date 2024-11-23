using Microsoft.EntityFrameworkCore;
using MiniETradeMicroservice.Products.WebAPI.Models;
using MiniETradeMicroservice.ShoppingCarts.WebAPI.Context;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlServer"));
});
builder.Services.Configure<List<ApiUrl>>(builder.Configuration.GetSection("ApiUrls"));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var srv = scope.ServiceProvider;
    var context = srv.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}

app.Run();
