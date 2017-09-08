using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    internal static class ExpressionReflector
    {
        internal static MemberExpression GetWritableMember(this LambdaExpression propertyPicker)
        {
            var me = propertyPicker.Body as MemberExpression;
            if (me == null)
                me = propertyPicker.TryUnwrapConversion();
            if (me == null)
                throw new ArgumentException("The expression's Body is not a MemberExpression. Most likely this is because it does not represent access to a property or field.", nameof(propertyPicker));

            var pi = me.Member as PropertyInfo;
            if (pi != null && pi.GetSetMethod() == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "The property \"{0}\" is read-only.", pi.Name), nameof(propertyPicker));
            }
            return me;
        }

        /// <summary>
        /// Attempt to unwrap a member access expression from within a
        /// conversion expression.
        /// </summary>
        /// <param name="exp">
        /// The expression that may be a conversion expression.
        /// </param>
        /// <returns>
        /// The wrapped member access expression, if one was found; otherwise
        /// <see langword="null" />.
        /// </returns>
        /// <remarks>
        /// <para>
        /// A seemingly innocuous-looking use of the
        /// <see cref="Dsl.IPostprocessComposer{T}.With{TProperty}(Expression{Func{T, TProperty}}, TProperty)" />
        /// method from C# may lead to an implicit conversion - e.g. if the
        /// property or field type is System.Byte, but the supplied value is a
        /// System.Int32 value. Since there's an implicit conversion, the
        /// resulting expression may be (x =&gt; Convert(x.Property)). The
        /// Convert expression is a UnaryExpression, which may contain the
        /// member access expression ('x.Property').
        /// </para>
        /// </remarks>
        private static MemberExpression TryUnwrapConversion(
            this LambdaExpression exp)
        {
            var ue = exp.Body as UnaryExpression;
            if (ue != null)
                return ue.Operand as MemberExpression;
            else
                return null;
        }
    }
}
