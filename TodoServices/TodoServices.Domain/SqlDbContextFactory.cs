using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Shared.Enum;
using System.IO;

namespace TodoServices.Domain
{

    public class SqlDbContextFactory : IDesignTimeDbContextFactory<SqlDbContext>
    {
        public SqlDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json")
                .Build();

            var connectionString = DbProvider.GetConnectionString(configuration, DbProvider.SqlServer);

            var sqlBuilder = new DbContextOptionsBuilder<SqlDbContext>();
            sqlBuilder.UseSqlServer(connectionString);

            return new SqlDbContext(sqlBuilder.Options);
        }

    }
}
