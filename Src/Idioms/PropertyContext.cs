using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Idioms
{
    public class PropertyContext : IPropertyContext
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

        public ISpecimenBuilderComposer Composer
        {
            get { return this.composer; }
        }

        public PropertyInfo PropertyInfo
        {
            get { return this.propertyInfo; }
        }

        #region IPropertyContext Members

        public void VerifyWritable()
        {
            if (this.isPropertyReadOnly)
            {
                throw new PropertyContextException("The supplied PropertyInfo is read-only.");
            }

            var specimen = this.composer.CreateAnonymous(this.PropertyInfo.ReflectedType);
            var propertyValue = this.composer.CreateAnonymous(this.PropertyInfo.PropertyType);

            this.PropertyInfo.SetValue(specimen, propertyValue, null);
            var result = this.PropertyInfo.GetValue(specimen, null);

            if (!propertyValue.Equals(result))
            {
                throw new PropertyContextException("The supplied PropertyInfo does not point out a well-behaved property.");
            }
        }

        #endregion

        #region IMemberContext Members

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

            var specimen = this.composer.CreateAnonymous(this.PropertyInfo.ReflectedType);
            Action<object> setProperty = x => this.PropertyInfo.SetValue(specimen, x, null);

            var behaviors = from b in convention.CreateBoundaryBehaviors(this.PropertyInfo.PropertyType)
                            select b.UnwrapReflectionExceptions();
            foreach (var b in behaviors)
            {
                b.Assert(setProperty, string.Format(CultureInfo.InvariantCulture, "Assigning the property {0}", this.PropertyInfo.Name));
            }
        }

        #endregion
    }
}