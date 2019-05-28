using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using AutoFixture.Kernel;

namespace AutoFixture.Idioms
{
    /// <summary>
    /// Utility to take open generic types and close them respecting the generic constrains.
    /// </summary>
    internal class OpenGenericTypeClosingUtil
    {
        private ISpecimenBuilder Builder { get; }

        public OpenGenericTypeClosingUtil(ISpecimenBuilder builder)
        {
            this.Builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        public ConstructorInfo CloseGenericType(ConstructorInfo constructorInfo)
        {
            return (ConstructorInfo)this.ResolveUnclosedGenericType(constructorInfo, t => t.GetConstructors());
        }

        public PropertyInfo CloseGenericType(PropertyInfo propertyInfo)
        {
            return propertyInfo.ReflectedType.ContainsGenericParameters
                ? new AutoGenericType(this.Builder, propertyInfo.ReflectedType)
                    .Value
                    .GetProperties()
                    .Single(pi => string.Equals(pi.Name, propertyInfo.Name, StringComparison.Ordinal))
                : propertyInfo;
        }

        public MethodInfo CloseGenericMethod(MethodInfo methodInfo)
        {
            return methodInfo.ContainsGenericParameters
                ? new AutoGenericMethod(this.Builder, methodInfo)
                    .Value
                : methodInfo;
        }

        public MethodInfo CloseGenericType(MethodInfo methodInfo)
        {
            return (MethodInfo)this.ResolveUnclosedGenericType(methodInfo, t => t.GetMethods());
        }

        private MethodBase ResolveUnclosedGenericType(MethodBase method,
            Func<Type, IEnumerable<MethodBase>> methodSetToMatch)
        {
            if (!method.ReflectedType.ContainsGenericParameters)
            {
                return method;
            }

            var autoGenericType = new AutoGenericType(this.Builder, method.ReflectedType);
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
            private readonly ISpecimenBuilder specimenBuilder;
            private readonly Type unclosedGenericType;
            private readonly AutoGenericArgumentCollection autoGenericArguments;

            public AutoGenericType(ISpecimenBuilder specimenBuilder, Type unclosedGenericType)
            {
                this.specimenBuilder = specimenBuilder;
                this.unclosedGenericType = unclosedGenericType;
                this.autoGenericArguments = new AutoGenericArgumentCollection();
            }

            public Type Value
            {
                get
                {
                    return this.unclosedGenericType
                        .GetGenericTypeDefinition()
                        .MakeGenericType(this.GetTypedArguments());
                }
            }

            public IEnumerable<Type> ResolveUnclosedParameterTypes(IEnumerable<ParameterInfo> parameterInfos)
            {
                return parameterInfos.Select(
                    pi => pi.ParameterType.IsByRef
                        ? this.ResolveUnclosedParameterType(pi.ParameterType.GetElementType()).MakeByRefType()
                        : this.ResolveUnclosedParameterType(pi.ParameterType));
            }

            private Type ResolveUnclosedParameterType(Type parameterType)
            {
                if (parameterType.IsArray)
                    return this.ResolveNestedArrayParameterType(parameterType);

                if (parameterType.IsGenericType)
                    return this.ReosolveNestedGenericParameterType(parameterType);

                return this.ResolveGenericParameter(parameterType);
            }

            private Type ResolveNestedArrayParameterType(Type parameterType)
            {
                var elementType = this.ResolveUnclosedParameterType(parameterType.GetElementType());
                var rank = parameterType.GetArrayRank();
                return rank == 1 ? elementType.MakeArrayType() : elementType.MakeArrayType(rank);
            }

            private Type ReosolveNestedGenericParameterType(Type parameterType)
            {
                var genericArguments = parameterType.GetGenericArguments();
                var typeArguments = genericArguments.Select(this.ResolveUnclosedParameterType).ToArray();
                return parameterType.GetGenericTypeDefinition().MakeGenericType(typeArguments);
            }

            private Type ResolveGenericParameter(Type parameterType)
            {
                return this.IsGenericTypeParameter(parameterType)
                    ? this.autoGenericArguments[parameterType.Name].Value
                    : parameterType;
            }

            private bool IsGenericTypeParameter(Type parameterType)
            {
                return parameterType.IsGenericParameter
                    && this.autoGenericArguments.Contains(parameterType.Name);
            }

            private Type[] GetTypedArguments()
            {
                return this.unclosedGenericType
                    .GetGenericArguments()
                    .Select(t =>
                    {
                        if (!t.IsGenericParameter)
                        {
                            return t;
                        }

                        var autoGenericArgument = new AutoGenericArgument(this.specimenBuilder, t);
                        this.autoGenericArguments.Add(autoGenericArgument);
                        return autoGenericArgument.Value;
                    })
                    .ToArray();
            }
        }

        private class AutoGenericMethod
        {
            private readonly ISpecimenBuilder specimenBuilder;
            private readonly MethodInfo unclosedGenericMethod;

            public AutoGenericMethod(ISpecimenBuilder specimenBuilder, MethodInfo unclosedGenericMethod)
            {
                this.specimenBuilder = specimenBuilder;
                this.unclosedGenericMethod = unclosedGenericMethod;
            }

            public MethodInfo Value
            {
                get
                {
                    return this.unclosedGenericMethod
                        .MakeGenericMethod(this.GetTypedArguments());
                }
            }

            private Type[] GetTypedArguments()
            {
                return this.unclosedGenericMethod
                    .GetGenericArguments()
                    .Select(t => t.IsGenericParameter
                        ? new AutoGenericArgument(this.specimenBuilder, t).Value
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
            private readonly ISpecimenBuilder specimenBuilder;
            private Type value;

            public AutoGenericArgument(ISpecimenBuilder specimenBuilder, Type genericArgument)
            {
                this.specimenBuilder = specimenBuilder;
                this.GenericArgument = genericArgument;
            }

            public Type GenericArgument { get; }

            public Type Value
            {
                get
                {
                    if (this.value == null)
                    {
                        this.value = new DynamicDummyType(
                                this.specimenBuilder, this.GetBaseType(), this.GetInterfaces())
                            .Value;
                    }

                    return this.value;
                }
            }

            private Type GetBaseType()
            {
                if (this.HasClassConstraint())
                {
                    return typeof(object);
                }

                return this.GetConstraintType() ?? typeof(ValueType);
            }

            private Type GetConstraintType()
            {
                return this.GenericArgument
                    .GetGenericParameterConstraints()
                    .SingleOrDefault(t => !t.IsInterface);
            }

            private bool HasClassConstraint()
            {
                return (this.GenericArgument.GenericParameterAttributes
                        & GenericParameterAttributes.ReferenceTypeConstraint)
                       == GenericParameterAttributes.ReferenceTypeConstraint;
            }

            private Type[] GetInterfaces()
            {
                return this.GenericArgument
                    .GetGenericParameterConstraints()
                    .Where(t => t.IsInterface)
                    .ToArray();
            }
        }

        private class DynamicDummyType
        {
            private const string SpecimenBuilderFieldName = "specimenBuilder";

            private static readonly AssemblyBuilder AssemblyBuilder =
                AssemblyBuilder.DefineDynamicAssembly(
                    new AssemblyName("AutoFixture.DynamicProxyAssembly"),
                    AssemblyBuilderAccess.Run);

            private static readonly ModuleBuilder ModuleBuilder =
                AssemblyBuilder.DefineDynamicModule("DynamicProxyModule");

            private static readonly MethodInfo FixtureCreateGenericMethod =
                typeof(SpecimenFactory).GetMethod("Create", new[] { typeof(ISpecimenBuilder) });

            private readonly ISpecimenBuilder specimenBuilder;
            private readonly Type baseType;
            private readonly Type[] interfaces;
            private ConstructorBuilder constructorBuilder;
            private TypeBuilder typeBuilder;
            private MethodBuilder methodBuilder;
            private MethodInfo methodInfo;
            private FieldBuilder specimenBuilderFieldBuilder;
            private ConstructorInfo baseTypeConstructor;

            public DynamicDummyType(ISpecimenBuilder specimenBuilder, Type baseType, Type[] interfaces)
            {
                this.specimenBuilder = specimenBuilder;
                this.baseType = baseType;
                this.interfaces = interfaces;
            }

            public Type Value
            {
                get
                {
                    this.DefineTypeBuilder();
                    this.ImplementDefaultConstructor();
                    this.ImplementAbstractMethods();
                    this.ImplementInterfaceMethods();
                    var dummyType = this.typeBuilder.CreateTypeInfo();
                    this.SetStaticSpecimenBuilderField(dummyType);
                    return dummyType;
                }
            }

            private void DefineTypeBuilder()
            {
                lock (ModuleBuilder)
                {
                    this.typeBuilder = ModuleBuilder.DefineType(
                        this.GetBaseTypeName(),
                        TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.Sealed,
                        this.baseType,
                        this.interfaces);
                }
            }

            private string GetBaseTypeName()
            {
                return this.baseType.Name + Guid.NewGuid().ToString().Replace("-", string.Empty);
            }

            private void ImplementDefaultConstructor()
            {
                this.DefineConstructorBuilder();
                this.SetBaseTypeConstructor();
                this.EmitDefaultConstructor();
            }

            private void DefineConstructorBuilder()
            {
                this.constructorBuilder = this.typeBuilder.DefineConstructor(
                    MethodAttributes.Public, CallingConventions.Standard, new Type[0]);
            }

            private void SetBaseTypeConstructor()
            {
                this.baseTypeConstructor =
                    this.baseType
                        .GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                        .Where(c => c.IsPublic || c.IsFamilyOrAssembly || c.IsFamily)
                        .OrderBy(c => c.GetParameters().Length)
                        .FirstOrDefault();

                this.EnsureBaseTypeConstructorIsAccessible();
            }

            private void EnsureBaseTypeConstructorIsAccessible()
            {
                if (this.baseTypeConstructor != null)
                {
                    return;
                }

                var message = "Cannot create a dummy type because the base type '{0}' does not have any accessible " +
                              "constructor.";

                throw new ArgumentException(string.Format(
                    CultureInfo.CurrentCulture,
                    message,
                    this.typeBuilder.BaseType));
            }

            private void EmitDefaultConstructor()
            {
                var generator = this.constructorBuilder.GetILGenerator();
                if (this.baseTypeConstructor.GetParameters().Any())
                {
                    this.DefineStaticSpecimenBuilderFieldBuilder();
                    this.EmitCallBaseTypeConstructor(generator);
                }

                generator.Emit(OpCodes.Ret);
            }

            private void DefineStaticSpecimenBuilderFieldBuilder()
            {
                if (this.specimenBuilderFieldBuilder != null)
                {
                    return;
                }

                this.specimenBuilderFieldBuilder = this.typeBuilder.DefineField(
                    SpecimenBuilderFieldName,
                    typeof(IFixture),
                    FieldAttributes.Private | FieldAttributes.Static | FieldAttributes.InitOnly);
            }

            private void EmitCallBaseTypeConstructor(ILGenerator generator)
            {
                generator.Emit(OpCodes.Ldarg_0);
                foreach (var parameterInfo in this.baseTypeConstructor.GetParameters())
                {
                    this.EmitCallFixtureCreate(generator, parameterInfo.ParameterType);
                }

                generator.Emit(OpCodes.Call, this.baseTypeConstructor);
            }

            private void EmitCallFixtureCreate(ILGenerator generator, Type returnType)
            {
                generator.Emit(OpCodes.Ldsfld, this.specimenBuilderFieldBuilder);
                generator.Emit(OpCodes.Call, FixtureCreateGenericMethod.MakeGenericMethod(returnType));
            }

            private void ImplementAbstractMethods()
            {
                foreach (MethodInfo method in this.GetAbstractMethods())
                {
                    this.methodInfo = method;
                    this.ImplementMethod();
                }
            }

            private void ImplementInterfaceMethods()
            {
                foreach (var @interface in this.interfaces)
                {
                    this.ImplementInterfaceMethods(@interface);
                }
            }

            private void ImplementInterfaceMethods(Type @interface)
            {
                foreach (var method in @interface.GetMethods())
                {
                    this.methodInfo = method;
                    this.ImplementMethod();
                }

                foreach (Type parentType in @interface.GetInterfaces())
                {
                    this.ImplementInterfaceMethods(parentType);
                }
            }

            private IEnumerable<MethodInfo> GetAbstractMethods()
            {
                return this.typeBuilder.BaseType
                    .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(m => m.IsAbstract);
            }

            private void ImplementMethod()
            {
                this.DefineMethodBuilder();
                this.DefineMethodGenericParameters();
                this.DefineStaticSpecimenBuilderFieldBuilder();
                this.EmitReturningDefaultValue();
            }

            private void DefineMethodBuilder()
            {
                this.methodBuilder = this.typeBuilder.DefineMethod(
                    this.methodInfo.Name,
                    MethodAttributes.Public | MethodAttributes.Virtual,
                    CallingConventions.Standard,
                    this.methodInfo.ReturnType,
                    this.methodInfo.GetParameters().Select(p => p.ParameterType).ToArray());
            }

            private void DefineMethodGenericParameters()
            {
                var genericArguments = this.methodInfo.GetGenericArguments();
                if (genericArguments.Any())
                {
                    var typeParameters =
                        this.methodBuilder.DefineGenericParameters(genericArguments.Select(a => a.Name).ToArray());
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
                var generator = this.methodBuilder.GetILGenerator();
                if (this.methodBuilder.ReturnType != typeof(void))
                {
                    this.EmitCallFixtureCreate(generator, this.methodInfo.ReturnType);
                }

                generator.Emit(OpCodes.Ret);
            }

            private void SetStaticSpecimenBuilderField(Type dummyType)
            {
                if (this.specimenBuilderFieldBuilder == null)
                {
                    return;
                }

                dummyType.GetField(SpecimenBuilderFieldName, BindingFlags.Static | BindingFlags.NonPublic)
                    .SetValue(null, this.specimenBuilder);
            }
        }
    }
}
