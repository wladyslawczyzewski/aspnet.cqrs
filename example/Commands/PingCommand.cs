using System.Threading.Tasks;
using VladyslavChyzhevskyi.ASPNET.CQRS.Commands;

namespace VladyslavChyzhevskyi.ASPNET.CQRS.Example.Commands
{
    [CQRSRoute("/ping")]
    public class PingCommand : ICommand
    {
        public Task Execute()
        {
            return Task.CompletedTask;
        }
    }
}
