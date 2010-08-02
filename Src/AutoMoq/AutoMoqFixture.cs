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
        /// <param name="fixture">The composer upon which to enable auto-mocking.</param>
        public static void EnableAutoMocking(this IFixture fixture)
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
    }
}
