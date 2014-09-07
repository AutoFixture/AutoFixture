using System;
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
            this.matcher = new FalseRequestSpecification();
        }

        public ISpecimenBuilder Builder
        {
            get { return builder; }
        }

        IMatchComposer IMatchComposer.Or
        {
            get { return this.Or; }
        }

        public IMatchComposer<T> Or
        {
            get
            {
                this.orExpression = true;
                return this;
            }
        }

        public IMatchComposer<T> ByExactType()
        {
            this.ByExactType(typeof(T));
            return this;
        }

        public IMatchComposer ByExactType(Type targetType)
        {
            AddCondition(
                new OrRequestSpecification(
                    new ExactTypeSpecification(typeof(T)),
                    new SeedRequestSpecification(typeof(T))));
            return this;
        }

        public IMatchComposer<T> ByBaseType()
        {
            this.ByBaseType(typeof(T));
            return this;
        }

        public IMatchComposer ByBaseType(Type targetType)
        {
            AddCondition(new BaseTypeSpecification(targetType));
            return this;
        }

        public IMatchComposer<T> ByInterfaces()
        {
            this.ByInterfaces(typeof(T));
            return this;
        }

        public IMatchComposer ByInterfaces(Type targetType)
        {
            AddCondition(new ImplementedInterfaceSpecification(typeof(T)));
            return this;
        }

        public IMatchComposer<T> ByParameterName(string name)
        {
            this.ByParameterName(typeof(T), name);
            return this;
        }

        public IMatchComposer ByParameterName(Type targetType, string name)
        {
            AddCondition(new ParameterSpecification(typeof(T), name));
            return this;
        }

        public IMatchComposer<T> ByPropertyName(string name)
        {
            this.ByPropertyName(typeof(T), name);
            return this;
        }

        public IMatchComposer ByPropertyName(Type targetType, string name)
        {
            AddCondition(new PropertySpecification(typeof(T), name));
            return this;
        }

        public IMatchComposer<T> ByFieldName(string name)
        {
            this.ByFieldName(typeof(T), name);
            return this;
        }

        public IMatchComposer ByFieldName(Type targetType, string name)
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
