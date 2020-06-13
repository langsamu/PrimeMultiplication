namespace UnitTestProject1.ClassLibrary1
{
    using System;
    using System.Collections;
    using FluentAssertions;
    using global::ClassLibrary1;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class EnumeratorTests
    {
        [TestMethod]
        public void Cannot_be_reset()
        {
            var generator = new PrimeGenerator();
            var enumerable = (IEnumerable)generator;
            var enumerator = enumerable.GetEnumerator();

            enumerator.Invoking(e => e.Reset()).Should().Throw<InvalidOperationException>();
        }
    }
}
