using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Ploeh.Albedo;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

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
        public void ComparerIsCorrect()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var expectedComparer = new FakeEqualityComparer<object>();
            var dummyMatcher = new FakeReflectionElementComparer();
            var sut = new ConstructorInitializedMemberAssertion(
                dummyComposer, expectedComparer, dummyMatcher);
            // Exercise system
            IEqualityComparer result = sut.Comparer;
            // Verify outcome
            Assert.Equal(expectedComparer, result);
            // Teardown
        }

        [Fact]
        public void ConstructWithNullComparerThrows()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var dummyMatcher = new FakeReflectionElementComparer();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new ConstructorInitializedMemberAssertion(dummyComposer, null, dummyMatcher));
            // Teardown
        }

        [Fact]
        public void ParameterMemberMatcherIsCorrect()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var dummyComparer = new FakeEqualityComparer<object>();
            var expectedMatcher = new FakeReflectionElementComparer();
            var sut = new ConstructorInitializedMemberAssertion(
                dummyComposer, dummyComparer, expectedMatcher);
            // Exercise system
            IEqualityComparer<IReflectionElement> result = sut.ParameterMemberMatcher;
            // Verify outcome
            Assert.Equal(expectedMatcher, result);
            // Teardown
        }

        [Fact]
        public void ConstructWithNullParameterMemberMatcherThrows()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var dummyComparer = new FakeEqualityComparer<object>();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new ConstructorInitializedMemberAssertion(dummyComposer, dummyComparer, null));
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
        public void VerifyNullFieldThrows()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify((FieldInfo)null));
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
        public void VerifyWritablePropertyWithNoMatchingConstructorDoesNotThrow()
        {
            // Fixture setup
            var composer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(composer);
            var propertyInfo = typeof(PropertyHolder<object>).GetProperty("Property");
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() =>
                sut.Verify(propertyInfo));
            // Teardown
        }

        [Fact]
        public void VerifyWritableFieldWithNoMatchingConstructorDoesNotThrow()
        {
            // Fixture setup
            var composer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(composer);
            var propertyInfo = typeof(FieldHolder<object>).GetField("Field");
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() =>
                sut.Verify(propertyInfo));
            // Teardown
        }

        [Fact]
        public void VerifyReadOnlyPropertyWithPrivateSetterAndNoMatchingConstructorThrows()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var propertyInfo = typeof(ReadOnlyPropertyHolder<int>).GetProperty("Property");
            // Exercise system and verify outcome
            var e = Assert.Throws<ConstructorInitializedMemberException>(() =>
                sut.Verify(propertyInfo));
            AssertExceptionPropertiesEqual(e, propertyInfo);
            // Teardown
        }

        [Fact]
        public void VerifyReadOnlyPropertyWithNoSetterAndNoMatchingConstructorThrows()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var propertyInfo = typeof(ReadOnlyPropertyWithNoSetterHolder<int>).GetProperty("Property");
            // Exercise system and verify outcome
            var e = Assert.Throws<ConstructorInitializedMemberException>(() =>
                sut.Verify(propertyInfo));
            AssertExceptionPropertiesEqual(e, propertyInfo);
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
        public void VerifyIllBehavedPropertiesInitializedViaConstructorThrows()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var illBehavedType = typeof (PropertiesIncorrectlyInitializedViaConstructor<object, object>);
            var propertyInfo1 = illBehavedType.GetProperty("Property1");
            var propertyInfo2 = illBehavedType.GetProperty("Property2");
            // Exercise system and verify outcome
            var e1 = Assert.Throws<ConstructorInitializedMemberException>(() => sut.Verify(propertyInfo1));
            AssertExceptionPropertiesEqual(e1, propertyInfo1);
            var e2 = Assert.Throws<ConstructorInitializedMemberException>(() => sut.Verify(propertyInfo2));
            AssertExceptionPropertiesEqual(e2, propertyInfo2);
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
            AssertExceptionPropertiesEqual(e, fieldInfo);
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
            var e = Assert.Throws<ConstructorInitializedMemberException>(() => sut.Verify(ctor));
            var expectedMissingParam = ctor.GetParameters().Single(p => p.Name == "noMatchingField");
            AssertExceptionPropertiesEqual(e, ctor, expectedMissingParam);
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
            var e = Assert.Throws<ConstructorInitializedMemberException>(() => sut.Verify(ctor));
            var expectedMissingParam = ctor.GetParameters().Single(p => p.Name == "noMatchingProperty");
            AssertExceptionPropertiesEqual(e, ctor, expectedMissingParam);
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
            var e = Assert.Throws<ConstructorInitializedMemberException>(() => sut.Verify(ctor));
            var expectedMissingParam = ctor.GetParameters().Single(p => p.Name == "value");
            AssertExceptionPropertiesEqual(e, ctor, expectedMissingParam);
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

        [Fact]
        public void VerifyWhenMemberTypeIsComplexDoesNotThrow()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var ctor = typeof(ReadOnlyPropertiesInitializedViaConstructor<ComplexType, object>)
                .GetConstructor(new[] { typeof(ComplexType), typeof(object) });
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Verify(ctor));
            // Teardown
        }

        [Fact]
        public void VerifyWhenMemberTypeIsComplexWithIllBehavedConstructorThrows()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var ctor = typeof(ReadOnlyPropertiesInitializedViaConstructor<ComplexType, object>)
                .GetConstructor(new[] { typeof(ComplexType), typeof(object), typeof(TriState) });
            // Exercise system and verify outcome
            var e = Assert.Throws<ConstructorInitializedMemberException>(
                () => sut.Verify(ctor));
            var expectedMissingParameter = ctor.GetParameters().Single(p => p.Name == "noMatchingProperty");
            AssertExceptionPropertiesEqual(e, ctor, expectedMissingParameter);
            // Teardown
        }

        [Fact]
        public void VerifyWhenConstructorArgumentHasWriteOnlyPropertyDoesNotThrow()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var ctor = typeof(WriteOnlyPropertyHolder<ComplexType>).GetConstructors().First();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(
                () => sut.Verify(ctor));
            // Teardown
        }

        [Fact]
        public void VerifyWhenPropertyIsWriteOnlyDoesNotThrow()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var propertyInfo = typeof(WriteOnlyPropertyHolder<ComplexType>).GetProperty("WriteOnlyProperty");
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Verify(propertyInfo));
            // Teardown
        }

        [Fact]
        public void VerifyWhenPropertyGetterIsInternalDoesNotThrow()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var propertyInfo = typeof(InternalGetterPropertyHolder<ComplexType>).GetProperty("Property");
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Verify(propertyInfo));
            // Teardown
        }

        [Fact]
        public void VerifyTypeWithPublicWritablePropertyAndNoMatchingConstructorArgumentDoesNotThrow()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var typeToVerify = typeof(PropertyHolder<ComplexType>);
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Verify(typeToVerify));
            // Teardown
        }

        [Fact]
        public void VerifyTypeWithPublicStaticPropertyAndNoMatchingConstructorArgumentDoesNotThrow()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var typeToVerify = typeof(StaticPropertyHolder<ComplexType>);
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Verify(typeToVerify));
            // Teardown
        }

        [Fact]
        public void VerifyTypeWithPublicStaticFieldAndNoMatchingConstructorArgumentDoesNotThrow()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var typeToVerify = typeof(StaticFieldHolder<ComplexType>);
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Verify(typeToVerify));
            // Teardown
        }

        [Fact]
        public void VerifyTypeWithPublicStaticReadOnlyFieldAndNoMatchingGuardedConstuctorArgumentDoesNotThrow()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var typeToVerify = typeof(GuardedConstructorHostHoldingStaticReadOnlyField<ComplexType, int>);
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Verify(typeToVerify));
            // Teardown
        }

        [Fact]
        public void VerifyTypeWithPublicStaticReadOnlyPropertyAndNoMatchingGuardedConstuctorArgumentDoesNotThrow()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var typeToVerify = typeof(GuardedConstructorHostHoldingStaticReadOnlyProperty<ComplexType, int>);
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Verify(typeToVerify));
            // Teardown
        }

        [Fact]
        public void VerifyTypeWithReadOnlyPropertyAndIllBehavedConstructorThrows()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var typeToVerify = typeof(ReadOnlyPropertiesInitializedViaConstructor<int, string>);
            // Exercise system and verify outcome
            var e = Assert.Throws<ConstructorInitializedMemberException>(() => sut.Verify(typeToVerify));
            var expectedFailingConstructor = typeToVerify.GetConstructor(new[] { typeof(int), typeof(string), typeof(TriState) });
            var expectedFailingParameter = expectedFailingConstructor.GetParameters().Single(p => p.Name == "noMatchingProperty");
            AssertExceptionPropertiesEqual(e, expectedFailingConstructor, expectedFailingParameter);
            // Teardown
        }

        [Fact]
        public void VerifyTypeWithWritablePropertyAndMatchingIllBehavedConstructorArgumentThrows()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var typeToVerify = typeof(WritablePropertyAndIllBehavedConstructor);
            // Exercise system and verify outcome
            var e = Assert.Throws<ConstructorInitializedMemberException>(() => sut.Verify(typeToVerify));
            var expectedFailingProperty = typeToVerify.GetProperties().Single();
            AssertExceptionPropertiesEqual(e, expectedFailingProperty);
            // Teardown
        }

        [Fact]
        public void VerifyTypeWithPublicReadOnlyFieldsIncorrectlyInitialisedViaConstructorThrows()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(dummyComposer);
            var typeToVerify = typeof(PublicReadOnlyFieldNotInitializedByConstructor);
            // Exercise system and verify outcome
            var e = Assert.Throws<ConstructorInitializedMemberException>(() => sut.Verify(typeToVerify));
            var expectedFailingField = typeToVerify.GetFields().First();
            AssertExceptionPropertiesEqual(e, expectedFailingField);
            // Teardown
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
            // Fixture setup
            var fixture = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(fixture);

            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Verify(type));
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
            // Fixture setup
            var fixture = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(fixture);

            // Exercise system and verify outcome
            Assert.Throws<ConstructorInitializedMemberException>(() => sut.Verify(type));
        }

        [Theory]
        [InlineData(typeof(ReadOnlyFieldInitializedViaConstructor<TestDefaultOnlyEnum>))]
        [InlineData(typeof(ReadOnlyPropertyInitializedViaConstructor<TestDefaultOnlyEnum>))]
        [InlineData(typeof(ReadOnlyPropertyIncorrectlyInitializedViaConstructor<TestDefaultOnlyEnum>))]
        [InlineData(typeof(ReadOnlyPropertyIncorrectlyInitializedViaConstructor<TestDefaultOnlyEnum>))]
        public void VerifyDefaultOnlyEnumDoesThrowBecauseOfPotentialFalsePositive(Type type)
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = new ConstructorInitializedMemberAssertion(fixture);

            // Exercise system and verify outcome
            Assert.Throws<ConstructorInitializedMemberException>(() => sut.Verify(type));
        }

        static void AssertExceptionPropertiesEqual(ConstructorInitializedMemberException ex, ConstructorInfo ctor, ParameterInfo param)
        {
            Assert.Equal(param, ex.MissingParameter);
            Assert.Equal(ctor, ex.MemberInfo);
            Assert.Equal(ctor, ex.ConstructorInfo);
            Assert.Equal(null, ex.FieldInfo);
            Assert.Equal(null, ex.PropertyInfo);
        }

        static void AssertExceptionPropertiesEqual(ConstructorInitializedMemberException ex, PropertyInfo pi)
        {
            Assert.Equal(null, ex.ConstructorInfo);
            Assert.Equal(null, ex.MissingParameter);
            Assert.Equal(pi, ex.MemberInfo);
            Assert.Equal(null, ex.FieldInfo);
            Assert.Equal(pi, ex.PropertyInfo);
        }

        static void AssertExceptionPropertiesEqual(ConstructorInitializedMemberException ex, FieldInfo fi)
        {
            Assert.Equal(null, ex.ConstructorInfo);
            Assert.Equal(null, ex.MissingParameter);
            Assert.Equal(fi, ex.MemberInfo);
            Assert.Equal(fi, ex.FieldInfo);
            Assert.Equal(null, ex.PropertyInfo);
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
                return writeOnlyPropertyBackingField;
            }

            public T WriteOnlyProperty
            {
                set
                {
                    writeOnlyPropertyBackingField = value;
                }
            }
        }

        class ComplexType
        {
            public ComplexType()
            {
                Children = new Collection<ComplexTypeChild>();
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
