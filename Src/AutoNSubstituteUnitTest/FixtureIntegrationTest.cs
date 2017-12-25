using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Kernel;
using NSubstitute;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.AutoNSubstitute.UnitTest
{
    public class FixtureIntegrationTest
    {
        [Fact]
        public void FixtureAutoSubstitutesInterface()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            // Act
            var result = fixture.Create<IInterface>();
            // Assert
            Assert.IsAssignableFrom<IInterface>(result);
        }

        [Fact]
        public void FixtureAutoSubstitutesAbstractType()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            // Act
            var result = fixture.Create<AbstractType>();
            // Assert
            Assert.IsAssignableFrom<AbstractType>(result);
        }

        [Fact]
        public void FixtureCanPassValuesToAbstractGenericTypeWithNonDefaultConstructor()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            // Act
            var result = fixture.Create<AbstractTypeWithNonDefaultConstructor<int>>();
            // Assert
            Assert.NotEqual(0, result.Property);
        }

        [Fact]
        public void FixtureCanPassValuesToAbstractGenericTypeWithConstructorWithMultipleParameters()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            // Act
            var result = fixture.Create<AbstractTypeWithConstructorWithMultipleParameters<int, int>>();
            // Assert
            Assert.NotEqual(0, result.Property1);
            Assert.NotEqual(0, result.Property2);
        }

        [Fact]
        public void FixtureCanFreezeSubstitute()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            var dummy = new object();
            var substitute = fixture.Freeze<IInterface>();
            substitute.MakeIt(dummy).Returns(null);
            // Act
            var result = fixture.Create<IInterface>();
            result.MakeIt(dummy);
            // Assert
            result.Received().MakeIt(dummy);
        }

        [Fact]
        public void FixtureCanCreateList()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            // Act
            var result = fixture.Create<IList<ConcreteType>>();
            // Assert
            Assert.True(result.Any());
        }

        [Fact]
        public void FixtureCanCreateAbstractGenericTypeWithNonDefaultConstructor()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            // Act
            var result = fixture.Create<AbstractGenericType<object>>();
            // Assert
            Assert.IsAssignableFrom<AbstractGenericType<object>>(result);
        }

        [Fact]
        public void FixtureCanCreateAnonymousGuid()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            // Act
            var result = fixture.Create<Guid>();
            // Assert
            Assert.NotEqual(Guid.Empty, result);
        }

        [Fact]
        public void FixtureCanCreateAbstractGenericTypeWithConstructorWithMultipleParameters()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            // Act
            var result = fixture.Create<AbstractTypeWithConstructorWithMultipleParameters<int, int>>();
            // Assert
            Assert.IsAssignableFrom<AbstractTypeWithConstructorWithMultipleParameters<int, int>>(result);
        }

        [Theory]
        [InlineData(typeof(IEnumerable<object>))]
        [InlineData(typeof(ICollection<object>))]
        [InlineData(typeof(IList<object>))]
        public void FixtureDoesNotHijackCollectionInterfaces(Type collectionInterface)
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            var context = new SpecimenContext(fixture);
            // Act
            var result = context.Resolve(new SeededRequest(collectionInterface, null));
            // Assert
            Assert.NotEmpty((IEnumerable)result);
        }
    }
}
