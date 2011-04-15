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
        private readonly IEnumerable<ISpecimenBuilder> genericEnumerableBuilders;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoRhinoMockCustomization"/> class.
        /// </summary>
        public AutoRhinoMockCustomization()
            : this(new RhinoMockBuilder())
        {
        }

        public AutoRhinoMockCustomization(ISpecimenBuilder mockRelay, params ISpecimenBuilder[] genericEnumerableBuilders)
        {
            if (mockRelay == null)
            {
                throw new ArgumentNullException("mockRelay");
            }
            if (genericEnumerableBuilders.Any(geb => geb == null))
            {
                throw new ArgumentNullException("genericEnumerableBuilders", "null value in params array not allowed");
            }

            this.mockRelay = mockRelay;
            this.genericEnumerableBuilders = new List<ISpecimenBuilder>(genericEnumerableBuilders);
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

            foreach (var eb in this.genericEnumerableBuilders)
            {
                fixture.ResidueCollectors.Add(eb);
            }
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

        public IEnumerable<ISpecimenBuilder> GenericEnumerableBuilders
        {
            get
            {
                return this.genericEnumerableBuilders;
            }
        }
    }
}
