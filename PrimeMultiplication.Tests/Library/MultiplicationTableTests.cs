namespace PrimeMultiplication.Tests.Library
{
    using System.Threading.Tasks;
    using PrimeMultiplication;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MultiplicationTableTests
    {
        [TestMethod]
        public async Task Multiplies_primes()
        {
            var expected = new int?[,] {
                { null,  2,  3,   5 },
                {    2,  4,  6,  10 },
                {    3,  6,  9 , 15 },
                {    5, 10, 15 , 25 },
            };

            var count = 3;
            var actual = new int?[count + 1, count + 1];

            await using var table = new MultiplicationTable(count).GetAsyncEnumerator();

            for (var rowNumber = 0; rowNumber <= count && await table.MoveNextAsync(); rowNumber++)
            {
                await using var row = table.Current.GetAsyncEnumerator();

                for (var cellNumber = 0; cellNumber <= count && await row.MoveNextAsync(); cellNumber++)
                {
                    actual[rowNumber, cellNumber] = row.Current;
                }
            }

            actual.Should().BeEquivalentTo(expected);
        }
    }
}
