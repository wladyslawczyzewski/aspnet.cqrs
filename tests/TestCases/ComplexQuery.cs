using System.Threading.Tasks;
using VladyslavChyzhevskyi.ASPNET.CQRS.Queries;

namespace VladyslavChyzhevskyi.ASPNET.CQRS.Tests.TestCases
{
    public class ComplexQueryTestCase1 : IQueryHandler<object, object>
    {
        public Task<object> Handle(object parameters)
        {
            return Task.FromResult(new object());
        }
    }

    public class ComplexQueryTestCase2 : IQueryHandler<object, object>
    {
        public Task<object> Handle(object parameters)
        {
            return Task.FromResult(new object());
        }
    }
}
