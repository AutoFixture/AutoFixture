using System;
using System.Reflection;

namespace AutoFixture.Xunit
{
    /// <summary>
    /// The criteria used to determine which requests will be satisfied
    /// by the frozen specimen created for a parameter
    /// decorated with the <see cref="FrozenAttribute"/> attribute.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1714:FlagsEnumsShouldHavePluralNames", Justification = "This enumeration is designed to be used together with an attribute and is named to improve readability.")]
    [Flags]
    public enum Matching
    {
        /// <summary>
        /// Matches requests for the exact same <see cref="Type"/>
        /// as the type of the parameter.
        /// </summary>
        ExactType = 1,

        /// <summary>
        /// Matches requests for a <see cref="Type"/> that is
        /// a direct base of the type of the parameter.
        /// </summary>
        DirectBaseType = 2,

        /// <summary>
        /// Matches requests for an interface <see cref="Type"/> that is
        /// implemented by the type of the parameter.
        /// </summary>
        ImplementedInterfaces = 4,

        /// <summary>
        /// Matches requests for a <see cref="ParameterInfo"/> whose
        /// <see cref="Type"/> is compatible with the type of the parameter
        /// and has a specific name.
        /// </summary>
        ParameterName = 8,

        /// <summary>
        /// Matches requests for a <see cref="PropertyInfo"/> whose
        /// <see cref="Type"/> is compatible with the type of the parameter
        /// and has a specific name.
        /// </summary>
        PropertyName = 16,

        /// <summary>
        /// Matches requests for a <see cref="FieldInfo"/> whose
        /// <see cref="Type"/> is compatible with the type of the parameter
        /// and has a specific name.
        /// </summary>
        FieldName = 32,

        /// <summary>
        /// Matches requests for a parameter, property or field whose
        /// <see cref="Type"/> is compatible with the type of the parameter
        /// and has a specific name.
        /// </summary>
        MemberName = ParameterName | PropertyName | FieldName
    }
}
