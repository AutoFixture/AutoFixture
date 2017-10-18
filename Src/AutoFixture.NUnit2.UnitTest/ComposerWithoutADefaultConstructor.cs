using System;
using AutoFixture.Kernel;

namespace AutoFixture.NUnit2.UnitTest
{
    internal class ComposerWithoutADefaultConstructor : DelegatingFixture
    {
        public ComposerWithoutADefaultConstructor(Func<ISpecimenBuilder> onCompose)
        {
            if (onCompose == null)
                throw new ArgumentNullException("onCompose");
        }
    }
}
