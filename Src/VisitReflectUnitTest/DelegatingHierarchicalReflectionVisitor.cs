using System;

namespace Ploeh.VisitReflect.UnitTest
{
    internal class DelegatingHierarchicalReflectionVisitor<T> : IHierarchicalReflectionVisitor<T>
    {
        public T Value { get; set; }

        public DelegatingHierarchicalReflectionVisitor()
        {
            this.OnEnterAssemblyElement = e => { };
            this.OnExitAssemblyElement = e => { };
            this.OnEnterTypeElement = e => { };
            this.OnExitTypeElement = e => { };
            this.OnVisitParameterInfoElement = e => { };
            this.OnVisitPropertyInfoElement = e => { };
            this.OnVisitFieldInfoElement = e => { };
            this.OnEnterConstructorInfoElement = e => { };
            this.OnExitConstructorInfoElement = e => { };
            this.OnEnterMethodInfoElement = e => { };
            this.OnExitMethodInfoElement = e => { };
        }

        public Action<AssemblyElement> OnEnterAssemblyElement { get; set; }
        public Action<AssemblyElement> OnExitAssemblyElement { get; set; }
        public Action<TypeElement> OnEnterTypeElement { get; set; }
        public Action<TypeElement> OnExitTypeElement { get; set; }
        public Action<ParameterInfoElement> OnVisitParameterInfoElement { get; set; }
        public Action<PropertyInfoElement> OnVisitPropertyInfoElement { get; set; }
        public Action<FieldInfoElement> OnVisitFieldInfoElement { get; set; }
        public Action<ConstructorInfoElement> OnEnterConstructorInfoElement { get; set; }
        public Action<ConstructorInfoElement> OnExitConstructorInfoElement { get; set; }
        public Action<MethodInfoElement> OnEnterMethodInfoElement { get; set; }
        public Action<MethodInfoElement> OnExitMethodInfoElement { get; set; }


        public IHierarchicalReflectionVisitor<T> EnterAssembly(AssemblyElement assemblyElement)
        {
            OnEnterAssemblyElement(assemblyElement);
            return this;
        }

        public IHierarchicalReflectionVisitor<T> ExitAssembly(AssemblyElement assemblyElement)
        {
            OnExitAssemblyElement(assemblyElement);
            return this;
        }

        public IHierarchicalReflectionVisitor<T> EnterType(TypeElement typeElement)
        {
            OnEnterTypeElement(typeElement);
            return this;
        }

        public IHierarchicalReflectionVisitor<T> ExitType(TypeElement typeElement)
        {
            OnExitTypeElement(typeElement);
            return this;
        }

        public IHierarchicalReflectionVisitor<T> Visit(ParameterInfoElement parameterInfoElement)
        {
            OnVisitParameterInfoElement(parameterInfoElement);
            return this;
        }

        public IHierarchicalReflectionVisitor<T> Visit(PropertyInfoElement propertyInfoElement)
        {
            OnVisitPropertyInfoElement(propertyInfoElement);
            return this;
        }

        public IHierarchicalReflectionVisitor<T> Visit(FieldInfoElement fieldInfoElement)
        {
            OnVisitFieldInfoElement(fieldInfoElement);
            return this;
        }

        public IHierarchicalReflectionVisitor<T> EnterConstructor(ConstructorInfoElement constructorInfoElement)
        {
            OnEnterConstructorInfoElement(constructorInfoElement);
            return this;
        }

        public IHierarchicalReflectionVisitor<T> ExitConstructor(ConstructorInfoElement constructorInfoElement)
        {
            OnExitConstructorInfoElement(constructorInfoElement);
            return this;
        }

        public IHierarchicalReflectionVisitor<T> EnterMethod(MethodInfoElement methodInfoElement)
        {
            OnEnterMethodInfoElement(methodInfoElement);
            return this;
        }

        public IHierarchicalReflectionVisitor<T> ExitMethod(MethodInfoElement methodInfoElement)
        {
            OnExitMethodInfoElement(methodInfoElement);
            return this;
        }
    }
}