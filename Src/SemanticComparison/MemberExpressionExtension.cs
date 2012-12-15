using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Ploeh.SemanticComparison
{
    internal static class MemberExpressionExtension
    {
        private readonly static MethodInfo equalsMember = typeof(object).GetMethod("Equals", BindingFlags.Public | BindingFlags.Static);

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
                Expression source = srcParam;
                Expression destination = destParam;

                foreach (var expression in memberExpressionHierarchy)
                {
                    source = Expression.PropertyOrField(source, expression.Member.Name);
                    destination = Expression.PropertyOrField(destination, expression.Member.Name);
                }

                var eq = Expression.Equal(
                    Expression.TypeAs(source, typeof(object)),
                    Expression.TypeAs(destination, typeof(object)),
                    false,
                    MemberExpressionExtension.equalsMember);

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
    }
}