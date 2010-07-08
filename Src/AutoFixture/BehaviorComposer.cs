using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Dsl;
using Ploeh.AutoFixture.Kernel;
using System.Linq.Expressions;

namespace Ploeh.AutoFixture
{
    public class BehaviorComposer<T> : ICustomizationComposer<T>
    {
        private readonly ICustomizationComposer<T> composer;
        private readonly IEnumerable<ISpecimenBuilderTransformation> behaviors;

        public BehaviorComposer(ICustomizationComposer<T> composer, IEnumerable<ISpecimenBuilderTransformation> behaviors)
            : this(composer, behaviors.ToArray())
        {
        }

        public BehaviorComposer(ICustomizationComposer<T> composer, params ISpecimenBuilderTransformation[] behaviors)
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

        public ICustomizationComposer<T> Composer
        {
            get { return this.composer; }
        }

        public BehaviorComposer<T> With(ICustomizationComposer<T> newComposer)
        {
            if (newComposer == null)
            {
                throw new ArgumentNullException("newComposer");
            }

            return new BehaviorComposer<T>(newComposer, this.Behaviors);
        }

        public BehaviorPostprocessComposer<T> With(IPostprocessComposer<T> newComposer)
        {
            if (newComposer == null)
            {
                throw new ArgumentNullException("newComposer");
            }

            return new BehaviorPostprocessComposer<T>(newComposer, this.Behaviors);
        }

        #region IFactoryComposer<T> Members

        public IPostprocessComposer<T> FromSeed(Func<T, T> factory)
        {
            return this.With(this.Composer.FromSeed(factory));
        }

        public IPostprocessComposer<T> FromFactory(Func<T> factory)
        {
            return this.With(this.Composer.FromFactory(factory));
        }

        public IPostprocessComposer<T> FromFactory<TInput>(Func<TInput, T> factory)
        {
            return this.With(this.Composer.FromFactory(factory));
        }

        public IPostprocessComposer<T> FromFactory<TInput1, TInput2>(Func<TInput1, TInput2, T> factory)
        {
            return this.With(this.Composer.FromFactory(factory));
        }

        public IPostprocessComposer<T> FromFactory<TInput1, TInput2, TInput3>(Func<TInput1, TInput2, TInput3, T> factory)
        {
            return this.With(this.Composer.FromFactory(factory));
        }

        public IPostprocessComposer<T> FromFactory<TInput1, TInput2, TInput3, TInput4>(Func<TInput1, TInput2, TInput3, TInput4, T> factory)
        {
            return this.With(this.Composer.FromFactory(factory));
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
    }
}
