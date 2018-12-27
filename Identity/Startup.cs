using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EnergySuite.SelfService.Identity.Data;
using EnergySuite.SelfService.Identity.Models;
using EnergySuite.SelfService.Identity.Services;
using System.Reflection;
using Serilog;
using Shared.Enum;
using Microsoft.AspNetCore.Mvc;

namespace EnergySuite.SelfService.Identity
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

            // my db context tge same for all providers
            var activeProvider = DbProvider.GetActiveProvider(Configuration);
            var connectionString = DbProvider.GetConnectionString(Configuration, activeProvider);

            switch (activeProvider)
            {
                case DbProvider.Postgres:
                    services.AddDbContext<MyDbContext>(options =>
                        options.UseNpgsql(connectionString));
                    break;
                case DbProvider.SqlLite:
                    services.AddDbContext<MyDbContext>(options =>
                        options.UseSqlite(connectionString));
                    break;
                default:
                    services.AddDbContext<MyDbContext>(options =>
                        options.UseSqlServer(connectionString));
                    break;
            }

            services.AddIdentity<MyUser, MyRole>()
                .AddEntityFrameworkStores<MyDbContext>()
                .AddDefaultTokenProviders();
            
            // add application insight
            //services.AddApplicationInsightsTelemetry(Configuration);

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            // cookies from 2.1
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
            });

            services.AddMvc();
            //services.AddMvc()
            //    .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            // configure identity server with in-memory stores, keys, clients and scopes
            services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
                // TODO: need to replace it with real self signed certificate
                // https://stackoverflow.com/questions/46142991/what-is-the-signing-credential-in-identityserver4
                // https://blogs.msdn.microsoft.com/robert_mcmurray/2013/11/15/how-to-trust-the-iis-express-self-signed-certificate/
                .AddDeveloperSigningCredential() 
                .AddAspNetIdentity<MyUser>()
                // this adds the config data from DB (clients, resources)
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 30;
                });

            services.AddAuthentication();
            //.AddGoogle("Google", options =>
            //{
            //    options.ClientId = "434483408261-55tc8n0cs4ff1fe21ea8df2o443v2iuc.apps.googleusercontent.com";
            //    options.ClientSecret = "3gcoTrEDPPJ0ukn_aYYT6PWo";
            //})
            //.AddOpenIdConnect("oidc", "OpenID Connect", options =>
            //{
            //    options.Authority = "https://demo.identityserver.io/";
            //    options.ClientId = "implicit";
            //    options.SaveTokens = true;

            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        NameClaimType = "name",
            //        RoleClaimType = "role"
            //    };
            //});

            services.AddResponseCompression();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseBrowserLink();
                app.UseDatabaseErrorPage();

            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHttpsRedirection();
                app.UseHsts();
            }

            app.UseResponseCompression();
            app.UseCookiePolicy();
            app.UseIdentityServer();

            // read more here https://andrewlock.net/adding-cache-control-headers-to-static-files-in-asp-net-core/
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    const int durationInSeconds = 60 * 60 * 24;
                    ctx.Context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.CacheControl] =
                        "public,max-age=" + durationInSeconds;
                }
            });

            app.UseMvcWithDefaultRoute();

            Log.Information("{0} started", this.GetType().Assembly.FullName);
        }
    }
}
