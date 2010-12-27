using System;
using System.Linq.Expressions;

namespace Ploeh.AutoFixture.Idioms
{
    public static class FixtureExtensions
    {
        public static PickedProperty<T, TProperty> PickProperty<T, TProperty>(this IFixture fixture, Expression<Func<T, TProperty>> property)
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