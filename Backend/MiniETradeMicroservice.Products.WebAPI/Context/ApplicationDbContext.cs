using Microsoft.EntityFrameworkCore;
using MiniETradeMicroservice.Products.WebAPI.Entities;

namespace MiniETradeMicroservice.Products.WebAPI.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() { }
        public ApplicationDbContext(DbContextOptions options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(getAppSettingsFile().GetConnectionString("SqlServer"));
            base.OnConfiguring(optionsBuilder);
        }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().Property(p => p.Price).HasColumnType("money");
            base.OnModelCreating(modelBuilder);
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
