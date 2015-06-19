using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Provides arguments generated for a test method based on parameter types as well 
    /// as attributes applied to the test method's parameters.
    /// </summary>
    public class TestDataProvider : ITestDataProvider
    {
        private readonly IFixture fixture;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestDataProvider"/> class.
        /// </summary>
        /// <param name="fixture">
        /// An <see cref="IFixture"/> that will be used create argument values. The <paramref name="fixture"/> can be  
        /// customized by parameters marked with attributes implementing the 
        /// <see cref="IParameterCustomizationProvider"/> interface.
        /// </param>
        public TestDataProvider(IFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }

            this.fixture = fixture;
        }

        /// <summary>
        /// Returns arguments for invoking the given <paramref name="testMethod"/>.
        /// </summary>
        /// <param name="testMethod">A <see cref="MethodInfo"/> representing a test method.</param>
        public IEnumerable<object> GetData(MethodInfo testMethod)
        {
            if (testMethod == null)
            {
                throw new ArgumentNullException("testMethod");
            }

            var arguments = new List<object>();
            foreach (ParameterInfo parameter in testMethod.GetParameters())
            {
                this.CustomizeFixture(parameter);
                object argument = this.GenerateArgument(parameter);
                arguments.Add(argument);
            }

            return arguments;
        }

        private void CustomizeFixture(ParameterInfo parameter)
        {
            var providers = parameter.GetCustomAttributes(false).OfType<IParameterCustomizationProvider>();
            foreach (IParameterCustomizationProvider provider in providers)
            {
                ICustomization customization = provider.GetCustomization(parameter);
                this.fixture.Customize(customization);
            }
        }

        private object GenerateArgument(ParameterInfo parameter)
        {
            var context = new SpecimenContext(this.fixture);
            return context.Resolve(parameter);
        }
    }
}
