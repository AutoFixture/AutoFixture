using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Ploeh.AutoFixture.Dsl
{
    public interface IPostprocessComposer<T> : ISpecimenBuilderComposer
    {
        IPostprocessComposer<T> Do(Action<T> action);

        IPostprocessComposer<T> OmitAutoProperties();

        IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker);

        IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker, TProperty value);

        IPostprocessComposer<T> WithAutoProperties();

        IPostprocessComposer<T> Without<TProperty>(Expression<Func<T, TProperty>> propertyPicker);
    }
}
