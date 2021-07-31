using System.Threading.Tasks;
using ASPNET.CQRS.Commands;

namespace ASPNET.CQRS.Example.Commands
{
    [CQRSRoute("/ping")]
    public class PingCommandHandler : ICommandHandler<PingCommand>
    {
        public Task Handle(PingCommand command)
        {
            return Task.CompletedTask;
        }
    }

    public class PingCommand : ICommand
    {
    }
}
