using System.Threading.Tasks;
using VladyslavChyzhevskyi.ASPNET.CQRS.Commands;

namespace VladyslavChyzhevskyi.ASPNET.CQRS.Tests.TestCases
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
}
