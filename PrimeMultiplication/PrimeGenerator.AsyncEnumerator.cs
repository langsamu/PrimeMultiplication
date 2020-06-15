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
            private int candidate;

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
                    for (int divisor = 2, limit = (int)Math.Sqrt(this.candidate); divisor <= limit; divisor++)
                    {
                        this.ThrowIfNeeded();

                        if (this.candidate % divisor == 0)
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
                this.candidate = this.current;

                do
                {
                    if (this.ShouldStop)
                    {
                        this.current = NotInitialised;
                        return new ValueTask<bool>(false);
                    }

                    this.candidate++;
                }
                while (!this.IsPrime);

                this.current = this.candidate;
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
