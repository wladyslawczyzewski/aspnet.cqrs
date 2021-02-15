using System.Threading.Tasks;
using VladyslavChyzhevskyi.ASPNET.CQRS;
using VladyslavChyzhevskyi.ASPNET.CQRS.Queries;

namespace VladyslavChyzhevskyi.ASPNET.CQRS.Example.Queries
{
    [CQRSRoute("/ping")]
    public class PingQueryHandler : IQueryHandler<PingQuery>
    {
        public Task Handle(PingQuery query)
        {
            return Task.CompletedTask;
        }
    }

    public class PingQuery : IQuery
    {
    }
}
