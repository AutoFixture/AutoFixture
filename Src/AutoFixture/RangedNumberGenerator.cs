using System;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates a sequence of ranged numbers, starting at range minimum.
    /// </summary>
    public class RangedNumberGenerator : ISpecimenBuilder
    {
        private readonly object syncRoot;
        private object rangedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="RangedNumberGenerator"/> class.
        /// </summary>
        public RangedNumberGenerator()
        {
            this.syncRoot = new object();
            this.rangedValue = null;
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

            var value = context.Resolve(range.OperandType) as IComparable;
            if (value == null)
            {
                return new NoSpecimen(request);
            }

            try
            {
                this.CreateAnonymous(range, value);
            }
            catch (InvalidOperationException)
            {
                return new NoSpecimen(request);
            }

            return this.rangedValue;
        }

        private void CreateAnonymous(RangedNumberRequest range, IComparable value)
        {
            lock (this.syncRoot)
            {
                var minimum = (IComparable)range.Minimum;
                var maximum = (IComparable)range.Maximum;

                if (this.rangedValue != null && (minimum.CompareTo(this.rangedValue) <= 0 && maximum.CompareTo(this.rangedValue) > 0))
                {
                    this.rangedValue = RangedNumberGenerator.Add(this.rangedValue, 1);
                }
                else if (minimum.CompareTo(value) == 0)
                {
                    this.rangedValue = minimum;
                }
                else if (maximum.CompareTo(value) == 0)
                {
                    this.rangedValue = maximum;
                }
                else if (minimum.CompareTo(value) <= 0 && maximum.CompareTo(value) <= 0)
                {
                    this.rangedValue = minimum;
                }
                else
                {
                    this.rangedValue = RangedNumberGenerator.Add(minimum, value);
                }
            }
        }

        private static object Add(object a, object b)
        {
            switch (Type.GetTypeCode(a.GetType()))
            {
                case TypeCode.Int32:
                    return (int)a + (int)b;

                case TypeCode.Int64:
                    return (long)a + (long)b;

                case TypeCode.Decimal:
                    return (decimal)a + (decimal)b;

                case TypeCode.Double:
                    return (double)a + (double)b;

                default:
                    throw new InvalidOperationException("The underlying type code of the specified types is not supported.");
            }
        }
    }
}
