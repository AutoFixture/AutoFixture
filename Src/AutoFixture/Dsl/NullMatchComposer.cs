using System;
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

        IMatchComposer IMatchComposer.Or
        {
            get { return this; }
        }

        public IMatchComposer<T> Or
        {
            get { return this; }
        }

        public IMatchComposer<T> ByExactType()
        {
            this.ByExactType(typeof(T));
            return this;
        }

        public IMatchComposer ByExactType(Type targetType)
        {
            return this;
        }

        public IMatchComposer<T> ByBaseType()
        {
            this.ByBaseType(typeof(T));
            return this;
        }

        public IMatchComposer ByBaseType(Type targetType)
        {
            return this;
        }

        public IMatchComposer<T> ByInterfaces()
        {
            this.ByInterfaces(typeof(T));
            return this;
        }

        public IMatchComposer ByInterfaces(Type targetType)
        {
            return this;
        }

        public IMatchComposer<T> ByParameterName(string name)
        {
            this.ByParameterName(typeof(T), name);
            return this;
        }

        public IMatchComposer ByParameterName(Type targetType, string name)
        {
            return this;
        }

        public IMatchComposer<T> ByPropertyName(string name)
        {
            this.ByPropertyName(typeof(T), name);
            return this;
        }

        public IMatchComposer ByPropertyName(Type targetType, string name)
        {
            return this;
        }

        public IMatchComposer<T> ByFieldName(string name)
        {
            this.ByFieldName(typeof(T), name);
            return this;
        }

        public IMatchComposer ByFieldName(Type targetType, string name)
        {
            return this;
        }

        public object Create(object request, ISpecimenContext context)
        {
            return this.builder.Create(request, context);
        }
    }
}
