using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;

namespace Ploeh.VisitReflect.UnitTest
{
    public class AssemblyElementTest
    {
        [Fact]
        public void SutIsReflectionElement()
        {
            // Fixture setup
            // Exercise system
            var sut = new AssemblyElement(this.GetType().Assembly);
            // Verify outcome
            Assert.IsAssignableFrom<IReflectionElement>(sut);
            // Teardown
        }

        [Fact]
        public void AssemblyIsCorrect()
        {
            // Fixture setup
            var expectedAssembly = this.GetType().Assembly;
            var sut = new AssemblyElement(expectedAssembly);
            // Exercise system
            Assembly actualAssembly = sut.Assembly;
            // Verify outcome
            Assert.Equal(expectedAssembly, actualAssembly);
            // Teardown
        }

        [Fact]
        public void ConstructWithNullAssemblyThrows()
        {
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new AssemblyElement(null));
            // Teardown
        }

        [Fact]
        public void AcceptNullVisitorThrows()
        {
            // Fixture setup
            var sut = new AssemblyElement(this.GetType().Assembly);
            // Exercise system
            // Verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Accept((IReflectionVisitor<object>)null));
            // Teardown
        }

        [Fact]
        public void AcceptEntersItselfThenVisitsAllTypesThenExitsItself()
        {
            // Fixture setup
            var assembly = this.GetType().Assembly;
            var sut = new AssemblyElement(assembly);
            var expectedElements = new List<IReflectionElement>();
            expectedElements.Add(sut); // enter
            expectedElements.AddRange(assembly.GetTypes().Select(t => new TypeElement(t)));
            expectedElements.Add(sut); // exit

            var observedElements = new List<IReflectionElement>();
            var dummyVisitor = new DelegatingReflectionVisitor<bool>
            {
                OnEnterAssemblyElement = observedElements.Add,
                OnEnterTypeElement = observedElements.Add,
                OnExitAssemblyElement = observedElements.Add,
            };
            // Exercise system
            sut.Accept(dummyVisitor);
            // Verify outcome
            Assert.True(expectedElements.SequenceEqual(observedElements));
            // Teardown
        }
    }
}
