using System.Threading.Tasks;

namespace VladyslavChyzhevskyi.ASPNET.CQRS.Commands
{
    public interface ICommandHandler
    {
        Task Handle();
    }

    public interface ICommandHandler<TCommandParameters>
    {
        Task Handle(TCommandParameters parameters);
    }
}
