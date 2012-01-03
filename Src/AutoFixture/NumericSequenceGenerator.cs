using System;
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
        /// <returns>
        /// The next number in a consequtive sequence, if <paramref name="request"/> is a request
        /// for a numeric value; otherwise, a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            var requestedType = request as Type;
            if (requestedType == null)
                return new NoSpecimen(request);

            return this.CreateNumericSpecimen(requestedType);
        }

        private object CreateNumericSpecimen(Type request)
        {
            var typeCode = Type.GetTypeCode(request);

            switch (typeCode)
            {
                case TypeCode.Byte:
                    return (byte)this.GetNextNumberInSequence();
                case TypeCode.Decimal:
                    return (decimal)this.GetNextNumberInSequence();
                case TypeCode.Double:
                    return (double)this.GetNextNumberInSequence();
                case TypeCode.Int16:
                    return (short)this.GetNextNumberInSequence();
                case TypeCode.Int32:
                    return this.GetNextNumberInSequence();
                case TypeCode.Int64:
                    return (long)this.GetNextNumberInSequence();
                case TypeCode.SByte:
                    return (sbyte)this.GetNextNumberInSequence();
                case TypeCode.Single:
                    return (float)this.GetNextNumberInSequence();
                case TypeCode.UInt16:
                    return (ushort)this.GetNextNumberInSequence();
                case TypeCode.UInt32:
                    return (uint)this.GetNextNumberInSequence();
                case TypeCode.UInt64:
                    return (ulong)this.GetNextNumberInSequence();
                default:
                    return new NoSpecimen(request);
            }
        }

        private int GetNextNumberInSequence()
        {
            return Interlocked.Increment(ref this.baseValue);
        }
    }
}