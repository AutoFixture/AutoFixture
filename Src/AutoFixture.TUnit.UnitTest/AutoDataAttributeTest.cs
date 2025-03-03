using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Kernel;
using AutoFixture.TUnit.UnitTest.TestTypes;
using TestTypeFoundation;

namespace AutoFixture.TUnit.UnitTest
{
    public class AutoDataAttributeTest
    {
        [Test]
        public void SutIsDataAttribute()
        {
            // Arrange & Act
            var sut = new AutoDataAttribute();

            // Assert
            Assert.That(sut).IsAssignableFrom<NonTypedDataSourceGeneratorAttribute>();
        }

        [Test]
        public void InitializedWithDefaultConstructorHasCorrectFixture()
        {
            // Arrange
            var sut = new AutoDataAttribute();

            // Act
            var result = sut.FixtureFactory();

            // Assert
            Assert.That(result).IsAssignableFrom<Fixture>();
        }

        [Test]
        public void InitializedWithFixtureFactoryConstructorHasCorrectFixture()
        {
            // Arrange
            var fixture = new Fixture();

            // Act
            var sut = new DerivedAutoDataAttribute(() => fixture);

            // Assert
            Assert.That(sut.FixtureFactory()).IsSameReferenceAs(fixture);
        }

        [Test]
        public void InitializeWithNullFixtureFactoryThrows()
        {
            // Arrange
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new DerivedAutoDataAttribute(null));
        }

        [Test]
        public void DoesntActivateFixtureImmediately()
        {
            // Arrange
            var wasInvoked = false;

            // Act
            _ = new DerivedAutoDataAttribute(() =>
            {
                wasInvoked = true;
                return null;
            });

            // Assert
            Assert.That(wasInvoked).IsFalse();
        }

        [Test]
        public async Task GetDataWithNullMethodThrows()
        {
            // Arrange
            var sut = new AutoDataAttribute();

            // Act & assert
            await Assert.ThrowsAsync<ArgumentNullException>(
                () => sut.GetData(null!, new DisposalTracker()).AsTask());
        }

        [Test]
        public async Task GetDataReturnsCorrectResult()
        {
            // Arrange
            var method = typeof(TypeWithOverloadedMembers)
                .GetMethod("DoSomething", [typeof(object)]);
            var parameters = method!.GetParameters();
            var expectedResult = new object();

            object actualParameter = null;
            ISpecimenContext actualContext = null;
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) =>
                {
                    actualParameter = r;
                    actualContext = c;
                    return expectedResult;
                }
            };
            var composer = new DelegatingFixture { OnCreate = builder.OnCreate };
            var sut = new DerivedAutoDataAttribute(() => composer);

            // Act
            var result = (await sut.GetData(method, new DisposalTracker()))
                .Select(x => x.GetData()).ToArray();

            // Assert
            Assert.That(actualContext).IsNotNull();
            Assert.That(parameters).HasSingleItem();
            Assert.That(actualParameter).IsEqualTo(parameters[0]);
            Assert.That(result.Single()).IsEquivalentTo(new[] { expectedResult });
        }

        [Test]
        [Arguments("CreateWithFrozenAndFavorArrays")]
        [Arguments("CreateWithFavorArraysAndFrozen")]
        [Arguments("CreateWithFrozenAndFavorEnumerables")]
        [Arguments("CreateWithFavorEnumerablesAndFrozen")]
        [Arguments("CreateWithFrozenAndFavorLists")]
        [Arguments("CreateWithFavorListsAndFrozen")]
        [Arguments("CreateWithFrozenAndGreedy")]
        [Arguments("CreateWithGreedyAndFrozen")]
        [Arguments("CreateWithFrozenAndModest")]
        [Arguments("CreateWithModestAndFrozen")]
        [Arguments("CreateWithFrozenAndNoAutoProperties")]
        [Arguments("CreateWithNoAutoPropertiesAndFrozen")]
        public async Task GetDataOrdersCustomizationAttributes(string methodName)
        {
            // Arrange
            var method = typeof(TypeWithCustomizationAttributes)
                .GetMethod(methodName, [typeof(ConcreteType)]);
            var customizationLog = new List<ICustomization>();
            var fixture = new DelegatingFixture
            {
                OnCustomize = c => customizationLog.Add(c)
            };
            var sut = new DerivedAutoDataAttribute(() => fixture);

            // Act
            _ = (await sut.GetData(method!, new DisposalTracker()))
                .Select(x => x.GetData()).ToArray();

            // Assert
            Assert.That(customizationLog[0]).IsAssignableFrom<CompositeCustomization>();
            
            var composite = (CompositeCustomization) customizationLog[0];
            
            Assert.That(composite.Customizations.First()).IsNotTypeOf<FreezeOnMatchCustomization>();
            Assert.That(composite.Customizations.Last()).IsTypeOf<FreezeOnMatchCustomization>();
        }

        [Test]
        public async Task ShouldRecognizeAttributesImplementingIParameterCustomizationSource()
        {
            // Arrange
            var method = typeof(TypeWithIParameterCustomizationSourceUsage)
                .GetMethod(nameof(TypeWithIParameterCustomizationSourceUsage.DecoratedMethod));

            var customizationLog = new List<ICustomization>();
            var fixture = new DelegatingFixture
            {
                OnCustomize = c => customizationLog.Add(c)
            };
            var sut = new DerivedAutoDataAttribute(() => fixture);

            // Act
            _ = (await sut.GetData(method!, new DisposalTracker()))
                .Select(x => x.GetData()).ToArray();

            // Assert
            Assert.That(customizationLog[0]).IsTypeOf<TypeWithIParameterCustomizationSourceUsage.Customization>();
        }
    }
}