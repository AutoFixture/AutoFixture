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
        private readonly ISpecimenBuilder mockRelay;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoRhinoMockCustomization"/> class.
        /// </summary>
        public AutoRhinoMockCustomization()
            : this(new RhinoMockInterfaceBuilder())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoRhinoMockCustomization"/> class.
        /// </summary>
        /// <param name="mockRelay">The mock relay.</param>
        public AutoRhinoMockCustomization(ISpecimenBuilder mockRelay)
        {
            if (mockRelay == null)
            {
                throw new ArgumentNullException("mockRelay");
            }

            this.mockRelay = mockRelay;
        }

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

            fixture.ResidueCollectors.Add(this.MockRelay);
        }

        #endregion

        /// <summary>
        /// Gets the relay that will be added to <see cref="IFixture.ResidueCollectors" /> when
        /// <see cref="Customize"/> is invoked.
        /// </summary>
        /// <seealso cref="AutoRhinoMockCustomization(ISpecimenBuilder)" />
        public ISpecimenBuilder MockRelay
        {
            get
            {
                return this.mockRelay;
            }
        }
    }
}
