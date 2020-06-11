namespace ConsoleApp1
{
    using System.CommandLine;
    using System.Threading.Tasks;

    internal class Program
    {
        private static async Task Main(string[] args) =>
            await new PrimeMultiplicationCommand().InvokeAsync(args);
    }
}
