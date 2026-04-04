using System;
using System.Collections.Concurrent;
using System.Globalization;
using AutoFixture.Kernel;

namespace AutoFixture;

/// <summary>
/// Creates a random sequence for a given type within a given range without repeating in the range until
/// all values are exhausted. Once exhausted, will automatically reset the set and continue choosing randomly
/// within the range.  Multiple requests (whether the same or different object) for the same
/// operand type, minimum, and maximum are treated as being drawn from the same set.
/// </summary>
public class RandomRangedNumberGenerator : ISpecimenBuilder
{
    private readonly ConcurrentDictionary<RangedNumberRequest, ISpecimenBuilder> _generatorMap;

    /// <summary>
    /// Initializes a new instance of the <see cref="RandomRangedNumberGenerator" /> class.
    /// </summary>
    public RandomRangedNumberGenerator()
    {
        _generatorMap = new ConcurrentDictionary<RangedNumberRequest, ISpecimenBuilder>();
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
        if (context == null) throw new ArgumentNullException(nameof(context));

        var rangedNumberRequest = request as RangedNumberRequest;
        if (rangedNumberRequest == null)
            return NoSpecimen.Instance;

        try
        {
            return SelectGenerator(rangedNumberRequest).Create(rangedNumberRequest.OperandType, context);
        }
        catch (ArgumentException)
        {
            return NoSpecimen.Instance;
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
        return _generatorMap.GetOrAdd(request, CreateRandomGenerator);
    }

    private static ISpecimenBuilder CreateRandomGenerator(RangedNumberRequest request)
    {
        var typeCode = Type.GetTypeCode(request.OperandType);
        if (!request.Minimum.GetType().IsNumberType() || !request.Maximum.GetType().IsNumberType())
        {
            throw new ArgumentException("Limit values should be of numeric type.", nameof(request));
        }

        if (request.Minimum.Equals(request.Maximum))
        {
            return new FixedBuilder(request.Minimum);
        }

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
                return new RandomNumericSequenceGenerator(
                    Convert.ToInt64(request.Minimum, CultureInfo.CurrentCulture),
                    Convert.ToInt64(request.Maximum, CultureInfo.CurrentCulture));

            case TypeCode.Single:
            case TypeCode.Double:
                return new FloatingPointRangedGenerator(request.Minimum, request.Maximum, typeCode);

            case TypeCode.UInt64:
            case TypeCode.Decimal:
                return new HighPrecisionRangedGenerator(request.Minimum, request.Maximum, typeCode);

            default:
                throw new ArgumentException(
                    string.Format(CultureInfo.InvariantCulture, "Request of '{0}' type is not supported.", typeCode),
                    nameof(request));
        }
    }

    private class FloatingPointRangedGenerator : ISpecimenBuilder
    {
        private const long RandomValueRange = int.MaxValue;

        private readonly double _factor;
        private readonly double _minimum;
        private readonly double _maximum;
        private readonly TypeCode _resultTypeCode;
        private readonly RandomNumericSequenceGenerator _generator;

        public FloatingPointRangedGenerator(object minimum, object maximum, TypeCode resultTypeCode)
        {
            _minimum = Convert.ToDouble(minimum, CultureInfo.CurrentCulture);
            _maximum = Convert.ToDouble(maximum, CultureInfo.CurrentCulture);
            _resultTypeCode = resultTypeCode;
            _generator = new RandomNumericSequenceGenerator(0, RandomValueRange);

            // (max - min) could lead to overflow, so we divide each part on range individually.
            _factor = Math.Abs((_maximum / RandomValueRange) - (_minimum / RandomValueRange));
        }

        public object Create(object request, ISpecimenContext context)
        {
            var randomValue = _generator.Create(typeof(double), context);
            if (randomValue is NoSpecimen) return NoSpecimen.Instance;

            // Half offset is needed to avoid overflow - full offset might be larger than double range.
            double halfOffset = (_factor / 2) * (double)randomValue;
            double result = Math.Min(_minimum + halfOffset + halfOffset, _maximum);

            return Convert.ChangeType(result, _resultTypeCode, CultureInfo.CurrentCulture);
        }
    }

    private class HighPrecisionRangedGenerator : ISpecimenBuilder
    {
        private const long RandomValueRange = int.MaxValue;

        private readonly decimal _factor;
        private readonly decimal _minimum;
        private readonly decimal _maximum;
        private readonly TypeCode _resultTypeCode;
        private readonly RandomNumericSequenceGenerator _generator;

        public HighPrecisionRangedGenerator(object minimum, object maximum, TypeCode resultTypeCode)
        {
            _minimum = Convert.ToDecimal(minimum, CultureInfo.CurrentCulture);
            _maximum = Convert.ToDecimal(maximum, CultureInfo.CurrentCulture);
            _resultTypeCode = resultTypeCode;
            _generator = new RandomNumericSequenceGenerator(0, RandomValueRange);

            // (max - min) could lead to overflow, so we divide each part on range individually.
            _factor = Math.Abs((_maximum / RandomValueRange) - (_minimum / RandomValueRange));
        }

        public object Create(object request, ISpecimenContext context)
        {
            var randomValue = _generator.Create(typeof(decimal), context);
            if (randomValue is NoSpecimen) return NoSpecimen.Instance;

            // Half offset is needed to avoid overflow - full offset might be larger than decimal range.
            var halfOffset = (_factor / 2) * (decimal)randomValue;

            decimal result = Math.Min(_minimum + halfOffset + halfOffset, _maximum);
            return Convert.ChangeType(result, _resultTypeCode, CultureInfo.CurrentCulture);
        }
    }
}