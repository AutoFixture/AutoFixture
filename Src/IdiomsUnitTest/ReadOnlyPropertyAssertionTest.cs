using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class ReadOnlyPropertyAssertionTest
    {
        [Fact]
        public void SutIsIdiomaticAssertion()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            // Exercise system
            var sut = new ReadOnlyPropertyAssertion(dummyComposer);
            // Verify outcome
            Assert.IsAssignableFrom<IdiomaticAssertion>(sut);
            // Teardown
        }

        [Fact]
        public void ComposerIsCorrect()
        {
            // Fixture setup
            var expectedComposer = new Fixture();
            var sut = new ReadOnlyPropertyAssertion(expectedComposer);
            // Exercise system
            ISpecimenBuilder result = sut.Builder;
            // Verify outcome
            Assert.Equal(expectedComposer, result);
            // Teardown
        }

        [Fact]
        public void ConstructWithNullComposerThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new ReadOnlyPropertyAssertion(null));
            // Teardown
        }

        [Fact]
        public void VerifyNullPropertyThrows()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new ReadOnlyPropertyAssertion(dummyComposer);
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify((PropertyInfo)null));
            // Teardown
        }

        [Fact]
        public void VerifyNullConstructorInfoThrows()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new ReadOnlyPropertyAssertion(dummyComposer);
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify((ConstructorInfo)null));
            // Teardown
        }

        [Fact]
        public void VerifyDefaultConstructorDoesNotThrow()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new ReadOnlyPropertyAssertion(dummyComposer);
            // Exercise system and verify outcome
            var constructorWithNoParameters = typeof (PropertyHolder<object>).GetConstructors().First();
            Assert.Equal(0, constructorWithNoParameters.GetParameters().Length);
            Assert.DoesNotThrow(() =>
                sut.Verify(constructorWithNoParameters));
            // Teardown
        }

        [Fact]
        public void VerifyWritablePropertyDoesNotThrow()
        {
            // Fixture setup
            var composer = new Fixture();
            var sut = new ReadOnlyPropertyAssertion(composer);

            var propertyInfo = typeof(PropertyHolder<object>).GetProperty("Property");
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() =>
                sut.Verify(propertyInfo));
            // Teardown
        }

        [Fact]
        public void VerifyReadOnlyPropertyWithNoMatchingConstructorThrows()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new ReadOnlyPropertyAssertion(dummyComposer);
            var propertyInfo = typeof(ReadOnlyPropertyHolder<object>).GetProperty("Property");
            // Exercise system and verify outcome
            var e = Assert.Throws<ReadOnlyPropertyException>(() =>
                sut.Verify(propertyInfo));
            Assert.Equal(propertyInfo, e.PropertyInfo);
            // Teardown
        }
        
        [Fact]
        public void VerifyWellBehavedReadOnlyPropertyInitialisedViaConstructorDoesNotThrow()
        {
            // Given a PropertyInfo or FieldInfo, it should verify that for all 
            // constructor having a matching argument, the value should be preserved
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new ReadOnlyPropertyAssertion(dummyComposer);
            var propertyInfo = typeof(ReadOnlyPropertyInitialisedViaConstructor<object>).GetProperty("Property");
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Verify(propertyInfo));
            // Teardown
        }

        [Fact]
        public void VerifyIllBehavedReadOnlyPropertiesInitialisedViaConstructorThrows()
        {
            // Given a PropertyInfo or FieldInfo, it should verify that for all 
            // constructor having a matching argument, the value should be preserved
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new ReadOnlyPropertyAssertion(dummyComposer);
            var illBehavedType = typeof (ReadOnlyPropertiesIncorrectlyInitialisedViaConstructor<object, object>);
            var propertyInfo1 = illBehavedType.GetProperty("Property1");
            var propertyInfo2 = illBehavedType.GetProperty("Property2");
            // Exercise system and verify outcome
            var e1 = Assert.Throws<ReadOnlyPropertyException>(() => sut.Verify(propertyInfo1));
            Assert.Equal(propertyInfo1, e1.PropertyInfo);
            var e2 = Assert.Throws<ReadOnlyPropertyException>(() => sut.Verify(propertyInfo2));
            Assert.Equal(propertyInfo2, e2.PropertyInfo);
            // Teardown
        }

        [Fact]
        public void VerifyWellBehavedReadOnlyFieldInitialisedViaConstructorDoesNotThrow()
        {
            // Given a PropertyInfo or FieldInfo, it should verify that for all 
            // constructor having a matching argument, the value should be preserved
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new ReadOnlyPropertyAssertion(dummyComposer);
            var fieldInfo = typeof(ReadOnlyFieldInitialisedViaConstructor<object>).GetField("Field");
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Verify(fieldInfo));
            // Teardown
        }

        [Fact]
        public void VerifyAllConstructorArgumentsAreExposedAsFieldsDoesNotThrow()
        {
            // Given a ConstructorInfo, it should verify that all constructor arguments 
            // are properly exposed as either fields or Inspection Properties.
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new ReadOnlyPropertyAssertion(dummyComposer);
            var ctor1 = typeof(ReadOnlyFieldsInitialisedViaConstructor<object, int>).GetConstructor(new[] { typeof(object) });
            var ctor2 = typeof(ReadOnlyFieldsInitialisedViaConstructor<object, int>).GetConstructor(new[] { typeof(int) });
            var ctor3 = typeof(ReadOnlyFieldsInitialisedViaConstructor<object, int>).GetConstructor(new[] { typeof(object), typeof(int) });
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Verify(ctor1));
            Assert.DoesNotThrow(() => sut.Verify(ctor2));
            Assert.DoesNotThrow(() => sut.Verify(ctor3));
            // Teardown
        }

        [Fact]
        public void VerifyWhenNotAllConstructorArgumentsAreExposedAsFieldsThrows()
        {
            // Given a ConstructorInfo, it should verify that all constructor arguments 
            // are properly exposed as either fields or Inspection Properties.
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new ReadOnlyPropertyAssertion(dummyComposer);
            var ctor = typeof (ReadOnlyFieldsInitialisedViaConstructor<object, int>)
                .GetConstructor(new[] {typeof (object), typeof (int), typeof (TriState)});
            // Exercise system and verify outcome
            Assert.Throws<ReadOnlyPropertyException>(() => sut.Verify(ctor));
            // Teardown
        }

        [Fact]
        public void VerifyConstructorArgumentsAreExposedAsProperties()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new ReadOnlyPropertyAssertion(dummyComposer);
            var ctor1 = typeof(ReadOnlyPropertiesInitialisedViaConstructor<object, int>).GetConstructor(new[] { typeof(object) });
            var ctor2 = typeof(ReadOnlyPropertiesInitialisedViaConstructor<object, int>).GetConstructor(new[] { typeof(int) });
            var ctor3 = typeof(ReadOnlyPropertiesInitialisedViaConstructor<object, int>).GetConstructor(new[] { typeof(object), typeof(int) });
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Verify(ctor1));
            Assert.DoesNotThrow(() => sut.Verify(ctor2));
            Assert.DoesNotThrow(() => sut.Verify(ctor3));
            // Teardown
        }

        [Fact]
        public void VerifyWhenNotAllConstructorArgumentsAreExposedAsPropertiesThrows()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new ReadOnlyPropertyAssertion(dummyComposer);
            var ctor = typeof (ReadOnlyPropertiesInitialisedViaConstructor<object, int>)
                .GetConstructor(new[] {typeof (object), typeof (int), typeof (TriState)});
            // Exercise system and verify outcome
            Assert.Throws<ReadOnlyPropertyException>(() => sut.Verify(ctor));
            // Teardown
        }

        class ReadOnlyFieldInitialisedViaConstructor<T>
        {
            public readonly T Field;

            public ReadOnlyFieldInitialisedViaConstructor(T value)
            {
                this.Field = value;
            }
        }

        class ReadOnlyPropertiesInitialisedViaConstructor<T1, T2>
        {
            public ReadOnlyPropertiesInitialisedViaConstructor(T1 property1)
            {
                this.Property1 = property1;
            }

            public ReadOnlyPropertiesInitialisedViaConstructor(T2 property2)
            {
                this.Property2 = property2;
            }

            public ReadOnlyPropertiesInitialisedViaConstructor(T1 property1, T2 property2)
            {
                this.Property1 = property1;
                this.Property2 = property2;
            }

            public ReadOnlyPropertiesInitialisedViaConstructor(T1 property1, T2 property2, TriState noMatchingProperty)
            {
                this.Property1 = property1;
                this.Property2 = property2;
            }

            public T1 Property1 { get; private set; }
            public T2 Property2 { get; private set; }
        }

        class ReadOnlyPropertiesIncorrectlyInitialisedViaConstructor<T1, T2>
        {
            public ReadOnlyPropertiesIncorrectlyInitialisedViaConstructor(T1 property1, T2 property2)
            {
            }

            public ReadOnlyPropertiesIncorrectlyInitialisedViaConstructor(T1 property1)
            {
            }

            public ReadOnlyPropertiesIncorrectlyInitialisedViaConstructor(T2 property2)
            {
            }

            public T1 Property1 { get; private set; }
            public T2 Property2 { get; private set; }
        }

        class ReadOnlyFieldsInitialisedViaConstructor<T1, T2>
        {
            public ReadOnlyFieldsInitialisedViaConstructor(T1 field1)
            {
                this.Field1 = field1;
            }

            public ReadOnlyFieldsInitialisedViaConstructor(T2 field2)
            {
                this.Field2 = field2;
            }

            public ReadOnlyFieldsInitialisedViaConstructor(T1 field1, T2 field2)
            {
                this.Field1 = field1;
                this.Field2 = field2;
            }

            public ReadOnlyFieldsInitialisedViaConstructor(T1 field1, T2 field2, TriState noMatchingField)
            {
                this.Field1 = field1;
                this.Field2 = field2;
            }

            public T1 Field1;
            public T2 Field2;
        }

        class ReadOnlyPropertyInitialisedViaConstructor<T>
        {
            public ReadOnlyPropertyInitialisedViaConstructor(T property)
            {
                this.Property = property;
            }

            public T Property { get; private set; }
        }

    }
}
