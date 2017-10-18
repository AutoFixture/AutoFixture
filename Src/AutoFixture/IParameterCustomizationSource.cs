using System.Reflection;

namespace AutoFixture
{
    /// <summary>
    ///     Source of the <see cref="ICustomization" /> instances specific for the particular
    ///     <see cref="ParameterInfo" /> parameter.
    ///     The main clients of this interface might be glue libraries which provide support for the parameter specific
    ///     customizations.
    /// </summary>
    public interface IParameterCustomizationSource
    {
        /// <summary>
        ///     Gets a customization for a parameter.
        /// </summary>
        /// <param name="parameter">The parameter for which the customization is requested.</param>
        /// <returns></returns>
        ICustomization GetCustomization(ParameterInfo parameter);
    }
}