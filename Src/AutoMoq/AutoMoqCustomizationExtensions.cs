using System;
using Moq;

namespace Ploeh.AutoFixture.AutoMoq
{
    /// <summary>
    /// Provides a set of customizations methods with ability 
    /// to setup <see cref="Mock{T}"/> object.
    /// </summary>
    public static class AutoMoqCustomizationExtensions
    {
        /// <summary>
        /// Customizes the creation of a <see cref="Mock{T}"/> object.
        /// </summary>
        /// <typeparam name="T">The mocked type.</typeparam>
        /// <param name="fixture">The fixture to customize.</param>
        /// <param name="setup">The setup method for a <see cref="Mock{T}"/> object.</param>
        public static void CustomizeMock<T>(
            this IFixture fixture,
            Action<Mock<T>> setup) where T : class
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));
            if (setup == null) throw new ArgumentNullException(nameof(setup));

            fixture.Customize<Mock<T>>(composer => composer
                .Do(setup));
        }

        /// <summary>
        /// Customizes the creation of a <see cref="Mock{T}"/> object.
        /// </summary>
        /// <typeparam name="T">The mocked type.</typeparam>
        /// <param name="fixture">The fixture to customize.</param>
        /// <param name="setup1">The first setup method for a <see cref="Mock{T}"/> object.</param>
        /// <param name="setup2">The second setup method for a <see cref="Mock{T}"/> object.</param>
        public static void CustomizeMock<T>(
            this IFixture fixture,
            Action<Mock<T>> setup1,
            Action<Mock<T>> setup2) where T : class
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));
            if (setup1 == null) throw new ArgumentNullException(nameof(setup1));
            if (setup2 == null) throw new ArgumentNullException(nameof(setup2));

            fixture.Customize<Mock<T>>(composer => composer
                .Do(setup2)
                .Do(setup2));
        }

        /// <summary>
        /// Customizes the creation of a <see cref="Mock{T}"/> object.
        /// </summary>
        /// <typeparam name="T">The mocked type.</typeparam>
        /// <param name="fixture">The fixture to customize.</param>
        /// <param name="setup1">The first setup method for a <see cref="Mock{T}"/> object.</param>
        /// <param name="setup2">The second setup method for a <see cref="Mock{T}"/> object.</param>
        /// <param name="setup3">The third setup method for a <see cref="Mock{T}"/> object.</param>
        public static void CustomizeMock<T>(
            this IFixture fixture,
            Action<Mock<T>> setup1,
            Action<Mock<T>> setup2,
            Action<Mock<T>> setup3) where T : class
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));
            if (setup1 == null) throw new ArgumentNullException(nameof(setup1));
            if (setup2 == null) throw new ArgumentNullException(nameof(setup2));
            if (setup3 == null) throw new ArgumentNullException(nameof(setup3));

            fixture.Customize<Mock<T>>(composer => composer
                .Do(setup2)
                .Do(setup2)
                .Do(setup3));
        }

        /// <summary>
        /// Customizes the creation of a <see cref="Mock{T}"/> object.
        /// </summary>
        /// <typeparam name="T">The mocked type.</typeparam>
        /// <param name="fixture">The fixture to customize.</param>
        /// <param name="setup1">The first setup method for a <see cref="Mock{T}"/> object.</param>
        /// <param name="setup2">The second setup method for a <see cref="Mock{T}"/> object.</param>
        /// <param name="setup3">The third setup method for a <see cref="Mock{T}"/> object.</param>
        /// <param name="setup4">The fourth setup method for a <see cref="Mock{T}"/> object.</param>
        public static void CustomizeMock<T>(
            this IFixture fixture,
            Action<Mock<T>> setup1,
            Action<Mock<T>> setup2,
            Action<Mock<T>> setup3,
            Action<Mock<T>> setup4) where T : class
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));
            if (setup1 == null) throw new ArgumentNullException(nameof(setup1));
            if (setup2 == null) throw new ArgumentNullException(nameof(setup2));
            if (setup3 == null) throw new ArgumentNullException(nameof(setup3));
            if (setup4 == null) throw new ArgumentNullException(nameof(setup4));

            fixture.Customize<Mock<T>>(composer => composer
                .Do(setup2)
                .Do(setup2)
                .Do(setup3)
                .Do(setup4));
        }

        /// <summary>
        /// Customizes the creation of a <see cref="Mock{T}"/> object.
        /// </summary>
        /// <typeparam name="T">The mocked type.</typeparam>
        /// <param name="fixture">The fixture to customize.</param>
        /// <param name="setup1">The first setup method for a <see cref="Mock{T}"/> object.</param>
        /// <param name="setup2">The second setup method for a <see cref="Mock{T}"/> object.</param>
        /// <param name="setup3">The third setup method for a <see cref="Mock{T}"/> object.</param>
        /// <param name="setup4">The fourth setup method for a <see cref="Mock{T}"/> object.</param>
        /// <param name="setup5">The fifth setup method for a <see cref="Mock{T}"/> object.</param>
        public static void CustomizeMock<T>(
            this IFixture fixture,
            Action<Mock<T>> setup1,
            Action<Mock<T>> setup2,
            Action<Mock<T>> setup3,
            Action<Mock<T>> setup4,
            Action<Mock<T>> setup5) where T : class
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));
            if (setup1 == null) throw new ArgumentNullException(nameof(setup1));
            if (setup2 == null) throw new ArgumentNullException(nameof(setup2));
            if (setup3 == null) throw new ArgumentNullException(nameof(setup3));
            if (setup4 == null) throw new ArgumentNullException(nameof(setup4));
            if (setup5 == null) throw new ArgumentNullException(nameof(setup5));

            fixture.Customize<Mock<T>>(composer => composer
                .Do(setup2)
                .Do(setup2)
                .Do(setup3)
                .Do(setup4)
                .Do(setup5));
        }

        /// <summary>
        /// Customizes the creation of a <see cref="Mock{T}"/> object.
        /// </summary>
        /// <typeparam name="T">The mocked type.</typeparam>
        /// <param name="fixture">The fixture to customize.</param>
        /// <param name="setup1">The first setup method for a <see cref="Mock{T}"/> object.</param>
        /// <param name="setup2">The second setup method for a <see cref="Mock{T}"/> object.</param>
        /// <param name="setup3">The third setup method for a <see cref="Mock{T}"/> object.</param>
        /// <param name="setup4">The fourth setup method for a <see cref="Mock{T}"/> object.</param>
        /// <param name="setup5">The fifth setup method for a <see cref="Mock{T}"/> object.</param>
        /// <param name="setup6">The sixth setup method for a <see cref="Mock{T}"/> object.</param>
        public static void CustomizeMock<T>(
            this IFixture fixture,
            Action<Mock<T>> setup1,
            Action<Mock<T>> setup2,
            Action<Mock<T>> setup3,
            Action<Mock<T>> setup4,
            Action<Mock<T>> setup5,
            Action<Mock<T>> setup6) where T : class
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));
            if (setup1 == null) throw new ArgumentNullException(nameof(setup1));
            if (setup2 == null) throw new ArgumentNullException(nameof(setup2));
            if (setup3 == null) throw new ArgumentNullException(nameof(setup3));
            if (setup4 == null) throw new ArgumentNullException(nameof(setup4));
            if (setup5 == null) throw new ArgumentNullException(nameof(setup5));
            if (setup6 == null) throw new ArgumentNullException(nameof(setup6));

            fixture.Customize<Mock<T>>(composer => composer
                .Do(setup2)
                .Do(setup2)
                .Do(setup3)
                .Do(setup4)
                .Do(setup5)
                .Do(setup6));
        }

        /// <summary>
        /// Customizes the creation of a <see cref="Mock{T}"/> object.
        /// </summary>
        /// <typeparam name="T">The mocked type.</typeparam>
        /// <param name="fixture">The fixture to customize.</param>
        /// <param name="setup1">The first setup method for a <see cref="Mock{T}"/> object.</param>
        /// <param name="setup2">The second setup method for a <see cref="Mock{T}"/> object.</param>
        /// <param name="setup3">The third setup method for a <see cref="Mock{T}"/> object.</param>
        /// <param name="setup4">The fourth setup method for a <see cref="Mock{T}"/> object.</param>
        /// <param name="setup5">The fifth setup method for a <see cref="Mock{T}"/> object.</param>
        /// <param name="setup6">The sixth setup method for a <see cref="Mock{T}"/> object.</param>
        /// <param name="setup7">The seventh setup method for a <see cref="Mock{T}"/> object.</param>
        public static void CustomizeMock<T>(
            this IFixture fixture,
            Action<Mock<T>> setup1,
            Action<Mock<T>> setup2,
            Action<Mock<T>> setup3,
            Action<Mock<T>> setup4,
            Action<Mock<T>> setup5,
            Action<Mock<T>> setup6,
            Action<Mock<T>> setup7) where T : class
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));
            if (setup1 == null) throw new ArgumentNullException(nameof(setup1));
            if (setup2 == null) throw new ArgumentNullException(nameof(setup2));
            if (setup3 == null) throw new ArgumentNullException(nameof(setup3));
            if (setup4 == null) throw new ArgumentNullException(nameof(setup4));
            if (setup5 == null) throw new ArgumentNullException(nameof(setup5));
            if (setup6 == null) throw new ArgumentNullException(nameof(setup6));
            if (setup7 == null) throw new ArgumentNullException(nameof(setup7));

            fixture.Customize<Mock<T>>(composer => composer
                .Do(setup2)
                .Do(setup2)
                .Do(setup3)
                .Do(setup4)
                .Do(setup5)
                .Do(setup6)
                .Do(setup7));
        }

        /// <summary>
        /// Freezes the <see cref="Mock{T}"/> object to a single value.
        /// </summary>
        /// <typeparam name="T">The mocked type to freeze.</typeparam>
        /// <param name="fixture">The fixture.</param>
        /// <param name="setup">The setup method for a <see cref="Mock{T}"/> object.</param>
        /// <returns>
        /// The <see cref="Mock{T}"/> object that will subsequently always be created for <typeparamref name="T"/>.
        /// </returns>
        public static Mock<T> FreezeMock<T>(
            this IFixture fixture,
            Action<Mock<T>> setup) where T : class
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));
            if (setup == null) throw new ArgumentNullException(nameof(setup));

            return fixture.Freeze<Mock<T>>(composer => composer
                .Do(setup));
        }

        /// <summary>
        /// Freezes the <see cref="Mock{T}"/> object to a single value.
        /// </summary>
        /// <typeparam name="T">The mocked type to freeze.</typeparam>
        /// <param name="fixture">The fixture.</param>
        /// <param name="setup1">The first setup method for a <see cref="Mock{T}"/> object.</param>
        /// <param name="setup2">The second setup method for a <see cref="Mock{T}"/> object.</param>
        /// <returns>
        /// The <see cref="Mock{T}"/> object that will subsequently always be created for <typeparamref name="T"/>.
        /// </returns>
        public static Mock<T> FreezeMock<T>(
            this IFixture fixture,
            Action<Mock<T>> setup1,
            Action<Mock<T>> setup2) where T : class
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));
            if (setup1 == null) throw new ArgumentNullException(nameof(setup1));
            if (setup2 == null) throw new ArgumentNullException(nameof(setup2));

            return fixture.Freeze<Mock<T>>(composer => composer
                .Do(setup1)
                .Do(setup2));
        }

        /// <summary>
        /// Freezes the <see cref="Mock{T}"/> object to a single value.
        /// </summary>
        /// <typeparam name="T">The mocked type to freeze.</typeparam>
        /// <param name="fixture">The fixture.</param>
        /// <param name="setup1">The first setup method for a <see cref="Mock{T}"/> object.</param>
        /// <param name="setup2">The second setup method for a <see cref="Mock{T}"/> object.</param>
        /// <param name="setup3">The third setup method for a <see cref="Mock{T}"/> object.</param>
        /// <returns>
        /// The <see cref="Mock{T}"/> object that will subsequently always be created for <typeparamref name="T"/>.
        /// </returns>
        public static Mock<T> FreezeMock<T>(
            this IFixture fixture,
            Action<Mock<T>> setup1,
            Action<Mock<T>> setup2,
            Action<Mock<T>> setup3) where T : class
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));
            if (setup1 == null) throw new ArgumentNullException(nameof(setup1));
            if (setup2 == null) throw new ArgumentNullException(nameof(setup2));
            if (setup3 == null) throw new ArgumentNullException(nameof(setup3));

            return fixture.Freeze<Mock<T>>(composer => composer
                .Do(setup1)
                .Do(setup2)
                .Do(setup3));
        }

        /// <summary>
        /// Freezes the <see cref="Mock{T}"/> object to a single value.
        /// </summary>
        /// <typeparam name="T">The mocked type to freeze.</typeparam>
        /// <param name="fixture">The fixture.</param>
        /// <param name="setup1">The first setup method for a <see cref="Mock{T}"/> object.</param>
        /// <param name="setup2">The second setup method for a <see cref="Mock{T}"/> object.</param>
        /// <param name="setup3">The third setup method for a <see cref="Mock{T}"/> object.</param>
        /// <param name="setup4">The fourth setup method for a <see cref="Mock{T}"/> object.</param>
        /// <returns>
        /// The <see cref="Mock{T}"/> object that will subsequently always be created for <typeparamref name="T"/>.
        /// </returns>
        public static Mock<T> FreezeMock<T>(
            this IFixture fixture,
            Action<Mock<T>> setup1,
            Action<Mock<T>> setup2,
            Action<Mock<T>> setup3,
            Action<Mock<T>> setup4) where T : class
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));
            if (setup1 == null) throw new ArgumentNullException(nameof(setup1));
            if (setup2 == null) throw new ArgumentNullException(nameof(setup2));
            if (setup3 == null) throw new ArgumentNullException(nameof(setup3));
            if (setup4 == null) throw new ArgumentNullException(nameof(setup4));

            return fixture.Freeze<Mock<T>>(composer => composer
                .Do(setup1)
                .Do(setup2)
                .Do(setup3)
                .Do(setup4));
        }

        /// <summary>
        /// Freezes the <see cref="Mock{T}"/> object to a single value.
        /// </summary>
        /// <typeparam name="T">The mocked type to freeze.</typeparam>
        /// <param name="fixture">The fixture.</param>
        /// <param name="setup1">The first setup method for a <see cref="Mock{T}"/> object.</param>
        /// <param name="setup2">The second setup method for a <see cref="Mock{T}"/> object.</param>
        /// <param name="setup3">The third setup method for a <see cref="Mock{T}"/> object.</param>
        /// <param name="setup4">The fourth setup method for a <see cref="Mock{T}"/> object.</param>
        /// <param name="setup5">The fifth setup method for a <see cref="Mock{T}"/> object.</param>
        /// <returns>
        /// The <see cref="Mock{T}"/> object that will subsequently always be created for <typeparamref name="T"/>.
        /// </returns>
        public static Mock<T> FreezeMock<T>(
            this IFixture fixture,
            Action<Mock<T>> setup1,
            Action<Mock<T>> setup2,
            Action<Mock<T>> setup3,
            Action<Mock<T>> setup4,
            Action<Mock<T>> setup5) where T : class
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));
            if (setup1 == null) throw new ArgumentNullException(nameof(setup1));
            if (setup2 == null) throw new ArgumentNullException(nameof(setup2));
            if (setup3 == null) throw new ArgumentNullException(nameof(setup3));
            if (setup4 == null) throw new ArgumentNullException(nameof(setup4));
            if (setup5 == null) throw new ArgumentNullException(nameof(setup5));

            return fixture.Freeze<Mock<T>>(composer => composer
                .Do(setup1)
                .Do(setup2)
                .Do(setup3)
                .Do(setup4)
                .Do(setup5));
        }

        /// <summary>
        /// Freezes the <see cref="Mock{T}"/> object to a single value.
        /// </summary>
        /// <typeparam name="T">The mocked type to freeze.</typeparam>
        /// <param name="fixture">The fixture.</param>
        /// <param name="setup1">The first setup method for a <see cref="Mock{T}"/> object.</param>
        /// <param name="setup2">The second setup method for a <see cref="Mock{T}"/> object.</param>
        /// <param name="setup3">The third setup method for a <see cref="Mock{T}"/> object.</param>
        /// <param name="setup4">The fourth setup method for a <see cref="Mock{T}"/> object.</param>
        /// <param name="setup5">The fifth setup method for a <see cref="Mock{T}"/> object.</param>
        /// <param name="setup6">The sixth setup method for a <see cref="Mock{T}"/> object.</param>
        /// <returns>
        /// The <see cref="Mock{T}"/> object that will subsequently always be created for <typeparamref name="T"/>.
        /// </returns>
        public static Mock<T> FreezeMock<T>(
            this IFixture fixture,
            Action<Mock<T>> setup1,
            Action<Mock<T>> setup2,
            Action<Mock<T>> setup3,
            Action<Mock<T>> setup4,
            Action<Mock<T>> setup5,
            Action<Mock<T>> setup6) where T : class
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));
            if (setup1 == null) throw new ArgumentNullException(nameof(setup1));
            if (setup2 == null) throw new ArgumentNullException(nameof(setup2));
            if (setup3 == null) throw new ArgumentNullException(nameof(setup3));
            if (setup4 == null) throw new ArgumentNullException(nameof(setup4));
            if (setup5 == null) throw new ArgumentNullException(nameof(setup5));
            if (setup6 == null) throw new ArgumentNullException(nameof(setup6));

            return fixture.Freeze<Mock<T>>(composer => composer
                .Do(setup1)
                .Do(setup2)
                .Do(setup3)
                .Do(setup4)
                .Do(setup5)
                .Do(setup6));
        }

        /// <summary>
        /// Freezes the <see cref="Mock{T}"/> object to a single value.
        /// </summary>
        /// <typeparam name="T">The mocked type to freeze.</typeparam>
        /// <param name="fixture">The fixture.</param>
        /// <param name="setup1">The first setup method for a <see cref="Mock{T}"/> object.</param>
        /// <param name="setup2">The second setup method for a <see cref="Mock{T}"/> object.</param>
        /// <param name="setup3">The third setup method for a <see cref="Mock{T}"/> object.</param>
        /// <param name="setup4">The fourth setup method for a <see cref="Mock{T}"/> object.</param>
        /// <param name="setup5">The fifth setup method for a <see cref="Mock{T}"/> object.</param>
        /// <param name="setup6">The sixth setup method for a <see cref="Mock{T}"/> object.</param>
        /// <param name="setup7">The seventh setup method for a <see cref="Mock{T}"/> object.</param>
        /// <returns>
        /// The <see cref="Mock{T}"/> object that will subsequently always be created for <typeparamref name="T"/>.
        /// </returns>
        public static Mock<T> FreezeMock<T>(
            this IFixture fixture,
            Action<Mock<T>> setup1,
            Action<Mock<T>> setup2,
            Action<Mock<T>> setup3,
            Action<Mock<T>> setup4,
            Action<Mock<T>> setup5,
            Action<Mock<T>> setup6,
            Action<Mock<T>> setup7) where T : class
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));
            if (setup1 == null) throw new ArgumentNullException(nameof(setup1));
            if (setup2 == null) throw new ArgumentNullException(nameof(setup2));
            if (setup3 == null) throw new ArgumentNullException(nameof(setup3));
            if (setup4 == null) throw new ArgumentNullException(nameof(setup4));
            if (setup5 == null) throw new ArgumentNullException(nameof(setup5));
            if (setup6 == null) throw new ArgumentNullException(nameof(setup6));
            if (setup7 == null) throw new ArgumentNullException(nameof(setup7));

            return fixture.Freeze<Mock<T>>(composer => composer
                .Do(setup1)
                .Do(setup2)
                .Do(setup3)
                .Do(setup4)
                .Do(setup5)
                .Do(setup6)
                .Do(setup7));
        }
    }
}