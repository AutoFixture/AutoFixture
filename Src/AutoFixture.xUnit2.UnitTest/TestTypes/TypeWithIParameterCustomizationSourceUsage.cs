using System;
using System.Reflection;

namespace AutoFixture.Xunit2.UnitTest.TestTypes
{
    public class TypeWithIParameterCustomizationSourceUsage
    {
        public void DecoratedMethod([CustomizationSource] int arg)
        {
        }

        [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
        public class CustomizationSourceAttribute : Attribute, IParameterCustomizationSource
        {
            public ICustomization GetCustomization(ParameterInfo parameter)
            {
                return new Customization();
            }
        }

        public class Customization : ICustomization
        {
            public void Customize(IFixture fixture)
            {
            }
        }
    }
}