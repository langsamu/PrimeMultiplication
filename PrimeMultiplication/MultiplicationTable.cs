// MIT License, Copyright 2020 Samu Lang

namespace PrimeMultiplication
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    /// <summary>
    /// Represents an asynchronous multiplication table of primes.
    /// </summary>
    public sealed class MultiplicationTable : IAsyncEnumerable<IAsyncEnumerable<int?>>
    {
        private readonly int count;
        private readonly PrimeGeneratorOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiplicationTable"/> class.
        /// </summary>
        /// <param name="count">The number of primes used as the size of the table.</param>
        public MultiplicationTable(int count)
            : this(count, PrimeGeneratorOptions.None)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiplicationTable"/> class.
        /// </summary>
        /// <param name="count">The number of primes used as the size of the table.</param>
        /// <param name="options">Options for the prime generator used by the table.</param>
        public MultiplicationTable(int count, PrimeGeneratorOptions options)
        {
            this.count = count;
            this.options = options;
        }

        /// <inheritdoc/>
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
