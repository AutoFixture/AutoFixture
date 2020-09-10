using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using AutoFixture.Kernel;

namespace AutoFixture.Idioms
{
    /// <summary>
    /// Encapsulates a unit test that verifies that a type implementing <see cref="IEqualityComparer{T}"/> implements it correctly
    /// with respect of the rule: calling GetHashCode(x) should always return same value.
    /// </summary>
    public class EqualityComparerGetHashCodeAssertion : IdiomaticAssertion
    {
        /// <summary>
        /// Gets the builder supplied by the constructor.
        /// </summary>
        public ISpecimenBuilder Builder { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EqualityComparerGetHashCodeAssertion"/> class.
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
        public EqualityComparerGetHashCodeAssertion(ISpecimenBuilder builder)
        {
            this.Builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <summary>
        /// Verifies that `calling GetHashCode(x) should always return same value`
        /// if the supplied method is an implementation of <see cref="IEqualityComparer{T}.GetHashCode(T)"/>.
        /// </summary>
        /// <param name="methodInfo">The method to verify.</param>
        public override void Verify(MethodInfo methodInfo)
        {
            if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));
            if (!IsEqualityComparerGetHashCode(methodInfo)) return;

            var comparer = this.Builder.CreateAnonymous(methodInfo.ReflectedType);
            var testSubject = this.Builder.CreateAnonymous(methodInfo.GetParameters()[0].ParameterType);

            var firstHashCode = (int)methodInfo.Invoke(comparer, new[] { testSubject });
            var secondHashCode = (int)methodInfo.Invoke(comparer, new[] { testSubject });

            if (firstHashCode != secondHashCode)
            {
                throw new EqualityComparerImplementationException(string.Format(CultureInfo.CurrentCulture,
                    "The type '{0}' implements the `IEqualityComparer<T>` interface incorrectly: " +
                    "calling GetHashCode(x) should always return same value.",
                    methodInfo.ReflectedType?.FullName));
            }
        }

        private static bool IsEqualityComparerGetHashCode(MethodInfo methodInfo)
        {
            return methodInfo is { Name: nameof(IEqualityComparer.GetHashCode), ReflectedType: { } type }
                   && methodInfo.GetParameters().Length == 1
                   && type.ImplementsGenericInterfaceDefinition(typeof(IEqualityComparer<>))
                   && type.ImplementsGenericInterface(
                       typeof(IEqualityComparer<>),
                       methodInfo.GetParameters()[0].ParameterType);
        }
    }
}