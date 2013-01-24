using System;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    public class RandomDateTimeSequenceGenerator : ISpecimenBuilder
    {
        private readonly DateTime minDate;
        private readonly DateTime maxDate;
        private readonly Random randomizer;

        public RandomDateTimeSequenceGenerator()
            : this(DateTime.MinValue, DateTime.MaxValue)
        {
        }

        public RandomDateTimeSequenceGenerator(DateTime minDate, DateTime maxDate)
        {
            if (minDate > maxDate)
            {
                throw new ArgumentException("The 'minDate' argument must be less than or equal to the 'maxDate'.");
            }

            this.minDate = minDate;
            this.maxDate = maxDate;
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

        private long ReduceNumberToTicksRange(ulong number)
        {
            var minTicks = (ulong)this.minDate.Ticks;
            var maxTicks = (ulong)this.maxDate.Ticks;
            var ticks = ((number % (maxTicks - minTicks + 1)) + minTicks);
            return (long)ticks;
        }
    }
}
