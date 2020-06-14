// MIT License, Copyright 2020 Samu Lang

namespace PrimeMultiplication.Tests.Library
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PrimeMultiplication;

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
            using var timeout = new CancellationTokenSource(1);
            await using var enumerator = generator.WithCancellation(timeout.Token).GetAsyncEnumerator();

            // Exhaust it
            while (await enumerator.MoveNextAsync())
            {
            }

            enumerator.Invoking(e => e.Current).Should().Throw<InvalidOperationException>("you cannot get current after move next is false");
        }
    }
}
