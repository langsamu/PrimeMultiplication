namespace UnitTestProject1.WebApplication1
{
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using FluentAssertions;
    using global::WebApplication1;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class WebTests
    {
        private static WebApplicationFactory<Startup> factory;
        private static HttpClient client;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            factory = new WebApplicationFactory<Startup>();
            client = factory.CreateClient();
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            client.Dispose();
            factory.Dispose();
        }

        [TestMethod]
        [DataRow("/api")]
        [DataRow("/api?count")]
        [DataRow("/api?count=")]
        [DataRow("/api?count=NOTANUMBER")]
        [DataRow("/api?count=0")]
        [DataRow("/api?count=1&timeout=0")]
        [DataRow("/api?count=1&timeout=NOTANUMBER")]
        [DataRow("/api?count=1&timeout=1&throwOnCancel")]
        [DataRow("/api?count=1&timeout=1&throwOnCancel=")]
        [DataRow("/api?count=1&timeout=1&throwOnCancel=NOTABOOL")]
        public async Task Validates_parameters(string pathAndQuery)
        {
            var response = await client.GetAsync(pathAndQuery);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [TestMethod]
        [DataRow("xml", "text/xml")]
        [DataRow("json", "application/json")]
        [DataRow("csv", "text/csv")]
        public async Task Negotiates_extensions(string extension, string contentType)
        {
            var pathAndQuery = $"/api.{extension}?count=1";
            var response = await client.GetAsync(pathAndQuery);

            response.Content.Headers.ContentType.MediaType.Should().BeEquivalentTo(contentType);
        }

        [TestMethod]
        [DataRow("text/xml")]
        [DataRow("application/json")]
        [DataRow("text/csv")]
        public async Task Negotiates_accept_headers(string mediaType)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "/api?count=1");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

            using var response = await client.SendAsync(request);
            response.Content.Headers.ContentType.MediaType.Should().BeEquivalentTo(mediaType);
        }

        [TestMethod]
        [DataRow("xml")]
        [DataRow("json")]
        [DataRow("csv")]
        public async Task Can_fail_ubruptly(string extension)
        {
            var pathAndQuery = $"/api.{extension}?count=1000&timeout=1&throwOnCancel=true";
            using var request = new HttpRequestMessage(HttpMethod.Get, pathAndQuery);

            try
            {
                using var response = await client.SendAsync(request);
            }
            catch (HttpRequestException)
            {
            }
        }
    }
}
