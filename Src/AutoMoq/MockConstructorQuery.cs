using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoMoq
{
    /// <summary>
    /// Selects appropriate constructors to create <see cref="Moq.Mock{T}"/> instances.
    /// </summary>
    public class MockConstructorQuery : IMethodQuery
    {
        /// <summary>
        /// Selects constructors for the supplied <see cref="Moq.Mock{T}"/> type.
        /// </summary>
        /// <param name="type">The mock type.</param>
        /// <returns>
        /// Constructors for <paramref name="type"/>.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method only returns constructors if <paramref name="type"/> is a
        /// <see cref="Moq.Mock{T}"/> type. If not, an empty sequence is returned.
        /// </para>
        /// <para>
        /// If the type is the type of a constructed <see cref="Moq.Mock{T}"/>, constructors are
        /// returned according to the generic type argument's constructors. If the type is an
        /// interface, the <see cref="Moq.Mock{T}()"/> default constructor is returned. If the type
        /// is a class, constructors are returned according to all the public and protected
        /// constructors of the underlying type. In this case, the
        /// <see cref="Moq.Mock{T}(object[])"/> constructor that takes a params array is returned 
        /// for each underlying constructor, with information about the appropriate parameters for
        /// each constructor.
        /// </para>
        /// </remarks>
        public IEnumerable<IMethod> SelectMethods(Type type)
        {
            if (!type.IsMock())
            {
                return Enumerable.Empty<IMethod>();
            }

            var mockType = type.GetMockedType();
            if (mockType.IsInterface || mockType.IsDelegate())
            {
                return new[] { new ConstructorMethod(type.GetDefaultConstructor()) };
            }

            return from ci in mockType.GetPublicAndProtectedConstructors()
                   let paramInfos = ci.GetParameters()
                   orderby paramInfos.Length ascending
                   select new MockConstructorMethod(type.GetParamsConstructor(), paramInfos) as IMethod;
        }
    }
}
