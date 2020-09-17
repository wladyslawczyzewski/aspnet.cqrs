using System;

namespace VladyslavChyzhevskyi.ASPNET.CQRS
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class CQRSRouteAttribute : System.Attribute
    {
        public string Path { get; private set; }

        public CQRSRouteAttribute(string path)
        {
            Path = path;
        }
    }
}
