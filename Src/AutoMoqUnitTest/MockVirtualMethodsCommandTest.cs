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
    public class MockVirtualMethodsCommandTest
    {
        [Fact]
        public void SetupThrowsWhenMockIsNull()
        {
            // Fixture setup
            var context = new Mock<ISpecimenContext>();
            var sut = new MockVirtualMethodsCommand();
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
            var sut = new MockVirtualMethodsCommand();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(
                () => sut.Execute(mock, null));
            // Teardown
        }

        [Fact]
        public void SetsUpInterfaceMethods_ToRetrieveReturnValueFromContext()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<IInterfaceWithParameterlessMethod>();

            var sut = new MockVirtualMethodsCommand();
            // Exercise system
            sut.Execute(mock, new SpecimenContext(fixture));
            // Verify outcome
            var result = mock.Object.Method();
            Assert.Same(frozenString, result);
            // Teardown
        }

        [Fact]
        public void SetsUpInterfaceBaseMethods_ToRetrieveReturnValueFromContext()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<IDerivedInterface>();

            var sut = new MockVirtualMethodsCommand();
            // Exercise system
            sut.Execute(mock, new SpecimenContext(fixture));
            // Verify outcome
            var result = mock.Object.Method();
            Assert.Same(frozenString, result);
            // Teardown
        }

        [Fact]
        public void SetsUpInterfaceNewMethods_ToRetrieveReturnValueFromContext()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<IInterfaceWithNewMethod>();

            var sut = new MockVirtualMethodsCommand();
            // Exercise system
            sut.Execute(mock, new SpecimenContext(fixture));
            // Verify outcome
            var result = mock.Object.Method(0);
            Assert.Same(frozenString, result);
            // Teardown
        }

        [Fact]
        public void SetsUpInterfaceShadowedMethods_ToRetrieveReturnValueFromContext()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<IInterfaceWithNewMethod>();

            var sut = new MockVirtualMethodsCommand();
            // Exercise system
            sut.Execute(mock, new SpecimenContext(fixture));
            // Verify outcome
            var result = (mock.Object as IInterfaceWithShadowedMethod).Method(0);
            Assert.Same(frozenString, result);
            // Teardown
        }

        [Fact]
        public void SetsUpVirtualMethods_ToRetrieveReturnValueFromContext()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<TypeWithVirtualMembers>();

            var sut = new MockVirtualMethodsCommand();
            // Exercise system
            sut.Execute(mock, new SpecimenContext(fixture));
            // Verify outcome
            var result = mock.Object.VirtualMethod();
            Assert.Equal(frozenString, result);
            // Teardown
        }

        [Fact]
        public void SetsUpPropertyGetters_ToRetrieveReturnValueFromContext()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<IInterfaceWithProperty>();

            var sut = new MockVirtualMethodsCommand();
            // Exercise system
            sut.Execute(mock, new SpecimenContext(fixture));
            // Verify outcome
            var result = mock.Object.Property;
            Assert.Equal(frozenString, result);
            // Teardown
        }

        [Fact]
        public void SetsUpMethodsWithParameters()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<IInterfaceWithMethod>();

            var sut = new MockVirtualMethodsCommand();
            // Exercise system
            sut.Execute(mock, new SpecimenContext(fixture));
            // Verify outcome
            var result = mock.Object.Method("hi");
            Assert.Same(frozenString, result);
            // Teardown
        }

        [Fact]
        public void SetsUpMethodsWithOutParameters()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenInt = fixture.Freeze<int>();
            var mock = new Mock<IInterfaceWithOutMethod>();

            var sut = new MockVirtualMethodsCommand();
            // Exercise system
            sut.Execute(mock, new SpecimenContext(fixture));
            // Verify outcome
            int outResult;
            mock.Object.Method(out outResult);
            Assert.Equal(frozenInt, outResult);
            // Teardown
        }

        [Fact]
        public void SetsUpIndexers()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenInt = fixture.Freeze<int>();
            var mock = new Mock<IInterfaceWithIndexer>();

            var sut = new MockVirtualMethodsCommand();
            // Exercise system
            sut.Execute(mock, new SpecimenContext(fixture));
            // Verify outcome
            int result = mock.Object[3];
            Assert.Equal(frozenInt, result);
            // Teardown
        }

        [Fact]
        public void SetsUpMethodsLazily()
        {
            // Fixture setup
            var context = new Mock<ISpecimenContext>();
            var mock = new Mock<IInterfaceWithParameterlessMethod>();

            var sut = new MockVirtualMethodsCommand();
            // Exercise system
            sut.Execute(mock, context.Object);
            // Verify outcome
            context.Verify(ctx => ctx.Resolve(It.IsAny<object>()), Times.Never());
            mock.Object.Method();
            context.Verify(ctx => ctx.Resolve(It.IsAny<object>()), Times.Once());
            // Teardown
        }

        [Fact]
        public void IgnoresMethodsWithRefParameters()
        {
            // Fixture setup
            var fixture = new Fixture();
            var mock = new Mock<IInterfaceWithRefMethod>();

            var sut = new MockVirtualMethodsCommand();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Execute(mock, new SpecimenContext(fixture)));
        }

        [Fact]
        public void IgnoresSealedMethods()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<TypeWithSealedMembers>();

            var sut = new MockVirtualMethodsCommand();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Execute(mock, new SpecimenContext(fixture)));
            Assert.NotEqual(frozenString, mock.Object.ImplicitlySealedMethod());
            Assert.NotEqual(frozenString, mock.Object.ExplicitlySealedMethod());
        }

        [Fact]
        public void IgnoresVoidMethods()
        {
            // Fixture setup
            var fixture = new Fixture();
            var mock = new Mock<IInterfaceWithVoidMethod>();

            var sut = new MockVirtualMethodsCommand();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Execute(mock, new SpecimenContext(fixture)));
        }

        [Fact]
        public void IgnoresGenericMethods()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<IInterfaceWithGenericMethod>();

            var sut = new MockVirtualMethodsCommand();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Execute(mock, new SpecimenContext(fixture)));
            Assert.NotEqual(frozenString, mock.Object.GenericMethod<string>());
        }

        [Fact]
        public void IgnoresStaticMethods()
        {
            // Fixture setup
            var fixture = new Fixture();
            var mock = new Mock<TypeWithStaticMethod>();

            var sut = new MockVirtualMethodsCommand();
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
