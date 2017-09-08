using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Idioms;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    #region <object>
    public class IndexedReplacementTestOfObject : IndexedReplacementTest<object>
    {
        protected override object CreateItem()
        {
            return new object();
        }
    }
    #endregion
    #region <int>
    public class IndexedReplacementTestOfInt : IndexedReplacementTest<int>
    {
        private int i;

        protected override int CreateItem()
        {
            return i++;
        }
    }
    #endregion
    #region <string>
    public class IndexedReplacementTestOfString : IndexedReplacementTest<string>
    {
        protected override string CreateItem()
        {
            return Guid.NewGuid().ToString();
        }
    }
    #endregion
    #region <Version>
    public class IndexedReplacementTestOfVersion : IndexedReplacementTest<Version>
    {
        protected override Version CreateItem()
        {
            return new Version();
        }
    }
    #endregion
    public abstract class IndexedReplacementTest<T>
    {
        [Fact]
        public void SutIsExpansion()
        {
            // Fixture setup
            var dummyIndex = 0;
            // Exercise system
            var sut = new IndexedReplacement<T>(dummyIndex);
            // Verify outcome
            Assert.IsAssignableFrom<IExpansion<T>>(sut);
            // Teardown
        }

        [Fact]
        public void SourceIsCorrectWhenUsingEnumerableConstructor()
        {
            // Fixture setup
            var dummyIndex = 0;
            var source = Enumerable.Range(1, 3).Select(i => this.CreateItem()).ToList().AsEnumerable();
            var sut = new IndexedReplacement<T>(dummyIndex, source);
            // Exercise system
            IEnumerable<T> result = sut.Source;
            // Verify outcome
            Assert.True(source.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void SourceIsCorrectWhenUsingArrayConstructor()
        {
            // Fixture setup
            var dummyIndex = 0;
            var source = Enumerable.Range(1, 3).Select(i => this.CreateItem()).ToArray();
            var sut = new IndexedReplacement<T>(dummyIndex, source);
            // Exercise system
            IEnumerable<T> result = sut.Source;
            // Verify outcome
            Assert.True(source.SequenceEqual(result));
            // Teardown
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(42)]
        public void ReplacementIndexIsCorrectWhenUsingEnumerableConstructor(int expectedIndex)
        {
            // Fixture setup
            var sut = new IndexedReplacement<T>(expectedIndex, Enumerable.Empty<T>());
            // Exercise system
            int result = sut.ReplacementIndex;
            // Verify outcome
            Assert.Equal(expectedIndex, result);
            // Teardown
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(42)]
        public void ReplacementIndexIsCorrectWhenUsingArrayConstructor(int expectedIndex)
        {
            // Fixture setup
            var sut = new IndexedReplacement<T>(expectedIndex);
            // Exercise system
            int result = sut.ReplacementIndex;
            // Verify outcome
            Assert.Equal(expectedIndex, result);
            // Teardown
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void ExpandReturnsCorrectResult(int replacementIndex)
        {
            // Fixture setup
            var source = Enumerable.Range(1, 3).Select(i => this.CreateItem()).ToList();
            var sut = new IndexedReplacement<T>(replacementIndex, source);
            // Exercise system
            var replacementValue = this.CreateItem();
            var result = sut.Expand(replacementValue);
            // Verify outcome
            var expected = source.ToList();
            expected[replacementIndex] = replacementValue;
            Assert.True(expected.SequenceEqual(result));
            // Teardown
        }

        protected abstract T CreateItem();
    }
}
