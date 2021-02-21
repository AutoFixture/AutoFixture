using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Numerics;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Creates a sequence of random, unique, numbers starting at 1.
    /// </summary>
    public class RandomNumericSequenceGenerator : ISpecimenBuilder
    {
        private readonly long[] limits;
        private readonly Random randomizer;
        private readonly object syncRoot;
        private LinearCongruentialGenerator innerGenerator;

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
            this.randomizer = new Random();
        }

        /// <summary>
        /// Gets the sequence of limits.
        /// </summary>
        public IEnumerable<long> Limits => this.limits;

        /// <summary>
        /// Creates an anonymous number.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// The next random number in a sequence, if <paramref name="request" /> is a request
        /// for a numeric value; otherwise, a <see cref="NoSpecimen" /> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (!(request is Type type))
                return new NoSpecimen();

            return this.CreateRandom(type);
        }

        private static void ValidateThatLimitsAreStrictlyAscending(long[] limits)
        {
            if (limits.Zip(limits.Skip(1), (a, b) => a >= b).Any(b => b))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(limits),
                    "Limits must be ascending numbers, but the passed limits are not: " +
                    string.Join(",", limits.Select(l => l.ToString(CultureInfo.CurrentCulture))));
            }
        }

        private object CreateRandom(Type request)
        {
            switch (Type.GetTypeCode(request))
            {
                case TypeCode.Byte:
                    return (byte)this.GetNextRandom();

                case TypeCode.SByte:
                    return (sbyte)this.GetNextRandom();

                case TypeCode.Int16:
                    return (short)this.GetNextRandom();

                case TypeCode.UInt16:
                    return (ushort)this.GetNextRandom();

                case TypeCode.Int32:
                    return (int)this.GetNextRandom();

                case TypeCode.UInt32:
                    return (uint)this.GetNextRandom();

                case TypeCode.Int64:
                    return this.GetNextRandom();

                case TypeCode.UInt64:
                    return (ulong)this.GetNextRandom();

                case TypeCode.Decimal:
                    return (decimal)this.GetNextRandom();

                case TypeCode.Single:
                    return (float)this.GetNextRandom();

                case TypeCode.Double:
                    return (double)this.GetNextRandom();

                default:
                    return new NoSpecimen();
            }
        }

        private long GetNextRandom()
        {
            lock (this.syncRoot)
            {
                this.EnsureGeneratorAndUniqueValues();
                return this.innerGenerator.GenerateNextValue();
            }
        }

        private void EnsureGeneratorAndUniqueValues()
        {
            if (this.innerGenerator != null && this.innerGenerator.ProducesUniqueValues)
                return;

            var (nextLower, nextUpper) = this.EvaluateNextRange(this.innerGenerator?.MaxValueInclusive);

            this.innerGenerator = new LinearCongruentialGenerator(nextLower, nextUpper, this.randomizer);
        }

        private (long NextLower, long NextUpper) EvaluateNextRange(long? previousUpperBoundary)
        {
            /* We guarantee to not return duplicates until we produce all the possible random values.
             * After we reach out the end of the cycle, we should start from the beginning.
             * The LG generator used in the code expects inclusive lower and upper boundaries.
             *
             * Consider the following configured boundaries: 1,3,5,6.
             * The result of this method should be following:
             *
             * 1st iteration: [1;3]
             * 2nd iteration: [4;5]
             * 3rd iteration: [6;6]
             * 4th iteration: [1;3] and so on.
             */

            if (previousUpperBoundary == null || previousUpperBoundary.Value == this.limits.Last())
                return (this.limits[0], this.limits[1]);

            var indexOfPreviousUpper = this.limits.IndexOf(previousUpperBoundary.Value);
            Debug.Assert(indexOfPreviousUpper != -1, "Unable to find the previous upper boundary.");

            var newLowerIndex = indexOfPreviousUpper;

            var newLower = this.limits[newLowerIndex] + 1;
            var newUpper = this.limits[newLowerIndex + 1];

            return (newLower, newUpper);
        }

        /// <summary>
        /// Generator that produces unique values within the specified range. After all the numbers were generated
        /// it starts range from the beginning producing the exactly same sequence.
        /// <para>
        /// Use the <see cref="ProducesUniqueValues"/> property to check whether end of the cycle was reached. After
        /// that create a new instance of generator, so it's populated with a new unique seed and constants.
        /// </para>
        /// <para>
        /// See wiki for more detail: https://en.wikipedia.org/wiki/Linear_congruential_generator.
        /// </para>
        /// </summary>
        private class LinearCongruentialGenerator
        {
            // Those good constants were copied from this doc:
            // http://www.ams.org/journals/mcom/1999-68-225/S0025-5718-99-00996-5/S0025-5718-99-00996-5.pdf
            // Some constants were generated by ourself, because they were wrong (like for M=1021).
            private static readonly C[] GeneratorConstants =
            {
                new C { M = /* 2^8-5    */ 251, A = { 33, 213, 55, 178 } },
                new C { M = /* 2^9-3    */ 509, A = { 35, 160, 110, 236, 273, 399, 349, 474 } },
                new C { M = /* 2^10-3   */ 1021, A = { 65, 377, 585, 739 } },
                new C { M = /* 2^11-9   */ 2039, A = { 995, 1498, 328, 603, 393, 799 } },
                new C { M = /* 2^12-3   */ 4093, A = { 209, 3858, 235, 3884 } },
                new C { M = /* 2^13-1   */ 8191, A = { 884, 7459, 1716, 5580, 2685, 6083 } },
                new C { M = /* 2^14-3   */ 16381, A = { 572, 13374, 3007, 15809, 665, 3424 } },
                new C { M = /* 2^15-19  */ 32749, A = { 219, 30805, 1944, 32530, 9515, 10088 } },
                new C { M = /* 2^16-15  */ 65521, A = { 17364, 32236, 33285, 48157, 2469, 47104 } },
                new C { M = /* 2^17-1   */ 131071, A = { 43165, 66284, 29223, 119858, 29803, 76704 } },
                new C { M = /* 2^18-5   */ 262139, A = { 92717, 166972, 21876, 118068 } },
                new C { M = /* 2^19-1   */ 524287, A = { 283741, 358899, 37698, 127574, 155411, 157781 } },
                new C { M = /* 2^20-3   */ 1048573, A = { 380985, 444362, 604211, 667588, 100768, 463964 } },
                new C { M = /* 2^21-9   */ 2097143, A = { 360889, 1372180, 1043187, 1352851, 1939807, 1969917 } },
                new C { M = /* 2^22-3   */ 4194301, A = { 914334, 1406151, 2788150, 3279967 } },
                new C { M = /* 2^23-15  */ 8388593, A = { 653276, 5169235, 3219358, 7735317, 1706325, 6513898 } },
                new C { M = /* 2^24-3   */ 16777213, A = { 6423135, 9726917, 7050296, 10354078, 4408741, 6180188 } },
                new C { M = /* 2^25-39  */ 33554393, A = { 25907312, 32544832, 12836191, 5420585, 28133808 } },
                new C { M = /* 2^26-5   */ 67108859, A = { 26590841, 11526618, 19552116, 24409594, 66117721 } },
                new C { M = /* 2^27-39  */ 134217689, A = { 45576512, 70391260, 63826429, 88641177, 3162696 } },
                new C { M = /* 2^28-57  */ 268435399, A = { 246049789, 150873839, 140853223, 102445941, 29908911 } },
                new C { M = /* 2^29-3   */ 536870909, A = { 520332806, 219118189, 530877178, 475905290 } },
                new C { M = /* 2^30-35  */ 1073741789, A = { 771645345, 599290962, 295397169, 1017586987 } },
                new C { M = /* 2^31-1   */ 2147483647, A = { 1583458089, 1132489760, 784588716, 163490618 } },
                new C { M = /* 2^32-5   */ 4294967291, A = { 1588635695, 3870709308, 1223106847, 4223879656 } },
                new C { M = /* 2^33-9   */ 8589934583, A = { 7425194315, 8436767804, 2278442619, 1729516095 } },
                new C { M = /* 2^34-41  */ 17179869143, A = { 5295517759, 2447157083, 473186378, 6625295500 } },
                new C { M = /* 2^35-31  */ 34359738337, A = { 3124199165, 27181987157, 22277574834, 16353251630 } },
                new C { M = /* 2^36-5   */ 68719476731, A = { 49865143810, 44525253482, 45453986995, 40162435147 } },
                new C { M = /* 2^37-25  */ 137438953447, A = { 76886758244, 31450092817, 2996735870, 105638438130 } },
                new C { M = /* 2^38-45  */ 274877906899, A = { 17838542566, 234584904863, 101262352583 } },
                new C { M = /* 2^39-7   */ 549755813881, A = { 61992693052, 486583348513, 541240737696 } },
                new C { M = /* 2^40-87  */ 1099511627689, A = { 1038914804222, 88718554611 } },
                new C { M = /* 2^41-21  */ 2199023255531, A = { 140245111714, 416480024109 } },
                new C { M = /* 2^42-11  */ 4398046511093, A = { 2214813540776, 2928603677866 } },
                new C { M = /* 2^43-57  */ 8796093022151, A = { 4928052325348, 4204926164974 } },
                new C { M = /* 2^44-17  */ 17592186044399, A = { 6307617245999, 11394954323348 } },
                new C { M = /* 2^45-55  */ 35184372088777, A = { 25933916233908, 18586042069168 } },
                new C { M = /* 2^46-21  */ 70368744177643, A = { 63975993200055, 15721062042478 } },
                new C { M = /* 2^47-115 */ 140737488355213, A = { 72624924005429, 47912952719020 } },
                new C { M = /* 2^48-59  */ 281474976710597, A = { 49235258628958, 51699608632694 } },
                new C { M = /* 2^49-81  */ 562949953421231, A = { 265609885904224, 480567615612976 } },
                new C { M = /* 2^50-27  */ 1125899906842597, A = { 1087141320185010, 157252724901243 } },
                new C { M = /* 2^51-129 */ 2251799813685119, A = { 349044191547257, 277678575478219 } },
                new C { M = /* 2^52-47  */ 4503599627370449, A = { 4359287924442956, 3622689089018661 } },
                new C { M = /* 2^53-111 */ 9007199254740881, A = { 2082839274626558, 4179081713689027 } },
                new C { M = /* 2^54-33  */ 18014398509481951, A = { 9131148267933071, 3819217137918427 } },
                new C { M = /* 2^55-55  */ 36028797018963913, A = { 33266544676670489, 19708881949174686 } },
                new C { M = /* 2^56-5   */ 72057594037927931, A = { 4595551687825993, 26093644409268278 } },
                new C { M = /* 2^57-13  */ 144115188075855859, A = { 75953708294752990, 95424006161758065 } },
                new C { M = /* 2^58-27  */ 288230376151711717, A = { 101565695086122187, 163847936876980536 } },
                new C { M = /* 2^59-55  */ 576460752303423433, A = { 346764851511064641, 124795884580648576 } },
                new C { M = /* 2^60-93  */ 1152921504606846883, A = { 561860773102413563, 439138238526007932 } },
                new C { M = /* 2^61-1   */ 2305843009213693951, A = { 1351750484049952003, 1070922063159934167 } },
                new C { M = /* 2^62-57  */ 4611686018427387847, A = { 2774243619903564593, 431334713195186118 } },
                new C { M = /* 2^63-25  */ 9223372036854775783, A = { 4645906587823291368, 2551091334535185398 } },
                new C { M = /* 2^64-59  */ 18446744073709551557, A = { 13891176665706064842, 2227057010910366687 } }
            };

            private readonly ulong a;
            private readonly ulong m;

            // Generated value is in [1;upperBoundary] range.
            private readonly ulong generatorUpperBoundary;

            private ulong count;
            private ulong lastGeneratedValue;

            public long MinValueInclusive { get; }

            public long MaxValueInclusive { get; }

            public bool ProducesUniqueValues => this.count < this.generatorUpperBoundary;

            public LinearCongruentialGenerator(long minValueInclusive, long maxValueInclusive, Random randomizer)
            {
                this.MinValueInclusive = minValueInclusive;
                this.MaxValueInclusive = maxValueInclusive;

                this.generatorUpperBoundary = SubtractMinFromMax(maxValueInclusive, minValueInclusive);

                // Generated value range starts from 1, so increase upper boundary to get a full range.
                IncrementWithoutOverflow(ref this.generatorUpperBoundary);

                (this.m, this.a) = GetGeneratorConstants(this.generatorUpperBoundary, randomizer);
                this.lastGeneratedValue = GenerateSeed(this.generatorUpperBoundary, randomizer);
            }

            public long GenerateNextValue()
            {
                do
                {
                    this.lastGeneratedValue = ComputeNextRandomValue(this.lastGeneratedValue, this.a, this.m);
                }
                while (this.lastGeneratedValue > this.generatorUpperBoundary);

                IncrementWithoutOverflow(ref this.count);

                // Generated value starts from 1, so decrement it to make the min boundary inclusive.
                ulong offsetToAppend = this.lastGeneratedValue - 1;
                return AddOffsetToMinimalValue(this.MinValueInclusive, offsetToAppend);
            }

            private static ulong SubtractMinFromMax(long max, long min)
            {
                Debug.Assert(max >= min, "Maximum should be greater or equal min.");

                checked
                {
                    // No overflow could happen if both numbers have the same sign.
                    if (Math.Sign(max) == Math.Sign(min))
                    {
                        return (ulong)(max - min);
                    }

                    // Math.Abs(long.MinValue) fails with overflow, so hardcode the absolute value.
                    ulong minAbs = min == long.MinValue ? 9223372036854775808UL : (ulong)Math.Abs(min);
                    return (ulong)Math.Abs(max) + minAbs;
                }
            }

            private static void IncrementWithoutOverflow(ref ulong value)
            {
                if (value < ulong.MaxValue) value++;
            }

            private static ulong GenerateSeed(ulong upperBoundary, Random randomizer)
            {
                Debug.Assert(upperBoundary > 0, "Max number is expected to be greater than zero.");

                // Random handles the upper boundary exclusively.
                IncrementWithoutOverflow(ref upperBoundary);
                var coercedMax = (int)Math.Min(upperBoundary, int.MaxValue);

                return (ulong)randomizer.Next(1, coercedMax);
            }

            private static (ulong M, ulong A) GetGeneratorConstants(ulong generatorUpperBoundary, Random rand)
            {
                var constSetToUse = GeneratorConstants.FirstOrDefault(p => p.M > generatorUpperBoundary)
                                    ?? GeneratorConstants.Last();

                var m = constSetToUse.M;
                var a = constSetToUse.A[rand.Next(1, constSetToUse.A.Count)];

                return (m, a);
            }

            private static ulong ComputeNextRandomValue(ulong lastValue, ulong a, ulong m)
            {
                const ulong noOverflowULongMultiplierLimit = uint.MaxValue;
                if ((m <= noOverflowULongMultiplierLimit) && (a <= noOverflowULongMultiplierLimit))
                    return checked((lastValue * a) % m);

                // Try handle as decimal - it works a bit quicker than BigInteger.
                const ulong noOverflowDecimalMultiplierLimit = 0xFFFF_FFFF_FFFF /* 2^48-1 */;
                if ((m <= noOverflowDecimalMultiplierLimit) && (a <= noOverflowDecimalMultiplierLimit))
                    return (ulong)(((decimal)lastValue * a) % m);

                return (ulong)(((BigInteger)lastValue * a) % m);
            }

            private static long AddOffsetToMinimalValue(long min, ulong offset)
            {
                checked
                {
                    var result = min;

                    if (offset > long.MaxValue)
                    {
                        result += long.MaxValue;
                        offset -= long.MaxValue;
                    }

                    result += (long)offset;
                    return result;
                }
            }

            private class C
            {
                public ulong M { get; set; }

                public IList<ulong> A { get; } = new List<ulong>();
            }
        }
    }
}