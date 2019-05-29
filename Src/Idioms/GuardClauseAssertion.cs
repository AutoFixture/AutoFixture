using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoFixture.Kernel;

namespace AutoFixture.Idioms
{
    /// <summary>
    /// Encapsulates a unit test that verifies that a method or constructor has appropriate Guard
    /// Clauses in place.
    /// </summary>
    public class GuardClauseAssertion : IdiomaticAssertion
    {
        private readonly OpenGenericTypeClosingUtil openGenericTypeClosingUtil;

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
            this.Builder = builder;
            this.BehaviorExpectation = behaviorExpectation;

            this.openGenericTypeClosingUtil = new OpenGenericTypeClosingUtil(builder);
        }

        /// <summary>
        /// Gets the builder supplied via the constructor.
        /// </summary>
        public ISpecimenBuilder Builder { get; }

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
        public IBehaviorExpectation BehaviorExpectation { get; }

        /// <summary>
        /// Configuration flag allowing to support lazily thrown exception for methods with `async` modifier.
        /// While the fast-fail principle is violated, the `async` methods are usually awaited so it might be acceptable.
        /// </summary>
        public bool AllowAsyncMethods { get; set; }

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
                throw new ArgumentNullException(nameof(constructorInfo));

            constructorInfo = this.openGenericTypeClosingUtil.CloseGenericType(constructorInfo);

            var method = new ConstructorMethod(constructorInfo);
            this.DoVerify(method, isReturnValueDeferrable: false, isReturnValueTask: false, isAsyncMethod: false);
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
                throw new ArgumentNullException(nameof(methodInfo));

            if (methodInfo.IsEqualsMethod() ||
                methodInfo.IsGetHashCodeMethod() ||
                methodInfo.IsToString() ||
                methodInfo.IsGetType() ||
                methodInfo.IsAbstract)
                return;

            methodInfo = this.openGenericTypeClosingUtil.CloseGenericType(methodInfo);
            methodInfo = this.openGenericTypeClosingUtil.CloseGenericMethod(methodInfo);

            var method = this.CreateMethod(methodInfo);
            var returnType = methodInfo.ReturnType;

            // According to MSDN method with yield could have only 4 possible types:
            // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/yield
            var isReturnTypePossibleDeferrable = returnType == typeof(IEnumerable)
                   || returnType == typeof(IEnumerator)
                   || (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                   || (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(IEnumerator<>));

            var containsByRefArgs = methodInfo.GetParameters().Select(p => p.ParameterType).Any(t => t.IsByRef);

            var isReturnValueDeferrable = isReturnTypePossibleDeferrable && !containsByRefArgs;

            var isReturnValueTask = typeof(Task).IsAssignableFrom(returnType);

            // According to official docs it's a reliable way to detect async methods
            // https://docs.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.asyncstatemachineattribute
            var isAsyncMethod = methodInfo.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) != null;

            this.DoVerify(method, isReturnValueDeferrable, isReturnValueTask, isAsyncMethod);
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
                throw new ArgumentNullException(nameof(propertyInfo));

            if (propertyInfo.GetSetMethod() == null)
                return;

            propertyInfo = this.openGenericTypeClosingUtil.CloseGenericType(propertyInfo);

            var owner = this.CreateOwner(propertyInfo);
            var command = new PropertySetCommand(propertyInfo, owner);
            var unwrapper = new ReflectionExceptionUnwrappingCommand(command);
            this.BehaviorExpectation.Verify(unwrapper);
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

        private void DoVerify(IMethod method, bool isReturnValueDeferrable, bool isReturnValueTask, bool isAsyncMethod)
        {
            if (isReturnValueDeferrable)
            {
                this.VerifyDeferrableIterator(method);
                return;
            }

            if (this.AllowAsyncMethods && isAsyncMethod)
            {
                this.VerifyAsyncMethod(method);
                return;
            }

            if (isReturnValueTask)
            {
                this.VerifyDeferrableTask(method);
                return;
            }

            this.VerifyNormal(method);
        }

        private void VerifyDeferrableIterator(IMethod method)
        {
            foreach (var command in this.GetParameterGuardCommands(method))
            {
                this.BehaviorExpectation.Verify(new IteratorMethodInvokeCommand(command));
            }
        }

        private void VerifyDeferrableTask(IMethod method)
        {
            foreach (var command in this.GetParameterGuardCommands(method))
            {
                this.BehaviorExpectation.Verify(new TaskReturnMethodInvokeCommand(command));
            }
        }

        private void VerifyAsyncMethod(IMethod method)
        {
            method = new AsyncAwaitingMethod(method);
            foreach (var command in this.GetParameterGuardCommands(method))
            {
                this.BehaviorExpectation.Verify(command);
            }
        }

