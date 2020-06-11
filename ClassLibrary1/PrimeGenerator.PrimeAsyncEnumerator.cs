namespace ClassLibrary1
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed partial class PrimeGenerator
    {
        private sealed class PrimeAsyncEnumerator : IAsyncEnumerator<int>
        {
            private const int notInitialised = 1;
            private readonly PrimeGeneratorOptions options;
            private readonly CancellationToken cancellationToken;
            private int current = notInitialised;

            internal PrimeAsyncEnumerator(PrimeGeneratorOptions options = PrimeGeneratorOptions.None, CancellationToken cancellationToken = default)
            {
                this.options = options;
                this.cancellationToken = cancellationToken;
            }

            int IAsyncEnumerator<int>.Current
            {
                get
                {
                    if (this.current == notInitialised)
                    {
                        throw new InvalidOperationException();
                    }

                    return this.current;
                }
            }

            private bool IsPrime
            {
                get
                {
                    for (int i = 2; i * i <= this.current; i++)
                    {
                        this.ThrowIfNeeded();

                        if (this.current % i == 0)
                        {
                            return false;
                        }
                    }

                    return true;
                }
            }

            private bool ShouldStop =>
                !this.ShouldThrowOnCancel && this.cancellationToken.IsCancellationRequested;

            ValueTask IAsyncDisposable.DisposeAsync() =>
                default;

            ValueTask<bool> IAsyncEnumerator<int>.MoveNextAsync()
            {
                do
                {
                    if (this.ShouldStop)
                    {
                        return new ValueTask<bool>(false);
                    }

                    this.current++;

                } while (!this.IsPrime);

                return new ValueTask<bool>(true);
            }

            private void ThrowIfNeeded()
            {
                if (this.ShouldThrowOnCancel)
                {
                    this.cancellationToken.ThrowIfCancellationRequested();
                }
            }

            private bool ShouldThrowOnCancel =>
                this.options.HasFlag(PrimeGeneratorOptions.ThrowOnCancel);
        }
    }
}
