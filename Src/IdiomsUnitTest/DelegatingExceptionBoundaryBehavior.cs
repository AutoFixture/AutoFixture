using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Idioms;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class DelegatingExceptionBoundaryBehavior : ExceptionBoundaryBehavior
    {
        public DelegatingExceptionBoundaryBehavior()
        {
            this.OnExercise = a => a(new object());
            this.OnIsSatisfiedBy = e => false;
            this.WritableDescription = string.Empty;
        }

        public Action<Action<object>> OnExercise { get; set; }

        public Func<Exception, bool> OnIsSatisfiedBy { get; set; }

        public override void Exercise(Action<object> action)
        {
            this.OnExercise(action);
        }

        public override bool IsSatisfiedBy(Exception exception)
        {
            return this.OnIsSatisfiedBy(exception);
        }

        public override string Description
        {
            get { return this.WritableDescription; }
        }

        public string WritableDescription { get; set; }
    }
}
