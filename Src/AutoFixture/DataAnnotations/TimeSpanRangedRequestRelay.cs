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
            TimeSpanRange range;

            if (rangedRequest.Minimum is string)
            {
                range = GetTimeSpanRangeFromStringValues(rangedRequest);
            }
            else if (rangedRequest.Minimum.GetType().IsNumberType())
            {
                range = GetTimeSpanRangeFromNumberValues(rangedRequest);
            }
            else
            {
                return new NoSpecimen();
            }

            return RandomizeTimeSpanInRange(range, context);
        }

        private static TimeSpanRange GetTimeSpanRangeFromStringValues(RangedRequest rangedRequest)
        {
            return new TimeSpanRange
            {
                MillisecondsMin = StringToMilliseconds((string)rangedRequest.Minimum),
                MillisecondsMax = StringToMilliseconds((string)rangedRequest.Maximum)
            };
        }

        private static double StringToMilliseconds(string serializedTimeSpan)
        {
            return TimeSpan.Parse(serializedTimeSpan, CultureInfo.CurrentCulture).TotalMilliseconds;
        }

        private static TimeSpanRange GetTimeSpanRangeFromNumberValues(RangedRequest rangedRequest)
        {
            return new TimeSpanRange
            {
                MillisecondsMin = (double)rangedRequest.GetConvertedMinimum(typeof(double)),
                MillisecondsMax = (double)rangedRequest.GetConvertedMaximum(typeof(double))
            };
        }

        private static object RandomizeTimeSpanInRange(TimeSpanRange range, ISpecimenContext context)
        {
            var millisecondsInRange = context.Resolve(
                new RangedNumberRequest(typeof(double), range.MillisecondsMin, range.MillisecondsMax));

            return millisecondsInRange is NoSpecimen
                ? millisecondsInRange
                : TimeSpan.FromMilliseconds((double)millisecondsInRange);
        }

        private class TimeSpanRange
        {
            public double MillisecondsMin { get; set; }
            public double MillisecondsMax { get; set; }
        }
    }
}
