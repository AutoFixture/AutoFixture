module Ploeh.AutoFixture.AutoFoq.UnitTest.FoqMethodQueryTest

open Ploeh.AutoFixture.Kernel
open Ploeh.AutoFixture.AutoFoq
open System
open Xunit

[<Fact>]
let SutIsMethodQuery() =
    // Fixture setup
    // Exercise system
    let sut = FoqMethodQuery()
    // Verify outcome
    Assert.IsAssignableFrom<IMethodQuery>(sut)
    // Teardown
