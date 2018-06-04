using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace AutoFixture.Xunit.UnitTest
{
    public class AutoDataAttributeTest
    {
        [Fact]
        public void SutIsDataAttribute()
        {
            // Arrange
            // Act
            var sut = new AutoDataAttribute();
            // Assert
            Assert.IsAssignableFrom<DataAttribute>(sut);
        }

        [Fact]
        public void InitializedWithDefaultConstructorHasCorrectFixture()
        {
            // Arrange
            var sut = new AutoDataAttribute();
            // Act
#pragma warning disable 618
            IFixture result = sut.Fixture;
#pragma warning restore 618
            // Assert
            Assert.IsAssignableFrom<Fixture>(result);
        }

        [Fact]
        public void InitializedWithFixtureFactoryConstrucorHasCorrectFixture()
        {
            // Arrange
            var fixture = new Fixture();

            // Act
            var sut = new DerivedAutoDataAttribute(() => fixture);

            // Assert
#pragma warning disable 618
            Assert.Same(fixture, sut.Fixture);
#pragma warning restore 618
        }

        [Fact]
        public void InitializeWithNullFixtureThrows()
        {
            // Arrange
            // Act & assert
#pragma warning disable 612
            Assert.Throws<ArgumentNullException>(() =>
                new DerivedAutoDataAttribute((IFixture)null));
#pragma warning restore 612
        }

        [Fact]
        public void InitializeWithNullFixtureFactoryThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new DerivedAutoDataAttribute((Func<IFixture>)null));
        }

        [Fact]
        public void FixtureFactoryIsNotInvokedImmediately()
        {
            // Arrange
            bool wasInvoked = false;
            Func<IFixture> fixtureFactory = () =>
            {
                wasInvoked = true;
                return null;
            };

            // Act
            var sut = new DerivedAutoDataAttribute(fixtureFactory);

            // Assert
            Assert.False(wasInvoked);
        }

        [Fact]
        public void InitializedWithComposerHasCorrectComposer()
        {
            // Arrange
            var expectedComposer = new DelegatingFixture();
#pragma warning disable 612
            var sut = new DerivedAutoDataAttribute(expectedComposer);
#pragma warning restore 612
            // Act
#pragma warning disable 618
            var result = sut.Fixture;
#pragma warning restore 618
            // Assert
            Assert.Equal(expectedComposer, result);
        }

        [Fact]
        [Obsolete]
        public void InitializeWithNullTypeThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
#pragma warning disable 618
                new AutoDataAttribute((Type)null));
#pragma warning restore 618
        }

        [Fact]
        [Obsolete]
        public void InitializeWithNonComposerTypeThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentException>(() =>
#pragma warning disable 618
                new AutoDataAttribute(typeof(object)));
#pragma warning restore 618
        }

        [Fact]
        [Obsolete]
        public void InitializeWithComposerTypeWithoutDefaultConstructorThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentException>(() =>
#pragma warning disable 618
                new AutoDataAttribute(typeof(ComposerWithoutADefaultConstructor)));
