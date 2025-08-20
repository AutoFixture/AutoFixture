using System;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Creates new <see cref="TimeZoneInfo"/> instances.
    /// </summary>
    public class TimeZoneInfoGenerator : ISpecimenBuilder
    {
        /// <summary>
        /// Creates a new TimeZoneInfo.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// The requested specimen if possible; otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));
            if (!typeof(TimeZoneInfo).Equals(request)) return NoSpecimen.Instance;

            var timeZoneRangeRequest = new RangedNumberRequest(typeof(int), -12, 14);
            var offsetResult = context.Resolve(timeZoneRangeRequest);
            if (offsetResult is NoSpecimen or OmitSpecimen)
            {
                return offsetResult;
            }

            if (offsetResult is not int offset)
            {
                throw new InvalidOperationException(
                    $"The result of ranged number request must be an int, but was {offsetResult?.GetType().FullName ?? "null"}.");
            }

            var baseUtcOffset = TimeSpan.FromHours(offset);
            var sign = baseUtcOffset < TimeSpan.Zero ? "-" : "+";
            var id = $"UTC{sign}{baseUtcOffset:hh}";
            var displayName = $"(UTC{sign}{baseUtcOffset:hh\\:mm}) Test Time Zone{sign}{baseUtcOffset:hh}";

            return TimeZoneInfo.CreateCustomTimeZone(
                id: id,
                baseUtcOffset: baseUtcOffset,
                displayName: displayName,
                standardDisplayName: id);
        }
    }
}