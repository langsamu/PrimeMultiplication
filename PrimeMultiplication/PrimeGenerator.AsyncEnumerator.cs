// MIT License, Copyright 2020 Samu Lang

namespace PrimeMultiplication
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents an asynchronous stream of prime numbers.
    /// </summary>
    public sealed partial class PrimeGenerator
    {
        private sealed class AsyncEnumerator : IAsyncEnumerator<int>
        {
            private const int NotInitialised = 1;
            private readonly PrimeGeneratorOptions options;
            private readonly CancellationToken cancellationToken;
            private int current = NotInitialised;

            internal AsyncEnumerator(PrimeGeneratorOptions options = PrimeGeneratorOptions.None, CancellationToken cancellationToken = default)
            {
                this.options = options;
                this.cancellationToken = cancellationToken;
            }

            int IAsyncEnumerator<int>.Current
            {
                get
                {
                    if (this.current == NotInitialised)
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

            private bool ShouldThrowOnCancel =>
                this.options.HasFlag(PrimeGeneratorOptions.ThrowOnCancel);

            ValueTask IAsyncDisposable.DisposeAsync() =>
                default;

            ValueTask<bool> IAsyncEnumerator<int>.MoveNextAsync()
            {
                do
                {
                    if (this.ShouldStop)
                    {
                        this.current = NotInitialised;
                        return new ValueTask<bool>(false);
                    }

                    this.current++;
                }
                while (!this.IsPrime);

                return new ValueTask<bool>(true);
            }

            private void ThrowIfNeeded()
            {
                if (this.ShouldThrowOnCancel)
                {
                    this.cancellationToken.ThrowIfCancellationRequested();
                }
            }
        }
    }
}
