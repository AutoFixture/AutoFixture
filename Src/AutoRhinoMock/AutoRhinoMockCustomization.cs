using System;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoRhinoMock
{
    /// <summary>
    /// Enables IFixture auto-mocking of abstract classes and interfaces using RhinoMocks.
    /// </summary>
    public class AutoRhinoMockCustomization : ICustomization
    {
        /// <summary>
        /// Customizes an <see cref="IFixture"/> to enable auto-mocking with Rhino Mocks.
        /// </summary>
        /// <param name="fixture">The fixture upon which to enable auto-mocking.</param>
        public void Customize(IFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }

            fixture.ResidueCollectors.Add(
                new RhinoMockAroundAdvice(
                    new MethodInvoker(
                        new RhinoMockConstructorQuery())));
        }
    }
}
