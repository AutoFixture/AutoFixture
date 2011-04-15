using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoRhinoMock
{
    /// <summary>
    /// Enables IFixture auto-mocking of abstract classes and interfaces using RhinoMocks.
    /// </summary>
    public class AutoRhinoMockCustomization : ICustomization
    {
        #region ICustomization Members

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

            fixture.Customizations.Add(
                new RhinoMockPostprocessor(
                    new ConstructorInvoker(
                        new RhinoMockConstructorQuery())));
        }

        #endregion
    }
}
