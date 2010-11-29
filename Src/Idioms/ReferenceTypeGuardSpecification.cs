using System;

namespace Ploeh.AutoFixture.Idioms
{
    public class ReferenceTypeGuardSpecification : ITypeGuardSpecification
    {
        #region Implementation of ITypeGuardSpecification

        public IBoundaryConvention IsSatisfiedBy(Type type)
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