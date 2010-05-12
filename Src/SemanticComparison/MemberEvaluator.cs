using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;

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
                throw new ArgumentNullException("member");
            }
            if (evaluator == null)
            {
                throw new ArgumentNullException("evaluator");
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
