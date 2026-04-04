using System;
using System.Collections.Generic;
using System.Reflection;
using AutoFixture.Idioms;

namespace AutoFixture.IdiomsUnitTest;

public class DelegatingIdiomaticAssertion : IdiomaticAssertion
{
    public DelegatingIdiomaticAssertion()
    {
        OnAssemblyArrayVerify = a => { };
        OnAssembliesVerify = a => { };
        OnAssemblyVerify = a => { };
        OnTypeArrayVerify = t => { };
        OnTypesVerify = t => { };
        OnTypeVerify = t => { };
        OnMemberInfoArrayVerify = m => { };
        OnMemberInfosVerify = m => { };
        OnMemberInfoVerify = m => { };
        OnConstructorInfoArrayVerify = c => { };
        OnConstructorInfosVerify = c => { };
        OnConstructorInfoVerify = c => { };
        OnMethodInfoArrayVerify = m => { };
        OnMethodInfosVerify = m => { };
        OnMethodInfoVerify = m => { };
        OnPropertyInfoArrayVerify = p => { };
        OnPropertyInfosVerify = p => { };
        OnPropertyInfoVerify = p => { };
        OnFieldInfoArrayVerify = p => { };
        OnFieldInfosVerify = p => { };
        OnFieldInfoVerify = p => { };
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
        OnAssemblyArrayVerify(assemblies);
        base.Verify(assemblies);
    }

    public override void Verify(IEnumerable<Assembly> assemblies)
    {
        OnAssembliesVerify(assemblies);
        base.Verify(assemblies);
    }

    public override void Verify(Assembly assembly)
    {
        OnAssemblyVerify(assembly);
        base.Verify(assembly);
    }

    public override void Verify(params Type[] types)
    {
        OnTypeArrayVerify(types);
        base.Verify(types);
    }

    public override void Verify(IEnumerable<Type> types)
    {
        OnTypesVerify(types);
        base.Verify(types);
    }

    public override void Verify(Type type)
    {
        OnTypeVerify(type);
        base.Verify(type);
    }

    public override void Verify(params MemberInfo[] memberInfos)
    {
        OnMemberInfoArrayVerify(memberInfos);
        base.Verify(memberInfos);
    }

    public override void Verify(IEnumerable<MemberInfo> memberInfos)
    {
        OnMemberInfosVerify(memberInfos);
        base.Verify(memberInfos);
    }

    public override void Verify(MemberInfo memberInfo)
    {
        OnMemberInfoVerify(memberInfo);
        base.Verify(memberInfo);
    }

    public override void Verify(params ConstructorInfo[] constructorInfos)
    {
        OnConstructorInfoArrayVerify(constructorInfos);
        base.Verify(constructorInfos);
    }

    public override void Verify(IEnumerable<ConstructorInfo> constructorInfos)
    {
        OnConstructorInfosVerify(constructorInfos);
        base.Verify(constructorInfos);
    }

    public override void Verify(ConstructorInfo constructorInfo)
    {
        OnConstructorInfoVerify(constructorInfo);
        base.Verify(constructorInfo);
    }

    public override void Verify(params MethodInfo[] methodInfos)
    {
        OnMethodInfoArrayVerify(methodInfos);
        base.Verify(methodInfos);
    }

    public override void Verify(IEnumerable<MethodInfo> methodInfos)
    {
        OnMethodInfosVerify(methodInfos);
        base.Verify(methodInfos);
    }

    public override void Verify(MethodInfo methodInfo)
    {
        OnMethodInfoVerify(methodInfo);
        base.Verify(methodInfo);
    }

    public override void Verify(params PropertyInfo[] propertyInfos)
    {
        OnPropertyInfoArrayVerify(propertyInfos);
        base.Verify(propertyInfos);
    }

    public override void Verify(IEnumerable<PropertyInfo> propertyInfos)
    {
        OnPropertyInfosVerify(propertyInfos);
        base.Verify(propertyInfos);
    }

    public override void Verify(PropertyInfo propertyInfo)
    {
        OnPropertyInfoVerify(propertyInfo);
        base.Verify(propertyInfo);
    }

    public override void Verify(params FieldInfo[] fieldInfos)
    {
        OnFieldInfoArrayVerify(fieldInfos);
        base.Verify(fieldInfos);
    }

    public override void Verify(IEnumerable<FieldInfo> fieldInfos)
    {
        OnFieldInfosVerify(fieldInfos);
        base.Verify(fieldInfos);
    }

    public override void Verify(FieldInfo fieldInfo)
    {
        OnFieldInfoVerify(fieldInfo);
        base.Verify(fieldInfo);
    }
}