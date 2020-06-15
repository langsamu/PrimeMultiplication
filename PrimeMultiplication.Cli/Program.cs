// MIT License, Copyright 2020 Samu Lang

namespace PrimeMultiplication.Cli
{
    using System.CommandLine;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.Extensibility;

    /// <summary>
    /// Represents a command line interface for generating a multiplication table of primes.
    /// </summary>
    public static class Program
    {
        internal const string InstrumentationKey = "75c77ab9-c0fe-4d24-8f0b-0950b0c22e10";

        /// <summary>
        /// The entry point of the command line interface.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public static async Task Main(string[] args)
        {
            using var telemetryConfiguration = TelemetryConfiguration.CreateDefault();
            telemetryConfiguration.InstrumentationKey = InstrumentationKey;

            var telemetry = new TelemetryClient(telemetryConfiguration);
            telemetry.TrackEvent("CLI launched");

            await new PrimeMultiplicationCommand().InvokeAsync(args);

            telemetry.TrackEvent("CLI terminating");

            telemetry.Flush();
            await Task.Delay(300);
        }
    }
}
