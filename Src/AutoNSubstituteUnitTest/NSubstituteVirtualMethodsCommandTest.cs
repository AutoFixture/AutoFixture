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
            // Fixture setup
            var context = Substitute.For<ISpecimenContext>();
            var sut = new NSubstituteVirtualMethodsCommand();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(
                () => sut.Execute(null, context));
            // Teardown
        }

        [Fact]
        public void SetupThrowsWhenContextIsNull()
        {
            // Fixture setup
            var mock = Substitute.For<object>();
            var sut = new NSubstituteVirtualMethodsCommand();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(
                () => sut.Execute(mock, null));
            // Teardown
        }

        [Fact]
        public void SetsUpInterfaceMethodsToRetrieveReturnValueFromContext()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var substitute = Substitute.For<IInterfaceWithParameterlessMethod>();
            var sut = new NSubstituteVirtualMethodsCommand();
            // Exercise system
            sut.Execute(substitute, new SpecimenContext(fixture));
            // Verify outcome
            var result = substitute.Method();
            Assert.Same(frozenString, result);
            // Teardown
        }

        [Fact]
        public void SetsUpVirtualMethodsToRetrieveReturnValueFromContext()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var substitute = Substitute.For<TypeWithVirtualMembers>();
            var sut = new NSubstituteVirtualMethodsCommand();
            // Exercise system
            sut.Execute(substitute, new SpecimenContext(fixture));
            // Verify outcome
            var result = substitute.VirtualMethod();
            Assert.Equal(frozenString, result);
            // Teardown
        }

        [Fact]
        public void SetsUpPropertyGettersToRetrieveReturnValueFromContext()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var substitute = Substitute.For<IInterfaceWithProperty>();
            var sut = new NSubstituteVirtualMethodsCommand();
            // Exercise system
            sut.Execute(substitute, new SpecimenContext(fixture));
            // Verify outcome
            var result = substitute.Property;
            Assert.Equal(frozenString, result);
            // Teardown
        }

        [Fact]
        public void SetsUpMethodsWithParameters()
        {
            // Fixture setup
            var fixture = new Fixture();
            var anonymousString = fixture.Create<string>();
            var frozenString = fixture.Freeze<string>();
            var substitute = Substitute.For<IInterfaceWithMethod>();
            var sut = new NSubstituteVirtualMethodsCommand();
            // Exercise system
            sut.Execute(substitute, new SpecimenContext(fixture));
            // Verify outcome
            var result = substitute.Method(anonymousString);
            Assert.Same(frozenString, result);
            // Teardown
        }

        [Fact]
        public void SetsUpMethodsWithOutParameters()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenInt = fixture.Freeze<int>();
            var substitute = Substitute.For<IInterfaceWithOutMethod>();
            var sut = new NSubstituteVirtualMethodsCommand();
            // Exercise system
            sut.Execute(substitute, new SpecimenContext(fixture));
            // Verify outcome
            int outResult;
            substitute.Method(out outResult);
            Assert.Equal(frozenInt, outResult);
            // Teardown
        }

        [Fact]
        public void IgnoresVoidMethodsWithOutParameters()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenInt = fixture.Freeze<int>();
            var substitute = Substitute.For<IInterfaceWithOutVoidMethod>();
            var sut = new NSubstituteVirtualMethodsCommand();
            // Exercise system
            sut.Execute(substitute, new SpecimenContext(fixture));
            // Verify outcome
            int outResult;
            substitute.Method(out outResult);
            Assert.NotEqual(frozenInt, outResult);
            // Teardown
        }

        [Fact]
        public void SetsUpIndexers()
        {
            // Fixture setup
            var fixture = new Fixture();
            var anonymousIndex = fixture.Create<int>();
            var frozenInt = fixture.Freeze<int>();
            var substitute = Substitute.For<IInterfaceWithIndexer>();
            var sut = new NSubstituteVirtualMethodsCommand();
            // Exercise system
            sut.Execute(substitute, new SpecimenContext(fixture));
            // Verify outcome
            int result = substitute[anonymousIndex];
            Assert.Equal(frozenInt, result);
            // Teardown
        }

        [Fact]
        public void SetsUpMethodsLazily()
        {
            // Fixture setup
            var context = Substitute.For<ISpecimenContext>();
            var substitute = Substitute.For<IInterfaceWithParameterlessMethod>();
            var sut = new NSubstituteVirtualMethodsCommand();
            // Exercise system
            sut.Execute(substitute, context);
            // Verify outcome
            context.Received(0).Resolve(Arg.Any<object>());
            substitute.Method();
            context.Received(1).Resolve(Arg.Any<object>());
            // Teardown
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
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var substitute = Substitute.For<TypeWithSealedMembers>();
            var sut = new NSubstituteVirtualMethodsCommand();
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() => sut.Execute(substitute, new SpecimenContext(fixture))));

            Assert.NotEqual(frozenString, substitute.ImplicitlySealedMethod());
            Assert.NotEqual(frozenString, substitute.ExplicitlySealedMethod());
        }

        [Fact]
        public void IgnoresVoidMethods()
        {
            // Fixture setup
            var fixture = new Fixture();
            var substitute = Substitute.For<IInterfaceWithVoidMethod>();
            var sut = new NSubstituteVirtualMethodsCommand();
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() => sut.Execute(substitute, new SpecimenContext(fixture))));
        }

        [Fact]
        public void IgnoresVoidMethodsWithParameters()
        {
            // Fixture setup
            var fixture = new Fixture();
            var substitute = Substitute.For<IInterfaceWithParameterVoidMethod>();
            var sut = new NSubstituteVirtualMethodsCommand();
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() => sut.Execute(substitute, new SpecimenContext(fixture))));
        }

        [Fact]
        public void IgnoresGenericMethods()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var substitute = Substitute.For<IInterfaceWithGenericMethod>();
            var sut = new NSubstituteVirtualMethodsCommand();
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() => sut.Execute(substitute, new SpecimenContext(fixture))));

            Assert.NotEqual(frozenString, substitute.GenericMethod<string>());
        }

        [Fact]
        public void IgnoresStaticMethods()
        {
            // Fixture setup
            var fixture = new Fixture();
            var substitute = Substitute.For<TypeWithStaticMethod>();
            var sut = new NSubstituteVirtualMethodsCommand();
            // Exercise system and verify outcome
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
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var substitute = Substitute.For<IDerivedInterface>();

            var sut = new NSubstituteVirtualMethodsCommand();
            // Exercise system
            sut.Execute(substitute, new SpecimenContext(fixture));
            // Verify outcome
            var result = substitute.Method();
            Assert.Same(frozenString, result);
            // Teardown
        }

        [Fact]
        public void SetsUpInterfaceNewMethodsToRetrieveReturnValueFromContext()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var substitute = Substitute.For<IInterfaceWithNewMethod>();

            var sut = new NSubstituteVirtualMethodsCommand();
            // Exercise system
            sut.Execute(substitute, new SpecimenContext(fixture));
            // Verify outcome
            var result = substitute.Method(0);
            Assert.Same(frozenString, result);
            // Teardown
        }

        [Fact]
        public void SetsUpInterfaceShadowedMethodsToRetrieveReturnValueFromContext()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var substitute = Substitute.For<IInterfaceWithNewMethod>();

            var sut = new NSubstituteVirtualMethodsCommand();
            // Exercise system
            sut.Execute(substitute, new SpecimenContext(fixture));
            // Verify outcome
            var result = (substitute as IInterfaceWithShadowedMethod).Method(0);
            Assert.Same(frozenString, result);
            // Teardown
        }
    }
}