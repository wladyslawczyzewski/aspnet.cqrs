using System;

namespace VladyslavChyzhevskyi.ASPNET.CQRS
{
    public class CQRSRouteDescriptor
    {
        public string Path { get; set; }

        public Type HandlerType { get; set; }

        public Type HandlerParameterType { get; set; }

        public Type HandlerOutputType { get; set; }
    }
}
