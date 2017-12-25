using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Kernel;
using Rhino.Mocks;
using Xunit;

namespace AutoFixture.AutoRhinoMock.UnitTest
{
    public class AutoRhinoMockCustomizationTest
    {
        [Fact]
        public void SutImplementsICustomization()
        {
            // Arrange
            // Act
            var sut = new AutoRhinoMockCustomization();
            // Assert
            Assert.IsAssignableFrom<ICustomization>(sut);
        }

        [Fact]
        public void CustomizeNullFixtureThrows()
        {
            // Arrange
            var sut = new AutoRhinoMockCustomization();
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Customize(null));
        }

        [Fact]
        public void CustomizeAddsAppropriateResidueCollectors()
        {
            // Arrange
            var residueCollectors = new List<ISpecimenBuilder>();
            var fixtureStub = MockRepository.GenerateMock<IFixture>();
            fixtureStub.Stub(c => c.ResidueCollectors).Return(residueCollectors);

            var sut = new AutoRhinoMockCustomization();
            // Act
            sut.Customize(fixtureStub);
            // Assert
            var postprocessor = residueCollectors.OfType<RhinoMockAroundAdvice>().Single();
            var ctorInvoker = Assert.IsAssignableFrom<MethodInvoker>(postprocessor.Builder);
            Assert.IsAssignableFrom<RhinoMockConstructorQuery>(ctorInvoker.Query);
        }
    }
}
