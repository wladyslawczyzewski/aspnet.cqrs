namespace VladyslavChyzhevskyi.ASPNET.CQRS.Commands
{
    public interface ICommand<TCommandParameters>
    {
        void Execute(TCommandParameters parameters);
    }
}
