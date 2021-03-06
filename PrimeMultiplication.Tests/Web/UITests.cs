﻿// MIT License, Copyright 2020 Samu Lang

namespace PrimeMultiplication.Tests.Web
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using AngleSharp.Html.Parser;
    using FluentAssertions;
    using FluentAssertions.Extensions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public sealed class UITests : TestBase
    {
        [TestMethod]
        public async Task Home_page_works()
        {
            var value = await this.Client.GetStringAsync("/");

            value.Should().Contain("Prime Multiplication");
        }

        [TestMethod]
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:Code should not contain multiple whitespace in a row", Justification = "Abnormal whitespace here elucidates behaviour")]
        public async Task Multiplies_primes()
        {
            var expected = new[]
            {
                new int?[] { null,  2,  3,   5 },
                new int?[] {    2,  4,  6,  10 },
                new int?[] {    3,  6,  9, 15 },
                new int?[] {    5, 10, 15, 25 },
            };

            using var response = await this.Client.GetStreamAsync("/multiply/3");
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

                return new int?(int.Parse(cell.TextContent, CultureInfo.InvariantCulture));
            }
        }

        [TestMethod]
        public void Can_be_cancelled()
        {
            Func<Task> enumerateWithTimeout = async () =>
            {
                await this.Client.GetStringAsync("/multiply/1000/stop-after/1000");
            };

            enumerateWithTimeout.ExecutionTime().Should().BeLessThan(3.Seconds());
        }

        [TestMethod]
        public async Task Can_throw_when_cancelled()
        {
            using var response = await this.Client.GetAsync("/multiply/1000/fail-after/1");

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
            var response = await this.Client.GetAsync(pathAndQuery);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
