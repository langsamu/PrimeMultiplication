﻿namespace UnitTestProject1.ConsoleApp1
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using FluentAssertions;
    using FluentAssertions.Extensions;
    using global::ConsoleApp1;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ProgramTests
    {
        [TestMethod]
        public async Task Prints_help_and_usage()
        {
            var expected = @"
testhost:
  Generates a prime multiplication table of <size> optionally within <timeout>

Usage:
  testhost [options] <size>

Arguments:
  <size>    Number of columns and rows

Options:
  -t, --timeout <timeout>    Time limit in milliseconds
  -c, --throw-on-cancel      Fail on timeout instead of just stopping
  --version                  Show version information
  -?, -h, --help             Show help and usage information

"[2..].Replace("\r\n", Environment.NewLine);

            using var writer = new StringWriter();

            Console.SetOut(writer);

            await Program.Main(new[] { "--help" });

            var actual = writer.ToString();

            actual.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public async Task Requires_size()
        {
            using var writer = new StringWriter();

            Console.SetError(writer);

            await Program.Main(Array.Empty<string>());

            var actual = writer.ToString();

            actual.Should().StartWith("Required argument missing for command");
        }

        [TestMethod]
        public async Task Multiplies_primes()
        {
            var expected = @"
                   2         3         5         7
         2         4         6        10        14
         3         6         9        15        21
         5        10        15        25        35
         7        14        21        35        49
"[2..].Replace("\r\n", Environment.NewLine);

            using var writer = new StringWriter();

            Console.SetOut(writer);

            await Program.Main(new[] { "4" });

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

            runWithTimeout.ExecutionTime().Should().BeCloseTo(1.Seconds(), 0.5.Seconds());
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
    }
}