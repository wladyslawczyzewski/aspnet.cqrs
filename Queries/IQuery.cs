using System.Threading.Tasks;

namespace VladyslavChyzhevskyi.ASPNET.CQRS.Queries
{
    public interface IQuery
    {
        Task Execute();
    }

    public interface IQuery<TQueryParameters, TQueryResult>
    {
        Task<TQueryResult> Execute(TQueryParameters parameters);
    }
}
