﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoMoq
{
    /// <summary>
    /// Enables auto-mocking and auto-setup with Moq.
    /// Members of a mock will be automatically setup to retrieve the return values from a fixture.
    /// </summary>
    public class AutoConfiguredMoqCustomization : ICustomization
    {
        private readonly ISpecimenBuilder relay;

        /// <summary>
        /// Creates a new instance of <see cref="AutoConfiguredMoqCustomization"/>.
        /// </summary>
        public AutoConfiguredMoqCustomization()
            : this(new MockRelay())
        {

        }

        /// <summary>
        /// Creates a new instance of <see cref="AutoConfiguredMoqCustomization"/>.
        /// </summary>
        /// <param name="relay">A mock relay to be added to <see cref="IFixture.ResidueCollectors"/></param>
        public AutoConfiguredMoqCustomization(ISpecimenBuilder relay)
        {
            if (relay == null)
                throw new ArgumentNullException("relay");

            this.relay = relay;
        }

        /// <summary>
        /// Gets the relay that will be added to <see cref="IFixture.ResidueCollectors"/> when <see cref="Customize"/> is invoked.
        /// </summary>
        public ISpecimenBuilder Relay
        {
            get { return relay; }
        }

        /// <summary>
        /// Customizes a <see cref="IFixture"/> to enable auto-mocking and auto-setup with Moq.
        /// Members of a mock will be automatically setup to retrieve the return values from <paramref name="fixture"/>.
        /// </summary>
        /// <param name="fixture">The fixture to customize.</param>
        public void Customize(IFixture fixture)
        {
            if (fixture == null) throw new ArgumentNullException("fixture");

            fixture.Customizations.Add(
                new Postprocessor(
                    builder: new MockPostprocessor(
                                new MethodInvoker(
                                    new MockConstructorQuery())),
                    command: new CompositeSpecimenCommand(
                                new MockVirtualMethodsCommand(),
                                new StubPropertiesCommand(),
                                new AutoMockPropertiesCommand())));

            fixture.ResidueCollectors.Add(Relay);
        }
    }
}
