using Microsoft.EntityFrameworkCore;
using MiniETradeMicroservice.Products.WebAPI.Context;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(opt =>
{
    opt.ListenAnyIP(5501);
});
builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

WebApplication app = builder.Build();

app.MapControllers();

app.MapGet("/", () => "Hello World!");

using (IServiceScope scope = app.Services.CreateScope())
{
    IServiceProvider srv = scope.ServiceProvider;
    using ApplicationDbContext context = srv.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}
app.Run();
