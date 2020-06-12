namespace ClassLibrary1
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public sealed partial class PrimeGenerator
    {
        private sealed class Enumerator : IEnumerator<int>
        {
            private readonly IAsyncEnumerator<int> primeAsyncEnumerator;

            internal Enumerator(AsyncEnumerator primeAsyncEnumerator) =>
                this.primeAsyncEnumerator = primeAsyncEnumerator;

            public int Current =>
                this.primeAsyncEnumerator.Current;

            object IEnumerator.Current =>
                this.Current;

            void IDisposable.Dispose() =>
                this.primeAsyncEnumerator.DisposeAsync().GetAwaiter().GetResult();

            bool IEnumerator.MoveNext() =>
                this.primeAsyncEnumerator.MoveNextAsync().GetAwaiter().GetResult();

            void IEnumerator.Reset() =>
                throw new InvalidOperationException();
        }
    }
}
