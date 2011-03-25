using System.Linq;
using System.Reflection;
using Xunit;
using Ploeh.AutoFixture.Kernel;
using System;

namespace Ploeh.AutoFixture.AutoRhinoMock.UnitTest
{
    public class RhinoMockConstructorMethodTest
    {
        [Fact]
        public void SutImplementsISpecimenBuilder()
        {
            // Exercise system
            var sut = new RhinoMockConstructorMethod(typeof(RhinoMockConstructorMethod).GetConstructors().First(), Enumerable.Empty<ParameterInfo>().ToArray());

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
                new RhinoMockConstructorMethod(typeof(RhinoMockConstructorMethod).GetConstructors().First(), null));
        }
    }
}