using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Ploeh.AutoFixture.Dsl
{
    public interface IPostprocessComposer<T> : ISpecimenBuilderComposer
    {
        IPostprocessComposer<T> Without<TProperty>(Expression<Func<T, TProperty>> propertyPicker);
    }
}
