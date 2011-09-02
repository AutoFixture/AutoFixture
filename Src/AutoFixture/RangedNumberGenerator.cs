using System;
using System.Linq;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates a sequence of ranged numbers, starting at range minimum.
    /// </summary>
    public class RangedNumberGenerator : ISpecimenBuilder
    {
        private readonly object syncRoot;
        private object numberic;

        /// <summary>
        /// Initializes a new instance of the <see cref="RangedNumberGenerator"/> class.
        /// </summary>
        public RangedNumberGenerator()
        {
            this.syncRoot = new object();
            this.numberic = null;
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

            this.numberic = this.CreateAnonymous(range, value);

            return this.numberic;
        }

        private object CreateAnonymous(RangedNumberRequest range, IComparable value)
        {
            lock (this.syncRoot)
            {
                object result;

                var minimum = (IComparable)range.Minimum;
                var maximum = (IComparable)range.Maximum;

                if (this.numberic != null && (minimum.CompareTo(this.numberic) <= 0 && maximum.CompareTo(this.numberic) > 0))
                {
                    RangedNumberGenerator.TryAdd(this.numberic, 1, out result);
                    return result;
                }

                if (minimum.CompareTo(value) == 0)
                {
                    return minimum;
                }

                if (maximum.CompareTo(value) == 0)
                {
                    return maximum;
                }

                if (minimum.CompareTo(value) <= 0 &&
                    maximum.CompareTo(value) <= 0)
                {
                    return minimum;
                }

                RangedNumberGenerator.TryAdd(minimum, value, out result);
                return result;
            }
        }

        private static bool TryAdd(object a, object b, out object result)
        {
            Type elementType = a.GetType();

            Array array = Array.CreateInstance(elementType, 2);
            array.SetValue(a, 0);
            array.SetValue(b, 1);

            result = null;

            switch (Type.GetTypeCode(elementType))
            {
                case TypeCode.Int32:
                    result = array.Cast<object>().Select(Convert.ToInt32).Sum();
                    break;

                case TypeCode.Double:
                    result = array.Cast<object>().Select(Convert.ToDouble).Sum();
                    break;

                case TypeCode.Int64:
                    result = array.Cast<object>().Select(Convert.ToInt64).Sum();
                    break;

                case TypeCode.Decimal:
                    result = array.Cast<object>().Select(Convert.ToDecimal).Sum();
                    break;
            }

            return result != null;
        }
    }
}
