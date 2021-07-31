using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using ASPNET.CQRS.Helpers;
using ASPNET.CQRS.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace ASPNET.CQRS
{
    partial class CQRSMiddleware
    {
        private readonly ConcurrentDictionary<string, CQRSHandlerDescriptor> queryCache = new ConcurrentDictionary<string, CQRSHandlerDescriptor>();

        private readonly Func<Type, bool> _isQueryWithOutput = @interface => @interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IQueryHandler<,>);

        private async Task HandleQuery(HttpContext httpContext, IServiceScope scope, string path)
        {
            var descriptor = queryCache.GetOrAdd(path, GetQueryTypeForGivenPath);
            if (descriptor == null)
            {
                httpContext.ClearAndSetStatusCode(HttpStatusCode.NotFound);
                return;
            }

            Dictionary<string, object> queryString = new Dictionary<string, object>();
            if (httpContext.Request.QueryString.HasValue)
            {
                queryString = QueryHelpers
                                .ParseQuery(httpContext.Request.QueryString.Value)
                                .ToDictionary(
                                    x => x.Key,
                                    x => x.Value.Count == 1
                                        ? (object)x.Value.ElementAtOrDefault(0)
                                        : (object)x.Value);
            }

            var queryHandlerType = descriptor.HandlerType;
            var queryType = descriptor.HandlerParameterType;
            var query = queryString.Any()
                ? JsonConvert.DeserializeObject(JsonConvert.SerializeObject(queryString), queryType)
                : Activator.CreateInstance(queryType);

            var queryHandlerCtors = queryHandlerType.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
            queryHandlerCtors.ThrowExceptionIfTheresMoreThenOneCtor(descriptor, _logger);
            var queryHandlerCtorArgs = queryHandlerCtors[0].ResolveCtorArguments(scope);

            if (descriptor.HandlerOutputType != null)
            {
                var result = await ReflectionHelpers.HandleQueryAndGetResult(queryHandlerType, queryHandlerCtorArgs, query);
                httpContext.ClearAndSetStatusCode(HttpStatusCode.OK);
                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(result));
            }
            else
            {
                await ReflectionHelpers.HandleQuery(queryHandlerType, queryHandlerCtorArgs, query);
                httpContext.ClearAndSetStatusCode(HttpStatusCode.NoContent);
            }
        }
    }
}
