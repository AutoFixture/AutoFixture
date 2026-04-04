using System;
using AutoFixture.AutoNSubstitute.UnitTest.TestTypes;
using AutoFixture.Kernel;
using NSubstitute;
using Xunit;

// This class tests the obsolete component, so disable the warning.
#pragma warning disable 618

namespace AutoFixture.AutoNSubstitute.UnitTest
{
    public class NSubstituteVirtualMethodsCommandTest
    {
        [Fact]
        public void SetupThrowsWhenSubstituteIsNull()
        {
            // Arrange
            var context = Substitute.For<ISpecimenContext>();
            var sut = new NSubstituteVirtualMethodsCommand();
            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => sut.Execute(null, context));
        }

        [Fact]
        public void SetupThrowsWhenContextIsNull()
        {
            // Arrange
            var mock = Substitute.For<object>();
            var sut = new NSubstituteVirtualMethodsCommand();
            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => sut.Execute(mock, null));
        }

        [Fact]
        public void SetsUpInterfaceMethodsToRetrieveReturnValueFromContext()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var substitute = Substitute.For<IInterfaceWithParameterlessMethod>();
            var sut = new NSubstituteVirtualMethodsCommand();
            // Act
            sut.Execute(substitute, new SpecimenContext(fixture));
            // Assert
            var result = substitute.Method();
            Assert.Same(frozenString, result);
        }

        [Fact]
        public void SetsUpVirtualMethodsToRetrieveReturnValueFromContext()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var substitute = Substitute.For<TypeWithVirtualMembers>();
            var sut = new NSubstituteVirtualMethodsCommand();
            // Act
            sut.Execute(substitute, new SpecimenContext(fixture));
            // Assert
            var result = substitute.VirtualMethod();
            Assert.Equal(frozenString, result);
        }

        [Fact]
        public void SetsUpPropertyGettersToRetrieveReturnValueFromContext()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var substitute = Substitute.For<IInterfaceWithProperty>();
            var sut = new NSubstituteVirtualMethodsCommand();
            // Act
            sut.Execute(substitute, new SpecimenContext(fixture));
            // Assert
            var result = substitute.Property;
            Assert.Equal(frozenString, result);
        }

        [Fact]
        public void SetsUpMethodsWithParameters()
        {
            // Arrange
            var fixture = new Fixture();
            var anonymousString = fixture.Create<string>();
            var frozenString = fixture.Freeze<string>();
            var substitute = Substitute.For<IInterfaceWithMethod>();
            var sut = new NSubstituteVirtualMethodsCommand();
            // Act
            sut.Execute(substitute, new SpecimenContext(fixture));
            // Assert
            var result = substitute.Method(anonymousString);
            Assert.Same(frozenString, result);
        }

        [Fact]
        public void SetsUpMethodsWithOutParameters()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenInt = fixture.Freeze<int>();
            var substitute = Substitute.For<IInterfaceWithOutMethod>();
            var sut = new NSubstituteVirtualMethodsCommand();
            // Act
            sut.Execute(substitute, new SpecimenContext(fixture));
            // Assert
            int outResult;
            substitute.Method(out outResult);
            Assert.Equal(frozenInt, outResult);
        }

        [Fact]
        public void IgnoresVoidMethodsWithOutParameters()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenInt = fixture.Freeze<int>();
            var substitute = Substitute.For<IInterfaceWithOutVoidMethod>();
            var sut = new NSubstituteVirtualMethodsCommand();
            // Act
            sut.Execute(substitute, new SpecimenContext(fixture));
            // Assert
            int outResult;
            substitute.Method(out outResult);
            Assert.NotEqual(frozenInt, outResult);
        }

        [Fact]
        public void SetsUpIndexers()
        {
            // Arrange
            var fixture = new Fixture();
            var anonymousIndex = fixture.Create<int>();
            var frozenInt = fixture.Freeze<int>();
            var substitute = Substitute.For<IInterfaceWithIndexer>();
            var sut = new NSubstituteVirtualMethodsCommand();
            // Act
            sut.Execute(substitute, new SpecimenContext(fixture));
            // Assert
            int result = substitute[anonymousIndex];
            Assert.Equal(frozenInt, result);
        }

        [Fact]
        public void SetsUpMethodsLazily()
        {
            // Arrange
            var context = Substitute.For<ISpecimenContext>();
            var substitute = Substitute.For<IInterfaceWithParameterlessMethod>();
            var sut = new NSubstituteVirtualMethodsCommand();
            // Act
            sut.Execute(substitute, context);
            // Assert
            context.Received(0).Resolve(Arg.Any<object>());
            substitute.Method();
            context.Received(1).Resolve(Arg.Any<object>());
        }

        [Fact]
        public void SetsUpMethodsWithRefParameters()
        {
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var substitute = Substitute.For<IInterfaceWithRefMethod>();
            var sut = new NSubstituteVirtualMethodsCommand();

            sut.Execute(substitute, new SpecimenContext(fixture));

            string refResult = null;
            substitute.Method(ref refResult);
            Assert.Equal(frozenString, refResult);
        }

        [Fact]
        public void IgnoresSealedMethods()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var substitute = Substitute.For<TypeWithSealedMembers>();
            var sut = new NSubstituteVirtualMethodsCommand();
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Execute(substitute, new SpecimenContext(fixture))));

            Assert.NotEqual(frozenString, substitute.ImplicitlySealedMethod());
            Assert.NotEqual(frozenString, substitute.ExplicitlySealedMethod());
        }

        [Fact]
        public void IgnoresVoidMethods()
        {
            // Arrange
            var fixture = new Fixture();
            var substitute = Substitute.For<IInterfaceWithVoidMethod>();
            var sut = new NSubstituteVirtualMethodsCommand();
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Execute(substitute, new SpecimenContext(fixture))));
        }

        [Fact]
        public void IgnoresVoidMethodsWithParameters()
        {
            // Arrange
            var fixture = new Fixture();
            var substitute = Substitute.For<IInterfaceWithParameterVoidMethod>();
            var sut = new NSubstituteVirtualMethodsCommand();
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Execute(substitute, new SpecimenContext(fixture))));
        }

        [Fact]
        public void IgnoresGenericMethods()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var substitute = Substitute.For<IInterfaceWithGenericMethod>();
            var sut = new NSubstituteVirtualMethodsCommand();
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Execute(substitute, new SpecimenContext(fixture))));

            Assert.NotEqual(frozenString, substitute.GenericMethod<string>());
        }

        [Fact]
        public void IgnoresStaticMethods()
        {
            // Arrange
            var fixture = new Fixture();
            var substitute = Substitute.For<TypeWithStaticMethod>();
            var sut = new NSubstituteVirtualMethodsCommand();
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Execute(substitute, new SpecimenContext(fixture))));
        }

        [Fact]
        public void IgnoresNonSubstituteSpecimens()
        {
            var context = Substitute.For<ISpecimenContext>();
            var specimen = new ConcreteTypeWithVirtualMembers();
            var sut = new NSubstituteVirtualMethodsCommand();

            Assert.Null(Record.Exception(() => sut.Execute(specimen, context)));
            context.DidNotReceiveWithAnyArgs().Resolve(null);
        }

        [Fact]
        public void SetsUpMethodsToReturnSameValueOnSubsequentCalls()
        {
            var fixture = new Fixture();
            var anonymousString = fixture.Create<string>();
            var substitute = Substitute.For<IInterfaceWithMethod>();
            var sut = new NSubstituteVirtualMethodsCommand();

            sut.Execute(substitute, new SpecimenContext(fixture));

            var result1 = substitute.Method(anonymousString);
            var result2 = substitute.Method(anonymousString);
            Assert.Same(result1, result2);
        }

        [Fact]
        public void SetsUpMethodsToReturnDifferentValueOnSubsequentCallsWithAnotherParameter()
        {
            var fixture = new Fixture();
            var anonymousString1 = fixture.Create<string>();
            var anonymousString2 = fixture.Create<string>();
            var substitute = Substitute.For<IInterfaceWithMethod>();
            var sut = new NSubstituteVirtualMethodsCommand();

            sut.Execute(substitute, new SpecimenContext(fixture));

            var result1 = substitute.Method(anonymousString1);
            var result2 = substitute.Method(anonymousString2);
            Assert.NotEqual(result1, result2);
        }

        [Fact]
        public void SetsUpMethodsToReturnDifferentValueOnSubsequentCallsOfAnotherSubstitute()
        {
            var fixture = new Fixture();
            var anonymousString = fixture.Create<string>();
            var substitute1 = Substitute.For<IInterfaceWithMethod>();
            var substitute2 = Substitute.For<IInterfaceWithMethod>();
            var sut = new NSubstituteVirtualMethodsCommand();

            sut.Execute(substitute1, new SpecimenContext(fixture));
            sut.Execute(substitute2, new SpecimenContext(fixture));

            var result1 = substitute1.Method(anonymousString);
            var result2 = substitute2.Method(anonymousString);
            Assert.NotEqual(result1, result2);
        }

        [Fact]
        public void SetsUpMethodsToReturnSameOutValueOnSubsequentCalls()
        {
            var fixture = new Fixture();
            var substitute = Substitute.For<IInterfaceWithOutMethod>();
            var sut = new NSubstituteVirtualMethodsCommand();

            sut.Execute(substitute, new SpecimenContext(fixture));

            int outResult1;
            substitute.Method(out outResult1);
            int outResult2;
            substitute.Method(out outResult2);
            Assert.Equal(outResult1, outResult2);
        }

        [Fact]
        public void SetsUpMethodsToReturnSameOutValueOnSubsequentCallsWithSameParameters()
        {
            var fixture = new Fixture();
            var anonymousString = fixture.Create<string>();
            var substitute = Substitute.For<IInterfaceWithParameterAndOutMethod>();
            var sut = new NSubstituteVirtualMethodsCommand();

            sut.Execute(substitute, new SpecimenContext(fixture));

            int outResult1;
            substitute.Method(anonymousString, out outResult1);
            int outResult2;
            substitute.Method(anonymousString, out outResult2);
            Assert.Equal(outResult1, outResult2);
        }

        [Fact]
        public void SetsUpMethodsToReturnDifferentOutValueOnSubsequentCallsWithAnotherParameter()
        {
            var fixture = new Fixture();
            var anonymousString1 = fixture.Create<string>();
            var anonymousString2 = fixture.Create<string>();
            var substitute = Substitute.For<IInterfaceWithParameterAndOutMethod>();
            var sut = new NSubstituteVirtualMethodsCommand();

            sut.Execute(substitute, new SpecimenContext(fixture));

            int outResult1;
            substitute.Method(anonymousString1, out outResult1);
            int outResult2;
            substitute.Method(anonymousString2, out outResult2);
            Assert.NotEqual(outResult1, outResult2);
        }

        [Fact]
        public void SetsUpMethodsToReturnDifferentOutValueOnSubsequentCallsOfAnotherSubstitute()
        {
            var fixture = new Fixture();
            var substitute1 = Substitute.For<IInterfaceWithOutMethod>();
            var substitute2 = Substitute.For<IInterfaceWithOutMethod>();
            var sut = new NSubstituteVirtualMethodsCommand();

            sut.Execute(substitute1, new SpecimenContext(fixture));
            sut.Execute(substitute2, new SpecimenContext(fixture));

            int outResult1;
            substitute1.Method(out outResult1);
            int outResult2;
            substitute2.Method(out outResult2);
            Assert.NotEqual(outResult1, outResult2);
        }

        [Fact]
        public void SetsUpInterfaceBaseMethodsToRetrieveReturnValueFromContext()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var substitute = Substitute.For<IDerivedInterface>();

            var sut = new NSubstituteVirtualMethodsCommand();
            // Act
            sut.Execute(substitute, new SpecimenContext(fixture));
            // Assert
            var result = substitute.Method();
            Assert.Same(frozenString, result);
        }

        [Fact]
        public void SetsUpInterfaceNewMethodsToRetrieveReturnValueFromContext()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var substitute = Substitute.For<IInterfaceWithNewMethod>();

            var sut = new NSubstituteVirtualMethodsCommand();
            // Act
            sut.Execute(substitute, new SpecimenContext(fixture));
            // Assert
            var result = substitute.Method(0);
            Assert.Same(frozenString, result);
        }

        [Fact]
        public void SetsUpInterfaceShadowedMethodsToRetrieveReturnValueFromContext()
        {
            // Arrange
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var substitute = Substitute.For<IInterfaceWithNewMethod>();

            var sut = new NSubstituteVirtualMethodsCommand();
            // Act
            sut.Execute(substitute, new SpecimenContext(fixture));
            // Assert
            var result = (substitute as IInterfaceWithShadowedMethod).Method(0);
            Assert.Same(frozenString, result);
        }
    }
}