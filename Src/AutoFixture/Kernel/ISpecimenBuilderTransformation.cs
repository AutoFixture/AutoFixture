using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    public interface ISpecimenBuilderTransformation
    {
        ISpecimenBuilder Transform(ISpecimenBuilder builder);
    }
}
