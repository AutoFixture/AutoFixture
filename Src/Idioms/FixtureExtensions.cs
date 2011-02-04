using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Ploeh.AutoFixture.Idioms
{
    public static class FixtureExtensions
    {
        public static PropertyContext ForProperty<T, TProperty>(this IFixture fixture, Expression<Func<T, TProperty>> property)
        {
            var propertyInfo = Reflect<T>.GetProperty(property);
            return fixture.ForProperty(propertyInfo);
        }

        public static PropertyContext ForProperty(this IFixture fixture, PropertyInfo propertyInfo)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }
            if (propertyInfo == null)
            {
                throw new ArgumentNullException("propertyInfo");
            }

            return new PropertyContext(fixture, propertyInfo);
        }

        public static MethodContext ForMethod<T>(this IFixture fixture, Expression<Action<T>> methodPicker)
        {
            return fixture.ForMethod(Reflect<T>.GetMethod(methodPicker));
        }

        public static MethodContext ForMethod<T, TInput>(this IFixture fixture, Expression<Action<T, TInput>> methodPicker)
        {
            return fixture.ForMethod(Reflect<T>.GetMethod(methodPicker));
        }

        public static MethodContext ForMethod<T, TInput1, TInput2>(this IFixture fixture, Expression<Action<T, TInput1, TInput2>> methodPicker)
        {
            return fixture.ForMethod(Reflect<T>.GetMethod(methodPicker));
        }

        public static MethodContext ForMethod<T, TInput1, TInput2, TInput3>(this IFixture fixture, Expression<Action<T, TInput1, TInput2, TInput3>> methodPicker)
        {
            return fixture.ForMethod(Reflect<T>.GetMethod(methodPicker));
        }

        public static MethodContext ForMethod(this IFixture fixture, MethodBase methodBase)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }
            if (methodBase == null)
            {
                throw new ArgumentNullException("methodBase");
            }

            return new MethodContext(fixture, methodBase);
        }
    }
}