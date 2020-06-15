// MIT License, Copyright 2020 Samu Lang

namespace PrimeMultiplication.Tests.Cli
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using FluentAssertions;
    using FluentAssertions.Extensions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PrimeMultiplication.Cli;

    [TestClass]
    public class ProgramTests
    {
        [TestMethod]
        public async Task Multiplies_primes()
        {
            var expected = @"
                   2         3         5
         2         4         6        10
         3         6         9        15
         5        10        15        25
";
            expected = expected[2..].Replace("\r\n", Environment.NewLine, StringComparison.InvariantCulture);

            using var writer = new StringWriter();

            Console.SetOut(writer);

            await Program.Main(new[] { "3" });

            var actual = writer.ToString();

            actual.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void Can_be_cancelled()
        {
            Func<Task> runWithTimeout = async () =>
            {
                await Program.Main(new[] { "1000", "--timeout=1000" });
            };

            runWithTimeout.ExecutionTime().Should().BeCloseTo(2.Seconds(), 1.Seconds());
        }

        [TestMethod]
        public async Task Can_throw_when_cancelled()
        {
            using var writer = new StringWriter();

            Console.SetError(writer);

            await Program.Main(new[] { "1000", "--timeout=1000", "--throw-on-cancel" });

            var actual = writer.ToString();

            actual.Should().StartWith("Unhandled exception: System.OperationCanceledException: The operation was canceled.");
        }

        [TestMethod]
        public async Task Prints_help_and_usage()
        {
            var expected = @"
testhost:
  Generates a prime multiplication table of <size> optionally within <timeout>

Usage:
  testhost [options] <count>

Arguments:
  <count>    Number of columns and rows

Options:
  -t, --timeout <timeout>    Time limit in milliseconds
  -c, --throw-on-cancel      Fail on timeout instead of just stopping
  --version                  Show version information
  -?, -h, --help             Show help and usage information

";

            expected = expected[2..].Replace("\r\n", Environment.NewLine, StringComparison.InvariantCulture);

            using var writer = new StringWriter();

            Console.SetOut(writer);

            await Program.Main(new[] { "--help" });

            var actual = writer.ToString();

            actual.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public async Task Requires_count()
        {
            using var writer = new StringWriter();

            Console.SetError(writer);

            await Program.Main(Array.Empty<string>());

            var actual = writer.ToString();

            actual.Should().StartWith("Required argument missing for command");
        }
    }
}
