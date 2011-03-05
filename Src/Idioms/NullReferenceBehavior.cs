using System;
using System.Globalization;

namespace Ploeh.AutoFixture.Idioms
{
    public class NullReferenceBehavior : ExceptionBoundaryBehavior
    {
        public override void Exercise(Action<object> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            action(null);
        }

        public override bool IsSatisfiedBy(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }

            return exception is ArgumentNullException;
        }

        public override string Description
        {
            get { return "null"; }
        }
    }
}