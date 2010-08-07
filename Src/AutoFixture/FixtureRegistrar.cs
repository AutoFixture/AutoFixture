using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Contains extension methods for registering specimens in <see cref="IFixture"/> instances.
    /// </summary>
    public static class FixtureRegistrar
    {
        /// <summary>
        /// Injects a specific instance for a specific type.
        /// </summary>
        /// <typeparam name="T">
        /// The type for which <paramref name="item"/> should be injected.
        /// </typeparam>
        /// <param name="fixture">The fixture.</param>
        /// <param name="item">The item to inject.</param>
        public static void Inject<T>(this IFixture fixture, T item)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }

            fixture.Customize<T>(c => c.FromFactory(() => item).OmitAutoProperties());
        }

        /// <summary>
        /// Registers a creation function for a specifc type.
        /// </summary>
        /// <typeparam name="T">
        /// The type for which <paramref name="creator"/> should be registered.
        /// </typeparam>
        /// <param name="fixture">The fixture.</param>
        /// <param name="creator">
        /// A function that will be used to create objects of type <typeparamref name="T"/> every
        /// time the <see cref="Fixture"/> is asked to create an object of that type.
        /// </param>
        public static void Register<T>(this IFixture fixture, Func<T> creator)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }

            fixture.Customize<T>(c => c.FromFactory(creator).OmitAutoProperties());
        }

        /// <summary>
        /// Registers a creation function for a specific type, when that creation function requires
        /// a single input parameter.
        /// </summary>
        /// <typeparam name="TInput">
        /// The type of the input parameter used by <paramref name="creator"/>.
        /// </typeparam>
        /// <typeparam name="T">
        /// The type for which <paramref name="creator"/> should be registered.
        /// </typeparam>
        /// <param name="fixture">The fixture.</param>
        /// <param name="creator">
        /// A function that will be used to create objects of type <typeparamref name="T"/> every
        /// time the <see cref="Fixture"/> is asked to create an object of that type.
        /// </param>
        public static void Register<TInput, T>(this IFixture fixture, Func<TInput, T> creator)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }

            fixture.Customize<T>(c => c.FromFactory(creator).OmitAutoProperties());
        }

        /// <summary>
        /// Registers a creation function for a specific type, when that creation function requires
        /// two input parameters.
        /// </summary>
        /// <typeparam name="TInput1">
        /// The type of the first input parameter used by <paramref name="creator"/>.
        /// </typeparam>
        /// <typeparam name="TInput2">
        /// The type of the second input parameter used by <paramref name="creator"/>.
        /// </typeparam>
        /// <typeparam name="T">
        /// The type for which <paramref name="creator"/> should be registered.
        /// </typeparam>
        /// <param name="fixture">The fixture.</param>
        /// <param name="creator">
        /// A function that will be used to create objects of type <typeparamref name="T"/> every
        /// time the <see cref="Fixture"/> is asked to create an object of that type.
        /// </param>
        public static void Register<TInput1, TInput2, T>(this IFixture fixture, Func<TInput1, TInput2, T> creator)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }

            fixture.Customize<T>(c => c.FromFactory(creator).OmitAutoProperties());
        }

        /// <summary>
        /// Registers a creation function for a specific type, when that creation function requires
        /// three input parameters.
        /// </summary>
        /// <typeparam name="TInput1">
        /// The type of the first input parameter used by <paramref name="creator"/>.
        /// </typeparam>
        /// <typeparam name="TInput2">
        /// The type of the second input parameter used by <paramref name="creator"/>.
        /// </typeparam>
        /// <typeparam name="TInput3">
        /// The type of the third input parameter used by <paramref name="creator"/>.
        /// </typeparam>
        /// <typeparam name="T">
        /// The type for which <paramref name="creator"/> should be registered.
        /// </typeparam>
        /// <param name="fixture">The fixture.</param>
        /// <param name="creator">
        /// A function that will be used to create objects of type <typeparamref name="T"/> every
        /// time the <see cref="Fixture"/> is asked to create an object of that type.
        /// </param>
        public static void Register<TInput1, TInput2, TInput3, T>(this IFixture fixture, Func<TInput1, TInput2, TInput3, T> creator)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }

            fixture.Customize<T>(c => c.FromFactory(creator).OmitAutoProperties());
        }

        /// <summary>
        /// Registers a creation function for a specific type, when that creation function requires
        /// four input parameters.
        /// </summary>
        /// <typeparam name="TInput1">
        /// The type of the first input parameter used by <paramref name="creator"/>.
        /// </typeparam>
        /// <typeparam name="TInput2">
        /// The type of the second input parameter used by <paramref name="creator"/>.
        /// </typeparam>
        /// <typeparam name="TInput3">
        /// The type of the third input parameter used by <paramref name="creator"/>.
        /// </typeparam>
        /// <typeparam name="TInput4">
        /// The type of the fourth input parameter used by <paramref name="creator"/>.
        /// </typeparam>
        /// <typeparam name="T">
        /// The type for which <paramref name="creator"/> should be registered.
        /// </typeparam>
        /// <param name="fixture">The fixture.</param>
        /// <param name="creator">
        /// A function that will be used to create objects of type <typeparamref name="T"/> every
        /// time the <see cref="Fixture"/> is asked to create an object of that type.
        /// </param>
        public static void Register<TInput1, TInput2, TInput3, TInput4, T>(this IFixture fixture, Func<TInput1, TInput2, TInput3, TInput4, T> creator)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }

            fixture.Customize<T>(c => c.FromFactory(creator).OmitAutoProperties());
        }
    }
}
