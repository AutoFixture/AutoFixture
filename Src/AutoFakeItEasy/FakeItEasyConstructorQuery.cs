using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoFakeItEasy
{
    /// <summary>
    /// Selects appropriate constructors to create <see cref="FakeItEasy.Fake{T}"/> instances.
    /// </summary>
#pragma warning disable 618
    public class FakeItEasyConstructorQuery : IMethodQuery, IConstructorQuery
#pragma warning restore 618
    {
        /// <summary>
        /// Selects constructors for the supplied <see cref="FakeItEasy.Fake{T}"/> type.
        /// </summary>
        /// <param name="type">The fake type.</param>
        /// <returns>
        /// Constructors for <paramref name="type"/>.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method only returns constructors if <paramref name="type"/> is a
        /// <see cref="FakeItEasy.Fake{T}"/> type. If not, an empty sequence is returned.
        /// </para>
        /// <para>
        /// If the type is the type of a constructed <see cref="FakeItEasy.Fake{T}"/>, constructors are
        /// returned according to the generic type argument's constructors. If the type is an
        /// interface, the <see cref="FakeItEasy.Fake{T}()"/> default constructor is returned.
        /// </para>
        /// </remarks>
        public IEnumerable<IMethod> SelectConstructors(Type type)
        {
            return this.SelectMethods(type);
        }

        /// <summary>
        /// Selects constructors for the supplied <see cref="FakeItEasy.Fake{T}"/> type.
        /// </summary>
        /// <param name="type">The fake type.</param>
        /// <returns>
        /// Constructors for <paramref name="type"/>.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method only returns constructors if <paramref name="type"/> is a
        /// <see cref="FakeItEasy.Fake{T}"/> type. If not, an empty sequence is returned.
        /// </para>
        /// <para>
        /// If the type is the type of a constructed <see cref="FakeItEasy.Fake{T}"/>, constructors are
        /// returned according to the generic type argument's constructors. If the type is an
        /// interface, the <see cref="FakeItEasy.Fake{T}()"/> default constructor is returned.
        /// </para>
        /// </remarks>
        public IEnumerable<IMethod> SelectMethods(Type type)
        {
            if (!type.IsFake())
            {
                return Enumerable.Empty<IMethod>();
            }

            var fakeType = type.GetFakedType();
            if (fakeType.IsInterface)
            {
                return new[] { new ConstructorMethod(type.GetDefaultConstructor()) };
            }

            return from ci in fakeType.GetPublicAndProtectedConstructors()
                   let paramInfos = ci.GetParameters()
                   orderby paramInfos.Length ascending
                   select new FakeItEasyConstructorMethod(type.GetConstructor(Type.EmptyTypes), paramInfos) as IMethod;
        }
    }
}
