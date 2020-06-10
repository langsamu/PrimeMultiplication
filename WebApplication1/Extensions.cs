namespace WebApplication1
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;

    internal static class Extensions
    {
        internal static IEnumerable<int> Primes(this int count)
        {
            var n = 1; // will start at two anyway

            // Decrement count until zero
            while (count-- > 0)
            {
                // Increment number until prime
                while (!(++n).IsPrime())
                {
                }

                yield return n;
            }
        }

        internal static async IAsyncEnumerable<int> PrimesAsync(this int count, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var n = 1; // will start at two anyway

            // Decrement count until zero
            while (count-- > 0)
            {
                await Task.Run(IncrementUntilPrime, cancellationToken);

                yield return n;
            }

            void IncrementUntilPrime()
            {
                while (!(++n).IsPrime())
                {
                }
            }
        }

        internal static bool IsPrime(this int n)
        {
            var i = 2; // one divides all

            // Check up to square root of n
            while (i * i <= n)
            {
                // Check if divides and increment
                if (n % i++ == 0)
                {
                    return false;
                }
            }

            return true;
        }
    }

}
