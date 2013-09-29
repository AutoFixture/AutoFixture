using System;

namespace Ploeh.VisitReflect
{
    public abstract class ReflectionVisitor<T> : IReflectionVisitor<T>
    {
        public abstract T Value { get; }

        public virtual IReflectionVisitor<T> EnterAssembly(AssemblyElement assemblyElement)
        {
            return this;
        }

        public virtual IReflectionVisitor<T> ExitAssembly(AssemblyElement assemblyElement)
        {
            return this;
        }

        public virtual IReflectionVisitor<T> EnterType(TypeElement typeElement)
        {
            return this;
        }

        public virtual IReflectionVisitor<T> ExitType(TypeElement typeElement)
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

        public virtual IReflectionVisitor<T> EnterConstructor(ConstructorInfoElement constructorInfoElement)
        {
            return this;
        }

        public virtual IReflectionVisitor<T> ExitConstructor(ConstructorInfoElement constructorInfoElement)
        {
            return this;
        }

        public virtual IReflectionVisitor<T> EnterMethod(MethodInfoElement methodInfoElement)
        {
            return this;
        }

        public virtual IReflectionVisitor<T> ExitMethod(MethodInfoElement methodInfoElement)
        {
            return this;
        }
    }
}