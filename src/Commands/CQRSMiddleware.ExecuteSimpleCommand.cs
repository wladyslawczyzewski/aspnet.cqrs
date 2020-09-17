using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using VladyslavChyzhevskyi.ASPNET.CQRS.Commands;
using VladyslavChyzhevskyi.ASPNET.CQRS.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace VladyslavChyzhevskyi.ASPNET.CQRS
{
    partial class CQRSMiddleware
    {
        private async Task ExecuteSimpleCommand(HttpContext httpContext, IServiceScope scope, CQRSRouteDescriptor descriptor)
        {
            var type = descriptor.UnderlyingType;
            var ctors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
            ctors.ThrowExceptionIfTheresMoreThenOneCtor(descriptor, _logger);

            var ctorArgs = ctors.Single().ResolveCtorArguments(scope);

            var command = Activator.CreateInstance(type, ctorArgs) as ICommand;
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
    }
}
