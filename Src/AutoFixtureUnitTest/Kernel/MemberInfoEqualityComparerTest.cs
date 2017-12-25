using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class MemberInfoEqualityComparerTest
    {
        [Fact]
        public void SutIsStronglyTypedEqualityComparer()
        {
            // Arrange
            // Act
            var sut = new MemberInfoEqualityComparer();
            // Assert
            Assert.IsAssignableFrom<IEqualityComparer<MemberInfo>>(sut);
        }

        [Fact]
        public void SutIsWeaklyTypedEqualityComparer()
        {
            // Arrange
            // Act
            var sut = new MemberInfoEqualityComparer();
            // Assert
            Assert.IsAssignableFrom<IEqualityComparer>(sut);
        }

        [Theory]
        [InlineData(null, "", false)]
        [InlineData("", null, false)]
        [InlineData("", "", false)]
        [InlineData(1, 1, false)]
        [InlineData(null, null, true)]
        [InlineData(null, typeof(object), false)]
        [InlineData(typeof(object), null, false)]
        [InlineData(typeof(object), typeof(object), true)]
        [InlineData(typeof(string), typeof(object), false)]
        [InlineData(typeof(string), typeof(string), true)]
        public void WeaklyTypedEqualsReturnsCorrectResult(object x, object y, bool expectedResult)
        {
            // Arrange
            IEqualityComparer sut = new MemberInfoEqualityComparer();
            // Act
            var result = sut.Equals(x, y);
            // Assert
            Assert.Equal(expectedResult, result);
        }

#pragma warning disable xUnit1010 // Value is not convertiable to the MemberInfo - it's wrong and test doesn't fail during execution.
        [Theory]
        [InlineData(null, null, true)]
        [InlineData(null, typeof(object), false)]
        [InlineData(typeof(object), null, false)]
        [InlineData(typeof(object), typeof(object), true)]
        [InlineData(typeof(string), typeof(object), false)]
        [InlineData(typeof(string), typeof(string), true)]
#pragma warning restore xUnit1010 // Value is not convertiable to the MemberInfo
        public void StronglyTypedEqualsReturnsCorrectResult(MemberInfo x, MemberInfo y, bool expectedResult)
        {
            // Arrange
            var sut = new MemberInfoEqualityComparer();
            // Act
            var result = sut.Equals(x, y);
            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void EqualsOfDeclaredAndDerivedPropertyReturnsCorrectResult()
        {
            // Arrange
            var sut = new MemberInfoEqualityComparer();
            var declaredProperty = typeof(AbstractType).GetProperty("Property1");
            var derivedProperty = typeof(ConcreteType).GetProperty("Property1");
            // Act
            var result = sut.Equals(declaredProperty, derivedProperty);
            // Assert
            Assert.True(result);
        }

        [Fact]
        public void EqualsOfDerivedAndDeclaredPropertyReturnsCorrectResult()
        {
            // Arrange
            var sut = new MemberInfoEqualityComparer();
            var declaredProperty = typeof(AbstractType).GetProperty("Property1");
            var derivedProperty = typeof(ConcreteType).GetProperty("Property1");
            // Act
            var result = sut.Equals(derivedProperty, declaredProperty);
            // Assert
            Assert.True(result);
        }

        [Fact]
        public void EqualsOfDeclaredAndOverriddenPropertyReturnsCorrectResult()
        {
            // Arrange
            var sut = new MemberInfoEqualityComparer();
            var declaredProperty = typeof(AbstractType).GetProperty("Property4");
            var overriddenProperty = typeof(ConcreteType).GetProperty("Property4");
            // Act
            var result = sut.Equals(declaredProperty, overriddenProperty);
            // Assert
            Assert.True(result);
        }

        [Fact]
        public void EqualsOfOverriddenAndDeclaredPropertyReturnsCorrectResult()
        {
            // Arrange
            var sut = new MemberInfoEqualityComparer();
            var declaredProperty = typeof(AbstractType).GetProperty("Property4");
            var overriddenProperty = typeof(ConcreteType).GetProperty("Property4");
            // Act
            var result = sut.Equals(overriddenProperty, declaredProperty);
            // Assert
            Assert.True(result);
        }

        [Fact]
        public void EqualsOfPropertyAndTypeReturnsCorrectResult()
        {
            // Arrange
            var sut = new MemberInfoEqualityComparer();
            var pi = typeof(ConcreteType).GetProperty("Property4");
            var t = typeof(object).GetTypeInfo();
            // Act
            var actual = sut.Equals(pi, t);
            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void WeaklyTypedGetHashCodeOfNullThrows()
        {
            // Arrange
            IEqualityComparer sut = new MemberInfoEqualityComparer();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetHashCode(null));
        }

        [Theory]
        [InlineData("")]
        [InlineData("ploeh")]
        [InlineData(1)]
        [InlineData(98)]
        public void WeaklyTypedGetHashCodeReturnsCorrectResultForNonMemberInfos(object obj)
        {
            // Arrange
            IEqualityComparer sut = new MemberInfoEqualityComparer();
            // Act
            var result = sut.GetHashCode(obj);
            // Assert
            var expectedHashCode = obj.GetHashCode();
            Assert.Equal(expectedHashCode, result);
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(DateTime))]
        public void WeaklyTypedGetHashCodeReturnsCorrectResultForTypes(Type t)
        {
            // Arrange
            IEqualityComparer sut = new MemberInfoEqualityComparer();
            // Act
            var result = sut.GetHashCode(t);
            // Assert
            var expectedHashCode = t.Name.GetHashCode();
            Assert.Equal(expectedHashCode, result);
        }

        [Fact]
        public void WeaklyTypedGetHashCodeReturnsCorrectResultForPropertyInfo()
        {
            // Arrange
            IEqualityComparer sut = new MemberInfoEqualityComparer();
            var propertyInfo = typeof(PropertyHolder<object>).GetProperty("Property");
            // Act
            var result = sut.GetHashCode(propertyInfo);
            // Assert
            var expectedHashCode = propertyInfo.DeclaringType.GetHashCode() ^ propertyInfo.Name.GetHashCode();
            Assert.Equal(expectedHashCode, result);
        }

        [Fact]
        public void StronglyTypedGetHashCodeWithNullShouldNotThrowAsExceptionIsNotExpectedThere()
        {
            // Arrange
            var sut = new MemberInfoEqualityComparer();
            // Act & assert
            Assert.Null(Record.Exception(() =>
                sut.GetHashCode(null)));
        }
    }
}
