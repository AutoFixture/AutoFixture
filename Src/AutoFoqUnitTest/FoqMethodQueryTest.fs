module Ploeh.AutoFixture.AutoFoq.UnitTest.FoqMethodQueryTest

open Ploeh.AutoFixture.Kernel
open Ploeh.AutoFixture.AutoFoq
open Ploeh.TestTypeFoundation
open System
open System.Linq
open Xunit

[<Fact>]
let SutIsMethodQuery() =
    // Fixture setup
    // Exercise system
    let sut = FoqMethodQuery()
    // Verify outcome
    Assert.IsAssignableFrom<IMethodQuery>(sut)
    // Teardown

[<Fact>]
let SelectMethodThrowsForNullType() =
    // Fixture setup
    let requestType = typeof<IInterface>
    let sut = FoqMethodQuery()
    // Exercise system and verify outcome
    Assert.Throws<ArgumentNullException>(fun () -> 
        sut.SelectMethods(null)|> ignore)

[<Fact>]
let SelectMethodReturnsMethodForInterface() =
    // Fixture setup
    let requestType = typeof<IInterface>
    let sut = FoqMethodQuery()
    // Exercise system
    let result = sut.SelectMethods(requestType)
    // Verify outcome
    Assert.IsAssignableFrom<IMethod[]>(result)

[<Fact>]
let SelectMethodReturnsMethodWithoutParametersForInterface() =
    // Fixture setup
    let requestType = typeof<IInterface>
    let sut = FoqMethodQuery()
    // Exercise system
    let result = sut.SelectMethods(requestType).First().Parameters
    // Verify outcome
    Assert.Empty(result)
