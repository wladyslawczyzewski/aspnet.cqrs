using System.Threading.Tasks;

namespace VladyslavChyzhevskyi.ASPNET.CQRS.Commands
{
    public interface ICommand
    {
        Task Execute();
    }

    public interface ICommand<TCommandParameters>
    {
        Task Execute(TCommandParameters parameters);
    }
}
