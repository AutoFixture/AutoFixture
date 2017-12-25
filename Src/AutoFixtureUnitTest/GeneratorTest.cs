using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class GeneratorTestOfObject : GeneratorTest<object> { }
    public class GeneratorTestOfString : GeneratorTest<string> { }
    public class GeneratorTestOfInt32 : GeneratorTest<int> { }
    public class GeneratorTestOfGuid : GeneratorTest<Guid> { }
    public class GeneratorTestOfNetPipeStyleUriParser : GeneratorTest<ConcreteType> { }
    public abstract class GeneratorTest<T>
    {
        [Theory, ClassData(typeof(CountTestCases))]
        public void StronglyTypedEnumerationYieldsNonDefaultValues(int count)
        {
            // Arrange
            var sut = new Generator<T>(new Fixture());
            // Act
            var actual = sut.Take(count);
            // Assert
            Assert.Equal(count, actual.Count(x => !object.Equals(default(T), x)));
        }

        [Theory, ClassData(typeof(CountTestCases))]
        public void StronglyTypedEnumerationYieldsUniqueValues(int count)
        {
            // Arrange
            var sut = new Generator<T>(new Fixture());
            // Act
            var actual = sut.Take(count);
            // Assert
            Assert.Equal(count, actual.Distinct().Count());
        }

        [Theory, ClassData(typeof(CountTestCases))]
        public void WeaklyTypedEnumerationYieldsNonDefaultValues(int count)
        {
            // Arrange
            IEnumerable sut = new Generator<T>(new Fixture());
            // Act
            var actual = sut.OfType<T>().Take(count);
            // Assert
            Assert.Equal(count, actual.Count(x => !object.Equals(default(T), x)));
        }

        [Theory, ClassData(typeof(CountTestCases))]
        public void WeaklyTypedEnumerationYieldsUniqueValues(int count)
        {
            // Arrange
            IEnumerable sut = new Generator<T>(new Fixture());
            // Act
            var actual = sut.OfType<T>().Take(count);
            // Assert
            Assert.Equal(count, actual.Distinct().Count());
        }

        [Fact]
        public void ConstructWithNullSpecimenBuilderThrows()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new Generator<T>((ISpecimenBuilder)null));
        }

        [Theory, ClassData(typeof(CountTestCases))]
        public void ConstructedWithSpecimenBuilderStronglyTypedEnumerationYieldsNonDefaultValues(int count)
        {
            // Arrange
            var builder = new Fixture();
            var sut = new Generator<T>(builder);
            // Act
            var actual = sut.OfType<T>().Take(count);
            // Assert
            Assert.Equal(
                count,
                actual.Count(x => !object.Equals(default(T), x)));
        }

        [Theory, ClassData(typeof(CountTestCases))]
        public void ConstructedWithSpecimenBuilderWeaklyTypedEnumerationYieldsNonDefaultValues(int count)
        {
            // Arrange
            var builder = new Fixture();
            IEnumerable sut = new Generator<T>(builder);
            // Act
            var actual = sut.OfType<T>().Take(count);
            // Assert
            Assert.Equal(
                count,
                actual.Count(x => !object.Equals(default(T), x)));
        }

        [Theory, ClassData(typeof(CountTestCases))]
        public void ConstructedWithSpecimenBuilderStronglyTypedEnumerationYieldsDistinctValues(int count)
        {
            // Arrange
            var builder = new Fixture();
            var sut = new Generator<T>(builder);
            // Act
            var actual = sut.OfType<T>().Take(count);
            // Assert
            Assert.Equal(
                count,
                actual.Distinct().Count());
        }

        [Theory, ClassData(typeof(CountTestCases))]
        public void ConstructedWithSpecimenBuilderWeaklyTypedEnumerationYieldsDistinctValues(int count)
        {
            // Arrange
            var builder = new Fixture();
            IEnumerable sut = new Generator<T>(builder);
            // Act
            var actual = sut.OfType<T>().Take(count);
            // Assert
            Assert.Equal(
                count,
                actual.Distinct().Count());
        }
    }

    internal class CountTestCases : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 0 };
            yield return new object[] { 1 };
            yield return new object[] { 2 };
            yield return new object[] { 3 };
            yield return new object[] { 20 };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
