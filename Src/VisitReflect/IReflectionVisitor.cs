namespace Ploeh.VisitReflect
{
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

        // etc.
    }
}