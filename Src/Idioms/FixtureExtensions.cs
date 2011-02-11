using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Idioms
{
    public static class FixtureExtensions
    {
        public static IPropertyContext ForProperty<T, TProperty>(this ISpecimenBuilderComposer composer, Expression<Func<T, TProperty>> property)
        {
            var propertyInfo = Reflect<T>.GetProperty(property);
            return composer.ForProperty(propertyInfo);
        }

        public static IPropertyContext ForProperty(this ISpecimenBuilderComposer composer, PropertyInfo propertyInfo)
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

        public static IMethodContext ForMethod<T>(this ISpecimenBuilderComposer composer, Expression<Action<T>> methodPicker)
        {
            return composer.ForMethod(Reflect<T>.GetMethod(methodPicker));
        }

        public static IMethodContext ForMethod<T, TInput>(this ISpecimenBuilderComposer composer, Expression<Action<T, TInput>> methodPicker)
        {
            return composer.ForMethod(Reflect<T>.GetMethod(methodPicker));
        }

        public static IMethodContext ForMethod<T, TInput1, TInput2>(this ISpecimenBuilderComposer composer, Expression<Action<T, TInput1, TInput2>> methodPicker)
        {
            return composer.ForMethod(Reflect<T>.GetMethod(methodPicker));
        }

        public static IMethodContext ForMethod<T, TInput1, TInput2, TInput3>(this ISpecimenBuilderComposer composer, Expression<Action<T, TInput1, TInput2, TInput3>> methodPicker)
        {
            return composer.ForMethod(Reflect<T>.GetMethod(methodPicker));
        }

        public static IMethodContext ForMethod(this ISpecimenBuilderComposer composer, MethodInfo methodInfo)
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

        public static IMethodContext ForConstructor(this ISpecimenBuilderComposer composer, ConstructorInfo constructorInfo)
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