using System;
using System.Linq.Expressions;
using System.Threading;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates a sequence of consecutive numbers, starting at 1.
    /// </summary>
    public class NumericSequenceGenerator : ISpecimenBuilder
    {
        private int baseValue;

        /// <summary>
        /// Creates an anonymous number.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">Not used.</param>
        /// <exception cref="ArgumentNullException"><paramref name="context"/> is null.</exception>
        /// <returns>
        /// The next number in a consequtive sequence, if <paramref name="request"/> is a request
        /// for a numeric value; otherwise, a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var requestedType = request as Type;

            if (requestedType == null)
            {
                return new NoSpecimen(request);
            }

            var specimen = this.CreateNumericSpecimen(requestedType);

            return specimen;
        }

        private object CreateNumericSpecimen(Type request)
        {
            var typeCode = Type.GetTypeCode(request);

            switch (typeCode)
            {
                case TypeCode.Byte:
                    return GetNextNumberInSequenceAs<byte>();
                case TypeCode.Decimal:
                    return GetNextNumberInSequenceAs<decimal>();
                case TypeCode.Double:
                    return GetNextNumberInSequenceAs<double>();
                case TypeCode.Int16:
                    return GetNextNumberInSequenceAs<short>();
                case TypeCode.Int32:
                    return GetNextNumberInSequenceAs<int>();
                case TypeCode.Int64:
                    return GetNextNumberInSequenceAs<long>();
                case TypeCode.SByte:
                    return GetNextNumberInSequenceAs<sbyte>();
                case TypeCode.Single:
                    return GetNextNumberInSequenceAs<float>();
                case TypeCode.UInt16:
                    return GetNextNumberInSequenceAs<ushort>();
                case TypeCode.UInt32:
                    return GetNextNumberInSequenceAs<uint>();
                case TypeCode.UInt64:
                    return GetNextNumberInSequenceAs<ulong>();
                default:
                    return new NoSpecimen(request);
            }
        }

        private TRequest GetNextNumberInSequenceAs<TRequest>()
        {
            Interlocked.Increment(ref this.baseValue);

            var conversionMethod = Expression.Convert(Expression.Constant(this.baseValue), typeof(TRequest));
            var value = Expression.Lambda<Func<TRequest>>(conversionMethod).Compile()();

            return value;
        }
    }
}