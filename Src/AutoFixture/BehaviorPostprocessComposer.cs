using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Dsl;
using System.Linq.Expressions;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    public class BehaviorPostprocessComposer<T> : IPostprocessComposer<T>
    {
        private readonly IPostprocessComposer<T> composer;
        private readonly IEnumerable<ISpecimenBuilderTransformation> behaviors;

        public BehaviorPostprocessComposer(IPostprocessComposer<T> composer, IEnumerable<ISpecimenBuilderTransformation> behaviors)
            : this(composer, behaviors.ToArray())
        {
        }

        public BehaviorPostprocessComposer(IPostprocessComposer<T> composer, params ISpecimenBuilderTransformation[] behaviors)
        {
            if (composer == null)
            {
                throw new ArgumentNullException("composer");
            }
            if (behaviors == null)
            {
                throw new ArgumentNullException("behaviors");
            }

            this.composer = composer;
            this.behaviors = behaviors;
        }

        public IEnumerable<ISpecimenBuilderTransformation> Behaviors
        {
            get { return this.behaviors; }
        }

        public IPostprocessComposer<T> Composer
        {
            get { return this.composer; }
        }

        public BehaviorPostprocessComposer<T> With(IPostprocessComposer<T> newComposer)
        {
            if (newComposer == null)
            {
                throw new ArgumentNullException("newComposer");
            }

            return new BehaviorPostprocessComposer<T>(newComposer, this.Behaviors);
        }

        #region IPostprocessComposer<T> Members

        public IPostprocessComposer<T> Do(Action<T> action)
        {
            return this.With(this.Composer.Do(action));
        }

        public IPostprocessComposer<T> OmitAutoProperties()
        {
            return this.With(this.Composer.OmitAutoProperties());
        }

        public IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker)
        {
            return this.With(this.Composer.With(propertyPicker));
        }

        public IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker, TProperty value)
        {
            return this.With(this.Composer.With(propertyPicker, value));
        }

        public IPostprocessComposer<T> WithAutoProperties()
        {
            return this.With(this.Composer.WithAutoProperties());
        }

        public IPostprocessComposer<T> Without<TProperty>(Expression<Func<T, TProperty>> propertyPicker)
        {
            return this.With(this.Composer.Without(propertyPicker));
        }

        #endregion

        #region ISpecimenBuilderComposer Members

        public ISpecimenBuilder Compose()
        {
            return this.Behaviors.Aggregate(
                this.Composer.Compose(),
                (builder, behavior) => 
                    behavior.Transform(builder));
        }

        #endregion
    }
}
