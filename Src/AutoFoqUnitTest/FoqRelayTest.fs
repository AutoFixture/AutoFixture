module Ploeh.AutoFixture.AutoFoq.UnitTest.FoqRelayTest
 
open Foq
open Ploeh.AutoFixture.Kernel
open Ploeh.AutoFixture.AutoFoq
open System
open Xunit
open Xunit.Extensions
 
[<Fact>]
let SutIsSpecimenBuilder() =
    // Exercise system
    let dummyBuilder = Mock<ISpecimenBuilder>().Create()
    let sut = FoqRelay(dummyBuilder)
    // Verify outcome
    Assert.IsAssignableFrom<ISpecimenBuilder>(sut)
    // Teardown

[<Fact>]
let BuilderIsCorrect() =
    // Fixture setup
    let expectedBuilder = Mock<ISpecimenBuilder>().Create()
    let sut = FoqRelay(expectedBuilder)
    // Exercise system
    let result = sut.Builder
    // Verify outcome
    Assert.Equal(expectedBuilder, result)
    // Teardown

[<Fact>]
let SpecificationIsCorrect() =
    // Fixture setup
    let dummyBuilder = Mock<ISpecimenBuilder>().Create()
    let specification = TrueRequestSpecification()
    let sut = FoqRelay(dummyBuilder, specification)
    // Exercise system
    let result = sut.Specification
    // Verify outcome
    Assert.Equal(specification, result :?> TrueRequestSpecification)
    // Teardown

[<Fact>]
let InitializedWithDefaultConstructorHasCorrectSpecification() =
    // Fixture setup
    let dummyBuilder = Mock<ISpecimenBuilder>().Create()
    let sut = FoqRelay(dummyBuilder)
    // Exercise system
    let result = sut.Specification
    // Verify outcome
    Assert.IsType<AbstractTypeSpecification>(result)
    // Teardown