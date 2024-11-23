using Microsoft.EntityFrameworkCore;
using MiniETradeMicroservice.Gateway.YARP.Entities;

namespace MiniETradeMicroservice.Gateway.YARP.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() { }
        public ApplicationDbContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(getAppSettingsFile().GetConnectionString("PostgreSqlServer"));
            base.OnConfiguring(optionsBuilder);
        }
        private IConfigurationRoot getAppSettingsFile()
        {
            string appsettingsJsonFile = "appsettings.json";
#if DEBUG
            appsettingsJsonFile = "appsettings.Development.json";
#endif
            IConfigurationRoot configuration = new ConfigurationBuilder()
           .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
           .AddJsonFile(appsettingsJsonFile)
           .Build();
            return configuration;
        }
    }
}