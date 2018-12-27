using Microsoft.Extensions.Configuration;

namespace Shared.Enum
{
    public sealed class DbProvider
    {
        public const string Active = "Active";

        public const string SqlServer = "SqlServer";
        public const string Postgres = "Postgres";
        public const string SqlLite = "Sqlite";
        public const string SqlAzure = "SqlAzure";

        public static string GetActiveProvider(IConfiguration config)
        {
            return config.GetConnectionString(Active);
        }

        public static string GetConnectionString(IConfiguration config, string active)
        {
            return config.GetConnectionString(active);
        }

        public static string GetConnectionString(IConfiguration config)
        {
            string active = GetActiveProvider(config);
            return config.GetConnectionString(active);
        }
    }
}
