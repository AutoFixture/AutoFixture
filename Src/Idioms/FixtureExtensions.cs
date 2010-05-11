using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Ploeh.AutoFixture.Idioms
{
    public static class FixtureExtensions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Nested generics and Expression's is a very happy marriage.")]
        public static IPickedProperty PickProperty<T, TProperty>(this Fixture fixture, Expression<Func<T, TProperty>> property)
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }

            var propertyInfo = Reflect<T>.GetProperty(property);
            return new PickedProperty<T, TProperty>(fixture, propertyInfo);
        }
    }
}