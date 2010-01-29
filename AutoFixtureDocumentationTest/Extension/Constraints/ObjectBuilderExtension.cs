using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture;
using System.Linq.Expressions;

namespace Ploeh.AutoFixtureDocumentationTest.Extension.Constraints
{
    public static class ObjectBuilderExtension
    {
        public static ObjectBuilder<T> With<T>(
            this ObjectBuilder<T> ob,
            Expression<Func<T, string>> propertyPicker,
            int minimumLength,
            int maximumLength)
        {
            var me = (MemberExpression)propertyPicker.Body;
            var name = me.Member.Name;
            var generator =
                new ConstrainedStringGenerator(
                    minimumLength, maximumLength);
            var value = generator.CreateaAnonymous(name);
            return ob.With(propertyPicker, value);
        }
    }
}
