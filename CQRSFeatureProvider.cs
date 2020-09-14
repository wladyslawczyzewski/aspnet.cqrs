using System;
using System.Linq;
using System.Reflection;
using VladyslavChyzhevskyi.ASPNET.CQRS.Commands;
using VladyslavChyzhevskyi.ASPNET.CQRS.Queries;

namespace VladyslavChyzhevskyi.ASPNET.CQRS
{

    public class CQRSFeatureProvider : ICQRSFeatureProvider
    {
        private static Func<Type, bool> IsQuerySelector = @interface => @interface == typeof(IQuery) // query without parameters and output
                                                || (@interface.IsGenericTypeDefinition && @interface.GetGenericTypeDefinition() == typeof(IQuery<,>)) // query with parameters && output
                                                ;

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
                .Where(type => !type.IsInterface && type.GetInterfaces().Any(IsQuerySelector))
                .ToArray();

            _feature.Commands = appDomainExportedTypes
                .Where(type => !type.IsInterface && type.GetInterfaces().Any(IsQuerySelector))
                .ToArray();
        }

        public CQRSFeature Get()
        {
            return _feature ?? throw new NullReferenceException();
        }
    }
}
