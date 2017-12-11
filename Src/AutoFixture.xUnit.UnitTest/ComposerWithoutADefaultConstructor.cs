using System;
using AutoFixture.Kernel;

namespace AutoFixture.Xunit.UnitTest
{
    internal class ComposerWithoutADefaultConstructor : DelegatingFixture
    {
        public ComposerWithoutADefaultConstructor(Func<ISpecimenBuilder> onCompose)
        {
            if (onCompose == null) throw new ArgumentNullException(nameof(onCompose));
        }
    }
}
