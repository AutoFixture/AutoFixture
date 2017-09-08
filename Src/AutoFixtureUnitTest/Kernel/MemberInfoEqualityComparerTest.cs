using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class MemberInfoEqualityComparerTest
    {
        [Fact]
        public void SutIsStronglyTypedEqualityComparer()
        {
            // Fixture setup
            // Exercise system
            var sut = new MemberInfoEqualityComparer();
            // Verify outcome
            Assert.IsAssignableFrom<IEqualityComparer<MemberInfo>>(sut);
            // Teardown
        }

        [Fact]
        public void SutIsWeaklyTypedEqualityComparer()
        {
            // Fixture setup
            // Exercise system
            var sut = new MemberInfoEqualityComparer();
            // Verify outcome
            Assert.IsAssignableFrom<IEqualityComparer>(sut);
            // Teardown
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
            // Fixture setup
            IEqualityComparer sut = new MemberInfoEqualityComparer();
            // Exercise system
            var result = sut.Equals(x, y);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
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
            // Fixture setup
            var sut = new MemberInfoEqualityComparer();
            // Exercise system
            var result = sut.Equals(x, y);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void EqualsOfDeclaredAndDerivedPropertyReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new MemberInfoEqualityComparer();
            var declaredProperty = typeof(AbstractType).GetProperty("Property1");
            var derivedProperty = typeof(ConcreteType).GetProperty("Property1");
            // Exercise system
            var result = sut.Equals(declaredProperty, derivedProperty);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void EqualsOfDerivedAndDeclaredPropertyReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new MemberInfoEqualityComparer();
            var declaredProperty = typeof(AbstractType).GetProperty("Property1");
            var derivedProperty = typeof(ConcreteType).GetProperty("Property1");
            // Exercise system
            var result = sut.Equals(derivedProperty, declaredProperty);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void EqualsOfDeclaredAndOverriddenPropertyReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new MemberInfoEqualityComparer();
            var declaredProperty = typeof(AbstractType).GetProperty("Property4");
            var overriddenProperty = typeof(ConcreteType).GetProperty("Property4");
            // Exercise system
            var result = sut.Equals(declaredProperty, overriddenProperty);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void EqualsOfOverriddenAndDeclaredPropertyReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new MemberInfoEqualityComparer();
            var declaredProperty = typeof(AbstractType).GetProperty("Property4");
            var overriddenProperty = typeof(ConcreteType).GetProperty("Property4");
            // Exercise system
            var result = sut.Equals(overriddenProperty, declaredProperty);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void EqualsOfPropertyAndTypeReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new MemberInfoEqualityComparer();
            var pi = typeof(ConcreteType).GetProperty("Property4");
            var t = typeof(object).GetTypeInfo();
            // Exercise system
            var actual = sut.Equals(pi, t);
            // Verify outcome
            Assert.False(actual);
            // Teardown
        }

        [Fact]
        public void WeaklyTypedGetHashCodeOfNullThrows()
        {
            // Fixture setup
            IEqualityComparer sut = new MemberInfoEqualityComparer();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetHashCode(null));
            // Teardown
        }

        [Theory]
        [InlineData("")]
        [InlineData("ploeh")]
        [InlineData(1)]
        [InlineData(98)]
        public void WeaklyTypedGetHashCodeReturnsCorrectResultForNonMemberInfos(object obj)
        {
            // Fixture setup
            IEqualityComparer sut = new MemberInfoEqualityComparer();
            // Exercise system
            var result = sut.GetHashCode(obj);
            // Verify outcome
            var expectedHashCode = obj.GetHashCode();
            Assert.Equal(expectedHashCode, result);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(DateTime))]
        public void WeaklyTypedGetHashCodeReturnsCorrectResultForTypes(Type t)
        {
            // Fixture setup
            IEqualityComparer sut = new MemberInfoEqualityComparer();
            // Exercise system
            var result = sut.GetHashCode(t);
            // Verify outcome
            var expectedHashCode = t.Name.GetHashCode();
            Assert.Equal(expectedHashCode, result);
            // Teardown
        }

        [Fact]
        public void WeaklyTypedGetHashCodeReturnsCorrectResultForPropertyInfo()
        {
            // Fixture setup
            IEqualityComparer sut = new MemberInfoEqualityComparer();
            var propertyInfo = typeof(PropertyHolder<object>).GetProperty("Property");
            // Exercise system
            var result = sut.GetHashCode(propertyInfo);
            // Verify outcome
            var expectedHashCode = propertyInfo.DeclaringType.GetHashCode() ^ propertyInfo.Name.GetHashCode();
            Assert.Equal(expectedHashCode, result);
            // Teardown
        }

        [Fact]
        public void StronglyTypedGetHashCodeWithNullSutThrows()
        {
            // Fixture setup
            var sut = new MemberInfoEqualityComparer();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetHashCode(null));
            // Teardown
        }
    }
}
