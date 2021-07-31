using NUnit.Framework;
using ASPNET.CQRS;
using ASPNET.CQRS.Tests.TestCases;

namespace ASPNET.CQRS.Tests
{
    [TestFixture]
    public class QuerySlectorTests
    {
        [TestCase]
        public void SimpleQuerySelectorTests()
        {
            var isSimpleQuerySelector = CQRSFeatureProvider.IsSimpleQuerySelector;

            Assert.IsTrue(isSimpleQuerySelector(typeof(SimpleQueryTestCase1)));
            Assert.IsTrue(isSimpleQuerySelector(typeof(SimpleQueryTestCase2)));
            Assert.IsFalse(isSimpleQuerySelector(typeof(ComplexQueryTestCase1)));
            Assert.IsFalse(isSimpleQuerySelector(typeof(ComplexQueryTestCase2)));
        }

        [TestCase]
        public void ComplexQuerySelectorTests()
        {
            var isComplexQuerySelector = CQRSFeatureProvider.IsComplexQuerySelector;

            Assert.IsFalse(isComplexQuerySelector(typeof(SimpleQueryTestCase1)));
            Assert.IsFalse(isComplexQuerySelector(typeof(SimpleQueryTestCase2)));
            Assert.IsTrue(isComplexQuerySelector(typeof(ComplexQueryTestCase1)));
            Assert.IsTrue(isComplexQuerySelector(typeof(ComplexQueryTestCase2)));
        }
    }
}
