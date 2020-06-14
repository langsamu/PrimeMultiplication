namespace PrimeMultiplication
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public sealed class MultiplicationTable : IAsyncEnumerable<IAsyncEnumerable<int?>>
    {
        private readonly int count;
        private readonly PrimeGeneratorOptions options;

        public MultiplicationTable(int count)
            : this(count, PrimeGeneratorOptions.None)
        {
        }

        public MultiplicationTable(int count, PrimeGeneratorOptions options)
        {
            this.count = count;
            this.options = options;
        }

        public async IAsyncEnumerator<IAsyncEnumerable<int?>> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            // header row
            yield return this.GenerateRowAsync(cancellationToken);

            var primes = new PrimeGenerator(this.options).GetAsyncEnumerator(cancellationToken);

            // rest of rows
            for (var i = 0; i < this.count && await primes.MoveNextAsync(); i++)
            {
                yield return this.GenerateRowAsync(cancellationToken, primes.Current);
            }
        }

        private async IAsyncEnumerable<int?> GenerateRowAsync([EnumeratorCancellation] CancellationToken cancellationToken, int? header = null)
        {
            // header column cell
            yield return header;

            var primes = new PrimeGenerator(this.options).GetAsyncEnumerator(cancellationToken);

            // rest of cells
            for (var i = 0; i < this.count && await primes.MoveNextAsync(); i++)
            {
                yield return (header ?? 1) * primes.Current;
            }
        }
    }
}
