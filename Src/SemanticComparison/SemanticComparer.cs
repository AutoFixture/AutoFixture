using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ploeh.SemanticComparison
{
    public class SemanticComparer<TSource, TDestination> : IEqualityComparer, IEqualityComparer<object>
    {
        private readonly IEnumerable<MemberEvaluator<TSource, TDestination>> evaluators;
        private readonly Func<IEnumerable<MemberInfo>> defaultMembersGenerator;

        public SemanticComparer()
            :this(Enumerable.Empty<MemberEvaluator<TSource, TDestination>>(), DefaultMembers.Generate<TDestination>)
        {
        }

        internal SemanticComparer(IEnumerable<MemberEvaluator<TSource, TDestination>> evaluators, Func<IEnumerable<MemberInfo>> defaultMembersGenerator)
        {
            this.evaluators = evaluators;
            this.defaultMembersGenerator = defaultMembersGenerator;
        }

        internal IEnumerable<MemberEvaluator<TSource, TDestination>> Evaluators
        {
            get { return evaluators; }
        }

        internal Func<IEnumerable<MemberInfo>> DefaultMembersGenerator
        {
            get { return defaultMembersGenerator; }
        }

        public new bool Equals(object x, object y)
        {
            if ((x == null) && (y == null))
            {
                return true;
            }

            if (x == null)
            {
                return false;
            }

            if (y == null)
            {
                return false;
            }

            if (x.Equals(y))
            {
                return true;
            }

            // Check whether each instance is compatible to the given type.
            // NOTE: The type parameters 'TSource' and 'TDestination' cannot be used with the 'as'
            // operator because they do not have a class type constraint nor a 'class' constraint.
            if (x is TSource && y is TDestination)
            {
                return this.GetEvaluators().All(me => me.Evaluator((TSource)x, (TDestination)y));
            }
            
            return false;
        }

        public int GetHashCode(object obj)
        {
            return obj == null ? 0 : obj.GetHashCode();
        }

        bool IEqualityComparer.Equals(object x, object y)
        {
            return this.Equals(x, y);
        }

        int IEqualityComparer.GetHashCode(object obj)
        {
            return this.GetHashCode(obj);
        }

        internal IEnumerable<MemberEvaluator<TSource, TDestination>> GetEvaluators()
        {
            var defaultMembers = this.DefaultMembersGenerator();
            var undefinedMembers = defaultMembers.Except(this.Evaluators.Select(e => e.Member), new MemberInfoNameComparer());
            var undefinedEvaluators = from mi in undefinedMembers
                                      select mi.ToEvaluator<TSource, TDestination>();
            return this.Evaluators.Concat(undefinedEvaluators);
        }

        internal static class DefaultMembers
        {
            internal static IEnumerable<MemberInfo> Generate<T>()
            {
                return typeof(T)
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Concat(typeof(T)
                        .GetFields(BindingFlags.Public | BindingFlags.Instance)
                        .Cast<MemberInfo>());
            }
        }
    }
}