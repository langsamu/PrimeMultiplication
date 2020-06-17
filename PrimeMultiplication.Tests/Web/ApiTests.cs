// MIT License, Copyright 2020 Samu Lang

namespace PrimeMultiplication.Tests.Web
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text.Json;
    using System.Threading.Tasks;
    using FluentAssertions;
    using FluentAssertions.Extensions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Net.Http.Headers;
    using Microsoft.OpenApi.Readers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PrimeMultiplication.Web;

    [TestClass]
    public class ApiTests
    {
        private static WebApplicationFactory<Startup> factory;
        private static HttpClient client;

        [ClassInitialize]
        [SuppressMessage("Usage", "CA1801:Review unused parameters", Justification = "Required by test framework")]
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
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:Code should not contain multiple whitespace in a row", Justification = "Abnormal whitespace here elucidates behaviour")]
        public async Task Multiplies_primes()
        {
            var expected = new[]
            {
                new int?[] { null,  2,  3,  5 },
                new int?[] {    2,  4,  6, 10 },
                new int?[] {    3,  6,  9, 15 },
                new int?[] {    5, 10, 15, 25 },
            };

            using var response = await client.GetStreamAsync("/api/multiply?count=3");
            var actual = await JsonSerializer.DeserializeAsync<int?[][]>(response);

            actual.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void Can_be_cancelled()
        {
            Func<Task> enumerateWithTimeout = async () =>
            {
                await client.GetStringAsync("/api/multiply?count=1000&timeout=1000");
            };

            enumerateWithTimeout.ExecutionTime().Should().BeLessThan(3.Seconds());
        }

        [TestMethod]
        [DataRow("/api/multiply")]
        [DataRow("/api/multiply?count")]
        [DataRow("/api/multiply?count=")]
        [DataRow("/api/multiply?count=NOTANUMBER")]
        [DataRow("/api/multiply?count=0")]
        [DataRow("/api/multiply?count=1&timeout=0")]
        [DataRow("/api/multiply?count=1&timeout=NOTANUMBER")]
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
            var pathAndQuery = $"/api/multiply.{extension}?count=1";
            var response = await client.GetAsync(pathAndQuery);

            response.Content.Headers.ContentType.MediaType.Should().BeEquivalentTo(contentType);
        }

        [TestMethod]
        [DataRow("text/xml")]
        [DataRow("application/json")]
        [DataRow("text/csv")]
        public async Task Negotiates_accept_headers(string mediaType)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "/api/multiply?count=1");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

            using var response = await client.SendAsync(request);
            response.Content.Headers.ContentType.MediaType.Should().BeEquivalentTo(mediaType);
        }

        [TestMethod]
        public async Task OpenApi_document_is_valid()
        {
            using var response = await client.GetAsync($"/openapi.json");
            var reader = new OpenApiStreamReader();

            using var stream = await response.Content.ReadAsStreamAsync();
            reader.Read(stream, out var diagnostic);

            diagnostic.Errors.Should().BeEmpty();
        }

        [TestMethod]
        public async Task SwaggerUI_works()
        {
            var value = await client.GetStringAsync("/openapi");

            value.Should().Contain("SwaggerUIBundle");
        }

        [TestMethod]
        public async Task Sends_CORS_headers()
        {
            using var request = new HttpRequestMessage(HttpMethod.Options, "/");
            request.Headers.Add(HeaderNames.Origin, "example.com");
            request.Headers.Add(HeaderNames.AccessControlRequestHeaders, HeaderNames.ContentType);
            request.Headers.Add(HeaderNames.AccessControlRequestMethod, HttpMethods.Post);

            using var response = await client.SendAsync(request);
            var exists = response.Headers.TryGetValues(HeaderNames.AccessControlAllowOrigin, out var values);
            exists.Should().BeTrue();
            values.Should().Contain("*");

            exists = response.Headers.TryGetValues(HeaderNames.AccessControlAllowHeaders, out values);
            exists.Should().BeTrue();
            values.Should().Contain(HeaderNames.ContentType);

            exists = response.Headers.TryGetValues(HeaderNames.AccessControlAllowMethods, out values);
            exists.Should().BeTrue();
            values.Should().Contain(HttpMethods.Post);
        }
    }
}
