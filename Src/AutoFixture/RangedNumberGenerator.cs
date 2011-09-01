using System;
using System.Linq;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    public class RangedNumberGenerator : ISpecimenBuilder
    {
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

            return Add(minimum, value) ?? new NoSpecimen(request);
        }

        private object Add(object a, object b)
        {
            Array array = Array.CreateInstance(a.GetType(), 2);
            array.SetValue(a, 0);
            array.SetValue(b, 1);

            switch (Type.GetTypeCode(a.GetType()))
            {
                case TypeCode.Int32:
                    return array.Cast<object>().Select(Convert.ToInt32).Sum();

                case TypeCode.Double:
                    return array.Cast<object>().Select(Convert.ToDouble).Sum();

                case TypeCode.Int64:
                    return array.Cast<object>().Select(Convert.ToInt64).Sum();
            }

            return null;
        }
    }
}
