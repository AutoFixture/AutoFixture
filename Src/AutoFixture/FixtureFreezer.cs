using System;
using System.ComponentModel;
using Ploeh.AutoFixture.Dsl;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
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
        /// The <see cref="Freeze(IFixture)"/> method freezes the type to always return the same
        /// instance whenever an instance of the type is requested either directly, or indirectly as a
        /// nested value of other types.
        /// </para>
        /// </remarks>
        /// <seealso cref="Freeze{T}(IFixture, T)"/>
        /// <seealso cref="Freeze{T}(IFixture, Func{ICustomizationComposer{T}, ISpecimenBuilder})"/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Although this CA warning should never be suppressed, this particular usage scenario has been discussed and accepted on the FxCop DL.")]
        public static T Freeze<T>(this IFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            var value = fixture.Create<T>();
            fixture.Inject(value);
            return value;
        }

        /// <summary>
        /// Freezes the type to a single value.
        /// </summary>
        /// <typeparam name="T">The type to freeze.</typeparam>
        /// <param name="fixture">The fixture.</param>
        /// <param name="seed">
        /// Any data that adds additional information when creating the
        /// anonymous object. Hypothetically, this value might be the value
        /// being frozen, but this is not likely.
        /// </param>
        /// <returns>
        /// The value that will subsequently always be created for <typeparamref name="T"/>.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The <see cref="Freeze{T}(IFixture, T)"/> method freezes the type to always return the
        /// same instance whenever an instance of the type is requested either directly, or
        /// indirectly as a nested value of other types.
        /// </para>
        /// <para>
        /// Please notice that the <paramref name="seed" /> isn't likely to be
        /// used as the frozen value, unless you've customized
        /// <paramref name="fixture" /> to do this. If you wish to inject a
        /// specific value into the Fixture, you should use the
        /// <see cref="FixtureRegistrar.Inject" /> method instead.
        /// </para>
        /// </remarks>
        /// <seealso cref="Freeze{T}(IFixture)"/>
        /// <seealso cref="Freeze{T}(IFixture, Func{ICustomizationComposer{T}, ISpecimenBuilder})"/>
        /// <seealso cref="FixtureRegistrar.Inject" />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static T Freeze<T>(this IFixture fixture, T seed)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            var value = fixture.Create<T>(seed);
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
        /// The <see cref="Freeze{T}(IFixture, T)"/> method freezes the type to always return the
        /// same instance whenever an instance of the type is requested either directly, or
        /// indirectly as a nested value of other types. The frozen instance is created by an
        /// <see cref="ISpecimenBuilder" /> that is the result of applying the
        /// <paramref name="composerTransformation" />.
        /// </para>
        /// </remarks>
        /// <seealso cref="Freeze{T}(IFixture)"/>
        /// <seealso cref="Freeze{T}(IFixture, T)"/>
        public static T Freeze<T>(this IFixture fixture, Func<ICustomizationComposer<T>, ISpecimenBuilder> composerTransformation)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }
            if (composerTransformation == null)
            {
                throw new ArgumentNullException(nameof(composerTransformation));
            }

            var c = fixture.Build<T>();
            var value = composerTransformation(c).Create<T>();
            fixture.Inject(value);
            return value;
        }
    }
}
