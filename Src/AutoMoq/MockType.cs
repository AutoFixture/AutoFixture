using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Moq;
using Moq.Language;
using Moq.Language.Flow;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoMoq
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
                    && type.IsGenericType
                    && typeof (Mock<>).IsAssignableFrom(type.GetGenericTypeDefinition())
                    && !type.GetMockedType().IsGenericParameter);
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
            return type.GetConstructor(new[] {typeof (object[])});
        }

        internal static Type GetMockedType(this Type type)
        {
            return type.GetGenericArguments().Single();
        }

        /// <summary>
        /// Sets up a non-void method with a given member access expression, and returns an instance of <see cref="ISetup{TMock, TResult}"/>
        /// </summary>
        /// <param name="mock">The mock being set up.</param>
        /// <param name="memberType">The return type of the member being set up.</param>
        /// <param name="memberAccessExpression">The expression needed to setup the member.</param>
        /// <returns>The result of setting up <paramref name="mock"/> with <paramref name="memberAccessExpression"/>.</returns>
        internal static object Setup(this Mock mock, Type memberType, Expression memberAccessExpression)
        {
            var setupMethod = mock.GetType()
                                  .GetMethods()
                                  .First(method => method.Name == "Setup" &&
                                                   method.IsGenericMethod &&
                                                   method.GetGenericArguments().Count() == 1)
                                  .MakeGenericMethod(memberType);

            return setupMethod.Invoke(mock, new object[] {memberAccessExpression});
        }

        /// <summary>
        /// Sets up a void method with a given member access expression, and returns an instance of <see cref="ISetup{TMock}"/>
        /// </summary>
        /// <param name="mock">The mock being set up.</param>
        /// <param name="memberAccessExpression">The expression needed to setup the member.</param>
        /// <returns>The result of setting up <paramref name="mock"/> with <paramref name="memberAccessExpression"/>.</returns>
        internal static object Setup(this Mock mock, Expression memberAccessExpression)
        {
            var setupMethod = mock.GetType()
                                  .GetMethods()
                                  .First(method => method.Name == "Setup" &&
                                                   !method.IsGenericMethod);

            return setupMethod.Invoke(mock, new object[] {memberAccessExpression});
        }

        /// <summary>
        /// Configures an instance of <see cref="ISetup{TMock,TResult}"/> to retrieve the return value from <paramref name="context"/>.
        /// </summary>
        /// <param name="setup">An instance of <see cref="ISetup{TMock,TResult}"/>.</param>
        /// <param name="context">The context (fixture) that will be used to retrieve the return value for the mock's member being setup.</param>
        /// <param name="mockedType">The type of the object being mocked.</param>
        /// <param name="memberType">The return type of the member of being setup.</param>
        /// <returns>The result of setting up <paramref name="setup"/> to retrieve the return value from <paramref name="context"/>.</returns>
        internal static object ReturnsUsingContext(this object setup, ISpecimenContext context,
                                                   Type mockedType, Type memberType)
        {
            var returns = typeof (MockType).GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
                                           .Where(method => method.Name == "ReturnsUsingContext")
                                           .Single(method => method.IsGenericMethod)
                                           .MakeGenericMethod(new[] {mockedType, memberType});

            return returns.Invoke(null, new[] {setup, context});
        }

        internal static IReturnsResult<TMock> ReturnsUsingContext<TMock, TResult>(this IReturns<TMock, TResult> setup,
                                                                                  ISpecimenContext context)
            where TMock : class
        {
            return setup.Returns(() =>
                {
                    var result = (TResult) context.Resolve(typeof (TResult));
                    setup.Returns(result);
                    return result;
                });
        }
    }
}
