using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;
using Moq;
using Moq.Language;
using Moq.Language.Flow;

namespace AutoFixture.AutoMoq
{
    /// <summary>
    /// Contains extension methods to manipulate/setup instances of <see cref="Mock{T}"/>.
    /// </summary>
    public static class MockType
    {
        /// <summary>
        /// Sets up a member to lazily retrieve the return value from a fixture.
        /// </summary>
        /// <typeparam name="TMock">The type of the object being mocked.</typeparam>
        /// <typeparam name="TResult">The return type of the object's member being mocked.</typeparam>
        /// <param name="setup">The member setup.</param>
        /// <param name="fixture">The fixture from which the return value will be retrieved.</param>
        /// <returns>The result of setting up <paramref name="setup"/> to retrieve the return value from <paramref name="fixture"/>.</returns>
        [CLSCompliant(false)]
        public static IReturnsResult<TMock> ReturnsUsingFixture<TMock, TResult>(this IReturns<TMock, TResult> setup,
                                                                                ISpecimenBuilder fixture)
            where TMock : class
        {
            if (setup == null) throw new ArgumentNullException("setup");
            if (fixture == null) throw new ArgumentNullException("fixture");

            return setup.ReturnsUsingContext(new SpecimenContext(fixture));
        }

        internal static bool IsMock(this Type type)
        {
            return (type != null
                && type.GetTypeInfo().IsGenericType
                && typeof(Mock<>).IsAssignableFrom(type.GetGenericTypeDefinition())
                && !type.GetMockedType().IsGenericParameter);
        }

        internal static bool IsDelegate(this Type type)
        {
            /* Test against MulticastDelegate instead of Delegate base class
             * because Brad Abrams says that we "should pretend that [Delegate 
             * and MulticaseDelegate] are merged and that only 
             * MulticastDelegate exists."
             * http://blogs.msdn.com/b/brada/archive/2004/02/05/68415.aspx */
            return typeof(MulticastDelegate).IsAssignableFrom(type.GetTypeInfo().BaseType);
        }

        internal static ConstructorInfo GetDefaultConstructor(this Type type)
        {
            return type.GetConstructor(Type.EmptyTypes);
        }

        internal static IEnumerable<ConstructorInfo> GetPublicAndProtectedConstructors(this Type type)
        {
            return type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(ctor => !ctor.IsPrivate);
        }

        internal static ConstructorInfo GetParamsConstructor(this Type type)
        {
            return type.GetConstructor(new[] { typeof(object[]) });
        }

        internal static Type GetMockedType(this Type type)
        {
            return type.GetTypeInfo().GetGenericArguments().Single();
        }

        internal static IReturnsResult<TMock> ReturnsUsingContext<TMock, TResult>(this IReturns<TMock, TResult> setup,
            ISpecimenContext context)
            where TMock : class
        {
            return setup.Returns(() =>
            {
                var specimen = context.Resolve(typeof (TResult));

                // check if specimen is null but member is non-nullable value type
                if (specimen == null && (default(TResult) != null))
                    throw new InvalidOperationException(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            "Tried to setup a member with a return type of {0}, but null was found instead.",
                            typeof (TResult)));

                // check if specimen can be safely converted to TResult
                if (specimen != null && !(specimen is TResult))
                    throw new InvalidOperationException(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            "Tried to setup a member with a return type of {0}, but an instance of {1} was found instead.",
                            typeof (TResult),
                            specimen.GetType()));

                TResult result = (TResult) specimen;

                //"cache" value for future invocations
                setup.Returns(result);
                return result;
            });
        }
    }
}
