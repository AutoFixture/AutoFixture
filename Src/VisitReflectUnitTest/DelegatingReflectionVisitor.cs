using System;

namespace Ploeh.VisitReflect.UnitTest
{
    internal class DelegatingReflectionVisitor<T> : IReflectionVisitor<T>
    {
        public T Value { get; set; }

        public DelegatingReflectionVisitor()
        {
            this.OnVisitAssemblyElement = e => { };
            this.OnVisitTypeElement = e => { };
            this.OnVisitParameterInfoElement = e => { };
            this.OnVisitPropertyInfoElement = e => { };
            this.OnVisitFieldInfoElement = e => { };
            this.OnVisitConstructorInfoElement = e => { };
            this.OnVisitMethodInfoElement = e => { };
        }

        public Action<AssemblyElement> OnVisitAssemblyElement { get; set; }
        public Action<TypeElement> OnVisitTypeElement { get; set; }
        public Action<ParameterInfoElement> OnVisitParameterInfoElement { get; set; }
        public Action<PropertyInfoElement> OnVisitPropertyInfoElement { get; set; }
        public Action<FieldInfoElement> OnVisitFieldInfoElement { get; set; }
        public Action<ConstructorInfoElement> OnVisitConstructorInfoElement { get; set; }
        public Action<MethodInfoElement> OnVisitMethodInfoElement { get; set; }

        public IReflectionVisitor<T> Visit(AssemblyElement assemblyElement)
        {
            OnVisitAssemblyElement(assemblyElement);
            return this;
        }

        public IReflectionVisitor<T> Visit(TypeElement typeElement)
        {
            OnVisitTypeElement(typeElement);
            return this;
        }

        public IReflectionVisitor<T> Visit(ParameterInfoElement parameterInfoElement)
        {
            OnVisitParameterInfoElement(parameterInfoElement);
            return this;
        }

        public IReflectionVisitor<T> Visit(PropertyInfoElement propertyInfoElement)
        {
            OnVisitPropertyInfoElement(propertyInfoElement);
            return this;
        }

        public IReflectionVisitor<T> Visit(FieldInfoElement fieldInfoElement)
        {
            OnVisitFieldInfoElement(fieldInfoElement);
            return this;
        }

        public IReflectionVisitor<T> Visit(ConstructorInfoElement constructorInfoElement)
        {
            OnVisitConstructorInfoElement(constructorInfoElement);
            return this;
        }

        public IReflectionVisitor<T> Visit(MethodInfoElement methodInfoElement)
        {
            OnVisitMethodInfoElement(methodInfoElement);
            return this;
        }
    }
}