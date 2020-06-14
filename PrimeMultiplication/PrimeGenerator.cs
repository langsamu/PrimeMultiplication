// MIT License, Copyright 2020 Samu Lang

namespace PrimeMultiplication
{
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>
    /// Represents an asynchronous stream of prime numbers.
    /// </summary>
    public sealed partial class PrimeGenerator : IAsyncEnumerable<int>
    {
        private readonly PrimeGeneratorOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimeGenerator"/> class.
        /// </summary>
        /// <param name="options">Options for generating primes.</param>
        public PrimeGenerator(PrimeGeneratorOptions options = PrimeGeneratorOptions.None) =>
            this.options = options;

        /// <inheritdoc/>
        public IAsyncEnumerator<int> GetAsyncEnumerator(CancellationToken cancellationToken = default) =>
            new AsyncEnumerator(this.options, cancellationToken);
    }
}
