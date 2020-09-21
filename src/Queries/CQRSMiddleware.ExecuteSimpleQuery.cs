using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using VladyslavChyzhevskyi.ASPNET.CQRS.Helpers;
using VladyslavChyzhevskyi.ASPNET.CQRS.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace VladyslavChyzhevskyi.ASPNET.CQRS
{
    partial class CQRSMiddleware
    {
        private async Task ExecuteSimpleQuery(HttpContext httpContext, IServiceScope scope, CQRSRouteDescriptor descriptor)
        {
            var type = descriptor.UnderlyingType;
            var ctors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
            ctors.ThrowExceptionIfTheresMoreThenOneCtor(descriptor, _logger);

            var ctorArgs = ctors.Single().ResolveCtorArguments(scope);

            var executeMethod = type.GetMethod(nameof(IQuery<object>.Execute), BindingFlags.Instance | BindingFlags.Public);
            var resultPropType = executeMethod
                ?.ReturnType
                ?.GetProperty(nameof(Task<object>.Result), BindingFlags.Instance | BindingFlags.Public)
                ?.PropertyType;

            if (resultPropType == null)
            {
                var query = Activator.CreateInstance(type, ctorArgs) as IQuery;
                await query.Execute();
                httpContext.ClearAndSetStatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                var result = await ReflectionHelpers.ExecuteQueryAndGetResult(type, ctorArgs, null);
                httpContext.ClearAndSetStatusCode(HttpStatusCode.OK);
                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(result));
            }
        }
    }
}
