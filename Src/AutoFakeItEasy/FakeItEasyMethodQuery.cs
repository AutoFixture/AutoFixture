using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoFakeItEasy
{
    /// <summary>
    /// Selects appropriate methods to create <see cref="FakeItEasy.Fake{T}"/> instances.
    /// </summary>
    public class FakeItEasyMethodQuery : IMethodQuery
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
        /// </remarks>
        public IEnumerable<IMethod> SelectMethods(Type type)
        {
            if (!type.IsFake())
            {
                return Enumerable.Empty<IMethod>();
            }

            var mockType = type.GetFakedType();
            if (mockType.IsInterface)
            {
                return new[] { new FakeItEasyMethod(type, new ParameterInfo[0]) };
            }

            return from ci in mockType.GetPublicAndProtectedConstructors()
                   let paramInfos = ci.GetParameters()
                   orderby paramInfos.Length ascending
                   select new FakeItEasyMethod(ci.DeclaringType, paramInfos) as IMethod;
        }
    }
}
