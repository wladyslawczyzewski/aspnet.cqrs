using System.Threading.Tasks;
using VladyslavChyzhevskyi.ASPNET.CQRS.Queries;

namespace VladyslavChyzhevskyi.ASPNET.CQRS.Tests.TestCases
{
    public class ComplexQueryTestCase1 : IQuery<object, object>
    {
        public Task<object> Execute(object parameters)
        {
            return Task.FromResult(new object());
        }
    }

    public class ComplexQueryTestCase2 : IQuery<object, object>
    {
        public Task<object> Execute(object parameters)
        {
            return Task.FromResult(new object());
        }
    }
}
