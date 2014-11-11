using System;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    public class FreezeOnMatchCustomization : ICustomization
    {
        private readonly Type targetType;
        private readonly IRequestSpecification matcher;

        public FreezeOnMatchCustomization(Type targetType)
            : this(targetType, new ExactTypeSpecification(targetType))
        {
        }

        public FreezeOnMatchCustomization(
            Type targetType,
            IRequestSpecification matcher)
        {
            if (targetType == null)
            {
                throw new ArgumentNullException("targetType");
            }

            if (matcher == null)
            {
                throw new ArgumentNullException("matcher");
            }

            this.targetType = targetType;
            this.matcher = matcher;
        }

        public Type TargetType
        {
            get { return this.targetType; }
        }

        public IRequestSpecification Matcher
        {
            get { return this.matcher; }
        }

        public void Customize(IFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }

            FreezeTypeForMatchingRequests(fixture);
        }

        private void FreezeTypeForMatchingRequests(IFixture fixture)
        {
            fixture.Customizations.Add(
                new FilteringSpecimenBuilder(
                    FreezeTargetType(fixture),
                    this.matcher));
        }

        private ISpecimenBuilder FreezeTargetType(IFixture fixture)
        {
            var context = new SpecimenContext(fixture);
            var specimen = context.Resolve(this.targetType);
            return new FixedBuilder(specimen);
        }
    }
}
