using System.Threading.Tasks;
using VladyslavChyzhevskyi.ASPNET.CQRS.Commands;

namespace VladyslavChyzhevskyi.ASPNET.CQRS.Example.Commands
{
    [CQRSRoute("/ping-complex")]
    public class PingComplexCommand : ICommandHandler<PingComplexCommandParameters>
    {
        public Task Handle(PingComplexCommandParameters parameters)
        {
            return Task.CompletedTask;
        }
    }

    public class PingComplexCommandParameters
    {
    }
}
