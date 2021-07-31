using System.Threading.Tasks;

namespace ASPNET.CQRS.Commands
{
    public interface ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        Task Handle(TCommand command);
    }

    public interface IFireAndForgetCommandHandler<TCommand>
        where TCommand : ICommand
    {
        Task Handle(TCommand command);
    }
}
