using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Moq;
using Ploeh.AutoFixture.AutoMoq.UnitTest.TestTypes;
using Ploeh.AutoFixture.Kernel;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.AutoMoq.UnitTest
{
    public class MockSealedPropertiesCommandTest
    {
        [Fact]
        public void SetupThrowsWhenMockIsNull()
        {
            // Fixture setup
            var context = new Mock<ISpecimenContext>();
            var sut = new MockSealedPropertiesCommand();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(
                () => sut.Execute(null, context.Object));
            // Teardown
        }

        [Fact]
        public void SetupThrowsWhenContextIsNull()
        {
            // Fixture setup
            var mock = new Mock<object>();
            var sut = new MockSealedPropertiesCommand();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(
                () => sut.Execute(mock, null));
            // Teardown
        }

        [Fact]
        public void InitializesSealedPropertyUsingContext()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<TypeWithSealedMembers>();

            var sut = new MockSealedPropertiesCommand();
            // Exercise system
            sut.Execute(mock, new SpecimenContext(fixture));
            // Verify outcome
            Assert.Equal(frozenString, mock.Object.ExplicitlySealedProperty);
            Assert.Equal(frozenString, mock.Object.ImplicitlySealedProperty);
            // Teardown
        }

        [Fact]
        public void IgnoresGetOnlyProperties()
        {
            // Fixture setup
            var fixture = new Fixture();
            var mock = new Mock<TypeWithGetOnlyProperty>();

            var sut = new MockSealedPropertiesCommand();
            //Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Execute(mock, new SpecimenContext(fixture)));
        }

        [Fact]
        public void IgnoresVirtualProperties()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<TypeWithVirtualMembers>();

            var sut = new MockSealedPropertiesCommand();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Execute(mock, new SpecimenContext(fixture)));
            Assert.NotEqual(frozenString, mock.Object.VirtualProperty);
        }

        [Fact]
        public void IgnoresPropertiesWithPrivateSetter()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<TypeWithPropertyWithPrivateSetter>();

            var sut = new MockSealedPropertiesCommand();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Execute(mock, new SpecimenContext(fixture)));
            Assert.NotEqual(frozenString, mock.Object.PropertyWithPrivateSetter);
        }

        [Fact]
        public void IgnoresPrivateProperties()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<TypeWithPrivateProperty>();
            var privateProperty = typeof (TypeWithPrivateProperty)
                .GetProperty("PrivateProperty",
                             BindingFlags.Instance | BindingFlags.NonPublic);

            var sut = new MockSealedPropertiesCommand();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Execute(mock, new SpecimenContext(fixture)));
            Assert.NotEqual(frozenString, privateProperty.GetValue(mock.Object, null));
        }

        [Fact]
        public void IgnoresInterfaceProperties()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<IInterfaceWithProperty>();

            var sut = new MockSealedPropertiesCommand();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Execute(mock, new SpecimenContext(fixture)));
            Assert.NotEqual(frozenString, mock.Object.Property);
        }

        [Fact]
        public void IgnoresStaticProperties()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<TypeWithStaticProperty>();

            var sut = new MockSealedPropertiesCommand();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Execute(mock, new SpecimenContext(fixture)));
            Assert.NotEqual(frozenString, TypeWithStaticProperty.Property);
        }

        [Fact]
        public void IgnoresIndexers()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenInt = fixture.Freeze<int>();
            var mock = new Mock<TypeWithIndexer>();

            var sut = new MockSealedPropertiesCommand();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Execute(mock, new SpecimenContext(fixture)));
            Assert.NotEqual(frozenInt, mock.Object[2]);
        }
    }
}
