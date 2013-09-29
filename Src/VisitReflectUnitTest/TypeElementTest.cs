using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;

namespace Ploeh.VisitReflect.UnitTest
{
    public class TypeElementTest
    {
        [Fact]
        public void SutIsReflectionElement()
        {
            // Fixture setup
            var type = this.GetType();
            // Exercise system
            var sut = new TypeElement(type);
            // Verify outcome
            Assert.IsAssignableFrom<IReflectionElement>(sut);
            // Teardown
        }

        [Fact]
        public void TypeIsCorrect()
        {
            // Fixture setup
            var type = this.GetType();
            var sut = new TypeElement(type);
            // Exercise system
            var actual = sut.Type;
            // Verify outcome
            Assert.Equal(type, actual);
            // Teardown
        }

        [Fact]
        public void ConstructWithNullTypeThrows()
        {
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new TypeElement(null));
            // Teardown
        }

        [Fact]
        public void AcceptNullVisitorThrows()
        {
            // Fixture setup
            var sut = new TypeElement(this.GetType());
            // Exercise system
            // Verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Accept((IReflectionVisitor<object>)null));
            // Teardown
        }
        
        [Fact]
        public void AcceptEntersItselfThenVisitsChildElementsThenExitsItself()
        {
            // Fixture setup
            var type = typeof (TypeWithCtorMethodPropertyField);
            var sut = new TypeElement(type);

            var ctor = type.GetConstructors().First();
            var ctorParameters = ctor.GetParameters();
            var method = type.GetMethods().First();
            var methodParameters = method.GetParameters();

            var expectedElements = new List<IReflectionElement>();
            expectedElements.Add(sut);
            expectedElements.Add(new ConstructorInfoElement(ctor));
            expectedElements.AddRange(ctorParameters.Select(p => new ParameterInfoElement(p)));
            expectedElements.Add(new MethodInfoElement(method));
            expectedElements.AddRange(methodParameters.Select(p => new ParameterInfoElement(p)));
            expectedElements.AddRange(type.GetProperties().Select(p => new PropertyInfoElement(p)));
            expectedElements.AddRange(type.GetFields().Select(f => new FieldInfoElement(f)));

            var observedElements = new List<IReflectionElement>();
            var dummyVisitor = new DelegatingReflectionVisitor<bool>
            {
                OnEnterMethodInfoElement = observedElements.Add,
                OnEnterConstructorInfoElement = observedElements.Add,
                OnVisitFieldInfoElement = observedElements.Add,
                OnVisitParameterInfoElement = observedElements.Add,
                OnVisitPropertyInfoElement = observedElements.Add,
                OnEnterTypeElement = observedElements.Add,
                OnExitTypeElement = observedElements.Add,
            };

            // Exercise system
            sut.Accept(dummyVisitor);
            // Verify outcome
            Assert.True(expectedElements.All(observedElements.Contains));
            // Teardown
        }

        class TypeWithCtorMethodPropertyField
        {
            public void Method1(int methodParam1)
            {
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "ctorParam1", Justification = "It's used via reflection.")]
            public TypeWithCtorMethodPropertyField(int ctorParam1)
            {
            }

            public int Property1 { get; set; }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "It's used via reflection.")]
            public int Field1;

        }
    }
}
