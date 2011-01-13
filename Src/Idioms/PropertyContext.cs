using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Globalization;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Idioms
{
    public class PropertyContext : IVerifiableBoundary
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

            var specimen = this.composer.CreateAnonymous(this.propertyInfo.ReflectedType);
            var propertyValue = this.composer.CreateAnonymous(this.propertyInfo.PropertyType);

            this.propertyInfo.SetValue(specimen, propertyValue, null);
            var result = this.propertyInfo.GetValue(specimen, null);

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

            var specimen = this.composer.CreateAnonymous(this.propertyInfo.ReflectedType);
            Action<object> setProperty = x => this.propertyInfo.SetValue(specimen, x, null);

            var behaviors = from b in convention.CreateBoundaryBehaviors(this.propertyInfo.PropertyType)
                            select b.UnwrapReflectionExceptions();
            foreach (var b in behaviors)
            {
                b.Assert(setProperty);
            }
        }

        #endregion
    }
}