using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.VisitReflect.UnitTest
{
    public class FieldInfoElementTest
    {
        [Fact]
        public void SutIsReflectionElement()
        {
            // Fixture setup
            // Exercise system
            var sut = new FieldInfoElement(typeof(TypeWithField<int>).GetFields().First());
            // Verify outcome
            Assert.IsAssignableFrom<IReflectionElement>(sut);
            // Teardown
        }

        [Fact]
        public void SutIsHierarchicalReflectionElement()
        {
            // Fixture setup
            // Exercise system
            var sut = new FieldInfoElement(typeof(TypeWithField<int>).GetFields().First());
            // Verify outcome
            Assert.IsAssignableFrom<IHierarchicalReflectionElement>(sut);
            // Teardown
        }

        [Fact]
        public void FieldInfoIsCorrect()
        {
            // Fixture setup
            var expectedField = typeof(TypeWithField<int>).GetFields().First();
            var sut = new FieldInfoElement(expectedField);
            // Exercise system
            var actual = sut.FieldInfo;
            // Verify outcome
            Assert.Equal(expectedField, actual);
            // Teardown
        }

        [Fact]
        public void ConstructWithNullFieldInfoThrows()
        {
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new FieldInfoElement(null));
            // Teardown
        }

        [Fact]
        public void AcceptNullVisitorThrows()
        {
            // Fixture setup
            var sut = new FieldInfoElement(typeof(TypeWithField<int>).GetFields().First());
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
            var observed = new List<FieldInfoElement>();
            var dummyVisitor = new DelegatingReflectionVisitor<int> { OnVisitFieldInfoElement = observed.Add };
            var sut = new FieldInfoElement(typeof(TypeWithField<int>).GetFields().First());
            // Exercise system
            sut.Accept(dummyVisitor);
            // Verify outcome
            Assert.True(new[] { sut }.SequenceEqual(observed));
            // Teardown
        }

        public class TypeWithField<T>
        {
            public TypeWithField(T field)
            {
            }

            public T Field;
        }

    }
}
