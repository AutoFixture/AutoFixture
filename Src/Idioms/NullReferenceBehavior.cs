using System;
using System.Globalization;

namespace Ploeh.AutoFixture.Idioms
{
    public class NullReferenceBehavior : IBoundaryBehavior
    {
        #region Implementation of IBoundaryBehavior

        public void Exercise(Action<object> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            action(null);
        }

        public bool IsSatisfiedBy(Type exceptionType)
        {
            if (exceptionType == null)
            {
                throw new ArgumentNullException("exceptionType");
            }

            return exceptionType == typeof(ArgumentNullException);
        }

        public string Description
        {
            get { return "null reference"; }
        }

        #endregion
    }
}