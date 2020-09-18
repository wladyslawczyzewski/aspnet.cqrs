using System;
using Microsoft.Extensions.DependencyInjection;

namespace VladyslavChyzhevskyi.ASPNET.CQRS
{
    public static class IServiceCollectionExtensions
    {
        public static void AddCQRS(this IServiceCollection services, Action<CQRSOptions> configure)
        {
            services.AddOptions<CQRSOptions>().Configure(configure);
            services.AddSingleton<ICQRSFeatureProvider, CQRSFeatureProvider>();
        }
    }
}
