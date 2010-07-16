using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoMoq
{
    public static class AutoMoqFixture
    {
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
