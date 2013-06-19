using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class ConstructorInitializedMemberAssertionTest
    {
        [Fact]
        public void SutIsIdiomaticAssertion()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            // Exercise system
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            // Verify outcome
            Assert.IsAssignableFrom<IdiomaticAssertion>(sut);
            // Teardown
        }

        [Fact]
        public void ComposerIsCorrect()
        {
            // Fixture setup
            var expectedComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(expectedComposer);
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
                new ConstructorInitializedMemberAssertion(null));
            // Teardown
        }

        [Fact]
        public void VerifyNullPropertyThrows()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
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
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
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
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            // Exercise system and verify outcome
            var constructorWithNoParameters = typeof (PropertyHolder<object>).GetConstructors().First();
            Assert.Equal(0, constructorWithNoParameters.GetParameters().Length);
            Assert.DoesNotThrow(() =>
                sut.Verify(constructorWithNoParameters));
            // Teardown
        }

        [Fact]
        public void VerifyWritablePropertyWithNoMatchingConstructorThrows()
        {
            // Fixture setup
            var composer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(composer);
            var propertyInfo = typeof(PropertyHolder<object>).GetProperty("Property");
            // Exercise system and verify outcome
            var e = Assert.Throws<ConstructorInitializedMemberException>(() =>
                sut.Verify(propertyInfo));
            Assert.Equal(propertyInfo, e.MemberInfo);
            Assert.Equal(propertyInfo, e.PropertyInfo);
            Assert.Null(e.FieldInfo);
            // Teardown
        }

        [Fact]
        public void VerifyReadOnlyPropertyWithNoMatchingConstructorThrows()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var propertyInfo = typeof(ReadOnlyPropertyHolder<object>).GetProperty("Property");
            // Exercise system and verify outcome
            var e = Assert.Throws<ConstructorInitializedMemberException>(() =>
                sut.Verify(propertyInfo));
            Assert.Equal(propertyInfo, e.MemberInfo);
            Assert.Equal(propertyInfo, e.PropertyInfo);
            Assert.Null(e.FieldInfo);
            // Teardown
        }
        
        [Fact]
        public void VerifyWellBehavedReadOnlyPropertyInitializedViaConstructorDoesNotThrow()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var propertyInfo = typeof(ReadOnlyPropertyInitializedViaConstructor<object>).GetProperty("Property");
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Verify(propertyInfo));
            // Teardown
        }

        [Fact]
        public void VerifyIllBehavedReadOnlyPropertiesInitializedViaConstructorThrows()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var illBehavedType = typeof (PropertiesIncorrectlyInitializedViaConstructor<object, object>);
            var propertyInfo1 = illBehavedType.GetProperty("Property1");
            var propertyInfo2 = illBehavedType.GetProperty("Property2");
            // Exercise system and verify outcome
            var e1 = Assert.Throws<ConstructorInitializedMemberException>(() => sut.Verify(propertyInfo1));
            Assert.Equal(e1.MemberInfo, propertyInfo1);
            Assert.Equal(propertyInfo1, e1.PropertyInfo);
            Assert.Null(e1.FieldInfo);
            var e2 = Assert.Throws<ConstructorInitializedMemberException>(() => sut.Verify(propertyInfo2));
            Assert.Equal(propertyInfo2, e2.MemberInfo);
            Assert.Equal(propertyInfo2, e2.PropertyInfo);
            Assert.Null(e2.FieldInfo);
            // Teardown
        }

        [Fact]
        public void VerifyWellBehavedReadOnlyFieldInitializedViaConstructorDoesNotThrow()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var fieldInfo = typeof(ReadOnlyFieldInitializedViaConstructor<object>).GetField("Field");
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Verify(fieldInfo));
            // Teardown
        }

        [Fact]
        public void VerifyReadOnlyFieldInitializedViaConstructorWithDifferentTypeThrows()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var fieldInfo = typeof(ReadOnlyFieldInitializedViaConstructorWithDifferentType).GetField("Field");
            // Exercise system and verify outcome
            var e = Assert.Throws<ConstructorInitializedMemberException>(() => sut.Verify(fieldInfo));
            Assert.Equal(fieldInfo, e.MemberInfo);
            Assert.Equal(fieldInfo, e.FieldInfo);
            Assert.Null(e.PropertyInfo);
            // Teardown
        }

        [Fact]
        public void VerifyAllConstructorArgumentsAreExposedAsFieldsDoesNotThrow()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var ctor1 = typeof(FieldsInitializedViaConstructor<object, int>).GetConstructor(new[] { typeof(object) });
            var ctor2 = typeof(FieldsInitializedViaConstructor<object, int>).GetConstructor(new[] { typeof(int) });
            var ctor3 = typeof(FieldsInitializedViaConstructor<object, int>).GetConstructor(new[] { typeof(object), typeof(int) });
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Verify(ctor1));
            Assert.DoesNotThrow(() => sut.Verify(ctor2));
            Assert.DoesNotThrow(() => sut.Verify(ctor3));
            // Teardown
        }

        [Fact]
        public void VerifyWhenNotAllConstructorArgumentsAreExposedAsFieldsThrows()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var ctor = typeof (FieldsInitializedViaConstructor<object, int>)
                .GetConstructor(new[] {typeof (object), typeof (int), typeof (TriState)});
            // Exercise system and verify outcome
            Assert.Throws<ConstructorInitializedMemberException>(() => sut.Verify(ctor));
            // Teardown
        }

        [Fact]
        public void VerifyConstructorArgumentsAreExposedAsProperties()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var ctor1 = typeof(ReadOnlyPropertiesInitializedViaConstructor<object, int>).GetConstructor(new[] { typeof(object) });
            var ctor2 = typeof(ReadOnlyPropertiesInitializedViaConstructor<object, int>).GetConstructor(new[] { typeof(int) });
            var ctor3 = typeof(ReadOnlyPropertiesInitializedViaConstructor<object, int>).GetConstructor(new[] { typeof(object), typeof(int) });
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
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var ctor = typeof(ReadOnlyPropertiesInitializedViaConstructor<object, int>)
                .GetConstructor(new[] { typeof(object), typeof(int), typeof(TriState) });
            // Exercise system and verify outcome
            Assert.Throws<ConstructorInitializedMemberException>(() => sut.Verify(ctor));
            // Teardown
        }

        [Fact]
        public void VerifyWhenConstructorArgumentTypeIsDifferentToFieldThrows()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var ctor = typeof(ReadOnlyFieldInitializedViaConstructorWithDifferentType).GetConstructors().First();
            // Exercise system and verify outcome
            Assert.Throws<ConstructorInitializedMemberException>(() => sut.Verify(ctor));
            // Teardown
        }

        [Fact]
        public void VerifyWhenPropertyTypeIsAssignableFromParameterTypeDoesNotThrow()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var ctor = typeof(PropertyIsAssignableFromConstructorArgumentType).GetConstructors().First();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() =>
                sut.Verify(ctor));
            // Teardown
        }

        public class PropertyIsAssignableFromConstructorArgumentType
        {
            private readonly IEnumerable<string> bribbets;

            public PropertyIsAssignableFromConstructorArgumentType(params string[] bribbets)
            {
                this.bribbets = bribbets;
            }

            public IEnumerable<string> Bribbets
            {
                get { return this.bribbets; }
            }
        }

        class ReadOnlyFieldInitializedViaConstructorWithDifferentType
        {
            public readonly string Field;

            public ReadOnlyFieldInitializedViaConstructorWithDifferentType(int value)
            {
                this.Field = value.ToString(CultureInfo.CurrentCulture);
            }
        }

        class ReadOnlyFieldInitializedViaConstructor<T>
        {
            public readonly T Field;

            public ReadOnlyFieldInitializedViaConstructor(T field)
            {
                this.Field = field;
            }
        }

        class ReadOnlyPropertiesInitializedViaConstructor<T1, T2>
        {
            public ReadOnlyPropertiesInitializedViaConstructor(T1 property1)
            {
                this.Property1 = property1;
            }

            public ReadOnlyPropertiesInitializedViaConstructor(T2 property2)
            {
                this.Property2 = property2;
            }

            public ReadOnlyPropertiesInitializedViaConstructor(T1 property1, T2 property2)
            {
                this.Property1 = property1;
                this.Property2 = property2;
            }

            public ReadOnlyPropertiesInitializedViaConstructor(T1 property1, T2 property2, TriState noMatchingProperty)
            {
                this.Property1 = property1;
                this.Property2 = property2;
            }

            public T1 Property1 { get; private set; }
            public T2 Property2 { get; private set; }
        }

        class PropertiesIncorrectlyInitializedViaConstructor<T1, T2>
        {
            public PropertiesIncorrectlyInitializedViaConstructor(T1 property1, T2 property2)
            {
            }

            public PropertiesIncorrectlyInitializedViaConstructor(T1 property1)
            {
            }

            public PropertiesIncorrectlyInitializedViaConstructor(T2 property2)
            {
            }

            public T1 Property1 { get; set; }
            public T2 Property2 { get; set; }
        }

        class FieldsInitializedViaConstructor<T1, T2>
        {
            public FieldsInitializedViaConstructor(T1 field1)
            {
                this.Field1 = field1;
            }

            public FieldsInitializedViaConstructor(T2 field2)
            {
                this.Field2 = field2;
            }

            public FieldsInitializedViaConstructor(T1 field1, T2 field2)
            {
                this.Field1 = field1;
                this.Field2 = field2;
            }

            public FieldsInitializedViaConstructor(T1 field1, T2 field2, TriState noMatchingField)
            {
                this.Field1 = field1;
                this.Field2 = field2;
            }

            public T1 Field1;
            public T2 Field2;
        }

        class ReadOnlyPropertyInitializedViaConstructor<T>
        {
            public ReadOnlyPropertyInitializedViaConstructor(T property)
            {
                this.Property = property;
            }

            public T Property { get; private set; }
        }

    }
}
