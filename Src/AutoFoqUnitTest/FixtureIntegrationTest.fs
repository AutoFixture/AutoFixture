module Ploeh.AutoFixture.AutoFoq.UnitTest.FixtureIntegrationTest

open Ploeh.AutoFixture
open Ploeh.AutoFixture.AutoFoq
open Ploeh.TestTypeFoundation
open System
open System.Collections.Generic
open Xunit

let private verify = Swensen.Unquote.Assertions.test

[<Fact>]
let FixtureAutoMocksInterface() =
    // Fixture setup
    let fixture = Fixture().Customize(AutoFoqCustomization())
    // Exercise system
    let result = fixture.Create<IInterface>()
    // Verify outcome
    verify <@ typeof<IInterface>.IsAssignableFrom(result.GetType()) @>
    // Teardown

[<Fact>]
let FixtureAutoMocksAbstractType() =
    // Fixture setup
    let fixture = Fixture().Customize(AutoFoqCustomization())
    // Exercise system
    let result = fixture.Create<AbstractType>()
    // Verify outcome
    verify <@ typeof<AbstractType>.IsAssignableFrom(result.GetType()) @>
    // Teardown

[<Fact>]
let FixtureAutoMocksAbstractGenericTypeWithNonDefaultConstructor() =
    // Fixture setup
    let fixture = Fixture().Customize(AutoFoqCustomization())
    // Exercise system
    let result = fixture.Create<AbstractGenericType<obj>>()
    // Verify outcome
    verify <@ typeof<AbstractGenericType<obj>>.IsAssignableFrom(result.GetType()) @>

[<Fact>]
let FixtureAutoMocksAbstractGenericTypeWithNonDefaultConstructorWithMultipleParameters() =
    // Fixture setup
    let fixture = Fixture().Customize(AutoFoqCustomization())
    // Exercise system
    let result = fixture.Create<AbstractTypeWithConstructorWithMultipleParameters<int, int>>()
    // Verify outcome
    verify 
        <@ typeof<AbstractTypeWithConstructorWithMultipleParameters<int, int>>
               .IsAssignableFrom(result.GetType()) @>

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
