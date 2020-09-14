using Microsoft.AspNetCore.Builder;

namespace VladyslavChyzhevskyi.ASPNET.CQRS
{
    public static class CQRSMiddlewareExtensions
    {
        public static void UseCQRS(this IApplicationBuilder applicationBuilder, string pathStartsWith)
        {
            applicationBuilder.MapWhen(
                ctx => ctx.Request.Path.Value.StartsWith(pathStartsWith),
                appBuilder => appBuilder.UseMiddleware<CQRSMiddleware>());
        }
    }
}
