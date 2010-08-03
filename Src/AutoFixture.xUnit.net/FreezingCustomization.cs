using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Xunit
{
    public class FreezingCustomization : ICustomization
    {
        private readonly Type targetType;

        public FreezingCustomization(Type targetType)
        {
            if (targetType == null)
            {
                throw new ArgumentNullException("targetType");
            }

            this.targetType = targetType;
        }

        public Type TargetType
        {
            get { return this.targetType; }
        }

        #region ICustomization Members

        public void Customize(IFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }

            var specimen = new SpecimenContext(fixture.Compose()).Resolve(this.TargetType);

            //var builder = new FilteringSpecimenBuilder(new SpecimenFactory
        }

        #endregion
    }
}
