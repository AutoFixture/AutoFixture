namespace Ploeh.AutoFixture.Idioms.FsCheckUnitTest.TestTypes

type AClass = 
    member this.VoidMethod() = ()
    member this.WriteOnlyProperty with set (value) = value

[<AbstractClass; Sealed>]
type AStaticClass =
    static member VoidMethod() = ()
    static member WriteOnlyProperty with set (value) = value