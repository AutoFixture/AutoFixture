using System;
using System.Reflection;
using NSubstitute.Core;
using NSubstitute.Exceptions;
using Ploeh.AutoFixture.AutoNSubstitute.Extensions;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoNSubstitute
{
    /// <summary>
    /// If the type of the object being substituted contains any fields and/or non-virtual/sealed
    /// settable properties, this initializer will resolve them from a given context.
    /// </summary>
    public class NSubstituteSealedPropertiesCommand : ISpecimenCommand
    {
        private static readonly AutoPropertiesCommand autoPropertiesCommand =
            new AutoPropertiesCommand(new NSubstituteSealedPropertySpecification());

        /// <summary>
        /// If the type of the object being substituted contains any fields and/or non-virtual/sealed
        /// settable properties, this initializer will resolve them from a given context.
        /// </summary>
        /// <param name="specimen">The substitute object.</param>
        /// <param name="context">The context.</param>
        public void Execute(object specimen, ISpecimenContext context)
        {
            if (specimen == null) throw new ArgumentNullException("specimen");
            if (context == null) throw new ArgumentNullException("context");

            try
            {
                SubstitutionContext.Current.GetCallRouterFor(specimen);
            }
            catch (NotASubstituteException)
            {
                return;
            }

            autoPropertiesCommand.Execute(specimen, context);
        }

        private class NSubstituteSealedPropertySpecification : IRequestSpecification
        {
            /// <summary>
            /// Satisfied by any fields and non-virtual/sealed properties.
            /// </summary>
            public bool IsSatisfiedBy(object request)
            {
                //exclude non-sealed properties
                var pi = request as PropertyInfo;
                if (pi != null)
                    return pi.GetSetMethod().IsSealed();

                //exclude interceptor fields
                var fi = request as FieldInfo;
                if (fi != null)
                    return !IsDynamicProxyMember(fi);

                return false;
            }

            /// <summary>
            /// Checks whether a <see cref="FieldInfo"/> belongs to a dynamic proxy.
            /// </summary>
            private static bool IsDynamicProxyMember(FieldInfo fi)
            {
                return fi.Name == "__interceptors" || 
                    fi.Name == "__mixin_NSubstitute_Core_ICallRouter";
            }
        }
    }
}
