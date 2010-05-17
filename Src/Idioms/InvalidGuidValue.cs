using System;
using System.Globalization;

namespace Ploeh.AutoFixture.Idioms
{
    public class InvalidGuidValue : IInvalidValue
    {
        #region Implementation of IInvalidValue<T>

        public Guid Value
        {
            get { return Guid.Empty; }
        }

        public void Assert(Action<object> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            action(this.Value);
        }

        public bool IsSatisfiedBy(Type exceptionType)
        {
            if (exceptionType == null)
            {
                throw new ArgumentNullException("exceptionType");
            }

            return exceptionType == typeof(ArgumentException);
        }

        public string Description
        {
            get { return "empty Guid"; }
        }

        #endregion
    }
}