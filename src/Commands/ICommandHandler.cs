using System.Threading.Tasks;

namespace VladyslavChyzhevskyi.ASPNET.CQRS.Commands
{
    public interface ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        Task Handle(TCommand command);
    }
}
