using NUnit.Framework;
using VladyslavChyzhevskyi.ASPNET.CQRS;
using VladyslavChyzhevskyi.ASPNET.CQRS.Tests.TestCases;

namespace VladyslavChyzhevskyi.ASPNET.CQRS.Tests
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
            Assert.IsTrue(isSimpleQuerySelector(typeof(SimpleQueryTestCase3)));
            Assert.IsTrue(isSimpleQuerySelector(typeof(SimpleQueryTestCase4)));
            Assert.IsFalse(isSimpleQuerySelector(typeof(ComplexQueryTestCase1)));
            Assert.IsFalse(isSimpleQuerySelector(typeof(ComplexQueryTestCase2)));
        }

        [TestCase]
        public void ComplexQuerySelectorTests()
        {
            var isComplexQuerySelector = CQRSFeatureProvider.IsComplexQuerySelector;

            Assert.IsFalse(isComplexQuerySelector(typeof(SimpleQueryTestCase1)));
            Assert.IsFalse(isComplexQuerySelector(typeof(SimpleQueryTestCase2)));
            Assert.IsFalse(isComplexQuerySelector(typeof(SimpleQueryTestCase3)));
            Assert.IsFalse(isComplexQuerySelector(typeof(SimpleQueryTestCase4)));
            Assert.IsTrue(isComplexQuerySelector(typeof(ComplexQueryTestCase1)));
            Assert.IsTrue(isComplexQuerySelector(typeof(ComplexQueryTestCase2)));
        }
    }
}
