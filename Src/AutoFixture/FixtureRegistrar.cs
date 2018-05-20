using System;

namespace AutoFixture
{
    /// <summary>
    /// Contains extension methods for registering specimens in <see cref="IFixture"/> instances.
    /// </summary>
    public static class FixtureRegistrar
    {
        /// <summary>
        /// Injects a specific instance for a specific type, in order to make
        /// that instance a shared instance, no matter how many times the
        /// Fixture is asked to create an instance of that type.
        /// </summary>
        /// <typeparam name="T">
        /// The type for which <paramref name="item"/> should be injected.
        /// </typeparam>
        /// <param name="fixture">The fixture.</param>
        /// <param name="item">The item to inject.</param>
        /// <remarks>
        /// <para>
        /// Injecting an instance of a specific type into a
        /// <see cref="Fixture" /> effectively 'locks' the type to that
        /// specific instance. The injected <paramref name="item" /> becomes a
        /// shared instance. No matter how many times the Fixture instance is
        /// asked to create an instance of <typeparamref name="T" />, the
        /// shared item is returned.
        /// </para>
        /// <para>
        /// It's possible to inject a sub-type of T into the Fixture. As long
        /// as the item can be converted to T (i.e. as long at the code
        /// compiles), you can inject a sub-type of T into the Fixture. This
        /// can for example be used to lock an interface to a specific instance
        /// of a concrete type.
        /// </para>
        /// <para>
        /// If you are familiar with DI Container lifetime management, the
        /// following parallel may be helpful. If not, skip the next paragraph.
        /// </para>
        /// <para>
        /// Most DI Containers come with several built-in lifetime styles. The
        /// two most common lifetime styles are Transient (a new instance is
        /// created for every request) and Singleton (the same instance is used
        /// for all requests) (don't confuse the Singleton lifetime style with
        /// the Singleton design pattern; they are related, but different). By
        /// default, Fixture uses the Transient lifetime style: it creates a
        /// new instance for every request. However, using the Inject method,
        /// effectively changes the lifetime style for that particular type to
        /// Singleton.
        /// </para>
        /// </remarks>
        /// <example>
        /// This example demonstrates that when injecting an instance of the
        /// custom class MyClass into a Fixture instance, that Fixture instance
        /// will return the originally injected MyClass instance every time
        /// it's asked to create an instance of MyClass.
        /// <code>
        /// var fixture = new Fixture();
        /// var original = new MyClass();
        /// fixture.Inject(original);
        ///
        /// var actual1 = fixture.Create&lt;MyClass&gt;();
        /// var actual2 = fixture.Create&lt;MyClass&gt;();
        ///
        /// // actual1 and actual2 are equal, and equal to original
        /// Assert.Same(actual1, actual2);
        /// Assert.Same(original, actual1);
        /// Assert.Same(original, actual2);
        /// </code>
        /// </example>
        /// <seealso cref="Register{T}(IFixture, Func{T})" />
        /// <seealso cref="Register{TInput, T}(IFixture, Func{TInput, T})" />
        /// <seealso cref="Register{TInput1, TInput2, T}(IFixture, Func{TInput1, TInput2, T})" />
        /// <seealso cref="Register{TInput1, TInput2, TInput3, T}(IFixture, Func{TInput1, TInput2, TInput3, T})" />
        /// <seealso cref="Register{TInput1, TInput2, TInput3, TInput4, T}(IFixture, Func{TInput1, TInput2, TInput3, TInput4, T})" />
        public static void Inject<T>(this IFixture fixture, T item)
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

            fixture.Register(() => item);
        }

        /// <summary>
        /// Registers a creation function for a specific type.
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
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

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
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

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
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

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
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

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
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

            fixture.Customize<T>(c => c.FromFactory(creator).OmitAutoProperties());
        }
    }
}
