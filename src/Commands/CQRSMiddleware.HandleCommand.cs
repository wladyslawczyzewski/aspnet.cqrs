using System;
using System.Collections.Concurrent;
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
        private readonly ConcurrentDictionary<string, CQRSRouteDescriptor> commandCache = new ConcurrentDictionary<string, CQRSRouteDescriptor>();

        private async Task HandleCommand(HttpContext httpContext, IServiceScope scope, string path)
        {
            var descriptor = commandCache.GetOrAdd(path, GetCommandForGivePath);
            if (descriptor == null)
            {
                httpContext.ClearAndSetStatusCode(HttpStatusCode.NotFound);
                return;
            }

            var input = string.Empty;
            if (httpContext.Request.Body.CanRead)
            {
                using (var buffer = new StreamReader(httpContext.Request.Body, Encoding.UTF8))
                {
                    input = await buffer.ReadToEndAsync();
                }
            }

            var commandType = descriptor.UnderlyingType
                .GetInterfaces()
                .FirstOrDefault(@interface => @interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(ICommandHandler<>))
                .GetGenericArguments()
                .ElementAt(0);
            var command = string.IsNullOrWhiteSpace(input)
                ? Activator.CreateInstance(commandType)
                : JsonConvert.DeserializeObject(input, commandType);

            var commandHandlerType = descriptor.UnderlyingType;
            var commandHandlerCtors = commandHandlerType.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
            commandHandlerCtors.ThrowExceptionIfTheresMoreThenOneCtor(descriptor, _logger);
            var commandHandlerCtorArgs = commandHandlerCtors.Single().ResolveCtorArguments(scope);

            var commandHandler = Activator.CreateInstance(commandHandlerType, commandHandlerCtorArgs);
            var handleMethod = commandHandlerType
                .GetMethod(nameof(ICommandHandler<ICommand>.Handle), BindingFlags.Instance | BindingFlags.Public);
            var handleMethodInvocation = (Task)handleMethod.Invoke(commandHandler, new[] { command });
            await handleMethodInvocation.ConfigureAwait(false);

            httpContext.ClearAndSetStatusCode(HttpStatusCode.NoContent);
        }
    }
}
