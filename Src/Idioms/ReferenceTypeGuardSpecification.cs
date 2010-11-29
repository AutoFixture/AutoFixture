using System;

namespace Ploeh.AutoFixture.Idioms
{
    public class ReferenceTypeGuardSpecification : ITypeGuardSpecification
    {
        #region Implementation of ITypeGuardSpecification

        public IValueGuardConvention IsSatisfiedBy(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (type.IsClass)
            {
                return new ReferenceTypeGuardConvention();
            }

            return new NullValueGuardConvention();
        }

        #endregion
    }
}