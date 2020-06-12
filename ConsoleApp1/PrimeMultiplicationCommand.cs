namespace ConsoleApp1
{
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.CommandLine.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using ClassLibrary1;

    internal class PrimeMultiplicationCommand : RootCommand
    {
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
            if (timeout.HasValue)
            {
                cancellationToken = 
                    CancellationTokenSource.CreateLinkedTokenSource(
                        cancellationToken,
                        new CancellationTokenSource(timeout.Value).Token).Token;
            }

            var options = throwOnCancel ?
                PrimeGeneratorOptions.ThrowOnCancel :
                PrimeGeneratorOptions.None;

            var table = new MultiplicationTable(size, options);

            await foreach (var row in table.WithCancellation(cancellationToken))
            {
                await foreach (var cell in row)
                {
                    console.Out.Write(string.Format("{0, 10}", cell));
                }

                console.Out.WriteLine(); // LF
            }
        }
    }
}
