using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoMoq
{
    /// <summary>
    /// Enables auto-mocking with Moq.
    /// </summary>
    public class AutoMoqCustomization : ICustomization
    {
        #region ICustomization Members

        /// <summary>
        /// Customizes a <see cref="IFixture"/> to enable auto-mocking with Moq.
        /// </summary>
        /// <param name="fixture">The fixture upon which to enable auto-mocking.</param>
        public void Customize(IFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }

            fixture.Customizations.Add(
                new MockPostprocessor(
                    new ConstructorInvoker(
                        new MockConstructorQuery())));
            fixture.ResidueCollectors.Add(new MockRelay());
        }

        #endregion
    }
}
