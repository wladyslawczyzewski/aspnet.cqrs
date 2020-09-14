using NUnit.Framework;

namespace VladyslavChyzhevskyi.ASPNET.CQRS.Tests
{
    [TestFixture]
    public class InitTest
    {
        [TestCase]
        public void Success()
        {
            Assert.True(true);
        }

        [TestCase]
        public void Fail()
        {
            Assert.True(false);
        }
    }
}
