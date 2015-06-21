using System;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoNSubstitute
{
    /// <summary>
    /// Allows an <see cref="IFixture"/> to create dynamic test doubles with NSubstitute.
    /// </summary>
    public class AutoSubstituteCustomization : ICustomization
    {
        /// <summary>
        /// Adds NSubstitute components to the given <paramref name="fixture"/>.
        /// </summary>
        public void Customize(IFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }

            fixture.Customizations.Insert(0, new SubstituteBuilder(new MethodInvoker(new NSubstituteMethodQuery())));
            fixture.Customizations.Insert(0, new SubstituteAttributeRelay());
            fixture.ResidueCollectors.Insert(0, new SubstituteRelay());
        }
    }
}
