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
    member this.Create (request, context) = 
        (this :> ISpecimenBuilder).Create(request, context)
    interface ISpecimenBuilder with
        member this.Create (request, context) = 
            match this.Specification.IsSatisfiedBy(request) with
            | false -> NoSpecimen(request) :> obj
            | true  -> let specimen = this.Builder.Create(request, context) 
                       match specimen with
                       | null -> NoSpecimen(request) :> obj
                       | _    -> specimen
