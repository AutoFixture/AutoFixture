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
            : this(new RhinoMockBuilder())
        {
        }

        public AutoRhinoMockCustomization(ISpecimenBuilder mockRelay)
        {
            if (mockRelay == null)
            {
                throw new ArgumentNullException("mockRelay");
            }

            this.mockRelay = mockRelay;
        }

        #region ICustomization Members

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

        public ISpecimenBuilder MockRelay
        {
            get
            {
                return this.mockRelay;
            }
        }
    }
}
