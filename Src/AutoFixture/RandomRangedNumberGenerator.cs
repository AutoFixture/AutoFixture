using System;
using System.Collections.Concurrent;
using System.Globalization;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates a random sequence for a given type within a given range without repeating in the range until 
    /// all values are exhausted. Once exhausted, will automatically reset the set and continue choosing randomly 
    /// within the range.  Multiple requests (whether the same or different object) for the same
    /// operand type, minimum, and maximum are treated as being drawn from the same set. 
    /// </summary>
    public class RandomRangedNumberGenerator : ISpecimenBuilder
    {
        private readonly ConcurrentDictionary<RangedNumberRequest, ISpecimenBuilder> generatorMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomRangedNumberGenerator" /> class       
        /// </summary>
        public RandomRangedNumberGenerator()
        {
            this.generatorMap = new ConcurrentDictionary<RangedNumberRequest, ISpecimenBuilder>();
        }

        /// <summary>
        /// Creates a random number within the request range.
        /// </summary>
        /// <param name="request">The request that describes what to create. Other requests for same type and limits 
        /// denote the same set. </param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// The next random number in a range <paramref name="request"/> is a request
        /// for a numeric value; otherwise, a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (request == null)
                return new NoSpecimen();

            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var rangedNumberRequest = request as RangedNumberRequest;

            if (rangedNumberRequest == null)
                return new NoSpecimen();

            try
            {
                return SelectGenerator(rangedNumberRequest).Create(rangedNumberRequest.OperandType, context);
            }
            catch (ArgumentException)
            {
                return new NoSpecimen();
            }
        }

        /// <summary>
        /// Choose the RandomNumericSequenceGenerator to fulfill the request.  Will add the request as a new key
        /// to generatorMap if the request does not already have a generator for it.  Throws ArgumentException
        /// if either of the limits in the request are non-numeric. 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private ISpecimenBuilder SelectGenerator(RangedNumberRequest request)
        {
            return this.generatorMap.GetOrAdd(request, CreateRandomGenerator);
        }


        /// <summary>
        /// Converts provided minimum and maximum into a long array of size 2.  Throws ArgumentException
        /// if either value is non-numeric or otherwise fails conversion. 
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <returns></returns>
        private static long[] ConvertLimits(object minimum, object maximum)
        {
            return new long[] {ConvertLimit(minimum), ConvertLimit(maximum)};
        }

        /// <summary>
        /// Converts the provided limit into an Int64.  Throws ArgumentException if limit is a non-numeric type.
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        private static long ConvertLimit(object limit)
        {
            switch (Convert.GetTypeCode(limit))
            {
                case TypeCode.Byte:                   
                        return (long)(byte)limit;

                case TypeCode.Decimal:
                        return (long)(decimal)limit;

                case TypeCode.Double:
                        return (long)(double)limit;

                case TypeCode.Int16:
                        return (long)(short)limit;

                case TypeCode.Int32:
                        return (long)(int)limit;

                case TypeCode.Int64:
                        return (long)limit;

                case TypeCode.SByte:
                        return (long)(sbyte)limit;

                case TypeCode.Single:
                        return (long)(float)limit;

                case TypeCode.UInt16:
                        return (long)(ushort)limit;

                case TypeCode.UInt32:
                        return (long)(uint)limit;

                case TypeCode.UInt64:
                        return (long)(ulong)limit;
            }

            throw new ArgumentException("Limit parameter is non-numeric ", nameof(limit));
        }

        private static ISpecimenBuilder CreateRandomGenerator(RangedNumberRequest request)
        {
            var typeCode = Type.GetTypeCode(request.OperandType);

            switch (typeCode)
            {
                // Can be safely converted to long without overflow.
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                    var convertedLimits = ConvertLimits(request.Minimum, request.Maximum);
                    return new RandomNumericSequenceGenerator(convertedLimits);

                case TypeCode.Single:
                case TypeCode.Double:
                    return new FloatingPointRangedGenerator(request.Minimum, request.Maximum, typeCode);

                case TypeCode.UInt64:
                case TypeCode.Decimal:
                    return new HighPrecisionRangedGenerator(request.Minimum, request.Maximum, typeCode);

                default:
                    throw new ArgumentException($"Request of '{typeCode}' type is not supported.", nameof(request));
            }
        }


        private class FloatingPointRangedGenerator : ISpecimenBuilder
        {
            private const long RandomValueRange = int.MaxValue;

            private readonly double factor;
            private readonly double minimum;
            private readonly double maximum;
            private readonly TypeCode resultTypeCode;
            private readonly RandomNumericSequenceGenerator generator;

            public FloatingPointRangedGenerator(object minimum, object maximum, TypeCode resultTypeCode)
            {
                this.minimum = Convert.ToDouble(minimum, CultureInfo.CurrentCulture);
                this.maximum = Convert.ToDouble(maximum, CultureInfo.CurrentCulture);
                this.resultTypeCode = resultTypeCode;
                this.generator = new RandomNumericSequenceGenerator(0, RandomValueRange);

                // (max - min) could lead to overflow, so we divide each part on range individually.
                this.factor = Math.Abs(this.maximum / RandomValueRange - this.minimum / RandomValueRange);
            }

            public object Create(object request, ISpecimenContext context)
            {
                var randomValue = this.generator.Create(typeof(double), context);
                if (randomValue is NoSpecimen) return new NoSpecimen();

                // Half offset is needed to avoid overflow - full offset might be larger than double range.
                double halfOffset = (this.factor / 2) * (double) randomValue;
                double result = Math.Min(this.minimum + halfOffset + halfOffset, this.maximum);

                return Convert.ChangeType(result, this.resultTypeCode, CultureInfo.CurrentCulture);
            }
        }

        private class HighPrecisionRangedGenerator : ISpecimenBuilder
        {
            private const long RandomValueRange = int.MaxValue;

            private readonly decimal factor;
            private readonly decimal minimum;
            private readonly decimal maximum;
            private readonly TypeCode resultTypeCode;
            private readonly RandomNumericSequenceGenerator generator;

            public HighPrecisionRangedGenerator(object minimum, object maximum, TypeCode resultTypeCode)
            {
                this.minimum = Convert.ToDecimal(minimum, CultureInfo.CurrentCulture);
                this.maximum = Convert.ToDecimal(maximum, CultureInfo.CurrentCulture);
                this.resultTypeCode = resultTypeCode;
                this.generator = new RandomNumericSequenceGenerator(0, RandomValueRange);

                // (max - min) could lead to overflow, so we divide each part on range individually.
                this.factor = Math.Abs(this.maximum / RandomValueRange - this.minimum / RandomValueRange);
            }

            public object Create(object request, ISpecimenContext context)
            {
                var randomValue = this.generator.Create(typeof(decimal), context);
                if (randomValue is NoSpecimen) return new NoSpecimen();

                // Half offset is needed to avoid overflow - full offset might be larger than decimal range.
                var halfOffset = (this.factor / 2) * (decimal) randomValue;

                decimal result = Math.Min(this.minimum + halfOffset + halfOffset, this.maximum);
                return Convert.ChangeType(result, this.resultTypeCode, CultureInfo.CurrentCulture);
            }
        }
    }
}