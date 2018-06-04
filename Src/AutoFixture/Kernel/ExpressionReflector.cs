using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace AutoFixture.Kernel
{
    internal static class ExpressionReflector
    {
        internal static MemberExpression GetWritableMember(this LambdaExpression propertyPicker)
        {
            var bodyExpr = propertyPicker.Body;

            // Method from C# may lead to an implicit conversion - e.g. if the property or field type is System.Byte,
            // but the supplied value is a System.Int32 value. Since there's an implicit conversion, the resulting
            // expression may be (x => Convert(x.Property)) and we need to unwrap it.
            var memberExpr = bodyExpr.UnwrapIfConversionExpression() as MemberExpression;
            if (memberExpr == null)
            {
                throw new ArgumentException(
                    "The expression's Body is not a MemberExpression. " +
                    "Most likely this is because it does not represent access to a property or field.",
                    nameof(propertyPicker));
            }

            if (memberExpr.Member is PropertyInfo pi && pi.GetSetMethod() == null)
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        "The property \"{0}\" is read-only.", pi.Name),
                    nameof(propertyPicker));
            }
            return memberExpr;
        }

        internal static void VerifyIsNonNestedWritableMemberExpression(LambdaExpression expression)
        {
            var memberExpression = expression.GetWritableMember();

            // It could happen that parameter is implicitly converted to an interface when e.g. a custom helper
            // extension method is used. In such cases we need to unwrap the expression to access the underlying
            // parameter expression.
            var argExpression = memberExpression.Expression.UnwrapIfConversionExpression();

            var parameterExpression = argExpression as ParameterExpression;
            if (parameterExpression == null)
            {
                throw new ArgumentException(
                    "The expression contains access to a nested property or field. " +
                    "Configuration API doesn't support this feature, therefore please rewrite the expression to avoid nested fields or properties.",
                    nameof(expression));
            }
        }

        /// <summary>
        /// If current expression is a conversion expression, unwrap it and return the underlying expression.
        /// Otherwise, do nothing.
        /// </summary>
        private static Expression UnwrapIfConversionExpression(this Expression exp)
        {
            if (exp is UnaryExpression convExpr && convExpr.NodeType == ExpressionType.Convert)
                return convExpr.Operand;

            return exp;
        }
    }
}
