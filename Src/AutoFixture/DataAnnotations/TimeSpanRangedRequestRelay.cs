using System;
using System.Globalization;
using AutoFixture.Kernel;

namespace AutoFixture.DataAnnotations
{
    /// <summary>
    /// Handles <see cref="RangedRequest"/> of TimeSpan type by forwarding requests
    /// to the <see cref="RangedNumberRequest"/> with min and max TimeSpan as milliseconds values.
    /// </summary>
    public class TimeSpanRangedRequestRelay : ISpecimenBuilder
    {
        /// <inheritdoc />
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var rangedRequest = request as RangedRequest;

            if (rangedRequest == null)
                return new NoSpecimen();

            if (rangedRequest.MemberType != typeof(TimeSpan))
                return new NoSpecimen();

            return CreateRangedTimeSpanSpecimen(rangedRequest, context);
        }

        private static object CreateRangedTimeSpanSpecimen(RangedRequest rangedRequest, ISpecimenContext context)
        {
            if (!(rangedRequest.Minimum is string) || !(rangedRequest.Maximum is string))
                return new NoSpecimen();

            var range = ParseTimeSpanRange(rangedRequest);
            return RandomizeTimeSpanInRange(range, context);
        }

        private static TimeSpanRange ParseTimeSpanRange(RangedRequest rangedRequest)
        {
            return new TimeSpanRange
            {
                Min = TimeSpan.Parse((string)rangedRequest.Minimum, CultureInfo.CurrentCulture),
                Max = TimeSpan.Parse((string)rangedRequest.Maximum, CultureInfo.CurrentCulture)
            };
        }

        private static object RandomizeTimeSpanInRange(TimeSpanRange range, ISpecimenContext context)
        {
            var millisecondsInRange = context.Resolve(
                new RangedNumberRequest(typeof(double), range.Min.TotalMilliseconds, range.Max.TotalMilliseconds));

            if (millisecondsInRange is NoSpecimen)
                return new NoSpecimen();

            return TimeSpan.FromMilliseconds((double)millisecondsInRange);
        }

        private struct TimeSpanRange
        {
            public TimeSpan Min { get; set; }

            public TimeSpan Max { get; set; }
        }
    }
}
