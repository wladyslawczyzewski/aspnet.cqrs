using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace VladyslavChyzhevskyi.ASPNET.CQRS
{
    partial class CQRSMiddleware
    {
        private async Task ExecuteQuery(HttpContext httpContext, IServiceScope scope, string path)
        {
            var descriptor = queryCache.GetOrAdd(path, GetQueryTypeForGivenPath);
            if (descriptor == null)
            {
                httpContext.ClearAndSetStatusCode(HttpStatusCode.NotFound);
                return;
            }

            if (!descriptor.IsSimple)
            {
                await ExecuteComplexQuery(httpContext, scope, descriptor);
            }
            else
            {
                await ExecuteSimpleQuery(httpContext, scope, descriptor);
            }
        }
    }
}
