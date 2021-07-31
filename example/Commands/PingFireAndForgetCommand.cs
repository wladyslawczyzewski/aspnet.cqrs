using System;
using System.Threading.Tasks;
using ASPNET.CQRS.Commands;
using Microsoft.Extensions.Logging;

namespace ASPNET.CQRS.Example.Commands
{
    [CQRSRoute("/ping-fire-and-forget")]
    public class PingFireAndForgetCommandHandler : IFireAndForgetCommandHandler<PingFireAndForgetCommand>
    {
        private readonly ILogger<PingFireAndForgetCommandHandler> _logger;

        public PingFireAndForgetCommandHandler(ILogger<PingFireAndForgetCommandHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(PingFireAndForgetCommand command)
        {
            // simulating long running command handling
            await Task.Delay(TimeSpan.FromSeconds(10));
            _logger.LogTrace($"Execute command {nameof(PingFireAndForgetCommandHandler)} in fire and forget manner.");
        }
    }

    public class PingFireAndForgetCommand : ICommand
    {
    }
}
