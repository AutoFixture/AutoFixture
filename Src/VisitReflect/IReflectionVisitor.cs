namespace Ploeh.VisitReflect
{
    /// <summary>
    /// Represents a visitor which can visit reflection elements.
    /// </summary>
    /// <typeparam name="T">The type of observations or calculations the 
    /// visitor makes</typeparam>
    public interface IReflectionVisitor<T>
    {
        T Value { get; }

        IReflectionVisitor<T> Visit(AssemblyElement assemblyElement);

        IReflectionVisitor<T> Visit(TypeElement typeElement);

        IReflectionVisitor<T> Visit(ParameterInfoElement parameterInfoElement);

        IReflectionVisitor<T> Visit(PropertyInfoElement propertyInfoElement);

        IReflectionVisitor<T> Visit(FieldInfoElement fieldInfoElement);

        IReflectionVisitor<T> Visit(ConstructorInfoElement constructorInfoElement);

        IReflectionVisitor<T> Visit(MethodInfoElement methodInfoElement);
    }
}
