using System;
using System.Reflection;

namespace Ploeh.AutoFixture.Xunit
{
    /// <summary>
    /// An attribute that can be applied to parameters in an <see cref="AutoDataAttribute"/>-driven
    /// Theory to indicate that the parameter value should be frozen so that the same instance is
    /// returned every time the <see cref="IFixture"/> creates an instance of that type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class FrozenAttribute : CustomizeAttribute
    {
        private FreezingCustomization customization;

        /// <summary>
        /// Gets or sets the <see cref="Type"/> that the frozen parameter value
        /// should be mapped to in the <see cref="IFixture"/>.
        /// </summary>
        public Type As { get; set; }

        /// <summary>
        /// Gets a customization that freezes the <see cref="Type"/> of the parameter.
        /// </summary>
        /// <param name="parameter">The parameter for which the customization is requested.</param>
        /// <returns>
        /// A customization that freezes the <see cref="Type"/> of the parameter.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="parameter"/> is null.
        /// </exception>
        public override ICustomization GetCustomization(ParameterInfo parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException("parameter");
            }

            if (FrozenValueShouldBeRegisteredAsDifferentType())
            {
                CreateFreezingCustomizationForTargetTypeAsRegisteredType(parameter.ParameterType);
            }
            else
            {
                CreateFreezingCustomizationForTargetType(parameter.ParameterType);
            }

            return customization;
        }

        private bool FrozenValueShouldBeRegisteredAsDifferentType()
        {
            return As != null;
        }

        private void CreateFreezingCustomizationForTargetTypeAsRegisteredType(Type targetType)
        {
            customization = new FreezingCustomization(targetType, As);
        }

        private void CreateFreezingCustomizationForTargetType(Type targetType)
        {
            customization = new FreezingCustomization(targetType);
        }
    }
}
