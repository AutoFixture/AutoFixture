using System;
using System.Collections.Generic;
using System.Reflection;
using AutoFixture.Kernel;

namespace AutoFixture.Xunit2
{
    /// <summary>
    /// An attribute that can be applied to parameters in an <see cref="AutoDataAttribute"/>-driven
    /// Theory to indicate that the parameter value should be created using a constructor with one
    /// or more <see cref="IList{T}" /> arguments, if applicable.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class FavorListsAttribute : CustomizeAttribute
    {
        /// <summary>
        /// Gets a customization that associates a <see cref="ListFavoringConstructorQuery"/> with
        /// the <see cref="Type"/> of the parameter.
        /// </summary>
        /// <param name="parameter">The parameter for which the customization is requested.</param>
        /// <returns>
        /// A customization that associates a <see cref="ListFavoringConstructorQuery"/> with the
        /// <see cref="Type"/> of the parameter.
        /// </returns>
        public override ICustomization GetCustomization(ParameterInfo parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            return new ConstructorCustomization(parameter.ParameterType, new ListFavoringConstructorQuery());
        }
    }
}
