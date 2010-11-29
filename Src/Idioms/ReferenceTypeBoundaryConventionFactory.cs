using System;

namespace Ploeh.AutoFixture.Idioms
{
    public class ReferenceTypeBoundaryConventionFactory : IBoundaryConventionFactory
    {
        #region Implementation of IBoundaryConventionFactory

        public IBoundaryConvention GetConvention(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (type.IsClass)
            {
                return new ReferenceTypeBoundaryConvention();
            }

            return new NullBoundaryConvention();
        }

        #endregion
    }
}