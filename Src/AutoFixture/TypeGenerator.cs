using System;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Generates Type instances.
    /// </summary>
    public class TypeGenerator : ISpecimenBuilder
    {
        /// <summary>
        /// Creates a new specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// The <see cref="object"/> <see cref="Type"/> if <paramref name="request"/> is a Type
        /// Type; otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (typeof(Type).Equals(request))
            {
                return typeof(object);
            }

            return new NoSpecimen();
        }
    }
}
