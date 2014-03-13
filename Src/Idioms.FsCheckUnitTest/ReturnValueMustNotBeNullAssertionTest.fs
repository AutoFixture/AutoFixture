namespace Ploeh.AutoFixture.Idioms.FsCheckUnitTest

open Grean.Exude
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
    let VerifyVoidMembersDoesNotThrow () =
        let sut = ReturnValueMustNotBeNullAssertion(Fixture())
        [   
            typeof<AClass>.GetMethod("VoidMethod") :> MemberInfo
            typeof<AStaticClass>.GetMethod("VoidMethod") :> MemberInfo
            typeof<AClass>.GetProperty("WriteOnlyProperty") :> MemberInfo
            typeof<AStaticClass>.GetProperty("WriteOnlyProperty") :> MemberInfo 
        ]
        |> Seq.map (fun element -> TestCase (fun _ -> sut.Verify(element)))

    [<FirstClassTests>]
    let VerifyMembersWithReturnValueDoesNotThrow () =
        let sut = ReturnValueMustNotBeNullAssertion(Fixture())
        [   
            typeof<AClass>.GetProperty("ReadOnlyProperty") :> MemberInfo
            typeof<AClass>.GetMethod("MethodWithReturnValue") :> MemberInfo
            typeof<AStaticClass>.GetProperty("ReadOnlyProperty") :> MemberInfo
            typeof<AStaticClass>.GetMethod("MethodWithReturnValue") :> MemberInfo 
        ]
        |> Seq.map (fun element -> TestCase (fun _ -> sut.Verify(element)))

    [<FirstClassTests>]
    let VerifyMembersWithNullReturnValueThrows () =
        let sut = ReturnValueMustNotBeNullAssertion(Fixture())
        [   
            typeof<AClass>.GetMethod("NullReturnValueMethod") :> MemberInfo
            typeof<AClass>.GetProperty("NullReturnValueProperty") :> MemberInfo
            typeof<AStaticClass>.GetMethod("NullReturnValueMethod") :> MemberInfo
            typeof<AStaticClass>.GetProperty("NullReturnValueProperty") :> MemberInfo
            typeof<AClass>.GetMethod("NullReturnValueMethodWithParameters") :> MemberInfo
            typeof<AStaticClass>.GetMethod("NullReturnValueMethodWithParameters") :> MemberInfo
            typeof<AClass>.GetMethod("NullReturnValueMethodWithParametersAndBranching") :> MemberInfo
            typeof<AStaticClass>.GetMethod("NullReturnValueMethodWithParametersAndBranching") :> MemberInfo
        ]
        |> Seq.map (fun element -> TestCase (fun _ -> 
            raises<ReturnValueMustNotBeNullException> <@ sut.Verify(element) @>))
