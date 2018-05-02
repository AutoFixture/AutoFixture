using System;
using Xunit;

namespace AutoFixture.AutoMoq.UnitTest
{
    public class MockCustomizeAttributeTest
    {
        [Fact]
        public void TestableSutIsSut()
        {
            // Arrange
            // Act
            var sut = new DelegatingMockCustomizeAttribute();
            // Assert
            Assert.IsAssignableFrom<MockCustomizeAttribute>(sut);
        }

        [Fact]
        public void SutIsAttribute()
        {
            // Arrange
            // Act
            var sut = new DelegatingMockCustomizeAttribute();
            // Assert
            Assert.IsAssignableFrom<Attribute>(sut);
        }

        [Fact]
        public void SutImplementsIParameterCustomizationSource()
        {
            // Arrange
            // Act
            var sut = new DelegatingMockCustomizeAttribute();
            // Assert
            Assert.IsAssignableFrom<IParameterCustomizationSource>(sut);
        }
    }
}
