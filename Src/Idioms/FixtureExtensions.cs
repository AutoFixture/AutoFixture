using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Ploeh.AutoFixture.Idioms
{
    public static class FixtureExtensions
    {
        public static IPickedProperty PickProperty<T, TProperty>(this Fixture f, Expression<Func<T, TProperty>> property)
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }

            var propertyInfo = Reflect<T>.GetProperty(property);
            return new PickedProperty<T, TProperty>(f, propertyInfo);
        }
    }
}