using System;
using AutoFixture.Kernel;

namespace AutoFixture.Xunit.v3.UnitTest.TestTypes
{
    internal class ComposerWithoutADefaultConstructor : DelegatingFixture
    {
        public ComposerWithoutADefaultConstructor(Func<ISpecimenBuilder> onCompose)
        {
            if (onCompose is null)
            {
                throw new ArgumentNullException(nameof(onCompose));
            }
        }
    }
}