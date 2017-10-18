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
            // Fixture setup
            var fixture = new Mock<IFixture>();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(
                () => MockType.ReturnsUsingFixture<string, string>(null, fixture.Object));
            // Teardown
        }

        [Fact]
        public void ReturnsUsingFixtureThrowsWhenFixtureIsNull()
        {
            // Fixture setup
            var mock = new Mock<IInterfaceWithProperty>();
            var setup = mock.Setup(x => x.Property);
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(
                () => setup.ReturnsUsingFixture(null));
            // Teardown
        }

        [Fact]
        public void ReturnsUsingFixtureSetsMockUp()
        {
            // Fixture setup
            var mock = new Mock<IInterfaceWithProperty>();
            var setup = mock.Setup(x => x.Property);
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            // Exercise system
            setup.ReturnsUsingFixture(fixture);
            // Verify outcome
            Assert.Equal(frozenString, mock.Object.Property);
            // Teardown
        }

        [Fact]
        public void ReturnsUsingFixtureSetsMockUpLazily()
        {
            // Fixture setup
            var mock = new Mock<IInterfaceWithProperty>();
            var setup = mock.Setup(x => x.Property);
            var fixture = new Mock<ISpecimenBuilder>();
            // Exercise system
            setup.ReturnsUsingFixture(fixture.Object);
            // Verify outcome
            fixture.Verify(f => f.Create(typeof (string), It.IsAny<SpecimenContext>()), Times.Never());
            var result = mock.Object.Property;
            fixture.Verify(f => f.Create(typeof (string), It.IsAny<SpecimenContext>()), Times.Once());
        }

        [Fact]
        public void ReturnsUsingFixture_SetsMockUpWithNull_WhenContextReturnsNull_AndMemberIsReferenceType()
        {
            // Fixture setup
            var mock = new Mock<IInterfaceWithProperty>();
            var fixture = new Mock<ISpecimenBuilder>();
            fixture.Setup(f => f.Create(typeof (string), It.IsAny<ISpecimenContext>()))
                .Returns(null as string);
            // Exercise system
            mock.Setup(x => x.Property).ReturnsUsingFixture(fixture.Object);
            // Verify outcome
            Assert.Null(mock.Object.Property);
        }

        [Fact]
        public void ReturnsUsingFixture_SetsMockUpWithNull_WhenContextReturnsNull_AndMemberIsNullableValueType()
        {
            // Fixture setup
            var mock = new Mock<IInterfaceWithNullableValueTypeProperty>();
            var fixture = new Mock<ISpecimenBuilder>();
            fixture.Setup(f => f.Create(typeof (int?), It.IsAny<ISpecimenContext>()))
                .Returns(null as int?);
            // Exercise system
            mock.Setup(x => x.Property).ReturnsUsingFixture(fixture.Object);
            // Verify outcome
            Assert.Null(mock.Object.Property);
        }

        [Fact]
        public void ReturnsUsingFixture_Throws_WhenContextReturnsNull_AndMemberIsNonNullableValueType()
        {
            // Fixture setup
            var mock = new Mock<IInterfaceWithNonNullableValueTypeProperty>();
            var fixture = new Mock<ISpecimenBuilder>();
            fixture.Setup(f => f.Create(typeof (int), It.IsAny<ISpecimenContext>()))
                .Returns(null as object);
            // Exercise system
            mock.Setup(x => x.Property).ReturnsUsingFixture(fixture.Object);
            // Verify outcome
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
            // Fixture setup
            var mock = new Mock<IInterfaceWithProperty>();
            var specimen = Activator.CreateInstance(specimenType);
            var fixture = new Mock<ISpecimenBuilder>();
            fixture.Setup(f => f.Create(typeof (string), It.IsAny<ISpecimenContext>()))
                .Returns(specimen);
            var expectedExceptionMessage =
                string.Format(
                    "Tried to setup a member with a return type of System.String, but an instance of {0} was found instead.",
                    specimenType.FullName);

            // Exercise system
            mock.Setup(x => x.Property).ReturnsUsingFixture(fixture.Object);
            // Verify outcome
            var ex = Assert.Throws<InvalidOperationException>(() => mock.Object.Property);
            Assert.Equal(expectedExceptionMessage, ex.Message);
        }
    }
}
