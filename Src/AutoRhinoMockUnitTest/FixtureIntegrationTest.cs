using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.TestTypeFoundation;
using Rhino.Mocks;
using Xunit;
using Rhino.Mocks.Interfaces;
using Ploeh.AutoFixture.AutoRhinoMock;
using Ploeh.AutoFixture;

namespace AutoRhinoMockUnitTest
{
    public class FixtureIntegrationTest
    {
        [Fact]
        public void FixtureAutoMocksInterface()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Exercise system
            var result = fixture.CreateAnonymous<IInterface>();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void FixtureAutoMocksInterfaceCorrectly()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Exercise system
            var result = fixture.CreateAnonymous<IInterface>();
            // Verify outcome
            Assert.IsAssignableFrom<IMockedObject>(result);
            // Teardown
        }

        [Fact]
        public void AutoMockedInterfaceHasMockRepository()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Exercise system
            var result = (IMockedObject)fixture.CreateAnonymous<IInterface>();
            // Verify outcome
            Assert.DoesNotThrow(() => { var repo = result.Repository; });
            // Teardown
        }

        [Fact]
        public void FixtureAutoMocksAbstractType()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Exercise system
            var result = fixture.CreateAnonymous<AbstractType>();
            // Verify outcome
            Assert.NotNull(result.GetMockRepository());
            // Teardown
        }

        [Fact]
        public void FixtureAutoMocksAbstractTypeWithNonDefaultConstructor()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Exercise system
            var result = fixture.CreateAnonymous<AbstractTypeWithNonDefaultConstructor<int>>();
            // Verify outcome
            Assert.NotEqual(0, result.Property);
            // Teardown
        }

        [Fact]
        public void FixtureAutoMocksNestedAbstractTypeWithNonDefaultConstructor()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Exercise system
            var result = fixture.CreateAnonymous<AbstractTypeWithNonDefaultConstructor<RhinoMockTestTypes.AnotherAbstractTypeWithNonDefaultConstructor<int>>>();
            // Verify outcome
            Assert.NotEqual(0, result.Property.Value);
            // Teardown
        }

        [Fact]
        public void FixtureCanCreateIList()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Exercise system
            var result = fixture.CreateAnonymous<IList<ConcreteType>>();
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }

        [Fact]
        public void FixtureCanCreateList()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Exercise system
            var result = fixture.CreateAnonymous<List<ConcreteType>>();
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }

        [Fact]
        public void FixtureCanCreateICollection()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Exercise system
            var result = fixture.CreateAnonymous<ICollection<ConcreteType>>();
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }

        [Fact]
        public void FixtureCanCreateStack()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Exercise system
            var result = fixture.CreateAnonymous<Stack<ConcreteType>>();
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }

        [Fact]
        public void FixtureCanCreateHashSet()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Exercise system
            var result = fixture.CreateAnonymous<HashSet<ConcreteType>>();
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }

        [Fact]
        public void FixtureCanCreateDictionary()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Exercise system
            var result = fixture.CreateAnonymous<IDictionary<ConcreteType, IInterface>>();
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }

    }
}
