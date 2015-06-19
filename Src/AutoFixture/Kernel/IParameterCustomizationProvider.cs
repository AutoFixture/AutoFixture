using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Provides an <see cref="ICustomization"/> for a given <see cref="ParameterInfo"/>.
    /// </summary>
    public interface IParameterCustomizationProvider
    {
        /// <summary>
        /// Gets a customization for a parameter.
        /// </summary>
        /// <param name="parameter">The parameter for which the customization is requested.</param>
        ICustomization GetCustomization(ParameterInfo parameter);
    }
}
