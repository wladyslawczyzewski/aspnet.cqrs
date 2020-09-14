using System;

namespace VladyslavChyzhevskyi.ASPNET.CQRS
{
    public class CQRSFeature
    {
        public string PathStartsWith { get; internal set; }

        public CQRSRouteDescriptor[] Queries { get; internal set; }

        public Type[] Commands { get; internal set; }
    }
}
