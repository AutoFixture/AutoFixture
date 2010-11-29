using System;

namespace Ploeh.AutoFixture.Idioms
{
    public class StringGuardSpecification : ITypeGuardSpecification
    {
        #region Implementation of ITypeGuardSpecification

        public IBoundaryConvention IsSatisfiedBy(Type type)
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