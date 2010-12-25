using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Idioms
{
    public class CompositeBoundaryConvention : IBoundaryConvention
    {
        private readonly IEnumerable<IBoundaryConvention> conventions;

        public CompositeBoundaryConvention(IEnumerable<IBoundaryConvention> conventions)
            : this(conventions.ToArray())
        {
        }

        public CompositeBoundaryConvention(params IBoundaryConvention[] conventions)
        {
            if (conventions == null)
            {
                throw new ArgumentNullException("conventions");
            }

            this.conventions = conventions;
        }

        public IEnumerable<IBoundaryConvention> Conventions
        {
            get { return conventions; }
        }

        #region IBoundaryConvention Members

        public IEnumerable<ExceptionBoundaryBehavior> CreateBoundaryBehaviors(Type type)
        {
            return from c in this.conventions
                   from b in c.CreateBoundaryBehaviors(type)
                   select b;
        }

        #endregion
    }
}
