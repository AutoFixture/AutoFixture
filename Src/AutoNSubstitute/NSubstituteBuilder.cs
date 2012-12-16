using System;
using NSubstitute;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoNSubstitute
{
    public class NSubstituteBuilder : ISpecimenBuilder
    {
        private readonly ISpecimenBuilder builder;
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
            throw new NotImplementedException();
        }
    }
}
