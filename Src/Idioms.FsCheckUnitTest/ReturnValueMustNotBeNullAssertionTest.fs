module Ploeh.AutoFixture.Idioms.FsCheckUnitTest.ReturnValueMustNotBeNullAssertionTest

open Ploeh.AutoFixture
open Ploeh.AutoFixture.Idioms
open Ploeh.AutoFixture.Idioms.FsCheck
open Ploeh.AutoFixture.Idioms.FsCheckUnitTest.TestDsl
open Xunit

[<Fact>]
let SutIsIdiomaticAssertion =
    let sut = ReturnValueMustNotBeNullAssertion();
    verify <@ sut |> implements<IdiomaticAssertion> @>
    