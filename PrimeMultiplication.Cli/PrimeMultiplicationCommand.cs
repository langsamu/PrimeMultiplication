// MIT License, Copyright 2020 Samu Lang

namespace PrimeMultiplication.Cli
{
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.CommandLine.IO;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using PrimeMultiplication;

    internal class PrimeMultiplicationCommand : RootCommand
    {
        internal PrimeMultiplicationCommand()
        {
            this.Description = "Generates a prime multiplication table of <size> optionally within <timeout>";
            this.Handler = CommandHandler.Create<int, int?, bool, IConsole, CancellationToken>(this.ExecuteAsync);

            var countArgument = new Argument<int>("count", "Number of columns and rows");
            this.AddArgument(countArgument);

            var timeoutOption = new Option<int?>("--timeout", "Time limit in milliseconds");
            timeoutOption.AddAlias("-t");
            this.AddOption(timeoutOption);

            var throwOption = new Option("--throw-on-cancel", "Fail on timeout instead of just stopping");
            throwOption.AddAlias("-c");
            this.AddOption(throwOption);
        }

        [SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Out of scope for this demo")]
        private async Task ExecuteAsync(int count, int? timeout, bool throwOnCancel, IConsole console, CancellationToken cancellationToken)
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

            var table = new MultiplicationTable(count, options);

            await foreach (var row in table.WithCancellation(cancellationToken))
            {
                await foreach (var cell in row)
                {
                    console.Out.Write($"{cell,10}");
                }

                console.Out.WriteLine(); // LF
            }
        }
    }
}
