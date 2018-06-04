using System;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class StaticMethodTest
    {
        [Fact]
        public void SutIsMethod()
        {
            // Arrange
            Action dummy = () => { };
            // Act
            var sut = new StaticMethod(dummy.GetMethodInfo());
            // Assert
            Assert.IsAssignableFrom<IMethod>(sut);
        }

        [Fact]
        public void InitializeOneParamsConstructorWithNullMethodInfoThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new StaticMethod(null));
        }

        [Fact]
        public void InitializeWithNullMethodInfoThrows()
        {
            // Arrange
            Action dummy = () => { };
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new StaticMethod(null, dummy.GetMethodInfo().GetParameters()));
        }

        [Fact]
        public void InitializeWithNullMethodParametersThrows()
        {
            // Arrange
            Action dummy = () => { };
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new StaticMethod(dummy.GetMethodInfo(), null));
        }

        [Fact]
        public void MethodIsCorrect()
        {
            // Arrange
            Action action = () => { };
            var expectedMethod = action.GetMethodInfo();
            var sut = new StaticMethod(expectedMethod);
            // Act
            var result = sut.Method;
            // Assert
            Assert.Equal(expectedMethod, result);
        }

        [Fact]
        public void ParametersIsCorrectWhenPassedOnOneParamsConstructor()
        {
            // Arrange
            Action<object> dummy = o => { };
            var expectedParameters = dummy.GetMethodInfo().GetParameters();
            var sut = new StaticMethod(dummy.GetMethodInfo());
            // Act
            var result = sut.Parameters;
            // Assert
            Assert.True(expectedParameters.SequenceEqual(result));
        }

        [Fact]
        public void ParametersIsCorrect()
        {
            // Arrange
            Action<int, double> dummy = (i, d) => { };
            var expectedParameters = dummy.GetMethodInfo().GetParameters();
            var sut = new StaticMethod(dummy.GetMethodInfo(), expectedParameters);
            // Act
            var result = sut.Parameters;
            // Assert
            Assert.True(expectedParameters.SequenceEqual(result));
        }

        [Theory]
        [InlineData(typeof(TypeWithFactoryMethod), 0, new object[] { })]
        [InlineData(typeof(TypeWithFactoryMethod), 1, new object[] { "abc" })]
        [InlineData(typeof(TypeWithFactoryMethod), 2, new object[] { new[] { "ab", "c" } })]
        public void InvokeWithFactoryMethodReturnsCorrectResult(Type targetType, int index, object values)
        {
            // Arrange
            var method = (from mi in targetType
                              .GetMethods(BindingFlags.Static | BindingFlags.Public)
                          where mi.ReturnType == targetType
                          select mi).ElementAt(index);
            var sut = new StaticMethod(method);

            // Act
            var result = sut.Invoke((object[])values);
            // Assert
            Assert.IsAssignableFrom(targetType, result);
        }

        [Fact]
        public void SutIsEquatable()
        {
            // Arrange
            Action<object> action = o => { };
            var method = action.GetMethodInfo();
            // Act
            var sut = new StaticMethod(method);
            // Assert
            Assert.IsAssignableFrom<IEquatable<StaticMethod>>(sut);
        }

        [Fact]
        public void SutDoesNotEqualNullObject()
        {
            // Arrange
            Action<object> action = o => { };
            var method = action.GetMethodInfo();
            var sut = new StaticMethod(method);
            // Act
            var result = sut.Equals((object)null);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SutDoesNotEqualNullSut()
        {
            // Arrange
            Action<object> action = o => { };
            var method = action.GetMethodInfo();
            var sut = new StaticMethod(method);
            // Act
            var result = sut.Equals((StaticMethod)null);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SutDoesNotEqualSomeOtherObject()
        {
            // Arrange
            Action<object> action = o => { };
            var method = action.GetMethodInfo();
            var sut = new StaticMethod(method);
            // Act
            var result = sut.Equals(new object());
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SutDoesNotEqualOtherObjectWithDifferentMethod()
        {
            // Arrange
            Action<object> action = o => { };
            var method = action.GetMethodInfo();
            var sut = new StaticMethod(method);

            Action<int> otherAction = i => { };
            var otherMethod = otherAction.GetMethodInfo();
            object other = new StaticMethod(otherMethod);

            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SutDoesNotEqualOtherObjectWithDifferentParameters()
        {
            // Arrange
            Action<object> action = o => { };
            var method = action.GetMethodInfo();
            var sut = new StaticMethod(method, method.GetParameters());

            Action<int> otherAction = i => { };
            var otherParameters = otherAction.GetMethodInfo().GetParameters();
            object other = new StaticMethod(method, otherParameters);

            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SutEqualsOtherObjectWithSameMethod()
        {
            Action<object> action = o => { };
            var method = action.GetMethodInfo();
            var sut = new StaticMethod(method);
            object other = new StaticMethod(method);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.True(result);
        }

        [Fact]
        public void SutEqualsOtherObjectWithSameMethodAndSameParameters()
        {
            Action<object> action = o => { };
            var method = action.GetMethodInfo();
            var parameters = method.GetParameters();
            var sut = new StaticMethod(method, parameters);
            object other = new StaticMethod(method, parameters);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GetHashCodeReturnsCorrectResult()
        {
            // Arrange
            Action<object> action = o => { };
            var method = action.GetMethodInfo();
            var sut = new StaticMethod(method);
            // Act
            var result = sut.GetHashCode();
            // Assert
            var expectedHasCode = method.GetHashCode() ^ method.GetParameters().Aggregate(0, (current, parameter) => current + parameter.GetHashCode());
            Assert.Equal(expectedHasCode, result);
        }
    }
}
