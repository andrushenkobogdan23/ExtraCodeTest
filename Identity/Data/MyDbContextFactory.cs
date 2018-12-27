using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Shared.Enum;
using System.IO;

namespace EnergySuite.SelfService.Identity.Data
{
    /// <summary>
    /// to read https://docs.microsoft.com/en-us/ef/core/miscellaneous/configuring-dbcontext
    /// </summary>
    public class MyDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
    {
        public MyDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json")
                .Build();

            var activeProvider = DbProvider.GetActiveProvider(configuration);
            var connectionString = DbProvider.GetConnectionString(configuration, activeProvider);
            var builder = new DbContextOptionsBuilder<MyDbContext>();

            switch (activeProvider)
            {
                case DbProvider.Postgres:
                    builder.UseNpgsql(connectionString);
                    break;
                case DbProvider.SqlLite:
                    builder.UseSqlite(connectionString);
                    break;
                default:
                    builder.UseSqlServer(connectionString);
                    break;
            }
            return new MyDbContext(builder.Options);
        }
    }
}