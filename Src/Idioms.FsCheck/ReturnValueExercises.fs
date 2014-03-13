namespace Ploeh.AutoFixture.Idioms.FsCheck

open FsCheck
open FsCheck.Fluent
open System
open System.Reflection

[<AbstractClass; Sealed>]
type internal FsCheckInvoker () =
    static member Invoke<'a> (methodInfo: MethodInfo, owner) =
       Check.QuickThrowOnFailure((fun (x : 'a) ->
           methodInfo.Invoke(
               owner,
               FsCheckInvoker.GetValues(typeof<'a>, x)) <> null))

    static member GetValues (tuple, x) =
        seq {
            for property in tuple.GetProperties() do
                yield tuple.GetProperty(property.Name).GetValue(x, null) }
        |> Seq.toArray

[<AutoOpen>]
module internal ReturnValueExercises =
    let Exercise
        (methodInfo : MethodInfo)
        (owner)
        (parameters : ParameterInfo list) =
        let keys =
            parameters
            |> Seq.map (fun p -> p.ParameterType)
            |> Seq.toArray

        let tuple =
            Type.GetType("System.Tuple`" + keys.Length.ToString())
                .MakeGenericType(keys);

        try
            typeof<FsCheckInvoker>
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
