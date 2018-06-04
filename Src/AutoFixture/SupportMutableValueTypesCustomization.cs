using System;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// A customization that changes how custom <see langword="struct"/> are generated. Uses <see cref="MutableValueTypeGenerator"/>.
    /// </summary>
    public class SupportMutableValueTypesCustomization : ICustomization
    {
        /// <summary>
        /// Customizes specified fixture by adding <see cref="MutableValueTypeGenerator"/> as a default strategy for
        /// creating new custom <see langword="struct"/> that has only default constructor.
        /// </summary>
        /// <param name="fixture">The fixture to customize.</param>
        public void Customize(IFixture fixture)
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

            fixture.Customizations.Add(
                new Postprocessor(
                    new MutableValueTypeGenerator(),
                    new AutoPropertiesCommand()));
        }
    }
}