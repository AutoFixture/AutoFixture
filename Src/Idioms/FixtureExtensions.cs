using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Idioms
{
    public static class FixtureExtensions
    {
        public static PropertyContext ForProperty<T, TProperty>(this ISpecimenBuilderComposer composer, Expression<Func<T, TProperty>> property)
        {
            var propertyInfo = Reflect<T>.GetProperty(property);
            return composer.ForProperty(propertyInfo);
        }

        public static PropertyContext ForProperty(this ISpecimenBuilderComposer composer, PropertyInfo propertyInfo)
        {
            if (composer == null)
            {
                throw new ArgumentNullException("composer");
            }
            if (propertyInfo == null)
            {
                throw new ArgumentNullException("propertyInfo");
            }

            return new PropertyContext(composer, propertyInfo);
        }

        public static MethodContext ForMethod<T>(this ISpecimenBuilderComposer composer, Expression<Action<T>> methodPicker)
        {
            return composer.ForMethod(Reflect<T>.GetMethod(methodPicker));
        }

        public static MethodContext ForMethod<T, TInput>(this ISpecimenBuilderComposer composer, Expression<Action<T, TInput>> methodPicker)
        {
            return composer.ForMethod(Reflect<T>.GetMethod(methodPicker));
        }

        public static MethodContext ForMethod<T, TInput1, TInput2>(this ISpecimenBuilderComposer composer, Expression<Action<T, TInput1, TInput2>> methodPicker)
        {
            return composer.ForMethod(Reflect<T>.GetMethod(methodPicker));
        }

        public static MethodContext ForMethod<T, TInput1, TInput2, TInput3>(this ISpecimenBuilderComposer composer, Expression<Action<T, TInput1, TInput2, TInput3>> methodPicker)
        {
            return composer.ForMethod(Reflect<T>.GetMethod(methodPicker));
        }

        public static MethodContext ForMethod(this ISpecimenBuilderComposer composer, MethodInfo methodInfo)
        {
            if (composer == null)
            {
                throw new ArgumentNullException("composer");
            }
            if (methodInfo == null)
            {
                throw new ArgumentNullException("methodBase");
            }

            return new MethodContext(composer, methodInfo);
        }

        public static MethodContext ForMethod(this ISpecimenBuilderComposer composer, ConstructorInfo constructorInfo)
        {
            if (composer == null)
            {
                throw new ArgumentNullException("composer");
            }
            if (constructorInfo == null)
            {
                throw new ArgumentNullException("constructorInfo");
            }

            return new MethodContext(composer, constructorInfo);
        }
    }
}