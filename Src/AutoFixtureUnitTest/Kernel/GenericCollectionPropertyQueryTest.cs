using System.Collections;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class GenericCollectionPropertyQueryTest
    {
        [Fact]
        public void SutIsPropertyQuery()
        {
            // Arrange
            // Act
            var sut = new GenericCollectionPropertyQuery();

            // Assert
            Assert.IsAssignableFrom<IPropertyQuery>(sut);
        }

        [Fact]
        public void GenericCollectionPropertiesWillBeSelected()
        {
            // Arrange
            var sut = new GenericCollectionPropertyQuery();
            var type = typeof(CollectionHolder<string>);
            var expectedResult = type.GetTypeInfo().GetProperties();

            // Act
            var result = sut.SelectProperties(type);

            // Assert
            Assert.True(expectedResult.SequenceEqual(result));
        }

        [Fact]
        public void PropertiesThatAreNotGenericCollectionsWillNotBeSelected()
        {
            // Arrange
            var sut = new GenericCollectionPropertyQuery();
            var type = typeof(DoublePropertyHolder<string, ArrayList>);
            var expectedResult = Enumerable.Empty<PropertyInfo>();

            // Act
            var result = sut.SelectProperties(type);

            // Assert
            Assert.True(expectedResult.SequenceEqual(result));
        }
    }
}