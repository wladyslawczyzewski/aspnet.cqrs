using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using VladyslavChyzhevskyi.ASPNET.CQRS.Commands;
using VladyslavChyzhevskyi.ASPNET.CQRS.Queries;
using Microsoft.Extensions.Options;

[assembly: InternalsVisibleTo("VladyslavChyzhevskyi.ASPNET.CQRS.Tests")]

namespace VladyslavChyzhevskyi.ASPNET.CQRS
{

    internal class CQRSFeatureProvider : ICQRSFeatureProvider
    {
        internal static Func<Type, bool> IsSimpleQuerySelector = type => !type.IsInterface && GetSimpleQueryDefinition(type) != null;

        internal static Func<Type, bool> IsComplexQuerySelector = type => !type.IsInterface && GetComplexQueryDefinition(type) != null;

        internal static Func<Type, bool> IsSimpleCommandSelector = type => !type.IsInterface && type.GetInterfaces().Any(@interface => @interface == typeof(ICommandHandler));

        internal static Func<Type, bool> IsComplexCommandSelector = type => !type.IsInterface && GetComplexCommandDefinition(type) != null;

        private CQRSFeature _feature;

        public CQRSFeatureProvider(IOptions<CQRSOptions> options)
        {
            Configure(options.Value);
        }

        public void Configure(CQRSOptions options)
        {
            var pathStartsWith = options.BasePath;
            var assemblies = options.Assemblies;
            _feature = new CQRSFeature();
            _feature.PathStartsWith = pathStartsWith;

            var appDomainExportedTypes = assemblies
                            .SelectMany(assembly => assembly.GetExportedTypes());
            _feature.Queries = appDomainExportedTypes
                .Where(type => IsSimpleQuerySelector(type)
                                || IsComplexQuerySelector(type))
                .SelectMany(type => type
                    .GetCustomAttributes<CQRSRouteAttribute>(false)
                    .Select(routeAttrib =>
                    {
                        bool isSimple = IsSimpleQuerySelector(type);
                        bool isComplex = IsComplexQuerySelector(type);
                        return new CQRSRouteDescriptor
                        {
                            Path = routeAttrib.Path,
                            IsQuery = true,
                            IsSimple = isSimple && !isComplex,
                            UnderlyingType = type,
                            ParameterType = isComplex ? GetComplexQueryDefinition(type).GetGenericArguments().ElementAtOrDefault(0) : null,
                            ResultType = isComplex ? GetComplexQueryDefinition(type).GetGenericArguments().ElementAtOrDefault(1) : GetSimpleQueryDefinition(type)?.GetGenericArguments()?.ElementAtOrDefault(0)
                        };
                    }))
                .ToArray();

            _feature.Commands = appDomainExportedTypes
                .Where(type => IsSimpleCommandSelector(type)
                                || IsComplexCommandSelector(type))
                .SelectMany(type => type.GetCustomAttributes<CQRSRouteAttribute>(false)
                .Select(routeAttrib =>
                {
                    bool isSimple = IsSimpleCommandSelector(type);
                    bool isComplex = IsComplexCommandSelector(type);
                    return new CQRSRouteDescriptor
                    {
                        Path = routeAttrib.Path,
                        IsQuery = false,
                        IsSimple = isSimple && !isComplex,
                        UnderlyingType = type,
                        ParameterType = isComplex ? GetComplexCommandDefinition(type).GetGenericArguments().ElementAtOrDefault(0) : null,
                        ResultType = null
                    };
                }))
                .ToArray();
        }

        public CQRSFeature Get()
        {
            return _feature ?? throw new NullReferenceException();
        }

        private static Type GetComplexQueryDefinition(Type type)
        {
            return type.GetInterfaces()
                .FirstOrDefault(@interface => @interface.IsGenericType
                    && @interface.GetGenericTypeDefinition() == typeof(IQueryHandler<,>));
        }

        private static Type GetSimpleQueryDefinition(Type type)
        {
            return type.GetInterfaces()
                .FirstOrDefault(@interface => @interface.IsGenericType
                    && @interface.GetGenericTypeDefinition() == typeof(IQueryHandler<>));
        }

        private static Type GetComplexCommandDefinition(Type type)
        {
            return type.GetInterfaces()
                .FirstOrDefault(@interface => @interface.IsGenericType
                    && @interface.GetGenericTypeDefinition() == typeof(ICommandHandler<>));
        }
    }
}
