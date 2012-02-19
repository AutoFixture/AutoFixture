using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ploeh.SemanticComparison
{
    /// <summary>
    /// Provides a class with implementations of the System.Collections.Generic.IEqualityComparer{T}
    /// and System.Collections.Generic.IEqualityComparer interfaces for convention-based object 
    /// equality comparison for use when comparing two semantically equivalent, but different, objects.
    /// </summary>
    /// <typeparam name="TSource">The type of the source value (against which the destination value 
    /// will be compared for equality).</typeparam>
    /// <typeparam name="TDestination">The type of the destination value which will be compared for 
    /// equality against the source value.</typeparam>
    public class SemanticComparer<TSource, TDestination> : IEqualityComparer, IEqualityComparer<object>
    {
        private readonly IEnumerable<MemberEvaluator<TSource, TDestination>> evaluators;
        private readonly Func<IEnumerable<MemberInfo>> defaultMembersGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="SemanticComparer&lt;TSource, TDestination&gt;"/> class.
        /// </summary>
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

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="x">The source value (against which the destination value will be compared for
        /// equality).</param>
        /// <param name="y">The destination value which will be compared for equality against the 
        /// source value.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; 
        ///   otherwise, <c>false</c>.
        /// </returns>
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

            if (x is TSource && y is TDestination)
            {
                return this.GetEvaluators().All(me => me.Evaluator((TSource)x, (TDestination)y));
            }
            
            if (x is TDestination && y is TSource)
            {
                return this.GetEvaluators().All(me => me.Evaluator((TSource)y, (TDestination)x));
            }

            return false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures 
        /// like a hash table. 
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// The type of <paramref name="obj"/> is a reference type and <paramref name="obj"/> is null.
        ///   </exception>
        public int GetHashCode(object obj)
        {
            return obj == null ? 0 : obj.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="x">The source value (against which the destination value will be compared for
        /// equality).</param>
        /// <param name="y">The destination value which will be compared for equality against the 
        /// source value.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; 
        ///   otherwise, <c>false</c>.
        /// </returns>
        bool IEqualityComparer.Equals(object x, object y)
        {
            return this.Equals(x, y);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures 
        /// like a hash table. 
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// The type of <paramref name="obj"/> is a reference type and <paramref name="obj"/> is null.
        ///   </exception>
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