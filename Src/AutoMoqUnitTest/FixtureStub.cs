using System;
using System.Collections.Generic;
using AutoFixture.Dsl;
using AutoFixture.Kernel;

namespace AutoFixture.AutoMoq.UnitTest
{
    public class FixtureStub : IFixture
    {
        public bool OmitAutoProperties { get; set; }
        public int RepeatCount { get; set; }
        public IList<ISpecimenBuilderTransformation> Behaviors { get; set; } = new List<ISpecimenBuilderTransformation>();
        public IList<ISpecimenBuilder> Customizations { get; set; } = new List<ISpecimenBuilder>();
        public IList<ISpecimenBuilder> ResidueCollectors { get; set; } = new List<ISpecimenBuilder>();

        object ISpecimenBuilder.Create(object request, ISpecimenContext context) =>
            throw new NotSupportedException();

        ICustomizationComposer<T> IFixture.Build<T>() =>
            throw new NotSupportedException();

        IFixture IFixture.Customize(ICustomization customization) =>
            throw new NotSupportedException();

        void IFixture.Customize<T>(Func<ICustomizationComposer<T>, ISpecimenBuilder> composerTransformation) =>
            throw new NotSupportedException();
    }
}