using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VladyslavChyzhevskyi.ASPNET.CQRS.Commands;
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
        private readonly ConcurrentDictionary<string, CQRSRouteDescriptor> commandCache = new ConcurrentDictionary<string, CQRSRouteDescriptor>();

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
                await ExecuteCommand(httpContext, path);
            }
            else if (method == "get")
            {
                await ExecuteQuery(httpContext, path);
            }
            else
            {
                _logger.LogError($"Not supported method: {method}");
                httpContext.ClearAndSetStatusCode(HttpStatusCode.InternalServerError);
            }
        }

        private async Task ExecuteCommand(HttpContext httpContext, string path)
        {
            var descriptor = commandCache.GetOrAdd(path, GetCommandForGivePath);
            if (descriptor == null)
            {
                httpContext.ClearAndSetStatusCode(HttpStatusCode.NotFound);
                return;
            }

            if (!descriptor.IsSimple)
            {
                await ExecuteComplexCommand(httpContext, descriptor);
            }
            else
            {
                await ExecuteSimpleCommand(httpContext, descriptor);
            }
        }

        private async Task ExecuteQuery(HttpContext httpContext, string path)
        {
            var descriptor = queryCache.GetOrAdd(path, GetQueryTypeForGivenPath);
            if (descriptor == null)
            {
                httpContext.ClearAndSetStatusCode(HttpStatusCode.NotFound);
                return;
            }

            if (!descriptor.IsSimple)
            {
                await ExecuteComplexQuery(httpContext, descriptor);
            }
            else
            {
                await ExecuteSimpleQuery(httpContext, descriptor);
            }
        }

        private async Task ExecuteSimpleCommand(HttpContext httpContext, CQRSRouteDescriptor descriptor)
        {
            var command = Activator.CreateInstance(descriptor.UnderlyingType) as ICommand;
            try
            {
                await command.Execute();
                httpContext.ClearAndSetStatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Caught exception");
                httpContext.ClearAndSetStatusCode(HttpStatusCode.InternalServerError);
            }
        }

        private async Task ExecuteComplexCommand(HttpContext httpContext, CQRSRouteDescriptor descriptor)
        {
            var input = string.Empty;
            using (var a = new StreamReader(httpContext.Request.Body, Encoding.UTF8))
            {
                input = await a.ReadToEndAsync();
            }

            var argument = JsonConvert.DeserializeObject(input, descriptor.ParameterType);

            var command = Activator.CreateInstance(descriptor.UnderlyingType);
            var method = descriptor.UnderlyingType
                .GetMethod(nameof(ICommand<object>.Execute), BindingFlags.Instance | BindingFlags.Public);
            var methodInvoke = (Task)method.Invoke(command, new[] { argument });
            await methodInvoke.ConfigureAwait(false);

            httpContext.ClearAndSetStatusCode(HttpStatusCode.NoContent);
        }

        private async Task ExecuteSimpleQuery(HttpContext httpContext, CQRSRouteDescriptor descriptor)
        {
            var query = Activator.CreateInstance(descriptor.UnderlyingType) as IQuery;
            try
            {
                await query.Execute();
                httpContext.ClearAndSetStatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Caught exception");
                httpContext.ClearAndSetStatusCode(HttpStatusCode.InternalServerError);
            }
        }

        private async Task ExecuteComplexQuery(HttpContext httpContext, CQRSRouteDescriptor descriptor)
        {
            if (!httpContext.Request.QueryString.HasValue)
            {
                httpContext.ClearAndSetStatusCode(HttpStatusCode.BadRequest);
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

            httpContext.ClearAndSetStatusCode(HttpStatusCode.OK);
            await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(result));
        }

        private CQRSRouteDescriptor? GetQueryTypeForGivenPath(string path)
        {
            return _feature.Queries.SingleOrDefault(q => q.Path == path);
        }

        private CQRSRouteDescriptor GetCommandForGivePath(string path)
        {
            return _feature.Commands.SingleOrDefault(c => c.Path == path);
        }
    }
}
