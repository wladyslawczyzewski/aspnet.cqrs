using System.Threading.Tasks;

namespace VladyslavChyzhevskyi.ASPNET.CQRS.Queries
{
    public interface IQueryHandler
    {
        Task Handle();
    }

    public interface IQueryHandler<TQueryResult>
    {
        Task<TQueryResult> Handle();
    }

    public interface IQueryHandler<TQueryParameters, TQueryResult>
    {
        Task<TQueryResult> Handle(TQueryParameters parameters);
    }
}
