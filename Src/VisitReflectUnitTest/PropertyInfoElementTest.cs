using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;

namespace Ploeh.VisitReflect.UnitTest
{
    public class PropertyInfoElementTest
    {
        [Fact]
        public void SutIsReflectionElement()
        {
            // Fixture setup
            var property = typeof(TypeWithProperty).GetProperties().First();
            // Exercise system
            var sut = new PropertyInfoElement(property);
            // Verify outcome
            Assert.IsAssignableFrom<IReflectionElement>(sut);
            // Teardown
        }

        [Fact]
        public void PropertyInfoIsCorrect()
        {
            // Fixture setup
            var property = typeof(TypeWithProperty).GetProperties().First();
            var sut = new PropertyInfoElement(property);
            // Exercise system
            var actual = sut.PropertyInfo;
            // Verify outcome
            Assert.Equal(property, actual);
            // Teardown
        }

        [Fact]
        public void ConstructWithNullPropertyInfoThrows()
        {
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new PropertyInfoElement(null));
            // Teardown
        }

        [Fact]
        public void AcceptNullVisitorThrows()
        {
            // Fixture setup
            var property = typeof(TypeWithProperty).GetProperties().First();
            var sut = new PropertyInfoElement(property);
            // Exercise system
            // Verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Accept((IReflectionVisitor<object>)null));
            // Teardown
        }

        public class TypeWithProperty
        {
            public int Property { get; set; }
        }

    }
}
