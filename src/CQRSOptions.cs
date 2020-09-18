using System.Reflection;

namespace VladyslavChyzhevskyi.ASPNET.CQRS
{
    public class CQRSOptions
    {
        public string BasePath { get; set; }
        public Assembly[] Assemblies { get; set; }
    }
}
