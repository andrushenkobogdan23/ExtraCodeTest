using TodoServices.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Shared.Enum;
using Shared.Common;
using Shared.Queue.Rabbit;
using Microsoft.AspNetCore.Mvc.Versioning;
using TodoServices.Domain.Postgres;
using Shared.Common.Sql;
using Shared.Common.Interfaces;

namespace TodoServices.Command
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
                    services.AddScoped<IUnitOfWork, UnitOfWork<PostgresDbContext>>(); 
                    break;
                case DbProvider.SqlLite:
                    services.AddEntityFrameworkSqlite()
                        .AddDbContext<SqlDbContext>(optionsAction => optionsAction.UseSqlite(connectionString));
                    services.AddScoped<IUnitOfWork, UnitOfWork<SqlDbContext>>();
                    break;
                default:
                    services.AddEntityFrameworkSqlServer()
                        .AddDbContext<SqlDbContext>(optionsAction => optionsAction.UseSqlServer(connectionString));
                    services.AddScoped<IUnitOfWork, UnitOfWork<SqlDbContext>>();
                    break;
            }

            // add API versioning https://github.com/Microsoft/aspnet-api-versioning/wiki/API-Version-Reader
            services.AddApiVersioning(o => o.ApiVersionReader = ApiVersionReader.Combine(
                    new QueryStringApiVersionReader(),
                    new HeaderApiVersionReader()
                    {
                        HeaderNames = { "api-version" }
                    }));

            //
            services.AddSingleton<IMessageBus>(s => new RabbitMessageBus(APIUrls.Rabbit));


            // core services
            services.AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters();

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = APIUrls.IdentityServer;
                    options.RequireHttpsMetadata = false;

                    options.ApiName = IdentityScope.TodoCmd;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
            Log.Information("{0} started", this.GetType().Assembly.FullName);
        }
    }
}
