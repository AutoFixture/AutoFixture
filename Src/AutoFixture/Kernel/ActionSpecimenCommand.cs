using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    public class ActionSpecimenCommand<T> : ISpecimenCommand
    {
        private readonly Action<T> action;

        public ActionSpecimenCommand(Action<T> action)
        {
            this.action = action;
        }

        public void Execute(object specimen, ISpecimenContext context)
        {
            this.action((T)specimen);
        }
    }
}
