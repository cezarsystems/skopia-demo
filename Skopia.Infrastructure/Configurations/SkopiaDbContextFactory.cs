using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Skopia.Infrastructure.Data;

namespace Skopia.Infrastructure.Configurations
{
    public class SkopiaDbContextFactory : IDesignTimeDbContextFactory<SkopiaDbContext>
    {
        public SkopiaDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<SkopiaDbContext>();
            var connectionString = config.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlite(connectionString);

            return new SkopiaDbContext(optionsBuilder.Options);
        }
    }
}