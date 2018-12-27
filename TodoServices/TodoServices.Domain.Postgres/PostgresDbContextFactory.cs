using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Shared.Enum;
using System.IO;

namespace TodoServices.Domain.Postgres
{
    public class PostgresDbContextFactory : IDesignTimeDbContextFactory<PostgresDbContext>
    {
        public PostgresDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json")
                .Build();

            var connectionString = DbProvider.GetConnectionString(configuration, DbProvider.Postgres);

            var postgresBuilder = new DbContextOptionsBuilder<PostgresDbContext>();
            postgresBuilder.UseNpgsql(connectionString);

            return new PostgresDbContext(postgresBuilder.Options);
        }

    }
}
