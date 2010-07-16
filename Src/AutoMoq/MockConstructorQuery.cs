using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoMoq
{
    public class MockConstructorQuery : IConstructorQuery
    {
        #region IConstructorQuery Members

        public IEnumerable<ConstructorInfo> SelectConstructors(Type type)
        {
            if (!type.IsMock())
            {
                return Enumerable.Empty<ConstructorInfo>();
            }

            var mockType = type.GetMockedType();
            if (mockType.IsInterface)
            {
                return new[] { type.GetDefaultConstructor() };
            }

            return from ci in mockType.GetPublicAndProtectedConstructors()
                   select new MockConstructorInfo(type.GetParamsConstructor(), ci.GetParameters()) as ConstructorInfo;
        }

        #endregion
    }
}
