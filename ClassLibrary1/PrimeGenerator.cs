namespace ClassLibrary1
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;

    public sealed partial class PrimeGenerator : IAsyncEnumerable<int>, IEnumerable<int>
    {
        private readonly PrimeGeneratorOptions options;

        public PrimeGenerator(PrimeGeneratorOptions options = PrimeGeneratorOptions.None) =>
            this.options = options;

        public IAsyncEnumerator<int> GetAsyncEnumerator(CancellationToken cancellationToken = default) =>
            new PrimeAsyncEnumerator(this.options, cancellationToken);

        public IEnumerator<int> GetEnumerator() =>
            new PrimeEnumerator(new PrimeAsyncEnumerator());

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
