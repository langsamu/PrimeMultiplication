namespace UnitTestProject1.WebApplication1
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using AngleSharp.Html.Parser;
    using FluentAssertions;
    using FluentAssertions.Extensions;
    using global::WebApplication1;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class UITests
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

            using var response = await client.GetStreamAsync("/multiply/3");
            using var document = await new HtmlParser().ParseDocumentAsync(response, CancellationToken.None);

            var actual =
                document
                    .QuerySelectorAll("tr")
                    .Select(row =>
                        row.QuerySelectorAll("td")
                        .Select(ParseCell)
                        .ToArray())
                    .ToArray();

            actual.Should().BeEquivalentTo(expected);

            static int? ParseCell(AngleSharp.Dom.IElement cell)
            {
                if (string.IsNullOrEmpty(cell.TextContent))
                {
                    return null;
                }

                return new int?(int.Parse(cell.TextContent));
            }
        }

        [TestMethod]
        public void Can_be_cancelled()
        {
            Func<Task> enumerateWithTimeout = async () =>
            {
                await client.GetStringAsync("/multiply/1000/stop-after/1000");
            };

            enumerateWithTimeout.ExecutionTime().Should().BeCloseTo(1.Seconds(), 0.5.Seconds());
        }

        [TestMethod]
        public async Task Can_throw_when_cancelled()
        {
            using var response = await client.GetAsync("/multiply/1000/fail-after/1000");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [TestMethod]
        [DataRow("/multiply/0")]
        [DataRow("/multiply/NOTANUMBER")]
        [DataRow("/multiply/1/stop-after/0")]
        [DataRow("/multiply/1/stop-after/NOTANUMBER")]
        [DataRow("/multiply/1/fail-after/0")]
        [DataRow("/multiply/1/fail-after/NOTANUMBER")]
        public async Task Validates_parameters(string pathAndQuery)
        {
            var response = await client.GetAsync(pathAndQuery);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
