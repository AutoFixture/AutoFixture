using System;
using System.Linq;
using System.Linq.Expressions;

namespace Ploeh.AutoFixture.Idioms
{
    public static class FixtureExtensions
    {
        public static PropertyContext ForProperty<T, TProperty>(this IFixture fixture, Expression<Func<T, TProperty>> property)
        {
            var propertyInfo = Reflect<T>.GetProperty(property);
            return new PropertyContext(fixture, propertyInfo);
        }

        public static MethodContext ForMethod<T>(this IFixture fixture, Expression<Action<T>> methodPicker)
        {
            return new MethodContext(fixture, Reflect<T>.GetMethod(methodPicker));
        }

        public static MethodContext ForMethod<T, TInput>(this IFixture fixture, Expression<Action<T, TInput>> methodPicker)
        {
            return new MethodContext(fixture, Reflect<T>.GetMethod(methodPicker));
        }

        public static MethodContext ForMethod<T, TInput1, TInput2>(this IFixture fixture, Expression<Action<T, TInput1, TInput2>> methodPicker)
        {
            return new MethodContext(fixture, Reflect<T>.GetMethod(methodPicker));
        }

        public static MethodContext ForMethod<T, TInput1, TInput2, TInput3>(this IFixture fixture, Expression<Action<T, TInput1, TInput2, TInput3>> methodPicker)
        {
            return new MethodContext(fixture, Reflect<T>.GetMethod(methodPicker));
        }
    }
}