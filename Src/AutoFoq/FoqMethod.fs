namespace Ploeh.AutoFixture.AutoFoq

open Foq
open Ploeh.AutoFixture.Kernel
open System
open System.Reflection

type private FoqMethod<'T when 'T : not struct>(parameterInfos) =
    interface IMethod with
        member this.Parameters = parameterInfos
        member this.Invoke parameters = Mock<'T>().Create(parameters |> Seq.toArray) :> obj

[<AbstractClass; Sealed>]
type private FoqMethod() =
    static member Create(targetType: Type, parameterInfos: ParameterInfo[]) = 
        Activator.CreateInstance(
            typedefof<FoqMethod<_>>
                .MakeGenericType(targetType), 
            parameterInfos)
    static member Create targetType = 
        FoqMethod.Create(targetType, Array.empty)