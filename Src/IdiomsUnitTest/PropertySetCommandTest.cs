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
            var dummyValue = new object();
            // Exercise system
            var sut = new PropertySetCommand(dummyProperty, dummyOwner, dummyValue);
            // Verify outcome
            Assert.IsAssignableFrom<IContextualCommand>(sut);
            // Teardown
        }

        [Fact]
        public void PropertyInfoIsCorrect()
        {
            // Fixture setup
            var dummyOwner = new PropertyHolder<object>();
            var propertyInfo = dummyOwner.GetType().GetProperty("Property");
            var dummyValue = new object();
            var sut = new PropertySetCommand(propertyInfo, dummyOwner, dummyValue);
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
            var dummyValue = new object();
            var sut = new PropertySetCommand(dummyProperty, owner, dummyValue);
            // Exercise system
            var result = sut.Owner;
            // Verify outcome
            Assert.Equal(owner, result);
            // Teardown
        }

        [Fact]
        public void ValueIsCorrect()
        {
            // Fixture setup
            var dummyOwner = new PropertyHolder<object>();
            var dummyProperty = dummyOwner.GetType().GetProperty("Property");
            var value = new object();
            var sut = new PropertySetCommand(dummyProperty, dummyOwner, value);
            // Exercise system
            var result = sut.Value;
            // Verify outcome
            Assert.Equal(value, result);
            // Teardown
        }

        [Fact]
        public void ExecuteAssignsValueToOwnerProperty()
        {
            // Fixture setup
            var owner = new PropertyHolder<object>();
            var property = owner.GetType().GetProperty("Property");
            var value = new object();
            var sut = new PropertySetCommand(property, owner, value);
            // Exercise system
            var dummyValue = new object();
            sut.Execute(dummyValue);
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
            var dummyValue = new Version();
            var sut = new PropertySetCommand(property, dummyOwner, dummyValue);
            // Exercise system
            var result = sut.ContextType;
            // Verify outcome
            Assert.Equal(property.PropertyType, result);
            // Teardown
        }
    }
}
