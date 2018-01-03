using System;
using System.Linq;

namespace AutoFixture.DataAnnotations
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
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

            var nodeToDelete = fixture.Customizations.FirstOrDefault(x => x is DataAnnotationsSupportNode);
            if (nodeToDelete != null)
            {
                fixture.Customizations.Remove(nodeToDelete);
            }
        }
    }
}