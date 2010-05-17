using System;

namespace Ploeh.AutoFixture.Idioms
{
    public class ValueTypeGuardSpecification : ITypeGuardSpecification
    {
        #region Implementation of ITypeGuardSpecification

        public IValueGuardConvention IsSatisfiedBy(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (type.IsValueType)
            {
                return new ValueTypeGuardConvention();
            }

            return null;
        }

        #endregion
    }
}