namespace Ploeh.AutoFixture.AutoFoq

open Ploeh.AutoFixture.Kernel
open System

type FoqRelay(builder: ISpecimenBuilder, specification: IRequestSpecification) =
    new(builder: ISpecimenBuilder) = 
        FoqRelay(builder, AbstractTypeSpecification())
    member this.Builder
        with get() = builder
    member this.Specification
        with get() = specification
    interface ISpecimenBuilder with
        member this.Create (request, context) = 
             raise (new NotImplementedException())
