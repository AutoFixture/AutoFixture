using System;

namespace Ploeh.AutoFixture.DataAnnotations
{
    /// <summary>
    /// A customization for supporting specimens decorated with a RangeAttribute.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The returned values fall on the range specified by the RangeAttribute's Minimum and Maximum
    /// as well as the returned types match the OperandType specified by the RangeAttribute.
    /// </para>
    /// </remarks>
    public class RangeAttributeCustomization : ICustomization
    {
        /// <summary>
        /// Customizes the specified fixture by adding a RangeAttributeRelay and a RangedNumberGenerator.
        /// </summary>
        /// <param name="fixture">The fixture to customize.</param>
        public void Customize(IFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }

            fixture.Customizations.Add(new RangeAttributeRelay());
            fixture.Customizations.Add(new RangedNumberGenerator());
        }
    }
}
