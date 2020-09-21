using System.Threading.Tasks;
using VladyslavChyzhevskyi.ASPNET.CQRS.Commands;

namespace VladyslavChyzhevskyi.ASPNET.CQRS.Example.Commands
{
    [CQRSRoute("/ping")]
    public class PingCommand : ICommandHandler
    {
        public Task Handle()
        {
            return Task.CompletedTask;
        }
    }
}
