using System.IdentityModel.Tokens.Jwt;
using Shared.Enum;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Serilog;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Localization;
using Shared.Common.Middleware.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using Microsoft.AspNetCore.Localization.Routing;
using System.Linq;
using System.Collections.Generic;

namespace MVC
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

            // cookies from 2.1
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
            });

            // localization work
            services.AddMvc(opts =>
            {
                opts.Conventions.Insert(0, new LocalizationConvention());
                opts.Filters.Add(new MiddlewareFilterAttribute(typeof(LocalizationPipeline)));
            })
            .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
            .AddDataAnnotationsLocalization();

            //services.AddLocalization(ops => ops.ResourcesPath = "Resources"); // native localization
            services.AddPortableObjectLocalization(opt => opt.ResourcesPath = "Localization");
        

            var locOptions = new RequestLocalizationOptions()
            {
                DefaultRequestCulture = new RequestCulture(SupportedCultures.Items.Last()),
                SupportedCultures = SupportedCultures.Items,
                SupportedUICultures = SupportedCultures.Items
            };

            locOptions.RequestCultureProviders = new[] {
                new RouteDataRequestCultureProvider()
                    {
                        RouteDataStringKey = "culture",
                        Options = locOptions
                    }
                };

            services.AddSingleton(locOptions);


            services.Configure<RouteOptions>(opts =>
                opts.ConstraintMap.Add("culturecode", typeof(CultureRouteConstraint)));


            // auth work
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "Cookies";
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.SignInScheme = "Cookies";

                    options.Authority = APIUrls.IdentityServer;
                    options.RequireHttpsMetadata = false;

                    options.ClientId = IdentityScope.TodoClient;//.MVCClientName;
                    options.ClientSecret = IdentityScope.Secret;
                    options.ResponseType = "code id_token";

                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;


                    options.Scope.Add(IdentityScope.TodoRead); 
                    options.Scope.Add(IdentityScope.TodoCmd);

                    options.Scope.Add("offline_access");
                });

            services.AddResponseCompression();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHttpsRedirection();
                app.UseHsts();
            }

            app.UseResponseCompression();
            app.UseAuthentication();
            app.UseCookiePolicy();

            // read more here https://andrewlock.net/adding-cache-control-headers-to-static-files-in-asp-net-core/
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    const int durationInSeconds = 60 * 60 * 24;
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] =
                        "public,max-age=" + durationInSeconds;
                }
            });

            //app.UseMvcWithDefaultRoute();
            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                           name: "default",
                           template: "{culture:culturecode}/{controller=Home}/{action=Index}/{id?}");

                routes.MapGet("{culture:culturecode}/{*path}", appBuilder =>
                {
                    appBuilder.Response.StatusCode = 404;
                    return Task.FromResult(0);
                });

                routes.MapGet("{*path}", (RequestDelegate)(ctx =>
                {
                    var defaultCulture = locOptions.Value.DefaultRequestCulture.Culture.Name;
                    var path = ctx.GetRouteValue("path") ?? string.Empty;
                    var culturedPath = $"{ctx.Request.PathBase}/{defaultCulture}/{path}";
                    //var culturedPath = $"/{defaultCulture}/{path}";
                    ctx.Response.Redirect(culturedPath);
                    return Task.CompletedTask;
                }));
            });

            Log.Information("{0} started", this.GetType().Assembly.FullName);
        }
    }

    public sealed class SupportedCultures
    {
        static CultureInfo[] _items = new[]
                {
                    new CultureInfo("uk-UA"),
                    new CultureInfo("en-US"),
                };

        public static CultureInfo[] Items
        {
            get
            {
                return _items;
            }
        }
    }
}
