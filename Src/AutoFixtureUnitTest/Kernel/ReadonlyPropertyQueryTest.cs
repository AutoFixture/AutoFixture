using System.Collections;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class ReadonlyPropertyQueryTest
    {
        [Fact]
        public void SutIsPropertyQuery()
        {
            // Arrange
            // Act
            var sut = new ReadonlyPropertyQuery();

            // Assert
            Assert.IsAssignableFrom<IPropertyQuery>(sut);
        }

        [Fact]
        public void ReadonlyPropertiesWillBeSelected()
        {
            // Arrange
            var sut = new ReadonlyPropertyQuery();
            var type = typeof(DoubleParameterType<string, ArrayList>);
            var expectedResult = type.GetTypeInfo().GetProperties();

            // Act
            var result = sut.SelectProperties(type);

            // Assert
            Assert.True(expectedResult.SequenceEqual(result));
        }

        [Fact]
        public void WriteablePropertiesWillNotBeSelected()
        {
            // Arrange
            var sut = new ReadonlyPropertyQuery();
            var type = typeof(DoublePropertyHolder<string, ArrayList>);
            var expectedResult = Enumerable.Empty<PropertyInfo>();

            // Act
            var result = sut.SelectProperties(type);

            // Assert
            Assert.True(expectedResult.SequenceEqual(result));
        }
    }
}