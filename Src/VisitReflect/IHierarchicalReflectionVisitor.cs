namespace Ploeh.VisitReflect
{
    public interface IHierarchicalReflectionVisitor<T>
    {
        T Value { get; }

        IHierarchicalReflectionVisitor<T> EnterAssembly(AssemblyElement assemblyElement);
        IHierarchicalReflectionVisitor<T> ExitAssembly(AssemblyElement assemblyElement);

        IHierarchicalReflectionVisitor<T> EnterType(TypeElement typeElement);
        IHierarchicalReflectionVisitor<T> ExitType(TypeElement typeElement);

        IHierarchicalReflectionVisitor<T> Visit(ParameterInfoElement parameterInfoElement);

        IHierarchicalReflectionVisitor<T> Visit(PropertyInfoElement propertyInfoElement);

        IHierarchicalReflectionVisitor<T> Visit(FieldInfoElement fieldInfoElement);

        IHierarchicalReflectionVisitor<T> EnterConstructor(ConstructorInfoElement constructorInfoElement);
        IHierarchicalReflectionVisitor<T> ExitConstructor(ConstructorInfoElement constructorInfoElement);

        IHierarchicalReflectionVisitor<T> EnterMethod(MethodInfoElement methodInfoElement);
        IHierarchicalReflectionVisitor<T> ExitMethod(MethodInfoElement methodInfoElement);
    }
}