using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using System.Globalization;

namespace Ploeh.AutoFixture.Kernel
{
    internal static class ExpressionReflector
    {
        internal static MemberExpression GetWritableMember(this LambdaExpression propertyPicker)
        {
            var me = propertyPicker.Body as MemberExpression;
            if (me == null)
            {
                throw new ArgumentException("The expression's Body is not a MemberExpression. Most likely this is because it does not represent access to a property or field.", "propertyPicker");
            }

            var pi = me.Member as PropertyInfo;
            if (pi != null && pi.GetSetMethod() == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "The property \"{0}\" is read-only.", pi.Name), "propertyPicker");
            }
            return me;
        }
    }
}
