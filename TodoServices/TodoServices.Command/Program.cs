using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using TodoServices.Domain;
using TodoServices.Domain.Postgres;
using Shared.Enum;

namespace TodoServices.Command
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = Assembly.GetExecutingAssembly().FullName;

            var host = BuildWebHost(args);

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var env = services.GetRequiredService<IHostingEnvironment>();
                var toSeed = env.IsDevelopment();

                // find wel known service
                var context = services.GetService<SqlDbContext>();
                context = context ?? services.GetService<PostgresDbContext>();
                //TodoContextInitializer.RemoveData(context);
                //context = context ?? services.GetService<SqlDbContext>(); // add another context, for example SqlLiteDbContext

                try
                {
                    context.Database.Migrate();
                    Log.Information("Database migration complete");
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex, "An error occurred while migrating the database.");
                }

                if (toSeed)
                {
                    try
                    {
                        TodoContextInitializer.Seed(context);
                        Log.Information("Database seeding complete");

                    }
                    catch (Exception ex)
                    {
                        Log.Fatal(ex, "An error occurred while seeding the database.");
                    }
                }
            }

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls(APIUrls.TodosCmd)
                .Build();
    }
}