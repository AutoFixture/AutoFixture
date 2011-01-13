using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Globalization;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Idioms
{
    public class PropertyContext<T, TProperty> : IVerifiableBoundary
    {
        private readonly ISpecimenBuilderComposer composer;
        private readonly PropertyInfo propertyInfo;
        private readonly bool isPropertyReadOnly;

        public PropertyContext(ISpecimenBuilderComposer composer, PropertyInfo propertyInfo)
        {
            if (composer == null)
            {
                throw new ArgumentNullException("composer");
            }
            if (propertyInfo == null)
            {
                throw new ArgumentNullException("propertyInfo");
            }

            this.composer = composer;
            this.propertyInfo = propertyInfo;
            this.isPropertyReadOnly = this.propertyInfo.GetSetMethod() == null;
        }

        public void VerifyWritable()
        {
            if (this.isPropertyReadOnly)
            {
                throw new PropertyContextException("The supplied PropertyInfo is read-only.");
            }

            var sut = this.composer.CreateAnonymous<T>();
            var propertyValue = this.composer.CreateAnonymous<TProperty>();

            this.propertyInfo.SetValue(sut, propertyValue, null);
            var result = this.propertyInfo.GetValue(sut, null);

            if (!propertyValue.Equals(result))
            {
                throw new PropertyContextException("The supplied PropertyInfo does not point out a well-behaved property.");
            }
        }

        #region IVerifiableBoundary Members

        public void VerifyBoundaries(IBoundaryConvention convention)
        {
            if (convention == null)
            {
                throw new ArgumentNullException("convention");
            }

            if (this.isPropertyReadOnly)
            {
                return;
            }

            var sut = this.composer.CreateAnonymous<T>();
            Action<object> setProperty = x => this.propertyInfo.SetValue(sut, x, null);

            var behaviors = from b in convention.CreateBoundaryBehaviors(typeof(TProperty))
                            select b.UnwrapReflectionExceptions();
            foreach (var b in behaviors)
            {
                b.Assert(setProperty);
            }
        }

        #endregion
    }
}