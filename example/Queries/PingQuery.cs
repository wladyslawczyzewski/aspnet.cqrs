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

    [CQRSRoute("/ping-withoutput")]
    public class PingWithOutputQuery : IQuery<PingWithOutputQueryResult>
    {
        public Task<PingWithOutputQueryResult> Execute()
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
