﻿using System;
using System.Reflection;

namespace AutoFixture.Xunit2
{
    /// <summary>
    /// An attribute that can be applied to parameters in an <see cref="AutoDataAttribute"/>-driven
    /// Theory to indicate that the parameter value should not have properties auto populated
    /// when the <see cref="IFixture"/> creates an instance of that type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class NoAutoPropertiesAttribute : CustomizeAttribute
    {
        /// <summary>
        /// Gets a customization that stops auto population of properties for the type of the parameter.
        /// </summary>
        /// <param name="parameter">The parameter for which the customization is requested.</param>
        /// <returns>
        /// A customization that stops auto population of the <see cref="Type"/> of the parameter.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="parameter"/> is null.
        /// </exception>
        public override ICustomization GetCustomization(ParameterInfo parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            var targetType = parameter.ParameterType;
            return new NoAutoPropertiesCustomization(targetType);
        }
    }
}
