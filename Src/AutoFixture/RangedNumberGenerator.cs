using System;
using System.Linq;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    public class RangedNumberGenerator : ISpecimenBuilder
    {
        private readonly object syncRoot;
        private IComparable o;

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

            var range = request as RangedNumberRequest;
            if (range == null)
            {
                return new NoSpecimen(request);
            }

            var value = context.Resolve(request) as IComparable;
            if (value == null)
            {
                return new NoSpecimen(request);
            }

            return this.CreateAnonymous(range, value);
        }

        /// <summary>
        /// Creates an anonymous number within a range specified by a RangeNumberRequest.
        /// </summary>
        /// <param name="range">The request.</param>
        /// <param name="value">The context value.</param>
        /// <returns>
        /// The next number in a consequtive sequence within a range specified by a RangeNumberRequest.
        /// </returns>
        private object CreateAnonymous(RangedNumberRequest range, IComparable value)
        {
            lock (this.syncRoot)
            {
                var minimum = (IComparable)range.Minimum;
                if (minimum.CompareTo(value) == 0)
                {
                    return minimum;
                }

                var maximum = (IComparable)range.Maximum;
                if (maximum.CompareTo(value) == 0)
                {
                    return maximum;
                }

                if (minimum.CompareTo(value) <= 0 &&
                    maximum.CompareTo(value) <= 0)
                {
                    return minimum;
                }

                Type elementType = minimum.GetType();

                Array array = Array.CreateInstance(elementType, 2);
                array.SetValue(minimum, 0);
                array.SetValue(value, 1);

                switch (Type.GetTypeCode(elementType))
                {
                    case TypeCode.Int32:
                        return array.Cast<object>().Select(Convert.ToInt32).Sum();

                    case TypeCode.Double:
                        return array.Cast<object>().Select(Convert.ToDouble).Sum();

                    case TypeCode.Int64:
                        return array.Cast<object>().Select(Convert.ToInt64).Sum();

                    case TypeCode.Decimal:
                        return array.Cast<object>().Select(Convert.ToDecimal).Sum();
                }

                return new NoSpecimen(range);
            }
        }
    }
}
