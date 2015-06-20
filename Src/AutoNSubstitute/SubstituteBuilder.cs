using System;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoNSubstitute
{
    /// <summary>
    /// Creates a substitute in response to the <see cref="SubstituteRequest"/>.
    /// </summary>
    /// <remarks>
    /// This class serves as a fixture customization, responding to explicit substitute requests in the form of the
    /// <see cref="SubstituteRequest"/> instances. The actual construction of the substitute is delegated to a 
    /// substitute constructor, responsible for invoking an appropriate constructor for the target type with
    /// automatically generated arguments.
    /// </remarks>
    public class SubstituteBuilder : ISpecimenBuilder
    {
        private readonly ISpecimenBuilder substituteConstructor;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubstituteBuilder"/> class with the specified 
        /// <paramref name="substituteConstructor"/>.
        /// </summary>
        /// <param name="substituteConstructor">
        /// An <see cref="ISpecimenBuilder"/> responsible for creating a substitute instance from a requested 
        /// <see cref="Type"/>.
        /// </param>
        public SubstituteBuilder(ISpecimenBuilder substituteConstructor)
        {
            if (substituteConstructor == null)
            {
                throw new ArgumentNullException("substituteConstructor");
            }

            this.substituteConstructor = substituteConstructor;
        }

        /// <summary>
        /// Returns an <see cref="ISpecimenBuilder"/> responsible for creating a substitute instance based on a target
        /// <see cref="Type"/> type.
        /// </summary>
        public ISpecimenBuilder SubstituteConstructor 
        {
            get { return this.substituteConstructor; }
        }

        /// <summary>
        /// Creates a substitute when <paramref name="request"/> is an explicit <see cref="SubstituteRequest"/>.
        /// </summary>
        /// <returns>
        /// A substitute created by the substitute constructor when <paramref name="request"/> is a 
        /// <see cref="SubstituteRequest"/> or <see cref="NoSpecimen"/> for all other requests.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            var substituteRequest = request as SubstituteRequest;
            if (substituteRequest == null)
            {
                return new NoSpecimen(request);
            }

            return this.substituteConstructor.Create(substituteRequest.TargetType, context);
        }
    }
}
