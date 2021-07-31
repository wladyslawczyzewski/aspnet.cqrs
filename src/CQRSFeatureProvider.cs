using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using ASPNET.CQRS.Commands;
using ASPNET.CQRS.Queries;
using Microsoft.Extensions.Options;

[assembly: InternalsVisibleTo("ASPNET.CQRS.Tests")]

namespace ASPNET.CQRS
{

    internal class CQRSFeatureProvider : ICQRSFeatureProvider
    {
        internal static Func<Type, bool> IsSimpleQuerySelector = type => !type.IsInterface && GetSimpleQueryDefinition(type) != null;

        internal static Func<Type, bool> IsComplexQuerySelector = type => !type.IsInterface && GetComplexQueryDefinition(type) != null;

        internal static Func<Type, bool> IsSimpleCommandSelector = type => !type.IsInterface && GetSimpleCommandDefinition(type) != null;

        internal static Func<Type, bool> IsFireAndForgetCommandSelector = type => !type.IsInterface && GetFireAndForgetCommandDefinition(type) != null;

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
            _feature.BasePath = pathStartsWith;

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
                        return new CQRSHandlerDescriptor
                        {
                            Path = routeAttrib.Path,
                            HandlerType = type,
                            HandlerParameterType = isSimple && !isComplex
                                ? GetSimpleQueryDefinition(type).GetGenericArguments().ElementAt(0)
                                : GetComplexQueryDefinition(type).GetGenericArguments().ElementAt(0),
                            HandlerOutputType = !isSimple && isComplex
                                ? GetComplexQueryDefinition(type).GetGenericArguments().ElementAt(1)
                                : null
                        };
                    }))
                .ToArray();

            _feature.Commands = appDomainExportedTypes
                .Where(type => IsSimpleCommandSelector(type) || IsFireAndForgetCommandSelector(type))
                .SelectMany(type => type.GetCustomAttributes<CQRSRouteAttribute>(false)
                .Select(routeAttrib =>
                {
                    bool isSimple = IsSimpleCommandSelector(type);
                    bool isFireAndForget = IsFireAndForgetCommandSelector(type);
                    return new CQRSHandlerDescriptor
                    {
                        Path = routeAttrib.Path,
                        HandlerType = type,
                        HandlerParameterType = isSimple
                            ? GetSimpleCommandDefinition(type).GetGenericArguments().ElementAt(0)
                            : isFireAndForget
                                ? GetFireAndForgetCommandDefinition(type).GetGenericArguments().ElementAt(0)
                                : throw new ArgumentOutOfRangeException(),
                        HandlerOutputType = null,
                    };
                }))
                .ToArray();
        }

        public CQRSFeature Get()
        {
            return _feature ?? throw new NullReferenceException();
        }

        private static Type GetSimpleQueryDefinition(Type type)
        {
            return type.GetInterfaces()
                .FirstOrDefault(@interface => @interface.IsGenericType
                    && @interface.GetGenericTypeDefinition() == typeof(IQueryHandler<>));
        }

        private static Type GetComplexQueryDefinition(Type type)
        {
            return type.GetInterfaces()
                .FirstOrDefault(@interface => @interface.IsGenericType
                    && @interface.GetGenericTypeDefinition() == typeof(IQueryHandler<,>));
        }

        private static Type GetSimpleCommandDefinition(Type type)
        {
            return type.GetInterfaces()
                .FirstOrDefault(@interface => @interface.IsGenericType
                    && @interface.GetGenericTypeDefinition() == typeof(ICommandHandler<>));
        }

        private static Type GetFireAndForgetCommandDefinition(Type type)
        {
            return type.GetInterfaces()
                .FirstOrDefault(@interface => @interface.IsGenericType
                    && @interface.GetGenericTypeDefinition() == typeof(IFireAndForgetCommandHandler<>));
        }
    }
}
