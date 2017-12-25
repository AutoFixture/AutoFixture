using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Idioms;
using Xunit;

namespace AutoFixture.IdiomsUnitTest
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
            return this.i++;
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
            // Arrange
            var dummyIndex = 0;
            // Act
            var sut = new IndexedReplacement<T>(dummyIndex);
            // Assert
            Assert.IsAssignableFrom<IExpansion<T>>(sut);
        }

        [Fact]
        public void SourceIsCorrectWhenUsingEnumerableConstructor()
        {
            // Arrange
            var dummyIndex = 0;
            var source = Enumerable.Range(1, 3).Select(i => this.CreateItem()).ToList().AsEnumerable();
            var sut = new IndexedReplacement<T>(dummyIndex, source);
            // Act
            IEnumerable<T> result = sut.Source;
            // Assert
            Assert.True(source.SequenceEqual(result));
        }

        [Fact]
        public void SourceIsCorrectWhenUsingArrayConstructor()
        {
            // Arrange
            var dummyIndex = 0;
            var source = Enumerable.Range(1, 3).Select(i => this.CreateItem()).ToArray();
            var sut = new IndexedReplacement<T>(dummyIndex, source);
            // Act
            IEnumerable<T> result = sut.Source;
            // Assert
            Assert.True(source.SequenceEqual(result));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(42)]
        public void ReplacementIndexIsCorrectWhenUsingEnumerableConstructor(int expectedIndex)
        {
            // Arrange
            var sut = new IndexedReplacement<T>(expectedIndex, Enumerable.Empty<T>());
            // Act
            int result = sut.ReplacementIndex;
            // Assert
            Assert.Equal(expectedIndex, result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(42)]
        public void ReplacementIndexIsCorrectWhenUsingArrayConstructor(int expectedIndex)
        {
            // Arrange
            var sut = new IndexedReplacement<T>(expectedIndex);
            // Act
            int result = sut.ReplacementIndex;
            // Assert
            Assert.Equal(expectedIndex, result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void ExpandReturnsCorrectResult(int replacementIndex)
        {
            // Arrange
            var source = Enumerable.Range(1, 3).Select(i => this.CreateItem()).ToList();
            var sut = new IndexedReplacement<T>(replacementIndex, source);
            // Act
            var replacementValue = this.CreateItem();
            var result = sut.Expand(replacementValue);
            // Assert
            var expected = source.ToList();
            expected[replacementIndex] = replacementValue;
            Assert.True(expected.SequenceEqual(result));
        }

        protected abstract T CreateItem();
    }
}
