using System;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
#pragma warning disable 618
    internal class DelegatingSpecifiedSpecimenCommand<T> : DelegatingRequestSpecification, ISpecifiedSpecimenCommand<T>
    {
        public DelegatingSpecifiedSpecimenCommand()
        {
            this.OnExecute = (s, c) => { };
        }

        public void Execute(T specimen, ISpecimenContext container)
        {
            this.OnExecute(specimen, container);
        }

        internal Action<T, ISpecimenContext> OnExecute { get; set; }
    }
#pragma warning restore 618
}
