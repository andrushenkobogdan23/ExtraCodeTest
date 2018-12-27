using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.Reflection;
using Shared.Enum;

namespace MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = Assembly.GetExecutingAssembly().FullName;

            CreateBuildFrontendWebHost(args).Build().Run();
        }

        public static IWebHostBuilder CreateBuildFrontendWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls(APIUrls.MVC);
    }
}
