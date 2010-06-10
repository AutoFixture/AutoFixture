using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    public class UnspecifiedSpecimenCommand<T> : ISpecifiedSpecimenCommand<T>
    {
        private readonly Action<T> action;

        public UnspecifiedSpecimenCommand(Action<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            this.action = action;
        }

        public Action<T> Action
        {
            get { return this.action; }
        }

        #region ISpecifiedSpecimenCommand<T> Members

        public void Execute(T specimen, ISpecimenContainer container)
        {
            this.Action(specimen);
        }

        #endregion

        #region IRequestSpecification Members

        public bool IsSatisfiedBy(object request)
        {
            return false;
        }

        #endregion
    }
}
