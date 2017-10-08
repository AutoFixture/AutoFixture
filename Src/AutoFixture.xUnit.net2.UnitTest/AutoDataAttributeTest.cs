using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Sdk;

namespace Ploeh.AutoFixture.Xunit2.UnitTest
{
    public class AutoDataAttributeTest
    {
        [Fact]
        public void SutIsDataAttribute()
        {
            // Fixture setup
            // Exercise system
            var sut = new AutoDataAttribute();
            // Verify outcome
            Assert.IsAssignableFrom<DataAttribute>(sut);
            // Teardown
        }

        [Fact]
        public void InitializedWithDefaultConstructorHasCorrectFixture()
        {
            // Fixture setup
            var sut = new AutoDataAttribute();
            // Exercise system
#pragma warning disable 618
            IFixture result = sut.Fixture;
#pragma warning restore 618
            // Verify outcome
            Assert.IsAssignableFrom<Fixture>(result);
            // Teardown
        }
        
        [Fact]
        public void InitializedWithFixtureFactoryConstrucorHasCorrectFixture()
        {
            // Fixture setup
            var fixture = new Fixture();
            
            // Exercise system
            var sut = new DerivedAutoDataAttribute(() => fixture);
            
            // Verify outcome
#pragma warning disable 618
            Assert.Same(fixture, sut.Fixture);
#pragma warning restore 618
            // Teardown
        }

        [Fact]
        public void InitializeWithNullFixtureThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
#pragma warning disable 612
                new DerivedAutoDataAttribute((IFixture)null));
#pragma warning restore 612
            // Teardown
        }
        
        [Fact]
        public void InitializeWithNullFixtureFactoryThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new DerivedAutoDataAttribute((Func<IFixture>) null));
            // Teardown
        }
        
        [Fact]
        public void FixtureFactoryIsNotInvokedImmediately()
        {
            // Fixture setup
            bool wasInvoked = false;
            Func<IFixture> fixtureFactory = () =>
            {
                wasInvoked = true;
                return null;
            };

            // Exercise system
            var sut = new DerivedAutoDataAttribute(fixtureFactory);
            
            // Verify outcome
            Assert.False(wasInvoked);
            // Teardown
        }

        [Fact]
        public void InitializedWithComposerHasCorrectComposer()
        {
            // Fixture setup
            var expectedComposer = new DelegatingFixture();
#pragma warning disable 612
            var sut = new DerivedAutoDataAttribute(expectedComposer);
#pragma warning restore 612
            // Exercise system
#pragma warning disable 618
            var result = sut.Fixture;
#pragma warning restore 618
            // Verify outcome
            Assert.Equal(expectedComposer, result);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void InitializeWithNullTypeThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
#pragma warning disable 618
                new AutoDataAttribute((Type)null));
#pragma warning disable 618
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void InitializeWithNonComposerTypeThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() =>
                new AutoDataAttribute(typeof(object)));
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void InitializeWithComposerTypeWithoutDefaultConstructorThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() =>
                new AutoDataAttribute(typeof(ComposerWithoutADefaultConstructor)));
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void InitializedWithCorrectComposerTypeHasCorrectComposer()
        {
            // Fixture setup
            var composerType = typeof(DelegatingFixture);
            var sut = new AutoDataAttribute(composerType);
            // Exercise system
            var result = sut.Fixture;
            // Verify outcome
            Assert.IsAssignableFrom(composerType, result);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void FixtureTypeIsCorrect()
        {
            // Fixture setup
            var composerType = typeof(DelegatingFixture);
            var sut = new AutoDataAttribute(composerType);
            // Exercise system
            var result = sut.FixtureType;
            // Verify outcome
            Assert.Equal(composerType, result);
            // Teardown
        }

        [Fact]
        public void GetDataWithNullMethodThrows()
        {
            // Fixture setup
            var sut = new AutoDataAttribute();
            var dummyTypes = Type.EmptyTypes;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetData(null));
            // Teardown
        }

        [Fact]
        public void GetDataReturnsCorrectResult()
        {
            // Fixture setup
            var method = typeof(TypeWithOverloadedMembers).GetMethod("DoSomething", new[] { typeof(object) });
            var parameters = method.GetParameters();

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
            // Exercise system
            var result = sut.GetData(method);
            // Verify outcome
            Assert.True(new[] { expectedResult }.SequenceEqual(result.Single()));
            // Teardown
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
            // Fixture setup
            var method = typeof(TypeWithCustomizationAttributes).GetMethod(methodName, new[] { typeof(ConcreteType) });
            var customizationLog = new List<ICustomization>();
            var fixture = new DelegatingFixture();
            fixture.OnCustomize = c =>
            {
                customizationLog.Add(c);
                return fixture;
            };
            var sut = new DerivedAutoDataAttribute(() => fixture);
            // Exercise system
            sut.GetData(method);
            // Verify outcome
            Assert.False(customizationLog[0] is FreezeOnMatchCustomization);
            Assert.True(customizationLog[1] is FreezeOnMatchCustomization);
            // Teardown
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
        
        private class TypeWithIParameterCustomizationSourceUsage
        {
            public void DecoratedMethod([CustomizationSourceAttribute] int arg)
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
            // Fixture setup
            var method = typeof(TypeWithIParameterCustomizationSourceUsage)
                .GetMethod(nameof(TypeWithIParameterCustomizationSourceUsage.DecoratedMethod));
            
            var customizationLog = new List<ICustomization>();
            var fixture = new DelegatingFixture();
            fixture.OnCustomize = c =>
            {
                customizationLog.Add(c);
                return fixture;
            };
            var sut = new DerivedAutoDataAttribute(() => fixture);
            
            // Exercise system
            sut.GetData(method);
            // Verify outcome
            Assert.True(customizationLog[0] is TypeWithIParameterCustomizationSourceUsage.Customization);
            // Teardown
        }
    }
}