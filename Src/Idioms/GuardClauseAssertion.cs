using System;
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
        private readonly ISpecimenBuilderComposer composer;
        private readonly IBehaviorExpectation behaviorExpectation;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuardClauseAssertion"/> class.
        /// </summary>
        /// <param name="composer">
        /// A composer which can create instances required to implement the idiomatic unit test.
        /// </param>
        /// <remarks>
        /// <para>
        /// <paramref name="composer" /> will typically be a <see cref="Fixture" /> instance.
        /// </para>
        /// </remarks>
        public GuardClauseAssertion(ISpecimenBuilderComposer composer)
            : this(composer, new CompositeBehaviorExpectation(
                new NullReferenceBehaviorExpectation(), 
                new EmptyGuidBehaviorExpectation()))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GuardClauseAssertion"/> class.
        /// </summary>
        /// <param name="composer">
        /// A composer which can create instances required to implement the idiomatic unit test.
        /// </param>
        /// <param name="behaviorExpectation">
        /// A behavior expectation to override the default expectation.
        /// </param>
        /// <remarks>
        /// <para>
        /// <paramref name="composer" /> will typically be a <see cref="Fixture" /> instance.
        /// </para>
        /// </remarks>
        public GuardClauseAssertion(ISpecimenBuilderComposer composer, IBehaviorExpectation behaviorExpectation)
        {
            this.composer = composer;
            this.behaviorExpectation = behaviorExpectation;
        }

        /// <summary>
        /// Gets the composer supplied via the constructor.
        /// </summary>
        public ISpecimenBuilderComposer Composer
        {
            get { return this.composer; }
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
        /// <seealso cref="GuardClauseAssertion(ISpecimenBuilderComposer, IBehaviorExpectation)" />
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
        public override void Verify(ConstructorInfo constructorInfo)
        {
            var method = new ConstructorMethod(constructorInfo);
            this.Verify(method);
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

            var owner = this.Composer.CreateAnonymous(methodInfo.ReflectedType);
            var method = new InstanceMethod(methodInfo, owner);
            this.Verify(method);
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

            var owner = this.Composer.CreateAnonymous(propertyInfo.ReflectedType);
            var command = new PropertySetCommand(propertyInfo, owner);
            var unwrapper = new ReflectionExceptionUnwrappingCommand(command);
            this.BehaviorExpectation.Verify(unwrapper);
        }

        private void Verify(IMethod method)
        {
            var parameters = (from pi in method.Parameters
                              select this.Composer.CreateAnonymous(pi.ParameterType)).ToList();

            var i = 0;
            foreach (var pi in method.Parameters)
            {
                var expansion = new IndexedReplacement<object>(i++, parameters);

                var command = new MethodInvokeCommand(method, expansion, pi);
                var unwrapper = new ReflectionExceptionUnwrappingCommand(command);
                this.BehaviorExpectation.Verify(unwrapper);
            }
        }
    }
}
