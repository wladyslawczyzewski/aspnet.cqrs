using System;
using System.Threading.Tasks;
using ASPNET.CQRS.Commands;

namespace ASPNET.CQRS.Tests.TestCases
{
    public class SimpleCommandTestCase1 : ICommandHandler<ICommand>
    {
        public Task Handle(ICommand command)
        {
            return Task.CompletedTask;
        }
    }

    public class SimpleCommandTestCase2 : ICommandHandler<ICommand>
    {
        public Task Handle(ICommand command)
        {
            return Task.CompletedTask;
        }
    }

    public class FireAndForgetCommandTestCase1 : IFireAndForgetCommandHandler<ICommand>
    {
        public async Task Handle(ICommand command)
        {
            await Task.Delay(TimeSpan.FromSeconds(10));
        }
    }

    public class FireAndForgetCommandTestCase2 : IFireAndForgetCommandHandler<ICommand>
    {
        public async Task Handle(ICommand command)
        {
            await Task.Delay(TimeSpan.FromSeconds(30));
        }
    }
}
