using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using AutoFixture.Kernel;

namespace AutoFixture.Idioms;

/// <summary>
/// Utility to take open generic types and close them respecting the generic constrains.
/// </summary>
internal class OpenGenericTypeClosingUtil
{
    private ISpecimenBuilder Builder { get; }

    public OpenGenericTypeClosingUtil(ISpecimenBuilder builder)
    {
        Builder = builder ?? throw new ArgumentNullException(nameof(builder));
    }

    public ConstructorInfo CloseGenericType(ConstructorInfo constructorInfo)
    {
        return (ConstructorInfo)ResolveUnclosedGenericType(constructorInfo, t => t.GetConstructors());
    }

    public PropertyInfo CloseGenericType(PropertyInfo propertyInfo)
    {
        return propertyInfo.ReflectedType.ContainsGenericParameters
            ? new AutoGenericType(Builder, propertyInfo.ReflectedType)
                .Value
                .GetProperties()
                .Single(pi => string.Equals(pi.Name, propertyInfo.Name, StringComparison.Ordinal))
            : propertyInfo;
    }

    public MethodInfo CloseGenericMethod(MethodInfo methodInfo)
    {
        return methodInfo.ContainsGenericParameters
            ? new AutoGenericMethod(Builder, methodInfo)
                .Value
            : methodInfo;
    }

    public MethodInfo CloseGenericType(MethodInfo methodInfo)
    {
        return (MethodInfo)ResolveUnclosedGenericType(methodInfo, t => t.GetMethods());
    }

    private MethodBase ResolveUnclosedGenericType(MethodBase method,
        Func<Type, IEnumerable<MethodBase>> methodSetToMatch)
    {
        if (!method.ReflectedType.ContainsGenericParameters)
        {
            return method;
        }

        var autoGenericType = new AutoGenericType(Builder, method.ReflectedType);
        return methodSetToMatch(autoGenericType.Value).Single(c => IsMatched(c, method, autoGenericType));
    }

    private static bool IsMatched(MethodBase resolved, MethodBase method, AutoGenericType autoGenericType)
    {
        return string.Equals(resolved.Name, method.Name, StringComparison.Ordinal) &&
               resolved.GetParameters()
                   .Select(pi => pi.ParameterType)
                   .SequenceEqual(autoGenericType.ResolveUnclosedParameterTypes(method.GetParameters()));
    }

    private class AutoGenericType
    {
        private readonly ISpecimenBuilder _specimenBuilder;
        private readonly Type _unclosedGenericType;
        private readonly AutoGenericArgumentCollection _autoGenericArguments;

        public AutoGenericType(ISpecimenBuilder specimenBuilder, Type unclosedGenericType)
        {
            _specimenBuilder = specimenBuilder;
            _unclosedGenericType = unclosedGenericType;
            _autoGenericArguments = new AutoGenericArgumentCollection();
        }

        public Type Value
        {
            get
            {
                return _unclosedGenericType
                    .GetGenericTypeDefinition()
                    .MakeGenericType(GetTypedArguments());
            }
        }

        public IEnumerable<Type> ResolveUnclosedParameterTypes(IEnumerable<ParameterInfo> parameterInfos)
        {
            return parameterInfos.Select(
                pi => pi.ParameterType.IsByRef
                    ? ResolveUnclosedParameterType(pi.ParameterType.GetElementType()).MakeByRefType()
                    : ResolveUnclosedParameterType(pi.ParameterType));
        }

        private Type ResolveUnclosedParameterType(Type parameterType)
        {
            if (parameterType.IsArray)
                return ResolveNestedArrayParameterType(parameterType);

            if (parameterType.IsGenericType)
                return ReosolveNestedGenericParameterType(parameterType);

            return ResolveGenericParameter(parameterType);
        }

        private Type ResolveNestedArrayParameterType(Type parameterType)
        {
            var elementType = ResolveUnclosedParameterType(parameterType.GetElementType());
            var rank = parameterType.GetArrayRank();
            return rank == 1 ? elementType.MakeArrayType() : elementType.MakeArrayType(rank);
        }

        private Type ReosolveNestedGenericParameterType(Type parameterType)
        {
            var genericArguments = parameterType.GetGenericArguments();
            var typeArguments = genericArguments.Select(ResolveUnclosedParameterType).ToArray();
            return parameterType.GetGenericTypeDefinition().MakeGenericType(typeArguments);
        }

        private Type ResolveGenericParameter(Type parameterType)
        {
            return IsGenericTypeParameter(parameterType)
                ? _autoGenericArguments[parameterType.Name].Value
                : parameterType;
        }

        private bool IsGenericTypeParameter(Type parameterType)
        {
            return parameterType.IsGenericParameter
                   && _autoGenericArguments.Contains(parameterType.Name);
        }

        private Type[] GetTypedArguments()
        {
            return _unclosedGenericType
                .GetGenericArguments()
                .Select(t =>
                {
                    if (!t.IsGenericParameter)
                    {
                        return t;
                    }

                    var autoGenericArgument = new AutoGenericArgument(_specimenBuilder, t);
                    _autoGenericArguments.Add(autoGenericArgument);
                    return autoGenericArgument.Value;
                })
                .ToArray();
        }
    }

