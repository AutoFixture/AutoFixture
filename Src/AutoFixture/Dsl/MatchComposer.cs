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

        public object Create(object request, ISpecimenContext context)
        {
            return new FilteringSpecimenBuilder(this.builder, this.matcher)
                   .Create(request, context);
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
            this.matcher = AddCondition(new BaseTypeSpecification(typeof(T)));
            return this;
        }

        public IMatchComposer<T> ByInterfaces()
        {
            this.matcher = AddCondition(new ImplementedInterfaceSpecification(typeof(T)));
            return this;
        }

        public IMatchComposer<T> ByExactType()
        {
            this.matcher = AddCondition(new ExactTypeSpecification(typeof(T)));
            return this;
        }

        public IMatchComposer<T> ByParameterName(string name)
        {
            this.matcher = AddCondition(new ParameterNameSpecification(name));
            return this;
        }

        public IMatchComposer<T> ByPropertyName(string name)
        {
            this.matcher = AddCondition(new PropertyNameSpecification(name));
            return this;
        }

        public IMatchComposer<T> ByFieldName(string name)
        {
            this.matcher = AddCondition(new FieldNameSpecification(name));
            return this;
        }

        private IRequestSpecification AddCondition(IRequestSpecification condition)
        {
            return this.orExpression
                ? new OrRequestSpecification(this.matcher, condition)
                : condition;
        }
    }
}
