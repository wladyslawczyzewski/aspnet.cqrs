namespace VladyslavChyzhevskyi.ASPNET.CQRS.Queries
{
    public interface IQuery
    {
        void Execute();
    }

    public interface IQuery<TQueryParameters, TQueryResult>
    {
        TQueryResult Execute(TQueryParameters parameters);
    }
}
