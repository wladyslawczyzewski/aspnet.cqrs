using System.Threading.Tasks;
using VladyslavChyzhevskyi.ASPNET.CQRS;
using VladyslavChyzhevskyi.ASPNET.CQRS.Queries;

namespace VladyslavChyzhevskyi.ASPNET.CQRS.Example.Queries
{
    [CQRSRoute("/ping-complex")]
    public class PingComplexQuery : IQueryHandler<PingComplexQueryParameters, PingComplexQueryResult>
    {
        public Task<PingComplexQueryResult> Handle(PingComplexQueryParameters parameters)
        {
            return Task.FromResult(new PingComplexQueryResult
            {
                Message = $"Hello, {parameters.Name}"
            });
        }
    }

    public class PingComplexQueryParameters
    {
        public string Name { get; set; }
    }

    public class PingComplexQueryResult
    {
        public string Message { get; set; }
    }
}
