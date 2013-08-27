namespace Ploeh.AutoFixture.AutoFoq

open Foq
open Ploeh.AutoFixture.Kernel
open System
open System.Linq
open System.Reflection

type private FoqMethod<'T when 'T : not struct>(parameterInfos) =
    interface IMethod with
        member this.Parameters = parameterInfos
        member this.Invoke parameters = obj()

[<AbstractClass; Sealed>]
type private FoqMethod() =
    static member Create(targetType: Type, parameterInfos: ParameterInfo[]) = 
        Activator.CreateInstance(
            typedefof<FoqMethod<_>>
                .MakeGenericType(targetType), 
            parameterInfos)
    static member Create targetType = 
        FoqMethod.Create(targetType, Array.empty)

type FoqMethodQuery() =
    member this.SelectMethods targetType = 
        (this :> IMethodQuery).SelectMethods targetType
    interface IMethodQuery with
        member this.SelectMethods targetType = 
            match targetType with
            | null -> raise (ArgumentNullException("targetType"))
            |  _   -> match targetType.IsInterface with
                      | true  -> [| FoqMethod.Create(targetType) :?> IMethod |].AsEnumerable()
                      | false -> Enumerable.Empty<IMethod>()



