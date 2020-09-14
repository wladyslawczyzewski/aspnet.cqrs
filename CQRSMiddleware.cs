using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using VladyslavChyzhevskyi.ASPNET.CQRS.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace VladyslavChyzhevskyi.ASPNET.CQRS
{
    public class CQRSMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CQRSMiddleware> _logger;
        private readonly CQRSFeature _feature = new CQRSFeature();
        private readonly ConcurrentDictionary<string, Type> queryCache = new ConcurrentDictionary<string, Type>();

        public CQRSMiddleware(RequestDelegate next, ILogger<CQRSMiddleware> logger, ICQRSFeatureProvider featureProvider)
        {
            _next = next;
            _logger = logger;
            _feature = featureProvider.Get();
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var path = httpContext.Request.Path.Value.Substring(_feature.PathStartsWith.Length - 1);
            string method = httpContext.Request.Method.ToLowerInvariant();
            _logger.LogTrace($"Executing request: {path}");

            if (method == "post")
            {
                // commands not implemented yet
                httpContext.Response.Clear();
                httpContext.Response.StatusCode = (int)HttpStatusCode.NotImplemented;
            }
            else if (method == "get")
            {
                var queryType = queryCache.GetOrAdd(path, GetQueryTypeForGivenPath);
                if (queryType == null)
                {
                    httpContext.Response.Clear();
                    httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return;
                }

                if (queryType.IsAssignableFrom(typeof(IQuery<,>)))
                {
                    // complex queries not implemented yet
                    httpContext.Response.Clear();
                    httpContext.Response.StatusCode = (int)HttpStatusCode.NotImplemented;
                    return;
                }

                var query = Activator.CreateInstance(queryType) as IQuery;
                try
                {
                    await query.Execute();
                    httpContext.Response.Clear();
                    httpContext.Response.StatusCode = (int)HttpStatusCode.NoContent;
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Caught exception");
                    httpContext.Response.Clear();
                    httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                }
            }
            else
            {
                _logger.LogError($"Not supported method: {method}");
                httpContext.Response.Clear();
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
        }

        private Type GetQueryTypeForGivenPath(string path)
        {
            return _feature.Queries.SingleOrDefault(q => q.GetCustomAttributes(typeof(CQRSRouteAttribute), false)
                        .OfType<CQRSRouteAttribute>()
                        .Any(routeAttrib => routeAttrib.Path == path));
        }
    }
}
