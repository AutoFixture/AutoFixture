using System;
using Ploeh.AutoFixture.Dsl;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    public class FreezeOnMatchCustomization<T> : ICustomization
    {
        private readonly Type targetType;
        private readonly Func<IMatchComposer<T>, IMatchComposer> criteria;
        private IFixture fixture;
        private IMatchComposer match;

        public FreezeOnMatchCustomization()
            : this(match => match.ByExactType())
        {
        }

        public FreezeOnMatchCustomization(
            Func<IMatchComposer, IMatchComposer<T>> criteria)
        {
            this.targetType = typeof(T);
            this.criteria = criteria;
        }

        public FreezeOnMatchCustomization(
            Func<IMatchComposer<T>, IMatchComposer> criteria)
        {
            this.targetType = typeof(T);
            this.criteria = criteria;
        }

        public void Customize(IFixture fixture)
        {
            Require.IsNotNull(fixture, "fixture");

            this.fixture = fixture;

            ApplyMatchingCriteria();
            FreezeTypeForMatchingRequests();
        }

        private void ApplyMatchingCriteria()
        {
            this.match = this.criteria(new MatchComposer<T>(FreezeTargetType()));
        }

        private ISpecimenBuilder FreezeTargetType()
        {
            var context = new SpecimenContext(this.fixture);
            var specimen = context.Resolve(this.targetType);
            return new FixedBuilder(specimen);
        }

        private void FreezeTypeForMatchingRequests()
        {
            this.fixture.Customize<T>(c => this.match);
        }
    }
}
