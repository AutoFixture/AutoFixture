namespace Ploeh.AutoFixture.Idioms.FsCheckUnitTest

open FsCheck
open Ploeh.AutoFixture
open Ploeh.AutoFixture.Idioms
open Ploeh.AutoFixture.Idioms.FsCheck
open Ploeh.AutoFixture.Kernel
open Swensen.Unquote
open System
open System.Reflection
open Xunit

type ReturnValueMustNotBeNullAssertionTest () = 

    [<Fact>]
    let SutIsIdiomaticAssertion () =
        let dummyBuilder = Fixture()
        let sut = ReturnValueMustNotBeNullAssertion(dummyBuilder);
        verify <@ sut |> implements<IdiomaticAssertion> @>

    [<Fact>]
    let BuilderIsCorrect () = 
        let expected = Fixture() :> ISpecimenBuilder
        let sut = ReturnValueMustNotBeNullAssertion(expected)

        let actual = sut.Builder

        verify <@ expected = actual @>

    [<Fact>]
    let InitializeWithNullBuilderThrows () = 
        raises<ArgumentNullException> <@ ReturnValueMustNotBeNullAssertion(null) @>

    [<Fact>]
    let VerifyNullPropertyThrows () = 
        let dummyBuilder = Fixture()
        let sut = ReturnValueMustNotBeNullAssertion(dummyBuilder)
        raises<ArgumentNullException> <@ sut.Verify(null :> PropertyInfo) @>

    [<Fact>]
    let VerifyNullMethodThrows () = 
        let dummyBuilder = Fixture()
        let sut = ReturnValueMustNotBeNullAssertion(dummyBuilder)
        raises<ArgumentNullException> <@ sut.Verify(null :> MethodInfo) @>

    [<Theory>]
    [<InlineData(typeof<AClass>)>]
    [<InlineData(typeof<AStaticClass>)>]
    let VerifyWriteOnlyPropertiesDoesNotThrow (t: Type) =
        let sut = ReturnValueMustNotBeNullAssertion(Fixture())
        let prop = t.GetProperty("WriteOnlyProperty")
        sut.Verify(prop)

    [<Theory>]
    [<InlineData(typeof<AClass>)>]
    [<InlineData(typeof<AStaticClass>)>]
    let VerifyVoidMethodsDoesNotThrow (t: Type) =
        let sut = ReturnValueMustNotBeNullAssertion(Fixture())
        let method = t.GetMethod("VoidMethod")
        sut.Verify(method)

    [<Theory>]
    [<InlineData(typeof<AClass>)>]
    [<InlineData(typeof<AStaticClass>)>]
    let VerifyPropertiesWithReturnValueDoesNotThrow (t: Type) =
        let sut = ReturnValueMustNotBeNullAssertion(Fixture())
        let prop = t.GetProperty("ReadOnlyProperty")
        sut.Verify(prop)

    [<Theory>]
    [<InlineData(typeof<AClass>)>]
    [<InlineData(typeof<AStaticClass>)>]
    let VerifyMethodsWithReturnValueDoesNotThrow (t: Type) =
        let sut = ReturnValueMustNotBeNullAssertion(Fixture())
        let method = t.GetMethod("MethodWithReturnValue")
        sut.Verify(method)

    [<Theory>]
    [<InlineData(typeof<AClass>)>]
    [<InlineData(typeof<AStaticClass>)>]
    let VerifyPropertiesWithNullReturnValueThrows (t: Type) =
        let sut = ReturnValueMustNotBeNullAssertion(Fixture())
        let prop = t.GetProperty("NullReturnValueProperty")
        raises<ReturnValueMustNotBeNullException> <@ sut.Verify(prop) @>

    [<Theory>]
    [<InlineData(typeof<AClass>, "NullReturnValueMethod")>]
    [<InlineData(typeof<AClass>, "NullReturnValueMethodWithParameters")>]
    [<InlineData(typeof<AClass>, "NullReturnValueMethodWithManyParameters")>]
    [<InlineData(typeof<AClass>, "NullReturnValueMethodWithParametersAndBranching")>]
    [<InlineData(typeof<AStaticClass>, "NullReturnValueMethod")>]
    [<InlineData(typeof<AStaticClass>, "NullReturnValueMethodWithParameters")>]
    [<InlineData(typeof<AStaticClass>, "NullReturnValueMethodWithManyParameters")>]
    [<InlineData(typeof<AStaticClass>, "NullReturnValueMethodWithParametersAndBranching")>]
    let VerifyMethodsWithNullReturnValueThrows (t: Type, mName) =
        let sut = ReturnValueMustNotBeNullAssertion(Fixture())
        let method = t.GetMethod(mName)
        raises<ReturnValueMustNotBeNullException> <@ sut.Verify(method) @>

    [<Theory>]
    [<InlineData(typeof<AClass>)>]
    [<InlineData(typeof<AStaticClass>)>]
    let VerifyMethodsWithComplexParametersDoesNotThrow (t: Type) =
        let sut = ReturnValueMustNotBeNullAssertion(Fixture())
        let method = t.GetMethod("MethodWithComplexParameters")
        Arb.register<Generators>() |> ignore
        sut.Verify(method)

    [<Theory>]
    [<InlineData(typeof<AClass>, "NullReturnValueMethodWithComplexParameters")>]
    [<InlineData(typeof<AStaticClass>, "NullReturnValueMethodWithComplexParameters")>]
    let VerifyMethodsWithComplexParametersAndNullReturnValueThrows (t: Type, mName) =
        let sut = ReturnValueMustNotBeNullAssertion(Fixture())
        let method = t.GetMethod(mName)
        Arb.register<Generators>() |> ignore
        raises<ReturnValueMustNotBeNullException> <@ sut.Verify(method) @>
