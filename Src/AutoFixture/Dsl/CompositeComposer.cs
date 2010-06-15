using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;
using System.Linq.Expressions;

namespace Ploeh.AutoFixture.Dsl
{
    public class CompositeComposer<T> : ICustomizationComposer<T>
    {
        private readonly IEnumerable<ICustomizationComposer<T>> composers;

        public CompositeComposer(IEnumerable<ICustomizationComposer<T>> composers)
            : this(composers.ToArray())
        {
        }

        public CompositeComposer(params ICustomizationComposer<T>[] composers)
        {
            if (composers == null)
            {
                throw new ArgumentNullException("composers");
            }

            this.composers = composers;
        }

        public IEnumerable<ICustomizationComposer<T>> Composers
        {
            get { return this.composers; }
        }

        #region IFactoryComposer<T> Members

        public IPostprocessComposer<T> FromSeed(Func<T, T> factory)
        {
            return new CompositePostprocessComposer<T>(from c in this.composers
                                                       select c.FromSeed(factory));
        }

        public IPostprocessComposer<T> FromFactory(Func<T> factory)
        {
            return new CompositePostprocessComposer<T>(from c in this.composers
                                                       select c.FromFactory(factory));
        }

        public IPostprocessComposer<T> FromFactory<TInput>(Func<TInput, T> factory)
        {
            return new CompositePostprocessComposer<T>(from c in this.composers
                                                       select c.FromFactory(factory));
        }

        public IPostprocessComposer<T> FromFactory<TInput1, TInput2>(Func<TInput1, TInput2, T> factory)
        {
            return new CompositePostprocessComposer<T>(from c in this.composers
                                                       select c.FromFactory(factory));
        }

        public IPostprocessComposer<T> FromFactory<TInput1, TInput2, TInput3>(Func<TInput1, TInput2, TInput3, T> factory)
        {
            return new CompositePostprocessComposer<T>(from c in this.composers
                                                       select c.FromFactory(factory));
        }

        public IPostprocessComposer<T> FromFactory<TInput1, TInput2, TInput3, TInput4>(Func<TInput1, TInput2, TInput3, TInput4, T> factory)
        {
            return new CompositePostprocessComposer<T>(from c in this.composers
                                                       select c.FromFactory(factory));
        }

        #endregion

        #region IPostprocessComposer<T> Members

        public IPostprocessComposer<T> Do(Action<T> action)
        {
            return new CompositePostprocessComposer<T>(this.composers.ToArray()).Do(action);
        }

        public IPostprocessComposer<T> OmitAutoProperties()
        {
            return new CompositePostprocessComposer<T>(this.composers.ToArray()).OmitAutoProperties();
        }

        public IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker)
        {
            return new CompositePostprocessComposer<T>(this.composers.ToArray()).With(propertyPicker);
        }

        public IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker, TProperty value)
        {
            return new CompositePostprocessComposer<T>(this.composers.ToArray()).With(propertyPicker, value);
        }

        public IPostprocessComposer<T> WithAutoProperties()
        {
            return new CompositePostprocessComposer<T>(this.composers.ToArray()).WithAutoProperties();
        }

        public IPostprocessComposer<T> Without<TProperty>(Expression<Func<T, TProperty>> propertyPicker)
        {
            return new CompositePostprocessComposer<T>(this.composers.ToArray()).Without(propertyPicker);
        }

        #endregion

        #region ISpecimenBuilderComposer Members

        public ISpecimenBuilder Compose()
        {
            return new CompositeSpecimenBuilder(from c in this.composers
                                                select c.Compose());
        }

        #endregion
    }
}
