using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Ploeh.SemanticComparison
{
    internal static class MemberExpressionExtension
    {
        private static readonly MethodInfo equalsMember = typeof(object).GetMethod("Equals", BindingFlags.Public | BindingFlags.Static);

        internal static IEnumerable<MemberExpression> GetMemberExpressionHierarchy(this MemberExpression memberExpression)
        {
            var result = new List<MemberExpression>();
            do
            {
                result.Add(memberExpression);
                memberExpression = memberExpression.Expression as MemberExpression;
            }
            while (memberExpression != null);

            result.Reverse();

            return result;
        }

        internal static MemberExpression GetRootMemberExpression(this MemberExpression memberExpression)
        {
            return memberExpression.GetMemberExpressionHierarchy().First();
        }

        internal static Expression GetSafeDeepEquals(Expression source, Expression destination, MemberExpression current, IEnumerable<MemberExpression> children)
        {
            var expression = current;
            source = Expression.PropertyOrField(source, expression.Member.Name);
            destination = Expression.PropertyOrField(destination, expression.Member.Name);
            Expression childExpression;

            var childrenEnumerator = children.GetEnumerator();
            if (!childrenEnumerator.MoveNext())
            {
                childExpression = Expression.Equal(
                    Expression.TypeAs(source, typeof(object)), Expression.TypeAs(destination, typeof(object)), false, equalsMember);
            }
            else
            {
                childExpression = GetSafeDeepEquals(source, destination, childrenEnumerator.Current, new EnumerableEnumerator<MemberExpression>(childrenEnumerator));
            }

            return Expression.Condition(
                Expression.Equal(source, Expression.Constant(null)),
                Expression.Equal(destination, Expression.Constant(null)),
                Expression.Condition(Expression.Equal(destination, Expression.Constant(null)), Expression.Constant(false), childExpression));
        }

        internal static MemberEvaluator<TSource, TDestination> ToDeepEvaluator<TSource, TDestination>(this MemberExpression memberExpression)
        {
            var memberExpressionHierarchy = memberExpression.GetMemberExpressionHierarchy().ToList();
            var rootMember = memberExpressionHierarchy.First().Member;
            var properties = memberExpressionHierarchy.Select(x => x.Member as PropertyInfo);

            if (properties.Any(x => x != null && x.GetIndexParameters().Length > 0))
            {
                return new MemberEvaluator<TSource, TDestination>(rootMember, (TSource s, TDestination d) => true);
            }

            var srcParam = Expression.Parameter(typeof(TSource), "x");
            var destParam = Expression.Parameter(typeof(TDestination), "y");

            try
            {
                var eq = GetSafeDeepEquals(srcParam, destParam, memberExpressionHierarchy.First(), memberExpressionHierarchy.Skip(1));
                    
                var exp = Expression.Lambda<Func<TSource, TDestination, bool>>(eq, srcParam, destParam);
                return new MemberEvaluator<TSource, TDestination>(rootMember, exp.Compile());
            }
            catch (ArgumentException)
            {
                return new MemberEvaluator<TSource, TDestination>(rootMember, (TSource s, TDestination d) => false);
            }
            catch (AmbiguousMatchException)
            {
                return new MemberEvaluator<TSource, TDestination>(rootMember, (TSource s, TDestination d) => false);
            }
        }
    }
}
