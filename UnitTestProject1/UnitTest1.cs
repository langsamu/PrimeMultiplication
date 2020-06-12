namespace UnitTestProject1
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using ClassLibrary1;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task ProofOfConceptAsync()
        {
            using var cts = new CancellationTokenSource(100);
            var table = new MultiplicationTable(1000, PrimeGeneratorOptions.ThrowOnCancel);

            await Assert.ThrowsExceptionAsync<OperationCanceledException>(async () =>
            {
                await Write(table, cts.Token);
            });
        }

        [TestMethod]
        public async Task ProofOfConceptAsyncDontThrow()
        {
            using var cts = new CancellationTokenSource(100);
            var table = new MultiplicationTable(1000);

            await Write(table, cts.Token);
        }

        private static async Task Write(MultiplicationTable table, CancellationToken cancellationToken)
        {
            await foreach (var row in table.WithCancellation(cancellationToken))
            {
                await foreach (var cell in row)
                {
                    Console.Write("{0, 10}", cell);
                }

                Console.WriteLine(); // LF
            }
        }
    }
}
