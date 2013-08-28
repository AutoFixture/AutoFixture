namespace Ploeh.AutoFixture.AutoFoq

open Ploeh.AutoFixture
open Ploeh.AutoFixture.Kernel
open System

type AutoFoqCustomization() =
    interface ICustomization with
        member this.Customize fixture = 
            raise (NotImplementedException())
