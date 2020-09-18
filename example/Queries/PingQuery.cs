using System.Threading.Tasks;
using VladyslavChyzhevskyi.ASPNET.CQRS;
using VladyslavChyzhevskyi.ASPNET.CQRS.Queries;

namespace VladyslavChyzhevskyi.ASPNET.CQRS.Example.Queries
{
    [CQRSRoute("/ping")]
    public class PingQuery : IQuery
    {
        public Task Execute()
        {
            return Task.CompletedTask;
        }
    }
}
