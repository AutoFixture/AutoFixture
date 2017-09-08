using System;
using Ploeh.AutoFixture.Kernel;
using Rhino.Mocks.Interfaces;

namespace Ploeh.AutoFixture.AutoRhinoMock
{
    /// <summary>
    /// Provides pre- and post-condition checks for requests for mock instances.
    /// </summary>
    /// <seealso cref="Create(object, ISpecimenContext)" />
    public class RhinoMockAroundAdvice : ISpecimenBuilder
    {
        private readonly ISpecimenBuilder builder;

        /// <summary>
        /// Initializes a new instance of the <see cref="RhinoMockAroundAdvice"/> class with an
        /// <see cref="ISpecimenBuilder" /> to decorate.
        /// </summary>
        /// <param name="builder">The builder which must build mock instances.</param>
        /// <remarks>
        /// <para>
        /// <paramref name="builder" /> is subsequently available through the <see cref="Builder"/>
        /// property.
        /// </para>
        /// </remarks>
        /// <seealso cref="Builder" />
        public RhinoMockAroundAdvice(ISpecimenBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }

            this.builder = builder;
        }

        /// <summary>
        /// Gets the decorated builder supplied through the constructor.
        /// </summary>
        /// <seealso cref="RhinoMockAroundAdvice(ISpecimenBuilder)" />
        public ISpecimenBuilder Builder
        {
            get { return this.builder; }
        }

        /// <summary>
        /// Creates a new specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// A mock instance created by Rhino Mocks if appropriate; otherwise a
        /// <see cref="NoSpecimen"/> instance.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The Create method checks whether a request is for an interface or abstract class. If so
        /// it delegates the call to the decorated <see cref="Builder"/>. When the specimen is
        /// returned from the decorated <see cref="ISpecimenBuilder"/> the method checks whether
        /// the returned instance is, indeed, a Rhino Mocks instance.
        /// </para>
        /// <para>
        /// If all pre- and post-conditions are satisfied, the mock instance is returned; otherwise
        /// a <see cref="NoSpecimen" /> instance.
        /// </para>
        /// </remarks>
        public object Create(object request, ISpecimenContext context)
        {
            if (!request.IsMockable())
            {
                return new NoSpecimen();
            }

            var built = this.builder.Create(request, context);
            var m = built as IMockedObject;
            if (m == null)
            {
                return new NoSpecimen();
            }

            return m;
        }
    }
}
