#nullable enable
using System;
using AutoFixture.Kernel;

namespace AutoFixture.Xunit3.Internal
{
    internal class FrozenValueCustomization : ICustomization
    {
        private readonly IRequestSpecification specification;
        private readonly object? value;

        public FrozenValueCustomization(IRequestSpecification specification, object? value)
        {
            this.specification = specification ?? throw new ArgumentNullException(nameof(specification));
            this.value = value;
        }

        public void Customize(IFixture fixture)
        {
            var builder = new FilteringSpecimenBuilder(
                builder: new FixedBuilder(this.value),
                specification: this.specification);

            fixture.Customizations.Insert(0, builder);
        }
    }
}