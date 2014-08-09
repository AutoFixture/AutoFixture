using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixture.AutoMoq.UnitTest
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

        public interface IInterfaceWithProperty
        {
            string Property { get; set; }
        }
    }
}
