// MIT License, Copyright 2020 Samu Lang

namespace PrimeMultiplication.Tests.Library
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using FluentAssertions.Extensions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PrimeMultiplication;

    [TestClass]
    public class PrimeGeneratorTests
    {
        [TestMethod]
        public async Task Generates_primes()
        {
            var count = 10;
            var expected = new[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29 };
            var actual = new int[count];

            await using var primes = new PrimeGenerator().GetAsyncEnumerator();

            for (var i = 0; i < count && await primes.MoveNextAsync(); i++)
            {
                actual[i] = primes.Current;
            }

            actual.Should().Equal(expected);
        }

        [TestMethod]
        public async Task Can_be_enumerated_asynchronously()
        {
            var generator = new PrimeGenerator();

            var i = 0;
            await foreach (var prime in generator)
            {
                if (i++ == 10)
                {
                    break;
                }
            }
        }

        [TestMethod]
        public void Can_be_cancelled()
        {
            var generator = new PrimeGenerator();
            using var timeout = new CancellationTokenSource(1000);

            Func<Task> enumerateWithTimeout = async () =>
              {
                  await foreach (var prime in generator.WithCancellation(timeout.Token))
                  {
                  }
              };

            enumerateWithTimeout.ExecutionTime().Should().BeCloseTo(1.Seconds(), 0.5.Seconds());
        }

        [TestMethod]
        public async Task Can_throw_when_cancelled()
        {
            var generator = new PrimeGenerator(PrimeGeneratorOptions.ThrowOnCancel);
            using var timeout = new CancellationTokenSource(1);

            Func<Task> enumerateWithTimeout = async () =>
              {
                  await foreach (var prime in generator.WithCancellation(timeout.Token))
                  {
                  }
              };

            await enumerateWithTimeout.Should().ThrowAsync<OperationCanceledException>();
        }
    }
}
