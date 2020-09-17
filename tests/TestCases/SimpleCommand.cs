using System.Threading.Tasks;
using VladyslavChyzhevskyi.ASPNET.CQRS.Commands;

namespace VladyslavChyzhevskyi.ASPNET.CQRS.Tests.TestCases
{
    public class SimpleCommandTestCase1 : ICommand
    {
        public Task Execute()
        {
            return Task.CompletedTask;
        }
    }

    public class SimpleCommandTestCase2 : ICommand
    {
        public Task Execute()
        {
            return Task.CompletedTask;
        }
    }
}
