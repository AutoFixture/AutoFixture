using System;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Issues <see cref="OmitSpecimen" /> instances if its encapsulated
    /// <see cref="IRequestSpecification" /> is satisfied.
    /// </summary>
    public class Omitter : ISpecimenBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Omitter" /> class.
        /// </summary>
        public Omitter()
            : this(new TrueRequestSpecification())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Omitter" /> class with
        /// the supplied <see cref="IRequestSpecification" />.
        /// </summary>
        /// <param name="specification">
        /// A specification used to control whether or not an
        /// <see cref="OmitSpecimen" /> instance should be issued.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// specification.
        /// </exception>
        /// <seealso cref="Specification" />
        public Omitter(IRequestSpecification specification)
        {
            this.Specification = specification ?? throw new ArgumentNullException(nameof(specification));
        }

        /// <summary>
        /// Creates a new specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// An <see cref="OmitSpecimen"/> instance if
        /// <see cref="Specification" /> allows it; otherwise a
        /// <see cref="NoSpecimen" /> instance.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">request.</exception>
        /// <remarks>
        ///   <para>
        /// The <paramref name="request" /> can be any object, but will often be a
        ///   <see cref="Type" /> or other <see cref="System.Reflection.MemberInfo" /> instances.
        ///   </para>
        /// </remarks>
        public object Create(object request, ISpecimenContext context)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            if (this.Specification.IsSatisfiedBy(request))
                return new OmitSpecimen();

            return new NoSpecimen();
        }

        /// <summary>
        /// Gets the specification used to control whether an
        /// <see cref="OmitSpecimen" /> instance should be issued upon request.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If the constructor overload that takes an
        /// <see cref="IRequestSpecification" /> as input was used, this
        /// property returns that instance.
        /// </para>
        /// </remarks>
        /// <seealso cref="Create" />
        /// <seealso cref="Omitter(IRequestSpecification)" />
        public IRequestSpecification Specification { get; }
    }
}
