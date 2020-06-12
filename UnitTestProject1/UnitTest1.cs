namespace UnitTestProject1
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using ClassLibrary1;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task ProofOfConcept()
        {
            const int n = 10;
            var primes = new PrimeGenerator().Take(n).ToArray();

            var largestPrime = primes.Last();
            var largestMultiple = largestPrime * largestPrime;
            var cellWidth = largestMultiple.ToString(CultureInfo.InvariantCulture).Length + 1;
            var format = $"{{0, {cellWidth}}}"; // Padding composite formatting string

            // Header row
            WriteRow();

            // Body
            foreach (var prime in primes)
            {
                WriteRow(prime);
            }

            void WriteRow(int? rowHeader = null)
            {
                // Header column
                WriteCell(rowHeader);

                foreach (var prime in primes)
                {
                    WriteCell(prime, rowHeader ?? 1);
                }

                Console.WriteLine(); // LF
            }

            void WriteCell(int? prime1 = null, int? prime2 = 1)
            {
                Console.Write(format, prime1 * prime2);
            }
        }

        [TestMethod]
        public async Task ProofOfConceptAsync()
        {
            using var cts = new CancellationTokenSource(100);
            var table = MultiplicationTable.GenerateAsync(1000, cts.Token, PrimeGeneratorOptions.ThrowOnCancel);

            await Assert.ThrowsExceptionAsync<OperationCanceledException>(async () =>
            {
                await Write(table);
            });
        }

        [TestMethod]
        public async Task ProofOfConceptAsyncDontThrow()
        {
            using var cts = new CancellationTokenSource(100);
            var table = MultiplicationTable.GenerateAsync(1000, cts.Token);

            await Write(table);
        }

        private static async Task Write(IAsyncEnumerable<IAsyncEnumerable<int?>> table)
        {
            await foreach (var row in table)
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
