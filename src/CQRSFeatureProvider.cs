using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using VladyslavChyzhevskyi.ASPNET.CQRS.Commands;
using VladyslavChyzhevskyi.ASPNET.CQRS.Queries;

[assembly: InternalsVisibleTo("VladyslavChyzhevskyi.ASPNET.CQRS.Tests")]

namespace VladyslavChyzhevskyi.ASPNET.CQRS
{

    public class CQRSFeatureProvider : ICQRSFeatureProvider
    {
        internal static Func<Type, bool> IsSimpleQuerySelector = type => !type.IsInterface && type.GetInterfaces().Any(@interface => @interface == typeof(IQuery));

        internal static Func<Type, bool> IsComplexQuerySelector = type => !type.IsInterface && type.GetInterfaces().Any(@interface => @interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IQuery<,>));

        private static Func<Type, bool> IsCommandSelector = @interface => @interface == typeof(ICommand) // command without parameters and output
                                                || (@interface.IsGenericTypeDefinition && @interface.GetGenericTypeDefinition() == typeof(ICommand<>)) // command with parameters && without output
                                                ;

        private CQRSFeature _feature;

        public void Configure(string pathStartsWith, params Assembly[] assemblies)
        {
            _feature = new CQRSFeature();
            _feature.PathStartsWith = pathStartsWith;

            var appDomainExportedTypes = assemblies
                            .SelectMany(assembly => assembly.GetExportedTypes());
            _feature.Queries = appDomainExportedTypes
                .Where(type => IsSimpleQuerySelector(type)
                                || IsComplexQuerySelector(type))
                .SelectMany(type => type
                    .GetCustomAttributes<CQRSRouteAttribute>(false)
                    .Select(routeAttrib => new CQRSRouteDescriptor
                    {
                        Path = routeAttrib.Path,
                        IsQuery = true,
                        IsSimple = IsSimpleQuerySelector(type) && !IsComplexQuerySelector(type),
                        UnderlyingType = type
                    }))
                .ToArray();

            _feature.Commands = appDomainExportedTypes
                .Where(type => !type.IsInterface && type.GetInterfaces().Any(IsSimpleQuerySelector))
                .ToArray();
        }

        public CQRSFeature Get()
        {
            return _feature ?? throw new NullReferenceException();
        }
    }
}
