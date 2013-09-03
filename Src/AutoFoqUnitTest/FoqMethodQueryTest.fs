module Ploeh.AutoFixture.AutoFoq.UnitTest.FoqMethodQueryTest

open Ploeh.AutoFixture.Kernel
open Ploeh.AutoFixture.AutoFoq
open Ploeh.TestTypeFoundation
open System
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
    Assert.IsAssignableFrom<seq<IMethod>>(result)
    |> ignore

[<Fact>]
let SelectMethodReturnsMethodWithoutParametersForInterface() =
    // Fixture setup
    let requestType = typeof<IInterface>
    let sut = FoqMethodQuery()
    // Exercise system
    let result = (sut.SelectMethods(requestType) |> Seq.head).Parameters
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
        |> Seq.map(fun ci -> ci.Parameters |> Seq.length)
    // Verify outcome
    let compareSequences = Seq.compareWith Operators.compare
    Assert.True((compareSequences expected result = 0))
    // Teardown   

[<Theory>][<PropertyData("TypesWithConstructors")>]
let SelectMethodsDefineCorrectParameters requestTypeName =
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
        expected |> Seq.forall(fun expectedParameters -> 
            result |> Seq.exists(fun resultParameters -> 
                expectedParameters = 
                    (resultParameters |> Seq.toArray))))
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
    let result = sut.SelectMethods(request) |> Seq.length
    // Verify outcome
    Assert.Equal(expected, result)
    // Teardown

let TypesWithConstructors : seq<obj[]> = 
    seq {
            yield [| "Ploeh.TestTypeFoundation.AbstractType" |]
            yield [| "Ploeh.TestTypeFoundation.ConcreteType" |]
            yield [| "Ploeh.TestTypeFoundation.MultiUnorderedConstructorType" |]
        }
