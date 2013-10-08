using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;

namespace Ploeh.VisitReflect.UnitTest
{
    public class ParameterInfoElementTest
    {
        [Fact]
        public void SutIsReflectionElement()
        {
            // Fixture setup
            var parameter = typeof (TypeWithParameter).GetMethods().First().GetParameters().First();
            // Exercise system
            var sut = new ParameterInfoElement(parameter);
            // Verify outcome
            Assert.IsAssignableFrom<IReflectionElement>(sut);
            // Teardown
        }

        [Fact]
        public void SutIsHierarchicalReflectionElement()
        {
            // Fixture setup
            var parameter = typeof(TypeWithParameter).GetMethods().First().GetParameters().First();
            // Exercise system
            var sut = new ParameterInfoElement(parameter);
            // Verify outcome
            Assert.IsAssignableFrom<IHierarchicalReflectionElement>(sut);
            // Teardown
        }

        [Fact]
        public void ParameterInfoIsCorrect()
        {
            // Fixture setup
            var parameter = typeof(TypeWithParameter).GetMethods().First().GetParameters().First();
            var sut = new ParameterInfoElement(parameter);
            // Exercise system
            var actual = sut.ParameterInfo;
            // Verify outcome
            Assert.Equal(parameter, actual);
            // Teardown
        }

        [Fact]
        public void ConstructWithNullParameterInfoThrows()
        {
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new ParameterInfoElement(null));
            // Teardown
        }

        [Fact]
        public void AcceptNullVisitorThrows()
        {
            // Fixture setup
            var parameter = typeof(TypeWithParameter).GetMethods().First().GetParameters().First();
            var sut = new ParameterInfoElement(parameter);
            // Exercise system
            // Verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Accept((IReflectionVisitor<object>)null));
            Assert.Throws<ArgumentNullException>(() =>
                sut.Accept((IHierarchicalReflectionVisitor<object>)null));
            // Teardown
        }

        [Fact]
        public void AcceptCallsVisitOnceWithCorrectType()
        {
            // Fixture setup
            var observed = new List<ParameterInfoElement>();
            var dummyVisitor = new DelegatingReflectionVisitor<int> { OnVisitParameterInfoElement = observed.Add };
            var sut = new ParameterInfoElement(typeof(TypeWithParameter).GetMethods().First().GetParameters().First());
            // Exercise system
            sut.Accept(dummyVisitor);
            // Verify outcome
            Assert.True(new[] { sut }.SequenceEqual(observed));
            // Teardown
        }

        public class TypeWithParameter
        {
            public void Method(int parameter)
            {   
            }
        }

    }
}
