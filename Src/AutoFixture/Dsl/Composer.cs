using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Dsl
{
    public class Composer<T> : IStrategyComposer<T>
    {
        private readonly ISpecimenBuilder rootBuilder;

        public Composer()
            : this(new ModestConstructorInvoker())
        {
        }

        public Composer(ISpecimenBuilder rootBuilder)
        {
            if (rootBuilder == null)
            {
                throw new ArgumentNullException("rootBuilder");
            }
        
            this.rootBuilder = rootBuilder;
        }

        public ISpecimenBuilder RootBuilder
        {
            get { return this.rootBuilder; }
        }

        #region IFactoryComposer<T> Members

        public IPostprocessComposer<T> FromSeed(Func<T, T> factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            return new Composer<T>(new SeededFactory<T>(factory));
        }

        public IPostprocessComposer<T> FromFactory(Func<T> factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }
        
            return new Composer<T>(new SpecimenFactory<T>(factory));
        }

        public IPostprocessComposer<T> FromFactory<TInput>(Func<TInput, T> factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            return new Composer<T>(new SpecimenFactory<TInput, T>(factory));
        }

        #endregion

        #region ISpecimenBuilderComposer Members

        public ISpecimenBuilder Compose()
        {
            return new FilteringSpecimenBuilder(this.rootBuilder, new ExactTypeSpecification(typeof(T)));
        }

        #endregion
    }
}
