using System.Threading.Tasks;
using VladyslavChyzhevskyi.ASPNET.CQRS.Commands;

namespace VladyslavChyzhevskyi.ASPNET.CQRS.Tests.TestCases
{
    public class SimpleCommandTestCase1 : ICommandHandler
    {
        public Task Handle()
        {
            return Task.CompletedTask;
        }
    }

    public class SimpleCommandTestCase2 : ICommandHandler
    {
        public Task Handle()
        {
            return Task.CompletedTask;
        }
    }
}
