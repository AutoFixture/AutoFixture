using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;
using System.Linq.Expressions;

namespace Ploeh.AutoFixture.Dsl
{
    public class Composer<T> : IStrategyComposer<T>
    {
        private readonly ISpecimenBuilder factory;
        private readonly IEnumerable<ISpecifiedSpecimenCommand<T>> postprocessors;
        private readonly bool enableAutoProperties;

        public Composer()
            : this(new ModestConstructorInvoker(), Enumerable.Empty<ISpecifiedSpecimenCommand<T>>(), false)
        {
        }

        public Composer(ISpecimenBuilder factory, IEnumerable<ISpecifiedSpecimenCommand<T>> postprocessors, bool enableAutoProperties)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }
            if (postprocessors == null)
            {
                throw new ArgumentNullException("postprocessors");
            }

            this.factory = factory;
            this.postprocessors = postprocessors.ToList();
            this.enableAutoProperties = enableAutoProperties;
        }

        public bool EnableAutoProperties
        {
            get { return this.enableAutoProperties; }
        }

        public ISpecimenBuilder Factory
        {
            get { return this.factory; }
        }

        public IEnumerable<ISpecifiedSpecimenCommand<T>> Postprocessors
        {
            get { return this.postprocessors; }
        }

        public Composer<T> WithAutoProperties(bool enable)
        {
            return new Composer<T>(this.Factory, this.Postprocessors, enable);
        }

        public Composer<T> WithFactory(ISpecimenBuilder factory)
        {
            return new Composer<T>(factory, this.postprocessors, this.EnableAutoProperties);
        }

        public Composer<T> WithPostprocessor(ISpecifiedSpecimenCommand<T> postprocessor)
        {
            if (postprocessor == null)
            {
                throw new ArgumentNullException("postprocessor");
            }

            return new Composer<T>(this.Factory, this.postprocessors.Concat(new[] { postprocessor }), this.EnableAutoProperties);
        }

        #region IFactoryComposer<T> Members

        public IPostprocessComposer<T> FromSeed(Func<T, T> factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            return this.WithFactory(new SeededFactory<T>(factory));
        }

        public IPostprocessComposer<T> FromFactory(Func<T> factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            return this.WithFactory(new SpecimenFactory<T>(factory));
        }

        public IPostprocessComposer<T> FromFactory<TInput>(Func<TInput, T> factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            return this.WithFactory(new SpecimenFactory<TInput, T>(factory));
        }

        public IPostprocessComposer<T> FromFactory<TInput1, TInput2>(Func<TInput1, TInput2, T> factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            return this.WithFactory(new SpecimenFactory<TInput1, TInput2, T>(factory));
        }

        public IPostprocessComposer<T> FromFactory<TInput1, TInput2, TInput3>(Func<TInput1, TInput2, TInput3, T> factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            return this.WithFactory(new SpecimenFactory<TInput1, TInput2, TInput3, T>(factory));
        }

        public IPostprocessComposer<T> FromFactory<TInput1, TInput2, TInput3, TInput4>(Func<TInput1, TInput2, TInput3, TInput4, T> factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            return this.WithFactory(new SpecimenFactory<TInput1, TInput2, TInput3, TInput4, T>(factory));
        }

        #endregion

        #region IPostprocessComposer<T> Members

        public IPostprocessComposer<T> Do(Action<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            var postprocessor = new UnspecifiedSpecimenCommand<T>(action);
            return this.WithPostprocessor(postprocessor);
        }

        public IPostprocessComposer<T> OmitAutoProperties()
        {
            return this.WithAutoProperties(false);
        }

        public IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker)
        {
            if (propertyPicker == null)
            {
                throw new ArgumentNullException("propertyPicker");
            }

            var postprocessor = new BindingCommand<T, TProperty>(propertyPicker);
            return this.WithPostprocessor(postprocessor);
        }

        public IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker, TProperty value)
        {
            if (propertyPicker == null)
            {
                throw new ArgumentNullException("propertyPicker");
            }

            var postprocessor = new BindingCommand<T, TProperty>(propertyPicker, value);
            return this.WithPostprocessor(postprocessor);
        }

        public IPostprocessComposer<T> WithAutoProperties()
        {
            return this.WithAutoProperties(true);
        }

        public IPostprocessComposer<T> Without<TProperty>(Expression<Func<T, TProperty>> propertyPicker)
        {
            if (propertyPicker == null)
            {
                throw new ArgumentNullException("propertyPicker");
            }

            var postprocessor = new SpecifiedNullCommand<T, TProperty>(propertyPicker);
            return this.WithPostprocessor(postprocessor);
        }

        #endregion

        #region ISpecimenBuilderComposer Members

        public ISpecimenBuilder Compose()
        {
            var builder = this.Factory;
            foreach (var p in this.Postprocessors)
            {
                builder = new Postprocessor<T>(builder, p.Execute);
            }

            if (this.EnableAutoProperties)
            {
                var reservedProperties =
                    new InverseRequestSpecification(
                        new OrRequestSpecification(
                            this.Postprocessors.Cast<IRequestSpecification>().Concat(new[] { new FalseRequestSpecification() })));
                builder = new Postprocessor<T>(
                    builder,
                    new AutoPropertiesCommand<T>(reservedProperties).Execute,
                    new AnyTypeSpecification());
            }

            return new FilteringSpecimenBuilder(builder, new ExactTypeSpecification(typeof(T)));
        }

        #endregion
    }
}
