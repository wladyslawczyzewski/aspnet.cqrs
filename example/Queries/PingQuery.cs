using System.Threading.Tasks;
using VladyslavChyzhevskyi.ASPNET.CQRS;
using VladyslavChyzhevskyi.ASPNET.CQRS.Queries;

namespace VladyslavChyzhevskyi.ASPNET.CQRS.Example.Queries
{
    [CQRSRoute("/ping")]
    public class PingQuery : IQueryHandler
    {
        public Task Handle()
        {
            return Task.CompletedTask;
        }
    }

    [CQRSRoute("/ping-withoutput")]
    public class PingWithOutputQuery : IQueryHandler<PingWithOutputQueryResult>
    {
        public Task<PingWithOutputQueryResult> Handle()
        {
            return Task.FromResult(new PingWithOutputQueryResult
            {
                Text = "Hello"
            });
        }
    }

    public class PingWithOutputQueryResult
    {
        public string Text { get; set; }
    }
}
