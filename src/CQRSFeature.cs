namespace VladyslavChyzhevskyi.ASPNET.CQRS
{
    public class CQRSFeature
    {
        public string BasePath { get; internal set; }

        public CQRSHandlerDescriptor[] Queries { get; internal set; }

        public CQRSHandlerDescriptor[] Commands { get; internal set; }
    }
}
