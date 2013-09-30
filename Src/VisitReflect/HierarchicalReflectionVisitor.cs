using System;

namespace Ploeh.VisitReflect
{
    public abstract class HierarchicalReflectionVisitor<T> : IHierarchicalReflectionVisitor<T>
    {
        public abstract T Value { get; }

        public virtual IHierarchicalReflectionVisitor<T> EnterAssembly(AssemblyElement assemblyElement)
        {
            return this;
        }

        public virtual IHierarchicalReflectionVisitor<T> ExitAssembly(AssemblyElement assemblyElement)
        {
            return this;
        }

        public virtual IHierarchicalReflectionVisitor<T> EnterType(TypeElement typeElement)
        {
            return this;
        }

        public virtual IHierarchicalReflectionVisitor<T> ExitType(TypeElement typeElement)
        {
            return this;
        }

        public virtual IHierarchicalReflectionVisitor<T> Visit(ParameterInfoElement parameterInfoElement)
        {
            return this;
        }

        public virtual IHierarchicalReflectionVisitor<T> Visit(PropertyInfoElement propertyInfoElement)
        {
            return this;
        }

        public virtual IHierarchicalReflectionVisitor<T> Visit(FieldInfoElement fieldInfoElement)
        {
            return this;
        }

        public virtual IHierarchicalReflectionVisitor<T> EnterConstructor(ConstructorInfoElement constructorInfoElement)
        {
            return this;
        }

        public virtual IHierarchicalReflectionVisitor<T> ExitConstructor(ConstructorInfoElement constructorInfoElement)
        {
            return this;
        }

        public virtual IHierarchicalReflectionVisitor<T> EnterMethod(MethodInfoElement methodInfoElement)
        {
            return this;
        }

        public virtual IHierarchicalReflectionVisitor<T> ExitMethod(MethodInfoElement methodInfoElement)
        {
            return this;
        }
    }
}