using System;
using System.Linq.Expressions;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Dsl
{
    public interface IPostprocessComposer<T>
    {
        ISpecimenBuilder Compose();

        IPostprocessComposer<T> Do(Action<T> action);

        IPostprocessComposer<T> OmitAutoProperties();

        IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker);

        IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker, TProperty value);

        IPostprocessComposer<T> WithAutoProperties();

        IPostprocessComposer<T> Without<TProperty>(Expression<Func<T, TProperty>> propertyPicker);
    }
}
