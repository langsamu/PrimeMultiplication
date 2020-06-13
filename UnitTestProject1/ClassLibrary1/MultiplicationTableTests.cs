namespace UnitTestProject1.ClassLibrary1
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using global::ClassLibrary1;
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

            var size = 4;
            var actual = new int?[size, size];

            await using var table = new MultiplicationTable(10).GetAsyncEnumerator();

            for (var y = 0; y < size && await table.MoveNextAsync(); y++)
            {
                await using var row = table.Current.GetAsyncEnumerator();

                for (var x = 0; x < size && await row.MoveNextAsync(); x++)
                {
                    actual[y, x] = row.Current;
                }
            }

            actual.Should().BeEquivalentTo(expected);
        }
    }
}
