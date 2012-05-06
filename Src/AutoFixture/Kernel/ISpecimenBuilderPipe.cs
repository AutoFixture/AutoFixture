using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
#warning Consider removing this interface, as it's most likely redundant
    public interface ISpecimenBuilderPipe
    {
        IEnumerable<ISpecimenBuilder> Pipe(IEnumerable<ISpecimenBuilder> builders);
    }
}
