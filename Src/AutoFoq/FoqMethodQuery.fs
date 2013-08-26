namespace Ploeh.AutoFixture.AutoFoq

open Ploeh.AutoFixture.Kernel
open System

type FoqMethodQuery() =
    interface IMethodQuery with
        member this.SelectMethods destination =
            raise (NotImplementedException())