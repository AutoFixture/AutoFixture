using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Dsl
{
    public class MatchComposer<T> : IMatchComposer<T>
    {
        private readonly ISpecimenBuilder builder;
        private IRequestSpecification matcher;

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

        public IMatchComposer<T> BaseType()
        {
            this.matcher = new BaseTypeSpecification(typeof(T));
            return this;
        }

        public IMatchComposer<T> ExactType()
        {
            this.matcher = new ExactTypeSpecification(typeof(T));
            return this;
        }
    }
}
