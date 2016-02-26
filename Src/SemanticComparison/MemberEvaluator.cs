using System;
using System.Reflection;

namespace Ploeh.SemanticComparison
{
    internal class MemberEvaluator<TSource, TDestination>
    {
        private readonly MemberInfo member;
        private readonly Func<TSource, TDestination, bool> evaluator;

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

            this.member = member;
            this.evaluator = evaluator;
        }

        internal Func<TSource, TDestination, bool> Evaluator
        {
            get { return this.evaluator; }
        }

        internal MemberInfo Member
        {
            get { return this.member; }
        }
    }
}
