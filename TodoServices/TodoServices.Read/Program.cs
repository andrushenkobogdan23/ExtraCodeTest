using System;
using System.Reflection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Shared.Enum;

namespace TodoServices.Read
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = Assembly.GetExecutingAssembly().FullName;

            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls(APIUrls.Todos)
                .Build();
    }
}
