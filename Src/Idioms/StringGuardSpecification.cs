using System;

namespace Ploeh.AutoFixture.Idioms
{
    public class StringGuardSpecification : ITypeGuardSpecification
    {
        #region Implementation of ITypeGuardSpecification

        public IValueGuardConvention IsSatisfiedBy(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (type != typeof(string))
            {
                return new NullValueGuardConvention();
            }

            return new StringGuardConvention();
        }

        #endregion
    }
}