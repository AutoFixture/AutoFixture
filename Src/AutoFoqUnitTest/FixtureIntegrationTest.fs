module Ploeh.AutoFixture.AutoFoq.UnitTest.FixtureIntegrationTest

open Ploeh.AutoFixture
open Ploeh.AutoFixture.AutoFoq
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
    Assert.IsAssignableFrom<IInterface>(result)
    // Teardown

[<Fact>]
let FixtureAutoMocksAbstractType() =
    // Fixture setup
    let fixture = Fixture().Customize(AutoFoqCustomization())
    // Exercise system
    let result = fixture.Create<AbstractType>()
    // Verify outcome
    Assert.IsAssignableFrom<AbstractType>(result)
    // Teardown

[<Fact>]
let FixtureAutoMocksAbstractGenericTypeWithNonDefaultConstructor() =
    // Fixture setup
    let fixture = Fixture().Customize(AutoFoqCustomization())
    // Exercise system
    let result = fixture.Create<AbstractGenericType<obj>>()
    // Verify outcome
    Assert.IsAssignableFrom<AbstractGenericType<obj>>(result)

[<Fact>]
let FixtureAutoMocksAbstractGenericTypeWithNonDefaultConstructorWithMultipleParameters() =
    // Fixture setup
    let fixture = Fixture().Customize(AutoFoqCustomization())
    // Exercise system
    let result = fixture.Create<AbstractTypeWithConstructorWithMultipleParameters<int, int>>()
    // Verify outcome
    Assert.IsAssignableFrom<AbstractTypeWithConstructorWithMultipleParameters<int, int>>(result)

[<Fact>]
let FixtureSuppliesValuesToAbstractGenericTypeWithNonDefaultConstructor() =
    // Fixture setup
    let fixture = Fixture().Customize(AutoFoqCustomization())
    // Exercise system
    let result = fixture.Create<AbstractTypeWithNonDefaultConstructor<int>>()
    // Verify outcome
    Assert.NotEqual(Unchecked.defaultof<int>, result.Property)
    // Teardown

[<Fact>]
let FixtureSuppliesValuesToAbstractGenericTypeWithNonDefaultConstructorWithMultipleParameters() =
    // Fixture setup
    let fixture = Fixture().Customize(AutoFoqCustomization())
    // Exercise system
    let result = fixture.Create<AbstractTypeWithConstructorWithMultipleParameters<int, int>>()
    // Verify outcome
    Assert.NotEqual(Unchecked.defaultof<int>, result.Property1)
    Assert.NotEqual(Unchecked.defaultof<int>, result.Property2)
    // Teardown
