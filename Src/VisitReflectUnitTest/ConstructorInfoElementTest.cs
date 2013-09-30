using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;

namespace Ploeh.VisitReflect.UnitTest
{
    public class ConstructorInfoElementTest
    {
        [Fact]
        public void SutIsReflectionElement()
        {
            // Fixture setup
            // Exercise system
            var sut = new ConstructorInfoElement(this.GetType().GetConstructors().First());
            // Verify outcome
            Assert.IsAssignableFrom<IReflectionElement>(sut);
            // Teardown
        }

        [Fact]
        public void SutIsHierarchicalReflectionElement()
        {
            // Fixture setup
            // Exercise system
            var sut = new ConstructorInfoElement(this.GetType().GetConstructors().First());
            // Verify outcome
            Assert.IsAssignableFrom<IHierarchicalReflectionElement>(sut);
            // Teardown
        }

        [Fact]
        public void ConstructorInfoIsCorrect()
        {
            // Fixture setup
            var expectedCtor = this.GetType().GetConstructors().First();
            var sut = new ConstructorInfoElement(expectedCtor);
            // Exercise system
            var actual = sut.ConstructorInfo;
            // Verify outcome
            Assert.Equal(expectedCtor, actual);
            // Teardown
        }

        [Fact]
        public void ConstructWithNullConstructorInfoThrows()
        {
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new ConstructorInfoElement(null));
            // Teardown
        }

        [Fact]
        public void AcceptNullVisitorThrows()
        {
            // Fixture setup
            var sut = new ConstructorInfoElement(this.GetType().GetConstructors().First());
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
            var observed = new List<ConstructorInfoElement>();
            var dummyVisitor = new DelegatingReflectionVisitor<int>
                { OnVisitConstructorInfoElement = observed.Add };
            var sut = new ConstructorInfoElement(
                typeof(TypeWithConstructorParameters<int, int, int>).GetConstructors().First());
            // Exercise system
            sut.Accept(dummyVisitor);
            // Verify outcome
            Assert.True(new[] { sut }.SequenceEqual(observed));
            // Teardown
        }

        [Fact]
        public void AcceptHierachicalEntersItselfThenVisitsAllParametersThenExitsItself()
        {
            // Fixture setup
            var ctor = typeof(TypeWithConstructorParameters<int, string, decimal>).GetConstructors().First();
            var sut = new ConstructorInfoElement(ctor);
            var expectedElements = new List<IReflectionElement>();
            expectedElements.Add(sut);
            expectedElements.AddRange(ctor.GetParameters().Select(p => new ParameterInfoElement(p)));
            expectedElements.Add(sut);

            var observedElements = new List<IReflectionElement>();
            var dummyVisitor = new DelegatingHierarchicalReflectionVisitor<bool>
            {
                OnEnterConstructorInfoElement = observedElements.Add,
                OnVisitParameterInfoElement = observedElements.Add,
                OnExitConstructorInfoElement = observedElements.Add,
            };
            // Exercise system
            sut.Accept(dummyVisitor);
            // Verify outcome
            Assert.True(expectedElements.SequenceEqual(observedElements));
            // Teardown
        }

        class TypeWithConstructorParameters<TParam1, TParam2, TParam3>
        {
            public TypeWithConstructorParameters(TParam1 param1, TParam2 param2, TParam3 param3)
            {
            }
        }

    }
}
