using System.Threading.Tasks;
using VladyslavChyzhevskyi.ASPNET.CQRS.Commands;

namespace VladyslavChyzhevskyi.ASPNET.CQRS.Tests.TestCases
{

    public class ComplexCommandTestCase1 : ICommandHandler<object>
    {
        public Task Handle(object parameters)
        {
            return Task.CompletedTask;
        }
    }

    public class ComplexCommandTestCase2 : ICommandHandler<object>
    {
        public Task Handle(object parameters)
        {
            return Task.CompletedTask;
        }
    }
}
