using System;

namespace VladyslavChyzhevskyi.ASPNET.CQRS
{
    public class CQRSRouteDescriptor
    {
        public string Path { get; set; }

        public bool IsQuery { get; set; }

        public bool IsSimple { get; set; }

        public Type UnderlyingType { get; set; }

        public Type ParameterType { get; set; }

        public Type ResultType { get; set; }
    }
}