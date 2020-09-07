namespace VladyslavChyzhevskyi.ASPNET.CQRS.Queries
{
    public interface IQuery<TQueryParameters, TQueryResult>
    {
        TQueryResult Execute(TQueryParameters parameters);
    }
}
