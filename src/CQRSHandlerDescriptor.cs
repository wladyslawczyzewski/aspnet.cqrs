using System;

namespace ASPNET.CQRS
{
    public class CQRSHandlerDescriptor
    {
        public string Path { get; set; }

        public Type HandlerType { get; set; }

        public Type HandlerParameterType { get; set; }

        public Type HandlerOutputType { get; set; }
    }
}
