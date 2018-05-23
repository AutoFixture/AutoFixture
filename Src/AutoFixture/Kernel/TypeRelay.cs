using System;
using System.Reflection;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Maps a request for one <see cref="Type" /> to a request for another Type.
    /// </summary>
    public class TypeRelay : ISpecimenBuilder
    {
        private readonly IRequestSpecification fromSpecification;

        /// <summary>
        /// Gets the type which is relayed from.
        /// </summary>
        public Type From { get; }

        /// <summary>
        /// Gets the type which is relayed to.
        /// </summary>
        public Type To { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeRelay"/> class.
        /// </summary>
        /// <param name="from">
        /// The <see cref="Type" /> from which the TypeRelay instance should map.
        /// </param>
        /// <param name="to">
        /// The <see cref="Type" /> to which the TypeRelay instance should map.
        /// </param>
        /// <remarks>
        /// <para>
        /// The <paramref name="from" /> and <paramref name="to" /> parameters are used by the
        /// <see cref="Create" /> method to map a request for the <i>from</i> Type into a request
        /// for the <i>to</i> Type.
        /// </para>
        /// </remarks>
        /// <example>
        /// In this example, BaseType is an abstract base class, and
        /// DerivedType is a concrete class that derives from BaseType. The
        /// Fixture instance is configured to relay all requests for BasetType
        /// to requests for DerivedType, so the actual result return from the
        /// fixture when BasetType is requested is a DerivedType instance.
        /// <code>
        /// var fixture = new Fixture();
        /// fixture.Customizations.Add(
        ///     new TypeRelay(
        ///         typeof(BaseType),
        ///         typeof(DerivedType)));
        ///
        /// var actual = fixture.Create&lt;BaseType&gt;();
        /// </code>
        /// </example>
        public TypeRelay(Type from, Type to)
        {
            this.From = from ?? throw new ArgumentNullException(nameof(from));
            this.To = to ?? throw new ArgumentNullException(nameof(to));

            if (from.GetTypeInfo().IsGenericTypeDefinition ^ to.GetTypeInfo().IsGenericTypeDefinition)
            {
                throw new ArgumentException("Relaying from open generic type to open generic type " +
                                            "or from closed type to closed type are supported only.");
            }

            this.fromSpecification = new ExactTypeSpecification(from);
        }

        /// <summary>
        /// Relays a request for the <i>from</i> Type into a request for the <i>to</i> Type.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// If <paramref name="request" /> is a request for the <i>from</i> Type, an instance of
        /// the <i>to</i> Type is returned; otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The <i>from</i> and <i>to</i> Types are defined by constructor arguments. An exact
        /// match is performed when evaluating <paramref name="request" /> against the <i>from</i>
        /// Type - i.e. derived types are not regarded as matches.
        /// </para>
        /// <para>
        /// If the request matches the <i>from</i> Type, an instance of the <i>to</i> Type is
        /// requested from the <paramref name="context" />.
        /// </para>
        /// </remarks>
        /// <seealso cref="TypeRelay(Type, Type)" />
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (request is Type t && this.fromSpecification.IsSatisfiedBy(request))
                return context.Resolve(this.GetRedirectedTypeRequest(t));

            return new NoSpecimen();
        }

        private Type GetRedirectedTypeRequest(Type originalRequest)
        {
            if (!this.From.GetTypeInfo().IsGenericTypeDefinition)
                return this.To;

            var genericArguments = originalRequest.GetTypeInfo().GenericTypeArguments;
            return this.To.GetTypeInfo().MakeGenericType(genericArguments);
        }
    }
}
