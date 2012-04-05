using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    public interface ISpecimenBuilderNode : ISpecimenBuilder, IEnumerable<ISpecimenBuilder>
    {
        ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders);
    }
}
