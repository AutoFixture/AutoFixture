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
            var ticks = this.GetRandomLongNumber();
            return ReduceNumberToTicksRange(ticks);
        }

        private long GetRandomLongNumber()
        {
            var buffer = new byte[8];
            this.randomizer.NextBytes(buffer);
            return BitConverter.ToInt64(buffer, 0);
        }

        private static long ReduceNumberToTicksRange(long number)
        {
            var minTicks = DateTime.MinValue.Ticks;
            var maxTicks = DateTime.MaxValue.Ticks;
            return Math.Abs((number % (maxTicks - minTicks)) + minTicks);
        }
    }
}
