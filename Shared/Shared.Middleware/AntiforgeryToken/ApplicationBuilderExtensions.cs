using Microsoft.AspNetCore.Builder;

namespace Shared.Common.Middleware.AntiforgeryToken
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseAntiforgeryTokens(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ValidateAntiForgeryTokenMiddleware>();
        }

    }
}