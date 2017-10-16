using System;
using System.ComponentModel;

namespace AutoFixture
{
    /// <summary>
    /// Contains extension methods for freezing specimens in <see cref="IFixture"/> instances with a specified seed.
    /// </summary>
    public static class FreezeSeedExtensions
    {
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
        /// <see cref="FixtureRegistrar.Inject{T}" /> method instead.
        /// </para>
        /// </remarks>
        /// <seealso cref="FixtureFreezer.Freeze{T}(IFixture)"/>
        /// <seealso cref="FixtureFreezer.Freeze{T}(AutoFixture.IFixture,System.Func{AutoFixture.Dsl.ICustomizationComposer{T},AutoFixture.Kernel.ISpecimenBuilder})"/>
        /// <seealso cref="FixtureRegistrar.Inject{T}" />
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
    }
}
