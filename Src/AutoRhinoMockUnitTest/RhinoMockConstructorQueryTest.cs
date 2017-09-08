using System;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.AutoRhinoMock.UnitTest
{
    public class RhinoMockConstructorQueryTest
    {
        [Fact]
        public void SutIsMethodQuery()
        {
            // Fixture setup
            // Exercise system
            var sut = new RhinoMockConstructorQuery();
            // Verify outcome
            Assert.IsAssignableFrom<IMethodQuery>(sut);
            // Teardown
        }

        [Fact]
        public void SelectMethodsFromNullTypeThrows()
        {
            // Fixture setup
            var sut = new RhinoMockConstructorQuery();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.SelectMethods(null));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(AbstractType))]
        public void SelectMethodsReturnsCorrectResultForNonInterfaces(Type t)
        {
            // Fixture setup
            var expectedCount = t.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Length;
            var sut = new RhinoMockConstructorQuery();
            // Exercise system
            var result = sut.SelectMethods(t);
            // Verify outcome
            Assert.Equal(expectedCount, result.Count());
            // Teardown
        }

        [Theory]
        [InlineData(typeof(IInterface))]
        [InlineData(typeof(IComparable<object>))]
        [InlineData(typeof(IComparable<string>))]
        [InlineData(typeof(IComparable<int>))]
        public void SelectMethodsReturnsCorrectNumberOfMethodsForInterface(Type t)
        {
            // Fixture setup
            var sut = new RhinoMockConstructorQuery();
            // Exercise system
            var result = sut.SelectMethods(t);
            // Verify outcome
            Assert.Equal(1, result.Count());
            // Teardown
        }

        [Theory]
        [InlineData(typeof(IInterface))]
        [InlineData(typeof(IComparable<object>))]
        [InlineData(typeof(IComparable<string>))]
        [InlineData(typeof(IComparable<int>))]
        public void SelectMethodsReturnsCorrectResultForInterface(Type t)
        {
            // Fixture setup
            var sut = new RhinoMockConstructorQuery();
            // Exercise system
            var result = sut.SelectMethods(t);
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
        public void SelectMethodsReturnsResultWithNoParametersForInterface(Type t)
        {
            // Fixture setup
            var sut = new RhinoMockConstructorQuery();
            // Exercise system
            var result = sut.SelectMethods(t);
            // Verify outcome
            var method = Assert.IsAssignableFrom<RhinoMockConstructorMethod>(result.Single());
            Assert.Empty(method.Parameters);
            // Teardown
        }
    }
}
