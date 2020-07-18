using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using TheRiceMill.Presentation;

namespace TheRiceMill.Application.Tests.Fixtures
{
    public class TestContext
    {
        public HttpClient Client;
        private TestServer _server;

        public TestContext()
        {
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            Client = _server.CreateClient();
        }
    }
}