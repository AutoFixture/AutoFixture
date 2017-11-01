using System;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Creates new <see cref="Uri"/> instances.
    /// </summary>
    public class UriGenerator : ISpecimenBuilder
    {
        /// <summary>
        /// Creates a new specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// The requested specimen if possible; otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (!typeof(Uri).Equals(request))
            {
                return new NoSpecimen();
            }

            var scheme = context.Resolve(typeof(UriScheme)) as UriScheme;
            if (scheme == null)
            {
                return new NoSpecimen();
            }

            var authority = context.Resolve(typeof(string)) as string;
            if (authority == null)
            {
                return new NoSpecimen();
            }

            return MakeUri(scheme, authority);
        }

        private static Uri MakeUri(UriScheme scheme, string authority)
        {
            return new Uri(scheme + "://" + authority);
        }
    }
}
