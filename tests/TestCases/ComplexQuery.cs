using System.Threading.Tasks;
using ASPNET.CQRS.Queries;

namespace ASPNET.CQRS.Tests.TestCases
{
    public class ComplexQueryTestCase1 : IQueryHandler<IQuery, object>
    {
        public Task<object> Handle(IQuery query)
        {
            return Task.FromResult((object)null);
        }
    }

    public class ComplexQueryTestCase2 : IQueryHandler<IQuery, object>
    {
        public Task<object> Handle(IQuery query)
        {
            return Task.FromResult((object)null);
        }
    }
}
