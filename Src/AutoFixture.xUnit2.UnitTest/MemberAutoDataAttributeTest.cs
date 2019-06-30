using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;
using Xunit.Sdk;

namespace AutoFixture.Xunit2.UnitTest
{
    public class MemberAutoDataAttributeTest
    {
        [Fact]
        public void SutIsCompositeDataAttribute()
        {
            // Arrange
            // Act
            var sut = new MemberAutoDataAttribute(Guid.NewGuid().ToString());
            // Assert
            Assert.IsAssignableFrom<CompositeDataAttribute>(sut);
        }

        [Theory]
        [MemberData(nameof(GetAttributes))]
        public void AttributesAreInCorrectOrder(MemberAutoDataAttribute sut)
        {
            // Arrange
            var expected = new[] { typeof(MemberDataAttribute), typeof(AutoDataAttribute) };
            // Act
            IEnumerable<DataAttribute> result = sut.Attributes;
            // Assert
            Assert.True(result.Select(d => d.GetType()).SequenceEqual(expected));
        }

        [Fact]
        public void AttributesContainsDefaultAttributeWhenConstructedWithDefaultConstructor()
        {
            // Arrange
            var sut = new MemberAutoDataAttribute(Guid.NewGuid().ToString());
            // Act
            var result = sut.Attributes;
            // Assert
            Assert.Contains(result, a => a is AutoDataAttribute);
        }

        [Fact]
        public void AttributesContainsAttributeWhenConstructedWithExplicitAutoDataAttribute()
        {
            // Arrange
            var autoDataAttribute = new AutoDataAttribute();
            var sut = new DerivedMemberAutoDataAttribute(autoDataAttribute, Guid.NewGuid().ToString());
            // Act
            var result = sut.Attributes;
            // Assert
            Assert.Contains(autoDataAttribute, result);
        }

        [Theory]
        [MemberData(nameof(GetAttributesWithMemberName))]
        public void AttributesContainsMemberDataAttributeWithRightMemberName(MemberAutoDataAttribute sut, string expectedMemberName)
        {
            // Act
            var result = sut.Attributes;
            // Assert
            var memberDataAttribute = result.OfType<MemberDataAttribute>().SingleOrDefault();
            Assert.NotNull(memberDataAttribute);
            Assert.Equal(expectedMemberName, memberDataAttribute.MemberName);
        }

        [Theory]
        [MemberData(nameof(GetAttributesWithParameters))]
        public void AttributesContainsMemberDataAttributeWithRightParameters(MemberAutoDataAttribute sut, object[] expectedParameters)
        {
            // Act
            var result = sut.Attributes;
            // Assert
            var memberDataAttribute = result.OfType<MemberDataAttribute>().SingleOrDefault();
            Assert.NotNull(memberDataAttribute);
            Assert.Equal(expectedParameters, memberDataAttribute.Parameters);
        }

        [Theory]
        [MemberData(nameof(GetAttributesWithMemberName))]
        public void MemberNameIsCorrect(MemberAutoDataAttribute sut, string expectedMemberName)
        {
            // Arrange
            var expected = expectedMemberName;
            // Act
            var result = sut.MemberName;
            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(GetAttributesWithParameters))]
        public void ParametersAreCorrect(MemberAutoDataAttribute sut, object[] expectedParameters)
        {
            // Arrange
            var expected = expectedParameters;
            // Act
            var result = sut.Parameters;
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void AutoDataAttributeIsCorrectWhenConstructedWithDefaultConstructor()
        {
            // Arrange
            var sut = new MemberAutoDataAttribute(Guid.NewGuid().ToString());
            // Act
            var result = sut.AutoDataAttribute;
            // Assert
            Assert.IsType<AutoDataAttribute>(result);
        }

        [Fact]
        public void AutoDataAttributeIsCorrectWhenConstructedWithExplicitAutoDataAttribute()
        {
            // Arrange
            var expected = new AutoDataAttribute();
            var sut = new DerivedMemberAutoDataAttribute(expected, Guid.NewGuid().ToString());
            // Act
            var result = sut.AutoDataAttribute;
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DoesntActivateFixtureImmediately()
        {
            // Arrange
            bool wasInvoked = false;
            var autoData = new DerivedAutoDataAttribute(() =>
            {
                wasInvoked = true;
                return null;
            });

            // Act
            var sut = new DerivedMemberAutoDataAttribute(autoData, Guid.NewGuid().ToString());

            // Assert
            Assert.False(wasInvoked);
        }

        [Fact]
        public void PreDiscoveryShouldBeDisabled()
        {
            // Arrange
            var expectedDiscovererType = typeof(NoPreDiscoveryDataDiscoverer).GetTypeInfo();
            var discovererAttr = typeof(MemberAutoDataAttribute).GetTypeInfo()
                .CustomAttributes
                .Single(x => x.AttributeType == typeof(DataDiscovererAttribute));

            var expectedType = expectedDiscovererType.FullName;
            var expectedAssembly = expectedDiscovererType.Assembly.GetName().Name;

            // Act
            var actualType = (string)discovererAttr.ConstructorArguments[0].Value;
            var actualAssembly = (string)discovererAttr.ConstructorArguments[1].Value;

            // Assert
            Assert.Equal(expectedType, actualType);
            Assert.Equal(expectedAssembly, actualAssembly);
        }

        private class DerivedMemberAutoDataAttribute : MemberAutoDataAttribute
        {
            public DerivedMemberAutoDataAttribute(
                DataAttribute autoDataAttribute,
                string memberName,
                params object[] parameters)
                : base(autoDataAttribute, memberName, parameters)
            {
            }
        }

        private class DerivedAutoDataAttribute : AutoDataAttribute
        {
            public DerivedAutoDataAttribute(Func<IFixture> fixtureFactory)
                : base(fixtureFactory)
            {
            }
        }

        public static IEnumerable<object[]> GetAttributes()
        {
            var memberName = Guid.NewGuid().ToString();
            yield return new object[] { new MemberAutoDataAttribute(memberName) };
            yield return new object[] { new DerivedMemberAutoDataAttribute(new AutoDataAttribute(), memberName) };
        }

        public static IEnumerable<object[]> GetAttributesWithMemberName()
        {
            var memberName = Guid.NewGuid().ToString();
            yield return new object[] { new MemberAutoDataAttribute(memberName), memberName };
            yield return new object[] { new DerivedMemberAutoDataAttribute(new AutoDataAttribute(), memberName), memberName };
        }

        public static IEnumerable<object[]> GetAttributesWithParameters()
        {
            var memberName = Guid.NewGuid().ToString();
            var parameters = new[] { new object(), new object(), new object() };
            yield return new object[] { new MemberAutoDataAttribute(memberName), Enumerable.Empty<object>() };
            yield return new object[] { new DerivedMemberAutoDataAttribute(new AutoDataAttribute(), memberName), Enumerable.Empty<object>() };
            yield return new object[] { new MemberAutoDataAttribute(memberName, parameters), parameters };
            yield return new object[] { new DerivedMemberAutoDataAttribute(new AutoDataAttribute(), memberName, parameters), parameters };
        }
    }
}