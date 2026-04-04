using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Kernel;

namespace AutoFixture;

/// <summary>
/// Creates a sequence of random, unique, numbers starting at 1.
/// </summary>
public class RandomNumericSequenceGenerator : ISpecimenBuilder
{
    private readonly long[] _limits;
    private readonly object _syncRoot;
    private readonly Random _random;
    private readonly HashSet<long> _numbers;
    private long _lower;
    private long _upper;
    private long _count;

    /// <summary>
    /// Initializes a new instance of the <see cref="RandomNumericSequenceGenerator" /> class
    /// with the default limits, 255, 32767, and 2147483647.
    /// </summary>
    public RandomNumericSequenceGenerator()
        : this(1, byte.MaxValue, short.MaxValue, int.MaxValue)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RandomNumericSequenceGenerator" /> class.
    /// </summary>
    /// <param name="limits">A sequence of at least two ascending numbers.</param>
    public RandomNumericSequenceGenerator(IEnumerable<long> limits)
        : this(limits.ToArray())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RandomNumericSequenceGenerator" /> class.
    /// </summary>
    /// <param name="limits">An array of at least two ascending numbers.</param>
    /// <exception cref="System.ArgumentNullException"></exception>
    /// <exception cref="System.ArgumentException"></exception>
    public RandomNumericSequenceGenerator(params long[] limits)
    {
        if (limits == null) throw new ArgumentNullException(nameof(limits));
        if (limits.Length < 2)
            throw new ArgumentException("Limits must be at least two ascending numbers.", nameof(limits));

        ValidateThatLimitsAreStrictlyAscending(limits);

        _limits = limits;
        _syncRoot = new object();
        _random = new Random();
        _numbers = new HashSet<long>();
        CreateRange();
    }

    /// <summary>
    /// Gets the sequence of limits.
    /// </summary>
    /// <value>
    /// The sequence of limits.
    /// </value>
    public IEnumerable<long> Limits
    {
        get { return _limits; }
    }

    /// <summary>
    /// Creates an anonymous number.
    /// </summary>
    /// <param name="request">The request that describes what to create.</param>
    /// <param name="context">A context that can be used to create other specimens.</param>
    /// <returns>
    /// The next random number in a sequence, if <paramref name="request"/> is a request
    /// for a numeric value; otherwise, a <see cref="NoSpecimen"/> instance.
    /// </returns>
    public object Create(object request, ISpecimenContext context)
    {
        var type = request as Type;
        if (type == null)
        {
            return NoSpecimen.Instance;
        }

        return CreateRandom(type);
    }

    private static void ValidateThatLimitsAreStrictlyAscending(long[] limits)
    {
        if (limits.Zip(limits.Skip(1), (a, b) => a >= b).Any(b => b))
        {
            throw new ArgumentOutOfRangeException(nameof(limits), "Limits must be ascending numbers.");
        }
    }

    private object CreateRandom(Type request)
    {
        switch (Type.GetTypeCode(request))
        {
            case TypeCode.Byte:
                return (byte)GetNextRandom();

            case TypeCode.Decimal:
                return (decimal)GetNextRandom();

            case TypeCode.Double:
                return (double)GetNextRandom();

            case TypeCode.Int16:
                return (short)GetNextRandom();

            case TypeCode.Int32:
                return (int)GetNextRandom();

            case TypeCode.Int64:
                return
                    GetNextRandom();

            case TypeCode.SByte:
                return (sbyte)GetNextRandom();

            case TypeCode.Single:
                return (float)GetNextRandom();

            case TypeCode.UInt16:
                return (ushort)GetNextRandom();

            case TypeCode.UInt32:
                return (uint)GetNextRandom();

            case TypeCode.UInt64:
                return (ulong)GetNextRandom();

            default:
                return NoSpecimen.Instance;
        }
    }

    private long GetNextRandom()
    {
        lock (_syncRoot)
        {
            EvaluateRange();

            long result;
            do
            {
                if (_lower >= int.MinValue &&
                    _upper <= int.MaxValue)
                {
                    result = _random.Next((int)_lower, (int)_upper);
                }
                else
                {
                    result = GetNextInt64InRange();
                }
            }
            while (_numbers.Contains(result));

            _numbers.Add(result);
            return result;
        }
    }

    private void EvaluateRange()
    {
        if (_count == (_upper - _lower))
        {
            _count = 0;
            CreateRange();
        }

        _count++;
    }

    private void CreateRange()
    {
        var remaining = _limits.Where(x => x > _upper - 1).ToArray();
        if (remaining.Any() && _numbers.Any())
        {
            _lower = _upper;
            _upper = remaining.Min() + 1;
        }
        else
        {
            _lower = _limits[0];
            _upper = GetUpperRangeFromLimits();
        }

        _numbers.Clear();
    }

    /// <summary>
    /// Returns upper limit + 1 when expecting to use upper as max value in Random.Next(Int32,Int32).
    /// This ensures that the upper limit is included in the possible values returned by Random.Next(Int32,Int32)
    ///
    /// When not expecting to use Random.Next(Int32,Int32).  It returns the original upper limit.
    /// </summary>
    /// <returns></returns>
    private long GetUpperRangeFromLimits()
    {
        return _limits[1] >= int.MaxValue
            ? _limits[1]
            : _limits[1] + 1;
    }

    private long GetNextInt64InRange()
    {
        var range = (ulong)(_upper - _lower);
        ulong limit = ulong.MaxValue - (ulong.MaxValue % range);
        ulong number;
        do
        {
            var buffer = new byte[sizeof(ulong)];
            _random.NextBytes(buffer);
            number = BitConverter.ToUInt64(buffer, 0);
        }
        while (number > limit);
        return (long)((number % range) + (ulong)_lower);
    }
}