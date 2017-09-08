using System;
using System.Reflection;
using Ploeh.AutoFixture.Idioms;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class PropertySetCommandTest
    {
        [Fact]
        public void SutIsGuardClauseCommand()
        {
            // Fixture setup
            var dummyOwner = new PropertyHolder<object>();
            var dummyProperty = dummyOwner.GetType().GetProperty("Property");
            // Exercise system
            var sut = new PropertySetCommand(dummyProperty, dummyOwner);
            // Verify outcome
            Assert.IsAssignableFrom<IGuardClauseCommand>(sut);
            // Teardown
        }

        [Fact]
        public void PropertyInfoIsCorrect()
        {
            // Fixture setup
            var dummyOwner = new PropertyHolder<object>();
            var propertyInfo = dummyOwner.GetType().GetProperty("Property");
            var sut = new PropertySetCommand(propertyInfo, dummyOwner);
            // Exercise system
            PropertyInfo result = sut.PropertyInfo;
            // Verify outcome
            Assert.Equal(propertyInfo, result);
            // Teardown
        }

        [Fact]
        public void OwnerIsCorrect()
        {
            // Fixture setup
            var owner = new PropertyHolder<object>();
            var dummyProperty = owner.GetType().GetProperty("Property");
            var sut = new PropertySetCommand(dummyProperty, owner);
            // Exercise system
            var result = sut.Owner;
            // Verify outcome
            Assert.Equal(owner, result);
            // Teardown
        }

        [Fact]
        public void ExecuteAssignsValueToOwnerProperty()
        {
            // Fixture setup
            var owner = new PropertyHolder<object>();
            var property = owner.GetType().GetProperty("Property");
            var sut = new PropertySetCommand(property, owner);
            var value = new object();
            // Exercise system
            sut.Execute(value);
            // Verify outcome
            Assert.Equal(value, owner.Property);
            // Teardown
        }

        [Fact]
        public void RequestedTypeIsCorrect()
        {
            // Fixture setup
            var dummyOwner = new PropertyHolder<Version>();
            var property = dummyOwner.GetType().GetProperty("Property");
            var sut = new PropertySetCommand(property, dummyOwner);
            // Exercise system
            var result = sut.RequestedType;
            // Verify outcome
            Assert.Equal(property.PropertyType, result);
            // Teardown
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
            // Fixture setup
            var dummyOwner = new PropertyHolder<Version>();
            var dummyProperty = dummyOwner.GetType().GetProperty("Property");
            var sut = new PropertySetCommand(dummyProperty, dummyOwner);
            // Exercise system
            var message = Guid.NewGuid().ToString();
            var result = sut.CreateException(message);
            // Verify outcome
            var e = Assert.IsAssignableFrom<GuardClauseException>(result);
            Assert.Contains(message, e.Message);
            // Teardown
        }

        [Fact]
        public void CreateExceptionWithInnerReturnsExceptionWithCorrectMessage()
        {
            // Fixture setup
            var dummyOwner = new PropertyHolder<Version>();
            var dummyProperty = dummyOwner.GetType().GetProperty("Property");
            var sut = new PropertySetCommand(dummyProperty, dummyOwner);
            // Exercise system
            var message = Guid.NewGuid().ToString();
            var inner = new Exception();
            var result = sut.CreateException(message, inner);
            // Verify outcome
            var e = Assert.IsAssignableFrom<GuardClauseException>(result);
            Assert.Contains(message, e.Message);
            // Teardown
        }

        [Fact]
        public void CreateExceptionWithInnerReturnsExceptionWithCorrectInnerException()
        {
            // Fixture setup
            var dummyOwner = new PropertyHolder<Version>();
            var dummyProperty = dummyOwner.GetType().GetProperty("Property");
            var sut = new PropertySetCommand(dummyProperty, dummyOwner);
            // Exercise system
            var message = Guid.NewGuid().ToString();
            var inner = new Exception();
            var result = sut.CreateException(message, inner);
            // Verify outcome
            var e = Assert.IsAssignableFrom<GuardClauseException>(result);
            Assert.Equal(inner, e.InnerException);
            // Teardown
        }
    }
}
