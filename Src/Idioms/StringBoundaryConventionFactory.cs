using System;

namespace Ploeh.AutoFixture.Idioms
{
    public class StringBoundaryConventionFactory : IBoundaryConventionFactory
    {
        #region Implementation of IBoundaryConventionFactory

        public IBoundaryConvention GetConvention(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (type != typeof(string))
            {
                return new NullBoundaryConvention();
            }

            return new StringBoundaryConvention();
        }

        #endregion
    }
}