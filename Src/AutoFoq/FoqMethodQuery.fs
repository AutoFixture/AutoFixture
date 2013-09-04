namespace Ploeh.AutoFixture.AutoFoq

open Ploeh.AutoFixture.AutoFoq.FoqType
open Ploeh.AutoFixture.Kernel
open System
open System.Reflection

/// <summary>
/// Selects appropriate methods to create <see cref="Foq.Mock&lt;T&gt;"/> 
/// instances.
/// </summary>
type FoqMethodQuery() =
    /// <summary>
    /// Selects methods for the supplied type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>
    /// Constructors for <paramref name="type"/>.
    /// </returns>
    /// <remarks>
    /// <para>
    /// This method returns a sequence of <see cref="IMethod"/> according to
    /// the public and protected constructors available on 
    /// <paramref name="type"/>.
    /// </para>
    /// </remarks>
    member this.SelectMethods targetType = 
        (this :> IMethodQuery).SelectMethods targetType

    interface IMethodQuery with
        member this.SelectMethods targetType = 
            match targetType with
            | null -> raise (ArgumentNullException("targetType"))
            |  _   -> match targetType.IsInterface with
                      | true  -> seq { yield FoqMethod.Create(targetType) :?> IMethod }
                      | _     -> targetType.GetPublicAndProtectedConstructors() 
                                 |> Seq.sortBy(fun x -> x.GetParameters().Length)
                                 |> Seq.map(fun ctor -> FoqMethod.Create(
                                                            targetType, 
                                                            ctor.GetParameters())
                                                        :?> IMethod)
