using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using AutoFixture.Kernel;

namespace AutoFixture.Idioms
{
    /// <summary>
    /// A base class for building classes that encapsulate a unit test that verifies that a type implementing <see cref="IEqualityComparer{T}"/> implements it correctly.
    /// </summary>
    public abstract class EqualityComparerEqualsAssertion : IdiomaticAssertion
    {
        /// <summary>
        /// Gets the builder supplied by the constructor.
        /// </summary>
        public ISpecimenBuilder Builder { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EqualityComparerEqualsAssertion"/> class.
        /// </summary>
        /// <param name="builder">
        /// A composer which can create instances required to implement the idiomatic unit test,
        /// such as the owner of the property, as well as the value to be assigned and read from
        /// the member.
        /// </param>
        /// <remarks>
        /// <para>
        /// <paramref name="builder" /> will typically be a <see cref="Fixture" /> instance.
        /// </para>
        /// </remarks>
        protected EqualityComparerEqualsAssertion(ISpecimenBuilder builder)
        {
            this.Builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <summary>
        /// Forwards the call to <see cref="VerifyEquals"/> if the supplied method is an implementation of <see cref="IEqualityComparer{T}.Equals(T,T)"/>.
        /// </summary>
        /// <param name="methodInfo">The method to verify.</param>
        public override void Verify(MethodInfo methodInfo)
        {
            if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));
            if (!IsEqualityComparerEqualsMethod(methodInfo)) return;

            this.VerifyEquals(methodInfo, methodInfo.GetParameters()[0].ParameterType);
        }

        /// <summary>
        /// Abstract method used to verify if the method implements correctly <see cref="IEqualityComparer{T}.Equals(T,T)"/>.
        /// </summary>
        /// <param name="methodInfo">The method to verify.</param>
        /// <param name="argumentType">The argument type of <see cref="IEqualityComparer{T}.Equals(T,T)"/>.</param>
        protected abstract void VerifyEquals(MethodInfo methodInfo, Type argumentType);

        private static bool IsEqualityComparerEqualsMethod(MethodInfo methodInfo)
        {
            return methodInfo is { Name: nameof(IEqualityComparer.Equals), ReflectedType: { } type }
                   && methodInfo.GetParameters().Length == 2
                   && type.ImplementsGenericInterfaceDefinition(typeof(IEqualityComparer<>))
                   && type.ImplementsGenericInterface(
                       typeof(IEqualityComparer<>),
                       methodInfo.GetParameters()[0].ParameterType);
        }
    }
}