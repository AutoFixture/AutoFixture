using System;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.AutoRhinoMock.UnitTest
{
    public class RhinoMockConstructorQueryTest
    {
        [Fact]
        public void SutIsMethodQuery()
        {
            // Arrange
            // Act
            var sut = new RhinoMockConstructorQuery();
            // Assert
            Assert.IsAssignableFrom<IMethodQuery>(sut);
        }

        [Fact]
        public void SelectMethodsFromNullTypeThrows()
        {
            // Arrange
            var sut = new RhinoMockConstructorQuery();
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.SelectMethods(null));
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(AbstractType))]
        public void SelectMethodsReturnsCorrectResultForNonInterfaces(Type t)
        {
            // Arrange
            var expectedCount = t.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Length;
            var sut = new RhinoMockConstructorQuery();
            // Act
            var result = sut.SelectMethods(t);
            // Assert
            Assert.Equal(expectedCount, result.Count());
        }

        [Theory]
        [InlineData(typeof(IInterface))]
        [InlineData(typeof(IComparable<object>))]
        [InlineData(typeof(IComparable<string>))]
        [InlineData(typeof(IComparable<int>))]
        public void SelectMethodsReturnsCorrectNumberOfMethodsForInterface(Type t)
        {
            // Arrange
            var sut = new RhinoMockConstructorQuery();
            // Act
            var result = sut.SelectMethods(t);
            // Assert
            Assert.Single(result);
        }

        [Theory]
        [InlineData(typeof(IInterface))]
        [InlineData(typeof(IComparable<object>))]
        [InlineData(typeof(IComparable<string>))]
        [InlineData(typeof(IComparable<int>))]
        public void SelectMethodsReturnsCorrectResultForInterface(Type t)
        {
            // Arrange
            var sut = new RhinoMockConstructorQuery();
            // Act
            var result = sut.SelectMethods(t);
            // Assert
            var method = Assert.IsAssignableFrom<RhinoMockConstructorMethod>(result.Single());
            Assert.Equal(t, method.MockTargetType);
        }

        [Theory]
        [InlineData(typeof(IInterface))]
        [InlineData(typeof(IComparable<object>))]
        [InlineData(typeof(IComparable<string>))]
        [InlineData(typeof(IComparable<int>))]
        public void SelectMethodsReturnsResultWithNoParametersForInterface(Type t)
        {
            // Arrange
            var sut = new RhinoMockConstructorQuery();
            // Act
            var result = sut.SelectMethods(t);
            // Assert
            var method = Assert.IsAssignableFrom<RhinoMockConstructorMethod>(result.Single());
            Assert.Empty(method.Parameters);
        }
    }
}
