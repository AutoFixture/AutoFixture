using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;
using Moq;

namespace Ploeh.AutoFixture.AutoMoq
{
    public class MockRelay : ISpecimenBuilder
    {
        #region ISpecimenBuilder Members

        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var t = request as Type;
            if (!MockRelay.ShouldBeMocked(t))
            {
                return new NoSpecimen(request);
            }

            var m = MockRelay.ResolveMock(t, context);
            if (m == null)
            {
                return new NoSpecimen(request);
            }

            return m.Object;
        }

        #endregion

        private static bool ShouldBeMocked(Type t)
        {
            return (t != null)
                && ((t.IsAbstract) || (t.IsInterface));
        }

        private static Mock ResolveMock(Type t, ISpecimenContext context)
        {
            var mockType = typeof(Mock<>).MakeGenericType(t);
            return context.Resolve(mockType) as Mock;
        }
    }
}
