using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Idioms;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class DelegatingBoundaryBehavior : IBoundaryBehavior
    {
        public DelegatingBoundaryBehavior()
        {
            this.OnExercise = a => a(new object());
            this.OnIsSatisfiedBy = e => false;
        }

        public Action<Action<object>> OnExercise { get; set; }

        public Func<Exception, bool> OnIsSatisfiedBy { get; set; }

        #region IBoundaryBehavior Members

        public void Exercise(Action<object> action)
        {
            this.OnExercise(action);
        }

        public bool IsSatisfiedBy(Exception exception)
        {
            return this.OnIsSatisfiedBy(exception);
        }

        public string Description { get; set; }

        #endregion
    }
}
