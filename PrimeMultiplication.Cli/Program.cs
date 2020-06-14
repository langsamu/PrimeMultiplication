// MIT License, Copyright 2020 Samu Lang

namespace PrimeMultiplication.Cli
{
    using System.CommandLine;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a command line interface for generating a multiplication table of primes.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The entry point of the command line interface.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public static async Task Main(string[] args) =>
            await new PrimeMultiplicationCommand().InvokeAsync(args);
    }
}
