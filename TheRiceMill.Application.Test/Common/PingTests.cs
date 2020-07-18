using NUnit.Framework;
using TestContext = TheRiceMill.Application.Tests.Fixtures.TestContext;

namespace TheRiceMill.Application.Tests.Common
{
    [TestFixture]
    public class PingTests
    {
        private TestContext _context;
        [SetUp]
        public void SetUp()
        {
            _context = new TestContext();
        }

        [Test]
        public void Ping_WhenHit_ShouldReturn200()
        {
           var result = _context.Client.GetAsync("api/v1/ping").Result;

            Assert.AreEqual(200, result.StatusCode);
        }
    }
}