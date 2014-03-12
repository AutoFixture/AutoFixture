namespace Ploeh.AutoFixture.Idioms.FsCheckUnitTest

type AClass () = 
    member this.VoidMethod () = ()
    member this.WriteOnlyProperty with set (value) = value |> ignore
    member this.ReadOnlyProperty  with get () = obj()
    member this.MethodWithReturnValue () = obj()
    member this.NullReturnValueProperty  with get () = null    

[<AbstractClass; Sealed>]
type AStaticClass () =
    static member VoidMethod () = ()
    static member WriteOnlyProperty with set (value) = value |> ignore
    static member ReadOnlyProperty with get () = obj()
    static member MethodWithReturnValue () = obj()
    static member NullReturnValueProperty  with get () = null