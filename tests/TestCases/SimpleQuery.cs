using System.Threading.Tasks;
using VladyslavChyzhevskyi.ASPNET.CQRS.Queries;

namespace VladyslavChyzhevskyi.ASPNET.CQRS.Tests.TestCases
{
    public class SimpleQueryTestCase1 : IQuery
    {
        public Task Execute()
        {
            return Task.CompletedTask;
        }
    }

    public class SimpleQueryTestCase2 : IQuery
    {
        public Task Execute()
        {
            return Task.CompletedTask;
        }
    }

    public class SimpleQueryTestCase3 : IQuery<object>
    {
        public Task<object> Execute()
        {
            return Task.FromResult((object)null);
        }
    }

    public class SimpleQueryTestCase4 : IQuery<object>
    {
        public Task<object> Execute()
        {
            return Task.FromResult((object)null);
        }
    }
}
