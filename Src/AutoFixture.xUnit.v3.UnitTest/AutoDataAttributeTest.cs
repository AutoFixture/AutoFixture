using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Kernel;
using AutoFixture.Xunit.v3.UnitTest.TestTypes;
using TestTypeFoundation;
using Xunit;
using Xunit.Sdk;
using Xunit.v3;

namespace AutoFixture.Xunit.v3.UnitTest
{
    public class AutoDataAttributeTest
    {
        [Fact]
        public void SutIsDataAttribute()
        {
            // Arrange & Act
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
            var result = sut.FixtureFactory();
#pragma warning restore 618

            // Assert
            Assert.IsAssignableFrom<Fixture>(result);
        }

        [Fact]
        public void InitializedWithFixtureFactoryConstructorHasCorrectFixture()
        {
            // Arrange
            var fixture = new Fixture();

            // Act
            var sut = new DerivedAutoDataAttribute(() => fixture);

            // Assert
#pragma warning disable 618
            Assert.Same(fixture, sut.FixtureFactory());
#pragma warning restore 618
        }

        [Fact]
        public void InitializeWithNullFixtureFactoryThrows()
        {
            // Arrange
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                                                     new DerivedAutoDataAttribute(null));
        }

        [Fact]
        public void DoesntActivateFixtureImmediately()
        {
            // Arrange
            var wasInvoked = false;

            IFixture FixtureFactory()
            {
                wasInvoked = true;
                return null;
            }

            // Act
            _ = new DerivedAutoDataAttribute(FixtureFactory);

            // Assert
            Assert.False(wasInvoked);
        }

        [Fact]
        public void GetDataWithNullMethodThrows()
        {
            // Arrange
            var sut = new AutoDataAttribute();

            // Act & assert
            Assert.Throws<ArgumentNullException>(() => sut.GetData(null, null));
        }

        [Fact]
        public void GetDataReturnsCorrectResult()
        {
            // Arrange
            var method = typeof(TypeWithOverloadedMembers)
                .GetMethod("DoSomething", new[] { typeof(object) });
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
            var disposalTracker = new DisposalTracker();
            var sut = new DerivedAutoDataAttribute(() => composer);

            // Act
            var result = sut.GetData(method, disposalTracker);

            // Assert
            Assert.NotNull(actualContext);
            Assert.Single(parameters);
            Assert.Equal(parameters[0], actualParameter);
            Assert.Equal(new[] { expectedResult }, result.Result);
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
            var method = typeof(TypeWithCustomizationAttributes)
                .GetMethod(methodName, new[] { typeof(ConcreteType) });
            var customizationLog = new List<ICustomization>();
            var fixture = new DelegatingFixture();
            fixture.OnCustomize = c =>
                                  {
                                      customizationLog.Add(c);
                                      return fixture;
                                  };
            var disposalTracker = new DisposalTracker();
            var sut = new DerivedAutoDataAttribute(() => fixture);

            // Act
            var data = sut.GetData(method, disposalTracker);

            // Assert
            var composite = Assert.IsAssignableFrom<CompositeCustomization>(customizationLog[0]);
            Assert.IsNotType<FreezeOnMatchCustomization>(composite.Customizations.First());
            Assert.IsType<FreezeOnMatchCustomization>(composite.Customizations.Last());
        }

        [Fact]
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
            var disposalTracker = new DisposalTracker();
            var sut = new DerivedAutoDataAttribute(() => fixture);

            // Act
            sut.GetData(method, disposalTracker);

            // Assert
            Assert.IsType<TypeWithIParameterCustomizationSourceUsage.Customization>(customizationLog[0]);
        }

        //[Fact]
        //public void PreDiscoveryShouldBeDisabled()
        //{
        //    // Arrange
        //    var expectedDiscovererType = typeof(NoPreDiscoveryDataDiscoverer).GetTypeInfo();
        //    var discovererAttr = typeof(AutoDataAttribute).GetTypeInfo()
        //                                                  .CustomAttributes
        //                                                  .Single(x => x.AttributeType == typeof(DataDiscovererAttribute));

        //    var expectedType = expectedDiscovererType.FullName;
        //    var expectedAssembly = expectedDiscovererType.Assembly.GetName().Name;

        //    // Act
        //    var actualType = (string)discovererAttr.ConstructorArguments[0].Value;
        //    var actualAssembly = (string)discovererAttr.ConstructorArguments[1].Value;

        //    // Assert
        //    Assert.Equal(expectedType, actualType);
        //    Assert.Equal(expectedAssembly, actualAssembly);
        //}
    }
}