using System;
using System.Linq.Expressions;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Dsl
{
    public class RuleComposer<T> : IRuleComposer<T>
    {
        #region IFactoryComposer<T> Members

        public IPostprocessComposer<T> FromSeed(Func<T, T> factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }
            return new RuleComposer<T>();
        }

        public IPostprocessComposer<T> FromFactory(Func<T> factory)
        {
            throw new NotImplementedException();
        }

        public IPostprocessComposer<T> FromFactory<TInput>(Func<TInput, T> factory)
        {
            throw new NotImplementedException();
        }

        public IPostprocessComposer<T> FromFactory<TInput1, TInput2>(Func<TInput1, TInput2, T> factory)
        {
            throw new NotImplementedException();
        }

        public IPostprocessComposer<T> FromFactory<TInput1, TInput2, TInput3>(Func<TInput1, TInput2, TInput3, T> factory)
        {
            throw new NotImplementedException();
        }

        public IPostprocessComposer<T> FromFactory<TInput1, TInput2, TInput3, TInput4>(Func<TInput1, TInput2, TInput3, TInput4, T> factory)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IPostprocessComposer<T> Members

        public ISpecimenBuilder Compose()
        {
            return new FilteringSpecimenBuilder(new ModestConstructorInvoker(), new ExactTypeSpecification(typeof (T)));
        }

        public IPostprocessComposer<T> Do(Action<T> action)
        {
            throw new NotImplementedException();
        }

        public IPostprocessComposer<T> OmitAutoProperties()
        {
            throw new NotImplementedException();
        }

        public IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker)
        {
            throw new NotImplementedException();
        }

        public IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker, TProperty value)
        {
            throw new NotImplementedException();
        }

        public IPostprocessComposer<T> WithAutoProperties()
        {
            throw new NotImplementedException();
        }

        public IPostprocessComposer<T> Without<TProperty>(Expression<Func<T, TProperty>> propertyPicker)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
