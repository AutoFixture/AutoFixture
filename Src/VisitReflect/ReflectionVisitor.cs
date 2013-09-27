namespace Ploeh.VisitReflect
{
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