#pragma warning restore 618
        }

        [Fact]
        [Obsolete]
        public void InitializedWithCorrectComposerTypeHasCorrectComposer()
        {
            // Arrange
            var composerType = typeof(DelegatingFixture);
#pragma warning disable 618
            var sut = new AutoDataAttribute(composerType);
#pragma warning restore 618
            // Act
            var result = sut.Fixture;
            // Assert
            Assert.IsAssignableFrom(composerType, result);
        }

        [Fact]
        [Obsolete]
        public void FixtureTypeIsCorrect()
        {
            // Arrange
            var composerType = typeof(DelegatingFixture);
#pragma warning disable 618
            var sut = new AutoDataAttribute(composerType);
#pragma warning restore 618
            // Act
            var result = sut.FixtureType;
            // Assert
            Assert.Equal(composerType, result);
        }

        [Fact]
        public void GetDataWithNullMethodThrows()
        {
            // Arrange
            var sut = new AutoDataAttribute();
            var dummyTypes = Type.EmptyTypes;
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetData(null, dummyTypes));
        }

        [Fact]
        public void GetDataReturnsCorrectResult()
        {
            // Arrange
            var method = typeof(TypeWithOverloadedMembers).GetMethod("DoSomething", new[] { typeof(object) });
            var parameters = method.GetParameters();
            var parameterTypes = (from pi in parameters
                                  select pi.ParameterType).ToArray();

            var expectedResult = new object();
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) =>
                    {
                        Assert.Equal(parameters.Single(), r);
                        Assert.NotNull(c);
                        return expectedResult;
                    }
            };
            var composer = new DelegatingFixture { OnCreate = builder.OnCreate };

            var sut = new DerivedAutoDataAttribute(() => composer);
            // Act
            var result = sut.GetData(method, parameterTypes);
            // Assert
            Assert.True(new[] { expectedResult }.SequenceEqual(result.Single()));
        }

        [Theory]
        [InlineData("CreateWithFrozenAndFavorArrays")]
        [InlineData("CreateWithFavorArraysAndFrozen")]
        [InlineData("CreateWithFrozenAndFavorEnumerables")]
        [InlineData("CreateWithFavorEnumerablesAndFrozen")]
        [InlineData("CreateWithFrozenAndFavorLists")]
        [InlineData("CreateWithFavorListsAndFrozen")]
        [InlineData("CreateWithFrozenAndGreedy")]
        [InlineData("CreateWithGreedyAndFrozen")]
        [InlineData("CreateWithFrozenAndModest")]
        [InlineData("CreateWithModestAndFrozen")]
        [InlineData("CreateWithFrozenAndNoAutoProperties")]
        [InlineData("CreateWithNoAutoPropertiesAndFrozen")]
        public void GetDataOrdersCustomizationAttributes(string methodName)
        {
            // Arrange
            var method = typeof(TypeWithCustomizationAttributes).GetMethod(methodName, new[] { typeof(ConcreteType) });

            var parameters = method.GetParameters();
            var parameterTypes = (from pi in parameters
                                  select pi.ParameterType).ToArray();

            var customizationLog = new List<ICustomization>();
            var fixture = new DelegatingFixture();
            fixture.OnCustomize = c =>
            {
                customizationLog.Add(c);
                return fixture;
            };
            var sut = new DerivedAutoDataAttribute(() => fixture);
            // Act
            sut.GetData(method, parameterTypes);
            // Assert
            Assert.False(customizationLog[0] is FreezeOnMatchCustomization);
            Assert.True(customizationLog[1] is FreezeOnMatchCustomization);
        }

        private class TypeWithIParameterCustomizationSourceUsage
        {
            public void DecoratedMethod([CustomizationSource] int arg)
            {
            }

            public class CustomizationSourceAttribute : Attribute, IParameterCustomizationSource
            {
                public ICustomization GetCustomization(ParameterInfo parameter)
                {
                    return new Customization();
                }
            }

            public class Customization : ICustomization
            {
                public void Customize(IFixture fixture)
                {
                }
            }
        }

        [Fact]
        public void ShouldRecognizeAttributesImplementingIParameterCustomizationSource()
        {
            // Arrange
            var method = typeof(TypeWithIParameterCustomizationSourceUsage)
                .GetMethod(nameof(TypeWithIParameterCustomizationSourceUsage.DecoratedMethod));

            var parameters = method.GetParameters();
            var parameterTypes = parameters.Select(p => p.ParameterType).ToArray();

            var customizationLog = new List<ICustomization>();
            var fixture = new DelegatingFixture();
            fixture.OnCustomize = c =>
            {
                customizationLog.Add(c);
                return fixture;
            };
            var sut = new DerivedAutoDataAttribute(() => fixture);

            // Act
            sut.GetData(method, parameterTypes);
            // Assert
            Assert.True(customizationLog[0] is TypeWithIParameterCustomizationSourceUsage.Customization);
        }

        private class DerivedAutoDataAttribute : AutoDataAttribute
        {
            [Obsolete]
            public DerivedAutoDataAttribute(IFixture fixture)
                : base(fixture)
            {
            }

            public DerivedAutoDataAttribute(Func<IFixture> fixtureFactory)
                : base(fixtureFactory)
            {
            }
        }
    }
}