module Ploeh.AutoFixture.AutoFoq.UnitTest.AutoFoqCustomizationTest

open Ploeh.AutoFixture
open Ploeh.AutoFixture.AutoFoq
open Xunit
 
[<Fact>]
let SutIsCustomization() =
    // Fixture setup
    // Exercise system
    let sut = AutoFoqCustomization()
    // Verify outcome
    Assert.IsAssignableFrom<ICustomization>(sut)
    // Teardown
