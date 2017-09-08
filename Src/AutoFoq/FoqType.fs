module internal Ploeh.AutoFixture.AutoFoq.FoqType

open System
open System.Reflection

type Type with 
    [<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "This method is a Type Extension and it's module is declared as internal.")>]
    member this.GetPublicAndProtectedConstructors() = 
        this.GetConstructors(
            BindingFlags.Public ||| 
            BindingFlags.Instance ||| 
            BindingFlags.NonPublic)