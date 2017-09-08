using System;
using System.Collections.Generic;
using System.Reflection;
using Ploeh.AutoFixture.Idioms;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class DelegatingIdiomaticAssertion : IdiomaticAssertion
    {
        public DelegatingIdiomaticAssertion()
        {
            this.OnAssemblyArrayVerify = a => { };
            this.OnAssembliesVerify = a => { };
            this.OnAssemblyVerify = a => { };
            this.OnTypeArrayVerify = t => { };
            this.OnTypesVerify = t => { };
            this.OnTypeVerify = t => { };
            this.OnMemberInfoArrayVerify = m => { };
            this.OnMemberInfosVerify = m => { };
            this.OnMemberInfoVerify = m => { };
            this.OnConstructorInfoArrayVerify = c => { };
            this.OnConstructorInfosVerify = c => { };
            this.OnConstructorInfoVerify = c => { };
            this.OnMethodInfoArrayVerify = m => { };
            this.OnMethodInfosVerify = m => { };
            this.OnMethodInfoVerify = m => { };
            this.OnPropertyInfoArrayVerify = p => { };
            this.OnPropertyInfosVerify = p => { };
            this.OnPropertyInfoVerify = p => { };
            this.OnFieldInfoArrayVerify = p => { };
            this.OnFieldInfosVerify = p => { };
            this.OnFieldInfoVerify = p => { };
        }

        public Action<Assembly[]> OnAssemblyArrayVerify { get; set; }

        public Action<IEnumerable<Assembly>> OnAssembliesVerify { get; set; }

        public Action<Assembly> OnAssemblyVerify { get; set; }

        public Action<Type[]> OnTypeArrayVerify { get; set; }

        public Action<IEnumerable<Type>> OnTypesVerify { get; set; }

        public Action<Type> OnTypeVerify { get; set; }

        public Action<MemberInfo[]> OnMemberInfoArrayVerify { get; set; }

        public Action<IEnumerable<MemberInfo>> OnMemberInfosVerify { get; set; }

        public Action<MemberInfo> OnMemberInfoVerify { get; set; }

        public Action<ConstructorInfo[]> OnConstructorInfoArrayVerify { get; set; }

        public Action<IEnumerable<ConstructorInfo>> OnConstructorInfosVerify { get; set; }

        public Action<ConstructorInfo> OnConstructorInfoVerify { get; set; }

        public Action<MethodInfo[]> OnMethodInfoArrayVerify { get; set; }

        public Action<IEnumerable<MethodInfo>> OnMethodInfosVerify { get; set; }

        public Action<MethodInfo> OnMethodInfoVerify { get; set; }

        public Action<PropertyInfo[]> OnPropertyInfoArrayVerify { get; set; }

        public Action<IEnumerable<PropertyInfo>> OnPropertyInfosVerify { get; set; }

        public Action<PropertyInfo> OnPropertyInfoVerify { get; set; }

        public Action<FieldInfo[]> OnFieldInfoArrayVerify { get; set; }

        public Action<IEnumerable<FieldInfo>> OnFieldInfosVerify { get; set; }

        public Action<FieldInfo> OnFieldInfoVerify { get; set; }

        public override void Verify(params Assembly[] assemblies)
        {
            this.OnAssemblyArrayVerify(assemblies);
            base.Verify(assemblies);
        }

        public override void Verify(IEnumerable<Assembly> assemblies)
        {
            this.OnAssembliesVerify(assemblies);
            base.Verify(assemblies);
        }

        public override void Verify(Assembly assembly)
        {
            this.OnAssemblyVerify(assembly);
            base.Verify(assembly);
        }

        public override void Verify(params Type[] types)
        {
            this.OnTypeArrayVerify(types);
            base.Verify(types);
        }

        public override void Verify(IEnumerable<Type> types)
        {
            this.OnTypesVerify(types);
            base.Verify(types);
        }

        public override void Verify(Type type)
        {
            this.OnTypeVerify(type);
            base.Verify(type);
        }

        public override void Verify(params MemberInfo[] memberInfos)
        {
            this.OnMemberInfoArrayVerify(memberInfos);
            base.Verify(memberInfos);
        }

        public override void Verify(IEnumerable<MemberInfo> memberInfos)
        {
            this.OnMemberInfosVerify(memberInfos);
            base.Verify(memberInfos);
        }

        public override void Verify(MemberInfo memberInfo)
        {
            this.OnMemberInfoVerify(memberInfo);
            base.Verify(memberInfo);
        }

        public override void Verify(params ConstructorInfo[] constructorInfos)
        {
            this.OnConstructorInfoArrayVerify(constructorInfos);
            base.Verify(constructorInfos);
        }

        public override void Verify(IEnumerable<ConstructorInfo> constructorInfos)
        {
            this.OnConstructorInfosVerify(constructorInfos);
            base.Verify(constructorInfos);
        }

        public override void Verify(ConstructorInfo constructorInfo)
        {
            this.OnConstructorInfoVerify(constructorInfo);
            base.Verify(constructorInfo);
        }

        public override void Verify(params MethodInfo[] methodInfos)
        {
            this.OnMethodInfoArrayVerify(methodInfos);
            base.Verify(methodInfos);
        }

        public override void Verify(IEnumerable<MethodInfo> methodInfos)
        {
            this.OnMethodInfosVerify(methodInfos);
            base.Verify(methodInfos);
        }

        public override void Verify(MethodInfo methodInfo)
        {
            this.OnMethodInfoVerify(methodInfo);
            base.Verify(methodInfo);
        }

        public override void Verify(params PropertyInfo[] propertyInfos)
        {
            this.OnPropertyInfoArrayVerify(propertyInfos);
            base.Verify(propertyInfos);
        }

        public override void Verify(IEnumerable<PropertyInfo> propertyInfos)
        {
            this.OnPropertyInfosVerify(propertyInfos);
            base.Verify(propertyInfos);
        }

        public override void Verify(PropertyInfo propertyInfo)
        {
            this.OnPropertyInfoVerify(propertyInfo);
            base.Verify(propertyInfo);
        }

        public override void Verify(params FieldInfo[] fieldInfos)
        {
            this.OnFieldInfoArrayVerify(fieldInfos);
            base.Verify(fieldInfos);
        }

        public override void Verify(IEnumerable<FieldInfo> fieldInfos)
        {
            this.OnFieldInfosVerify(fieldInfos);
            base.Verify(fieldInfos);
        }

        public override void Verify(FieldInfo fieldInfo)
        {
            this.OnFieldInfoVerify(fieldInfo);
            base.Verify(fieldInfo);
        }
    }
}
