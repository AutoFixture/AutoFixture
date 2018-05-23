using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Creates a sequence of random, unique, numbers starting at 1.
    /// </summary>
    public class RandomNumericSequenceGenerator : ISpecimenBuilder
    {
        private readonly long[] limits;
        private readonly object syncRoot;
        private readonly Random random;
        private readonly HashSet<long> numbers;
        private long lower;
        private long upper;
        private long count;

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

            this.limits = limits;
            this.syncRoot = new object();
            this.random = new Random();
            this.numbers = new HashSet<long>();
            this.CreateRange();
        }

        /// <summary>
        /// Gets the sequence of limits.
        /// </summary>
        /// <value>
        /// The sequence of limits.
        /// </value>
        public IEnumerable<long> Limits
        {
            get { return this.limits; }
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
                return new NoSpecimen();
            }

            return this.CreateRandom(type);
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
                    return (byte)this.GetNextRandom();

                case TypeCode.Decimal:
                    return (decimal)this.GetNextRandom();

                case TypeCode.Double:
                    return (double)this.GetNextRandom();

                case TypeCode.Int16:
                    return (short)this.GetNextRandom();

                case TypeCode.Int32:
                    return (int)this.GetNextRandom();

                case TypeCode.Int64:
                    return
                        this.GetNextRandom();

                case TypeCode.SByte:
                    return (sbyte)this.GetNextRandom();

                case TypeCode.Single:
                    return (float)this.GetNextRandom();

                case TypeCode.UInt16:
                    return (ushort)this.GetNextRandom();

                case TypeCode.UInt32:
                    return (uint)this.GetNextRandom();

                case TypeCode.UInt64:
                    return (ulong)this.GetNextRandom();

                default:
                    return new NoSpecimen();
            }
        }

        private long GetNextRandom()
        {
            lock (this.syncRoot)
            {
                this.EvaluateRange();

                long result;
                do
                {
                    if (this.lower >= int.MinValue &&
                        this.upper <= int.MaxValue)
                    {
                        result = this.random.Next((int)this.lower, (int)this.upper);
                    }
                    else
                    {
                        result = this.GetNextInt64InRange();
                    }
                }
                while (this.numbers.Contains(result));

                this.numbers.Add(result);
                return result;
            }
        }

        private void EvaluateRange()
        {
            if (this.count == (this.upper - this.lower))
            {
                this.count = 0;
                this.CreateRange();
            }

            this.count++;
        }

        private void CreateRange()
        {
            var remaining = this.limits.Where(x => x > this.upper - 1).ToArray();
            if (remaining.Any() && this.numbers.Any())
            {
                this.lower = this.upper;
                this.upper = remaining.Min() + 1;
            }
            else
            {
                this.lower = this.limits[0];
                this.upper = this.GetUpperRangeFromLimits();
            }

            this.numbers.Clear();
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
            return this.limits[1] >= int.MaxValue
                    ? this.limits[1]
                    : this.limits[1] + 1;
        }

        private long GetNextInt64InRange()
        {
            var range = (ulong)(this.upper - this.lower);
            ulong limit = ulong.MaxValue - (ulong.MaxValue % range);
            ulong number;
            do
            {
                var buffer = new byte[sizeof(ulong)];
                this.random.NextBytes(buffer);
                number = BitConverter.ToUInt64(buffer, 0);
            }
            while (number > limit);
            return (long)((number % range) + (ulong)this.lower);
        }
    }
}
