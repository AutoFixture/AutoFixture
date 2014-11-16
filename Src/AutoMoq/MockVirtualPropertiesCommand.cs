using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Moq;
using Ploeh.AutoFixture.AutoMoq.Extensions;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoMoq
{
    /// <summary>
    /// Stubs a mocked object's virtual properties, giving them "property behavior".
    /// This means setting a property's value will cause it to be saved and later returned when the property is accessed.
    /// The initial value will be retrieved from a fixture.
    /// </summary>
    /// <remarks>
    /// This will setup any virtual properties with public get *and* set accessors.
    /// </remarks>
    public class MockVirtualPropertiesCommand : ISpecimenCommand
    {
        private readonly ISpecimenCommand autoPropertiesCommand =
            new AutoPropertiesCommand(new MockVirtualPropertySpecification());

        /// <summary>
        /// Stubs a mocked object's virtual properties, giving them "property behavior".
        /// This means setting a property's value will cause it to be saved and later returned when the property is accessed.
        /// The initial value will be retrieved from a fixture.
        /// </summary>
        /// <param name="specimen">The mock to setup.</param>
        /// <param name="context">The context of the mock.</param>
        public void Execute(object specimen, ISpecimenContext context)
        {
            if (specimen == null) throw new ArgumentNullException("specimen");
            if (context == null) throw new ArgumentNullException("context");

            var mock = specimen as Mock;
            if (mock == null)
                return;

            //disable generation of default values, otherwise SetupAllProperties will hang if there's a circular dependency
            mock.DefaultValue = DefaultValue.Empty;

            //stub properties
            mock.GetType()
                .GetMethod("SetupAllProperties")
                .Invoke(mock, new object[0]);

            //set initial value
            autoPropertiesCommand.Execute(mock.Object, context);

            //re-enable generation of default values
            mock.DefaultValue = DefaultValue.Mock;
        }

        private class MockVirtualPropertySpecification : IRequestSpecification
        {
            /// <summary>
            /// Satisfied by overridable properties.
            /// </summary>
            public bool IsSatisfiedBy(object request)
            {
                var pi = request as PropertyInfo;
                return pi != null && CanBeConfigured(pi);
            }

            private static bool CanBeConfigured(PropertyInfo property)
            {
                return property.GetSetMethod() != null &&
                       property.GetGetMethod() != null &&
                       property.GetSetMethod().IsOverridable() &&
                       property.GetIndexParameters().Length == 0;
            }
        }
    }
}
