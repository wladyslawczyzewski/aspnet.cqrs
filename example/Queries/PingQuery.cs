using System.Threading.Tasks;
using ASPNET.CQRS;
using ASPNET.CQRS.Queries;

namespace ASPNET.CQRS.Example.Queries
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
