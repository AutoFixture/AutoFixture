using System;

namespace Ploeh.AutoFixture.Idioms
{
    public class GuidGuardSpecification : ITypeGuardSpecification
    {
        #region Implementation of ITypeGuardSpecification

        public IBoundaryConvention IsSatisfiedBy(Type type)
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