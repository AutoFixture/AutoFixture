using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Dsl
{
    public interface ISpecimenBuilderComposer
    {
        ISpecimenBuilder Compose();
    }
}
