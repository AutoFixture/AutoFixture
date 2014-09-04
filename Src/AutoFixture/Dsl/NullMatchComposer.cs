using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Dsl
{
    public class NullMatchComposer<T> : IMatchComposer<T>
    {
        private readonly ISpecimenBuilder builder;

        public NullMatchComposer()
            : this(new CompositeSpecimenBuilder())
        {
        }

        public NullMatchComposer(ISpecimenBuilder builder)
        {
            this.builder = builder;
        }

        public ISpecimenBuilder Builder
        {
            get { return builder; }
        }

        public IMatchComposer<T> Or
        {
            get { return this; }
        }

        public IMatchComposer<T> ByBaseType()
        {
            return this;
        }

        public IMatchComposer<T> ByInterfaces()
        {
            return this;
        }

        public IMatchComposer<T> ByExactType()
        {
            return this;
        }

        public IMatchComposer<T> ByParameterName(string name)
        {
            return this;
        }

        public IMatchComposer<T> ByPropertyName(string name)
        {
            return this;
        }

        public IMatchComposer<T> ByFieldName(string name)
        {
            return this;
        }

        public object Create(object request, ISpecimenContext context)
        {
            return this.builder.Create(request, context);
        }
    }
}
