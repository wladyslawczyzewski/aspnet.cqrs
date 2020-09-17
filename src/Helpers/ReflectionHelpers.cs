using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace VladyslavChyzhevskyi.ASPNET.CQRS.Helpers
{
    internal static class ReflectionHelpers
    {
        public static void ThrowExceptionIfTheresMoreThenOneCtor(this ConstructorInfo[] ctors, CQRSRouteDescriptor descriptor, ILogger logger)
        {
            if (ctors.Length > 1)
            {
                logger.LogError($"Type {descriptor.UnderlyingType.FullName} has more then one constructore. We support only a commands with one constructor.");
                throw new InvalidOperationException();
            }
        }

        public static object[] ResolveCtorArguments(this ConstructorInfo ctor, IServiceScope scope)
        {
            return ctor.GetParameters()
                .Select(param => scope.ServiceProvider.GetRequiredService(param.ParameterType))
                .ToArray();
        }
    }
}
