using System;
using System.Collections.Generic;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoNSubstitute
{
    /// <summary>
    /// Provides pre- and post-condition checks for requests for mock instances.
    /// </summary>
    /// <seealso cref="Create(object, ISpecimenContext)" />
    public class NSubstituteBuilder : ISpecimenBuilder
    {
        private readonly ISpecimenBuilder builder;

        /// <summary>
        /// Initializes a new instance of the <see cref="NSubstituteBuilder"/> class with an
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
        public NSubstituteBuilder(ISpecimenBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }

            this.builder = builder;
            this.substitutes = new Dictionary<object, Type>();

        }

        private readonly IDictionary<object, Type> substitutes;

        /// <summary>
        /// Gets the type of the supplied specimen
        /// </summary>
        /// <param name="specimen">The specimen for which the type should be returned.</param>
        /// <returns>The type of the passed specimen if it is not a mock. If it is a mock, the
        /// mocked type is returned</returns>
        public Type GetSpecimenType(object specimen)
        {
            var specimenType = specimen.GetType();

            return IsSubstitute(specimen) ? substitutes[specimen] : specimenType;
        }

        private bool IsSubstitute(object specimen)
        {
            return substitutes.ContainsKey(specimen);
        }

        /// <summary>
        /// Gets the decorated builder supplied through the constructor.
        /// </summary>
        /// <seealso cref="NSubstituteBuilder(Ploeh.AutoFixture.Kernel.ISpecimenBuilder)" />
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
        /// A mock instance created by NSubstitute if appropriate; otherwise a
        /// <see cref="NoSpecimen"/> instance.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The Create method checks whether a request is for an interface or abstract class. If so
        /// it delegates the call to the decorated <see cref="Builder"/>.
        /// </para>
        /// <para>
        /// If all pre- and post-conditions are satisfied, the mock instance is returned; otherwise
        /// a <see cref="NoSpecimen" /> instance.
        /// </para>
        /// </remarks>
        public object Create(object request, ISpecimenContext context)
        {
            var requestType = request as Type;
            if (requestType == null || !requestType.IsMockable())
            {
                return new NoSpecimen(request);
            }

            var specimen = this.builder.Create(request, context);
            substitutes.Add(specimen, requestType);
            return specimen;
        }
    }
}
