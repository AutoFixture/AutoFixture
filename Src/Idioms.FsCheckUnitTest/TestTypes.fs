namespace Ploeh.AutoFixture.Idioms.FsCheckUnitTest

type AClass () = 
    member this.WriteOnlyProperty with set (value) = value |> ignore
    member this.NullReturnValueProperty with get () = null
    member this.ReadOnlyProperty  with get () = obj()

    member this.NullReturnValueMethod () = this.NullReturnValueProperty
    member this.NullReturnValueMethodWithParameters (s : string, i : int) = this.NullReturnValueProperty
    member this.MethodWithReturnValue () = obj()
    member this.VoidMethod () = ()

    member this.NullReturnValueMethodWithParametersAndBranching
        (s : string, i : int) =

        let returnValue =
            match s with
            | "" -> this.NullReturnValueMethod()
            | _  -> match i with
                    |  0 -> this.NullReturnValueMethod()
                    | -1 -> this.NullReturnValueMethod()
                    |  _ -> this.MethodWithReturnValue()
        returnValue


[<AbstractClass; Sealed>]
type AStaticClass () =
    static member WriteOnlyProperty with set (value) = value |> ignore
    static member NullReturnValueProperty  with get () = null
    static member ReadOnlyProperty with get () = obj()

    static member NullReturnValueMethod () = AStaticClass.NullReturnValueProperty
    static member NullReturnValueMethodWithParameters (s : string, i : int) = AStaticClass.NullReturnValueProperty
    static member MethodWithReturnValue () = obj()
    static member VoidMethod () = ()

    static member NullReturnValueMethodWithParametersAndBranching
        (s : string, i : int) =

        let returnValue =
            match s with
            | "" -> AStaticClass.NullReturnValueMethod()
            | _  -> match i with
                    |  0 -> AStaticClass.NullReturnValueMethod()
                    | -1 -> AStaticClass.NullReturnValueMethod()
                    |  _ -> AStaticClass.MethodWithReturnValue()
        returnValue