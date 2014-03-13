namespace Ploeh.AutoFixture.Idioms.FsCheck

open Ploeh.AutoFixture.Idioms
open Ploeh.AutoFixture.Kernel
open System
open System.Reflection

type ReturnValueMustNotBeNullAssertion (builder) =
    inherit IdiomaticAssertion()
    
    do if builder = null then 
        raise <| ArgumentNullException("builder")
    
    member this.Builder = builder
    
    override this.Verify (propertyInfo : PropertyInfo) =
        if propertyInfo = null then 
            raise <| ArgumentNullException("propertyInfo")
        let getMethod = propertyInfo.GetGetMethod()
        if (getMethod <> null) then 
            this.Verify(propertyInfo.GetGetMethod());

    override this.Verify (methodInfo : MethodInfo) =
        if methodInfo = null then 
            raise <| ArgumentNullException("methodInfo")
        if methodInfo.ReturnType <> typeof<Void> then
            let owner = 
                match methodInfo.IsStatic with 
                | true  -> null 
                | false -> SpecimenContext(this.Builder).Resolve(methodInfo.ReflectedType);    
            
            let parameters = methodInfo.GetParameters() |> Seq.toList
            match parameters with
            | [] -> if methodInfo.Invoke(owner, null) = null then
                        raise <| ReturnValueMustNotBeNullException(
                            "The method "
                            + methodInfo.Name
                            + " returns null which is never an acceptable return"
                            + " value for a public Query (method that returns a value).")
            | _  -> Exercise methodInfo owner parameters |> ignore