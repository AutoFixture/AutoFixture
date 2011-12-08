using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    public interface ISpecimenBuilderPipe
    {
        IEnumerable<ISpecimenBuilder> Pipe(IEnumerable<ISpecimenBuilder> builders);
    }
}
