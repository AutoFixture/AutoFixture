using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;

namespace Ploeh.SemanticComparison
{
    internal static class MemberInfoExtension
    {
        private readonly static MethodInfo equalsMember = typeof(object).GetMethod("Equals", BindingFlags.Public | BindingFlags.Static);

        internal static Func<TSource, TDestination, bool> ToEvaluator<TSource, TDestination>(this MemberInfo member)
        {
            var property = member as PropertyInfo;
            if ((property != null) && (property.GetIndexParameters().Length > 0))
            {
                return (TSource s, TDestination d) => true;
            }

            var srcParam = Expression.Parameter(typeof(TSource), "x");
            var destParam = Expression.Parameter(typeof(TDestination), "y");

            try
            {
                var srcMember = Expression.PropertyOrField(srcParam, member.Name);
                var destMember = Expression.PropertyOrField(destParam, member.Name);

                var eq = Expression.Equal(
                    Expression.TypeAs(srcMember, typeof(object)),
                    Expression.TypeAs(destMember, typeof(object)),
                    false,
                    MemberInfoExtension.equalsMember);

                var exp = Expression.Lambda<Func<TSource, TDestination, bool>>(eq, srcParam, destParam);
                return exp.Compile();
            }
            catch (ArgumentException)
            {
                return (TSource s, TDestination d) => false;
            }
            catch (AmbiguousMatchException)
            {
                return (TSource s, TDestination d) => false;
            }
        }
    }
}
