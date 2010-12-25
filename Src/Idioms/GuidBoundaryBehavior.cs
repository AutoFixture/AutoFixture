using System;
using System.Globalization;

namespace Ploeh.AutoFixture.Idioms
{
    public class GuidBoundaryBehavior : ExceptionBoundaryBehavior
    {
        public override void Exercise(Action<object> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            action(Guid.Empty);
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
            get { return "empty Guid"; }
        }
    }
}