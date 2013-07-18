using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
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
        /// Initializes a new instance of the <see cref="GuardClauseAssertion"/> class.
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
        /// Initializes a new instance of the <see cref="GuardClauseAssertion"/> class.
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
        /// <see cref="IBehaviorExpectation"/>, but a custom behavior can also be supplied via one
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
        /// Verifies that a constructor has appripriate Guard Clauses in place.
        /// </summary>
        /// <param name="constructorInfo">The constructor.</param>
        /// <remarks>
        /// <para>
        /// Exactly which Guard Clauses are verified is defined by
        /// <see cref="BehaviorExpectation" />.
        /// </para>
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "AutoFixture", Justification = "Workaround for a bug in CA: https://connect.microsoft.com/VisualStudio/feedback/details/521030/")]
        public override void Verify(ConstructorInfo constructorInfo)
        {
            if (constructorInfo == null)
                throw new ArgumentNullException("constructorInfo");

            EnsureTypeIsNotGeneric(constructorInfo.ReflectedType);

            var method = new ConstructorMethod(constructorInfo);
            this.Verify(method, false);
        }

        /// <summary>
        /// Verifies that a method has appripriate Guard Clauses in place.
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

            if (methodInfo.IsEqualsMethod())
                return;

            EnsureTypeIsNotGeneric(methodInfo.ReflectedType);

            var owner = CreateOwner(methodInfo.ReflectedType);
            var method = new InstanceMethod(methodInfo, owner);

            var isReturnValueIterator =
                typeof(System.Collections.IEnumerable).IsAssignableFrom(methodInfo.ReturnType) ||
                typeof(System.Collections.IEnumerator).IsAssignableFrom(methodInfo.ReturnType);

            this.Verify(method, isReturnValueIterator);
        }

        /// <summary>
        /// Verifies that a property has appripriate Guard Clauses in place.
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

            EnsureTypeIsNotGeneric(propertyInfo.ReflectedType);

            var owner = CreateOwner(propertyInfo.ReflectedType);
            var command = new PropertySetCommand(propertyInfo, owner);
            var unwrapper = new ReflectionExceptionUnwrappingCommand(command);
            this.BehaviorExpectation.Verify(unwrapper);
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
                        @"AutoFixture was unable to create an instance of type {0}. "
                        + @"Please check the inner exception for more details",
                        type),
                    e);
            }
        }

        private void Verify(IMethod method, bool isReturnValueIterator)
        {
            var parameters = GetParameters(method);

            var i = 0;
            foreach (var pi in method.Parameters)
            {
                var expansion = new IndexedReplacement<object>(i++, parameters);

                var command = new MethodInvokeCommand(method, expansion, pi);
                var unwrapper = new ReflectionExceptionUnwrappingCommand(command);
                if (isReturnValueIterator)
                {
                    var iteratorDecorator = new IteratorMethodInvokeCommand(unwrapper);
                    this.behaviorExpectation.Verify(iteratorDecorator);
                }
                else
                    this.BehaviorExpectation.Verify(unwrapper);
            }
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
                            @"AutoFixture was unable to create an instance for parameter ""{1}"" of method ""{2}""."
                            + @"{0}Method Signature: {3}{0}Declaring Type: {4}{0}Reflected Type: {5}",
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

        private static void EnsureTypeIsNotGeneric(Type type)
        {
            if (type.IsGenericTypeDefinition)
            {
                throw new GuardClauseException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        "AutoFixture was unable to create an instance of {0}, because it's a generic type definition.",
                        type.Name));
            }
        }

        private static Type GetParameterType(ParameterInfo pi)
        {
            var pType = pi.ParameterType;
            return pType.IsByRef ? pType.GetElementType() : pi.ParameterType;
        }

        private class IteratorMethodInvokeCommand : IGuardClauseCommand
        {
            private const string message = @"A Guard Clause test was performed on a method that may contain a deferred iterator block, but the test failed. See the inner exception for more details. However, because of the deferred nature of the iterator block, this test failure may look like a false positive. Perhaps you already have a Guard Clause in place, but in conjunction with the 'yield' keyword (if you're using C#); if this is the case, the Guard Clause is dormant, and will first be triggered when a client starts looping over the iterator. This doesn't adhere to the Fail Fast principle, so should be addressed.
See e.g. http://msmvps.com/blogs/jon_skeet/archive/2008/03/02/c-4-idea-iterator-blocks-and-parameter-checking.aspx for more details.";

            private readonly IGuardClauseCommand command;

            public IteratorMethodInvokeCommand(IGuardClauseCommand command)
            {
                this.command = command;
            }

            public Type RequestedType
            {
                get { return this.command.RequestedType; }
            }

            public void Execute(object value)
            {
                this.command.Execute(value);
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "jonskeet", Justification = "False Positive. Code Analysis really shouldn't attempt to spell check URLs.")]
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "msmvps", Justification = "False Positive. Code Analysis really shouldn't attempt to spell check URLs.")]
            public Exception CreateException(string value)
            {
                var e = this.command.CreateException(value);
                return new GuardClauseException(message, e);
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "jonskeet", Justification = "False Positive. Code Analysis really shouldn't attempt to spell check URLs.")]
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "msmvps", Justification = "False Positive. Code Analysis really shouldn't attempt to spell check URLs.")]
            public Exception CreateException(string value, Exception innerException)
            {
                var e = this.command.CreateException(value, innerException);
                return new GuardClauseException(message, e);
            }
        }

    }
}
