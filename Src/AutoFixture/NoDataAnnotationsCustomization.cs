using System;
using System.Linq;
using Ploeh.AutoFixture.DataAnnotations;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// A customization that removes support for generating values that are customized
    /// based on the member's data annotations attributes. 
    /// </summary>
    /// <remarks>
    /// This removes the <see cref="RangeAttributeRelay"/>,
    /// <see cref="StringLengthAttributeRelay"/>, and
    /// <see cref="RegularExpressionAttributeRelay"/>. Support for the removal of data
    /// annotations is provided for performance reasons, however a typical user of 
    /// AutoFixture would not need to worry about the performance.
    /// </remarks>
    public class NoDataAnnotationsCustomization : ICustomization
    {
        /// <summary>
        /// Customizes specified fixture by removing any default data annotations relays.
        /// </summary>
        /// <param name="fixture">The fixture to customize.</param>
        public void Customize(IFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }

            var dataAnnotationsRelayTypes = new[]
            {
                typeof (RangeAttributeRelay),
                typeof (StringLengthAttributeRelay),
                typeof (RegularExpressionAttributeRelay),
            };

            fixture
                .Customizations
                .Where(c => dataAnnotationsRelayTypes.Contains(c.GetType()))
                .ToList()
                .ForEach(c => fixture.Customizations.Remove(c));
        }
    }
}