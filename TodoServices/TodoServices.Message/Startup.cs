using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Shared.Enum;
using Microsoft.AspNetCore.Mvc.Versioning;
using TodoServices.Message.Application;
using Shared.Common;
using Shared.Queue.Rabbit;
using Shared.Common.Interfaces;

namespace TodoServices.Message
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // initialize Serilog logger
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();

            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

            // add reading messages if u have some cached data and need to update it
            services.AddSingleton<IMessageBus>(s => new RabbitMessageBus(APIUrls.Rabbit));

            // add API versioning https://github.com/Microsoft/aspnet-api-versioning/wiki/API-Version-Reader
            services.AddApiVersioning(o => o.ApiVersionReader = ApiVersionReader.Combine(
                    new QueryStringApiVersionReader(),
                    new HeaderApiVersionReader()
                    {
                        HeaderNames = { "api-version" }
                    }));

            // core services
            services.AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters();

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = APIUrls.IdentityServer;
                    options.RequireHttpsMetadata = false;
                    options.ApiName = IdentityScope.TodoMessage;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var bus = scope.ServiceProvider.GetService<IMessageBus>();

                MessageLogic.Avatar = new MessageLogic(bus);
            }

            app.UseAuthentication();

            app.UseMvcWithDefaultRoute();

            Log.Information("{0} started", this.GetType().Assembly.FullName);
        }
    }
}
