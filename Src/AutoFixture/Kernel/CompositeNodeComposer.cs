using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Dsl;

namespace Ploeh.AutoFixture.Kernel
{
    public class CompositeNodeComposer<T> : ICustomizationComposer<T>
    {
        public IPostprocessComposer<T> FromSeed(Func<T, T> factory)
        {
            throw new NotImplementedException();
        }

        public IPostprocessComposer<T> FromFactory(ISpecimenBuilder factory)
        {
            throw new NotImplementedException();
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

        public ISpecimenBuilder Compose()
        {
            throw new NotImplementedException();
        }

        public IPostprocessComposer<T> Do(Action<T> action)
        {
            throw new NotImplementedException();
        }

        public IPostprocessComposer<T> OmitAutoProperties()
        {
            throw new NotImplementedException();
        }

        public IPostprocessComposer<T> With<TProperty>(System.Linq.Expressions.Expression<Func<T, TProperty>> propertyPicker)
        {
            throw new NotImplementedException();
        }

        public IPostprocessComposer<T> With<TProperty>(System.Linq.Expressions.Expression<Func<T, TProperty>> propertyPicker, TProperty value)
        {
            throw new NotImplementedException();
        }

        public IPostprocessComposer<T> WithAutoProperties()
        {
            throw new NotImplementedException();
        }

        public IPostprocessComposer<T> Without<TProperty>(System.Linq.Expressions.Expression<Func<T, TProperty>> propertyPicker)
        {
            throw new NotImplementedException();
        }
    }
}
