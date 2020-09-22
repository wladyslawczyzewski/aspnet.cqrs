using System;
using Microsoft.Extensions.DependencyInjection;

namespace VladyslavChyzhevskyi.ASPNET.CQRS
{
    public static class IServiceCollectionExtensions
    {
        public static void AddCQRS(this IServiceCollection services, Action<CQRSOptions> configure)
        {
            services.AddOptions<CQRSOptions>()
                .Configure(configure)
                .PostConfigure(options =>
                {
                    if (options.Assemblies.Length == 0)
                    {
                        throw new ArgumentNullException(nameof(options.Assemblies), "Assemblies is required configuration option.");
                    }

                    if (string.IsNullOrWhiteSpace(options.BasePath))
                    {
                        options.BasePath = "/";
                    }
                });
            services.AddSingleton<ICQRSFeatureProvider, CQRSFeatureProvider>();
        }
    }
}
