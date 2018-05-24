﻿using System;
using Xunit;

namespace AutoFixture.Xunit2.UnitTest
{
    public class CustomizeAttributeTest
    {
        [Fact]
        public void TestableSutIsSut()
        {
            // Arrange
            // Act
            var sut = new DelegatingCustomizeAttribute();
            // Assert
            Assert.IsAssignableFrom<CustomizeAttribute>(sut);
        }

        [Fact]
        public void SutIsAttribute()
        {
            // Arrange
            // Act
            var sut = new DelegatingCustomizeAttribute();
            // Assert
            Assert.IsAssignableFrom<Attribute>(sut);
        }

        [Fact]
        public void SutImplementsIParameterCustomizationSource()
        {
            // Arrange
            // Act
            var sut = new DelegatingCustomizeAttribute();
            // Assert
            Assert.IsAssignableFrom<IParameterCustomizationSource>(sut);
        }
    }
}
