using System;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.AutoRhinoMock.UnitTest
{
    public class RhinoMockConstructorMethodTest
    {
        [Fact]
        public void SutImplementsISpecimenBuilder()
        {
            // Act
            var sut = new RhinoMockConstructorMethod(typeof(RhinoMockConstructorMethod), Enumerable.Empty<ParameterInfo>().ToArray());

            // Assert
            Assert.IsAssignableFrom<IMethod>(sut);
        }

        [Fact]
        public void ConstructorWithNullConstructorMethodThrows()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => 
                new RhinoMockConstructorMethod(null, Enumerable.Empty<ParameterInfo>().ToArray()));
        }

        [Fact]
        public void ConstructorWithNullParameterInfoArray()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new RhinoMockConstructorMethod(typeof(RhinoMockConstructorMethod), null));
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        [InlineData(typeof(AbstractType))]
        [InlineData(typeof(IInterface))]
        [InlineData(typeof(IComparable<object>))]
        [InlineData(typeof(IComparable<string>))]
        [InlineData(typeof(IComparable<int>))]
        public void MockTargetTypeIsCorrect(Type t)
        {
            // Arrange
            var sut = new RhinoMockConstructorMethod(t, new ParameterInfo[0]);
            // Act
            Type result = sut.MockTargetType;
            // Assert
            Assert.Equal(t, result);
        }
    }
}