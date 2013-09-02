namespace Ploeh.AutoFixture.AutoFoq

open Ploeh.AutoFixture.Kernel
open System
open System.Reflection

type FoqMethodQuery() =
    member this.SelectMethods targetType = 
        (this :> IMethodQuery).SelectMethods targetType
    interface IMethodQuery with
        member this.SelectMethods targetType = 
            match targetType with
            | null -> raise (ArgumentNullException("targetType"))
            |  _   -> match targetType.IsInterface with
                      | true  -> seq { yield FoqMethod.Create(targetType) :?> IMethod }
                      | _     -> targetType.GetConstructors(
                                        BindingFlags.Public 
                                    ||| BindingFlags.Instance 
                                    ||| BindingFlags.NonPublic)
                                 |> Seq.sortBy(fun x -> x.GetParameters().Length)
                                 |> Seq.map(fun ctor -> FoqMethod.Create(
                                                            targetType, 
                                                            ctor.GetParameters())
                                                        :?> IMethod)
