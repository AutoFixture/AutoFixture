using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    public interface ISpecimenCommand
    {
        void Execute(object specimen, ISpecimenContext context);
    }
}
