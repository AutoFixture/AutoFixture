using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Idioms
{
    public class CompositeBoundaryConventionFactory : IBoundaryConventionFactory
    {
        private readonly IEnumerable<IBoundaryConventionFactory> factories;

        public CompositeBoundaryConventionFactory(IEnumerable<IBoundaryConventionFactory> factories)
            : this(factories.ToArray())
        {
        }

        public CompositeBoundaryConventionFactory(params IBoundaryConventionFactory[] factories)
        {
            if (factories == null)
            {
                throw new ArgumentNullException("factories");
            }

            this.factories = factories;
        }

        public IEnumerable<IBoundaryConventionFactory> Factories
        {
            get { return this.factories; }
        }

        #region IBoundaryConventionFactory Members

        public IBoundaryConvention GetConvention(Type type)
        {
            return new CompositeBoundaryConvention(from f in this.factories
                                                   select f.GetConvention(type));
        }

        #endregion
    }
}
