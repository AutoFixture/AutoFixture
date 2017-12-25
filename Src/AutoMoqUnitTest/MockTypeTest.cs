using System;
using System.Text;
using AutoFixture.AutoMoq.UnitTest.TestTypes;
using AutoFixture.Kernel;
using Moq;
using Xunit;

namespace AutoFixture.AutoMoq.UnitTest
{
    public class MockTypeTest
    {
        [Fact]
        public void ReturnsUsingFixtureThrowsWhenSetupIsNull()
        {
            // Arrange
            var fixture = new Mock<IFixture>();
            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => MockType.ReturnsUsingFixture<string, string>(null, fixture.Object));
        }

        [Fact]
        public void ReturnsUsingFixtureThrowsWhenFixtureIsNull()
        {
            // Arrange
            var mock = new Mock<IInterfaceWithProperty>();
            var setup = mock.Setup(x => x.Property);
            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => setup.ReturnsUsingFixture(null));
        }

        [Fact]
        public void ReturnsUsingFixtureSetsMockUp()
        {
            // Arrange
            var mock = new Mock<IInterfaceWithProperty>();
            var setup = mock.Setup(x => x.Property);
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            // Act
            setup.ReturnsUsingFixture(fixture);
            // Assert
            Assert.Equal(frozenString, mock.Object.Property);
        }

        [Fact]
        public void ReturnsUsingFixtureSetsMockUpLazily()
        {
            // Arrange
            var mock = new Mock<IInterfaceWithProperty>();
            var setup = mock.Setup(x => x.Property);
            var fixture = new Mock<ISpecimenBuilder>();
            // Act
            setup.ReturnsUsingFixture(fixture.Object);
            // Assert
            fixture.Verify(f => f.Create(typeof (string), It.IsAny<SpecimenContext>()), Times.Never());
            var result = mock.Object.Property;
            fixture.Verify(f => f.Create(typeof (string), It.IsAny<SpecimenContext>()), Times.Once());
        }

        [Fact]
        public void ReturnsUsingFixture_SetsMockUpWithNull_WhenContextReturnsNull_AndMemberIsReferenceType()
        {
            // Arrange
            var mock = new Mock<IInterfaceWithProperty>();
            var fixture = new Mock<ISpecimenBuilder>();
            fixture.Setup(f => f.Create(typeof (string), It.IsAny<ISpecimenContext>()))
                .Returns(null as string);
            // Act
            mock.Setup(x => x.Property).ReturnsUsingFixture(fixture.Object);
            // Assert
            Assert.Null(mock.Object.Property);
        }

        [Fact]
        public void ReturnsUsingFixture_SetsMockUpWithNull_WhenContextReturnsNull_AndMemberIsNullableValueType()
        {
            // Arrange
            var mock = new Mock<IInterfaceWithNullableValueTypeProperty>();
            var fixture = new Mock<ISpecimenBuilder>();
            fixture.Setup(f => f.Create(typeof (int?), It.IsAny<ISpecimenContext>()))
                .Returns(null as int?);
            // Act
            mock.Setup(x => x.Property).ReturnsUsingFixture(fixture.Object);
            // Assert
            Assert.Null(mock.Object.Property);
        }

        [Fact]
        public void ReturnsUsingFixture_Throws_WhenContextReturnsNull_AndMemberIsNonNullableValueType()
        {
            // Arrange
            var mock = new Mock<IInterfaceWithNonNullableValueTypeProperty>();
            var fixture = new Mock<ISpecimenBuilder>();
            fixture.Setup(f => f.Create(typeof (int), It.IsAny<ISpecimenContext>()))
                .Returns(null as object);
            // Act
            mock.Setup(x => x.Property).ReturnsUsingFixture(fixture.Object);
            // Assert
            var ex = Assert.Throws<InvalidOperationException>(() => mock.Object.Property);
            Assert.Equal("Tried to setup a member with a return type of System.Int32, but null was found instead.",
                ex.Message);
        }

        [Theory]
        [InlineData(typeof (OmitSpecimen))]
        [InlineData(typeof (NoSpecimen))]
        [InlineData(typeof (object))]
        [InlineData(typeof (StringBuilder))]
        public void ReturnsUsingFixture_Throws_WhenContextReturnsUnexpectedSpecimen(Type specimenType)
        {
            // Arrange
            var mock = new Mock<IInterfaceWithProperty>();
            var specimen = Activator.CreateInstance(specimenType);
            var fixture = new Mock<ISpecimenBuilder>();
            fixture.Setup(f => f.Create(typeof (string), It.IsAny<ISpecimenContext>()))
                .Returns(specimen);
            var expectedExceptionMessage =
                string.Format(
                    "Tried to setup a member with a return type of System.String, but an instance of {0} was found instead.",
                    specimenType.FullName);

            // Act
            mock.Setup(x => x.Property).ReturnsUsingFixture(fixture.Object);
            // Assert
            var ex = Assert.Throws<InvalidOperationException>(() => mock.Object.Property);
            Assert.Equal(expectedExceptionMessage, ex.Message);
        }
    }
}
