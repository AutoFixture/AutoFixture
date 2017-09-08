module Ploeh.AutoFixture.AutoFoq.UnitTest.FixtureIntegrationTest

open Ploeh.AutoFixture
open Ploeh.AutoFixture.AutoFoq
open Ploeh.AutoFixture.AutoFoq.UnitTest.TestDsl
open Ploeh.AutoFixture.Kernel
open Ploeh.TestTypeFoundation
open System
open System.Collections.Generic
open Xunit

[<Fact>]
let FixtureAutoMocksInterface() =
    // Fixture setup
    let fixture = Fixture().Customize(AutoFoqCustomization())
    // Exercise system
    let result = fixture.Create<IInterface>()
    // Verify outcome
    verify <@ result |> implements<IInterface> @>
    // Teardown

[<Fact>]
let FixtureAutoMocksAbstractType() =
    // Fixture setup
    let fixture = Fixture().Customize(AutoFoqCustomization())
    // Exercise system
    let result = fixture.Create<AbstractType>()
    // Verify outcome
    verify <@ result |> implements<AbstractType> @>
    // Teardown

[<Fact>]
let FixtureAutoMocksAbstractGenericTypeWithNonDefaultConstructor() =
    // Fixture setup
    let fixture = Fixture().Customize(AutoFoqCustomization())
    // Exercise system
    let result = fixture.Create<AbstractGenericType<obj>>()
    // Verify outcome
    verify <@ result |> implements<AbstractGenericType<obj>> @>

[<Fact>]
let FixtureAutoMocksAbstractGenericTypeWithNonDefaultConstructorWithMultipleParameters() =
    // Fixture setup
    let fixture = Fixture().Customize(AutoFoqCustomization())
    // Exercise system
    let result = fixture.Create<AbstractTypeWithConstructorWithMultipleParameters<int, int>>()
    // Verify outcome
    verify <@ result |> implements<AbstractTypeWithConstructorWithMultipleParameters<int, int>> @>

[<Fact>]
let FixtureSuppliesValuesToAbstractGenericTypeWithNonDefaultConstructor() =
    // Fixture setup
    let fixture = Fixture().Customize(AutoFoqCustomization())
    // Exercise system
    let result = fixture.Create<AbstractTypeWithNonDefaultConstructor<int>>()
    // Verify outcome
    verify <@ not <| (Unchecked.defaultof<int> = result.Property) @>
    // Teardown

[<Fact>]
let FixtureSuppliesValuesToAbstractGenericTypeWithNonDefaultConstructorWithMultipleParameters() =
    // Fixture setup
    let fixture = Fixture().Customize(AutoFoqCustomization())
    // Exercise system
    let result = fixture.Create<AbstractTypeWithConstructorWithMultipleParameters<int, int>>()
    // Verify outcome
    verify <@ not <| (Unchecked.defaultof<int> = result.Property1) @>
    verify <@ not <| (Unchecked.defaultof<int> = result.Property2) @>
    // Teardown

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
