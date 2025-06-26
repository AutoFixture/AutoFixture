using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class MethodByParametersCountQueryTest
    {
        [Fact]
        public void SutIsMethodQuery()
        {
            var sut = new MethodByParametersCountQuery(new object(), string.Empty, 1);

            Assert.IsAssignableFrom<IMethodQuery>(sut);
        }

        [Fact]
        public void ConstructionOfSutWithNullOwnerThrows()
        {
            Assert.Throws<ArgumentNullException>(() => new MethodByParametersCountQuery(null, string.Empty, 1));
        }

        [Fact]
        public void ConstructionOfSutWithNullMethodNameThrows()
        {
            Assert.Throws<ArgumentNullException>(() => new MethodByParametersCountQuery(new object(), null, 1));
        }

        [Fact]
        public void SelectMethodsReturnsMethodFromCollection()
        {
            // Arrange
            var owner = new CollectionHolder<string>().Collection;
            var methodName = nameof(CollectionHolder<string>.Collection.Add);
            var expectedContents = string.Empty;
            var sut = new MethodByParametersCountQuery(owner, methodName, 1);

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
            var sut = new MethodByParametersCountQuery(owner, methodName, 1);

            // Act
            var result = sut.SelectMethods();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void SelectMethodsReturnsMethodFromDictionary()
        {
            // Arrange
            var owner = new DictionaryHolder<string, string>().Dictionary;
            var methodName = nameof(DictionaryHolder<string, string>.Dictionary.Add);
            object expectedContents = new KeyValuePair<string, string>("key", "value");
            var sut = new MethodByParametersCountQuery(owner, methodName, 1);

            // Act
            var result = sut.SelectMethods();
            var enumerable = result.ToArray();
            enumerable.Single().Invoke(new[] { expectedContents });

            // Assert
            Assert.Single(enumerable);
            Assert.Single(owner, expectedContents);
        }

    }
}