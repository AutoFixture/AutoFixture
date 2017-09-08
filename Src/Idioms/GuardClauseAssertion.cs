using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Encapsulates a unit test that verifies that a method or constructor has appropriate Guard
    /// Clauses in place.
    /// </summary>
    public class GuardClauseAssertion : IdiomaticAssertion
    {
        private readonly ISpecimenBuilder builder;
        private readonly IBehaviorExpectation behaviorExpectation;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuardClauseAssertion" /> class.
        /// </summary>
        /// <param name="builder">
        /// A composer which can create instances required to implement the idiomatic unit test.
        /// </param>
        /// <remarks>
        /// <para>
        /// <paramref name="builder" /> will typically be a <see cref="Fixture" /> instance.
        /// </para>
        /// </remarks>
        public GuardClauseAssertion(ISpecimenBuilder builder)
            : this(builder, new CompositeBehaviorExpectation(
                new NullReferenceBehaviorExpectation(),
                new EmptyGuidBehaviorExpectation()))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GuardClauseAssertion" /> class.
        /// </summary>
        /// <param name="builder">
        /// A composer which can create instances required to implement the idiomatic unit test.
        /// </param>
        /// <param name="behaviorExpectation">
        /// A behavior expectation to override the default expectation.
        /// </param>
        /// <remarks>
        /// <para>
        /// <paramref name="builder" /> will typically be a <see cref="Fixture" /> instance.
        /// </para>
        /// </remarks>
        public GuardClauseAssertion(ISpecimenBuilder builder, IBehaviorExpectation behaviorExpectation)
        {
            this.builder = builder;
            this.behaviorExpectation = behaviorExpectation;
        }

        /// <summary>
        /// Gets the builder supplied via the constructor.
        /// </summary>
        public ISpecimenBuilder Builder
        {
            get { return this.builder; }
        }

        /// <summary>
        /// Gets the behavior expectation.
        /// </summary>
        /// <remarks>
        /// <para>
        /// GuardClauseAssertion contains an appropriate default implementation of
        /// <see cref="IBehaviorExpectation" />, but a custom behavior can also be supplied via one
        /// of the constructor overloads. In any case, this property exposes the behavior
        /// expectation.
        /// </para>
        /// </remarks>
        /// <seealso cref="GuardClauseAssertion(ISpecimenBuilder, IBehaviorExpectation)" />
        public IBehaviorExpectation BehaviorExpectation
        {
            get { return this.behaviorExpectation; }
        }

        /// <summary>
        /// Verifies that a constructor has appropriate Guard Clauses in place.
        /// </summary>
        /// <param name="constructorInfo">The constructor.</param>
        /// <remarks>
        /// <para>
        /// Exactly which Guard Clauses are verified is defined by
        /// <see cref="BehaviorExpectation" />.
        /// </para>
        /// </remarks>
        public override void Verify(ConstructorInfo constructorInfo)
        {
            if (constructorInfo == null)
                throw new ArgumentNullException("constructorInfo");

            constructorInfo = this.ResolveUnclosedGenericType(constructorInfo);

            var method = new ConstructorMethod(constructorInfo);
            this.Verify(method, false, false);
        }

        /// <summary>
        /// Verifies that a method has appropriate Guard Clauses in place.
        /// </summary>
        /// <param name="methodInfo">The method.</param>
        /// <remarks>
        /// <para>
        /// Exactly which Guard Clauses are verified is defined by
        /// <see cref="BehaviorExpectation" />.
        /// </para>
        /// </remarks>
        public override void Verify(MethodInfo methodInfo)
        {
            if (methodInfo == null)
                throw new ArgumentNullException("methodInfo");

            if (methodInfo.IsEqualsMethod() ||
                methodInfo.IsGetHashCodeMethod() ||
                methodInfo.IsToString() ||
                methodInfo.IsGetType() ||
                methodInfo.IsAbstract)
                return;

            methodInfo = this.ResolveUnclosedGenericType(methodInfo);
            methodInfo = this.ResolveUnclosedGenericMethod(methodInfo);

            var method = this.CreateMethod(methodInfo);

            var isReturnValueIterator =
                typeof(System.Collections.IEnumerable).IsAssignableFrom(methodInfo.ReturnType) ||
                    typeof(System.Collections.IEnumerator).IsAssignableFrom(methodInfo.ReturnType);

            var isReturnValueNonDeferred = IsNonDeferredEnumerable(methodInfo.ReturnType);
            var isReturnValueDeferable = isReturnValueIterator && !isReturnValueNonDeferred;

            var isReturnValueTask =
                typeof(System.Threading.Tasks.Task).IsAssignableFrom(methodInfo.ReturnType);

            this.Verify(method, isReturnValueDeferable, isReturnValueTask);
        }

        private static bool IsNonDeferredEnumerable(Type t)
        {
            var nonGenericCollectionTypes = new[]
            {
                typeof(System.Collections.ICollection),
                typeof(System.Collections.IList),
                typeof(System.Collections.IDictionary)
            };

            var genericCollectionTypeGtds = new[]
            {
                typeof(IList<>),
                typeof(ICollection<>),
                typeof(IDictionary<,>)
            };

            var isGeneric = t.IsGenericType;

            var gtdInterfaces = (isGeneric && !t.IsInterface)
                ? t.GetInterfaces()
                    .Where(i => i.IsGenericType)
                    .Select(i => i.GetGenericTypeDefinition())
                    .ToArray()
                : (isGeneric && t.IsInterface)
                    ? new[] { t.GetGenericTypeDefinition() }
                    : null;

            return t.IsArray ||
                nonGenericCollectionTypes.Any(gt => gt.IsAssignableFrom(t)) ||
                (isGeneric && (genericCollectionTypeGtds.Any(gtd => gtdInterfaces.Contains(gtd))));
        }

        /// <summary>
        /// Verifies that a property has appropriate Guard Clauses in place.
        /// </summary>
        /// <param name="propertyInfo">The property.</param>
        /// <remarks>
        /// <para>
        /// Exactly which Guard Clauses are verified is defined by
        /// <see cref="BehaviorExpectation" />.
        /// </para>
        /// </remarks>
        public override void Verify(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException("propertyInfo");

            if (propertyInfo.GetSetMethod() == null)
                return;

            propertyInfo = this.ResolveUnclosedGenericType(propertyInfo);

            var owner = this.CreateOwner(propertyInfo);
            var command = new PropertySetCommand(propertyInfo, owner);
            var unwrapper = new ReflectionExceptionUnwrappingCommand(command);
            this.BehaviorExpectation.Verify(unwrapper);
        }

        private static bool IsMatched(MethodBase resolved, MethodBase method, AutoGenericType autoGenericType)
        {
            return resolved.Name == method.Name &&
                resolved.GetParameters()
                    .Select(pi => pi.ParameterType)
                    .SequenceEqual(autoGenericType.ResolveUnclosedParameterTypes(method.GetParameters()));
        }

        private IMethod CreateMethod(MethodInfo methodInfo)
        {
            var owner = this.CreateOwner(methodInfo);
            return owner != null
                ? (IMethod)new InstanceMethod(methodInfo, owner)
                : new StaticMethod(methodInfo);
        }

        private object CreateOwner(PropertyInfo property)
        {
            return this.CreateOwner(property.GetSetMethod());
        }

        private object CreateOwner(MethodBase method)
        {
            return method.IsStatic ? null : this.CreateOwner(method.ReflectedType);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "AutoFixture", Justification = "Workaround for a bug in CA: https://connect.microsoft.com/VisualStudio/feedback/details/521030/")]
        private object CreateOwner(Type type)
        {
            try
            {
                return this.Builder.CreateAnonymous(type);
            }
            catch (ObjectCreationException e)
            {
                throw new GuardClauseException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        "AutoFixture was unable to create an instance of type {0}. Please check the inner exception for more details",
                        type),
                    e);
            }
        }

        private void Verify(IMethod method, bool isReturnValueDeferable, bool isReturnValueTask)
        {
            if (isReturnValueDeferable)
                VerifyDeferrableIterator(method);
            else if (isReturnValueTask)
                VerifyDeferrableTask(method);
            else
                VerifyNormal(method);
        }

        private void VerifyDeferrableIterator(IMethod method)
        {
            foreach (var command in GetParameterGuardCommands(method))
            {
                this.BehaviorExpectation.Verify(new IteratorMethodInvokeCommand(command));
            }
        }

        private void VerifyDeferrableTask(IMethod method)
        {
            foreach (var command in GetParameterGuardCommands(method))
            {
                this.BehaviorExpectation.Verify(new TaskReturnMethodInvokeCommand(command));
            }
        }

        private void VerifyNormal(IMethod method)
        {
            foreach (var command in GetParameterGuardCommands(method))
            {
                this.BehaviorExpectation.Verify(command);
            }
        }

        private IEnumerable<ReflectionExceptionUnwrappingCommand> GetParameterGuardCommands(IMethod method)
        {
            var arguments = this.GetParameters(method);
            return from pi in method.Parameters
                   where !pi.IsOut
                   let expansion = new IndexedReplacement<object>(pi.Position, arguments)
                   select new MethodInvokeCommand(method, expansion, pi)
                   into command
                   select new ReflectionExceptionUnwrappingCommand(command);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "AutoFixture", Justification = "Workaround for a bug in CA: https://connect.microsoft.com/VisualStudio/feedback/details/521030/")]
        private List<object> GetParameters(IMethod method)
        {
            var result = new List<object>();
            foreach (var pi in method.Parameters)
            {
                try
                {
                    result.Add(this.Builder.CreateAnonymous(GetParameterType(pi)));
                }
                catch (ObjectCreationException e)
                {
                    throw new GuardClauseException(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            "AutoFixture was unable to create an instance for parameter \"{1}\" of method \"{2}\".{0}Method Signature: {3}{0}Declaring Type: {4}{0}Reflected Type: {5}",
                            Environment.NewLine,
                            pi.Name,
                            pi.Member.Name,
                            pi.Member,
                            pi.Member.DeclaringType,
                            pi.Member.ReflectedType),
                        e);
                }
            }

            return result;
        }

        private static Type GetParameterType(ParameterInfo pi)
        {
            var pType = pi.ParameterType;
            return pType.IsByRef ? pType.GetElementType() : pi.ParameterType;
        }

        private ConstructorInfo ResolveUnclosedGenericType(ConstructorInfo constructorInfo)
        {
            return (ConstructorInfo)this.ResolveUnclosedGenericType(constructorInfo, t => t.GetConstructors());
        }

        private PropertyInfo ResolveUnclosedGenericType(PropertyInfo propertyInfo)
        {
            return propertyInfo.ReflectedType.ContainsGenericParameters
                ? new AutoGenericType(this.Builder, propertyInfo.ReflectedType)
                    .Value
                    .GetProperties()
                    .Single(pi => pi.Name == propertyInfo.Name)
                : propertyInfo;
        }

        private MethodInfo ResolveUnclosedGenericType(MethodInfo methodInfo)
        {
            return (MethodInfo)this.ResolveUnclosedGenericType(methodInfo, t => t.GetMethods());
        }

        private MethodBase ResolveUnclosedGenericType(
            MethodBase method, Func<Type, IEnumerable<MethodBase>> methodSetToMatch)
        {
            if (!method.ReflectedType.ContainsGenericParameters)
            {
                return method;
            }

            var autoGenericType = new AutoGenericType(this.Builder, method.ReflectedType);
            return methodSetToMatch(autoGenericType.Value).Single(c => IsMatched(c, method, autoGenericType));
        }

        private MethodInfo ResolveUnclosedGenericMethod(MethodInfo methodInfo)
        {
            return methodInfo.ContainsGenericParameters
                ? new AutoGenericMethod(this.Builder, methodInfo)
                    .Value
                : methodInfo;
        }

        private class TaskReturnMethodInvokeCommand : IGuardClauseCommand
        {
            private const string message = @"A Guard Clause test was performed on a method that returns a Task, Task<T> (possibly in an 'async' method), but the test failed. See the inner exception for more details. However, because of the async nature of the task, this test failure may look like a false positive. Perhaps you already have a Guard Clause in place, but inside the Task or inside a method marked with the 'async' keyword (if you're using C#); if this is the case, the Guard Clause is dormant, and will first be triggered when a client accesses the Result of the Task. This doesn't adhere to the Fail Fast principle, so should be addressed.
See https://github.com/AutoFixture/AutoFixture/issues/268 for more details.";

            private readonly IGuardClauseCommand command;

            public TaskReturnMethodInvokeCommand(IGuardClauseCommand command)
            {
                this.command = command;
            }

            public Type RequestedType
            {
                get { return this.command.RequestedType; }
            }

            public string RequestedParameterName
            {
                get { return this.command.RequestedParameterName; }
            }

            public void Execute(object value)
            {
                this.command.Execute(value);
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "AutoFixture",
                Justification = "False Positive. Code Analysis really shouldn't attempt to spell check URLs.")]
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "github",
                Justification = "False Positive. Code Analysis really shouldn't attempt to spell check URLs.")]
            public Exception CreateException(string value)
            {
                var e = this.command.CreateException(value);
                return new GuardClauseException(message, e);
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "AutoFixture"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "github")]
            public Exception CreateException(string value, Exception innerException)
            {
                var e = this.command.CreateException(value, innerException);
                return new GuardClauseException(message, e);
            }
        }

        private class IteratorMethodInvokeCommand : IGuardClauseCommand
        {
            private const string message = @"A Guard Clause test was performed on a method that may contain a deferred iterator block, but the test failed. See the inner exception for more details. However, because of the deferred nature of the iterator block, this test failure may look like a false positive. Perhaps you already have a Guard Clause in place, but in conjunction with the 'yield' keyword (if you're using C#); if this is the case, the Guard Clause is dormant, and will first be triggered when a client starts looping over the iterator. This doesn't adhere to the Fail Fast principle, so should be addressed.
See e.g. http://codeblog.jonskeet.uk/2008/03/02/c-4-idea-iterator-blocks-and-parameter-checking/ for more details.";

            private readonly IGuardClauseCommand command;

            public IteratorMethodInvokeCommand(IGuardClauseCommand command)
            {
                this.command = command;
            }

            public Type RequestedType
            {
                get { return this.command.RequestedType; }
            }

            public string RequestedParameterName
            {
                get { return this.command.RequestedParameterName; }
            }

            public void Execute(object value)
            {
                this.command.Execute(value);
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "codeblog", Justification = "False Positive. Code Analysis really shouldn't attempt to spell check URLs.")]
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "uk", Justification = "False Positive. Code Analysis really shouldn't attempt to spell check URLs.")]
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "jonskeet", Justification = "False Positive. Code Analysis really shouldn't attempt to spell check URLs.")]
            public Exception CreateException(string value)
            {
                var e = this.command.CreateException(value);
                return new GuardClauseException(message, e);
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "codeblog", Justification = "False Positive. Code Analysis really shouldn't attempt to spell check URLs.")]
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "uk", Justification = "False Positive. Code Analysis really shouldn't attempt to spell check URLs.")]
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "jonskeet", Justification = "False Positive. Code Analysis really shouldn't attempt to spell check URLs.")]
            public Exception CreateException(string value, Exception innerException)
            {
                var e = this.command.CreateException(value, innerException);
                return new GuardClauseException(message, e);
            }
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
                if (item == null)
                {
                    throw new ArgumentNullException("item");
                }

                return item.GenericArgument.Name;
            }
        }

        private class AutoGenericArgument
        {
            private readonly ISpecimenBuilder specimenBuilder;
            private readonly Type genericArgument;
            private Type value;

            public AutoGenericArgument(ISpecimenBuilder specimenBuilder, Type genericArgument)
            {
                this.specimenBuilder = specimenBuilder;
                this.genericArgument = genericArgument;
            }

            public Type GenericArgument
            {
                get
                {
                    return this.genericArgument;
                }
            }

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
                    .Where(t => !t.IsInterface)
                    .SingleOrDefault();
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
            private const string specimenBuilderFieldName = "specimenBuilder";

            private static readonly AssemblyBuilder assemblyBuilder =
                AppDomain.CurrentDomain.DefineDynamicAssembly(
                    new AssemblyName("AutoFixture.DynamicProxyAssembly"),
                    AssemblyBuilderAccess.Run);

            private static readonly ModuleBuilder moduleBuilder =
                assemblyBuilder.DefineDynamicModule("DynamicProxyModule");

            private static readonly MethodInfo fixtureCreateGenericMethod =
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
                    var dummyType = this.typeBuilder.CreateType();
                    this.SetStaticSpecimenBuilderField(dummyType);
                    return dummyType;
                }
            }

            private void DefineTypeBuilder()
            {
                lock (moduleBuilder)
                {
                    this.typeBuilder = moduleBuilder.DefineType(
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
                    specimenBuilderFieldName,
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
                generator.Emit(OpCodes.Call, fixtureCreateGenericMethod.MakeGenericMethod(returnType));
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

                dummyType.GetField(specimenBuilderFieldName, BindingFlags.Static | BindingFlags.NonPublic)
                    .SetValue(null, this.specimenBuilder);
            }
        }
    }
}
