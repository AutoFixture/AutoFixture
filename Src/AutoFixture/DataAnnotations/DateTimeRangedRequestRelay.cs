using AutoFixture.DataAnnotations;
using AutoFixture.Kernel;
using System;
using System.Globalization;

namespace QH.MemberService.XTests.Framework.Services
{
    public class DateTimeRangedRequestRelay : ISpecimenBuilder
    {
        /// <inheritdoc />
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var rangedRequest = request as RangedRequest;

            if (rangedRequest == null)
                return new NoSpecimen();

            if (rangedRequest.MemberType != typeof(DateTime))
                return new NoSpecimen();

            return CreateDateTimeSpecimen(rangedRequest, context);
        }

        private static object CreateDateTimeSpecimen(RangedRequest rangedRequest, ISpecimenContext context)
        {
            if (!(rangedRequest.Minimum is string) || !(rangedRequest.Maximum is string))
                return new NoSpecimen();

            var range = ParseTimeSpanRange(rangedRequest);
            return RandomizeDateTimeInRange(range, context);
        }

        private static DateTimeRange ParseTimeSpanRange(RangedRequest rangedRequest)
        {
            return new DateTimeRange
            {
                Min = DateTime.Parse((string)rangedRequest.Minimum, CultureInfo.CurrentCulture),
                Max = DateTime.Parse((string)rangedRequest.Maximum, CultureInfo.CurrentCulture)
            };
        }

        private static object RandomizeDateTimeInRange(DateTimeRange range, ISpecimenContext context)
        {
            var ticksInRange = context.Resolve(
                new RangedNumberRequest(typeof(long), range.Min.Ticks, range.Max.Ticks));

            if (ticksInRange is NoSpecimen)
                return new NoSpecimen();

            return new DateTime((long)ticksInRange);
        }

        private struct DateTimeRange
        {
            public DateTime Min { get; set; }

            public DateTime Max { get; set; }
        }
    }
}
