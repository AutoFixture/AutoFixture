using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Ploeh.SemanticComparison
{
    internal static class MemberInfoExtension
    {
        private readonly static MethodInfo equalsMember = typeof(object).GetMethod("Equals", BindingFlags.Public | BindingFlags.Static);

        internal static MemberEvaluator<TSource, TDestination> ToEvaluator<TSource, TDestination>(this MemberInfo member)
        {
            var property = member as PropertyInfo;
            if ((property != null) && (property.GetIndexParameters().Length > 0))
            {
                return new MemberEvaluator<TSource, TDestination>(member, (TSource s, TDestination d) => true);
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
                return new MemberEvaluator<TSource, TDestination>(member, exp.Compile());
            }
            catch (ArgumentException)
            {
                return new MemberEvaluator<TSource, TDestination>(member, (TSource s, TDestination d) => false);
            }
            catch (AmbiguousMatchException)
            {
                return new MemberEvaluator<TSource, TDestination>(member, (TSource s, TDestination d) => false);
            }
        }

        internal static Type ToUnderlyingType(this MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)member).FieldType;
                case MemberTypes.Property:
                    return ((PropertyInfo)member).PropertyType;
                default:
                    throw new ArgumentException("MemberInfo must either FieldInfo or PropertyInfo.", "member");
            }
        }
    }
}
