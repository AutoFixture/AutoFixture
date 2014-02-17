module Ploeh.AutoFixture.AutoFoq.UnitTest.FixtureIntegrationTest

open Ploeh.AutoFixture
open Ploeh.AutoFixture.AutoFoq
open Ploeh.AutoFixture.AutoFoq.UnitTest.TestDsl
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
    verify <@ implements<IInterface>(result) @>
    // Teardown

[<Fact>]
let FixtureAutoMocksAbstractType() =
    // Fixture setup
    let fixture = Fixture().Customize(AutoFoqCustomization())
    // Exercise system
    let result = fixture.Create<AbstractType>()
    // Verify outcome
    verify <@ implements<AbstractType>(result) @>
    // Teardown

[<Fact>]
let FixtureAutoMocksAbstractGenericTypeWithNonDefaultConstructor() =
    // Fixture setup
    let fixture = Fixture().Customize(AutoFoqCustomization())
    // Exercise system
    let result = fixture.Create<AbstractGenericType<obj>>()
    // Verify outcome
    verify <@ implements<AbstractGenericType<obj>>(result) @>

[<Fact>]
let FixtureAutoMocksAbstractGenericTypeWithNonDefaultConstructorWithMultipleParameters() =
    // Fixture setup
    let fixture = Fixture().Customize(AutoFoqCustomization())
    // Exercise system
    let result = fixture.Create<AbstractTypeWithConstructorWithMultipleParameters<int, int>>()
    // Verify outcome
    verify <@ implements<AbstractTypeWithConstructorWithMultipleParameters<int, int>>(result) @>

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
