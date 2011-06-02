using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;
using System.Reflection;

namespace Ploeh.AutoFixture.Idioms
{
    public class WritablePropertyAssertion : IdiomaticAssertion
    {
        private ISpecimenBuilderComposer composer;

        public WritablePropertyAssertion(ISpecimenBuilderComposer composer)
        {
            if (composer == null)
            {
                throw new ArgumentNullException("composer");
            }

            this.composer = composer;
        }

        public Kernel.ISpecimenBuilderComposer Composer
        {
            get { return this.composer; }
        }

        public override void Verify(PropertyInfo propertyInfo)
        {
            if (propertyInfo.GetSetMethod() == null)
            {
                return;
            }

            var specimen = this.composer.CreateAnonymous(propertyInfo.ReflectedType);
            var propertyValue = this.composer.CreateAnonymous(propertyInfo.PropertyType);

            propertyInfo.SetValue(specimen, propertyValue, null);
            var result = propertyInfo.GetValue(specimen, null);

            if (!propertyValue.Equals(result))
            {
                throw new WritablePropertyException(propertyInfo);
            }
        }
    }
}
