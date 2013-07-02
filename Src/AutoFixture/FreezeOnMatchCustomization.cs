using System;
using System.Collections.Generic;
using Ploeh.AutoFixture.Dsl;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    public class FreezeOnMatchCustomization<T> : ICustomization
    {
        private readonly Type targetType;
        private readonly Matching matchBy;
        private readonly string[] targetNames;
        private IFixture fixture;
        private IMatchComposer<T> filter;

        public FreezeOnMatchCustomization(
            Matching matchBy = Matching.ExactType,
            params string[] targetNames)
        {
            this.targetType = typeof(T);
            this.matchBy = matchBy;
            this.targetNames = targetNames;
        }

        public Type TargetType
        {
            get { return this.targetType; }
        }

        public Matching MatchBy
        {
            get { return this.matchBy; }
        }

        public IEnumerable<string> TargetNames
        {
            get { return this.targetNames; }
        }

        public void Customize(IFixture fixture)
        {
            Require.IsNotNull(fixture, "fixture");

            this.fixture = fixture;
            this.filter = new MatchComposer<T>(FreezeTargetType());

            MatchByType();
            MatchByName();
            FreezeTypeForMatchingRequests();
        }

        private ISpecimenBuilder FreezeTargetType()
        {
            var context = new SpecimenContext(this.fixture);
            var specimen = context.Resolve(this.targetType);
            return new FixedBuilder(specimen);
        }

        private void MatchByType()
        {
            MatchByExactType();
            MatchByBaseType();
            MatchByImplementedInterfaces();
        }

        private void MatchByName()
        {
            MatchByPropertyName();
            MatchByParameterName();
            MatchByFieldName();
        }

        private void MatchByExactType()
        {
            if (ShouldMatchBy(Matching.ExactType))
            {
                filter = filter.Or.ByExactType();
            }
        }

        private void MatchByBaseType()
        {
            if (ShouldMatchBy(Matching.BaseType))
            {
                filter = filter.Or.ByBaseType();
            }
        }

        private void MatchByImplementedInterfaces()
        {
            if (ShouldMatchBy(Matching.ImplementedInterfaces))
            {
                filter = filter.Or.ByInterfaces();
            }
        }

        private void MatchByPropertyName()
        {
            if (ShouldMatchBy(Matching.PropertyName))
            {
                foreach (var name in this.targetNames)
                {
                    filter = filter.Or.ByPropertyName(name);
                }
            }
        }

        private void MatchByParameterName()
        {
            if (ShouldMatchBy(Matching.ParameterName))
            {
                foreach (var name in this.targetNames)
                {
                    filter = filter.Or.ByParameterName(name);
                }
            }
        }

        private void MatchByFieldName()
        {
            if (ShouldMatchBy(Matching.FieldName))
            {
                foreach (var name in this.targetNames)
                {
                    filter = filter.Or.ByFieldName(name);
                }
            }
        }

        private bool ShouldMatchBy(Matching criteria)
        {
            return this.matchBy.HasFlag(criteria);
        }

        private void FreezeTypeForMatchingRequests()
        {
            this.fixture.Customize<T>(c => filter);
        }
    }
}
