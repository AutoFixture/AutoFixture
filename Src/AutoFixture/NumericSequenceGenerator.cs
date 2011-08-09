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

            if (!IsNumeric(requestedType))
            {
                return new NoSpecimen(request);
            }

            var specimen = CreateNumericSpecimen(requestedType);

            return specimen;
        }

        private bool IsNumeric(Type type)
        {
            var typeCode = Type.GetTypeCode(type);

            switch (typeCode)
            {
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
                default:
                    return false;
            }
        }

        private object CreateNumericSpecimen(Type request)
        {
            int baseSpecimen = Interlocked.Increment(ref this.baseValue);
            object specimen = Convert.ChangeType(baseSpecimen, request);

            return specimen;
        }
    }
}