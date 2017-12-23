using System;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class ConstructorMethodTest
    {
        [Fact]
        public void SutIsMethod()
        {
            // Arrange
            var dummyCtor = typeof(ConcreteType).GetConstructor(Type.EmptyTypes);
            // Act
            var sut = new ConstructorMethod(dummyCtor);
            // Assert
            Assert.IsAssignableFrom<IMethod>(sut);
        }

        [Fact]
        public void InitializeWithNullConstructorInfoThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new ConstructorMethod(null));
        }

        [Fact]
        public void ConstructorIsCorrect()
        {
            // Arrange
            var expectedCtor = typeof(ConcreteType).GetConstructor(Type.EmptyTypes);
            var sut = new ConstructorMethod(expectedCtor);
            // Act
            ConstructorInfo result = sut.Constructor;
            // Assert
            Assert.Equal(expectedCtor, result);
        }

        [Theory]
        [InlineData(typeof(ConcreteType), 0)]
        [InlineData(typeof(ConcreteType), 1)]
        [InlineData(typeof(ConcreteType), 2)]
        [InlineData(typeof(ConcreteType), 3)]
        [InlineData(typeof(SingleParameterType<int>), 0)]
        [InlineData(typeof(SingleParameterType<string>), 0)]
        public void ParametersReturnsCorrectResult(Type targetType, int index)
        {
            // Arrange
            var ctor = targetType.GetConstructors().ElementAt(index);
            var expectedParameters = ctor.GetParameters();

            var sut = new ConstructorMethod(ctor);
            // Act
            var result = sut.Parameters;
            // Assert
            Assert.True(expectedParameters.SequenceEqual(result));
        }

        [Theory]
        [InlineData(typeof(PropertyHolder<int>))]
        [InlineData(typeof(PropertyHolder<string>))]
        public void InvokeWithDefaultConstructorReturnsCorrectResult(Type targetType)
        {
            // Arrange
            var ctor = targetType.GetConstructor(Type.EmptyTypes);
            var sut = new ConstructorMethod(ctor);
            // Act
            var result = sut.Invoke(Enumerable.Empty<object>());
            // Assert
            Assert.IsAssignableFrom(targetType, result);
        }

        [Theory]
        [InlineData(typeof(SingleParameterType<int>), 0)]
        [InlineData(typeof(SingleParameterType<string>), "")]
        public void InvokeWithSingleParameterReturnsCorrectResult(Type targetType, object parameter)
        {
            // Arrange
            var ctor = targetType.GetConstructor(targetType.GetTypeInfo().GetGenericArguments().ToArray());
            var sut = new ConstructorMethod(ctor);
            // Act
            var result = sut.Invoke(new[] { parameter });
            // Assert
            Assert.IsAssignableFrom(targetType, result);
        }

        [Theory]
        [InlineData(typeof(DoubleParameterType<int, int>), 0, 1)]
        [InlineData(typeof(DoubleParameterType<string, string>), "", "foo")]
        [InlineData(typeof(DoubleParameterType<string, int>), "", 2)]
        public void InvokeWithTwoParametersReturnsCorrectResult(Type targetType, object parameter1, object parameter2)
        {
            // Arrange
            var ctor = targetType.GetConstructor(targetType.GetTypeInfo().GetGenericArguments().ToArray());
            var sut = new ConstructorMethod(ctor);
            // Act
            var result = sut.Invoke(new[] { parameter1, parameter2 });
            // Assert
            Assert.IsAssignableFrom(targetType, result);
        }

        [Fact]
        public void SutIsEquatable()
        {
            // Arrange
            var dummyCtor = typeof(ConcreteType).GetConstructor(Type.EmptyTypes);
            // Act
            var sut = new ConstructorMethod(dummyCtor);
            // Assert
            Assert.IsAssignableFrom<IEquatable<ConstructorMethod>>(sut);
        }

        [Fact]
        public void SutDoesNotEqualNullObject()
        {
            // Arrange
            var dummyCtor = typeof(ConcreteType).GetConstructor(Type.EmptyTypes);
            var sut = new ConstructorMethod(dummyCtor);
            // Act
            var result = sut.Equals((object)null);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SutDoesNotEqualNullSut()
        {
            // Arrange
            var dummyCtor = typeof(ConcreteType).GetConstructor(Type.EmptyTypes);
            var sut = new ConstructorMethod(dummyCtor);
            // Act
            var result = sut.Equals((ConstructorMethod)null);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SutDoesNotEqualSomeOtherObject()
        {
            // Arrange
            var dummyCtor = typeof(ConcreteType).GetConstructor(Type.EmptyTypes);
            var sut = new ConstructorMethod(dummyCtor);
            // Act
            var result = sut.Equals(new object());
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SutDoesNotEqualOtherObjectWithDifferentCtor()
        {
            // Arrange
            var ctor1 = typeof(ConcreteType).GetConstructor(Type.EmptyTypes);
            var sut = new ConstructorMethod(ctor1);

            var ctor2 = typeof(PropertyHolder<string>).GetConstructor(Type.EmptyTypes);
            object other = new ConstructorMethod(ctor2);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SutDoesNotEqualOtherSutWithDifferentCtor()
        {
            // Arrange
            var ctor1 = typeof(ConcreteType).GetConstructor(Type.EmptyTypes);
            var sut = new ConstructorMethod(ctor1);

            var ctor2 = typeof(PropertyHolder<string>).GetConstructor(Type.EmptyTypes);
            var other = new ConstructorMethod(ctor2);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SutEqualsOtherObjectWithSameCtor()
        {
            // Arrange
            var ctor = typeof(ConcreteType).GetConstructor(Type.EmptyTypes);
            var sut = new ConstructorMethod(ctor);
            object other = new ConstructorMethod(ctor);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.True(result);
        }

        [Fact]
        public void SutEqualsOtherSutWithSameCtor()
        {
            // Arrange
            var ctor = typeof(ConcreteType).GetConstructor(Type.EmptyTypes);
            var sut = new ConstructorMethod(ctor);
            var other = new ConstructorMethod(ctor);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GetHashCodeReturnsCorrectResult()
        {
            // Arrange
            var ctor = typeof(ConcreteType).GetConstructor(Type.EmptyTypes);
            var sut = new ConstructorMethod(ctor);
            // Act
            var result = sut.GetHashCode();
            // Assert
            var expectedHasCode = ctor.GetHashCode();
            Assert.Equal(expectedHasCode, result);
        }
    }
}
