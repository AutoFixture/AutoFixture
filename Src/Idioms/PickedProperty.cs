using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Globalization;

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

        public virtual void AssertInvariants()
        {
            this.AssertInvariants(new DefaultTypeGuardSpecificationCollection());
        }

        public virtual void AssertInvariants(IEnumerable<ITypeGuardSpecification> typeGuardSpecifications)
        {
            var boundaryConvention = (from tgs in typeGuardSpecifications
                                      let vgs = tgs.IsSatisfiedBy(typeof(TProperty))
                                      select vgs).FirstOrDefault();

            if (boundaryConvention == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "The supplied list of specifications cannot handle the Type {0}.", typeof(TProperty)), "typeGuardSpecifications");
            }

            var sut = this.fixture.CreateAnonymous<T>();
            Action<object> setProperty = x => this.propertyInfo.SetValue(sut, x, null);

            var behaviors = boundaryConvention.CreateBoundaryBehaviors(this.fixture);
            foreach (var b in behaviors)
            {
                b.Assert(setProperty);
            }
        }

        #endregion
    }
}