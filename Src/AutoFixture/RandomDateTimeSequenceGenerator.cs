using System;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    public class RandomDateTimeSequenceGenerator : ISpecimenBuilder
    {
        private readonly Random randomizer;

        public RandomDateTimeSequenceGenerator()
        {
            this.randomizer = new Random();
        }

        public object Create(object request, ISpecimenContext context)
        {
            if (IsNotDateTimeRequest(request))
            {
                return new NoSpecimen(request);
            }

            return CreateRandomDate();
        }

        private static bool IsNotDateTimeRequest(object request)
        {
            return !typeof(DateTime).IsAssignableFrom(request as Type);
        }

        private object CreateRandomDate()
        {
            return new DateTime(this.GetRandomNumberOfTicks());
        }

        private long GetRandomNumberOfTicks()
        {
            var number = this.GetRandomLongPositiveNumber();
            return ReduceNumberToTicksRange(number);
        }

        private ulong GetRandomLongPositiveNumber()
        {
            var buffer = new byte[8];
            this.randomizer.NextBytes(buffer);
            return BitConverter.ToUInt64(buffer, 0);
        }

        private static long ReduceNumberToTicksRange(ulong number)
        {
            var minTicks = (ulong)DateTime.MinValue.Ticks;
            var maxTicks = (ulong)DateTime.MaxValue.Ticks;
            var ticks = ((number % (maxTicks - minTicks + 1)) + minTicks);
            return (long)ticks;
        }
    }
}
