using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Dsl;

namespace Ploeh.AutoFixture
{
    public interface IComposableSpecimenFactory<T> : ISpecimenFactory<T>, IStrategyComposer<T>
    {
    }
}
