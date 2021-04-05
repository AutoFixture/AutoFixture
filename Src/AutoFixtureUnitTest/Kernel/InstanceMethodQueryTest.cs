using System;
using System.Linq;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class InstanceMethodQueryTest
    {
        [Fact]
        public void SutIsMethodQuery()
        {
            // Arrange
            var owner = new object();
            var methodName = string.Empty;

            // Act
            var sut = new InstanceMethodQuery(owner, methodName);

            // Assert
            Assert.IsAssignableFrom<IMethodQuery>(sut);
        }

        [Fact]
        public void ConstructionOfSutWithNullOwnerThrows()
        {
            // Arrange
            object owner = null;
            var methodName = string.Empty;

            // Act
            // Assert
            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.Throws<ArgumentNullException>(() => new InstanceMethodQuery(owner, methodName));
        }

        [Fact]
        public void ConstructionOfSutWithNullMethodNameThrows()
        {
            // Arrange
            var owner = new object();
            string methodName = null;

            // Act
            // Assert
            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.Throws<ArgumentNullException>(() => new InstanceMethodQuery(owner, methodName));
        }

        [Fact]
        public void SelectMethodsReturnsMethodFromOwner()
        {
            // Arrange
            var owner = new CollectionHolder<string>().Collection;
            var methodName = nameof(CollectionHolder<string>.Collection.Add);
            var expectedContents = string.Empty;
            var sut = new InstanceMethodQuery(owner, methodName);

            // Act
            var result = sut.SelectMethods();
            var enumerable = result.ToArray();
            enumerable.Single().Invoke(new[] { expectedContents });

            // Assert
            Assert.Single(enumerable);
            Assert.Single(owner, expectedContents);
        }

        [Fact]
        public void SelectMethodsReturnsEmptyEnumerableWhenOwnerDoesNotHaveMethod()
        {
            // Arrange
            var owner = new CollectionHolder<string>().Collection;
            var methodName = string.Empty;
            var sut = new InstanceMethodQuery(owner, methodName);

            // Act
            var result = sut.SelectMethods();

            // Assert
            Assert.Empty(result);
        }
    }
}