using System;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoNSubstitute
{
    public class NSubstituteBuilder : ISpecimenBuilder
    {
        private readonly ISpecimenBuilder builder;

        /// <summary>Initializes a new instance of the <see cref="NSubstituteBuilder"/> class with an
        ///     <see cref="ISpecimenBuilder"/> to decorate.</summary>
        /// <param name="builder">The builder which must build mock instances.</param>
        /// <remarks>
        ///     <para>
        ///         <paramref name="builder"/> is subsequently available through the <see cref="Builder"/>
        ///         property.
        ///     </para>
        /// </remarks>
        /// <seealso cref="Builder"/>
        public NSubstituteBuilder(ISpecimenBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException("builder");

            this.builder = builder;
        }

        /// <summary>Gets the decorated builder supplied through the constructor.</summary>
        /// <seealso cref="NSubstituteBuilder(ISpecimenBuilder)"/>
        public ISpecimenBuilder Builder
        {
            get { return builder; }
        }

        /// <summary>Creates a new specimen based on a request.</summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>The requested specimen if possible; otherwise a <see cref="NoSpecimen"/> instance.</returns>
        /// <remarks>
        ///     If the request is not a <see cref="Type"/> or the request does not represent an interface or an abstract class, this method returns a new
        ///     <see cref="NoSpecimen"/>; otherwise, it returns a substitute for the requested type.
        /// </remarks>
        public object Create(object request, ISpecimenContext context)
        {
            var type = request as Type;
            if (type == null || (!type.IsInterface && !type.IsAbstract))
                return new NoSpecimen(request);

            var substitute = Builder.Create(request, context);
            if (substitute == null)
                return new NoSpecimen(request);

            return substitute;
        }
    }
}
