module Ploeh.AutoFixture.AutoFoq.UnitTest.FoqMethodQueryTest

open Ploeh.AutoFixture.Kernel
open Ploeh.AutoFixture.AutoFoq
open Ploeh.TestTypeFoundation
open System
open System.Linq
open System.Reflection
open Xunit
open Xunit.Extensions

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

[<Theory>][<PropertyData("TypesWithConstructors")>]
let MethodsAreReturnedInCorrectOrder requestTypeName =
    // Fixture setup
    let request = typeof<IInterface>.Assembly.GetType(requestTypeName)
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
        |> Seq.map(fun ci -> ci.Parameters.Count())
    // Verify outcome
    Assert.True(expected.SequenceEqual(result))
    // Teardown   

[<Theory>][<PropertyData("TypesWithConstructors")>]
let MethodsDefineCorrectParameters requestTypeName =
    // Fixture setup
    let request = typeof<IInterface>.Assembly.GetType(requestTypeName)
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
    Assert.True(
        expected.All(fun expectedParameters -> 
            result.Any(fun resultParameters -> 
                expectedParameters.SequenceEqual(
                    resultParameters))))
    // Teardown

[<Theory>][<PropertyData("TypesWithConstructors")>]
let SelectMethodsReturnsCorrectNumberOfConstructorsForTypesWithConstructors 
    requestTypeName =
    // Fixture setup
    let request = typeof<IInterface>.Assembly.GetType(requestTypeName)
    let expected = 
        request
            .GetConstructors(
                BindingFlags.Public 
            ||| BindingFlags.Instance
            ||| BindingFlags.NonPublic)             
            .Length
    let sut = FoqMethodQuery()
    // Exercise system
    let result = sut.SelectMethods(request)
    // Verify outcome
    Assert.Equal(expected, result.Count())
    // Teardown

let TypesWithConstructors : seq<obj[]> = 
    seq {
            yield [| "Ploeh.TestTypeFoundation.AbstractType" |]
            yield [| "Ploeh.TestTypeFoundation.ConcreteType" |]
            yield [| "Ploeh.TestTypeFoundation.MultiUnorderedConstructorType" |]
        }
