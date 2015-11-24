using System;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates new <see cref="DomainName"/> instances.
    /// </summary>
    public class DomainNameGenerator : ISpecimenBuilder
    {
        private readonly string[] fictitiousDomains =
        {
            "example.com",
            "example.net",
            "example.org"
        };

        private readonly Random random = new Random();

        /// <summary>
        /// Creates a new specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// The requested specimen if possible; otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (request == null || !typeof(DomainName).Equals(request))
            {
                return new NoSpecimen(request);
            }

            var index = random.Next(0, fictitiousDomains.Length);
            return new DomainName(fictitiousDomains[index]);
        }
    }
}