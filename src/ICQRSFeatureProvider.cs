using System.Reflection;

namespace VladyslavChyzhevskyi.ASPNET.CQRS
{
    public interface ICQRSFeatureProvider
    {
        void Configure(string pathStartsWith, params Assembly[] assemblies);
        CQRSFeature Get();
    }
}
