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

            var query = Activator.CreateInstance(type, ctorArgs) as IQuery;
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
    }
}