        private void VerifyNormal(IMethod method)
        {
            foreach (var command in this.GetParameterGuardCommands(method))
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

        private class TaskReturnMethodInvokeCommand : IGuardClauseCommand
        {
            private const string Message = @"A Guard Clause test was performed on a method that returns a Task, Task<T> (possibly in an 'async' method), but the test failed. See the inner exception for more details. However, because of the async nature of the task, this test failure may look like a false positive. Perhaps you already have a Guard Clause in place, but inside the Task or inside a method marked with the 'async' keyword (if you're using C#); if this is the case, the Guard Clause is dormant, and will first be triggered when a client accesses the Result of the Task. This doesn't adhere to the Fail Fast principle, so should be addressed.
See https://github.com/AutoFixture/AutoFixture/issues/268 for more details.";

            private readonly IGuardClauseCommand command;

            public TaskReturnMethodInvokeCommand(IGuardClauseCommand command)
            {
                this.command = command;
            }

            public Type RequestedType => this.command.RequestedType;

            public string RequestedParameterName => this.command.RequestedParameterName;

            public void Execute(object value) => this.command.Execute(value);

            public Exception CreateException(string value)
            {
                var e = this.command.CreateException(value);
                return new GuardClauseException(Message, e);
            }

            public Exception CreateException(string value, Exception innerException)
            {
                var e = this.command.CreateException(value, innerException);
                return new GuardClauseException(Message, e);
            }

            public Exception CreateException(string value, string customError, Exception innerException)
            {
                var e = this.command.CreateException(value, customError, innerException);
                return new GuardClauseException(Message, e);
            }
        }

        private class IteratorMethodInvokeCommand : IGuardClauseCommand
        {
            private const string Message = @"A Guard Clause test was performed on a method that may contain a deferred iterator block, but the test failed. See the inner exception for more details. However, because of the deferred nature of the iterator block, this test failure may look like a false positive. Perhaps you already have a Guard Clause in place, but in conjunction with the 'yield' keyword (if you're using C#); if this is the case, the Guard Clause is dormant, and will first be triggered when a client starts looping over the iterator. This doesn't adhere to the Fail Fast principle, so should be addressed.
See e.g. http://codeblog.jonskeet.uk/2008/03/02/c-4-idea-iterator-blocks-and-parameter-checking/ for more details.";

            private readonly IGuardClauseCommand command;

            public IteratorMethodInvokeCommand(IGuardClauseCommand command)
            {
                this.command = command;
            }

            public Type RequestedType => this.command.RequestedType;

            public string RequestedParameterName => this.command.RequestedParameterName;

            public void Execute(object value) => this.command.Execute(value);

            public Exception CreateException(string value)
            {
                var e = this.command.CreateException(value);
                return new GuardClauseException(Message, e);
            }

            public Exception CreateException(string value, Exception innerException)
            {
                var e = this.command.CreateException(value, innerException);
                return new GuardClauseException(Message, e);
            }

            public Exception CreateException(string value, string customError, Exception innerException)
            {
                var e = this.command.CreateException(value, customError, innerException);
                return new GuardClauseException(Message, e);
            }
        }

        private class AsyncAwaitingMethod : IMethod
        {
            private readonly IMethod method;

            public AsyncAwaitingMethod(IMethod method)
            {
                this.method = method;
            }

            public IEnumerable<ParameterInfo> Parameters => this.method.Parameters;

            public object Invoke(IEnumerable<object> parameters)
            {
                var result = this.method.Invoke(parameters);

                if (result == null)
                {
                    throw new InvalidOperationException("Async method is never supposed to return null.");
                }

                // Force underlying state machine to execute.
                // Optimize task as it's most often return result.
                if (result is Task taskResult)
                {
                    taskResult.GetAwaiter().GetResult();
                }
                else
                {
                    // According to the contract, async method should return "something" which has `GetAwaiter` method.
                    // In its turn, that result should return something which has "GetResult" method.
                    // This case is hit e.g. for ValueTask
                    try
                    {
                        object awaiter = result.GetType().InvokeMember(nameof(Task.GetAwaiter),
                            BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, Type.DefaultBinder,
                            result, new object[0], CultureInfo.InvariantCulture);
                        awaiter.GetType().InvokeMember(nameof(TaskAwaiter.GetResult),
                            BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, Type.DefaultBinder,
                            awaiter, new object[0], CultureInfo.InvariantCulture);
                    }
                    catch (TargetInvocationException ex)
                    {
                        throw ex.InnerException;
                    }
                }

                return result;
            }
        }
    }
}
