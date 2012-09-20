using Ploeh.AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ploeh.AutoFixture
{
    /// <summary>Creates a sequence of random, unique, numbers starting at 1.</summary>
    /// <remarks>
    /// <para>
    /// The purpose of this class is to create truly constrained non-deterministic numbers.
    /// It starts by picking random numbers from the set [1, 255], and when that set is depleted,
    /// then random numbers from the set [256, 32767] and so on. The maximum is 2147483647, however
    /// it is possible to customize the boundaries by passing a sequence of limits.
    /// </para>
    /// </remarks>
    public class RandomNumericSequenceGenerator : ISpecimenBuilder
    {
        private readonly IEnumerable<int> boundaries;
        private readonly object syncRoot;
        private readonly Random random;
        private readonly List<int> numbers;
        private readonly Limit limit;

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomNumericSequenceGenerator" /> class
        /// using the boundaries [255, 32767, 2147483647].
        /// </summary>
        public RandomNumericSequenceGenerator()
            : this(Byte.MaxValue, Int16.MaxValue, Int32.MaxValue)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomNumericSequenceGenerator" /> class
        /// using the specified boundaries.
        /// </summary>
        /// <param name="boundaries">
        /// A sequence of limits where random numbers can be generated.
        /// </param>
        public RandomNumericSequenceGenerator(IEnumerable<int> boundaries)
            : this(boundaries.ToArray())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomNumericSequenceGenerator" /> class
        /// using the speified boundaries.
        /// </summary>
        /// <param name="boundaries">
        /// A sequence of limits where random numbers can be generated.
        /// </param>
        /// <exception cref="System.ArgumentNullException"><paramref name="boundaries"/> is null.
        /// </exception>
        public RandomNumericSequenceGenerator(params int[] boundaries)
        {
            if (boundaries == null)
            {
                throw new ArgumentNullException("boundaries");
            }

            this.boundaries = boundaries;
            this.syncRoot = new object();
            this.random = new Random();
            this.numbers = new List<int>();
            this.limit = new Limit(boundaries);
        }

        /// <summary>
        /// Gets the boundaries where random numbers can be generated.
        /// </summary>
        /// <value>
        /// The boundaries where random numbers can be generated.
        /// </value>
        public IEnumerable<int> Boundaries
        {
            get { return this.boundaries; }
        }

        /// <summary>
        /// Creates an anonymous number in the specified boundaries.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">Not used.</param>
        /// <returns>
        /// The next random number in a sequence, if <paramref name="request"/> is a request
        /// for a numeric value; otherwise, a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            var type = request as Type;
            if (type == null)
            {
                return new NoSpecimen(request);
            }

            return this.CreateRandom(type);
        }

        private object CreateRandom(Type request)
        {
            switch (Type.GetTypeCode(request))
            {
                case TypeCode.Byte:
                    return (byte)
                        this.GetNextRandom();

                case TypeCode.Decimal:
                    return (decimal)
                        this.GetNextRandom();

                case TypeCode.Double:
                    return (double)
                        this.GetNextRandom();

                case TypeCode.Int16:
                    return (short)
                        this.GetNextRandom();

                case TypeCode.Int32:
                    return
                        this.GetNextRandom();

                case TypeCode.Int64:
                    return (long)
                        this.GetNextRandom();

                case TypeCode.SByte:
                    return (sbyte)
                        this.GetNextRandom();

                case TypeCode.Single:
                    return (float)
                        this.GetNextRandom();

                case TypeCode.UInt16:
                    return (ushort)
                        this.GetNextRandom();

                case TypeCode.UInt32:
                    return (uint)
                        this.GetNextRandom();

                case TypeCode.UInt64:
                    return (ulong)
                        this.GetNextRandom();

                default:
                    return new NoSpecimen(request);
            }
        }

        private int GetNextRandom()
        {
            lock (this.syncRoot)
            {
                try
                {
                    this.limit.Calculate(this.numbers.Count);
                }
                catch (ArgumentException)
                {
                    throw new InvalidOperationException(
                        "The upper bound has been reached. Either increase the boundaries or use the default constructor to generate up to 2147483647 random numbers.");
                }

                if (this.limit.GetIsBoundFromAbove())
                {
                    int upperBound = this.boundaries.Last();
                    this.numbers.Add(upperBound);
                    return upperBound;
                }

                int result;
                do
                {
                    result = this.random.Next(this.limit.Minimum, this.limit.Maximum);
                }
                while (this.numbers.Contains(result));

                this.numbers.Add(result);
                return result;
            }
        }

        private sealed class Limit
        {
            private readonly int[] boundaries;
            private readonly int upperBound;

            private int minimum;
            private int maximum;

            private Limit(int[] boundaries, int minimum, int maximum)
            {
                this.boundaries = boundaries;
                this.upperBound = boundaries[boundaries.Length - 1];

                this.minimum = minimum;
                this.maximum = maximum;
            }

            internal Limit(int[] boundaries)
                : this(boundaries, 1, boundaries[0] + 1)
            {
            }

            internal int Minimum
            {
                get { return this.minimum; }
            }

            internal int Maximum
            {
                get { return this.maximum; }
            }

            internal void Calculate(int number)
            {
                if (number == this.upperBound)
                {
                    throw new ArgumentException(
                        "Number must be lower than the upper bound.");
                }

                if (number == this.maximum - 1)
                {
                    this.minimum = this.maximum;
                    this.maximum = this.boundaries.FirstOrDefault(x => x > this.minimum);
                }
            }

            internal bool GetIsBoundFromAbove()
            {
                return this.maximum == 0 && this.minimum == this.upperBound;
            }
        }
    }
}