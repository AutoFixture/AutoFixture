using System;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    public class RangedNumberGenerator : ISpecimenBuilder
    {
        private readonly object syncRoot;

        /// <summary>
        /// Initializes a new instance of the <see cref="RangedNumberGenerator"/> class.
        /// </summary>
        public RangedNumberGenerator()
        {
            this.syncRoot = new object();
        }

        /// <summary>
        /// Creates a new number based on a RangedNumberRequest.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// The requested number if possible; otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (request == null)
            {
                return new NoSpecimen();
            }

            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var rangedNumberRequest = request as RangedNumberRequest;
            if (rangedNumberRequest == null)
            {
                return new NoSpecimen(request);
            }

            var value = context.Resolve(request);
            if (value == null)
            {
                return new NoSpecimen(request);
            }

            return CreateAnonymous(rangedNumberRequest, value);
        }

        /// <summary>
        /// Creates an anonymous number within a range specified by a RangeNumberRequest.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="value">The context value.</param>
        /// <returns>
        /// The next number in a consequtive sequence within a range specified by a RangeNumberRequest.
        /// </returns>
        private object CreateAnonymous(RangedNumberRequest request, object value)
        {
            var minimum = (IComparable)request.Minimum;
            if (minimum.CompareTo(value) == 0)
            {
                return minimum;
            }

            var maximum = (IComparable)request.Maximum;
            if (maximum.CompareTo(value) == 0)
            {
                return maximum;
            }

            if (minimum.CompareTo(value) <= 0 &&
                maximum.CompareTo(value) <= 0)
            {
                return minimum;
            }

            return Add(minimum, value);
        }

        private object Add(object a, object b)
        {
            throw new NotImplementedException();
        }
    }
}
