using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Encapsulates a unit test which verifies a method correctly makes a copy of an
    /// object while updating one or more public properties or fields.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When dealing with immutable types, it can be useful to add a convenience method
    /// to change a single field/property in a more complex object; e.g.
    /// <code>
    /// public Foo WithBar(Bar newBar)
    /// {
    ///     return new Foo(this.baz, newBar, this.qux);
    /// }
    /// </code>
    /// </para>
    /// Testing this not only requires verification that newBar was properly used (and exposed
    /// as a field or Inspection Property), but that all other values contained by Foo are held
    /// constant.
    /// </remarks>
    public class CopyAndUpdateAssertion : IdiomaticAssertion
    {
        private readonly ISpecimenBuilder builder;
        private readonly IEqualityComparer comparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyAndUpdateAssertion"/> class.
        /// </summary>
        /// <param name="builder">
        /// A composer which can create instances required to implement the idiomatic unit test.
        /// </param>
        /// <param name="comparer">A comparer that compares public member values from the
        /// specimen with public member values from the 'copied' and updated' instance</param>
        /// <remarks>
        /// <para>
        /// <paramref name="builder" /> will typically be a <see cref="Fixture" /> instance.
        /// </para>
        /// </remarks>
        public CopyAndUpdateAssertion(
            ISpecimenBuilder builder,
            System.Collections.IEqualityComparer comparer)
        {
            if (builder == null)
                throw new ArgumentNullException("builder");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            this.builder = builder;
            this.comparer = comparer;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CopyAndUpdateAssertion"/> class.
        /// </summary>
        /// <param name="builder">
        /// A composer which can create instances required to implement the idiomatic unit test.
        /// </param>
        /// <remarks>
        /// <para>
        /// <paramref name="builder" /> will typically be a <see cref="Fixture" /> instance.
        /// </para>
        /// </remarks>
        public CopyAndUpdateAssertion(
            ISpecimenBuilder builder)
            : this(builder, EqualityComparer<object>.Default)
        {
        }

        /// <summary>
        /// Gets the builder supplied by the constructor.
        /// </summary>
        public ISpecimenBuilder Builder
        {
            get { return this.builder; }
        }

        /// <summary>
        /// Gets the comparer that tests for equality of the values on the specimen and the
        /// 'copied and updated' specimen.
        /// </summary>
        public IEqualityComparer Comparer
        {
            get { return this.comparer; }
        }

        /// <summary>
        /// Verifies that a method correctly makes a copy of an object while changing                                  
        /// one or more public properties or fields.
        /// </summary>
        /// <param name="methodInfo">The 'copy and update' method to verify</param>
        public override void Verify(MethodInfo methodInfo)
        {
            if (methodInfo == null)
                throw new ArgumentNullException("methodInfo");

            var publicMembers = GetPublicPropertiesAndFields(methodInfo.ReflectedType);
            var parameters = (
                from parameter in methodInfo.GetParameters()
                select new
                {
                    Parameter = parameter,
                    Member = publicMembers.FirstOrDefault(m => IsMatchingParameterAndMember(parameter, m)),
                    Value = this.builder.CreateAnonymous(parameter)
                })
                .ToArray();

            // Verify all parameters have a public member that can be inspected
            var firstParamWithNoMatchingMember = parameters.FirstOrDefault(m => m.Member == null);
            if (firstParamWithNoMatchingMember != null)
            {
                // Parameter has no matching public member
                throw new CopyAndUpdateException(methodInfo, firstParamWithNoMatchingMember.Parameter);
            }

            // Build a specimen and invoke the 'Copy and update' method
            var specimen = this.builder.CreateAnonymous(methodInfo.ReflectedType);
            var copiedAndUpdatedSpecimen = methodInfo.Invoke(specimen, parameters.Select(p => p.Value).ToArray());
            VerifyCopiedAndUpdatedSpecimenType(methodInfo, copiedAndUpdatedSpecimen, specimen);

            // Verify each parameter with a matching member is different
            var firstEqualValueExpectedToBeDifferent =
                parameters.FirstOrDefault(p => AreMemberValuesEqual(specimen, copiedAndUpdatedSpecimen, p.Member));

            if (firstEqualValueExpectedToBeDifferent != null)
            {
                throw new CopyAndUpdateException(methodInfo, firstEqualValueExpectedToBeDifferent.Member);
            }

            // Verify each member is the same (excluding those with a matching update parameter)
            var firstNonEqualMemberExpectedToBeEqual = publicMembers
                .Except(parameters.Select(p => p.Member))
                .FirstOrDefault(m => !AreMemberValuesEqual(specimen, copiedAndUpdatedSpecimen, m));

            if (firstNonEqualMemberExpectedToBeEqual != null)
            {
                throw new CopyAndUpdateException(methodInfo, firstNonEqualMemberExpectedToBeEqual);
            }
        }

        private static void VerifyCopiedAndUpdatedSpecimenType(
            MethodInfo methodInfo,
            object copiedAndUpdatedSpecimen,
            object specimen)
        {
            // Verify it's non-null
            if (copiedAndUpdatedSpecimen == null)
            {
                throw new CopyAndUpdateException(string.Format(CultureInfo.InvariantCulture,
                    "The 'copied and updated' instance from {0} is null.", methodInfo), methodInfo);
            }

            // verify it's a different instance
            if (object.ReferenceEquals(specimen, copiedAndUpdatedSpecimen))
            {
                throw new CopyAndUpdateException(string.Format(CultureInfo.InvariantCulture,
                    "The 'copied and updated' instance from {0} is not a new instance.", methodInfo), methodInfo);
            }

            // Verify the original specimen is an instance of 'copied and updated' instance
            if (!methodInfo.ReflectedType.IsInstanceOfType(copiedAndUpdatedSpecimen))
            {
                throw new CopyAndUpdateException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "The 'copied and updated' instance is not the expected type.{3}" +
                        "Copy and update method: {0}{3}" +
                        "Expected type: {1}{3}" +
                        "Actual type: {2}",
                        methodInfo,
                        methodInfo.ReflectedType,
                        copiedAndUpdatedSpecimen.GetType().FullName,
                        Environment.NewLine),
                    methodInfo);
            }
        }

        private bool AreMemberValuesEqual(object specimen, object copiedAndUpdatedSpecimen, MemberInfo member)
        {
            var propertyInfo = member as PropertyInfo;
            object specimenMemberValue;
            object copiedAndUpdatedMemberValue;

            if (propertyInfo != null)
            {
                specimenMemberValue = propertyInfo.GetValue(specimen, null);
                copiedAndUpdatedMemberValue = propertyInfo.GetValue(copiedAndUpdatedSpecimen, null);
            }
            else
            {
                var fieldInfo = (FieldInfo) member;
                specimenMemberValue = fieldInfo.GetValue(specimen);
                copiedAndUpdatedMemberValue = fieldInfo.GetValue(copiedAndUpdatedSpecimen);
            }

            return this.comparer.Equals(specimenMemberValue, copiedAndUpdatedMemberValue);
        }

        private static IEnumerable<MemberInfo> GetPublicPropertiesAndFields(Type t)
        {
            return t.GetMembers(BindingFlags.Instance | BindingFlags.Public)
                .Where(m => m.MemberType.HasFlag(MemberTypes.Field)
                            || m.MemberType.HasFlag(MemberTypes.Property));
        }

        private static bool IsMatch(
            string propertyOrFieldName, Type propertyOrFieldType, string parameterName, Type parameterType)
        {
            return propertyOrFieldName.Equals(parameterName, StringComparison.OrdinalIgnoreCase)
                   && propertyOrFieldType.IsAssignableFrom(parameterType);
        }

        private static bool IsMatchingParameterAndMember(ParameterInfo parameter, MemberInfo fieldOrProperty)
        {
            var fieldInfo = fieldOrProperty as FieldInfo;
            var propertyInfo = fieldOrProperty as PropertyInfo;

            if (fieldInfo == null && propertyInfo == null)
                return false;

            return propertyInfo != null
                ? IsMatchingParameter(propertyInfo)(parameter)
                : IsMatchingParameter(fieldInfo)(parameter);
        }

        private static Func<ParameterInfo, bool> IsMatchingParameter(PropertyInfo propertyInfo)
        {
            return p => IsMatch(propertyInfo.Name, propertyInfo.PropertyType, p.Name, p.ParameterType);
        }

        private static Func<ParameterInfo, bool> IsMatchingParameter(FieldInfo fieldInfo)
        {
            return p => IsMatch(fieldInfo.Name, fieldInfo.FieldType, p.Name, p.ParameterType);
        }

    }
}
