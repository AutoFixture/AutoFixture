using Ploeh.AutoFixture.Kernel;
using System;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// A customization that enables numeric specimens to be random and unique.
    /// </summary>
    public class RandomNumericSequenceCustomization : ICustomization
    {
        /// <summary>
        /// Customizes the specified fixture by adding a 
        /// <see cref="RandomNumericSequenceLimitGenerator"/> and a <see cref="RandomNumericSequenceGenerator"/>
        /// so that the <see cref="ISpecimenBuilderComposer.Compose"/> will take them into accout.
        /// </summary>
        /// <param name="fixture">The fixture to customize.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public void Customize(IFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }

            fixture.Customizations.Add(
                new CompositeSpecimenBuilder(
                    new RandomNumericSequenceLimitGenerator(),
                    new RandomNumericSequenceGenerator()));
        }
    }
}