    private class AutoGenericMethod
    {
        private readonly ISpecimenBuilder _specimenBuilder;
        private readonly MethodInfo _unclosedGenericMethod;

        public AutoGenericMethod(ISpecimenBuilder specimenBuilder, MethodInfo unclosedGenericMethod)
        {
            _specimenBuilder = specimenBuilder;
            _unclosedGenericMethod = unclosedGenericMethod;
        }

        public MethodInfo Value
        {
            get
            {
                return _unclosedGenericMethod
                    .MakeGenericMethod(GetTypedArguments());
            }
        }

        private Type[] GetTypedArguments()
        {
            return _unclosedGenericMethod
                .GetGenericArguments()
                .Select(t => t.IsGenericParameter
                    ? new AutoGenericArgument(_specimenBuilder, t).Value
                    : t)
                .ToArray();
        }
    }

    private class AutoGenericArgumentCollection : KeyedCollection<string, AutoGenericArgument>
    {
        protected override string GetKeyForItem(AutoGenericArgument item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            return item.GenericArgument.Name;
        }
    }

    private class AutoGenericArgument
    {
        private readonly ISpecimenBuilder _specimenBuilder;
        private Type _value;

        public AutoGenericArgument(ISpecimenBuilder specimenBuilder, Type genericArgument)
        {
            _specimenBuilder = specimenBuilder;
            GenericArgument = genericArgument;
        }

        public Type GenericArgument { get; }

        public Type Value
        {
            get
            {
                if (_value == null)
                {
                    _value = new DynamicDummyType(
                            _specimenBuilder, GetBaseType(), GetInterfaces())
                        .Value;
                }

                return _value;
            }
        }

        private Type GetBaseType()
        {
            if (HasClassConstraint())
            {
                return typeof(object);
            }

            return GetConstraintType() ?? typeof(ValueType);
        }

        private Type GetConstraintType()
        {
            return GenericArgument
                .GetGenericParameterConstraints()
                .SingleOrDefault(t => !t.IsInterface);
        }

        private bool HasClassConstraint()
        {
            return (GenericArgument.GenericParameterAttributes
                    & GenericParameterAttributes.ReferenceTypeConstraint)
                   == GenericParameterAttributes.ReferenceTypeConstraint;
        }

        private Type[] GetInterfaces()
        {
            return GenericArgument
                .GetGenericParameterConstraints()
                .Where(t => t.IsInterface)
                .ToArray();
        }
    }

    private class DynamicDummyType
    {
        private const string SpecimenBuilderFieldName = "specimenBuilder";

        private static readonly AssemblyBuilder s_assemblyBuilder =
            AssemblyBuilder.DefineDynamicAssembly(
                new AssemblyName("AutoFixture.DynamicProxyAssembly"),
                AssemblyBuilderAccess.Run);

        private static readonly ModuleBuilder s_moduleBuilder =
            s_assemblyBuilder.DefineDynamicModule("DynamicProxyModule");

        private static readonly MethodInfo s_fixtureCreateGenericMethod =
            typeof(SpecimenFactory).GetMethod("Create", new[] { typeof(ISpecimenBuilder) });

        private readonly ISpecimenBuilder _specimenBuilder;
        private readonly Type _baseType;
        private readonly Type[] _interfaces;
        private ConstructorBuilder _constructorBuilder;
        private TypeBuilder _typeBuilder;
        private MethodBuilder _methodBuilder;
        private MethodInfo _methodInfo;
        private FieldBuilder _specimenBuilderFieldBuilder;
        private ConstructorInfo _baseTypeConstructor;

        public DynamicDummyType(ISpecimenBuilder specimenBuilder, Type baseType, Type[] interfaces)
        {
            _specimenBuilder = specimenBuilder;
            _baseType = baseType;
            _interfaces = interfaces;
        }

        public Type Value
        {
            get
            {
                DefineTypeBuilder();
                ImplementDefaultConstructor();
                ImplementAbstractMethods();
                ImplementInterfaceMethods();
                var dummyType = _typeBuilder.CreateTypeInfo();
                SetStaticSpecimenBuilderField(dummyType);
                return dummyType;
            }
        }

        private void DefineTypeBuilder()
        {
            lock (s_moduleBuilder)
            {
                _typeBuilder = s_moduleBuilder.DefineType(
                    GetBaseTypeName(),
                    TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.Sealed,
                    _baseType,
                    _interfaces);
            }
        }

        private string GetBaseTypeName()
        {
#if NET5_0_OR_GREATER
                return _baseType.Name + Guid.NewGuid().ToString().Replace("-", string.Empty, StringComparison.OrdinalIgnoreCase);
#else
            return _baseType.Name + Guid.NewGuid().ToString().Replace("-", string.Empty);
#endif
        }

        private void ImplementDefaultConstructor()
        {
            DefineConstructorBuilder();
            SetBaseTypeConstructor();
            EmitDefaultConstructor();
        }

        private void DefineConstructorBuilder()
        {
            _constructorBuilder = _typeBuilder.DefineConstructor(
                MethodAttributes.Public, CallingConventions.Standard, new Type[0]);
        }

