namespace Ploeh.AutoFixture.Idioms.FsCheck

open Ploeh.AutoFixture.Idioms
open Ploeh.AutoFixture.Kernel
open System
open System.Reflection

/// <summary>
/// Encapsulates a unit test verifying that the return value for a public Query
/// (a method that returns a value) is not null.
/// </summary>
type ReturnValueMustNotBeNullAssertion (builder) =
    inherit IdiomaticAssertion()
    
    do if builder = null then 
        raise <| ArgumentNullException("builder")

    let Create request = SpecimenContext(builder).Resolve(request)
    
    /// Gets the <see cref="ISpecimenBuilder"/> which create instances if the
    /// public Query is an instance method.
    member this.Builder = builder

    /// <summary>
    /// Verifies that the return value for a Property is not null.
    /// </summary>
    /// <param name="propertyInfo">The property to verify.</param>
    override this.Verify (propertyInfo : PropertyInfo) =
        if propertyInfo = null then 
            raise <| ArgumentNullException("propertyInfo")
        let getMethod = propertyInfo.GetGetMethod()
        if (getMethod <> null) then 
            this.Verify(propertyInfo.GetGetMethod());

    /// <summary>
    /// Verifies that the return value for a method is not null.
    /// </summary>
    /// <remarks>
    /// <param name="methodInfo">The method to verify.</param>
    override this.Verify (methodInfo : MethodInfo) =
        if methodInfo = null then 
            raise <| ArgumentNullException("methodInfo")
        if methodInfo.ReturnType <> typeof<Void> then
            let owner = 
                match methodInfo.IsStatic with 
                | true  -> null 
                | false -> Create methodInfo.ReflectedType
            
            let parameters = methodInfo.GetParameters() |> Seq.toList
            match parameters with
            | [] -> if methodInfo.Invoke(owner, null) = null then
                        raise <| ReturnValueMustNotBeNullException(
                            "The method "
                            + methodInfo.Name
                            + " returns null which is never an acceptable return"
                            + " value for a public Query (a method that returns a value).")
            | _  -> Exercise methodInfo owner parameters |> ignore