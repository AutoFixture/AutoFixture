using System;
using AutoFixture.AutoMoq.UnitTest.TestTypes;
using AutoFixture.Kernel;
using Moq;
using Xunit;

namespace AutoFixture.AutoMoq.UnitTest
{
    public class MockVirtualMethodsCommandTest
    {
        [Theory]
        [ClassData(typeof (ValidNonMockSpecimens))]
        public void ExecuteShouldNotThrowWhenSpecimenIsValidNonMockSpecimen(object validNonMockSpecimen)
        {
            // Arrange
            var context = new Mock<ISpecimenContext>();
            var sut = new MockVirtualMethodsCommand();
            // Act & Assert
            Assert.Null(
                Record.Exception(() => sut.Execute(validNonMockSpecimen, context.Object)));
        }

        [Fact]
        public void SetupThrowsWhenContextIsNull()
        {
            // Arrange
            var mock = new Mock<object>();
            var sut = new MockVirtualMethodsCommand();
            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => sut.Execute(mock, null));
        }

        [Fact]
        public void SetsUpInterfaceMethods_ToRetrieveReturnValueFromContext()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<IInterfaceWithParameterlessMethod>();

            var sut = new MockVirtualMethodsCommand();
            // Act
            sut.Execute(mock, new SpecimenContext(fixture));
            // Assert
            var result = mock.Object.Method();
            Assert.Same(frozenString, result);
        }

        [Fact]
        public void SetsUpInterfaceBaseMethods_ToRetrieveReturnValueFromContext()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<IDerivedInterface>();

            var sut = new MockVirtualMethodsCommand();
            // Act
            sut.Execute(mock, new SpecimenContext(fixture));
            // Assert
            var result = mock.Object.Method();
            Assert.Same(frozenString, result);
        }

        [Fact]
        public void SetsUpInterfaceNewMethods_ToRetrieveReturnValueFromContext()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<IInterfaceWithNewMethod>();

            var sut = new MockVirtualMethodsCommand();
            // Act
            sut.Execute(mock, new SpecimenContext(fixture));
            // Assert
            var result = mock.Object.Method(0);
            Assert.Same(frozenString, result);
        }

        [Fact]
        public void SetsUpInterfaceShadowedMethods_ToRetrieveReturnValueFromContext()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<IInterfaceWithNewMethod>();

            var sut = new MockVirtualMethodsCommand();
            // Act
            sut.Execute(mock, new SpecimenContext(fixture));
            // Assert
            var result = (mock.Object as IInterfaceWithShadowedMethod).Method(0);
            Assert.Same(frozenString, result);
        }

        [Fact]
        public void SetsUpVirtualMethods_ToRetrieveReturnValueFromContext()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<TypeWithVirtualMembers>();

            var sut = new MockVirtualMethodsCommand();
            // Act
            sut.Execute(mock, new SpecimenContext(fixture));
            // Assert
            var result = mock.Object.VirtualMethod();
            Assert.Equal(frozenString, result);
        }

        [Fact]
        public void SetsUpPropertyGetters_ToRetrieveReturnValueFromContext()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<IInterfaceWithGetOnlyProperty>();

            var sut = new MockVirtualMethodsCommand();
            // Act
            sut.Execute(mock, new SpecimenContext(fixture));
            // Assert
            var result = mock.Object.GetOnlyProperty;
            Assert.Equal(frozenString, result);
        }

        [Fact]
        public void SetsUpVirtualPropertyGetters_ToRetrieveReturnValueFromContext()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<TypeWithVirtualMembers>();

            var sut = new MockVirtualMethodsCommand();
            // Act
            sut.Execute(mock, new SpecimenContext(fixture));
            // Assert
            var result = mock.Object.VirtualGetOnlyProperty;
            Assert.Equal(frozenString, result);
        }

        [Fact]
        public void SetsUpMethodsWithParameters()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<IInterfaceWithMethod>();

            var sut = new MockVirtualMethodsCommand();
            // Act
            sut.Execute(mock, new SpecimenContext(fixture));
            // Assert
            var result = mock.Object.Method("hi");
            Assert.Same(frozenString, result);
        }

        [Fact]
        public void SetsUpMethodsWithOutParameters()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenInt = fixture.Freeze<int>();
            var mock = new Mock<IInterfaceWithOutMethod>();

            var sut = new MockVirtualMethodsCommand();
            // Act
            sut.Execute(mock, new SpecimenContext(fixture));
            // Assert
            int outResult;
            mock.Object.Method(out outResult);
            Assert.Equal(frozenInt, outResult);
        }

        [Fact]
        public void SetsUpIndexers()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenInt = fixture.Freeze<int>();
            var mock = new Mock<IInterfaceWithIndexer>();

            var sut = new MockVirtualMethodsCommand();
            // Act
            sut.Execute(mock, new SpecimenContext(fixture));
            // Assert
            int result = mock.Object[3];
            Assert.Equal(frozenInt, result);
        }

        [Fact]
        public void SetsUpMethodsLazily()
        {
            // Arrange
            var context = new Mock<ISpecimenContext>();
            var mock = new Mock<IInterfaceWithParameterlessMethod>();

            var sut = new MockVirtualMethodsCommand();
            // Act
            sut.Execute(mock, context.Object);
            // Assert
            context.Verify(ctx => ctx.Resolve(It.IsAny<object>()), Times.Never());
            mock.Object.Method();
            context.Verify(ctx => ctx.Resolve(It.IsAny<object>()), Times.Once());
        }

        [Fact]
        public void IgnoresMethodsWithRefParameters()
        {
            // Arrange
            var fixture = new Fixture();
            var mock = new Mock<IInterfaceWithRefMethod>();

            var sut = new MockVirtualMethodsCommand();
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Execute(mock, new SpecimenContext(fixture))));
        }

        [Fact]
        public void IgnoresSealedMethods()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<TypeWithSealedMembers>();

            var sut = new MockVirtualMethodsCommand();
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Execute(mock, new SpecimenContext(fixture))));
            Assert.NotEqual(frozenString, mock.Object.ImplicitlySealedMethod());
            Assert.NotEqual(frozenString, mock.Object.ExplicitlySealedMethod());
        }

        [Fact]
        public void IgnoresVoidMethods()
        {
            // Arrange
            var fixture = new Fixture();
            var mock = new Mock<IInterfaceWithVoidMethod>();

            var sut = new MockVirtualMethodsCommand();
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Execute(mock, new SpecimenContext(fixture))));
        }

        [Fact]
        public void IgnoresGenericMethods()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<IInterfaceWithGenericMethod>();

            var sut = new MockVirtualMethodsCommand();
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Execute(mock, new SpecimenContext(fixture))));
            Assert.NotEqual(frozenString, mock.Object.GenericMethod<string>());
        }

        [Fact]
        public void IgnoresStaticMethods()
        {
            // Arrange
            var fixture = new Fixture();
            var mock = new Mock<TypeWithStaticMethod>();

            var sut = new MockVirtualMethodsCommand();
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Execute(mock, new SpecimenContext(fixture))));
        }

        [Fact]
        public void IgnoresNonMockSpecimens()
        {
            // Arrange
            // The context mock has a strict behaviour - if any of its members are invoked, an exception will be thrown
            var context = new Mock<ISpecimenContext>(MockBehavior.Strict);
            var specimen = new TypeWithVirtualMembers();

            var sut = new MockVirtualMethodsCommand();
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Execute(specimen, context.Object)));
        }

        [Fact]
        public void IgnoresPropertiesWithGettersAndSetters()
        {
            // Arrange
            var fixture = new Fixture();
            var mock = new Mock<IInterfaceWithProperty>();

            var sut = new MockVirtualMethodsCommand();
            // Act
            sut.Execute(mock, new SpecimenContext(fixture));
            // Assert
            var result = mock.Object;
            Assert.Null(result.Property);
        }

        [Fact]
        public void IgnoresPropertiesWithGettersAndSettersFromInheritedInterface()
        {
            // Arrange
            var fixture = new Fixture();
            var mock = new Mock<IDerivedInterfaceWithProperty>();

            var sut = new MockVirtualMethodsCommand();
            // Act
            sut.Execute(mock, new SpecimenContext(fixture));
            // Assert
            var result = mock.Object;
            Assert.Null(result.Property);
        }

        [Fact]
        public void IgnoresPropertiesWithGettersAndSettersFromInheritedInterfaceOfInheritedInterface()
        {
            // Arrange
            var fixture = new Fixture();
            var mock = new Mock<IDerivedInterfaceWithProperty>();

            var sut = new MockVirtualMethodsCommand();
            // Act
            sut.Execute(mock, new SpecimenContext(fixture));
            // Assert
            var result = mock.Object;
            Assert.Null(result.Property);
            Assert.Null(result.SecondProperty);
        }

        [Fact]
        public void IgnoresVirtualPropertiesWithGettersAndSetters()
        {
            // Arrange
            var fixture = new Fixture();
            var mock = new Mock<TypeWithVirtualMembers>();

            var sut = new MockVirtualMethodsCommand();
            // Act
            sut.Execute(mock, new SpecimenContext(fixture));
            // Assert
            var result = mock.Object;
            Assert.Null(result.VirtualProperty);
        }

        [Fact]
        public void SetsUpOutParametersWithNullWhenContextReturnsNull()
        {
            // Arrange
            var contextMock = new Mock<ISpecimenContext>();
            contextMock.Setup(ctx => ctx.Resolve(typeof (string)))
                .Returns(null);
            var mock = new Mock<IInterfaceWithMethodWithOutParameterOfReferenceType>();

            var sut = new MockVirtualMethodsCommand();
            // Act
            sut.Execute(mock, contextMock.Object);
            // Assert
            string outResult;
            mock.Object.Method(out outResult);
            Assert.Null(outResult);
        }

        [Fact]
        public void DoesNotSetupMethodsWithOutParametersWhenContextReturnsOmitSpecimen()
        {
            // Arrange
            var contextMock = new Mock<ISpecimenContext>();
            contextMock.Setup(ctx => ctx.Resolve(typeof (int)))
                .Returns(new OmitSpecimen());
            var mock = new Mock<IInterfaceWithOutMethod>(MockBehavior.Strict);

            var sut = new MockVirtualMethodsCommand();
            // Act
            sut.Execute(mock, contextMock.Object);
            // Assert
            // The mock has strict behaviour - calling methods that were not setup cause an exception to be thrown
            int outResult;
            Assert.Throws<MockException>(() => mock.Object.Method(out outResult));
        }
    }
}
