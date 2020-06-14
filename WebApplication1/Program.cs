namespace WebApplication1
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;

    public class Program
    {
        public const string TestingToken = "--test-timeout";

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
                    var millisecondsDelay = int.Parse(args[1]);
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
}
