using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using VladyslavChyzhevskyi.ASPNET.CQRS.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace VladyslavChyzhevskyi.ASPNET.CQRS
{
    public class CQRSMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CQRSMiddleware> _logger;
        private readonly CQRSFeature _feature = new CQRSFeature();
        private readonly ConcurrentDictionary<string, CQRSRouteDescriptor> queryCache = new ConcurrentDictionary<string, CQRSRouteDescriptor>();

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
                var descriptor = queryCache.GetOrAdd(path, GetQueryTypeForGivenPath);
                if (descriptor == null)
                {
                    httpContext.Response.Clear();
                    httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return;
                }

                if (!descriptor.IsSimple)
                {
                    await ExecuteComplexQuery(httpContext, descriptor);
                    return;
                }

                var query = Activator.CreateInstance(descriptor.UnderlyingType) as IQuery;
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

        private async Task ExecuteComplexQuery(HttpContext httpContext, CQRSRouteDescriptor descriptor)
        {
            if (!httpContext.Request.QueryString.HasValue)
            {
                httpContext.Response.Clear();
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return;
            }

            var queryString = QueryHelpers
                .ParseNullableQuery(httpContext.Request.QueryString.Value)
                .ToDictionary(
                    x => x.Key,
                    x => x.Value.Count == 1
                        ? (object)x.Value.ElementAtOrDefault(0)
                        : (object)x.Value);
            var queryStringSerializedToJson = JsonConvert.SerializeObject(queryString);
            var argument = JsonConvert.DeserializeObject(queryStringSerializedToJson, descriptor.ParameterType);

            var query = Activator.CreateInstance(descriptor.UnderlyingType);
            var method = descriptor.UnderlyingType
                .GetMethod(nameof(IQuery<object, object>.Execute), BindingFlags.Instance | BindingFlags.Public);
            var methodInvoke = (Task)method.Invoke(query, new[] { argument });
            await methodInvoke.ConfigureAwait(false);
            var result = methodInvoke.GetType()
                .GetProperty(nameof(Task<object>.Result), BindingFlags.Instance | BindingFlags.Public)
                .GetValue(methodInvoke);

            httpContext.Response.Clear();
            httpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(result));
        }

        private CQRSRouteDescriptor? GetQueryTypeForGivenPath(string path)
        {
            return _feature.Queries.SingleOrDefault(q => q.Path == path);
        }
    }
}
