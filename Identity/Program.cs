using EnergySuite.SelfService.Identity.Data;
using EnergySuite.SelfService.Identity.Data.Seeding;
using EnergySuite.SelfService.Identity.Models;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SerilogTimings;
using Shared.Enum;
using System;
using System.Reflection;

namespace EnergySuite.SelfService.Identity
{


    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = Assembly.GetExecutingAssembly().FullName;

            var host = CreateBuildWebHost(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var env = services.GetRequiredService<IHostingEnvironment>();
                var myDbContext = services.GetService<MyDbContext>();
                var grantDbContext = services.GetService<PersistedGrantDbContext>();
                var configDbContext = services.GetService<ConfigurationDbContext>();

                using (Operation.Time("Migrating Identity database"))
                {
                    myDbContext.Database.Migrate();
                }

                using (Operation.Time("Migrating PersistedGrant database"))
                {
                    grantDbContext.Database.Migrate();
                }


                using (Operation.Time("Migrating Configuration database"))
                {
                    configDbContext.Database.Migrate();
                }

                if (env.IsDevelopment())
                {
                    try
                    {
                        SeedData.EnsureSeedData(configDbContext);
                        Log.Debug("Persisted Grant database seeding complete");

                        var userManager = services.GetRequiredService<UserManager<MyUser>>();
                        SeedData.EnsureSeedData(userManager, myDbContext);
                        Log.Debug("Identity database seeding complete");
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "An error occurred while seeding the database.");
                    }
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateBuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls(APIUrls.IdentityServer);
    }
}