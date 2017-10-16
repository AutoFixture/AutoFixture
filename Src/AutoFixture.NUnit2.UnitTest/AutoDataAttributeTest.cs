using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture.NUnit2.Addins;
using NUnit.Framework;
using TestTypeFoundation;

namespace AutoFixture.NUnit2.UnitTest
{
    [TestFixture]
    public class AutoDataAttributeTest
    {
        [Test]
        public void SutIsDataAttribute()
        {
            // Fixture setup
            // Exercise system
            var sut = new AutoDataAttribute();
            // Verify outcome
            Assert.IsInstanceOf<DataAttribute>(sut);
            // Teardown
        }

        [Test]
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
        
        [Test]
        public void InitializedWithFixtureFactoryConstrucorHasCorrectFixture()
        {
            // Fixture setup
            var fixture = new Fixture();
            
            // Exercise system
            var sut = new DerivedAutoDataAttribute(() => fixture);
            
            // Verify outcome
#pragma warning disable 618
            Assert.AreSame(fixture, sut.Fixture);
#pragma warning restore 618
            // Teardown
        }

        [Test]
        public void InitializeWithNullFixtureThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
#pragma warning disable 612
            Assert.Throws<ArgumentNullException>(() =>
                new DerivedAutoDataAttribute((IFixture)null));
#pragma warning restore 612
            // Teardown
        }
        
        [Test]
        public void InitializeWithNullFixtureFactoryThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new DerivedAutoDataAttribute((Func<IFixture>) null));
            // Teardown
        }

        [Test]
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

        [Test]
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
            Assert.AreEqual(expectedComposer, result);
            // Teardown
        }

        [Test]
        [Obsolete]
        public void InitializeWithNullTypeThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
#pragma warning disable 618
                new AutoDataAttribute((Type)null));
#pragma warning restore 618
            // Teardown
        }

        [Test]
        [Obsolete]
        public void InitializeWithNonComposerTypeThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() =>
#pragma warning disable 618
                new AutoDataAttribute(typeof(object)));
#pragma warning restore 618
            // Teardown
        }

        [Test]
        [Obsolete]
        public void InitializeWithComposerTypeWithoutDefaultConstructorThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() =>
#pragma warning disable 618
                new AutoDataAttribute(typeof(ComposerWithoutADefaultConstructor)));
#pragma warning restore 618
            // Teardown
        }

        [Test]
        [Obsolete]
        public void InitializedWithCorrectComposerTypeHasCorrectComposer()
        {
            // Fixture setup
            var composerType = typeof(DelegatingFixture);
#pragma warning disable 618
            var sut = new AutoDataAttribute(composerType);
#pragma warning restore 618
            // Exercise system
            var result = sut.Fixture;
            // Verify outcome
            Assert.IsAssignableFrom(composerType, result);
            // Teardown
        }

        [Test]
        [Obsolete]
        public void FixtureTypeIsCorrect()
        {
            // Fixture setup
            var composerType = typeof(DelegatingFixture);
#pragma warning disable 618
            var sut = new AutoDataAttribute(composerType);
#pragma warning restore 618
            // Exercise system
            var result = sut.FixtureType;
            // Verify outcome
            Assert.AreEqual(composerType, result);
            // Teardown
        }

        [Test]
        public void GetArgumentsWithNullMethodThrows()
        {
            // Fixture setup
            var sut = new AutoDataAttribute();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetData(null));
            // Teardown
        }

        [Test]
        public void GetArgumentsReturnsCorrectResult()
        {
            // Fixture setup
            var method = typeof(TypeWithOverloadedMembers).GetMethod("DoSomething", new[] { typeof(object) });
            var parameters = method.GetParameters();

            var expectedResult = new object();
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) =>
                    {
                        Assert.AreEqual(parameters.Single(), r);
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

        [TestCase("CreateWithFrozenAndFavorArrays")]
        [TestCase("CreateWithFavorArraysAndFrozen")]
        [TestCase("CreateWithFrozenAndFavorEnumerables")]
        [TestCase("CreateWithFavorEnumerablesAndFrozen")]
        [TestCase("CreateWithFrozenAndFavorLists")]
        [TestCase("CreateWithFavorListsAndFrozen")]
        [TestCase("CreateWithFrozenAndGreedy")]
        [TestCase("CreateWithGreedyAndFrozen")]
        [TestCase("CreateWithFrozenAndModest")]
        [TestCase("CreateWithModestAndFrozen")]
        [TestCase("CreateWithFrozenAndNoAutoProperties")]
        [TestCase("CreateWithNoAutoPropertiesAndFrozen")]
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
            
            public class Customization: ICustomization
            {
                public void Customize(IFixture fixture)
                {
                }
            }
        }

        [Test]
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
