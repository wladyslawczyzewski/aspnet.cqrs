using System.Threading.Tasks;

namespace VladyslavChyzhevskyi.ASPNET.CQRS.Queries
{
    public interface IQueryHandler<TQuery>
        where TQuery : IQuery
    {
        Task Handle(TQuery query);
    }

    public interface IQueryHandler<TQuery, TQueryResult>
        where TQuery : IQuery
    {
        Task<TQueryResult> Handle(TQuery query);
    }
}
