using Ploeh.AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ploeh.AutoFixture
{
    public class RandomNumericSequenceGenerator : ISpecimenBuilder
    {
        private readonly IEnumerable<int> boundaries;

        private readonly Random random;
        private readonly List<int> numbers;
        private readonly MinMax minMax;

        public RandomNumericSequenceGenerator()
            : this(Byte.MaxValue, Int16.MaxValue, Int32.MaxValue)
        {
        }

        public RandomNumericSequenceGenerator(IEnumerable<int> boundaries)
            : this(boundaries.ToArray())
        {
        }

        public RandomNumericSequenceGenerator(params int[] boundaries)
        {
            if (boundaries == null)
            {
                throw new ArgumentNullException("boundaries");
            }

            this.boundaries = boundaries;

            this.random = new Random();
            this.numbers = new List<int>();
            this.minMax = new MinMax(boundaries);
        }

        public IEnumerable<int> Boundaries
        {
            get { return this.boundaries; }
        }

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
            MinMax current;
            try
            {
                current = this.minMax.Calculate(this.numbers.Count);
            }
            catch (ArgumentException)
            {
                throw new InvalidOperationException(
                    "The upper bound has been reached. Either increase the boundaries or use the default constructor to generate up to 2147483647 random numbers.");
            }

            if (current.GetIsBoundFromAbove())
            {
                int upperBound = this.boundaries.Last();
                this.numbers.Add(upperBound);
                return upperBound;
            }

            int result;
            do
            {
                result = this.random.Next(current.Minimum, current.Maximum);
            }
            while (this.numbers.Contains(result));

            this.numbers.Add(result);
            return result;
        }

        private sealed class MinMax
        {
            private readonly int[] boundaries;
            private readonly int upperBound;

            private int minimum;
            private int maximum;

            private MinMax(int[] boundaries, int minimum, int maximum)
            {
                this.boundaries = boundaries;
                this.upperBound = boundaries[boundaries.Length - 1];

                this.minimum = minimum;
                this.maximum = maximum;
            }

            internal MinMax(int[] boundaries)
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

            internal MinMax Calculate(int number)
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

                return new MinMax(this.boundaries, this.minimum, this.maximum);
            }

            internal bool GetIsBoundFromAbove()
            {
                return this.maximum == 0 && this.minimum == this.upperBound;
            }
        }
    }
}