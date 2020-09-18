namespace VladyslavChyzhevskyi.ASPNET.CQRS
{
    public interface ICQRSFeatureProvider
    {
        void Configure(CQRSOptions options);
        CQRSFeature Get();
    }
}
