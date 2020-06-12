namespace ClassLibrary1
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;

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

            var primes = new PrimeGenerator(this.options).Take(this.count);

            // rest of rows
            await foreach (var prime in primes.WithCancellation(cancellationToken))
            {
                yield return this.GenerateRowAsync(cancellationToken, prime);
            }
        }

        private async IAsyncEnumerable<int?> GenerateRowAsync([EnumeratorCancellation] CancellationToken cancellationToken, int? header = null)
        {
            // header column cell
            yield return header;

            var primes = new PrimeGenerator(this.options).Take(this.count);

            // rest of cells
            await foreach (var prime in primes.WithCancellation(cancellationToken))
            {
                yield return (header ?? 1) * prime;
            }
        }

        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "It's really just for fun")]
        private static ConfiguredCancelableAsyncEnumerable<IAsyncEnumerable<int?>> OneLinerJustForFun(int count, CancellationToken cancellationToken = default, PrimeGeneratorOptions options = PrimeGeneratorOptions.None) =>
           /* Elements of the produced table as they
            * relate to lines in the algorithm below:
            * 
            *         @4        @5
            *           ⭨ ┌─────┴─────┐
            *           ┌ ␀  2  3  5  7
            *           │ 2  4  6 10 14
            *        @3 ┤ 3  6  9 15 21
            *           │ 5 10 15 25 35
            *           └ 7 14 21 35 49
            *             ↑
            *            @11
            */

           /*  1: */ new PrimeGenerator(options)
           /*  2: */ .Take(count)                             // number of rows
           /*  3: */ .Select(prime => (int?)prime)            // make nullable for topmost cell
           /*  4: */ .Prepend(null)                           // column header (topmost) cell
           /*  5: */ .Select(rowHeader =>                     // rows
           /*  6: */     new PrimeGenerator(options)
           /*  7: */     .Take(count)                         // number of columns
           /*  8: */     .Select(prime => (int?)prime)        // make nullable for leftmost cell
           /*  9: */     .Select(columnHeader =>              // cells
           /* 10: */         (rowHeader ?? 1) * columnHeader) // coalesce multiplicative identity for headerless top row
           /* 11: */     .Prepend(rowHeader))                 // row header (leftmost) cell
           /* 12: */ .WithCancellation(cancellationToken);
    }
}
