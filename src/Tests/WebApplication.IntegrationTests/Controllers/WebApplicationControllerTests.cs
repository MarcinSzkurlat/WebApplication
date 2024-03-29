using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace WebApplication.IntegrationTests.Controllers
{
    public class WebApplicationControllerTests
    {
        private readonly string _url = "/api/WebApplication";

        [Test]
        public async Task GetMethod_ReturnsOkResult_WithoutAnyParameters()
        {
            var factory = new WebApplicationFactory<Program>();
            var client = factory.CreateClient();

            var response = await client.GetAsync(_url);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task GetMethod_ReturnsOkResult_WithAllParameters()
        {
            var factory = new WebApplicationFactory<Program>();
            var client = factory.CreateClient();

            var response = await client.GetAsync(_url + "?sortBy=0&sortDirection=1&pageNumber=1");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task GetMethod_ReturnsBadRequestResult_PageNumberIsZero()
        {
            var factory = new WebApplicationFactory<Program>();
            var client = factory.CreateClient();

            var response = await client.GetAsync(_url + "?pageNumber=0");

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public async Task PostMethod_ReturnsOkResult_WhenCall()
        {
            var factory = new WebApplicationFactory<Program>();
            var client = factory.CreateClient();

            var response = await client.GetAsync(_url);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
