using System;
using System.Reflection;

namespace AutoFixture.AutoMoq
{
    /// <summary>
    /// Base Mock class for customizing parameters in methods decorated with AutoDataAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
    public abstract class MockCustomizeAttribute : Attribute, IParameterCustomizationSource
    {
        /// <summary>
        /// Gets a customization for a parameter.
        /// </summary>
        /// <param name="parameter">The parameter for which the customization is requested.</param>
        /// <returns></returns>
        public abstract ICustomization GetCustomization(ParameterInfo parameter);
    }
}
