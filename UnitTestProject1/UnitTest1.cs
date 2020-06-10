namespace UnitTestProject1
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ProofOfConcept()
        {
            const int n = 10;
            var primes = n.Primes().ToArray();

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

            const int n = 2000;

            await Assert.ThrowsExceptionAsync<TaskCanceledException>(async () =>
            {
                // Header row
                await WriteRow();

                // Body
                await foreach (var prime in n.PrimesAsync(cts.Token))
                {
                    await WriteRow(prime);
                }
            });

            async Task WriteRow(int? rowHeader = null)
            {
                // Header column
                WriteCell(rowHeader);

                await foreach (var prime in n.PrimesAsync(cts.Token))
                {
                    WriteCell(prime, rowHeader ?? 1);
                }

                Console.WriteLine(); // LF
            }

            void WriteCell(int? prime1 = null, int? prime2 = 1)
            {
                Console.Write("{0, 10}", prime1 * prime2);
            }
        }
    }
}
