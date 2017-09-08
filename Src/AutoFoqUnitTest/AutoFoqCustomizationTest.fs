module Ploeh.AutoFixture.AutoFoq.UnitTest.AutoFoqCustomizationTest

open Foq
open Ploeh.AutoFixture
open Ploeh.AutoFixture.AutoFoq
open Ploeh.AutoFixture.AutoFoq.UnitTest.TestDsl
open Ploeh.AutoFixture.Kernel
open Swensen.Unquote.Assertions
open Xunit
open System
open System.Collections.Generic

[<Fact>]
let SutIsCustomization() =
    // Fixture setup
    // Exercise system
    let sut = AutoFoqCustomization()
    // Verify outcome
    verify <@ sut |> implements<ICustomization> @>
    // Teardown

[<Fact>]
let CustomizeWithNullFixtureThrows() =
    // Fixture setup
    let sut = AutoFoqCustomization()
    // Exercise system and verify outcome
    raises<ArgumentNullException> <@ sut.Customize(null) @>
    // Teardown

[<Fact>]
let CustomizeDoesNotAddCustomizations() =
    // Fixture setup
    let customizations = List<ISpecimenBuilder>()
    let dummyResidueCollectors = List<ISpecimenBuilder>()
    let fixtureStub =
         Mock<IFixture>()
            .Setup(fun x -> <@ x.Customizations @>)
            .Returns(customizations)
            .Setup(fun x -> <@ x.ResidueCollectors @>)
            .Returns(dummyResidueCollectors)
            .Create()
    let sut = AutoFoqCustomization()
    // Exercise system
    sut.Customize(fixtureStub)
    // Verify outcome
    verify <@ customizations |> Seq.isEmpty @>
    // Teardown

