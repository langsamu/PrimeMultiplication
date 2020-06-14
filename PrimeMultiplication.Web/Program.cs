// MIT License, Copyright 2020 Samu Lang

namespace PrimeMultiplication.Web
{
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Usage is convention based")]
    public static class Program
    {
        /// <summary>
        /// A magic string to indicate a testing scenario.
        /// </summary>
        public const string TestingToken = "--test-timeout";

        [SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Provided by framework")]
        [SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Out of scope for this demo")]
        public static async Task Main(string[] args)
        {
            /*
             * If the first argument is the testing token,
             * then the second argument is timeout in ms,
             * otherwise there's no timeout.
             *
             * This is so the host turns off automatically
             * in a test scenario.
             */

            var timeout = default(CancellationToken);
            if (args.Length == 2)
            {
                if (args[0] == TestingToken)
                {
                    var millisecondsDelay = int.Parse(args[1], CultureInfo.InvariantCulture);
                    timeout = new CancellationTokenSource(millisecondsDelay).Token;
                }
            }

            await CreateHostBuilder(args).Build().RunAsync(timeout);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
