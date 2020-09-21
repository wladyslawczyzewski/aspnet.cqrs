using System.Threading.Tasks;
using VladyslavChyzhevskyi.ASPNET.CQRS.Queries;

namespace VladyslavChyzhevskyi.ASPNET.CQRS.Tests.TestCases
{
    public class SimpleQueryTestCase1 : IQueryHandler
    {
        public Task Handle()
        {
            return Task.CompletedTask;
        }
    }

    public class SimpleQueryTestCase2 : IQueryHandler
    {
        public Task Handle()
        {
            return Task.CompletedTask;
        }
    }

    public class SimpleQueryTestCase3 : IQueryHandler<object>
    {
        public Task<object> Handle()
        {
            return Task.FromResult((object)null);
        }
    }

    public class SimpleQueryTestCase4 : IQueryHandler<object>
    {
        public Task<object> Handle()
        {
            return Task.FromResult((object)null);
        }
    }
}
