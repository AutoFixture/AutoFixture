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
    [<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "This method is examined and instantiated with Reflection.")>]
    static member Create(targetType: Type, parameterInfos: ParameterInfo[]) = 
        Activator.CreateInstance(
            typedefof<FoqMethod<_>>
                .MakeGenericType(targetType), 
            parameterInfos)
    [<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "This method is examined and instantiated with Reflection.")>]
    static member Create targetType = 
        FoqMethod.Create(targetType, Array.empty)