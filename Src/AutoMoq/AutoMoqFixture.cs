using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoMoq
{
    /// <summary>
    /// Provides configuration operations for AutoMocking with Moq.
    /// </summary>
    public static class AutoMoqFixture
    {
        /// <summary>
        /// Enables auto-mocking with Moq.
        /// </summary>
        /// <param name="composer">The composer upon which to enable auto-mocking.</param>
        public static void EnableAutoMocking(this ICustomizableComposer composer)
        {
            if (composer == null)
            {
                throw new ArgumentNullException("composer");
            }

            composer.Customizations.Add(
                new MockPostprocessor(
                    new ConstructorInvoker(
                        new MockConstructorQuery())));
            composer.ResidueCollectors.Add(new MockRelay());
        }
    }
}
