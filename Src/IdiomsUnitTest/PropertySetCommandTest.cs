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
    }
}
