using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.VisitReflect.UnitTest
{
    public class CompositeReflectionElementTest
    {
        [Fact]
        public void SutIsReflectionElement()
        {
            // Fixture setup
            // Exercise system
            var sut = new CompositeReflectionElement();
            // Verify outcome
            Assert.IsAssignableFrom<IReflectionElement>(sut);
            // Teardown
        }

        [Fact]
        public void AcceptCallsAcceptOnAllMultipleElementTypes()
        {
            // Fixture setup
            var observedElements = new List<IReflectionElement>();
            var elements = new IReflectionElement[]
            {
                new AssemblyElement(this.GetType().Assembly), 
                new TypeElement(this.GetType()), 
                new ConstructorInfoElement(this.GetType().GetConstructors().First()),
                new MethodInfoElement(typeof(UnguardedMethodHost).GetMethods().First()), 
                new PropertyInfoElement(typeof(PropertyHolder<int>).GetProperties().First()), 
                new FieldInfoElement(typeof(FieldHolder<int>).GetFields().First()), 
                new ParameterInfoElement(typeof(UnguardedMethodHost).GetMethods().First().GetParameters()[0]), 
            };

            var expectedElements = new List<IReflectionElement>(elements);

            var dummyVisitor = new DelegatingReflectionVisitor<int>
            {
                OnEnterAssemblyElement = observedElements.Add,
                OnEnterTypeElement = observedElements.Add,
                OnEnterConstructorInfoElement = observedElements.Add,
                OnVisitFieldInfoElement = observedElements.Add,
                OnEnterMethodInfoElement = observedElements.Add,
                OnVisitParameterInfoElement = observedElements.Add,
                OnVisitPropertyInfoElement = observedElements.Add,
            };

            var sut = new CompositeReflectionElement(elements);
            // Exercise system
            sut.Accept(dummyVisitor);
            // Verify outcome
            Assert.True(expectedElements.All(observedElements.Contains));
            // Teardown
        }

        [Fact]
        public void AcceptNullVisitorThrows()
        {
            // Fixture setup
            var sut = new CompositeReflectionElement();
            // Exercise system
            // Verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Accept((IReflectionVisitor<object>)null));
            // Teardown
        }
    }
}
