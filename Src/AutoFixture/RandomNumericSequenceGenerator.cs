using Ploeh.AutoFixture.Kernel;
using System;
using System.Collections.Generic;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates a sequence of random, unique, numbers starting at 1.
    /// </summary>
    public class RandomNumericSequenceGenerator : ISpecimenBuilder
    {
        private readonly object syncRoot;
        private readonly Random random;
        private readonly List<int> randomNumbers;

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomNumericSequenceGenerator" /> class.
        /// </summary>
        public RandomNumericSequenceGenerator()
        {
            this.syncRoot = new object();
            this.random = new Random();
            this.randomNumbers = new List<int>();
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
                return new NoSpecimen(request);
            }

            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            
            var limit = context.Resolve(typeof(RandomNumericSequenceLimit)) as RandomNumericSequenceLimit;
            if (limit == null)
            {
                return new NoSpecimen(request);
            }

            return this.CreateRandom(type, limit);
        }

        private object CreateRandom(Type request, RandomNumericSequenceLimit limit)
        {
            switch (Type.GetTypeCode(request))
            {
                case TypeCode.Byte:
                    return (byte)
                        this.GetNextRandom(limit);

                case TypeCode.Decimal:
                    return (decimal)
                        this.GetNextRandom(limit);

                case TypeCode.Double:
                    return (double)
                        this.GetNextRandom(limit);

                case TypeCode.Int16:
                    return (short)
                        this.GetNextRandom(limit);

                case TypeCode.Int32:
                    return
                        this.GetNextRandom(limit);

                case TypeCode.Int64:
                    return (long)
                        this.GetNextRandom(limit);

                case TypeCode.SByte:
                    return (sbyte)
                        this.GetNextRandom(limit);

                case TypeCode.Single:
                    return (float)
                        this.GetNextRandom(limit);

                case TypeCode.UInt16:
                    return (ushort)
                        this.GetNextRandom(limit);

                case TypeCode.UInt32:
                    return (uint)
                        this.GetNextRandom(limit);

                case TypeCode.UInt64:
                    return (ulong)
                        this.GetNextRandom(limit);

                default:
                    return new NoSpecimen(request);
            }
        }

        private int GetNextRandom(RandomNumericSequenceLimit limit)
        {
            lock (this.syncRoot)
            {
                try
                {
                    limit.Evaluate();
                }
                catch (ArgumentException)
                {
                    throw new InvalidOperationException(
                        "The upper limit has been reached. You may increase the limits by injecting a custom instance of the RandomNumericSequenceLimit class.");
                }
               
                int result;
                do
                {
                    result = this.random.Next(limit.Lower, limit.Upper);
                }
                while (this.randomNumbers.Contains(result));

                this.randomNumbers.Add(result);
                return result;
            }
        }
    }
}