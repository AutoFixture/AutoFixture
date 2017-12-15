using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.AutoFakeItEasy.UnitTest
{
    public class FixtureIntegrationTest
    {
        [Fact]
        public void FixtureAutoFakesInterface()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            // Act
            var result = fixture.Create<IInterface>();
            // Assert
            Assert.IsAssignableFrom<IInterface>(result);
        }

        [Fact]
        public void FixtureAutoFakesAbstractType()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            // Act
            var result = fixture.Create<AbstractType>();
            // Assert
            Assert.IsAssignableFrom<AbstractType>(result);
        }

        [Fact]
        public void FixtureCanPassValuesToAbstractGenericTypeWithNonDefaultConstructor()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            // Act
            var result = fixture.Create<AbstractTypeWithNonDefaultConstructor<int>>();
            // Assert
            Assert.NotEqual(0, result.Property);
        }

        [Fact]
        public void FixtureCanCreateFake()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            // Act
            var result = fixture.Create<Fake<AbstractType>>();
            // Assert
            Assert.IsAssignableFrom<Fake<AbstractType>>(result);
        }

        [Fact]
        public void FixtureCanFreezeFake()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            var dummy = new object();
            var fake = fixture.Freeze<Fake<IInterface>>();
            fake.CallsTo(x => x.MakeIt(dummy))
                .Returns(null);
            // Act
            var result = fixture.Create<IInterface>();
            result.MakeIt(dummy);
            // Assert
            A.CallTo(() => result.MakeIt(dummy)).MustHaveHappened();
        }

        [Fact]
        public void FixtureCanCreateList()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            // Act
            var result = fixture.Create<IList<ConcreteType>>();
            // Assert
            Assert.True(result.Any());
        }

        [Fact]
        public void FixtureCanCreateAbstractGenericTypeWithNonDefaultConstructor()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            // Act
            var result = fixture.Create<AbstractGenericType<object>>();
            // Assert
            Assert.IsAssignableFrom<AbstractGenericType<object>>(result);
        }

        [Fact]
        public void FixtureCanCreateAbstractGenericTypeWithConstructorWithMultipleParameters()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            // Act
            var result = fixture.Create<AbstractTypeWithConstructorWithMultipleParameters<int, int>>();
            // Assert
            Assert.IsAssignableFrom<AbstractTypeWithConstructorWithMultipleParameters<int, int>>(result);
        }

        [Fact]
        public void FixtureCanCreateAnonymousGuid()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
            // Act
            var result = fixture.Create<Guid>();
            // Assert
            Assert.NotEqual(Guid.Empty, result);
        }
    }
}