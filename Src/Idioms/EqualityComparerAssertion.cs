using System.Collections.Generic;
using AutoFixture.Kernel;

namespace AutoFixture.Idioms
{
    /// <summary>
    /// Encapsulates a unit test that verifies that a type implementing <see cref="IEqualityComparer{T}"/> implements it correctly.
    /// </summary>
    public class EqualityComparerAssertion : CompositeIdiomaticAssertion
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EqualityComparerAssertion"/> class.
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
        public EqualityComparerAssertion(ISpecimenBuilder builder)
            : base(CreateChildAssertions(builder))
        {
        }

        private static IEnumerable<IIdiomaticAssertion> CreateChildAssertions(ISpecimenBuilder builder)
        {
            yield return new EqualityComparerEqualsSelfAssertion(builder);
            yield return new EqualityComparerEqualsSymmetricAssertion(builder);
            yield return new EqualityComparerEqualsTransitiveAssertion(builder);
            yield return new EqualityComparerGetHashCodeAssertion(builder);
            yield return new EqualityComparerEqualsNullAssertion(builder);
        }
    }
}