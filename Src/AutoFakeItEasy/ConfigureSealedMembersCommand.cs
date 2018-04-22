using System;
using System.Reflection;
using AutoFixture.Kernel;

namespace AutoFixture.AutoFakeItEasy
{
    /// <summary>
    /// A command that populates all public writable sealed properties/fields of a Fake with anonymous values.
    /// </summary>
    internal class ConfigureSealedMembersCommand : ISpecimenCommand
    {
        private readonly ISpecimenCommand autoPropertiesCommand =
            new AutoPropertiesCommand(new FieldOrSealedPropertySpecification());

        /// <summary>
        /// Populates all public writable sealed properties/fields of a Fake with anonymous values.
        /// </summary>
        /// <param name="specimen">The Fake whose properties/fields will be populated.</param>
        /// <param name="context">The context that is used to create anonymous values.</param>
        public void Execute(object specimen, ISpecimenContext context)
        {
            if (specimen == null) return;
            if (context == null) throw new ArgumentNullException(nameof(context));

            var fake = specimen.GetType().GetProperty("FakedObject")?.GetValue(specimen, null);
            if (fake == null) return;

            this.autoPropertiesCommand.Execute(fake, context);
        }

        /// <summary>
        /// Evaluates whether a request to populate a member is valid.
        /// The request is valid if the member is a property or a field,
        /// and if it's sealed.
        /// </summary>
        private class FieldOrSealedPropertySpecification : IRequestSpecification
        {
            public bool IsSatisfiedBy(object request)
            {
                return (request is FieldInfo fi && !IsProxyMember(fi)) || 
                    (request is PropertyInfo pi && IsSealed(pi));
            }

            private static bool IsSealed(PropertyInfo pi)
            {
                var setMethod = pi.GetSetMethod();
                return setMethod != null && (IsExplicitlySealed(setMethod) || !setMethod.IsVirtual);
            }

            private static bool IsProxyMember(FieldInfo fi)
            {
                // DynamicProxy adds a special field that must remain intact
                return fi.Name.Equals("__interceptors", StringComparison.Ordinal);
            }

            private static bool IsExplicitlySealed(MethodInfo setMethod)
            {
                if (!setMethod.IsFinal) return false;
                var explicitImplementation = setMethod.Name.Contains(".");
                return !explicitImplementation;
            }
        }
    }
}
