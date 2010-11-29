using System;

namespace Ploeh.AutoFixture.Idioms
{
    public class GuidBoundaryConventionFactory : IBoundaryConventionFactory
    {
        #region Implementation of IBoundaryConventionFactory

        public IBoundaryConvention GetConvention(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (type != typeof(Guid))
            {
                return new NullBoundaryConvention();
            }

            return new GuidBoundaryConvention();
        }

        #endregion
    }
}