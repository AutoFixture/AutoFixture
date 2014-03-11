module Ploeh.AutoFixture.Idioms.FsCheckUnitTest.ReturnValueMustNotBeNullAssertionTest

open Ploeh.AutoFixture
open Ploeh.AutoFixture.Idioms
open Ploeh.AutoFixture.Idioms.FsCheck
open Ploeh.AutoFixture.Idioms.FsCheckUnitTest.TestDsl
open Ploeh.AutoFixture.Idioms.FsCheckUnitTest.TestTypes
open Ploeh.AutoFixture.Kernel
open Swensen.Unquote
open System
open System.Reflection
open Xunit
open Xunit.Extensions

let doesNotThrow = Xunit.Assert.DoesNotThrow

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

[<Theory; PropertyData("VoidMembers")>]
let VerifyVoidMembersDoesNotThrow (memberInfo : MemberInfo) =
    let dummyBuilder = Fixture()
    let sut = ReturnValueMustNotBeNullAssertion(dummyBuilder)
    doesNotThrow |> fun _ -> sut.Verify(memberInfo)

let VoidMembers : seq<MemberInfo[]> = seq { 
    yield [| typeof<AClass>.GetMethod("VoidMethod") |]
    yield [| typeof<AStaticClass>.GetMethod("VoidMethod") |]
    yield [| typeof<AClass>.GetProperty("WriteOnlyProperty") |]
    yield [| typeof<AStaticClass>.GetProperty("WriteOnlyProperty") |] }