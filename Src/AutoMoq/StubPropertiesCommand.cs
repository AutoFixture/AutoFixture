using System.Reflection;
using AutoFixture.Kernel;
using Moq;

namespace AutoFixture.AutoMoq
{
    /// <summary>
    /// Stubs a mocked object's properties, giving them "property behavior".
    /// Setting a property's value will cause it to be saved and later returned when the property is accessed.
    /// </summary>
    public class StubPropertiesCommand : ISpecimenCommand
    {
        /// <summary>
        /// Stubs a mocked object's properties, giving them "property behavior".
        /// Setting a property's value will cause it to be saved and later returned when the property is accessed.
        /// </summary>
        /// <param name="specimen">The mock to setup.</param>
        /// <param name="context">The context of the mock.</param>
        public void Execute(object specimen, ISpecimenContext context)
        {
            var mock = specimen as Mock;
            if (mock == null)
                return;

            //disable generation of default values (if enabled), otherwise SetupAllProperties will hang if there's a circular dependency
            var mockDefaultValueSetting = mock.DefaultValue;
            mock.DefaultValue = DefaultValue.Empty;

            //stub properties
            mock.GetType()
                .GetMethod("SetupAllProperties")
                .Invoke(mock, new object[0]);

            //restore setting
            mock.DefaultValue = mockDefaultValueSetting;
        }
    }
}
