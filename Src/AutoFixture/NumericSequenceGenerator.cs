using System;
using System.Threading;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Creates a sequence of consecutive numbers, starting at 1.
    /// </summary>
    public class NumericSequenceGenerator : ISpecimenBuilder
    {
        private long value = 0;

        /// <summary>
        /// Creates an anonymous number.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">Not used.</param>
        /// <returns>
        /// The next number in a consecutive sequence, if <paramref name="request"/> is a request
        /// for a numeric value; otherwise, a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            var type = request as Type;
            if (type == null)
                return new NoSpecimen();

            return this.CreateNumericSpecimen(type);
        }

        private object CreateNumericSpecimen(Type request)
        {
            var typeCode = Type.GetTypeCode(request);

            switch (typeCode)
            {
                case TypeCode.Byte:
                    return (byte)this.GetNextNumber();
                case TypeCode.Decimal:
                    return (decimal)this.GetNextNumber();
                case TypeCode.Double:
                    return (double)this.GetNextNumber();
                case TypeCode.Int16:
                    return (short)this.GetNextNumber();
                case TypeCode.Int32:
                    return (int)this.GetNextNumber();
                case TypeCode.Int64:
                    return this.GetNextNumber();
                case TypeCode.SByte:
                    return (sbyte)this.GetNextNumber();
                case TypeCode.Single:
                    return (float)this.GetNextNumber();
                case TypeCode.UInt16:
                    return (ushort)this.GetNextNumber();
                case TypeCode.UInt32:
                    return (uint)this.GetNextNumber();
                case TypeCode.UInt64:
                    return (ulong)this.GetNextNumber();
                default:
                    return new NoSpecimen();
            }
        }

        private long GetNextNumber()
        {
            return Interlocked.Increment(ref this.value);
        }
    }
}
