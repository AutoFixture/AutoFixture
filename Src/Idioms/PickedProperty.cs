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
            var valueGuardConvention = (from tgs in typeGuardSpecifications
                                        let vgs = tgs.IsSatisfiedBy(typeof(TProperty))
                                        select vgs).FirstOrDefault();

            if (valueGuardConvention == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "List of Type guard specifications cannot handle Type {0}", typeof(TProperty)));
            }

            var sut = this.fixture.CreateAnonymous<T>();
            Action<object> action = g => this.propertyInfo.SetValue(sut, g, null);

            var behaviors = valueGuardConvention.CreateBoundaryBehaviors(this.fixture);
            foreach (var b in behaviors)
            {
                b.Assert(action);
            }
        }

        #endregion
    }
}