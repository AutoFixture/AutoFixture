using System;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates random value <see langword="true"/> or <see langword="false"/>.
    /// </summary>
    public class RandomBooleanSequenceGenerator : ISpecimenBuilder
    {
        /// <summary>
        /// Returns <see langword="true"/> or <see langword="false"/> randomly.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">Not used.</param>
        /// <returns>
        /// <see langword="true"/> or <see langword="false"/> generated randomly using <see cref="Random"/>, 
        /// if <paramref name="request"/> is a request for a boolean; otherwise, a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (!typeof(bool).Equals(request))
            {
                return new NoSpecimen(request);
            }

            return GenerateBoolean();
        }
        
        private static bool GenerateBoolean()
        {
            var r = new Random();
            return r.Next(0, 2) == 0;
        }
    }
}