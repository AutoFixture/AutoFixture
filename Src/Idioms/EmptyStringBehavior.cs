using System;
using System.Globalization;

namespace Ploeh.AutoFixture.Idioms
{
    public class EmptyStringBehavior : ExceptionBoundaryBehavior
    {
        public override void Exercise(Action<object> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            action(string.Empty);
        }

        public override bool IsSatisfiedBy(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }

            return exception is ArgumentException;
        }

        public override string Description
        {
            get { return "empty string"; }
        }
    }
}