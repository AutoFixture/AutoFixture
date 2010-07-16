using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;
using Moq;

namespace Ploeh.AutoFixture.AutoMoq
{
    /// <summary>
    /// Relays a request for an interface or an abstract class to a request for a
    /// <see cref="Mock{T}"/> of that class.
    /// </summary>
    public class MockRelay : ISpecimenBuilder
    {
        #region ISpecimenBuilder Members

        /// <summary>
        /// Creates a new specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// A dynamic mock instance of the requested interface or abstract class if possible;
        /// otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
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
