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
            Action dummy = delegate { };
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
            Action dummy = delegate { };
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new StaticMethod(null, dummy.GetMethodInfo().GetParameters()));
        }

        [Fact]
        public void InitializeWithNullMethodParametersThrows()
        {
            // Arrange
            Action dummy = delegate { };
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new StaticMethod(dummy.GetMethodInfo(), null));
        }

        [Fact]
        public void MethodIsCorrect()
        {
            // Arrange
            var expectedMethod = ((Action)delegate { }).GetMethodInfo();
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
            Action<object> dummy = delegate { };
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
            Action<int, double> dummy = delegate { };
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
            var method = ((Action<object>)delegate { }).GetMethodInfo();
            // Act
            var sut = new StaticMethod(method);
            // Assert
            Assert.IsAssignableFrom<IEquatable<StaticMethod>>(sut);
        }

        [Fact]
        public void SutDoesNotEqualNullObject()
        {
            // Arrange
            var method = ((Action<object>)delegate { }).GetMethodInfo();
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
            var method = ((Action<object>)delegate { }).GetMethodInfo();
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
            var method = ((Action<object>)delegate { }).GetMethodInfo();
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
            var method = ((Action<object>)delegate { }).GetMethodInfo();
            var sut = new StaticMethod(method);

            var otherMethod = ((Action<int>)delegate { }).GetMethodInfo();
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
            var method = ((Action<object>)delegate { }).GetMethodInfo();
            var sut = new StaticMethod(method, method.GetParameters());

            var otherParameters = ((Action<int>)delegate { }).GetMethodInfo().GetParameters();
            object other = new StaticMethod(method, otherParameters);

            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SutEqualsOtherObjectWithSameMethod()
        {
            var method = ((Action<object>)delegate { }).GetMethodInfo();
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
            var method = ((Action<object>)delegate { }).GetMethodInfo();
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
            var method = ((Action<object>)delegate { }).GetMethodInfo();
            var sut = new StaticMethod(method);
            // Act
            var result = sut.GetHashCode();
            // Assert
            var expectedHasCode = method.GetHashCode() ^ method.GetParameters().Aggregate(0, (current, parameter) => current + parameter.GetHashCode());
            Assert.Equal(expectedHasCode, result);
        }
    }
}
