namespace VladyslavChyzhevskyi.ASPNET.CQRS
{
    public class CQRSFeature
    {
        public string PathStartsWith { get; internal set; }

        public CQRSHandlerDescriptor[] Queries { get; internal set; }

        public CQRSHandlerDescriptor[] Commands { get; internal set; }
    }
}
