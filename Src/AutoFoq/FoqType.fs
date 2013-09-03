module internal Ploeh.AutoFixture.AutoFoq.FoqType

open System
open System.Reflection

type Type with 
    member this.GetPublicAndProtectedConstructors() = 
        this.GetConstructors(
            BindingFlags.Public ||| 
            BindingFlags.Instance ||| 
            BindingFlags.NonPublic)