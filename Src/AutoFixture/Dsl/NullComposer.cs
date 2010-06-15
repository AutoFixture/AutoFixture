using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;
using System.Linq.Expressions;

namespace Ploeh.AutoFixture.Dsl
{
    public class NullComposer<T> : ICustomizationComposer<T>
    {
        private readonly Func<ISpecimenBuilder> compose;

        public NullComposer()
            : this(new CompositeSpecimenBuilder())
        {
        }

        public NullComposer(ISpecimenBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }

            this.compose = () => builder;
        }

        public NullComposer(Func<ISpecimenBuilder> factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            this.compose = factory;
        }

        #region IFactoryComposer<T> Members

        public IPostprocessComposer<T> FromSeed(Func<T, T> factory)
        {
            return this;
        }

        public IPostprocessComposer<T> FromFactory(Func<T> factory)
        {
            return this;
        }

        public IPostprocessComposer<T> FromFactory<TInput>(Func<TInput, T> factory)
        {
            return this;
        }

        public IPostprocessComposer<T> FromFactory<TInput1, TInput2>(Func<TInput1, TInput2, T> factory)
        {
            return this;
        }

        public IPostprocessComposer<T> FromFactory<TInput1, TInput2, TInput3>(Func<TInput1, TInput2, TInput3, T> factory)
        {
            return this;
        }

        public IPostprocessComposer<T> FromFactory<TInput1, TInput2, TInput3, TInput4>(Func<TInput1, TInput2, TInput3, TInput4, T> factory)
        {
            return this;
        }

        #endregion

        #region IPostprocessComposer<T> Members

        public IPostprocessComposer<T> Do(Action<T> action)
        {
            return this;
        }

        public IPostprocessComposer<T> OmitAutoProperties()
        {
            return this;
        }

        public IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker)
        {
            return this;
        }

        public IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker, TProperty value)
        {
            return this;
        }

        public IPostprocessComposer<T> WithAutoProperties()
        {
            return this;
        }

        public IPostprocessComposer<T> Without<TProperty>(Expression<Func<T, TProperty>> propertyPicker)
        {
            return this;
        }

        #endregion

        #region ISpecimenBuilderComposer Members

        public ISpecimenBuilder Compose()
        {
            return this.compose();
        }

        #endregion
    }
}
