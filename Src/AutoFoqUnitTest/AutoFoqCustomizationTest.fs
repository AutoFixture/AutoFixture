module AutoFixture.AutoFoq.UnitTest.AutoFoqCustomizationTest

open Foq
open AutoFixture
open AutoFixture.AutoFoq
open AutoFixture.AutoFoq.UnitTest.TestDsl
open AutoFixture.Kernel
open Swensen.Unquote.Assertions
open Xunit
open System
open System.Collections.Generic

[<Fact>]
let SutIsCustomization() =
    // Arrange
    // Act
    let sut = AutoFoqCustomization()
    // Assert
    verify <@ sut |> implements<ICustomization> @>

[<Fact>]
let CustomizeWithNullFixtureThrows() =
    // Arrange
    let sut = AutoFoqCustomization()
    // Act & Assert
    raises<ArgumentNullException> <@ sut.Customize(null) @>

[<Fact>]
let CustomizeDoesNotAddCustomizations() =
    // Arrange
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
    // Act
    sut.Customize(fixtureStub)
    // Assert
    verify <@ customizations |> Seq.isEmpty @>
