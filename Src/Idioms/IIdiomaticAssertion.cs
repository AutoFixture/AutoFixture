using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Represents an encapsulation of an idiomatic unit test assertion based on Reflection types.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Some unit tests tend to be very repeatable, following common idioms. Some such idiomatic
    /// tests can be expressed as general methods based on their <see cref="Type" /> or other
    /// Reflection-based instances.
    /// </para>
    /// </remarks>
    public interface IIdiomaticAssertion
    {
        /// <summary>
        /// Verifies that the idiomatic assertion can be verified for the specified assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        void Verify(params Assembly[] assemblies);

        /// <summary>
        /// Verifies that the idiomatic assertion can be verified for the specified assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        void Verify(IEnumerable<Assembly> assemblies);

        /// <summary>
        /// Verifies that the idiomatic assertion can be verified for an entire assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        void Verify(Assembly assembly);

        /// <summary>
        /// Verifies that the idiomatic assertion can be verified for the specified types.
        /// </summary>
        /// <param name="types">The types.</param>
        void Verify(params Type[] types);

        /// <summary>
        /// Verifies that the idiomatic assertion can be verified for the specified types.
        /// </summary>
        /// <param name="types">The types.</param>
        void Verify(IEnumerable<Type> types);

        /// <summary>
        /// Verifies that the idiomatic assertion can be verified for the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        void Verify(Type type);

        /// <summary>
        /// Verifies that the idiomatic assertion can be verified for the specified members.
        /// </summary>
        /// <param name="memberInfos">The members.</param>
        void Verify(params MemberInfo[] memberInfos);

        /// <summary>
        /// Verifies that the idiomatic assertion can be verified for the specified members.
        /// </summary>
        /// <param name="memberInfos">The members.</param>
        void Verify(IEnumerable<MemberInfo> memberInfos);

        /// <summary>
        /// Verifies that the idiomatic assertion can be verified for the specified member.
        /// </summary>
        /// <param name="memberInfo">The member.</param>
        void Verify(MemberInfo memberInfo);

        /// <summary>
        /// Verifies that the idiomatic assertion can be verified for the specified constructors.
        /// </summary>
        /// <param name="constructorInfos">The constructors.</param>
        void Verify(params ConstructorInfo[] constructorInfos);

        /// <summary>
        /// Verifies that the idiomatic assertion can be verified for the specified constructors.
        /// </summary>
        /// <param name="constructorInfos">The constructors.</param>
        void Verify(IEnumerable<ConstructorInfo> constructorInfos);

        /// <summary>
        /// Verifies that the idiomatic assertion can be verified for the specified constructor.
        /// </summary>
        /// <param name="constructorInfo">The constructor.</param>
        void Verify(ConstructorInfo constructorInfo);

        /// <summary>
        /// Verifies that the idiomatic assertion can be verified for the specified methods.
        /// </summary>
        /// <param name="methodInfos">The methods.</param>
        void Verify(params MethodInfo[] methodInfos);

        /// <summary>
        /// Verifies that the idiomatic assertion can be verified for the specified methods.
        /// </summary>
        /// <param name="methodInfos">The methods.</param>
        void Verify(IEnumerable<MethodInfo> methodInfos);

        /// <summary>
        /// Verifies that the idiomatic assertion can be verified for the specified method.
        /// </summary>
        /// <param name="methodInfo">The method.</param>
        void Verify(MethodInfo methodInfo);

        /// <summary>
        /// Verifies that the idiomatic assertion can be verified for the specified properties.
        /// </summary>
        /// <param name="propertyInfos">The properties.</param>
        void Verify(params PropertyInfo[] propertyInfos);

        /// <summary>
        /// Verifies that the idiomatic assertion can be verified for the specified properties.
        /// </summary>
        /// <param name="propertyInfos">The properties.</param>
        void Verify(IEnumerable<PropertyInfo> propertyInfos);

        /// <summary>
        /// Verifies that the idiomatic assertion can be verified for the specified property.
        /// </summary>
        /// <param name="propertyInfo">The property.</param>
        void Verify(PropertyInfo propertyInfo);

        /// <summary>
        /// Verifies that the idiomatic assertion can be verified for the specified field.
        /// </summary>
        /// <param name="fieldInfo">The field.</param>
        void Verify(FieldInfo fieldInfo);

        /// <summary>
        /// Verifies that the idiomatic assertion can be verified for the specified fields.
        /// </summary>
        /// <param name="fieldInfos">The fields.</param>
        void Verify(params FieldInfo[] fieldInfos);

        /// <summary>
        /// Verifies that the idiomatic assertion can be verified for the specified fields.
        /// </summary>
        /// <param name="fieldInfos">The Fields.</param>
        void Verify(IEnumerable<FieldInfo> fieldInfos);
    }
}
