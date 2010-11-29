using System;
using System.Globalization;

namespace Ploeh.AutoFixture.Idioms
{
    public class EmptyStringBehavior : IBoundaryBehavior
    {
        #region Implementation of IBoundaryBehavior

        public void Exercise(Action<object> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            action(string.Empty);
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
            get { return "empty string"; }
        }

        #endregion
    }
}