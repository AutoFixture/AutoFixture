using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Provides unit testing idioms expressed via <see cref="ISpecimenBuilderComposer"/>
    /// instances.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When writing unit tests with an <see cref="ISpecimenBuilderComposer"/> instance (typically
    /// a <see cref="Fixture"/>), a lot of tests turn out to follow repeated idioms. These
    /// extension methods provide an entry point to such idiomatic verifications.
    /// </para>
    /// </remarks>
    public static class SpecimenBuilderComposerIdioms
    {
        /// <summary>
        /// Provides a <see cref="IPropertyContext"/> for a particular property. The context can be
        /// used to declare the execution of idiomatic assertions related to the property.
        /// </summary>
        /// <typeparam name="T">The type owning the property.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="composer">The composer.</param>
        /// <param name="property">An expression that identifies the property.</param>
        /// <returns>
        /// An <see cref="IPropertyContext"/> that encapsulates <paramref name="property"/>.
        /// </returns>
        public static IPropertyContext ForProperty<T, TProperty>(this ISpecimenBuilderComposer composer, Expression<Func<T, TProperty>> property)
        {
            var propertyInfo = Reflect<T>.GetProperty(property);
            return composer.ForProperty(propertyInfo);
        }

        /// <summary>
        /// Provides a <see cref="IPropertyContext"/> for a particular property. The context can be
        /// used to declare the execution of idiomatic assertions related to the property.
        /// </summary>
        /// <param name="composer">The composer.</param>
        /// <param name="propertyInfo">The property.</param>
        /// <returns>
        /// An <see cref="IPropertyContext"/> that encapsulates <paramref name="propertyInfo"/>.
        /// </returns>
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

        /// <summary>
        /// Provides a <see cref="IPropertyContext"/> that aggregates all properties owned by a
        /// type. The context can be used to declare the execution of idiomatic assertions related
        /// to the properties.
        /// </summary>
        /// <typeparam name="T">The type owning the properties.</typeparam>
        /// <param name="composer">The composer.</param>
        /// <returns>An <see cref="IPropertyContext"/> that encapsulates the properties.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Although this CA warning should never be suppressed, this particular usage scenario has been discussed and accepted on the internal Microsoft FxCop DL.")]
        public static IPropertyContext ForAllPropertiesOf<T>(this ISpecimenBuilderComposer composer)
        {
            return composer.ForAllPropertiesOf(typeof(T));
        }

        /// <summary>
        /// Provides a <see cref="IPropertyContext"/> that aggregates all properties owned by a
        /// type. The context can be used to declare the execution of idiomatic assertions related
        /// to the properties.
        /// </summary>
        /// <param name="composer">The composer.</param>
        /// <param name="type">The type owning the properties.</param>
        /// <returns>An <see cref="IPropertyContext"/> that encapsulates the properties.</returns>
        public static IPropertyContext ForAllPropertiesOf(this ISpecimenBuilderComposer composer, Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return new CompositePropertyContext(from p in type.GetProperties()
                                                select new PropertyContext(composer, p) as IPropertyContext);
        }

        /// <summary>
        /// Provides an <see cref="IMethodContext"/> for a particular method. The context can be
        /// used to declare the execution of idiomatic assertions related to the method.
        /// </summary>
        /// <typeparam name="T">The type owning the method.</typeparam>
        /// <param name="composer">The composer.</param>
        /// <param name="methodPicker">An expression that identifies the method.</param>
        /// <returns>
        /// An <see cref="IMethodContext"/> that encapsulates the method identified by
        /// <paramref name="methodPicker"/>.
        /// </returns>
        public static IMethodContext ForMethod<T>(this ISpecimenBuilderComposer composer, Expression<Action<T>> methodPicker)
        {
            return composer.ForMethod(Reflect<T>.GetMethod(methodPicker));
        }

        /// <summary>
        /// Provides an <see cref="IMethodContext"/> for a particular method. The context can be
        /// used to declare the execution of idiomatic assertions related to the method.
        /// </summary>
        /// <typeparam name="T">The type owning the method.</typeparam>
        /// <typeparam name="TInput">The type of an input parameter to the method.</typeparam>
        /// <param name="composer">The composer.</param>
        /// <param name="methodPicker">An expression that identifies the method.</param>
        /// <returns>
        /// An <see cref="IMethodContext"/> that encapsulates the method identified by
        /// <paramref name="methodPicker"/>.
        /// </returns>
        public static IMethodContext ForMethod<T, TInput>(this ISpecimenBuilderComposer composer, Expression<Action<T, TInput>> methodPicker)
        {
            return composer.ForMethod(Reflect<T>.GetMethod(methodPicker));
        }

        /// <summary>
        /// Provides an <see cref="IMethodContext"/> for a particular method. The context can be
        /// used to declare the execution of idiomatic assertions related to the method.
        /// </summary>
        /// <typeparam name="T">The type owning the method.</typeparam>
        /// <typeparam name="TInput1">
        /// The type of the first input parameter to the method.
        /// </typeparam>
        /// <typeparam name="TInput2">
        /// The type of the second input parameter to the method.
        /// </typeparam>
        /// <param name="composer">The composer.</param>
        /// <param name="methodPicker">An expression that identifies the method.</param>
        /// <returns>
        /// An <see cref="IMethodContext"/> that encapsulates the method identified by
        /// <paramref name="methodPicker"/>.
        /// </returns>
        public static IMethodContext ForMethod<T, TInput1, TInput2>(this ISpecimenBuilderComposer composer, Expression<Action<T, TInput1, TInput2>> methodPicker)
        {
            return composer.ForMethod(Reflect<T>.GetMethod(methodPicker));
        }

        /// <summary>
        /// Provides an <see cref="IMethodContext"/> for a particular method. The context can be
        /// used to declare the execution of idiomatic assertions related to the method.
        /// </summary>
        /// <typeparam name="T">The type owning the method.</typeparam>
        /// <typeparam name="TInput1">
        /// The type of the first input parameter to the method.
        /// </typeparam>
        /// <typeparam name="TInput2">
        /// The type of the second input parameter to the method.
        /// </typeparam>
        /// <typeparam name="TInput3">
        /// The type of the third input parameter to the method.
        /// </typeparam>
        /// <param name="composer">The composer.</param>
        /// <param name="methodPicker">An expression that identifies the method.</param>
        /// <returns>
        /// An <see cref="IMethodContext"/> that encapsulates the method identified by
        /// <paramref name="methodPicker"/>.
        /// </returns>
        public static IMethodContext ForMethod<T, TInput1, TInput2, TInput3>(this ISpecimenBuilderComposer composer, Expression<Action<T, TInput1, TInput2, TInput3>> methodPicker)
        {
            return composer.ForMethod(Reflect<T>.GetMethod(methodPicker));
        }

        /// <summary>
        /// Provides an <see cref="IMethodContext"/> for a particular method. The context can be
        /// used to declare the execution of idiomatic assertions related to the method.
        /// </summary>
        /// <param name="composer">The composer.</param>
        /// <param name="methodInfo">The method.</param>
        /// <returns>
        /// An <see cref="IMethodContext"/> that encapsulates the method identified by
        /// <paramref name="methodInfo"/>.
        /// </returns>
        public static IMethodContext ForMethod(this ISpecimenBuilderComposer composer, MethodInfo methodInfo)
        {
            if (composer == null)
            {
                throw new ArgumentNullException("composer");
            }
            if (methodInfo == null)
            {
                throw new ArgumentNullException("methodInfo");
            }

            return new MethodContext(composer, methodInfo);
        }

        /// <summary>
        /// Provides a <see cref="IMethodContext"/> that aggregates all methods owned by a type.
        /// The context can be used to declare the execution of idiomatic assertions related
        /// to the methods.
        /// </summary>
        /// <typeparam name="T">The type owning the methods.</typeparam>
        /// <param name="composer">The composer.</param>
        /// <returns>An <see cref="IMethodContext"/> that encapsulates the methods.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Although this CA warning should never be suppressed, this particular usage scenario has been discussed and accepted on the internal Microsoft FxCop DL.")]
        public static IMethodContext ForAllMethodsOf<T>(this ISpecimenBuilderComposer composer)
        {
            return composer.ForAllMethodsOf(typeof(T));
        }

        /// <summary>
        /// Provides a <see cref="IMethodContext"/> that aggregates all methods owned by a type.
        /// The context can be used to declare the execution of idiomatic assertions related
        /// to the methods.
        /// </summary>
        /// <param name="composer">The composer.</param>
        /// <param name="type">The type owning the methods.</param>
        /// <returns>An <see cref="IMethodContext"/> that encapsulates the methods.</returns>
        public static IMethodContext ForAllMethodsOf(this ISpecimenBuilderComposer composer, Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return new CompositeMethodContext(from m in type.GetMethods().Except(type.GetProperties().SelectMany(p => p.GetAccessors()))
                                              where m.Name != "Equals"
                                              select new MethodContext(composer, m) as IMethodContext);
        }

        /// <summary>
        /// Provides an <see cref="IMethodContext"/> for a particular constructor. The context can
        /// beused to declare the execution of idiomatic assertions related to the constructor.
        /// </summary>
        /// <param name="composer">The composer.</param>
        /// <param name="constructorInfo">The constructor.</param>
        /// <returns>
        /// An <see cref="IMethodContext"/> that encapsulates the constructor identified by
        /// <paramref name="constructorInfo"/>.
        /// </returns>
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

        /// <summary>
        /// Provides a <see cref="IMethodContext"/> that aggregates all constructors of a type.
        /// The context can be used to declare the execution of idiomatic assertions related
        /// to the constructors.
        /// </summary>
        /// <typeparam name="T">The type owning the constructors.</typeparam>
        /// <param name="composer">The composer.</param>
        /// <returns>An <see cref="IMethodContext"/> that encapsulates the constructors.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Although this CA warning should never be suppressed, this particular usage scenario has been discussed and accepted on the internal Microsoft FxCop DL.")]
        public static IMethodContext ForAllConstructorsOf<T>(this ISpecimenBuilderComposer composer)
        {
            return composer.ForAllConstructorsOf(typeof(T));
        }

        /// <summary>
        /// Provides a <see cref="IMethodContext"/> that aggregates all constructors of a type.
        /// The context can be used to declare the execution of idiomatic assertions related
        /// to the constructors.
        /// </summary>
        /// <param name="composer">The composer.</param>
        /// <param name="type">The type owning the constructors.</param>
        /// <returns>An <see cref="IMethodContext"/> that encapsulates the constructors.</returns>
        public static IMethodContext ForAllConstructorsOf(this ISpecimenBuilderComposer composer, Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return new CompositeMethodContext(from c in type.GetConstructors()
                                              select new MethodContext(composer, c) as IMethodContext);
        }

        /// <summary>
        /// Provides a <see cref="IMemberContext"/> that aggregates all members of a type. The
        /// context can be used to declare the execution of idiomatic assertions related to the
        /// constructors.
        /// </summary>
        /// <typeparam name="T">The type owning the members.</typeparam>
        /// <param name="composer">The composer.</param>
        /// <returns>An <see cref="IMemberContext"/> that encapsulates the members.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Although this CA warning should never be suppressed, this particular usage scenario has been discussed and accepted on the internal Microsoft FxCop DL.")]
        public static IMemberContext ForAllMembersOf<T>(this ISpecimenBuilderComposer composer)
        {
            return composer.ForAllMembersOf(typeof(T));
        }

        /// <summary>
        /// Provides a <see cref="IMemberContext"/> that aggregates all members of a type. The
        /// context can be used to declare the execution of idiomatic assertions related to the
        /// constructors.
        /// </summary>
        /// <param name="composer">The composer.</param>
        /// <param name="type">The type owning the members.</param>
        /// <returns>An <see cref="IMemberContext"/> that encapsulates the members.</returns>
        public static IMemberContext ForAllMembersOf(this ISpecimenBuilderComposer composer, Type type)
        {
            return new CompositeMemberContext(
                composer.ForAllConstructorsOf(type),
                composer.ForAllMethodsOf(type),
                composer.ForAllPropertiesOf(type));
        }
    }
}