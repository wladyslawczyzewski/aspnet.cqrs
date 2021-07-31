using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace ASPNET.CQRS
{
    public static class CQRSMiddlewareExtensions
    {
        public static void UseCQRS(this IApplicationBuilder applicationBuilder)
        {
            var opts = applicationBuilder.ApplicationServices
                .GetService(typeof(IOptions<CQRSOptions>)) as IOptions<CQRSOptions>;
            applicationBuilder.MapWhen(
                ctx => ctx.Request.Path.Value.StartsWith(opts.Value.BasePath),
                appBuilder => appBuilder.UseMiddleware<CQRSMiddleware>());
        }
    }
}
