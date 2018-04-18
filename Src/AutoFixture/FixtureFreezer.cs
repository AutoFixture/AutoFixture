using System;
using AutoFixture.Dsl;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Contains extension methods for freezing specimens in <see cref="IFixture"/> instances.
    /// </summary>
    public static class FixtureFreezer
    {
        /// <summary>
        /// Freezes the type to a single value.
        /// </summary>
        /// <typeparam name="T">The type to freeze.</typeparam>
        /// <param name="fixture">The fixture.</param>
        /// <returns>
        /// The value that will subsequently always be created for <typeparamref name="T"/>.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The <see cref="Freeze{T}(AutoFixture.IFixture)"/> method freezes the type to always return the same
        /// instance whenever an instance of the type is requested either directly, or indirectly as a
        /// nested value of other types.
        /// </para>
        /// </remarks>
        /// <seealso cref="Freeze{T}(IFixture, Func{ICustomizationComposer{T}, ISpecimenBuilder})"/>
        public static T Freeze<T>(this IFixture fixture)
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

            var value = fixture.Create<T>();
            fixture.Inject(value);
            return value;
        }

        /// <summary>
        /// Freezes the type to a single value.
        /// </summary>
        /// <typeparam name="T">The type to freeze.</typeparam>
        /// <param name="fixture">The fixture.</param>
        /// <param name="composerTransformation">
        /// A function that customizes a given <see cref="ICustomizationComposer{T}"/> and returns
        /// the modified composer.
        /// </param>
        /// <returns>
        /// The value that will subsequently always be created for <typeparamref name="T"/>.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method freezes the type to always return the
        /// same instance whenever an instance of the type is requested either directly, or
        /// indirectly as a nested value of other types. The frozen instance is created by an
        /// <see cref="ISpecimenBuilder" /> that is the result of applying the
        /// <paramref name="composerTransformation" />.
        /// </para>
        /// </remarks>
        /// <seealso cref="Freeze{T}(IFixture)"/>
        public static T Freeze<T>(this IFixture fixture, Func<ICustomizationComposer<T>, ISpecimenBuilder> composerTransformation)
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));
            if (composerTransformation == null) throw new ArgumentNullException(nameof(composerTransformation));

            var c = fixture.Build<T>();
            var value = composerTransformation(c).Create<T>();
            fixture.Inject(value);
            return value;
        }
    }
}
