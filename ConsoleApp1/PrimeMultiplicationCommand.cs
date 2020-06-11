namespace ConsoleApp1
{
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.CommandLine.IO;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;
    using ClassLibrary1;

    internal class PrimeMultiplicationCommand : RootCommand
    {
        private int size;
        private PrimeGeneratorOptions options;
        private CancellationToken cancellationToken;
        private IConsole console;

        internal PrimeMultiplicationCommand()
        {
            this.Description = "Generates a prime multiplication table of <size> optionally within <timeout>";
            this.Handler = CommandHandler.Create<int, int?, bool, IConsole, CancellationToken>(this.ExecuteAsync);

            var countArgument = new Argument<int>("size", "Number of columns and rows");
            this.AddArgument(countArgument);

            var timeoutOption = new Option<int?>("--timeout", "Time limit in milliseconds");
            timeoutOption.AddAlias("-t");
            this.AddOption(timeoutOption);

            var throwOption = new Option("--throw-on-cancel", "Fail on timeout instead of just stopping");
            throwOption.AddAlias("-c");
            this.AddOption(throwOption);
        }

        private async Task ExecuteAsync(int size, int? timeout, bool throwOnCancel, IConsole console, CancellationToken cancellationToken)
        {
            this.size = size;

            this.cancellationToken = timeout.HasValue ?
                cancellationToken :
                CancellationTokenSource.CreateLinkedTokenSource(
                    cancellationToken,
                    new CancellationTokenSource(timeout.Value).Token).Token;

            this.options = throwOnCancel ?
                PrimeGeneratorOptions.ThrowOnCancel :
                PrimeGeneratorOptions.None;

            this.console = console;

            await this.WriteTableAsync();
        }

        private async Task WriteTableAsync()
        {
            // Header row (top)
            await this.WriteRowAsync();

            await using var primes = this.CreateGenerator();

            // Body
            for (var i = 0; i < this.size && await primes.MoveNextAsync(); i++)
            {
                await this.WriteRowAsync(primes.Current);
            }
        }

        private async Task WriteRowAsync(int? rowHeader = null)
        {
            // Row header cell (leftmost column)
            this.WriteCell(rowHeader);

            await using var primes = this.CreateGenerator();

            // Cells
            for (var i = 0; i < this.size && await primes.MoveNextAsync(); i++)
            {
                this.WriteCell(primes.Current, rowHeader ?? 1);
            }

            this.console.Out.WriteLine(); // LF
        }

        private void WriteCell(int? prime1 = null, int? prime2 = 1) =>
            this.console.Out.Write(
                string.Format(
                    "{0, 10}",
                    prime1 * prime2));

        private ConfiguredCancelableAsyncEnumerable<int>.Enumerator CreateGenerator() =>
            new PrimeGenerator(this.options)
            .WithCancellation(this.cancellationToken)
            .GetAsyncEnumerator();
    }
}
