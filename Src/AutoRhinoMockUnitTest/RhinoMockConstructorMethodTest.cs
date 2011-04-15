using System.Linq;
using System.Reflection;
using Xunit;
using Ploeh.AutoFixture.Kernel;
using System;
using Xunit.Extensions;
using Ploeh.TestTypeFoundation;

namespace Ploeh.AutoFixture.AutoRhinoMock.UnitTest
{
    public class RhinoMockConstructorMethodTest
    {
        [Fact]
        public void SutImplementsISpecimenBuilder()
        {
            // Exercise system
            var sut = new RhinoMockConstructorMethod(typeof(RhinoMockConstructorMethod), Enumerable.Empty<ParameterInfo>().ToArray());

            // Verify outcome
            Assert.IsAssignableFrom<IMethod>(sut);
            // Teardown
        }

        [Fact]
        public void ConstructorWithNullConstructorMethodThrows()
        {
            // Exercise system
            Assert.Throws<ArgumentNullException>(() => 
                new RhinoMockConstructorMethod(null, Enumerable.Empty<ParameterInfo>().ToArray()));
        }

        [Fact]
        public void ConstructorWithNullParameterInfoArray()
        {
            // Exercise system
            Assert.Throws<ArgumentNullException>(() =>
                new RhinoMockConstructorMethod(typeof(RhinoMockConstructorMethod), null));
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        [InlineData(typeof(AbstractType))]
        [InlineData(typeof(IInterface))]
        [InlineData(typeof(IComparable<object>))]
        [InlineData(typeof(IComparable<string>))]
        [InlineData(typeof(IComparable<int>))]
        public void MockTargetTypeIsCorrect(Type t)
        {
            // Fixture setup
            var sut = new RhinoMockConstructorMethod(t, new ParameterInfo[0]);
            // Exercise system
            Type result = sut.MockTargetType;
            // Verify outcome
            Assert.Equal(t, result);
            // Teardown
        }
    }
}