using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.TestTypeFoundation;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;
using Xunit;

namespace Ploeh.AutoFixture.AutoRhinoMock.UnitTest
{
    public class FixtureIntegrationTest
    {
        [Fact]
        public void FixtureDoesNotMockConcreteType()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Exercise system
            var result = fixture.Create<string>();
            // Verify outcome
            Assert.Throws<InvalidOperationException>( () => result.GetMockRepository());
            // Teardown
        }

        [Fact]
        public void FixtureAutoMocksInterface()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Exercise system
            var result = fixture.Create<IInterface>();
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
            var result = fixture.Create<IInterface>();
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
            var result = (IMockedObject)fixture.Create<IInterface>();
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
            var result = fixture.Create<AbstractType>();
            // Verify outcome
            Assert.NotNull(result.GetMockRepository());
            // Teardown
        }

        [Fact]
        public void FixtureCanCreateGuid()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Exercise system
            var result = fixture.Create<Guid>();
            // Verify outcome
            Assert.NotEqual(Guid.Empty, result);
            // Teardown
        }

        [Fact]
        public void FixtureAutoMocksAbstractTypeWithNonDefaultConstructorRequiringGuid()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Exercise system
            var result = fixture.Create<AbstractTypeWithNonDefaultConstructor<Guid>>();
            // Verify outcome
            Assert.NotEqual(Guid.Empty, result.Property);
            // Teardown
        }

        [Fact]
        public void FixtureAutoMocksAbstractTypeWithNonDefaultConstructor()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Exercise system
            var result = fixture.Create<AbstractTypeWithNonDefaultConstructor<int>>();
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
            var result = fixture.Create<AbstractTypeWithNonDefaultConstructor<RhinoMockTestTypes.AnotherAbstractTypeWithNonDefaultConstructor<int>>>();
            // Verify outcome
            Assert.NotNull(result.GetMockRepository());
            Assert.NotNull(result.Property.GetMockRepository());
            // Teardown
        }

        [Fact]
        public void FixtureDoesNotMockNestedConcreteTypeWithNonDefaultConstructor()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Exercise system
            var result = fixture.Create<AbstractTypeWithNonDefaultConstructor<RhinoMockTestTypes.ConcreteGenericType<int>>>();
            // Verify outcome
            Assert.NotNull(result.GetMockRepository());
            Assert.Throws<InvalidOperationException>(() => result.Property.GetMockRepository());
            // Teardown
        }

        [Fact]
        public void FixtureDoesNotMockParentOfNestedAbstractTypeWithNonDefaultConstructor()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Exercise system
            var result = fixture.Create<RhinoMockTestTypes.ConcreteGenericType<AbstractTypeWithNonDefaultConstructor<int>>>();
            // Verify outcome
            Assert.Throws<InvalidOperationException>(() => result.GetMockRepository());
            Assert.NotNull(result.Value.GetMockRepository());
            // Teardown
        }

        [Fact]
        public void FixtureMocksDoubleGenericTypeCorrectly()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Exercise system
            var result = fixture.Create<RhinoMockTestTypes.ConcreteDoublyGenericType<ConcreteType, AbstractTypeWithNonDefaultConstructor<int>>>();
            // Verify outcome
            Assert.Throws<InvalidOperationException>(() => result.GetMockRepository());
            Assert.Throws<InvalidOperationException>(() => result.Value1.GetMockRepository());
            Assert.NotNull(result.Value2.GetMockRepository());
            // Teardown
        }

        [Fact]
        public void CreateWithAbstractTypeReturnsMockedResult()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Exercise system
            var result = fixture.Create<AbstractType>();
            // Verify outcome
            Assert.NotNull(result.GetMockRepository());
        }

        [Fact]
        public void CreateAbstractGenericTypeWithNonDefaultConstructorIsCorrect()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Exercise system
            var result = fixture.Create<AbstractGenericType<object>>();
            // Verify outcome
            Assert.NotNull(result.GetMockRepository());
        }

        [Fact]
        public void CreateAbstractGenericTypeWithNonDefaultConstructorReturnsCorrectType()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Exercise system
            var result = fixture.Create<AbstractGenericType<object>>();
            // Verify outcome
            Assert.NotNull(result);
        }

        [Fact]
        public void CreateAbstractGenericTypeWithConcreteGenericParameterIsCorrect()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Exercise system
            var result = fixture.Create<AbstractGenericType<object>>();
            // Verify outcome
            Assert.Throws<InvalidOperationException>(() => result.Value.GetMockRepository());
        }

        [Fact]
        public void FixtureCanCreateList()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Exercise system
            var result = fixture.Create<List<ConcreteType>>();
            // Verify outcome
            Assert.True(result.Any());
            // Teardown
        }

        [Fact]
        public void FixtureCanCreateStack()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Exercise system
            var result = fixture.Create<Stack<ConcreteType>>();
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
            var result = fixture.Create<HashSet<ConcreteType>>();
            // Verify outcome
            Assert.True(result.Any());
            // Teardown
        }
    }
}
