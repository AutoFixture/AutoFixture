using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Dsl
{
    public class Composer<T> : IStrategyComposer<T>
    {
        private readonly ISpecimenBuilder factory;

        public Composer()
            : this(new ModestConstructorInvoker())
        {
        }

        public Composer(ISpecimenBuilder factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }
        
            this.factory = factory;
        }

        public ISpecimenBuilder Factory
        {
            get { return this.factory; }
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

        public IPostprocessComposer<T> FromFactory<TInput1, TInput2>(Func<TInput1, TInput2, T> factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            return new Composer<T>(new SpecimenFactory<TInput1, TInput2, T>(factory));
        }

        #endregion

        #region ISpecimenBuilderComposer Members

        public ISpecimenBuilder Compose()
        {
            return new FilteringSpecimenBuilder(this.factory, new ExactTypeSpecification(typeof(T)));
        }

        #endregion
    }
}
