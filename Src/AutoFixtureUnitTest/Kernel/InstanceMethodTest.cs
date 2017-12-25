using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class InstanceMethodTest
    {
        [Fact]
        public void SutIsMethod()
        {
            // Arrange
            var dummyMethod = typeof(object).GetMethods(BindingFlags.Public | BindingFlags.Instance).First();
            var dummyOwner = new object();
            // Act
            var sut = new InstanceMethod(dummyMethod, dummyOwner);
            // Assert
            Assert.IsAssignableFrom<IMethod>(sut);
        }

        [Fact]
        public void ConstructWithNullMethodThrows()
        {
            // Arrange
            var dummyOwner = new object();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new InstanceMethod(null, dummyOwner));
        }

        [Fact]
        public void ConstructWithNullOwnerThrows()
        {
            // Arrange
            var dummyMethod = typeof(object).GetMethods(BindingFlags.Public | BindingFlags.Instance).First();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new InstanceMethod(dummyMethod, null));
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(DateTime))]
        public void MethodIsCorrect(Type type)
        {
            // Arrange
            var expectedMethod = type.GetMethods(BindingFlags.Public | BindingFlags.Instance).First();
            var dummyOwner = new object();
            var sut = new InstanceMethod(expectedMethod, dummyOwner);
            // Act
            MethodInfo result = sut.Method;
            // Assert
            Assert.Equal(expectedMethod, result);
        }

        [Fact]
        public void OwnerIsCorrect()
        {
            // Arrange
            var dummyMethod = typeof(object).GetMethods(BindingFlags.Public | BindingFlags.Instance).First();
            var expectedOwner = new object();
            var sut = new InstanceMethod(dummyMethod, expectedOwner);
            // Act
            var result = sut.Owner;
            // Assert
            Assert.Equal(expectedOwner, result);
        }

        [Theory]
        [InlineData(typeof(GuardedMethodHost), 0)]
        [InlineData(typeof(GuardedMethodHost), 1)]
        [InlineData(typeof(GuardedMethodHost), 2)]
        [InlineData(typeof(GuardedMethodHost), 3)]
        [InlineData(typeof(GuardedMethodHost), 4)]
        [InlineData(typeof(GuardedMethodHost), 5)]
        [InlineData(typeof(GuardedMethodHost), 6)]
        public void ParametersReturnsCorrectResult(Type type, int index)
        {
            // Arrange
            var method = type.GetMethods(BindingFlags.Public | BindingFlags.Instance).ElementAt(index);
            var expectedParameters = method.GetParameters();

            var dummyOwner = new object();
            var sut = new InstanceMethod(method, dummyOwner);
            // Act
            var result = sut.Parameters;
            // Assert
            Assert.True(expectedParameters.SequenceEqual(result));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData("")]
        [InlineData("Ploeh")]
        [InlineData(false)]
        [InlineData(true)]
        public void InvokeParameterlessMethodReturnsCorrectResult(object owner)
        {
            // Arrange
            var method = owner.GetType().GetMethod("GetHashCode", Type.EmptyTypes);
            var sut = new InstanceMethod(method, owner);
            // Act
            var result = sut.Invoke(Enumerable.Empty<object>());
            // Assert
            var expected = owner.GetHashCode();
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 0)]
        [InlineData(0, 1)]
        [InlineData(1, 1)]
        [InlineData(42, 7)]
        public void InvokeMethodWithParametersReturnsCorrectResult(int x, int y)
        {
            // Arrange
            var owner = Comparer<int>.Default;
            var method = owner.GetType().GetMethod("Compare");
            var sut = new InstanceMethod(method, owner);
            // Act
            var result = sut.Invoke(new object[] { x, y });
            // Assert
            var expected = owner.Compare(x, y);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SutIsEquatable()
        {
            // Arrange
            var dummyMethod = typeof(object).GetMethods(BindingFlags.Public | BindingFlags.Instance).First();
            var dummyOwner = new object();
            // Act
            var sut = new InstanceMethod(dummyMethod, dummyOwner);
            // Assert
            Assert.IsAssignableFrom<IEquatable<InstanceMethod>>(sut);
        }

        [Fact]
        public void SutDoesNotEqualNullObject()
        {
            // Arrange
            var dummyMethod = typeof(object).GetMethods(BindingFlags.Public | BindingFlags.Instance).First();
            var dummyOwner = new object();
            var sut = new InstanceMethod(dummyMethod, dummyOwner);
            // Act
            var result = sut.Equals((object)null);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SutDoesNotEqualNullSut()
        {
            // Arrange
            var dummyMethod = typeof(object).GetMethods(BindingFlags.Public | BindingFlags.Instance).First();
            var dummyOwner = new object();
            var sut = new InstanceMethod(dummyMethod, dummyOwner);
            // Act
            var result = sut.Equals((InstanceMethod)null);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SutDoesNotEqualSomeOtherObject()
        {
            // Arrange
            var dummyMethod = typeof(object).GetMethods(BindingFlags.Public | BindingFlags.Instance).First();
            var dummyOwner = new object();
            var sut = new InstanceMethod(dummyMethod, dummyOwner);
            // Act
            var result = sut.Equals(new object());
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SutDoesNotEqualOtherObjectWithDifferentMethod()
        {
            // Arrange
            var method1 = typeof(object).GetMethods(BindingFlags.Public | BindingFlags.Instance).First();
            var dummyOwner = new object();
            var sut = new InstanceMethod(method1, dummyOwner);

            var method2 = typeof(object).GetMethods(BindingFlags.Public | BindingFlags.Instance).Skip(1).First();
            object other = new InstanceMethod(method2, dummyOwner);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SutDoesNotEqualOtherSutWithDifferentMethod()
        {
            // Arrange
            var method1 = typeof(object).GetMethods(BindingFlags.Public | BindingFlags.Instance).First();
            var dummyOwner = new object();
            var sut = new InstanceMethod(method1, dummyOwner);

            var method2 = typeof(object).GetMethods(BindingFlags.Public | BindingFlags.Instance).Skip(1).First();
            object other = new InstanceMethod(method2, dummyOwner);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SutDoesNotEqualOtherObjectWithDifferentOwner()
        {
            // Arrange
            var dummyMethod = typeof(object).GetMethods(BindingFlags.Public | BindingFlags.Instance).First();
            var owner1 = new object();
            var sut = new InstanceMethod(dummyMethod, owner1);

            var owner2 = new object();
            object other = new InstanceMethod(dummyMethod, owner2);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SutDoesNotEqualOtherSutWithDifferentOwner()
        {
            // Arrange
            var dummyMethod = typeof(object).GetMethods(BindingFlags.Public | BindingFlags.Instance).First();
            var owner1 = new object();
            var sut = new InstanceMethod(dummyMethod, owner1);

            var owner2 = new object();
            var other = new InstanceMethod(dummyMethod, owner2);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SutEqualsOtherObjectWithSameValues()
        {
            // Arrange
            var method = typeof(object).GetMethods(BindingFlags.Public | BindingFlags.Instance).First();
            var owner = new object();
            var sut = new InstanceMethod(method, owner);
            object other = new InstanceMethod(method, owner);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.True(result);
        }

        [Fact]
        public void SutEqualsOtherSutWithSameValues()
        {
            // Arrange
            var method = typeof(object).GetMethods(BindingFlags.Public | BindingFlags.Instance).First();
            var owner = new object();
            var sut = new InstanceMethod(method, owner);
            var other = new InstanceMethod(method, owner);
            // Act
            var result = sut.Equals(other);
            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GetHashCodeReturnsCorrectResult()
        {
            // Arrange
            var method = typeof(object).GetMethods(BindingFlags.Public | BindingFlags.Instance).First();
            var owner = new object();
            var sut = new InstanceMethod(method, owner);
            // Act
            var result = sut.GetHashCode();
            // Assert
            var expectedHasCode = method.GetHashCode() ^ owner.GetHashCode();
            Assert.Equal(expectedHasCode, result);
        }
    }
}
