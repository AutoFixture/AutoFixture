using System;
using Moq;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoMoq
{
    /// <summary>
    /// Relays a request for an interface or an abstract class to a request for a
    /// <see cref="Mock{T}"/> of that class.
    /// </summary>
    public class MockRelay : ISpecimenBuilder
    {
        private readonly IRequestSpecification mockableSpecification;

        /// <summary>
        /// Initializes a new instance of the <see cref="MockRelay"/> class.
        /// </summary>
        public MockRelay()
            : this(new IsMockableSpecification())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MockRelay"/> class with a specification
        /// that determines whether a type should be mocked.
        /// </summary>
        /// <param name="mockableSpecification">
        /// A specification that determines whether a type should be mocked or not.
        /// </param>
        public MockRelay(IRequestSpecification mockableSpecification)
        {
            if (mockableSpecification == null)
            {
                throw new ArgumentNullException("mockableSpecification");
            }

            this.mockableSpecification = mockableSpecification;
        }

        /// <summary>
        /// Gets a specification that determines whether a given request should
        /// be mocked.
        /// </summary>
        /// <value>The specification.</value>
        /// <remarks>
        /// <para>
        /// This specification determines whether a given type should be
        /// relayed as a request for a mock of the same type. By default it
        /// only returns <see langword="true"/> for interfaces and abstract
        /// classes, but a different specification can be supplied by using the
        /// overloaded constructor that takes an
        /// <see cref="IRequestSpecification" /> as input. In that case, this
        /// property returns the specification supplied to the constructor.
        /// </para>
        /// </remarks>
        /// <seealso cref="MockRelay(IRequestSpecification)" />
        public IRequestSpecification MockableSpecification
        {
            get { return this.mockableSpecification; }
        }

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
                throw new ArgumentNullException("context");

            if (!this.mockableSpecification.IsSatisfiedBy(request))
                return new NoSpecimen(request);

            var t = request as Type;
            if (t == null)
                return new NoSpecimen(request);

            var m = MockRelay.ResolveMock(t, context);
            if (m == null)
                return new NoSpecimen(request);

            return m.Object;
        }

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

        private class IsMockableSpecification : IRequestSpecification
        {
            public bool IsSatisfiedBy(object request)
            {
                var t = request as Type;
                if (t == null)
                    return false;

                return (t != null)
                    && ((t.IsAbstract) || (t.IsInterface));
            }
        }
    }
}
