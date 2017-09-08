namespace Ploeh.AutoFixture.Idioms.FsCheckUnitTest

open Grean.Exude
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

    [<FirstClassTests>]
    let VerifyWriteOnlyPropertiesDoesNotThrow () =
        let sut = ReturnValueMustNotBeNullAssertion(Fixture())
        [   
            typeof<AClass>.GetProperty("WriteOnlyProperty")
            typeof<AStaticClass>.GetProperty("WriteOnlyProperty")
        ]
        |> Seq.map (fun element -> TestCase (fun () -> sut.Verify(element)))

    [<FirstClassTests>]
    let VerifyVoidMethodsDoesNotThrow () =
        let sut = ReturnValueMustNotBeNullAssertion(Fixture())
        [   
            typeof<AClass>.GetMethod("VoidMethod")
            typeof<AStaticClass>.GetMethod("VoidMethod")
        ]
        |> Seq.map (fun element -> TestCase (fun () -> sut.Verify(element)))

    [<FirstClassTests>]
    let VerifyPropertiesWithReturnValueDoesNotThrow () =
        let sut = ReturnValueMustNotBeNullAssertion(Fixture())
        [   
            typeof<AClass>.GetProperty("ReadOnlyProperty")
            typeof<AStaticClass>.GetProperty("ReadOnlyProperty")
        ]
        |> Seq.map (fun element -> TestCase (fun () -> sut.Verify(element)))

    [<FirstClassTests>]
    let VerifyMethodsWithReturnValueDoesNotThrow () =
        let sut = ReturnValueMustNotBeNullAssertion(Fixture())
        [
            typeof<AClass>.GetMethod("MethodWithReturnValue")
            typeof<AStaticClass>.GetMethod("MethodWithReturnValue")
        ]
        |> Seq.map (fun element -> TestCase (fun () -> sut.Verify(element)))

    [<FirstClassTests>]
    let VerifyPropertiesWithNullReturnValueThrows () =
        let sut = ReturnValueMustNotBeNullAssertion(Fixture())
        [
            typeof<AClass>.GetProperty("NullReturnValueProperty")
            typeof<AStaticClass>.GetProperty("NullReturnValueProperty")
        ]
        |> Seq.map (fun element -> TestCase (fun () ->
            raises<ReturnValueMustNotBeNullException> <@ sut.Verify(element) @>))

    [<FirstClassTests>]
    let VerifyMethodsWithNullReturnValueThrows () =
        let sut = ReturnValueMustNotBeNullAssertion(Fixture())
        [
            typeof<AClass>.GetMethod("NullReturnValueMethod")
            typeof<AClass>.GetMethod("NullReturnValueMethodWithParameters")
            typeof<AClass>.GetMethod("NullReturnValueMethodWithManyParameters")
            typeof<AClass>.GetMethod("NullReturnValueMethodWithParametersAndBranching")

            typeof<AStaticClass>.GetMethod("NullReturnValueMethod")
            typeof<AStaticClass>.GetMethod("NullReturnValueMethodWithParameters")
            typeof<AStaticClass>.GetMethod("NullReturnValueMethodWithManyParameters")
            typeof<AStaticClass>.GetMethod("NullReturnValueMethodWithParametersAndBranching")
        ]
        |> Seq.map (fun element -> TestCase (fun () ->
            raises<ReturnValueMustNotBeNullException> <@ sut.Verify(element) @>))

    [<FirstClassTests>]
    let VerifyMethodsWithComplexParametersDoesNotThrow () =
        let sut = ReturnValueMustNotBeNullAssertion(Fixture())
        [
            typeof<AClass>.GetMethod("MethodWithComplexParameters")
            typeof<AStaticClass>.GetMethod("MethodWithComplexParameters")
        ]
        |> Seq.map (fun element -> TestCase (fun () ->
            Arb.register<Generators>() |> ignore
            sut.Verify(element)))

    [<FirstClassTests>]
    let VerifyMethodsWithComplexParametersAndNullReturnValueThrows () =
        let sut = ReturnValueMustNotBeNullAssertion(Fixture())
        [
            typeof<AClass>.GetMethod("NullReturnValueMethodWithComplexParameters")
            typeof<AStaticClass>.GetMethod("NullReturnValueMethodWithComplexParameters")
        ]
        |> Seq.map (fun element -> TestCase (fun () ->
            Arb.register<Generators>() |> ignore
            raises<ReturnValueMustNotBeNullException> <@ sut.Verify(element) @>))
