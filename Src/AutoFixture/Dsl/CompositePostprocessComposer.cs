using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Dsl
{
    public class CompositePostprocessComposer<T> : IPostprocessComposer<T>
    {
        private readonly IEnumerable<IPostprocessComposer<T>> composers;

        public CompositePostprocessComposer(IEnumerable<IPostprocessComposer<T>> composers)
            : this(composers.ToArray())
        {
        }

        public CompositePostprocessComposer(params IPostprocessComposer<T>[] composers)
        {
            if (composers == null)
            {
                throw new ArgumentNullException("composers");
            }

            this.composers = composers;
        }

        public IEnumerable<IPostprocessComposer<T>> Composers
        {
            get { return this.composers; }
        }

        #region IPostprocessComposer<T> Members

        public IPostprocessComposer<T> Do(Action<T> action)
        {
            return new CompositePostprocessComposer<T>(from c in this.composers
                                                       select c.Do(action));
        }

        public IPostprocessComposer<T> OmitAutoProperties()
        {
            return new CompositePostprocessComposer<T>(from c in this.composers
                                                       select c.OmitAutoProperties());
        }

        public IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker)
        {
            return new CompositePostprocessComposer<T>(from c in this.composers
                                                       select c.With(propertyPicker));
        }

        public IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker, TProperty value)
        {
            return new CompositePostprocessComposer<T>(from c in this.composers
                                                       select c.With(propertyPicker, value));
        }

        public IPostprocessComposer<T> WithAutoProperties()
        {
            return new CompositePostprocessComposer<T>(from c in this.composers
                                                       select c.WithAutoProperties());
        }

        public IPostprocessComposer<T> Without<TProperty>(Expression<Func<T, TProperty>> propertyPicker)
        {
            return new CompositePostprocessComposer<T>(from c in this.composers
                                                       select c.Without(propertyPicker));
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
