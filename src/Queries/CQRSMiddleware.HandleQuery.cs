using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using VladyslavChyzhevskyi.ASPNET.CQRS.Helpers;
using VladyslavChyzhevskyi.ASPNET.CQRS.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace VladyslavChyzhevskyi.ASPNET.CQRS
{
    partial class CQRSMiddleware
    {
        private readonly ConcurrentDictionary<string, CQRSRouteDescriptor> queryCache = new ConcurrentDictionary<string, CQRSRouteDescriptor>();

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

            var queryHandlerType = descriptor.UnderlyingType;
            var queryDefinition = descriptor.UnderlyingType
                .GetInterfaces()
                .FirstOrDefault(@interface => (@interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IQueryHandler<>))
                    || _isQueryWithOutput(@interface));
            var queryType = queryDefinition
                .GetGenericArguments()
                .ElementAt(0);
            var query = queryString.Any()
                ? JsonConvert.DeserializeObject(JsonConvert.SerializeObject(queryString), queryType)
                : Activator.CreateInstance(queryType);

            var queryHandlerCtors = queryHandlerType.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
            queryHandlerCtors.ThrowExceptionIfTheresMoreThenOneCtor(descriptor, _logger);
            var queryHandlerCtorArgs = queryHandlerCtors.Single().ResolveCtorArguments(scope);

            if (_isQueryWithOutput(queryDefinition))
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
