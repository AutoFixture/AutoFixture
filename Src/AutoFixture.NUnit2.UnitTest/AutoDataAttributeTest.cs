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
            // Arrange
            // Act
            var sut = new AutoDataAttribute();
            // Assert
            Assert.IsInstanceOf<DataAttribute>(sut);
        }

        [Test]
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

        [Test]
        public void InitializedWithFixtureFactoryConstrucorHasCorrectFixture()
        {
            // Arrange
            var fixture = new Fixture();

            // Act
            var sut = new DerivedAutoDataAttribute(() => fixture);

            // Assert
#pragma warning disable 618
            Assert.AreSame(fixture, sut.Fixture);
#pragma warning restore 618
        }

        [Test]
        public void InitializeWithNullFixtureThrows()
        {
            // Arrange
            // Act & Assert
#pragma warning disable 612
            Assert.Throws<ArgumentNullException>(() =>
                new DerivedAutoDataAttribute((IFixture)null));
#pragma warning restore 612
        }

        [Test]
        public void InitializeWithNullFixtureFactoryThrows()
        {
            // Arrange
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new DerivedAutoDataAttribute((Func<IFixture>)null));
        }

        [Test]
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

        [Test]
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
            Assert.AreEqual(expectedComposer, result);
        }

        [Test]
        [Obsolete]
        public void InitializeWithNullTypeThrows()
        {
            // Arrange
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
#pragma warning disable 618
                new AutoDataAttribute((Type)null));
#pragma warning restore 618
        }

        [Test]
        [Obsolete]
        public void InitializeWithNonComposerTypeThrows()
        {
            // Arrange
            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
#pragma warning disable 618
                new AutoDataAttribute(typeof(object)));
#pragma warning restore 618
        }

        [Test]
        [Obsolete]
        public void InitializeWithComposerTypeWithoutDefaultConstructorThrows()
        {
            // Arrange
            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
#pragma warning disable 618
                new AutoDataAttribute(typeof(ComposerWithoutADefaultConstructor)));
#pragma warning restore 618
        }

        [Test]
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

        [Test]
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
            Assert.AreEqual(composerType, result);
        }

        [Test]
        public void GetArgumentsWithNullMethodThrows()
        {
            // Arrange
            var sut = new AutoDataAttribute();
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetData(null));
        }

        [Test]
        public void GetArgumentsReturnsCorrectResult()
        {
            // Arrange
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
            // Act
            var result = sut.GetData(method);
            // Assert
            Assert.True(new[] { expectedResult }.SequenceEqual(result.Single()));
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
            // Arrange
            var method = typeof(TypeWithCustomizationAttributes).GetMethod(methodName, new[] { typeof(ConcreteType) });
            var customizationLog = new List<ICustomization>();
            var fixture = new DelegatingFixture();
            fixture.OnCustomize = c =>
            {
                customizationLog.Add(c);
                return fixture;
            };
            var sut = new DerivedAutoDataAttribute(() => fixture);
            // Act
            sut.GetData(method);
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

        [Test]
        public void ShouldRecognizeAttributesImplementingIParameterCustomizationSource()
        {
            // Arrange
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

            // Act
            sut.GetData(method);
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
