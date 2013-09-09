module Ploeh.AutoFixture.AutoFoq.UnitTest.FoqRelayTest
 
open Foq
open Ploeh.AutoFixture.Kernel
open Ploeh.AutoFixture.AutoFoq
open Ploeh.TestTypeFoundation
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
let InitializeWithNullBuilderThrows() =
    // Fixture setup
    let dummySpecification = Mock<IRequestSpecification>().Create();
    // Exercise system and verify outcome
    Assert.Throws<ArgumentNullException>(fun () -> 
        FoqRelay(null, dummySpecification) |> ignore)
    // Teardown

[<Fact>]
let InitializeWithNullSpecificationThrows() =
    // Fixture setup
    let dummyBuilder = Mock<ISpecimenBuilder>().Create()
    // Exercise system and verify outcome
    Assert.Throws<ArgumentNullException>(fun () -> 
        FoqRelay(dummyBuilder, null) |> ignore)
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
    let expectedSpecification = TrueRequestSpecification()
    let sut = FoqRelay(dummyBuilder, expectedSpecification)
    // Exercise system
    let result = sut.Specification :?> TrueRequestSpecification
    // Verify outcome
    Assert.Equal(expectedSpecification, result)
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

[<Fact>]
let CreateWithRequestThatDoesNotMatchSpecificationReturnsNoSpecimen() =
    // Fixture setup
    let dummyBuilder = Mock<ISpecimenBuilder>().Create()
    let specification = FalseRequestSpecification()
    let sut = FoqRelay(dummyBuilder, specification)
    let request = typedefof<ConcreteType>
    let dummyContext = Mock<ISpecimenContext>().Create()
    // Exercise system
    let result = sut.Create(request, dummyContext) :?> NoSpecimen
    // Verify outcome
    let expected = NoSpecimen(request)
    Assert.Equal(expected, result)

[<Theory>][<PropertyData("Abstractions")>]
let CreateWithAbstractTypeRequestReturnsNoSpecimenWhenBuilderReturnsNull (request: Type) =
    // Fixture setup
    let dummyContext = Mock<ISpecimenContext>().Create()
    let builderStub = 
        Mock<ISpecimenBuilder>()
            .Setup(fun x -> <@ x.Create(request, dummyContext) @>)
            .Returns(null)
            .Create()
    let sut = FoqRelay(builderStub)
    // Exercise system
    let result = sut.Create(request, dummyContext) :?> NoSpecimen
    // Verify outcome
    let expected = NoSpecimen(request)
    Assert.Equal(expected, result)

[<Theory>][<PropertyData("Abstractions")>]
let CreateWithAbstractTypeRequestReturnsCorrectResult (request: Type) =
    // Fixture setup
    let dummyContext = Mock<ISpecimenContext>().Create()
    let expected = obj()
    let builderStub = 
        Mock<ISpecimenBuilder>()
            .Setup(fun x -> <@ x.Create(request, dummyContext) @>)
            .Returns(expected)
            .Create()
    let sut = FoqRelay(builderStub)
    // Exercise system
    let result = sut.Create(request, dummyContext)
    // Verify outcome
    Assert.Same(expected, result)

let Abstractions : seq<Type[]> = 
    seq { 
            yield [| typeof<IInterface> |]
            yield [| typeof<AbstractType> |]
        }