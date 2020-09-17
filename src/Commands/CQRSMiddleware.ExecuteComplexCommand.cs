using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VladyslavChyzhevskyi.ASPNET.CQRS.Commands;
using VladyslavChyzhevskyi.ASPNET.CQRS.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace VladyslavChyzhevskyi.ASPNET.CQRS
{
    partial class CQRSMiddleware
    {

        private async Task ExecuteComplexCommand(HttpContext httpContext, IServiceScope scope, CQRSRouteDescriptor descriptor)
        {
            var input = string.Empty;
            using (var a = new StreamReader(httpContext.Request.Body, Encoding.UTF8))
            {
                input = await a.ReadToEndAsync();
            }

            var argument = JsonConvert.DeserializeObject(input, descriptor.ParameterType);

            var type = descriptor.UnderlyingType;
            var ctors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
            ctors.ThrowExceptionIfTheresMoreThenOneCtor(descriptor, _logger);

            var ctorArgs = ctors.Single().ResolveCtorArguments(scope);

            var command = Activator.CreateInstance(type, ctorArgs);
            var method = type
                .GetMethod(nameof(ICommand<object>.Execute), BindingFlags.Instance | BindingFlags.Public);
            var methodInvoke = (Task)method.Invoke(command, new[] { argument });
            await methodInvoke.ConfigureAwait(false);

            httpContext.ClearAndSetStatusCode(HttpStatusCode.NoContent);
        }

    }
}