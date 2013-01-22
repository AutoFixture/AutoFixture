using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    public class ActionSpecimenCommand<T> : ISpecimenCommand
    {
        private readonly Action<T, ISpecimenContext> action;

        public ActionSpecimenCommand(Action<T> action)
        {
            this.action = (s, c) => action(s);
        }

        public ActionSpecimenCommand(Action<T, ISpecimenContext> action)
        {
            this.action = action;
        }

        public void Execute(object specimen, ISpecimenContext context)
        {
            this.action((T)specimen, context);
        }
    }
}
