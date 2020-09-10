using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using AutoFixture.Kernel;

namespace AutoFixture.Idioms
{
    /// <summary>
    /// Encapsulates a unit test that verifies that a type implementing <see cref="IEqualityComparer{T}"/> implements it correctly
    /// with respect of the rule: calling Equals(x, z) should return same as Equals(x, y) and Equals(y, z).
    /// </summary>
    public class EqualityComparerEqualsTransitiveAssertion : EqualityComparerEqualsAssertion
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EqualityComparerEqualsTransitiveAssertion"/> class.
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
        public EqualityComparerEqualsTransitiveAssertion(ISpecimenBuilder builder)
            : base(builder)
        {
        }

        /// <summary>
        /// Verifies that `calling Equals(x, z) should return same as Equals(x, y) and Equals(y, z)`
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
            var thirdTestSubject = this.Builder.CreateAnonymous(argumentType);

            var firstToSecond = (bool)methodInfo.Invoke(comparer, new[] { firstTestSubject, secondTestSubject });
            var secondToThird = (bool)methodInfo.Invoke(comparer, new[] { secondTestSubject, thirdTestSubject });
            var firstToThird = (bool)methodInfo.Invoke(comparer, new[] { firstTestSubject, thirdTestSubject });

            if ((firstToSecond && secondToThird) != firstToThird)
            {
                throw new EqualityComparerImplementationException(string.Format(CultureInfo.CurrentCulture,
                    "The type '{0}' implements the `IEqualityComparer<T>` interface incorrectly: " +
                    "calling Equals(x, z) should return same as Equals(x, y) and Equals(y, z).",
                    methodInfo.ReflectedType!.FullName));
            }
        }
    }
}