namespace Ploeh.VisitReflect
{
    /// <summary>
    /// An implementation of the <see cref="IReflectionVisitor{T}"/> which does
    /// not visit any type of element, allowing easier implementation when only
    /// certain element types need to be visited.
    /// </summary>
    public abstract class ReflectionVisitor<T> : IReflectionVisitor<T>
    {
        public abstract T Value { get; }

        public virtual IReflectionVisitor<T> Visit(AssemblyElement assemblyElement)
        {
            return this;
        }

        public virtual IReflectionVisitor<T> Visit(TypeElement typeElement)
        {
            return this;
        }

        public virtual IReflectionVisitor<T> Visit(ParameterInfoElement parameterInfoElement)
        {
            return this;
        }

        public virtual IReflectionVisitor<T> Visit(PropertyInfoElement propertyInfoElement)
        {
            return this;
        }

        public virtual IReflectionVisitor<T> Visit(FieldInfoElement fieldInfoElement)
        {
            return this;
        }

        public virtual IReflectionVisitor<T> Visit(ConstructorInfoElement constructorInfoElement)
        {
            return this;
        }

        public virtual IReflectionVisitor<T> Visit(MethodInfoElement methodInfoElement)
        {
            return this;
        }
    }
}
