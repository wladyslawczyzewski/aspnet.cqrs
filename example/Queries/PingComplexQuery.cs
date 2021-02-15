using System.Threading.Tasks;
using VladyslavChyzhevskyi.ASPNET.CQRS;
using VladyslavChyzhevskyi.ASPNET.CQRS.Queries;

namespace VladyslavChyzhevskyi.ASPNET.CQRS.Example.Queries
{
    [CQRSRoute("/ping-complex")]
    public class PingComplexQueryHandler : IQueryHandler<PingComplexQuery, PingComplexQueryResult>
    {
        public Task<PingComplexQueryResult> Handle(PingComplexQuery parameters)
        {
            return Task.FromResult(new PingComplexQueryResult
            {
                Message = $"Hello, {parameters.Name}"
            });
        }
    }

    public class PingComplexQuery : IQuery
    {
        public string Name { get; set; }
    }

    public class PingComplexQueryResult
    {
        public string Message { get; set; }
    }
}
