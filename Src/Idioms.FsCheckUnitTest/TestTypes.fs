namespace Ploeh.AutoFixture.Idioms.FsCheckUnitTest.TestTypes

type AClass = 
    member this.VoidMethod () = ()
    member this.WriteOnlyProperty with set (value) = value
    member this.ReadOnlyProperty  with get () = obj()
    member this.MethodWithReturnValue () = obj()

[<AbstractClass; Sealed>]
type AStaticClass =
    static member VoidMethod () = ()
    static member WriteOnlyProperty with set (value) = value
    static member ReadOnlyProperty with get () = obj()
    static member MethodWithReturnValue () = obj()