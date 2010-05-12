using System;
using System.Linq;
using System.Reflection;

namespace Ploeh.AutoFixture.Idioms
{
    public class PickedProperty<T, TProperty> : IPickedProperty
    {
        private readonly Fixture fixture;
        private readonly PropertyInfo propertyInfo;

        public PickedProperty(Fixture fixture, PropertyInfo propertyInfo)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }
            if (propertyInfo == null)
            {
                throw new ArgumentNullException("propertyInfo");
            }

            this.fixture = fixture;
            this.propertyInfo = propertyInfo;
        }

        #region Implementation of IPickedProperty

        public void IsWellBehavedWritableProperty()
        {
            if (this.propertyInfo.GetSetMethod() == null)
            {
                throw new PickedPropertyException("The supplied PropertyInfo is read-only.");
            }

            var sut = this.fixture.CreateAnonymous<T>();
            var propertyValue = this.fixture.CreateAnonymous<TProperty>();

            this.propertyInfo.SetValue(sut, propertyValue, null);
            var result = (TProperty)this.propertyInfo.GetValue(sut, null);

            if (!propertyValue.Equals(result))
            {
                throw new PickedPropertyException("The supplied PropertyInfo does not point out a well-behaved property.");
            }
        }

        #endregion
    }
}