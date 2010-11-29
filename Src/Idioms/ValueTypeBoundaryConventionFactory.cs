using System;

namespace Ploeh.AutoFixture.Idioms
{
    public class ValueTypeBoundaryConventionFactory : IBoundaryConventionFactory
    {
        #region Implementation of IBoundaryConventionFactory

        public IBoundaryConvention GetConvention(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (type.IsValueType)
            {
                return new ValueTypeBoundaryConvention();
            }

            return new NullBoundaryConvention();
        }

        #endregion
    }
}