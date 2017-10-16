using System;
using AutoFixture.Kernel;

namespace AutoFixtureUnitTest.Kernel
{
    public class DelegatingSpecimenCommand : ISpecimenCommand
    {
        public DelegatingSpecimenCommand()
        {
            this.OnExecute = (s, c) => { };
        }

        public void Execute(object specimen, ISpecimenContext context)
        {
            this.OnExecute(specimen, context);
        }

        internal Action<object, ISpecimenContext> OnExecute { get; set; }
    }
}
