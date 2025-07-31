using System;
using AutoFixture.DataAnnotations;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Creates new <see cref="TimeZoneInfoGenerator"/> instances.
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
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (!typeof(TimeZoneInfo).Equals(request))
            {
                return new NoSpecimen();
            }

            var id = (string)context.Resolve(typeof(string));
            var tmp1 = new RangedNumberRequest(typeof(int), -14, 14);
            var tmp2 = context.Resolve(tmp1);
            int offset = (int)tmp2;
            var baseUtcOffset = TimeSpan.FromHours(offset);
            var displayName = (string)context.Resolve(typeof(string));
            var standardDisplayName = (string)context.Resolve(typeof(string));

            return TimeZoneInfo.CreateCustomTimeZone(id, baseUtcOffset, displayName, standardDisplayName);
        }
    }
}