using System.Collections.Concurrent;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace VladyslavChyzhevskyi.ASPNET.CQRS
{
    partial class CQRSMiddleware
    {
        private readonly ConcurrentDictionary<string, CQRSRouteDescriptor> commandCache = new ConcurrentDictionary<string, CQRSRouteDescriptor>();

        private async Task HandleCommand(HttpContext httpContext, IServiceScope scope, string path)
        {
            var descriptor = commandCache.GetOrAdd(path, GetCommandForGivePath);
            if (descriptor == null)
            {
                httpContext.ClearAndSetStatusCode(HttpStatusCode.NotFound);
                return;
            }

            if (!descriptor.IsSimple)
            {
                await HandleComplexCommand(httpContext, scope, descriptor);
            }
            else
            {
                await HandleSimpleCommand(httpContext, scope, descriptor);
            }
        }
    }
}
