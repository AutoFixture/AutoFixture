using System;
using AutoFixture.Kernel;

namespace AutoFixture.Xunit2.UnitTest.TestTypes
{
    internal class ComposerWithoutADefaultConstructor : DelegatingFixture
    {
        public ComposerWithoutADefaultConstructor(Func<ISpecimenBuilder> onCompose)
        {
            if (onCompose is null) throw new ArgumentNullException(nameof(onCompose));
        }
    }
}
