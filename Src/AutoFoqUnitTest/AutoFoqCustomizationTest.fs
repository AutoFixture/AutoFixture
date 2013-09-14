module Ploeh.AutoFixture.AutoFoq.UnitTest.AutoFoqCustomizationTest

open Foq
open Ploeh.AutoFixture
open Ploeh.AutoFixture.AutoFoq
open Ploeh.AutoFixture.Kernel
open Xunit
open System
open System.Collections.Generic
 
[<Fact>]
let SutIsCustomization() =
    // Fixture setup
    // Exercise system
    let sut = AutoFoqCustomization()
    // Verify outcome
    Assert.IsAssignableFrom<ICustomization>(sut)
    // Teardown

[<Fact>]
let InitializeWithNullRelayThrows() =
    // Fixture setup
    // Exercise system and verify outcome
    Assert.Throws<ArgumentNullException>(fun () -> 
        AutoFoqCustomization(null) |> ignore)
    // Teardown

[<Fact>]
let RelayIsCorrect() =
    // Fixture setup
    let expectedBuilder = Mock<ISpecimenBuilder>().Create()
    let sut = AutoFoqCustomization(expectedBuilder)
    // Exercise system
    let result = sut.Relay
    // Verify outcome
    Assert.Equal(expectedBuilder, result)
    // Teardown

[<Fact>]
let RelayIsCorrectWhenInitializedWithDefaultConstructor() =
    // Fixture setup
    let sut = AutoFoqCustomization()
    // Exercise system
    let result = sut.Relay
    // Verify outcome
    Assert.IsType<FilteringSpecimenBuilder>(result)
    // Teardown

[<Fact>]
let CustomizeWithNullFixtureThrows() =
    // Fixture setup
    let sut = AutoFoqCustomization()
    // Exercise system and verify outcome
    Assert.Throws<ArgumentNullException>(fun () -> 
        sut.Customize(null))
    // Teardown

[<Fact>]
let CustomizeAddsAppropriateResidueCollector() =
    // Fixture setup
    let residueCollectors = List<ISpecimenBuilder>()
    let fixtureStub =
         Mock<IFixture>()
            .Setup(fun x -> <@ x.ResidueCollectors @>)
            .Returns(residueCollectors)
            .Create()
    let sut = AutoFoqCustomization()
    // Exercise system
    sut.Customize(fixtureStub)
    // Verify outcome
    Assert.Contains(sut.Relay, residueCollectors)
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
    Assert.Empty(customizations)
    // Teardown

