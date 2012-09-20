using Ploeh.AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ploeh.AutoFixture
{
    public class RandomNumericSequenceGenerator : ISpecimenBuilder
    {
        private readonly IEnumerable<int> boundaries;
        private readonly int upperBound;
        private readonly int lowerBound;

        private readonly Random random;
        private readonly List<int> numbers;
        private int minimum;
        private int maximum;

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
            this.lowerBound = this.boundaries.First();
            this.upperBound = this.boundaries.Last();

            this.random = new Random();
            this.numbers = new List<int>();
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
            if (this.numbers.Count == this.upperBound)
            {
                throw new InvalidOperationException(
                    "The upper bound has been reached. Either increase the boundaries or use the default constructor to generate up to 2147483647 random numbers.");
            }

            if (this.numbers.Count == 0)
            {
                this.minimum = 1;
                this.maximum = this.lowerBound + 1;
            }
            else if (this.numbers.Count == this.maximum - 1)
            {
                this.minimum = this.maximum;
                this.maximum = this.boundaries.FirstOrDefault(x => x > this.minimum);

                if (this.maximum == 0 && this.minimum == this.upperBound)
                {
                    this.numbers.Add(this.upperBound);
                    return this.upperBound;
                }
            }

            int result;
            do
            {
                result = this.random.Next(this.minimum, this.maximum);
            }
            while (this.numbers.Contains(result));

            this.numbers.Add(result);
            return result;
        }
    }
}