module AutoFixture.AutoFoq.UnitTest.FixtureIntegrationTest

open AutoFixture
open AutoFixture.AutoFoq
open AutoFixture.AutoFoq.UnitTest.TestDsl
open AutoFixture.Kernel
open TestTypeFoundation
open System
open System.Collections.Generic
open Xunit

[<Fact>]
let FixtureAutoMocksInterface() =
    // Arrange
    let fixture = Fixture().Customize(AutoFoqCustomization())
    // Act
    let result = fixture.Create<IInterface>()
    // Assert
    verify <@ result |> implements<IInterface> @>

[<Fact>]
let FixtureAutoMocksAbstractType() =
    // Arrange
    let fixture = Fixture().Customize(AutoFoqCustomization())
    // Act
    let result = fixture.Create<AbstractType>()
    // Assert
    verify <@ result |> implements<AbstractType> @>

[<Fact>]
let FixtureAutoMocksAbstractGenericTypeWithNonDefaultConstructor() =
    // Arrange
    let fixture = Fixture().Customize(AutoFoqCustomization())
    // Act
    let result = fixture.Create<AbstractGenericType<obj>>()
    // Assert
    verify <@ result |> implements<AbstractGenericType<obj>> @>

[<Fact>]
let FixtureAutoMocksAbstractGenericTypeWithNonDefaultConstructorWithMultipleParameters() =
    // Arrange
    let fixture = Fixture().Customize(AutoFoqCustomization())
    // Act
    let result = fixture.Create<AbstractTypeWithConstructorWithMultipleParameters<int, int>>()
    // Assert
    verify <@ result |> implements<AbstractTypeWithConstructorWithMultipleParameters<int, int>> @>

[<Fact>]
let FixtureSuppliesValuesToAbstractGenericTypeWithNonDefaultConstructor() =
    // Arrange
    let fixture = Fixture().Customize(AutoFoqCustomization())
    // Act
    let result = fixture.Create<AbstractTypeWithNonDefaultConstructor<int>>()
    // Assert
    verify <@ not <| (Unchecked.defaultof<int> = result.Property) @>

[<Fact>]
let FixtureSuppliesValuesToAbstractGenericTypeWithNonDefaultConstructorWithMultipleParameters() =
    // Arrange
    let fixture = Fixture().Customize(AutoFoqCustomization())
    // Act
    let result = fixture.Create<AbstractTypeWithConstructorWithMultipleParameters<int, int>>()
    // Assert
    verify <@ not <| (Unchecked.defaultof<int> = result.Property1) @>
    verify <@ not <| (Unchecked.defaultof<int> = result.Property2) @>

type IInterface =
    abstract ReturnSomething : unit -> string

[<Fact>]
let ``Fixture supplies return values for non-explicitly setup test-doubles`` () =
    let sut = (Fixture().Customize(AutoFoqCustomization())).Create<IInterface>()
    let actual = sut.ReturnSomething()
    verify <@ not (isNull actual) @>

[<Fact>]
let ``Fixture customizations are propagated in Foq for non-explicitly setup test-doubles`` () =
    let fixture = Fixture()
    let expected = "Friday, 14 December 1984"
    fixture.Register<string>(fun () -> expected)
    fixture.Customize(AutoFoqCustomization()) |> ignore
    let sut = fixture.Create<IInterface>()

    let actual = sut.ReturnSomething()

    verify <@ expected = actual @>
