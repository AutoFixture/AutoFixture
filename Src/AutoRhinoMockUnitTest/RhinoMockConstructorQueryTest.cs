using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixture.AutoRhinoMock;
using Xunit.Extensions;
using Ploeh.TestTypeFoundation;
using Rhino.Mocks.Interfaces;
using System.Reflection;

namespace Ploeh.AutoFixture.AutoRhinoMock.UnitTest
{
    public class RhinoMockConstructorQueryTest
    {
        [Fact]
        public void SutIsConstructorQuery()
        {
            // Fixture setup
            // Exercise system
            var sut = new RhinoMockConstructorQuery();
            // Verify outcome
            Assert.IsAssignableFrom<IConstructorQuery>(sut);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(AbstractType))]
        [InlineData(typeof(IInterface))]
        public void SelectReturnsCorrectResult(Type t)
        {
            // Fixture setup
            var expectedCount = t.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Length;
            var sut = new RhinoMockConstructorQuery();
            // Exercise system
            var result = sut.SelectConstructors(t);
            // Verify outcome
            Assert.Equal(expectedCount, result.Count());
            // Teardown
        }

    }
}
