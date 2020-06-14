namespace PrimeMultiplication.Tests.Library
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using PrimeMultiplication;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AsyncEnumeratorTests
    {
        [TestMethod]
        public async Task Current_throws_before_first()
        {
            var generator = new PrimeGenerator();
            await using var enumerator = generator.GetAsyncEnumerator();

            enumerator.Invoking(e => e.Current).Should().Throw<InvalidOperationException>("you cannot get current before move next");
        }

        [TestMethod]
        public async Task Current_throws_after_last()
        {
            var generator = new PrimeGenerator();
            var timeout = new CancellationTokenSource(1).Token;
            await using var enumerator = generator.WithCancellation(timeout).GetAsyncEnumerator();

            // Exhaust it
            while (await enumerator.MoveNextAsync())
            {
            }

            enumerator.Invoking(e => e.Current).Should().Throw<InvalidOperationException>("you cannot get current after move next is false");
        }
    }
}
