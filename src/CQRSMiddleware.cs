using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace VladyslavChyzhevskyi.ASPNET.CQRS
{
    public partial class CQRSMiddleware
    {
        private readonly ILogger<CQRSMiddleware> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly CQRSFeature _feature = new CQRSFeature();

        public CQRSMiddleware(
            RequestDelegate next,
            ILogger<CQRSMiddleware> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _feature = serviceProvider.GetService<ICQRSFeatureProvider>().Get();
            _serviceProvider = serviceProvider;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var path = httpContext.Request.Path.Value.Substring(_feature.PathStartsWith.EndsWith('/') ? _feature.PathStartsWith.Length - 1 : _feature.PathStartsWith.Length);
            string method = httpContext.Request.Method.ToLowerInvariant();
            _logger.LogTrace($"Executing request: {path}");

            using (var scope = _serviceProvider.CreateScope())
            {
                if (method == "post")
                {
                    await HandleCommand(httpContext, scope, path);
                }
                else if (method == "get")
                {
                    await HandleQuery(httpContext, scope, path);
                }
                else
                {
                    _logger.LogError($"Not supported method: {method}");
                    httpContext.ClearAndSetStatusCode(HttpStatusCode.MethodNotAllowed);
                }
            }
        }

        private CQRSHandlerDescriptor GetQueryTypeForGivenPath(string path) => _feature.Queries.SingleOrDefault(q => q.Path == path);

        private CQRSHandlerDescriptor GetCommandForGivePath(string path) => _feature.Commands.SingleOrDefault(c => c.Path == path);
    }
}
