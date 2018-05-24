using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Albedo;
using AutoFixture.Idioms;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.IdiomsUnitTest
{
    public class ConstructorInitializedMemberAssertionTest
    {
        [Fact]
        public void SutIsIdiomaticAssertion()
        {
            // Arrange
            var dummyComposer = new Fixture();
            // Act
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            // Assert
            Assert.IsAssignableFrom<IdiomaticAssertion>(sut);
        }

        [Fact]
        public void ComposerIsCorrect()
        {
            // Arrange
            var expectedComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(expectedComposer);
            // Act
            ISpecimenBuilder result = sut.Builder;
            // Assert
            Assert.Equal(expectedComposer, result);
        }

        [Fact]
        public void ConstructWithNullComposerThrows()
        {
            // Arrange
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new ConstructorInitializedMemberAssertion(null));
        }

        [Fact]
        public void ComparerIsCorrect()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var expectedComparer = new FakeEqualityComparer<object>();
            var dummyMatcher = new FakeReflectionElementComparer();
            var sut = new ConstructorInitializedMemberAssertion(
                dummyComposer, expectedComparer, dummyMatcher);
            // Act
            IEqualityComparer result = sut.Comparer;
            // Assert
            Assert.Equal(expectedComparer, result);
        }

        [Fact]
        public void ConstructWithNullComparerThrows()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var dummyMatcher = new FakeReflectionElementComparer();
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new ConstructorInitializedMemberAssertion(dummyComposer, null, dummyMatcher));
        }

        [Fact]
        public void ParameterMemberMatcherIsCorrect()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var dummyComparer = new FakeEqualityComparer<object>();
            var expectedMatcher = new FakeReflectionElementComparer();
            var sut = new ConstructorInitializedMemberAssertion(
                dummyComposer, dummyComparer, expectedMatcher);
            // Act
            IEqualityComparer<IReflectionElement> result = sut.ParameterMemberMatcher;
            // Assert
            Assert.Equal(expectedMatcher, result);
        }

        [Fact]
        public void ConstructWithNullParameterMemberMatcherThrows()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var dummyComparer = new FakeEqualityComparer<object>();
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new ConstructorInitializedMemberAssertion(dummyComposer, dummyComparer, null));
        }

        [Fact]
        public void VerifyNullPropertyThrows()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify((PropertyInfo)null));
        }

        [Fact]
        public void VerifyNullFieldThrows()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify((FieldInfo)null));
        }

        [Fact]
        public void VerifyNullConstructorInfoThrows()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify((ConstructorInfo)null));
        }

        [Fact]
        public void VerifyDefaultConstructorDoesNotThrow()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            // Act & Assert
            var constructorWithNoParameters = typeof (PropertyHolder<object>).GetConstructors().First();
            Assert.Empty(constructorWithNoParameters.GetParameters());
            Assert.Null(Record.Exception(() =>
                sut.Verify(constructorWithNoParameters)));
        }

        [Fact]
        public void VerifyWritablePropertyWithNoMatchingConstructorDoesNotThrow()
        {
            // Arrange
            var composer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(composer);
            var propertyInfo = typeof(PropertyHolder<object>).GetProperty("Property");
            // Act & Assert
            Assert.Null(Record.Exception(() =>
                sut.Verify(propertyInfo)));
        }

        [Fact]
        public void VerifyWritableFieldWithNoMatchingConstructorDoesNotThrow()
        {
            // Arrange
            var composer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(composer);
            var propertyInfo = typeof(FieldHolder<object>).GetField("Field");
            // Act & Assert
            Assert.Null(Record.Exception(() =>
                sut.Verify(propertyInfo)));
        }

        [Fact]
        public void VerifyReadOnlyPropertyWithPrivateSetterAndNoMatchingConstructorThrows()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var propertyInfo = typeof(ReadOnlyPropertyHolder<int>).GetProperty("Property");
            // Act & Assert
            var e = Assert.Throws<ConstructorInitializedMemberException>(() =>
                sut.Verify(propertyInfo));
            AssertExceptionPropertiesEqual(e, propertyInfo);
        }

        [Fact]
        public void VerifyReadOnlyPropertyWithNoSetterAndNoMatchingConstructorThrows()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var propertyInfo = typeof(ReadOnlyPropertyWithNoSetterHolder<int>).GetProperty("Property");
            // Act & Assert
            var e = Assert.Throws<ConstructorInitializedMemberException>(() =>
                sut.Verify(propertyInfo));
            AssertExceptionPropertiesEqual(e, propertyInfo);
        }

        [Fact]
        public void VerifyWellBehavedReadOnlyPropertyInitializedViaConstructorDoesNotThrow()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var propertyInfo = typeof(ReadOnlyPropertyInitializedViaConstructor<object>).GetProperty("Property");
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Verify(propertyInfo)));
        }

        [Fact]
        public void VerifyIllBehavedPropertiesInitializedViaConstructorThrows()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var illBehavedType = typeof (PropertiesIncorrectlyInitializedViaConstructor<object, object>);
            var propertyInfo1 = illBehavedType.GetProperty("Property1");
            var propertyInfo2 = illBehavedType.GetProperty("Property2");
            // Act & Assert
            var e1 = Assert.Throws<ConstructorInitializedMemberException>(() => sut.Verify(propertyInfo1));
            AssertExceptionPropertiesEqual(e1, propertyInfo1);
            var e2 = Assert.Throws<ConstructorInitializedMemberException>(() => sut.Verify(propertyInfo2));
            AssertExceptionPropertiesEqual(e2, propertyInfo2);
        }

        [Fact]
        public void VerifyWellBehavedReadOnlyFieldInitializedViaConstructorDoesNotThrow()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var fieldInfo = typeof(ReadOnlyFieldInitializedViaConstructor<object>).GetField("Field");
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Verify(fieldInfo)));
        }

        [Fact]
        public void VerifyReadOnlyFieldInitializedViaConstructorWithDifferentTypeThrows()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var fieldInfo = typeof(ReadOnlyFieldInitializedViaConstructorWithDifferentType).GetField("Field");
            // Act & Assert
            var e = Assert.Throws<ConstructorInitializedMemberException>(() => sut.Verify(fieldInfo));
            AssertExceptionPropertiesEqual(e, fieldInfo);
        }

        [Fact]
        public void VerifyAllConstructorArgumentsAreExposedAsFieldsDoesNotThrow()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var ctor1 = typeof(FieldsInitializedViaConstructor<object, int>).GetConstructor(new[] { typeof(object) });
            var ctor2 = typeof(FieldsInitializedViaConstructor<object, int>).GetConstructor(new[] { typeof(int) });
            var ctor3 = typeof(FieldsInitializedViaConstructor<object, int>).GetConstructor(new[] { typeof(object), typeof(int) });
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Verify(ctor1)));
            Assert.Null(Record.Exception(() => sut.Verify(ctor2)));
            Assert.Null(Record.Exception(() => sut.Verify(ctor3)));
        }

        [Fact]
        public void VerifyWhenNotAllConstructorArgumentsAreExposedAsFieldsThrows()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var ctor = typeof (FieldsInitializedViaConstructor<object, int>)
                .GetConstructor(new[] {typeof (object), typeof (int), typeof (TriState)});
            // Act & Assert
            var e = Assert.Throws<ConstructorInitializedMemberException>(() => sut.Verify(ctor));
            var expectedMissingParam = ctor.GetParameters().Single(p => p.Name == "noMatchingField");
            AssertExceptionPropertiesEqual(e, ctor, expectedMissingParam);
        }

        [Fact]
        public void VerifyConstructorArgumentsAreExposedAsProperties()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var ctor1 = typeof(ReadOnlyPropertiesInitializedViaConstructor<object, int>).GetConstructor(new[] { typeof(object) });
            var ctor2 = typeof(ReadOnlyPropertiesInitializedViaConstructor<object, int>).GetConstructor(new[] { typeof(int) });
            var ctor3 = typeof(ReadOnlyPropertiesInitializedViaConstructor<object, int>).GetConstructor(new[] { typeof(object), typeof(int) });
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Verify(ctor1)));
            Assert.Null(Record.Exception(() => sut.Verify(ctor2)));
            Assert.Null(Record.Exception(() => sut.Verify(ctor3)));
        }

        [Fact]
        public void VerifyWhenNotAllConstructorArgumentsAreExposedAsPropertiesThrows()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var ctor = typeof(ReadOnlyPropertiesInitializedViaConstructor<object, int>)
                .GetConstructor(new[] { typeof(object), typeof(int), typeof(TriState) });
            // Act & Assert
            var e = Assert.Throws<ConstructorInitializedMemberException>(() => sut.Verify(ctor));
            var expectedMissingParam = ctor.GetParameters().Single(p => p.Name == "noMatchingProperty");
            AssertExceptionPropertiesEqual(e, ctor, expectedMissingParam);
        }

        [Fact]
        public void VerifyWhenConstructorArgumentTypeIsDifferentToFieldThrows()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var ctor = typeof(ReadOnlyFieldInitializedViaConstructorWithDifferentType).GetConstructors().First();
            // Act & Assert
            var e = Assert.Throws<ConstructorInitializedMemberException>(() => sut.Verify(ctor));
            var expectedMissingParam = ctor.GetParameters().Single(p => p.Name == "value");
            AssertExceptionPropertiesEqual(e, ctor, expectedMissingParam);
        }

        [Fact]
        public void VerifyWhenPropertyTypeIsAssignableFromParameterTypeShouldThrow()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var ctor = typeof(PropertyIsAssignableFromConstructorArgumentType).GetConstructors().First();
            // Act & Assert
            Assert.Throws<ConstructorInitializedMemberException>(() =>
                sut.Verify(ctor));
        }

        [Fact]
        public void VerifyWhenMemberTypeIsComplexDoesNotThrow()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var ctor = typeof(ReadOnlyPropertiesInitializedViaConstructor<ComplexType, object>)
                .GetConstructor(new[] { typeof(ComplexType), typeof(object) });
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Verify(ctor)));
        }

        [Fact]
        public void VerifyWhenMemberTypeIsComplexWithIllBehavedConstructorThrows()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var ctor = typeof(ReadOnlyPropertiesInitializedViaConstructor<ComplexType, object>)
                .GetConstructor(new[] { typeof(ComplexType), typeof(object), typeof(TriState) });
            // Act & Assert
            var e = Assert.Throws<ConstructorInitializedMemberException>(
                () => sut.Verify(ctor));
            var expectedMissingParameter = ctor.GetParameters().Single(p => p.Name == "noMatchingProperty");
            AssertExceptionPropertiesEqual(e, ctor, expectedMissingParameter);
        }

        [Fact]
        public void VerifyWhenConstructorArgumentHasWriteOnlyPropertyDoesNotThrow()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var ctor = typeof(WriteOnlyPropertyHolder<ComplexType>).GetConstructors().First();
            // Act & Assert
            Assert.Null(
                Record.Exception(() => sut.Verify(ctor)));
        }

        [Fact]
        public void VerifyWhenPropertyIsWriteOnlyDoesNotThrow()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var propertyInfo = typeof(WriteOnlyPropertyHolder<ComplexType>).GetProperty("WriteOnlyProperty");
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Verify(propertyInfo)));
        }

        [Fact]
        public void VerifyWhenPropertyGetterIsInternalDoesNotThrow()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var propertyInfo = typeof(InternalGetterPropertyHolder<ComplexType>).GetProperty("Property");
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Verify(propertyInfo)));
        }

        [Fact]
        public void VerifyTypeWithPublicWritablePropertyAndNoMatchingConstructorArgumentDoesNotThrow()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var typeToVerify = typeof(PropertyHolder<ComplexType>);
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Verify(typeToVerify)));
        }

        [Fact]
        public void VerifyTypeWithPublicStaticPropertyAndNoMatchingConstructorArgumentDoesNotThrow()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var typeToVerify = typeof(StaticPropertyHolder<ComplexType>);
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Verify(typeToVerify)));
        }

        [Fact]
        public void VerifyTypeWithPublicStaticFieldAndNoMatchingConstructorArgumentDoesNotThrow()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var typeToVerify = typeof(StaticFieldHolder<ComplexType>);
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Verify(typeToVerify)));
        }

        [Fact]
        public void VerifyTypeWithPublicStaticReadOnlyFieldAndNoMatchingGuardedConstuctorArgumentDoesNotThrow()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var typeToVerify = typeof(GuardedConstructorHostHoldingStaticReadOnlyField<ComplexType, int>);
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Verify(typeToVerify)));
        }

        [Fact]
        public void VerifyTypeWithPublicStaticReadOnlyPropertyAndNoMatchingGuardedConstuctorArgumentDoesNotThrow()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var typeToVerify = typeof(GuardedConstructorHostHoldingStaticReadOnlyProperty<ComplexType, int>);
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Verify(typeToVerify)));
        }

        [Fact]
        public void VerifyTypeWithReadOnlyPropertyAndIllBehavedConstructorThrows()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var typeToVerify = typeof(ReadOnlyPropertiesInitializedViaConstructor<int, string>);
            // Act & Assert
            var e = Assert.Throws<ConstructorInitializedMemberException>(() => sut.Verify(typeToVerify));
            var expectedFailingConstructor = typeToVerify.GetConstructor(new[] { typeof(int), typeof(string), typeof(TriState) });
            var expectedFailingParameter = expectedFailingConstructor.GetParameters().Single(p => p.Name == "noMatchingProperty");
            AssertExceptionPropertiesEqual(e, expectedFailingConstructor, expectedFailingParameter);
        }

        [Fact]
        public void VerifyTypeWithWritablePropertyAndMatchingIllBehavedConstructorArgumentThrows()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var typeToVerify = typeof(WritablePropertyAndIllBehavedConstructor);
            // Act & Assert
            var e = Assert.Throws<ConstructorInitializedMemberException>(() => sut.Verify(typeToVerify));
            var expectedFailingProperty = typeToVerify.GetProperties().Single();
            AssertExceptionPropertiesEqual(e, expectedFailingProperty);
        }

        [Fact]
        public void VerifyTypeWithPublicReadOnlyFieldsIncorrectlyInitialisedViaConstructorThrows()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var typeToVerify = typeof(PublicReadOnlyFieldNotInitializedByConstructor);
            // Act & Assert
            var e = Assert.Throws<ConstructorInitializedMemberException>(() => sut.Verify(typeToVerify));
            var expectedFailingField = typeToVerify.GetFields().First();
            AssertExceptionPropertiesEqual(e, expectedFailingField);
        }

        [Theory]
        [InlineData(typeof(ReadOnlyFieldInitializedViaConstructor<TestIntEnum>))]
        [InlineData(typeof(ReadOnlyPropertyInitializedViaConstructor<TestIntEnum>))]
        [InlineData(typeof(ReadOnlyFieldInitializedViaConstructor<TestByteEnum>))]
        [InlineData(typeof(ReadOnlyPropertyInitializedViaConstructor<TestByteEnum>))]
        [InlineData(typeof(ReadOnlyFieldInitializedViaConstructor<TestSByteEnum>))]
        [InlineData(typeof(ReadOnlyPropertyInitializedViaConstructor<TestSByteEnum>))]
        [InlineData(typeof(ReadOnlyFieldInitializedViaConstructor<TestShortEnum>))]
        [InlineData(typeof(ReadOnlyPropertyInitializedViaConstructor<TestShortEnum>))]
        [InlineData(typeof(ReadOnlyFieldInitializedViaConstructor<TestUShortEnum>))]
        [InlineData(typeof(ReadOnlyPropertyInitializedViaConstructor<TestUShortEnum>))]
        [InlineData(typeof(ReadOnlyFieldInitializedViaConstructor<TestUIntEnum>))]
        [InlineData(typeof(ReadOnlyPropertyInitializedViaConstructor<TestUIntEnum>))]
        [InlineData(typeof(ReadOnlyFieldInitializedViaConstructor<TestLongEnum>))]
        [InlineData(typeof(ReadOnlyPropertyInitializedViaConstructor<TestLongEnum>))]
        [InlineData(typeof(ReadOnlyFieldInitializedViaConstructor<TestULongEnum>))]
        [InlineData(typeof(ReadOnlyPropertyInitializedViaConstructor<TestULongEnum>))]
        [InlineData(typeof(ReadOnlyFieldInitializedViaConstructor<TestSingleNonDefaultEnum>))]
        [InlineData(typeof(ReadOnlyPropertyInitializedViaConstructor<TestSingleNonDefaultEnum>))]
        public void VerifyWellBehavedEnumInitializersDoNotThrow(Type type)
        {
            // Arrange
            var fixture = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(fixture);

            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Verify(type)));
        }

        [Theory]
        [InlineData(typeof(ReadOnlyFieldIncorrectlyInitializedViaConstructor<TestIntEnum>))]
        [InlineData(typeof(ReadOnlyPropertyIncorrectlyInitializedViaConstructor<TestIntEnum>))]
        [InlineData(typeof(ReadOnlyFieldIncorrectlyInitializedViaConstructor<TestByteEnum>))]
        [InlineData(typeof(ReadOnlyPropertyIncorrectlyInitializedViaConstructor<TestByteEnum>))]
        [InlineData(typeof(ReadOnlyFieldIncorrectlyInitializedViaConstructor<TestSByteEnum>))]
        [InlineData(typeof(ReadOnlyPropertyIncorrectlyInitializedViaConstructor<TestSByteEnum>))]
        [InlineData(typeof(ReadOnlyFieldIncorrectlyInitializedViaConstructor<TestShortEnum>))]
        [InlineData(typeof(ReadOnlyPropertyIncorrectlyInitializedViaConstructor<TestShortEnum>))]
        [InlineData(typeof(ReadOnlyFieldIncorrectlyInitializedViaConstructor<TestUShortEnum>))]
        [InlineData(typeof(ReadOnlyPropertyIncorrectlyInitializedViaConstructor<TestUShortEnum>))]
        [InlineData(typeof(ReadOnlyFieldIncorrectlyInitializedViaConstructor<TestUIntEnum>))]
        [InlineData(typeof(ReadOnlyPropertyIncorrectlyInitializedViaConstructor<TestUIntEnum>))]
        [InlineData(typeof(ReadOnlyFieldIncorrectlyInitializedViaConstructor<TestLongEnum>))]
        [InlineData(typeof(ReadOnlyPropertyIncorrectlyInitializedViaConstructor<TestLongEnum>))]
        [InlineData(typeof(ReadOnlyFieldIncorrectlyInitializedViaConstructor<TestULongEnum>))]
        [InlineData(typeof(ReadOnlyPropertyIncorrectlyInitializedViaConstructor<TestULongEnum>))]
        [InlineData(typeof(ReadOnlyFieldIncorrectlyInitializedViaConstructor<TestSingleNonDefaultEnum>))]
        [InlineData(typeof(ReadOnlyPropertyIncorrectlyInitializedViaConstructor<TestSingleNonDefaultEnum>))]
        public void VerifyIllBehavedEnumInitializersDoThrow(Type type)
        {
            // Arrange
            var fixture = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(fixture);

            // Act & Assert
            Assert.Throws<ConstructorInitializedMemberException>(() => sut.Verify(type));
        }

        [Theory]
        [InlineData(typeof(ReadOnlyFieldInitializedViaConstructor<TestDefaultOnlyEnum>))]
        [InlineData(typeof(ReadOnlyPropertyInitializedViaConstructor<TestDefaultOnlyEnum>))]
        [InlineData(typeof(ReadOnlyPropertyIncorrectlyInitializedViaConstructor<TestDefaultOnlyEnum>))]
        public void VerifyDefaultOnlyEnumDoesThrowBecauseOfPotentialFalsePositive(Type type)
        {
            // Arrange
            var fixture = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(fixture);

            // Act & Assert
            Assert.Throws<ConstructorInitializedMemberException>(() => sut.Verify(type));
        }

        static void AssertExceptionPropertiesEqual(ConstructorInitializedMemberException ex, ConstructorInfo ctor, ParameterInfo param)
        {
            Assert.Equal(param, ex.MissingParameter);
            Assert.Equal(ctor, ex.MemberInfo);
            Assert.Equal(ctor, ex.ConstructorInfo);
            Assert.Null(ex.FieldInfo);
            Assert.Null(ex.PropertyInfo);
        }

        static void AssertExceptionPropertiesEqual(ConstructorInitializedMemberException ex, PropertyInfo pi)
        {
            Assert.Null(ex.ConstructorInfo);
            Assert.Null(ex.MissingParameter);
            Assert.Equal(pi, ex.MemberInfo);
            Assert.Null(ex.FieldInfo);
            Assert.Equal(pi, ex.PropertyInfo);
        }

        static void AssertExceptionPropertiesEqual(ConstructorInitializedMemberException ex, FieldInfo fi)
        {
            Assert.Null(ex.ConstructorInfo);
            Assert.Null(ex.MissingParameter);
            Assert.Equal(fi, ex.MemberInfo);
            Assert.Equal(fi, ex.FieldInfo);
            Assert.Null(ex.PropertyInfo);
        }

        class PublicReadOnlyFieldNotInitializedByConstructor
        {
#pragma warning disable 649
            public readonly int Field;
#pragma warning restore 649
        }

        class ReadOnlyPropertyWithNoSetterHolder<T>
        {
            public T Property { get { return default(T); } }
        }

        class WritablePropertyAndIllBehavedConstructor
        {
            public WritablePropertyAndIllBehavedConstructor(int property)
            {
            }

            public int Property { get; set; }
        }

        class WriteOnlyPropertyHolder<T>
        {
            private T writeOnlyPropertyBackingField;

            public WriteOnlyPropertyHolder(T writeOnlyProperty)
            {
                this.writeOnlyPropertyBackingField = writeOnlyProperty;
            }

            public T GetWriteOnlyProperty()
            {
                return this.writeOnlyPropertyBackingField;
            }

            public T WriteOnlyProperty
            {
                set
                {
                    this.writeOnlyPropertyBackingField = value;
                }
            }
        }

        class ComplexType
        {
            public ComplexType()
            {
                this.Children = new Collection<ComplexTypeChild>();
            }

            public ICollection<ComplexTypeChild> Children { get; set; }
        }

        class ComplexTypeChild
        {
            public string Name { get; set; }
        }

        class PropertyIsAssignableFromConstructorArgumentType
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

        class ReadOnlyFieldIncorrectlyInitializedViaConstructor<T>
        {
#pragma warning disable 649
            public readonly T Field;
#pragma warning restore 649

            public ReadOnlyFieldIncorrectlyInitializedViaConstructor(T field)
            {
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

        class ReadOnlyPropertyIncorrectlyInitializedViaConstructor<T>
        {
            public ReadOnlyPropertyIncorrectlyInitializedViaConstructor(T property)
            {
            }

            public T Property { get; private set; }
        }

        class FakeReflectionElementComparer : IEqualityComparer<IReflectionElement>
        {
            public bool Equals(IReflectionElement x, IReflectionElement y)
            {
                throw new NotImplementedException();
            }

            public int GetHashCode(IReflectionElement obj)
            {
                throw new NotImplementedException();
            }
        }

        class FakeEqualityComparer<T> : IEqualityComparer
        {
            bool IEqualityComparer.Equals(object x, object y)
            {
                throw new NotImplementedException();
            }

            int IEqualityComparer.GetHashCode(object obj)
            {
                throw new NotImplementedException();
            }
        }

        //All approved enum type variants : https://msdn.microsoft.com/en-us/library/sbbt4032.aspx?f=255&MSPPError=-2147217396
        enum TestIntEnum { none = 0, one, two, three };
        enum TestByteEnum : byte { none = 0, one, two, three };
        enum TestSByteEnum : sbyte { none = 0, one, two, three };
        enum TestShortEnum : short { none = 0, one, two, three };
        enum TestUShortEnum : ushort { none = 0, one, two, three };
        enum TestUIntEnum : uint { none = 0, one, two, three };
        enum TestLongEnum : long { none = 0, one, two, three };
        enum TestULongEnum : ulong { none = 0, one, two, three };

        enum TestDefaultOnlyEnum { none = 0 };

        enum TestSingleNonDefaultEnum { none = 1 };
    }
}
