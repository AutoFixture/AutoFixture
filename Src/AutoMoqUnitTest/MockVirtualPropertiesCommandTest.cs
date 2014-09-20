using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using Ploeh.AutoFixture.AutoMoq.UnitTest.TestTypes;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixture.AutoMoq.UnitTest
{
    public class MockVirtualPropertiesCommandTest
    {
        [Fact]
        public void ExecuteThrowsWhenSpecimenIsNull()
        {
            // Fixture setup
            var context = new Mock<ISpecimenContext>();
            var sut = new MockVirtualPropertiesCommand();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => sut.Execute(null, context.Object));
        }

        [Fact]
        public void ExecuteThrowsWhenContextIsNull()
        {
            // Fixture setup
            var specimen = new Mock<IInterfaceWithProperty>();
            var sut = new MockVirtualPropertiesCommand();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => sut.Execute(specimen, null));
        }

        [Fact]
        public void StubsVirtualPropertiesWithGetAndSetAccessors()
        {
            // Fixture setup
            var fixture = new Fixture();
            var context = new SpecimenContext(fixture);
            var specimen = new Mock<IInterfaceWithProperty>();

            var sut = new MockVirtualPropertiesCommand();
            // Exercise system
            sut.Execute(specimen, context);
            // Verify outcome
            specimen.Object.Property = "a string";
            Assert.Equal("a string", specimen.Object.Property);
            // Teardown
        }

        [Fact]
        public void StubsInterfaceBaseProperties()
        {
            // Fixture setup
            var fixture = new Fixture();
            var context = new SpecimenContext(fixture);
            var specimen = new Mock<IDerivedInterface>();

            var sut = new MockVirtualPropertiesCommand();
            // Exercise system
            sut.Execute(specimen, context);
            // Verify outcome
            specimen.Object.Property = "a string";
            Assert.Equal("a string", specimen.Object.Property);
            // Teardown
        }

        [Fact]
        public void StubsInterfaceNewProperties()
        {
            // Fixture setup
            var fixture = new Fixture();
            var context = new SpecimenContext(fixture);
            var specimen = new Mock<IInterfaceWithNewProperty>();

            var sut = new MockVirtualPropertiesCommand();
            // Exercise system
            sut.Execute(specimen, context);
            // Verify outcome
            specimen.Object.Property = "a string";
            Assert.Equal("a string", specimen.Object.Property);
            // Teardown
        }

        [Fact]
        public void StubsInterfaceShadowedProperties()
        {
            // Fixture setup
            var fixture = new Fixture();
            var context = new SpecimenContext(fixture);
            var specimen = new Mock<IInterfaceWithNewProperty>();

            var sut = new MockVirtualPropertiesCommand();
            // Exercise system
            sut.Execute(specimen, context);
            // Verify outcome
            IInterfaceWithShadowedProperty objAsBaseInterface = specimen.Object;
            objAsBaseInterface.Property = "a string";
            Assert.Equal("a string", objAsBaseInterface.Property);
            // Teardown
        }

        [Fact]
        public void InitialValueIsResolvedFromFixture()
        {
            // Fixture setup
            var fixture = new Fixture();
            var context = new SpecimenContext(fixture);
            var specimen = new Mock<IInterfaceWithProperty>();
            var frozenString = fixture.Freeze<string>();

            var sut = new MockVirtualPropertiesCommand();
            // Exercise system
            sut.Execute(specimen, context);
            // Verify outcome
            var initialValue = specimen.Object.Property;
            Assert.Equal(frozenString, initialValue);
            // Teardown
        }

        [Fact]
        public void InitialValueIsResolvedLazily()
        {
            // Fixture setup
            var fixture = new Fixture();
            var context = new SpecimenContext(fixture);
            var specimen = new Mock<IInterfaceWithProperty>();

            var sut = new MockVirtualPropertiesCommand();
            // Exercise system
            sut.Execute(specimen, context);
            var frozenString = fixture.Freeze<string>();
            // Verify outcome
            var initialValue = specimen.Object.Property;
            Assert.Equal(frozenString, initialValue);
            // Teardown
        }

        [Fact]
        public void IgnoresPropertiesWithoutPublicGetAccessor()
        {
            // Fixture setup
            var fixture = new Fixture();
            var context = new SpecimenContext(fixture);
            var specimen = new Mock<TypeWithVirtualProtectedPropertyAccessors>(MockBehavior.Strict);

            var sut = new MockVirtualPropertiesCommand();
            // Exercise system
            sut.Execute(specimen, context);
            // Verify outcome
            // If this property wasn't setup, then calling the public setter should throw an exception
            Assert.Throws<MockException>(() => specimen.Object.PropertyWithProtectedGet = "");
            // Teardown
        }

        [Fact]
        public void IgnoresPropertiesWithoutPublicSetAccessor()
        {
            // Fixture setup
            var fixture = new Fixture();
            var context = new SpecimenContext(fixture);
            var specimen = new Mock<TypeWithVirtualProtectedPropertyAccessors>(MockBehavior.Strict);

            var sut = new MockVirtualPropertiesCommand();
            // Exercise system
            sut.Execute(specimen, context);
            // Verify outcome
            // If this property wasn't setup, then calling the public getter should throw an exception
            Assert.Throws<MockException>(() => specimen.Object.PropertyWithProtectedSet);
            // Teardown
        }

        [Fact]
        public void IgnoresIndexers()
        {
            // Fixture setup
            var fixture = new Fixture();
            var context = new SpecimenContext(fixture);
            var specimen = new Mock<IInterfaceWithIndexer>(MockBehavior.Strict);

            var sut = new MockVirtualPropertiesCommand();
            // Exercise system
            sut.Execute(specimen, context);
            // Verify outcome
            Assert.Throws<MockException>(() => specimen.Object[0]);
            Assert.Throws<MockException>(() => specimen.Object[0] = 123);
            // Teardown
        }

        [Fact]
        public void IgnoresSealedProperties()
        {
            // Fixture setup
            var fixture = new Fixture();
            var mock = new Mock<TypeWithSealedMembers>();

            var sut = new MockVirtualPropertiesCommand();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Execute(mock, new SpecimenContext(fixture)));
        }

        [Fact]
        public void IgnoresStaticProperties()
        {
            // Fixture setup
            var fixture = new Fixture();
            var mock = new Mock<TypeWithStaticProperty>();

            var sut = new MockVirtualPropertiesCommand();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Execute(mock, new SpecimenContext(fixture)));
        }

        [Fact]
        public void IgnoresNonMockSpecimens()
        {
            // Fixture setup
            // The context mock has a strict behaviour - if any of its members are invoked, an exception will be thrown
            var context = new Mock<ISpecimenContext>(MockBehavior.Strict);
            var specimen = new TypeWithVirtualMembers();

            var sut = new MockSealedPropertiesCommand();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Execute(specimen, context.Object));
        }
    }
}
