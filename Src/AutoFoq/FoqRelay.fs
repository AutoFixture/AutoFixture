namespace Ploeh.AutoFixture.AutoFoq

open Ploeh.AutoFixture.Kernel
open System

type FoqRelay =

    new(builder: ISpecimenBuilder) = 
        FoqRelay(builder, AbstractTypeSpecification())

    new(builder: ISpecimenBuilder, specification: IRequestSpecification) = 
        { 
            Builder = 
                ((if builder = null 
                  then raise(ArgumentNullException("builder"))); 
                  builder)

            Specification = 
                ((if specification = null 
                  then raise(ArgumentNullException("specification"))); 
                  specification)
        }

    val Builder: ISpecimenBuilder

    val Specification: IRequestSpecification
    
    member this.Create(request, context) = 
        (this :> ISpecimenBuilder).Create(request, context)
    
    interface ISpecimenBuilder with
        member this.Create(request, context) = 
            match this.Specification.IsSatisfiedBy(request) with
            | false -> NoSpecimen(request) :> obj
            | true  -> let specimen = this.Builder.Create(request, context) 
                       match specimen with
                       | null -> NoSpecimen(request) :> obj
                       | _    -> specimen
