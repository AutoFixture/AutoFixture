using System;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Dsl
{
    public class MatchComposer<T> : IMatchComposer<T>
    {
        private readonly ISpecimenBuilder builder;
        private readonly Type targetType;
        private IRequestSpecification matcher;
        private bool orExpression;

        public MatchComposer(ISpecimenBuilder builder)
        {
            this.builder = builder;
            this.targetType = typeof(T);
            this.matcher = new FalseRequestSpecification();
        }

        public ISpecimenBuilder Builder
        {
            get { return builder; }
        }

        public IMatchComposer<T> Or
        {
            get
            {
                this.orExpression = true;
                return this;
            }
        }

        public IMatchComposer<T> ByDirectBaseType()
        {
            AddCondition(new DirectBaseTypeSpecification(this.targetType));
            return this;
        }

        public IMatchComposer<T> ByInterfaces()
        {
            AddCondition(new ImplementedInterfaceSpecification(this.targetType));
            return this;
        }

        public IMatchComposer<T> ByExactType()
        {
            AddCondition(
                new OrRequestSpecification(
                    new ExactTypeSpecification(this.targetType),
                    new SeedRequestSpecification(this.targetType)));
            return this;
        }

        public IMatchComposer<T> ByParameterName(string name)
        {
            AddCondition(new ParameterSpecification(this.targetType, name));
            return this;
        }

        public IMatchComposer<T> ByPropertyName(string name)
        {
            AddCondition(new PropertySpecification(this.targetType, name));
            return this;
        }

        public IMatchComposer<T> ByFieldName(string name)
        {
            AddCondition(new FieldSpecification(this.targetType, name));
            return this;
        }

        public object Create(object request, ISpecimenContext context)
        {
            var filter = new FilteringSpecimenBuilder(this.builder, this.matcher);
            return filter.Create(request, context);
        }

        private void AddCondition(IRequestSpecification condition)
        {
            this.matcher = this.orExpression
                ? new OrRequestSpecification(this.matcher, condition)
                : condition;
        }
    }
}
