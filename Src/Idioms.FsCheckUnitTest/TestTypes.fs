namespace Ploeh.AutoFixture.Idioms.FsCheckUnitTest

open System

type AClass () = 
    member this.WriteOnlyProperty with set (value) = value |> ignore

    member this.NullReturnValueProperty with get () = null

    member this.ReadOnlyProperty  with get () = obj()

    member this.NullReturnValueMethod () = null :> obj

    member this.NullReturnValueMethodWithParameters
        (s : string, i : int) =
        null :> obj

    member this.NullReturnValueMethodWithManyParameters
        (s : string, i : int, d : double, g : Guid) =
        null :> obj

    member this.NullReturnValueMethodWithComplexParameters
        (s : string, v : Version) =
        null :> obj

    member this.MethodWithReturnValue () = obj()

    member this.MethodWithComplexParameters
        (s : string, v : Version) =
        obj()

    member this.VoidMethod () = ()

    member this.NullReturnValueMethodWithParametersAndBranching
        (s : string, i : int) =
        let returnValue =
            match s with
            | "" -> null
            | _  -> match i with
                    |  0 -> null
                    | -1 -> null
                    |  _ -> obj()
        returnValue

[<AbstractClass; Sealed>]
type AStaticClass () =
    static member WriteOnlyProperty with set (value) = value |> ignore

    static member NullReturnValueProperty  with get () = null

    static member ReadOnlyProperty with get () = obj()

    static member NullReturnValueMethod () = null :> obj

    static member NullReturnValueMethodWithParameters
        (s : string, i : int) =
        null :> obj

    static member NullReturnValueMethodWithManyParameters
        (s : string, i : int, d : double, g : Guid) =
        null :> obj

    static member NullReturnValueMethodWithComplexParameters
        (s : string, v : Version) =
        null :> obj

    static member MethodWithReturnValue () = obj()

    static member MethodWithComplexParameters
        (s : string, v : Version) =
        obj()

    static member VoidMethod () = ()

    static member NullReturnValueMethodWithParametersAndBranching
        (s : string, i : int) =
        let returnValue =
            match s with
            | "" -> null
            | _  -> match i with
                    |  0 -> null
                    | -1 -> null
                    |  _ -> obj()
        returnValue