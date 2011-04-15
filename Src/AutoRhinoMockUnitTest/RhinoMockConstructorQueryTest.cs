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

        [Fact]
        public void SelectConstructorsFromNullTypeThrows()
        {
            // Fixture setup
            var sut = new RhinoMockConstructorQuery();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.SelectConstructors(null));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(AbstractType))]
        public void SelectReturnsCorrectResultForNonInterfaces(Type t)
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

        [Theory]
        [InlineData(typeof(IInterface))]
        [InlineData(typeof(IComparable<object>))]
        [InlineData(typeof(IComparable<string>))]
        [InlineData(typeof(IComparable<int>))]
        public void SelectReturnsCorrectNumberOfMethodsForInterface(Type t)
        {
            // Fixture setup
            var sut = new RhinoMockConstructorQuery();
            // Exercise system
            var result = sut.SelectConstructors(t);
            // Verify outcome
            Assert.Equal(1, result.Count());
            // Teardown
        }

        [Theory]
        [InlineData(typeof(IInterface))]
        [InlineData(typeof(IComparable<object>))]
        [InlineData(typeof(IComparable<string>))]
        [InlineData(typeof(IComparable<int>))]
        public void SelectReturnsCorrectResultForInterface(Type t)
        {
            // Fixture setup
            var sut = new RhinoMockConstructorQuery();
            // Exercise system
            var result = sut.SelectConstructors(t);
            // Verify outcome
            var method = Assert.IsAssignableFrom<RhinoMockConstructorMethod>(result.Single());
            Assert.Equal(t, method.MockTargetType);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(IInterface))]
        [InlineData(typeof(IComparable<object>))]
        [InlineData(typeof(IComparable<string>))]
        [InlineData(typeof(IComparable<int>))]
        public void SelectReturnsResultWithNoParametersForInterface(Type t)
        {
            // Fixture setup
            var sut = new RhinoMockConstructorQuery();
            // Exercise system
            var result = sut.SelectConstructors(t);
            // Verify outcome
            var method = Assert.IsAssignableFrom<RhinoMockConstructorMethod>(result.Single());
            Assert.Empty(method.Parameters);
            // Teardown
        }
    }
}
