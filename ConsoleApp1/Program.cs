namespace ConsoleApp1
{
    using System.CommandLine;
    using System.Threading.Tasks;

    public static class Program
    {
        public static async Task Main(string[] args) =>
            await new PrimeMultiplicationCommand().InvokeAsync(args);
    }
}
