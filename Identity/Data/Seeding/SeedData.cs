using System.Linq;
using EnergySuite.SelfService.Identity.Models;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Serilog;
using SerilogTimings;

namespace EnergySuite.SelfService.Identity.Data.Seeding
{
    public class SeedData
    {
        //public static void EnsureSeedData(IServiceProvider serviceProvider)
        //{
        //    Log.Information("start seeding IdentityServer database ...");

        //    using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
        //    {
        //        using (Operation.Time("Migrating PersistedGrant database"))
        //        {
        //            scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
        //        }

        //        var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

        //        using (Operation.Time("Migrating Configuration database"))
        //        {
        //            context.Database.Migrate();
        //        }

        //        EnsureSeedData(context);
        //    }

        //    Log.Information("Done seeding database!");
        //}

        internal static void EnsureSeedData(ConfigurationDbContext context)
        {

            using (var op = Operation.Begin("IdentityServer clients population procedure"))
            {
                if (!context.Clients.Any())
                {
                    Log.Information("IdentityServer clients being populated");
                    foreach (var client in SeedConfig.GetClients().ToList())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                    op.Complete();
                }

            }


            using (var op = Operation.Begin("IdentityServer IdentityResources population procedure"))
            {


                if (!context.IdentityResources.Any())
                {
                    Log.Information("IdentityServer IdentityResources being populated");

                    foreach (var resource in SeedConfig.GetIdentityResources().ToList())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                    op.Complete();
                }
            }


            using (var op = Operation.Begin("IdentityServer ApiResources population procedure"))
            {
                if (!context.ApiResources.Any())
                {
                    Log.Information("IdentityServer ApiResources being populated");
                    foreach (var resource in SeedConfig.GetApiResources().ToList())
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges(); op.Complete();
                }

            }
        }

        internal static void EnsureSeedData(UserManager<MyUser> userManager, MyDbContext context)
        {
            using (var op = Operation.Begin("Identity users population procedure"))
            {
               
                if (!context.Users.Any())
                {
                    Log.Information("Identity users population being populated");
                    foreach (var user in SeedConfig.GetUsers().ToList())
                    {
                        var result = userManager.CreateAsync(user, "Admin@123456").Result;
                    }
                    op.Complete();
                }

            }
        }
    }
}