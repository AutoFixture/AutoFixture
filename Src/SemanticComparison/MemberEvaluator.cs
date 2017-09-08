using System;
using System.Reflection;

namespace Ploeh.SemanticComparison
{
    internal class MemberEvaluator<TSource, TDestination>
    {
        public MemberEvaluator(MemberInfo member, Func<TSource, TDestination, bool> evaluator)
        {
            if (member == null)
            {
                throw new ArgumentNullException(nameof(member));
            }
            if (evaluator == null)
            {
                throw new ArgumentNullException(nameof(evaluator));
            }

            this.Member = member;
            this.Evaluator = evaluator;
        }

        internal Func<TSource, TDestination, bool> Evaluator { get; }

        internal MemberInfo Member { get; }
    }
}
