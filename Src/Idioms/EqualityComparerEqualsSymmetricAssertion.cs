using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using AutoFixture.Kernel;

namespace AutoFixture.Idioms
{
    /// <summary>
    /// Encapsulates a unit test that verifies that a type implementing <see cref="IEqualityComparer{T}"/> implements it correctly
    /// with respect of the rule: calling Equals(x, y) should return same as Equals(y, x).
    /// </summary>
    public class EqualityComparerEqualsSymmetricAssertion : EqualityComparerEqualsAssertion
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EqualityComparerEqualsSymmetricAssertion"/> class.
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
        public EqualityComparerEqualsSymmetricAssertion(ISpecimenBuilder builder)
            : base(builder)
        {
        }

        /// <summary>
        /// Verifies that `calling Equals(x, y) should return same as Equals(y, x)`
        /// if the supplied method is an implementation of <see cref="IEqualityComparer{T}.Equals(T,T)"/>.
        /// </summary>
        /// <param name="methodInfo">The method to verify.</param>
        /// <param name="argumentType">The argument type of <see cref="IEqualityComparer{T}.Equals(T,T)"/>.</param>
        protected override void VerifyEquals(MethodInfo methodInfo, Type argumentType)
        {
            if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));
            if (argumentType == null) throw new ArgumentNullException(nameof(argumentType));

            var comparer = this.Builder.CreateAnonymous(methodInfo.ReflectedType);
            var firstTestSubject = this.Builder.CreateAnonymous(argumentType);
            var secondTestSubject = this.Builder.CreateAnonymous(argumentType);

            var directResult = (bool)methodInfo.Invoke(comparer, new[] { firstTestSubject, secondTestSubject });
            var invertedResult = (bool)methodInfo.Invoke(comparer, new[] { secondTestSubject, firstTestSubject });

            if (directResult != invertedResult)
            {
                throw new EqualityComparerImplementationException(string.Format(CultureInfo.CurrentCulture,
                    "The type '{0}' implements the `IEqualityComparer<T>` interface incorrectly: " +
                    "calling Equals(x, y) should return same as Equals(y, x).",
                    methodInfo.ReflectedType!.FullName));
            }
        }
    }
}