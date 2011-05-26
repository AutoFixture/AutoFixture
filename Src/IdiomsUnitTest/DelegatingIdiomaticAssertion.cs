using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Idioms;
using System.Reflection;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class DelegatingIdiomaticAssertion : IdiomaticAssertion
    {
        public DelegatingIdiomaticAssertion()
        {
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
        }

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

        public override void Verify(Assembly assembly)
        {
            this.OnAssemblyVerify(assembly);
        }

        public override void Verify(params Type[] types)
        {
            this.OnTypeArrayVerify(types);
        }

        public override void Verify(IEnumerable<Type> types)
        {
            this.OnTypesVerify(types);
        }

        public override void Verify(Type type)
        {
            this.OnTypeVerify(type);
        }

        public override void Verify(params MemberInfo[] memberInfos)
        {
            this.OnMemberInfoArrayVerify(memberInfos);
        }

        public override void Verify(IEnumerable<MemberInfo> memberInfos)
        {
            this.OnMemberInfosVerify(memberInfos);
        }

        public override void Verify(MemberInfo memberInfo)
        {
            this.OnMemberInfoVerify(memberInfo);
        }

        public override void Verify(params ConstructorInfo[] constructorInfos)
        {
            this.OnConstructorInfoArrayVerify(constructorInfos);
        }

        public override void Verify(IEnumerable<ConstructorInfo> constructorInfos)
        {
            this.OnConstructorInfosVerify(constructorInfos);
        }

        public override void Verify(ConstructorInfo constructorInfo)
        {
            this.OnConstructorInfoVerify(constructorInfo);
        }

        public override void Verify(params MethodInfo[] methodInfos)
        {
            this.OnMethodInfoArrayVerify(methodInfos);
        }

        public override void Verify(IEnumerable<MethodInfo> methodInfos)
        {
            this.OnMethodInfosVerify(methodInfos);
        }

        public override void Verify(MethodInfo methodInfo)
        {
            this.OnMethodInfoVerify(methodInfo);
        }

        public override void Verify(params PropertyInfo[] propertyInfos)
        {
            this.OnPropertyInfoArrayVerify(propertyInfos);
        }

        public override void Verify(IEnumerable<PropertyInfo> propertyInfos)
        {
            this.OnPropertyInfosVerify(propertyInfos);
        }

        public override void Verify(PropertyInfo propertyInfo)
        {
            this.OnPropertyInfoVerify(propertyInfo);
        }
    }
}
