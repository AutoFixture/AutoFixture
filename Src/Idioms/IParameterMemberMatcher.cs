using System.Reflection;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Determines if a parameter and member are 'matched' together.
    /// </summary>
    public interface IParameterMemberMatcher
    {
        /// <summary>
        /// Determines if a parameter and member are 'matched' together.
        /// </summary>
        /// <param name="parameter">The parameter being tested</param>
        /// <param name="member">The member (field or property) being tested</param>
        /// <returns><c>true</c> for a match, <c>false</c> otherwise.</returns>
        bool IsMatch(ParameterInfo parameter, MemberInfo member);
    }
}