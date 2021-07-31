using NUnit.Framework;
using ASPNET.CQRS;
using ASPNET.CQRS.Tests.TestCases;

namespace ASPNET.CQRS.Tests
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
        }

        [TestCase]
        public void FireAndForgetCommandSelectorTests()
        {
            var isFireAndForgetCommandSelector = CQRSFeatureProvider.IsFireAndForgetCommandSelector;

            Assert.IsTrue(isFireAndForgetCommandSelector(typeof(FireAndForgetCommandTestCase1)));
            Assert.IsTrue(isFireAndForgetCommandSelector(typeof(FireAndForgetCommandTestCase2)));
        }
    }
}
