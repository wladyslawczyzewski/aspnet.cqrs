namespace VladyslavChyzhevskyi.ASPNET.CQRS
{
    internal interface ICQRSFeatureProvider
    {
        void Configure(CQRSOptions options);
        CQRSFeature Get();
    }
}
