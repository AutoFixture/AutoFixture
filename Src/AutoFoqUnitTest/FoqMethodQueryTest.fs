module Ploeh.AutoFixture.AutoFoq.UnitTest.FoqMethodQueryTest

open Ploeh.AutoFixture.Kernel
open Ploeh.AutoFixture.AutoFoq
open Ploeh.TestTypeFoundation
open System
open System.Reflection
open Swensen.Unquote.Assertions
open Xunit
open Xunit.Extensions

let private verify = Swensen.Unquote.Assertions.test

[<Fact>]
let SutIsMethodQuery() =
    // Fixture setup
    // Exercise system
    let sut = FoqMethodQuery()
    // Verify outcome
    verify <@ typeof<IMethodQuery>.IsAssignableFrom(sut.GetType()) @>
    // Teardown

[<Fact>]
let SelectMethodThrowsForNullType() =
    // Fixture setup
    let sut = FoqMethodQuery()
    // Exercise system and verify outcome
    raises<ArgumentNullException> <@ sut.SelectMethods(null)|> ignore @>

[<Fact>]
let SelectMethodReturnsMethodForInterface() =
    // Fixture setup
    let requestType = typeof<IInterface>
    let sut = FoqMethodQuery()
    // Exercise system
    let result = sut.SelectMethods(requestType)
    // Verify outcome
    verify <@ typeof<seq<IMethod>>.IsAssignableFrom(result.GetType()) @>

[<Fact>]
let SelectMethodReturnsMethodWithoutParametersForInterface() =
    // Fixture setup
    let requestType = typeof<IInterface>
    let sut = FoqMethodQuery()
    // Exercise system
    let result = (sut.SelectMethods(requestType) |> Seq.head).Parameters
    // Verify outcome
    verify <@ result |> Seq.isEmpty @>

[<Theory>][<PropertyData("TypesWithConstructors")>]
let MethodsAreReturnedInCorrectOrder (request: Type) =
    // Fixture setup
    let expected = 
        request.GetConstructors(
                BindingFlags.Public 
            ||| BindingFlags.Instance
            ||| BindingFlags.NonPublic) 
        |> Seq.sortBy(fun ci -> ci.GetParameters().Length) 
        |> Seq.map(fun ci -> ci.GetParameters().Length)
    let sut = FoqMethodQuery()
    // Exercise system
    let result = 
        sut.SelectMethods(request)
        |> Seq.map(fun ci -> ci.Parameters |> Seq.length)
    // Verify outcome
    verify <@ (expected, result) ||> Seq.forall2 (=) @>
    // Teardown   

[<Theory>][<PropertyData("TypesWithConstructors")>]
let SelectMethodsDefineCorrectParameters (request: Type) =
    // Fixture setup
    let expected =
        request.GetConstructors(
                BindingFlags.Public 
            ||| BindingFlags.Instance
            ||| BindingFlags.NonPublic)         
        |> Seq.map(fun ci -> ci.GetParameters())
    let sut = FoqMethodQuery()
    // Exercise system
    let result = 
        sut.SelectMethods(request)
        |> Seq.map(fun ci -> ci.Parameters)
    // Verify outcome
    verify
        <@ expected |> Seq.forall(fun expectedParameters -> 
               result |> Seq.exists(fun resultParameters -> 
                   expectedParameters = 
                       (resultParameters |> Seq.toArray))) @>
    // Teardown

let TypesWithConstructors : seq<Type[]> = 
    seq {
            yield [| typeof<AbstractType> |]
            yield [| typeof<ConcreteType> |]
            yield [| typeof<MultiUnorderedConstructorType> |]
        }
