namespace Ploeh.VisitReflect
{
    public interface IReflectionVisitor<T>
    {
        T Value { get; }

        IReflectionVisitor<T> EnterAssembly(AssemblyElement assemblyElement);
        IReflectionVisitor<T> ExitAssembly(AssemblyElement assemblyElement);

        IReflectionVisitor<T> EnterType(TypeElement typeElement);
        IReflectionVisitor<T> ExitType(TypeElement typeElement);

        IReflectionVisitor<T> Visit(ParameterInfoElement parameterInfoElement);

        IReflectionVisitor<T> Visit(PropertyInfoElement propertyInfoElement);

        IReflectionVisitor<T> Visit(FieldInfoElement fieldInfoElement);

        IReflectionVisitor<T> EnterConstructor(ConstructorInfoElement constructorInfoElement);
        IReflectionVisitor<T> ExitConstructor(ConstructorInfoElement constructorInfoElement);

        IReflectionVisitor<T> EnterMethod(MethodInfoElement methodInfoElement);
        IReflectionVisitor<T> ExitMethod(MethodInfoElement methodInfoElement);
    }
}