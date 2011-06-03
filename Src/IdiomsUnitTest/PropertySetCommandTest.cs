using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Idioms;
using Ploeh.TestTypeFoundation;
using System.Reflection;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class PropertySetCommandTest
    {
        [Fact]
        public void SutIsContextualAction()
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
        public void MemberInfoIsCorrect()
        {
            // Fixture setup
            var dummyOwner = new PropertyHolder<object>();
            var propertyInfo = dummyOwner.GetType().GetProperty("Property");
            var sut = new PropertySetCommand(propertyInfo, dummyOwner);
            // Exercise system
            var result = sut.MemberInfo;
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
        public void ContextTypeIsCorrect()
        {
            // Fixture setup
            var dummyOwner = new PropertyHolder<Version>();
            var property = dummyOwner.GetType().GetProperty("Property");
            var sut = new PropertySetCommand(property, dummyOwner);
            // Exercise system
            var result = sut.ContextType;
            // Verify outcome
            Assert.Equal(property.PropertyType, result);
            // Teardown
        }

        [Fact]
        public void ThrowThrowsExceptionWithCorrectProperty()
        {
            // Fixture setup
            var dummyOwner = new PropertyHolder<Version>();
            var expectedProperty = dummyOwner.GetType().GetProperty("Property");
            var sut = new PropertySetCommand(expectedProperty, dummyOwner);
            // Exercise system and verify outcome
            var message = "Anonymous message";
            var e = Assert.Throws<GuardClauseException>(() =>
                sut.Throw(message));
            Assert.Equal(expectedProperty, e.MemberInfo);
            // Teardown
        }

        [Fact]
        public void ThrowThrowsExceptionWithCorrectValueType()
        {
            // Fixture setup
            var dummyOwner = new PropertyHolder<Version>();
            var property = dummyOwner.GetType().GetProperty("Property");
            var sut = new PropertySetCommand(property, dummyOwner);
            // Exercise system and verify outcome
            var message = "Anonymous message";
            var e = Assert.Throws<GuardClauseException>(() =>
                sut.Throw(message));
            Assert.Equal(property.PropertyType, e.ValueType);
            // Teardown
        }

        [Fact]
        public void ThrowThrowsExceptionWithCorrectMessage()
        {
            // Fixture setup
            var dummyOwner = new PropertyHolder<Version>();
            var dummyProperty = dummyOwner.GetType().GetProperty("Property");
            var sut = new PropertySetCommand(dummyProperty, dummyOwner);
            // Exercise system and verify outcome
            var message = Guid.NewGuid().ToString();
            var e = Assert.Throws<GuardClauseException>(() =>
                sut.Throw(message));
            Assert.Contains(message, e.Message);
            // Teardown
        }

        [Fact]
        public void ThrowWithInnerThrowsExceptionWithCorrectProperty()
        {
            // Fixture setup
            var dummyOwner = new PropertyHolder<Version>();
            var expectedProperty = dummyOwner.GetType().GetProperty("Property");
            var sut = new PropertySetCommand(expectedProperty, dummyOwner);
            // Exercise system and verify outcome
            var message = "Anonymous message";
            var inner = new Exception();
            var e = Assert.Throws<GuardClauseException>(() =>
                sut.Throw(message, inner));
            Assert.Equal(expectedProperty, e.MemberInfo);
            // Teardown
        }

        [Fact]
        public void ThrowWithInnerThrowsExceptionWithCorrectValueType()
        {
            // Fixture setup
            var dummyOwner = new PropertyHolder<Version>();
            var property = dummyOwner.GetType().GetProperty("Property");
            var sut = new PropertySetCommand(property, dummyOwner);
            // Exercise system and verify outcome
            var message = "Anonymous message";
            var inner = new Exception();
            var e = Assert.Throws<GuardClauseException>(() =>
                sut.Throw(message, inner));
            Assert.Equal(property.PropertyType, e.ValueType);
            // Teardown
        }

        [Fact]
        public void ThrowWithInnerThrowsExceptionWithCorrectMessage()
        {
            // Fixture setup
            var dummyOwner = new PropertyHolder<Version>();
            var dummyProperty = dummyOwner.GetType().GetProperty("Property");
            var sut = new PropertySetCommand(dummyProperty, dummyOwner);
            // Exercise system and verify outcome
            var message = Guid.NewGuid().ToString();
            var inner = new Exception();
            var e = Assert.Throws<GuardClauseException>(() =>
                sut.Throw(message, inner));
            Assert.Contains(message, e.Message);
            // Teardown
        }

        [Fact]
        public void ThrowWithInnerThrowsExceptionWithCorrectInnerException()
        {
            // Fixture setup
            var dummyOwner = new PropertyHolder<Version>();
            var dummyProperty = dummyOwner.GetType().GetProperty("Property");
            var sut = new PropertySetCommand(dummyProperty, dummyOwner);
            // Exercise system and verify outcome
            var message = Guid.NewGuid().ToString();
            var inner = new Exception();
            var e = Assert.Throws<GuardClauseException>(() =>
                sut.Throw(message, inner));
            Assert.Equal(inner, e.InnerException);
            // Teardown
        }
    }
}
