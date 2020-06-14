namespace PrimeMultiplication.Tests.Library
{
    using System;
    using System.Collections;
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
        public void Generates_primes()
        {
            var generator = new PrimeGenerator();
            var first10Primes = generator.Take(10);

            first10Primes.Should().Equal(new[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29 });
        }

        [TestMethod]
        public void Can_be_enumerated()
        {
            var generator = new PrimeGenerator();
            var enumerable = (IEnumerable)generator;

            var i = 0;
            foreach (object prime in enumerable)
            {
                if (i++ == 10)
                {
                    break;
                }
            }
        }

        [TestMethod]
        public void Can_be_enumerated_generically()
        {
            var generator = new PrimeGenerator();

            var i = 0;
            foreach (int prime in generator)
            {
                if (i++ == 10)
                {
                    break;
                }
            }
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
            var timeout = new CancellationTokenSource(1000).Token;

            Func<Task> enumerateWithTimeout = async () =>
              {
                  await foreach (var prime in generator.WithCancellation(timeout))
                  {
                  }
              };

            enumerateWithTimeout.ExecutionTime().Should().BeCloseTo(1.Seconds(), 0.5.Seconds());
        }

        [TestMethod]
        public async Task Can_throw_when_cancelled()
        {
            var generator = new PrimeGenerator(PrimeGeneratorOptions.ThrowOnCancel);
            var timeout = new CancellationTokenSource(1).Token;

            Func<Task> enumerateWithTimeout = async () =>
              {
                  await foreach (var prime in generator.WithCancellation(timeout))
                  {
                  }
              };

            await enumerateWithTimeout.Should().ThrowAsync<OperationCanceledException>();
        }
    }
}
