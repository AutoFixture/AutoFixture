using System;
using System.Reflection;

namespace Ploeh.SemanticComparison
{
    /// <summary>
    /// Encapsulates information about a member (property or field) which is used in a
    /// <see cref="Likeness{TSource, TDestination}"/> comparison.
    /// </summary>
    /// <typeparam name="TSource">The type of the source value.</typeparam>
    /// <typeparam name="TDestination">
    /// The type of the destination that is evaluated against the source value.
    /// </typeparam>
    public class LikenessMember<TSource, TDestination>
    {
        private readonly Likeness<TSource, TDestination> likeness;
        private readonly MemberInfo member;

        internal LikenessMember(Likeness<TSource, TDestination> likeness, MemberInfo memberInfo)
        {
            if (likeness == null)
            {
                throw new ArgumentNullException("likeness");
            }
            if (memberInfo == null)
            {
                throw new ArgumentNullException("memberInfo");
            }

            this.likeness = likeness;
            this.member = memberInfo;
        }

        /// <summary>
        /// Returns a new <see cref="Likeness{TSource, TDestination}"/> that includes the specified
        /// evaluator.
        /// </summary>
        /// <param name="evaluator">
        /// An expression that evaluates the source value against the destination value for the
        /// property or field encapsulated by the instance.</param>
        /// <returns>
        /// A new <see cref="Likeness{TSource, TDestination}"/> that includes
        /// <paramref name="evaluator"/>.
        /// </returns>
        public Likeness<TSource, TDestination> EqualsWhen(Func<TSource, TDestination, bool> evaluator)
        {
            var memberEvaluator = new MemberEvaluator<TSource, TDestination>(this.member, evaluator);
            return this.likeness.AddEvaluator(memberEvaluator);
        }
    }
}
