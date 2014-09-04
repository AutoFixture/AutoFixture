using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Dsl
{
    public class MatchComposer<T> : IMatchComposer<T>
    {
        private readonly ISpecimenBuilder builder;
        private IRequestSpecification matcher;
        private bool orExpression;

        public MatchComposer(ISpecimenBuilder builder)
        {
            this.builder = builder;
            this.matcher = new AnyTypeSpecification();
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

        public IMatchComposer<T> ByBaseType()
        {
            AddCondition(new BaseTypeSpecification(typeof(T)));
            return this;
        }

        public IMatchComposer<T> ByInterfaces()
        {
            AddCondition(new ImplementedInterfaceSpecification(typeof(T)));
            return this;
        }

        public IMatchComposer<T> ByExactType()
        {
            AddCondition(new ExactTypeSpecification(typeof(T)));
            return this;
        }

        public IMatchComposer<T> ByParameterName(string name)
        {
            AddCondition(new ParameterNameSpecification(name));
            return this;
        }

        public IMatchComposer<T> ByPropertyName(string name)
        {
            AddCondition(new PropertySpecification(typeof(T), name));
            return this;
        }

        public IMatchComposer<T> ByFieldName(string name)
        {
            AddCondition(new FieldSpecification(typeof(T), name));
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
