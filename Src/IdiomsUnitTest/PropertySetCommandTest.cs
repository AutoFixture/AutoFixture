using System;
using System.Reflection;
using AutoFixture.Idioms;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.IdiomsUnitTest
{
    public class PropertySetCommandTest
    {
        [Fact]
        public void SutIsGuardClauseCommand()
        {
            // Arrange
            var dummyOwner = new PropertyHolder<object>();
            var dummyProperty = dummyOwner.GetType().GetProperty("Property");
            // Act
            var sut = new PropertySetCommand(dummyProperty, dummyOwner);
            // Assert
            Assert.IsAssignableFrom<IGuardClauseCommand>(sut);
        }

        [Fact]
        public void PropertyInfoIsCorrect()
        {
            // Arrange
            var dummyOwner = new PropertyHolder<object>();
            var propertyInfo = dummyOwner.GetType().GetProperty("Property");
            var sut = new PropertySetCommand(propertyInfo, dummyOwner);
            // Act
            PropertyInfo result = sut.PropertyInfo;
            // Assert
            Assert.Equal(propertyInfo, result);
        }

        [Fact]
        public void OwnerIsCorrect()
        {
            // Arrange
            var owner = new PropertyHolder<object>();
            var dummyProperty = owner.GetType().GetProperty("Property");
            var sut = new PropertySetCommand(dummyProperty, owner);
            // Act
            var result = sut.Owner;
            // Assert
            Assert.Equal(owner, result);
        }

        [Fact]
        public void ExecuteAssignsValueToOwnerProperty()
        {
            // Arrange
            var owner = new PropertyHolder<object>();
            var property = owner.GetType().GetProperty("Property");
            var sut = new PropertySetCommand(property, owner);
            var value = new object();
            // Act
            sut.Execute(value);
            // Assert
            Assert.Equal(value, owner.Property);
        }

        [Fact]
        public void RequestedTypeIsCorrect()
        {
            // Arrange
            var dummyOwner = new PropertyHolder<Version>();
            var property = dummyOwner.GetType().GetProperty("Property");
            var sut = new PropertySetCommand(property, dummyOwner);
            // Act
            var result = sut.RequestedType;
            // Assert
            Assert.Equal(property.PropertyType, result);
        }

        [Fact]
        public void RequestedParameterNameIsCorrect()
        {
            var dummyOwner = new PropertyHolder<object>();
            var propertyDummy = dummyOwner.GetType().GetProperty("Property");

            var sut = new PropertySetCommand(propertyDummy, dummyOwner);

            Assert.Equal("value", sut.RequestedParameterName);
        }

        [Fact]
        public void CreateExceptionReturnsExceptionWithCorrectMessage()
        {
            // Arrange
            var dummyOwner = new PropertyHolder<Version>();
            var dummyProperty = dummyOwner.GetType().GetProperty("Property");
            var sut = new PropertySetCommand(dummyProperty, dummyOwner);
            // Act
            var message = Guid.NewGuid().ToString();
            var result = sut.CreateException(message);
            // Assert
            var e = Assert.IsAssignableFrom<GuardClauseException>(result);
            Assert.Contains(message, e.Message);
        }

        [Fact]
        public void CreateExceptionWithInnerReturnsExceptionWithCorrectMessage()
        {
            // Arrange
            var dummyOwner = new PropertyHolder<Version>();
            var dummyProperty = dummyOwner.GetType().GetProperty("Property");
            var sut = new PropertySetCommand(dummyProperty, dummyOwner);
            // Act
            var message = Guid.NewGuid().ToString();
            var inner = new Exception();
            var result = sut.CreateException(message, inner);
            // Assert
            var e = Assert.IsAssignableFrom<GuardClauseException>(result);
            Assert.Contains(message, e.Message);
        }

        [Fact]
        public void CreateExceptionWithInnerReturnsExceptionWithCorrectInnerException()
        {
            // Arrange
            var dummyOwner = new PropertyHolder<Version>();
            var dummyProperty = dummyOwner.GetType().GetProperty("Property");
            var sut = new PropertySetCommand(dummyProperty, dummyOwner);
            // Act
            var message = Guid.NewGuid().ToString();
            var inner = new Exception();
            var result = sut.CreateException(message, inner);
            // Assert
            var e = Assert.IsAssignableFrom<GuardClauseException>(result);
            Assert.Equal(inner, e.InnerException);
        }
    }
}
