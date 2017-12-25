using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.AutoMoq.UnitTest
{
    public class FixtureIntegrationWithAutoMoqCustomizationTest
    {
        [Fact]
        public void FixtureAutoMocksInterface()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            // Act
            var result = fixture.Create<IInterface>();
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void FixtureAutoMocksAbstractType()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            // Act
            var result = fixture.Create<AbstractType>();
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void FixtureAutoMocksAbstractTypeWithNonDefaultConstructor()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            // Act
            var result = fixture.Create<AbstractTypeWithNonDefaultConstructor<int>>();
            // Assert
            Assert.NotEqual(0, result.Property);
        }

        [Fact]
        public void FixtureCanCreateMock()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            // Act
            var result = fixture.Create<Mock<AbstractType>>();
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void FixtureCanFreezeMock()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var expected = new object();

            fixture.Freeze<Mock<IInterface>>()
                .Setup(a => a.MakeIt(It.IsAny<object>()))
                .Returns(expected);
            // Act
            var result = fixture.Create<IInterface>();
            // Assert
            var dummy = new object();
            Assert.Equal(expected, result.MakeIt(dummy));
        }

        [Fact]
        public void FixtureCanCreateList()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            // Act
            var result = fixture.Create<IList<ConcreteType>>();
            // Assert
            Assert.True(result.Any());
        }

        [Fact]
        public void FixtureCanCreateMockOfAction()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            // Act
            var actual = fixture.Create<Mock<Action<string>>>();
            // Assert
            Assert.NotNull(actual);
        }

        [Fact]
        public void FixtureCanCreateUsableMockOfFunc()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var expected = fixture.Create<Version>();
            var mockOfFunc = fixture.Create<Mock<Func<int, Version>>>();
            mockOfFunc.Setup(f => f(42)).Returns(expected);

            // Act
            var actual = mockOfFunc.Object(42);
            
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FixtureCanFreezeUsableMockOfFunc()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var expected = fixture.Create<Uri>();
            var mockOfFunc = fixture.Freeze<Mock<Func<Guid, decimal, Uri>>>();
            mockOfFunc
                .Setup(f => f(It.IsAny<Guid>(), 1337m))
                .Returns(expected);

            // Act
            var actual = mockOfFunc.Object(Guid.NewGuid(), 1337m);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FixtureCanCreateUsableMockOfCustomDelegate()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var expected = fixture.Create<string>();
            var mockOfDelegate = fixture.Create<Mock<DBaz>>();
            mockOfDelegate.Setup(f => f(13, 37)).Returns(expected);

            // Act
            var actual = mockOfDelegate.Object(13, 37);

            // Assert
            Assert.Equal(expected, actual);
        }

        public interface IFoo
        {
            IBar Bar { get; set; }
        }

        public interface IBar
        {
        }

        public delegate string DBaz(short s, byte b);
    }
}
