using NUnit.Framework;
using VladyslavChyzhevskyi.ASPNET.CQRS;
using VladyslavChyzhevskyi.ASPNET.CQRS.Tests.TestCases;

namespace VladyslavChyzhevskyi.ASPNET.CQRS.Tests
{
    [TestFixture]
    public class CommandSlectorTests
    {
        [TestCase]
        public void SimpleCommandSelectorTests()
        {
            var isSimpleCommandSelector = CQRSFeatureProvider.IsSimpleCommandSelector;

            Assert.IsTrue(isSimpleCommandSelector(typeof(SimpleCommandTestCase1)));
            Assert.IsTrue(isSimpleCommandSelector(typeof(SimpleCommandTestCase2)));
            Assert.IsFalse(isSimpleCommandSelector(typeof(ComplexCommandTestCase1)));
            Assert.IsFalse(isSimpleCommandSelector(typeof(ComplexCommandTestCase2)));
        }

        [TestCase]
        public void ComplexCommandSelectorTests()
        {
            var isComplexCommandSelector = CQRSFeatureProvider.IsComplexCommandSelector;

            Assert.IsFalse(isComplexCommandSelector(typeof(SimpleCommandTestCase1)));
            Assert.IsFalse(isComplexCommandSelector(typeof(SimpleCommandTestCase2)));
            Assert.IsTrue(isComplexCommandSelector(typeof(ComplexCommandTestCase1)));
            Assert.IsTrue(isComplexCommandSelector(typeof(ComplexCommandTestCase2)));
        }
    }
}
