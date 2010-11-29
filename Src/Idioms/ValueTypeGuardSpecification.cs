using System;

namespace Ploeh.AutoFixture.Idioms
{
    public class ValueTypeGuardSpecification : ITypeGuardSpecification
    {
        #region Implementation of ITypeGuardSpecification

        public IBoundaryConvention IsSatisfiedBy(Type type)
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