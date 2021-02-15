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
        }
    }
}
