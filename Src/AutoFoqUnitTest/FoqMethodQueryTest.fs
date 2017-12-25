module AutoFixture.AutoFoq.UnitTest.FoqMethodQueryTest

open AutoFixture.Kernel
open AutoFixture.AutoFoq
open AutoFixture.AutoFoq.UnitTest.TestDsl
open TestTypeFoundation
open System
open System.Reflection
open Swensen.Unquote.Assertions
open Xunit

let dummyBuilder =
    { new ISpecimenBuilder with
            member this.Create(r, c) = obj() }

[<Fact>]
let SutIsMethodQuery() =
    // Arrange
    // Act
    let sut = FoqMethodQuery dummyBuilder
    // Assert
    verify <@ sut |> implements<IMethodQuery> @>

[<Fact>]
let SelectMethodThrowsForNullType() =
    // Arrange
    let sut = FoqMethodQuery dummyBuilder
    // Act & Assert
    raises<ArgumentNullException> <@ sut.SelectMethods(null) @>

[<Fact>]
let SelectMethodReturnsMethodForInterface() =
    // Arrange
    let requestType = typeof<IInterface>
    let sut = FoqMethodQuery dummyBuilder
    // Act
    let result = sut.SelectMethods(requestType)
    // Assert
    verify <@ result |> implements<seq<IMethod>> @>

[<Fact>]
let SelectMethodReturnsMethodWithoutParametersForInterface() =
    // Arrange
    let requestType = typeof<IInterface>
    let sut = FoqMethodQuery dummyBuilder
    // Act
    let result = (sut.SelectMethods(requestType) |> Seq.head).Parameters
    // Assert
    verify <@ result |> Seq.isEmpty @>

[<Theory>][<MemberData("TypesWithConstructors")>]
let MethodsAreReturnedInCorrectOrder (request: Type) =
    // Arrange
    let expected = 
        request.GetConstructors(
                BindingFlags.Public 
            ||| BindingFlags.Instance
            ||| BindingFlags.NonPublic) 
        |> Seq.sortBy(fun ci -> ci.GetParameters().Length) 
        |> Seq.map(fun ci -> ci.GetParameters().Length)
    let sut = FoqMethodQuery dummyBuilder
    // Act
    let result = 
        sut.SelectMethods(request)
        |> Seq.map(fun ci -> ci.Parameters |> Seq.length)
    // Assert
    verify <@ (expected, result) ||> Seq.forall2 (=) @>

[<Theory>][<MemberData("TypesWithConstructors")>]
let SelectMethodsDefineCorrectParameters (request: Type) =
    // Arrange
    let expected =
        request.GetConstructors(
                BindingFlags.Public 
            ||| BindingFlags.Instance
            ||| BindingFlags.NonPublic)         
        |> Seq.map(fun ci -> ci.GetParameters())
    let sut = FoqMethodQuery dummyBuilder
    // Act
    let result = 
        sut.SelectMethods(request)
        |> Seq.map(fun ci -> ci.Parameters)
    // Assert
    verify
        <@ expected |> Seq.forall(fun expectedParameters -> 
               result |> Seq.exists(fun resultParameters -> 
                   expectedParameters = 
                       (resultParameters |> Seq.toArray))) @>

[<Fact>]
let ``Builder is correct`` () =
    let expected =
        { new ISpecimenBuilder with
              member this.Create(r, c) = obj() }
    let sut = FoqMethodQuery expected

    let actual = sut.Builder

    verify <@ expected = actual @>

let TypesWithConstructors : seq<Type[]> = 
    seq {
            yield [| typeof<AbstractType> |]
            yield [| typeof<ConcreteType> |]
            yield [| typeof<MultiUnorderedConstructorType> |]
        }
