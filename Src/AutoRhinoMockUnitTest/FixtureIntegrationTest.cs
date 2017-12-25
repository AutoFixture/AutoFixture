using System;
using System.Collections.Generic;
using System.Linq;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.AutoRhinoMock.UnitTest
{
    public class FixtureIntegrationTest
    {
        [Fact]
        public void FixtureDoesNotMockConcreteType()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Act
            var result = fixture.Create<string>();
            // Assert
            Assert.Throws<InvalidOperationException>( () => result.GetMockRepository());
        }

        [Fact]
        public void FixtureAutoMocksInterface()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Act
            var result = fixture.Create<IInterface>();
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void FixtureAutoMocksInterfaceCorrectly()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Act
            var result = fixture.Create<IInterface>();
            // Assert
            Assert.IsAssignableFrom<IMockedObject>(result);
        }

        [Fact]
        public void AutoMockedInterfaceHasMockRepository()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Act
            var result = (IMockedObject)fixture.Create<IInterface>();
            // Assert
            Assert.Null(Record.Exception(() => { var repo = result.Repository; }));
        }

        [Fact]
        public void FixtureAutoMocksAbstractType()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Act
            var result = fixture.Create<AbstractType>();
            // Assert
            Assert.NotNull(result.GetMockRepository());
        }

        [Fact]
        public void FixtureCanCreateGuid()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Act
            var result = fixture.Create<Guid>();
            // Assert
            Assert.NotEqual(Guid.Empty, result);
        }

        [Fact]
        public void FixtureAutoMocksAbstractTypeWithNonDefaultConstructorRequiringGuid()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Act
            var result = fixture.Create<AbstractTypeWithNonDefaultConstructor<Guid>>();
            // Assert
            Assert.NotEqual(Guid.Empty, result.Property);
        }

        [Fact]
        public void FixtureAutoMocksAbstractTypeWithNonDefaultConstructor()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Act
            var result = fixture.Create<AbstractTypeWithNonDefaultConstructor<int>>();
            // Assert
            Assert.NotEqual(0, result.Property);
        }

        [Fact]
        public void FixtureAutoMocksNestedAbstractTypeWithNonDefaultConstructor()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Act
            var result = fixture.Create<AbstractTypeWithNonDefaultConstructor<RhinoMockTestTypes.AnotherAbstractTypeWithNonDefaultConstructor<int>>>();
            // Assert
            Assert.NotNull(result.GetMockRepository());
            Assert.NotNull(result.Property.GetMockRepository());
        }

        [Fact]
        public void FixtureDoesNotMockNestedConcreteTypeWithNonDefaultConstructor()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Act
            var result = fixture.Create<AbstractTypeWithNonDefaultConstructor<RhinoMockTestTypes.ConcreteGenericType<int>>>();
            // Assert
            Assert.NotNull(result.GetMockRepository());
            Assert.Throws<InvalidOperationException>(() => result.Property.GetMockRepository());
        }

        [Fact]
        public void FixtureDoesNotMockParentOfNestedAbstractTypeWithNonDefaultConstructor()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Act
            var result = fixture.Create<RhinoMockTestTypes.ConcreteGenericType<AbstractTypeWithNonDefaultConstructor<int>>>();
            // Assert
            Assert.Throws<InvalidOperationException>(() => result.GetMockRepository());
            Assert.NotNull(result.Value.GetMockRepository());
        }

        [Fact]
        public void FixtureMocksDoubleGenericTypeCorrectly()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Act
            var result = fixture.Create<RhinoMockTestTypes.ConcreteDoublyGenericType<ConcreteType, AbstractTypeWithNonDefaultConstructor<int>>>();
            // Assert
            Assert.Throws<InvalidOperationException>(() => result.GetMockRepository());
            Assert.Throws<InvalidOperationException>(() => result.Value1.GetMockRepository());
            Assert.NotNull(result.Value2.GetMockRepository());
        }

        [Fact]
        public void CreateWithAbstractTypeReturnsMockedResult()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Act
            var result = fixture.Create<AbstractType>();
            // Assert
            Assert.NotNull(result.GetMockRepository());
        }

        [Fact]
        public void CreateAbstractGenericTypeWithNonDefaultConstructorIsCorrect()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Act
            var result = fixture.Create<AbstractGenericType<object>>();
            // Assert
            Assert.NotNull(result.GetMockRepository());
        }

        [Fact]
        public void CreateAbstractGenericTypeWithNonDefaultConstructorReturnsCorrectType()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Act
            var result = fixture.Create<AbstractGenericType<object>>();
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void CreateAbstractGenericTypeWithConcreteGenericParameterIsCorrect()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Act
            var result = fixture.Create<AbstractGenericType<object>>();
            // Assert
            Assert.Throws<InvalidOperationException>(() => result.Value.GetMockRepository());
        }

        [Fact]
        public void FixtureCanCreateList()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Act
            var result = fixture.Create<List<ConcreteType>>();
            // Assert
            Assert.True(result.Any());

        }

        [Fact]
        public void FixtureCanCreateStack()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Act
            var result = fixture.Create<Stack<ConcreteType>>();
            // Assert
            Assert.False(result.Any());
        }

        [Fact]
        public void FixtureCanCreateHashSet()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            // Act
            var result = fixture.Create<HashSet<ConcreteType>>();
            // Assert
            Assert.True(result.Any());
        }
    }
}
