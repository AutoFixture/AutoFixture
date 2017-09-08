using System;
using System.Linq.Expressions;
using Ploeh.AutoFixture.Dsl;

namespace Ploeh.AutoFixtureDocumentationTest.Extension.Constraints
{
    public static class ObjectBuilderExtension
    {
        public static IPostprocessComposer<T> With<T>(
            this ICustomizationComposer<T> ob,
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
