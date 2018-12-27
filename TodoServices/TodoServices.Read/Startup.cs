using TodoServices.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Shared.Enum;
using Microsoft.AspNetCore.Mvc.Versioning;
using TodoServices.Domain.Postgres;
using TodoServices.Read.Application;
using Shared.Queue.Rabbit;
using Shared.Common.Interfaces;

namespace TodoServices.Read
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

            var activeProvider = DbProvider.GetActiveProvider(Configuration);
            var connectionString = DbProvider.GetConnectionString(Configuration, activeProvider);

            switch (activeProvider)
            {
                case DbProvider.Postgres:
                    services.AddEntityFrameworkNpgsql()
                        .AddDbContext<PostgresDbContext>(optionsAction => optionsAction.UseNpgsql(connectionString));
                    break;
                case DbProvider.SqlLite:
                    services.AddEntityFrameworkSqlite()
                        .AddDbContext<SqlDbContext>(optionsAction => optionsAction.UseSqlite(connectionString));
                    break;
                default:
                    services.AddEntityFrameworkSqlServer()
                        .AddDbContext<SqlDbContext>(optionsAction => optionsAction.UseSqlServer(connectionString));
                    break;
            }

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
                    options.ApiName = IdentityScope.TodoRead;
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
                var ctx = scope.ServiceProvider.GetService<SqlDbContext>();
                ctx = ctx?? scope.ServiceProvider.GetService<PostgresDbContext>();

                // adjust listening to the event bus
                MessageLogic.Avatar = new MessageLogic(ctx, bus);
            }

            app.UseAuthentication();

            app.UseMvcWithDefaultRoute();

            Log.Information("{0} started", this.GetType().Assembly.FullName);
        }
    }
}
