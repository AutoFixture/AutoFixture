using System;
using System.Globalization;

namespace Ploeh.AutoFixture.Idioms
{
    public class GuidBoundaryBehavior : IBoundaryBehavior
    {
        #region Implementation of IBoundaryBehavior

        public void Exercise(Action<object> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            action(Guid.Empty);
        }

        public bool IsSatisfiedBy(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }

            return exception is ArgumentException;
        }

        public string Description
        {
            get { return "empty Guid"; }
        }

        #endregion
    }
}