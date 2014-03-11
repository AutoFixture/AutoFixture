module Ploeh.AutoFixture.Idioms.FsCheckUnitTest.ReturnValueMustNotBeNullAssertionTest

open Ploeh.AutoFixture
open Ploeh.AutoFixture.Idioms
open Ploeh.AutoFixture.Idioms.FsCheck
open Ploeh.AutoFixture.Idioms.FsCheckUnitTest.TestDsl
open Ploeh.AutoFixture.Kernel
open Xunit

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