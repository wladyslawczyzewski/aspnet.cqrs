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
}
