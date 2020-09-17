using System;
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
        private async Task ExecuteComplexQuery(HttpContext httpContext, IServiceScope scope, CQRSRouteDescriptor descriptor)
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

            var type = descriptor.UnderlyingType;
            var ctors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
            ctors.ThrowExceptionIfTheresMoreThenOneCtor(descriptor, _logger);

            var ctorArgs = ctors.Single().ResolveCtorArguments(scope);

            var query = Activator.CreateInstance(type, ctorArgs);
            var method = type
                .GetMethod(nameof(IQuery<object, object>.Execute), BindingFlags.Instance | BindingFlags.Public);
            var methodInvoke = (Task)method.Invoke(query, new[] { argument });
            await methodInvoke.ConfigureAwait(false);
            var result = methodInvoke.GetType()
                .GetProperty(nameof(Task<object>.Result), BindingFlags.Instance | BindingFlags.Public)
                .GetValue(methodInvoke);

            httpContext.ClearAndSetStatusCode(HttpStatusCode.OK);
            await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(result));
        }
    }
}
