using System.Reflection;

namespace ASPNET.CQRS
{
    public class CQRSOptions
    {
        public string BasePath { get; set; }
        public Assembly[] Assemblies { get; set; }
    }
}
