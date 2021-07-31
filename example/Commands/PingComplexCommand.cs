using System.Threading.Tasks;
using ASPNET.CQRS.Commands;
using Microsoft.Extensions.Logging;

namespace ASPNET.CQRS.Example.Commands
{
    [CQRSRoute("/ping-complex")]
    public class PingComplexCommandHandler : ICommandHandler<PingComplexCommand>
    {
        private readonly ILogger<PingComplexCommandHandler> _logger;

        public PingComplexCommandHandler(ILogger<PingComplexCommandHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(PingComplexCommand command)
        {
            _logger.LogTrace($"Hello, {command.Name}!");
            return Task.CompletedTask;
        }
    }

    public class PingComplexCommand : ICommand
    {
        public string Name { get; set; }
    }
}
