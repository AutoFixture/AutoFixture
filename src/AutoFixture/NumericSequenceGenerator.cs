using System;
using System.Threading;
using AutoFixture.Kernel;

namespace AutoFixture;

/// <summary>
/// Creates a sequence of consecutive numbers, starting at 1.
/// </summary>
public class NumericSequenceGenerator : ISpecimenBuilder
{
    private long _value;

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
            return NoSpecimen.Instance;

        return CreateNumericSpecimen(type);
    }

    private object CreateNumericSpecimen(Type request)
    {
        var typeCode = Type.GetTypeCode(request);

        switch (typeCode)
        {
            case TypeCode.Byte:
                return (byte)GetNextNumber();
            case TypeCode.Decimal:
                return (decimal)GetNextNumber();
            case TypeCode.Double:
                return (double)GetNextNumber();
            case TypeCode.Int16:
                return (short)GetNextNumber();
            case TypeCode.Int32:
                return (int)GetNextNumber();
            case TypeCode.Int64:
                return GetNextNumber();
            case TypeCode.SByte:
                return (sbyte)GetNextNumber();
            case TypeCode.Single:
                return (float)GetNextNumber();
            case TypeCode.UInt16:
                return (ushort)GetNextNumber();
            case TypeCode.UInt32:
                return (uint)GetNextNumber();
            case TypeCode.UInt64:
                return (ulong)GetNextNumber();
            default:
                return NoSpecimen.Instance;
        }
    }

    private long GetNextNumber()
    {
        return Interlocked.Increment(ref _value);
    }
}