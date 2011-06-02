using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Idioms;
using Ploeh.TestTypeFoundation;
using System.Reflection;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class PropertySetCommandTest
    {
        [Fact]
        public void SutIsContextualAction()
        {
            // Fixture setup
            var dummyProperty = typeof(PropertyHolder<object>).GetProperty("Property");
            // Exercise system
            var sut = new PropertySetCommand(dummyProperty);
            // Verify outcome
            Assert.IsAssignableFrom<IContextualCommand>(sut);
            // Teardown
        }

        [Fact]
        public void PropertyInfoIsCorrect()
        {
            // Fixture setup
            var propertyInfo = typeof(PropertyHolder<object>).GetProperty("Property");
            var sut = new PropertySetCommand(propertyInfo);
            // Exercise system
            PropertyInfo result = sut.PropertyInfo;
            // Verify outcome
            Assert.Equal(propertyInfo, result);
            // Teardown
        }
    }
}
