using System;

namespace Ploeh.AutoFixture.Idioms
{
    public class GuidGuardSpecification : ITypeGuardSpecification
    {
        #region Implementation of ITypeGuardSpecification

        public IValueGuardConvention IsSatisfiedBy(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (type != typeof(Guid))
            {
                return new NullValueGuardConvention();
            }

            return new GuidValueGuardConvention();
        }

        #endregion
    }
}