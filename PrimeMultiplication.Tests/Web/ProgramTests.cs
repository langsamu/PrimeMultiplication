namespace PrimeMultiplication.Tests.Web
{
    using System;
    using System.IO;
    using System.Threading;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PrimeMultiplication.Web;

    [TestClass]
    public class ProgramTests
    {
        [TestMethod]
        public void Creates_web_host()
        {
            using var writer = new StringWriter();
            Console.SetOut(writer);

            // As if we were running:
            // c:\>program.exe --test-timeout 500
            async static void main() =>
                await Program.Main(
                    new[]
                    {
                        Program.TestingToken,
                        500.ToString()
                    });

            var program = new Thread(main);
            program.Start();
            program.Join();

            var actual = writer.ToString();
            actual.Should().Contain("http://localhost:5000");
        }
    }
}
