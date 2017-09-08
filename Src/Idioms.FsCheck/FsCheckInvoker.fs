namespace Ploeh.AutoFixture.Idioms.FsCheck

open FsCheck
open FsCheck.Fluent
open System
open System.Reflection

[<AutoOpen>]
module internal FsCheckInvoker =
    let GetValues (tuple : Type, owner) =
        seq {
            for pi in tuple.GetProperties() do
                yield tuple.GetProperty(pi.Name).GetValue(owner, null) }
        |> Seq.toArray

    let Invoke<'tuple> (methodInfo : MethodInfo, owner) =
       Check.QuickThrowOnFailure((fun (x : 'tuple) ->
           methodInfo.Invoke(
               owner,
               GetValues(typeof<'tuple>, x)) <> null))

    let CreateTuple (parameters : ParameterInfo list) = 
        let keys =
            parameters
            |> Seq.map (fun p -> p.ParameterType)
            |> Seq.toArray
        Type.GetType("System.Tuple`" + keys.Length.ToString())
            .MakeGenericType(keys)

    let Exercise
        (methodInfo : MethodInfo)
        (owner)
        (parameters : ParameterInfo list) =
        let tuple = CreateTuple parameters
        try
            Assembly
                .GetExecutingAssembly()
                .GetType("Ploeh.AutoFixture.Idioms.FsCheck.FsCheckInvoker")
                .GetMethod(
                    "Invoke",
                    BindingFlags.Static ||| BindingFlags.NonPublic)
                .MakeGenericMethod(tuple)
                .Invoke(null, [| methodInfo; owner |]);
        with
        | e -> raise <| ReturnValueMustNotBeNullException(
                "The method "
                + methodInfo.Name
                + " returns null which is never an acceptable return"
                + " value for a public Query (method that returns a value)."
                + Environment.NewLine
                + e.InnerException.Message)
