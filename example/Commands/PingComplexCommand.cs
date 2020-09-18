using System.Threading.Tasks;
using VladyslavChyzhevskyi.ASPNET.CQRS.Commands;

namespace VladyslavChyzhevskyi.ASPNET.CQRS.Example.Commands
{
    [CQRSRoute("/ping-complex")]
    public class PingComplexCommand : ICommand<PingComplexCommandParameters>
    {
        public Task Execute(PingComplexCommandParameters parameters)
        {
            return Task.CompletedTask;
        }
    }

    public class PingComplexCommandParameters
    {
    }
}
