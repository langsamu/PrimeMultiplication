namespace PrimeMultiplication.Tests.Web
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text.Json;
    using System.Threading.Tasks;
    using FluentAssertions;
    using FluentAssertions.Extensions;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PrimeMultiplication.Web;

    [TestClass]
    public class ApiTests
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
        public async Task Multiplies_primes()
        {
            var expected = new[] {
                new int?[] { null,  2,  3,   5 },
                new int?[] {    2,  4,  6,  10 },
                new int?[] {    3,  6,  9 , 15 },
                new int?[] {    5, 10, 15 , 25 },
            };

            using var response = await client.GetStreamAsync("/api?count=3");
            var actual = await JsonSerializer.DeserializeAsync<int?[][]>(response);

            actual.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void Can_be_cancelled()
        {
            Func<Task> enumerateWithTimeout = async () =>
            {
                await client.GetStringAsync("/api?count=1000&timeout=1000");
            };

            enumerateWithTimeout.ExecutionTime().Should().BeCloseTo(1.Seconds(), 0.5.Seconds());
        }

        [TestMethod]
        [DataRow("/api")]
        [DataRow("/api?count")]
        [DataRow("/api?count=")]
        [DataRow("/api?count=NOTANUMBER")]
        [DataRow("/api?count=0")]
        [DataRow("/api?count=1&timeout=0")]
        [DataRow("/api?count=1&timeout=NOTANUMBER")]
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
    }
}