        private void SetBaseTypeConstructor()
        {
            _baseTypeConstructor =
                _baseType
                    .GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(c => c.IsPublic || c.IsFamilyOrAssembly || c.IsFamily)
                    .OrderBy(c => c.GetParameters().Length)
                    .FirstOrDefault();

            EnsureBaseTypeConstructorIsAccessible();
        }

        private void EnsureBaseTypeConstructorIsAccessible()
        {
            if (_baseTypeConstructor != null)
            {
                return;
            }

            var message = "Cannot create a dummy type because the base type '{0}' does not have any accessible " +
                          "constructor.";

            throw new ArgumentException(string.Format(
                CultureInfo.CurrentCulture,
                message,
                _typeBuilder.BaseType));
        }

        private void EmitDefaultConstructor()
        {
            var generator = _constructorBuilder.GetILGenerator();
            if (_baseTypeConstructor.GetParameters().Any())
            {
                DefineStaticSpecimenBuilderFieldBuilder();
                EmitCallBaseTypeConstructor(generator);
            }

            generator.Emit(OpCodes.Ret);
        }

        private void DefineStaticSpecimenBuilderFieldBuilder()
        {
            if (_specimenBuilderFieldBuilder != null)
            {
                return;
            }

            _specimenBuilderFieldBuilder = _typeBuilder.DefineField(
                SpecimenBuilderFieldName,
                typeof(IFixture),
                FieldAttributes.Private | FieldAttributes.Static);
        }

        private void EmitCallBaseTypeConstructor(ILGenerator generator)
        {
            generator.Emit(OpCodes.Ldarg_0);
            foreach (var parameterInfo in _baseTypeConstructor.GetParameters())
            {
                EmitCallFixtureCreate(generator, parameterInfo.ParameterType);
            }

            generator.Emit(OpCodes.Call, _baseTypeConstructor);
        }

        private void EmitCallFixtureCreate(ILGenerator generator, Type returnType)
        {
            generator.Emit(OpCodes.Ldsfld, _specimenBuilderFieldBuilder);
            generator.Emit(OpCodes.Call, s_fixtureCreateGenericMethod.MakeGenericMethod(returnType));
        }

        private void ImplementAbstractMethods()
        {
            foreach (MethodInfo method in GetAbstractMethods())
            {
                _methodInfo = method;
                ImplementMethod();
            }
        }

        private void ImplementInterfaceMethods()
        {
            foreach (var @interface in _interfaces)
            {
                ImplementInterfaceMethods(@interface);
            }
        }

        private void ImplementInterfaceMethods(Type @interface)
        {
            foreach (var method in @interface.GetMethods())
            {
                _methodInfo = method;
                ImplementMethod();
            }

            foreach (Type parentType in @interface.GetInterfaces())
            {
                ImplementInterfaceMethods(parentType);
            }
        }

        private IEnumerable<MethodInfo> GetAbstractMethods()
        {
            return _typeBuilder.BaseType
                .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(m => m.IsAbstract);
        }

        private void ImplementMethod()
        {
            DefineMethodBuilder();
            DefineMethodGenericParameters();
            DefineStaticSpecimenBuilderFieldBuilder();
            EmitReturningDefaultValue();
        }

        private void DefineMethodBuilder()
        {
            _methodBuilder = _typeBuilder.DefineMethod(
                _methodInfo.Name,
                MethodAttributes.Public | MethodAttributes.Virtual,
                CallingConventions.Standard,
                _methodInfo.ReturnType,
                _methodInfo.GetParameters().Select(p => p.ParameterType).ToArray());
        }

        private void DefineMethodGenericParameters()
        {
            var genericArguments = _methodInfo.GetGenericArguments();
            if (genericArguments.Any())
            {
                var typeParameters =
                    _methodBuilder.DefineGenericParameters(genericArguments.Select(a => a.Name).ToArray());
                for (int i = 0; i < genericArguments.Length; i++)
                {
                    DefineMethodGenericConstraints(genericArguments[i], typeParameters[i]);
                }
            }
        }

        private static void DefineMethodGenericConstraints(Type genericArgument,
            GenericTypeParameterBuilder typeParameter)
        {
            typeParameter.SetGenericParameterAttributes(genericArgument.GenericParameterAttributes);
            typeParameter.SetBaseTypeConstraint(genericArgument.BaseType);
            typeParameter.SetInterfaceConstraints(genericArgument.GetInterfaces());
        }

        private void EmitReturningDefaultValue()
        {
            var generator = _methodBuilder.GetILGenerator();
            if (_methodBuilder.ReturnType != typeof(void))
            {
                EmitCallFixtureCreate(generator, _methodInfo.ReturnType);
            }

            generator.Emit(OpCodes.Ret);
        }

        private void SetStaticSpecimenBuilderField(Type dummyType)
        {
            if (_specimenBuilderFieldBuilder == null)
            {
                return;
            }

            dummyType.GetField(SpecimenBuilderFieldName, BindingFlags.Static | BindingFlags.NonPublic)
                .SetValue(null, _specimenBuilder);
        }
    }
}