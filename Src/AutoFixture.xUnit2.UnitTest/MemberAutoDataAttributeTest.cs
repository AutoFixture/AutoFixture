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
            var memberName = Guid.NewGuid().ToString();

            // Act
            var sut = new MemberAutoDataAttribute(memberName);

            // Assert
            Assert.IsAssignableFrom<CompositeDataAttribute>(sut);
        }

        [Fact]
        public void AttributesContainsDefaultAttributeWhenConstructedWithDefaultConstructor()
        {
            // Arrange
            var memberName = Guid.NewGuid().ToString();
            var sut = new MemberAutoDataAttribute(memberName);

            // Act
            var result = sut.Attributes;

            // Assert
            Assert.Contains(result, a => a is AutoDataAttribute);
        }

        [Fact]
        public void AttributesAreInCorrectOrderWhenConstructedWithDefaultConstructor()
        {
            // Arrange
            var expected = new[] { typeof(MemberDataAttribute), typeof(AutoDataAttribute) };
            var memberName = Guid.NewGuid().ToString();
            var sut = new MemberAutoDataAttribute(memberName);

            // Act
            IEnumerable<DataAttribute> result = sut.Attributes;

            // Assert
            Assert.True(result.Select(d => d.GetType()).SequenceEqual(expected));
        }

        [Fact]
        public void AttributesContainsAttributeWhenConstructedWithExplicitAutoDataAttribute()
        {
            // Arrange
            var autoDataAttribute = new AutoDataAttribute();
            var memberName = Guid.NewGuid().ToString();
            var sut = new DerivedMemberAutoDataAttribute(autoDataAttribute, memberName);

            // Act
            var result = sut.Attributes;

            // Assert
            Assert.Contains(autoDataAttribute, result);
        }

        [Fact]
        public void AttributesAreInCorrectOrderWhenConstructedWithExplicitDataAttribute()
        {
            // Arrange
            var expected = new[] { typeof(MemberDataAttribute), typeof(AutoDataAttribute) };
            var memberName = Guid.NewGuid().ToString();
            var autoDataAttribute = new AutoDataAttribute();
            var sut = new DerivedMemberAutoDataAttribute(autoDataAttribute, memberName);

            // Act
            IEnumerable<DataAttribute> result = sut.Attributes;

            // Assert
            Assert.True(result.Select(d => d.GetType()).SequenceEqual(expected));
        }

        [Fact]
        public void AttributesContainsMemberDataAttributeWithRightMemberNameWhenConstructedWithDefaultCtor()
        {
            // Arrange
            var memberName = Guid.NewGuid().ToString();
            var sut = new MemberAutoDataAttribute(memberName);

            // Act
            var result = sut.Attributes;

            // Assert
            var memberDataAttribute = result.OfType<MemberDataAttribute>().SingleOrDefault();
            Assert.NotNull(memberDataAttribute);
            Assert.Equal(memberName, memberDataAttribute.MemberName);
        }

        [Fact]
        public void AttributesContainsMemberDataAttributeWithRightMemberNameWhenConstructedWithDataAttribute()
        {
            // Arrange
            var memberName = Guid.NewGuid().ToString();
            var dataAttribute = new AutoDataAttribute();
            var sut = new DerivedMemberAutoDataAttribute(dataAttribute, memberName);

            // Act
            var result = sut.Attributes;

            // Assert
            var memberDataAttribute = result.OfType<MemberDataAttribute>().SingleOrDefault();
            Assert.NotNull(memberDataAttribute);
            Assert.Equal(memberName, memberDataAttribute.MemberName);
        }

        [Fact]
        public void AttributesContainsMemberDataAttributeWithRightParametersWhenConstructedWithDefaultCtor()
        {
            // Act
            var memberName = Guid.NewGuid().ToString();
            var parameters = new object[] { 42, "42" };
            var sut = new MemberAutoDataAttribute(memberName, parameters);
            var result = sut.Attributes;

            // Assert
            var memberDataAttribute = result.OfType<MemberDataAttribute>().SingleOrDefault();
            Assert.NotNull(memberDataAttribute);
            Assert.Equal(parameters, memberDataAttribute.Parameters);
        }

        [Fact]
        public void AttributesContainsMemberDataAttributeWithRightParametersWhenConstructedWithDataAttribute()
        {
            // Act
            var memberName = Guid.NewGuid().ToString();
            var parameters = new object[] { 42, "42" };
            var dataAttribute = new AutoDataAttribute();
            var sut = new DerivedMemberAutoDataAttribute(dataAttribute, memberName, parameters);
            var result = sut.Attributes;

            // Assert
            var memberDataAttribute = result.OfType<MemberDataAttribute>().SingleOrDefault();
            Assert.NotNull(memberDataAttribute);
            Assert.Equal(parameters, memberDataAttribute.Parameters);
        }

        [Fact]
        public void MemberNameIsCorrectWhenConstructedWithDefaultConstructor()
        {
            // Arrange
            var memberName = Guid.NewGuid().ToString();
            var sut = new MemberAutoDataAttribute(memberName);

            // Act
            var result = sut.MemberName;

            // Assert
            Assert.Equal(memberName, result);
        }

        [Fact]
        public void MemberNameIsCorrectWhenConstructedWithDataAttribute()
        {
            // Arrange
            var memberName = Guid.NewGuid().ToString();
            var dataAttribute = new AutoDataAttribute();
            var sut = new DerivedMemberAutoDataAttribute(dataAttribute, memberName);

            // Act
            var result = sut.MemberName;

            // Assert
            Assert.Equal(memberName, result);
        }

        [Fact]
        public void ParametersAreCorrectWhenConstructedWithDefaultCtor()
        {
            // Arrange
            var memberName = Guid.NewGuid().ToString();
            var parameters = new object[] { 42, "42" };
            var sut = new MemberAutoDataAttribute(memberName, parameters);

            // Act
            var result = sut.Parameters;

            // Assert
            Assert.Equal(parameters, result);
        }

        [Fact]
        public void ParametersAreCorrectWhenConstructedWithDataAttribute()
        {
            // Arrange
            var memberName = Guid.NewGuid().ToString();
            var parameters = new object[] { 42, "42" };
            var dataAttribute = new AutoDataAttribute();
            var sut = new DerivedMemberAutoDataAttribute(dataAttribute, memberName, parameters);

            // Act
            var result = sut.Parameters;

            // Assert
            Assert.Equal(parameters, result);
        }

        [Fact]
        public void AutoDataAttributeIsCorrectWhenConstructedWithDefaultConstructor()
        {
            // Arrange
            var memberName = Guid.NewGuid().ToString();
            var sut = new MemberAutoDataAttribute(memberName);

            // Act
            var result = sut.AutoDataAttribute;

            // Assert
            Assert.IsType<AutoDataAttribute>(result);
        }

        [Fact]
        public void AutoDataAttributeIsCorrectWhenConstructedWithExplicitDataAttribute()
        {
            // Arrange
            var memberName = Guid.NewGuid().ToString();
            var expected = new AutoDataAttribute();
            var sut = new DerivedMemberAutoDataAttribute(expected, memberName);

            // Act
            var result = sut.AutoDataAttribute;

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DoesNotActivateFixtureImmediately()
        {
            // Arrange
            var memberName = Guid.NewGuid().ToString();
            bool wasInvoked = false;
            var autoData = new DerivedAutoDataAttribute(() =>
            {
                wasInvoked = true;
                return new Fixture();
            });

            // Act
            var sut = new DerivedMemberAutoDataAttribute(autoData, memberName);

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
    }
}