﻿using System;
using System.Linq;
using System.Reflection;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.Xunit2.UnitTest
{
    public class NoAutoPropertiesAttributeTest
    {
        [Fact]
        public void SutIsAttribute()
        {
            // Arrange
            // Act
            var sut = new NoAutoPropertiesAttribute();
            // Assert
            Assert.IsAssignableFrom<CustomizeAttribute>(sut);
        }

        [Fact]
        public void GetCustomizationFromNullParameterThrows()
        {
            // Arrange
            var sut = new NoAutoPropertiesAttribute();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetCustomization(null));
        }

        [Fact]
        public void GetCustomizationReturnsTheCorrectResult()
        {
            // Arrange
            var sut = new NoAutoPropertiesAttribute();
            var parameter = TypeWithOverloadedMembers
                .GetDoSomethingMethod(typeof(object))
                .GetParameters().Single();
            // Act
            var result = sut.GetCustomization(parameter);
            // Assert
            Assert.IsAssignableFrom<NoAutoPropertiesCustomization>(result);
        }
    }
}
