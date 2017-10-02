using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Moq;
using Ploeh.AutoFixture.AutoMoq.Extensions;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoMoq
{
    /// <summary>
    /// If the type of the object being mocked contains any fields and/or non-virtual/sealed settable properties,
    /// this initializer will resolve them from a given context.
    /// </summary>
    [Obsolete("This class will be removed in a future version of AutoFixture. Use AutoMockPropertiesCommand instead, which will populate both sealed and overridable properties.")]
    public class MockSealedPropertiesCommand : ISpecimenCommand
    {
        private readonly ISpecimenCommand autoPropertiesCommand =
            new AutoPropertiesCommand(new MockSealedPropertySpecification());

        /// <summary>
        /// If the type of the object being mocked contains any fields and/or non-virtual/sealed settable properties,
        /// this initializer will resolve them from a given context.
        /// </summary>
        /// <param name="specimen">The mock object.</param>
        /// <param name="context">The context.</param>
        public void Execute(object specimen, ISpecimenContext context)
        {
            if (specimen == null) throw new ArgumentNullException("specimen");
            if (context == null) throw new ArgumentNullException("context");

            var mock = specimen as Mock;
            if (mock == null)
                return;

            autoPropertiesCommand.Execute(mock.Object, context);
        }

        private class MockSealedPropertySpecification : IRequestSpecification
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
                return string.Equals(fi.Name, "__interceptors", StringComparison.Ordinal);
            }
        }
    }
}
