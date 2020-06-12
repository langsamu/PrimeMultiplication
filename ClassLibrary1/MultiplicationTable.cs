namespace ClassLibrary1
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public static class MultiplicationTable
    {
        public static async IAsyncEnumerable<IAsyncEnumerable<int?>> GenerateAsync(int count, [EnumeratorCancellation]CancellationToken cancellationToken = default, PrimeGeneratorOptions options = PrimeGeneratorOptions.None)
        {
            // header row
            yield return GenerateRowAsync(count, options, cancellationToken);

            var primes = new PrimeGenerator(options).GetAsyncEnumerator(cancellationToken);

            // rest of rows
            for (var i = 0; i < count && await primes.MoveNextAsync(); i++)
            {
                yield return GenerateRowAsync(count, options, cancellationToken, primes.Current);
            }
        }

        private static async IAsyncEnumerable<int?> GenerateRowAsync(int count, PrimeGeneratorOptions options, [EnumeratorCancellation]CancellationToken cancellationToken = default, int? header = null)
        {
            // header column cell
            yield return header;

            var primes = new PrimeGenerator(options).GetAsyncEnumerator(cancellationToken);

            // rest of cells
            for (var i = 0; i < count && await primes.MoveNextAsync(); i++)
            {
                yield return (header ?? 1) * primes.Current;
            }
        }
    }
}
