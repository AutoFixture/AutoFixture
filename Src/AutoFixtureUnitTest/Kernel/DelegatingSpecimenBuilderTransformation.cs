using System;
using AutoFixture.Kernel;

namespace AutoFixtureUnitTest.Kernel
{
    public class DelegatingSpecimenBuilderTransformation : ISpecimenBuilderTransformation
    {
        public DelegatingSpecimenBuilderTransformation()
        {
            this.OnTransform = b => b;
        }

        public ISpecimenBuilder Transform(ISpecimenBuilder builder)
        {
            return this.OnTransform(builder);
        }

        internal Func<ISpecimenBuilder, ISpecimenBuilder> OnTransform { get; set; }
    }
}
