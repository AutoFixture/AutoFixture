﻿using Ploeh.SemanticComparison.Fluent;
using Ploeh.SemanticComparison.UnitTest.TestTypes;
using Ploeh.TestTypeFoundation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.SemanticComparison.UnitTest
{
    public class LikenessTest
    {
        [Fact]
        public void CreateWithNullValueWillHoldCorrectValue()
        {
            // Fixture setup
            var sut = new Likeness<PropertyHolder<string>, FieldHolder<int>>(null);
            // Exercise system
            PropertyHolder<string> result = sut.Value;
            // Verify outcome
            Assert.Null(result);
            // Teardown
        }

        [Fact]
        public void ValueIsCorrect()
        {
            // Fixture setup
            var expectedValue = new ConcreteType();
            var sut = new Likeness<ConcreteType, PropertyHolder<double>>(expectedValue);
            // Exercise system
            ConcreteType result = sut.Value;
            // Verify outcome
            Assert.Equal(expectedValue, result);
            // Teardown
        }

        [Fact]
        public void SutIsEquatable()
        {
            // Fixture setup
            // Exercise system
            var sut = new Likeness<int, string>(1);
            // Verify outcome
            Assert.IsAssignableFrom<IEquatable<string>>(sut);
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualNullObject()
        {
            // Fixture setup
            var sut = new Likeness<DateTime, TimeSpan>(DateTime.Now);
            object nullObject = null;
            // Exercise system
            var result = sut.Equals(nullObject);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualNullValue()
        {
            // Fixture setup
            var sut = new Likeness<PropertyHolder<short>, PropertyHolder<string>>(new PropertyHolder<short>());
            PropertyHolder<string> nullValue = null;
            // Exercise system
            var result = sut.Equals(nullValue);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutEqualsNullValueWhenSourceIsNull()
        {
            // Fixture setup
            var sut = new Likeness<PropertyHolder<string>, PropertyHolder<string>>(null);
            PropertyHolder<string> nullValue = null;
            // Exercise system
            var result = sut.Equals(nullValue);
            // Verify outcome
            Assert.True(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutEqualsNullObjectWhenSourceIsNull()
        {
            // Fixture setup
            var sut = new Likeness<PropertyHolder<string>, PropertyHolder<string>>(null);
            object nullValue = null;
            // Exercise system
            var result = sut.Equals(nullValue);
            // Verify outcome
            Assert.True(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutWithValueDoesNotEqualNullValue()
        {
            // Fixture setup
            var sut = new Likeness<ConcreteType, AbstractType>(new ConcreteType());
            ConcreteType nullValue = null;
            // Exercise system
            var result = sut.Equals(nullValue);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutWithValueDoesNotEqualNullObject()
        {
            // Fixture setup
            var sut = new Likeness<ConcreteType, AbstractType>(new ConcreteType());
            object nullObject = null;
            // Exercise system
            var result = sut.Equals(nullObject);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutEqualsItself()
        {
            // Fixture setup
            var sut = new Likeness<ConcreteType, AbstractType>(new ConcreteType());
            // Exercise system
            var result = sut.Equals(sut);
            // Verify outcome
            Assert.True(result, "Equals");
            // Teardown
        }

        [Fact]
        public void GetHashCodeShouldReturnHashCodeOfContainedObject()
        {
            // Fixture setup
            var anonymousDateTime = new DateTime(2010, 1, 3);
            int expectedHashCode = anonymousDateTime.GetHashCode();

            var sut = new Likeness<DateTime, DateTimeOffset>(anonymousDateTime);
            // Exercise system
            var result = sut.GetHashCode();
            // Verify outcome
            Assert.Equal(expectedHashCode, result);
            // Teardown
        }

        [Fact]
        public void GetHashCodeWhenNullIsContainedWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new Likeness<string, string>(null);
            // Exercise system
            var result = sut.GetHashCode();
            // Verify outcome
            Assert.Equal(0, result);
            // Teardown
        }

        [Fact]
        public void ToStringReturnsCorrectResult()
        {
            // Fixture setup
            var anonymousTimeSpan = new TimeSpan(7, 4, 2, 1);
            string expectedText = "Likeness of " + anonymousTimeSpan.ToString();

            var sut = new Likeness<TimeSpan, TimeZoneInfo>(anonymousTimeSpan);
            // Exercise system
            var result = sut.ToString();
            // Verify outcome
            Assert.Equal(expectedText, result);
            // Teardown
        }

        [Fact]
        public void ToStringOfContainedNullWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new Likeness<string, string>(null);
            // Exercise system
            var result = sut.ToString();
            // Verify outcome
            Assert.Equal("Likeness of null", result);
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualAnonymousObject()
        {
            // Fixture setup
            var sut = new Likeness<ConcreteType, ConcreteType>(new ConcreteType());
            object anonymousObject = new object();
            // Exercise system
            var result = sut.Equals(anonymousObject);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutEqualsIdenticalStrongType()
        {
            // Fixture setup
            var ticks = 8293247;
            var value = TimeSpan.FromTicks(ticks);
            var sut = new Likeness<TimeSpan, TimeSpan>(value);
            var other = TimeSpan.FromTicks(ticks);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutEqualsIdenticalWeakType()
        {
            // Fixture setup
            var ticks = 8293247;
            var value = TimeSpan.FromTicks(ticks);
            var sut = new Likeness<TimeSpan, TimeSpan>(value);
            object other = TimeSpan.FromTicks(ticks);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result, "Equals");
            // Teardown
        }

        [Fact]
        public void ComparingStringPropertyHolderLikenessToRealStringPropertyHolderWillIndicateEquality()
        {
            // Fixture setup
            string anonymousText = "Anonymous text";

            PropertyHolder<string> likenObject = new PropertyHolder<string>();
            likenObject.Property = anonymousText;

            PropertyHolder<string> comparee = new PropertyHolder<string>();
            comparee.Property = anonymousText;

            // The rest of the test
            LikenessTest.CompareLikenessToObject(likenObject, comparee, true);
        }

        [Fact]
        public void ComparingStringPropertyHoldersWithDifferentValuesWillIndicateDifference()
        {
            // Fixture setup
            string anonymousText1 = "Anonymous text";
            string anonymousText2 = "Some other text";

            PropertyHolder<string> likenObject = new PropertyHolder<string>();
            likenObject.Property = anonymousText1;

            PropertyHolder<string> comparee = new PropertyHolder<string>();
            comparee.Property = anonymousText2;

            // The rest of the test
            LikenessTest.CompareLikenessToObject(likenObject, comparee, false);
        }

        [Fact]
        public void ComparingStringFieldHolderLikenessToRealStringFieldHolderWillIndicateEquality()
        {
            // Fixture setup
            string anonymousText = "Anonymous text";

            FieldHolder<string> likenObject = new FieldHolder<string>();
            likenObject.Field = anonymousText;

            FieldHolder<string> comparee = new FieldHolder<string>();
            comparee.Field = anonymousText;

            // The rest of the test
            LikenessTest.CompareLikenessToObject(likenObject, comparee, true);
        }

        [Fact]
        public void ComparingStringFieldHoldersWithDifferentValuesWillIndicateDifference()
        {
            // Fixture setup
            string anonymousText1 = "Anonymous text";
            string anonymousText2 = "Some other text";

            FieldHolder<string> likenObject = new FieldHolder<string>();
            likenObject.Field = anonymousText1;

            FieldHolder<string> comparee = new FieldHolder<string>();
            comparee.Field = anonymousText2;

            // The rest of the test
            LikenessTest.CompareLikenessToObject(likenObject, comparee, false);
        }

        [Fact]
        public void CompareAnonymousTypeLikenessToStringFieldHolderWillIndicateEqualityWhenValuesAreEqual()
        {
            // Fixture setup
            string anonymousText = "Anonymou text";

            var likenObject = new
            {
                Field = anonymousText
            };

            FieldHolder<string> comparee = new FieldHolder<string>();
            comparee.Field = anonymousText;

            // The rest of the test
            LikenessTest.CompareLikenessToObject(likenObject, comparee, true);
        }

        [Fact]
        public void CompareAnonymousTypeLikenessToStringPropertyHolderWillIndicateDifferenceWhenValuesAreDifferent()
        {
            // Fixture setup
            string anonymousText1 = "Anonymous text";
            string anonymousText2 = "Some other text";

            var likenObject = new
            {
                Field = anonymousText1
            };

            FieldHolder<string> comparee = new FieldHolder<string>();
            comparee.Field = anonymousText2;

            // The rest of the test
            LikenessTest.CompareLikenessToObject(likenObject, comparee, false);
        }

        [Fact]
        public void ObjectsWithNullPropertiesWillHaveLikeness()
        {
            // Fixture setup
            PropertyHolder<object> likenObject = new PropertyHolder<object>();
            likenObject.Property = null;

            PropertyHolder<object> comparee = new PropertyHolder<object>();
            comparee.Property = null;

            // The rest of the test
            LikenessTest.CompareLikenessToObject(likenObject, comparee, true);
        }

        [Fact]
        public void LikenessOfObjectWithNullPropertyWillNotBeEqualToObjectWithValuedProperty()
        {
            // Fixture setup
            PropertyHolder<object> likenObject = new PropertyHolder<object>();
            likenObject.Property = null;

            PropertyHolder<object> comparee = new PropertyHolder<object>();
            comparee.Property = new object();

            // The rest of the test
            LikenessTest.CompareLikenessToObject(likenObject, comparee, false);
        }

        [Fact]
        public void LikenessOfObjectWithValuePropertyWillNotBeEqualToObjectWithNullProperty()
        {
            // Fixture setup
            PropertyHolder<object> likenObject = new PropertyHolder<object>();
            likenObject.Property = new object();

            PropertyHolder<object> comparee = new PropertyHolder<object>();
            comparee.Property = null;

            // The rest of the test
            LikenessTest.CompareLikenessToObject(likenObject, comparee, false);
        }

        [Fact]
        public void LikenessOfObjectWithPropertyWillNotBeEqualToPropertyWithDifferentProperty()
        {
            // Fixture setup
            var likenObject = new { SomeOtherProperty = new object() };

            PropertyHolder<object> comparee = new PropertyHolder<object>();
            comparee.Property = new object();

            // The rest of the test
            LikenessTest.CompareLikenessToObject(likenObject, comparee, false);
        }

        [Fact]
        public void LikenessAgainstObjectWithOverloadedMembersWillNotThrow()
        {
            // Fixture setup
            var likenObject = new object();

            var comparee = new TypeWithOverloadedMembers();

            // The rest of the test
            LikenessTest.CompareLikenessToObject(likenObject, comparee, false);
        }

        [Fact]
        public void LikenessAgainstObjectWithIndexerWillNotThrow()
        {
            // Fixture setup
            var likenObject = new object();

            var comparee = new TypeWithIndexer();

            // The rest of the test
            LikenessTest.CompareLikenessToObject(likenObject, comparee, true);
        }

        /// <summary>
        /// This test reproduces a bug.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This test reproduces a bug where comparing these two instances causes the following
        /// exception to be thrown: "System.Security.VerificationException: Operation could
        /// destabilize the runtime."
        /// </para>
        /// </remarks>
        [Fact]
        public void LikenessAgainstDataErrorInfoWillNotThrow()
        {
            // Fixture setup
            var likenObject = new DataErrorInfo();

            var comparee = new DataErrorInfo();

            // The rest of the test
            LikenessTest.CompareLikenessToObject(likenObject, comparee, true);
        }

        [Fact]
        public void LikenessAgainstObjectWithHidingPropertyWillNotThrow()
        {
            // Fixture setup
            var likenObject = new A();

            var comparee = new B();

            // The rest of the test
            LikenessTest.CompareLikenessToObject(likenObject, comparee, false);
        }

        [Fact]
        public void LikenessOfObjectWithHidingPropertyWillNotThrow()
        {
            // Fixture setup
            var likenObject = new B();

            var comparee = new A();

            // The rest of the test
            LikenessTest.CompareLikenessToObject(likenObject, comparee, false);
        }

        [Fact]
        public void SutOfAbstractTypeEqualsConcreteInstancesThatDifferOnlyOnMemberNotDefinedByAbstraction()
        {
            // Fixture setup
            var value = new ConcreteType("Lorem", "ipsum", "dolor", "sit");
            value.Property5 = "Ploeh";
            var sut = new Likeness<ConcreteType, AbstractType>(value);

            var other = new ConcreteType();
            other.Property1 = value.Property1;
            other.Property2 = value.Property2;
            other.Property3 = value.Property3;
            other.Property4 = value.Property4;
            other.Property5 = "Fnaah";
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutOfAbstractTypeDoesNotEqualConcreteInstanceWhenPropertyDiffers()
        {
            // Fixture setup
            var value = new ConcreteType("Lorem", "ipsum", "dolor", "sit");
            value.Property4 = "Ploeh";
            var sut = new Likeness<ConcreteType, AbstractType>(value);

            var other = new ConcreteType();
            other.Property1 = value.Property1;
            other.Property2 = value.Property2;
            other.Property3 = value.Property3;
            other.Property4 = "Fnaah";
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void WithNullThrows()
        {
            // Fixture setup
            var sut = new Likeness<object, object>(new object());
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.With<object>(null));
            // Teardown
        }

        [Fact]
        public void EqualsIsTrueWhenCorrectMappingHasBeenDefined()
        {
            // Fixture setup
            var value = new PropertyHolder<string>();
            value.Property = "Foo";
            var sut = new Likeness<PropertyHolder<string>, FieldHolder<string>>(value)
                .With(d => d.Field).EqualsWhen((s, d) => s.Property == d.Field);

            var other = new FieldHolder<string>();
            other.Field = value.Property;
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutWithoutPropertyEqualsInstanceEvenIfItDiffersOnThatProperty()
        {
            // Fixture setup
            var value = new ConcreteType("Lorem", "ipsum", "dolor", "sit");
            value.Property5 = "amet";
            var sut = new Likeness<ConcreteType, ConcreteType>(value).Without(x => x.Property5);

            var other = new ConcreteType();
            other.Property1 = value.Property1;
            other.Property2 = value.Property2;
            other.Property3 = value.Property3;
            other.Property4 = value.Property4;
            other.Property5 = "Fnaah";
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutWithoutTwoPropertiesEqualsInstanceEvenThoughThosePropertyValuesDiffer()
        {
            // Fixture setup
            var value = new ConcreteType("Lorem", "ipsum", "dolor", "sit");
            value.Property5 = "amet";
            var sut = new Likeness<ConcreteType, ConcreteType>(value).Without(x => x.Property5).Without(x => x.Property1);

            var other = new ConcreteType();
            other.Property1 = "Ndøh";
            other.Property2 = value.Property2;
            other.Property3 = value.Property3;
            other.Property4 = value.Property4;
            other.Property5 = "Sqryt";
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutWithSemanticEqualityWillReturnTrue()
        {
            // Fixture setup
            var value = new ConcreteType("Lorem", "ipsum", "dolor", "sit");
            var sut = new Likeness<ConcreteType, DoublePropertyHolder<object, object>>(value);

            var other = new DoublePropertyHolder<object, object>();
            other.Property1 = value.Property1;
            other.Property2 = value.Property2;
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutWithValueThatDoesNotMatchTypeParameterWillReturnFalseEvenWhenComparedWithIdenticalValue()
        {
            // Fixture setup
            var value = new { Property1 = "Lorem", Ploeh = "ipsum" };
            var sut = value.AsSource().OfLikeness<DoublePropertyHolder<object, object>>();

            var other = new { Property1 = "Lorem", Ploeh = "ipsum" };
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutWithAnonymousValueWillEqualSemanticallyIdenticalValue()
        {
            // Fixture setup
            var value = new { Property1 = "Lorem", Property2 = "ipsum" };
            var sut = value.AsSource().OfLikeness<DoublePropertyHolder<object, object>>();

            var other = new DoublePropertyHolder<object, object> { Property1 = value.Property1, Property2 = value.Property2 };
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutWithAnonymousSourceAndMemberMappingWillEqualMappedIdenticalValue()
        {
            // Fixture setup
            var value = new { Property1 = "Lorem", Ploeh = "ipsum", Fnaah = "dolor" };
            var sut = value.AsSource().OfLikeness<DoubleFieldHolder<string, string>>()
                .With(d => d.Field1).EqualsWhen((s, d) => s.Property1 == d.Field1)
                .With(d => d.Field2).EqualsWhen((s, d) => s.Ploeh == d.Field2);

            var other = new DoubleFieldHolder<string, string> { Field1 = value.Property1, Field2 = value.Ploeh };
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result, "Equals");
            // Teardown
        }

        [Fact]
        public void EqualsWhenOverriddenSourcePropertyEqualsReturnsTrueWillReturnTrue()
        {
            // Fixture setup
            var equalityResponse = true;
            var value = new PropertyHolder<EqualityResponder>();
            value.Property = new EqualityResponder(equalityResponse);
            var sut = new Likeness<PropertyHolder<EqualityResponder>, PropertyHolder<object>>(value);

            var other = new PropertyHolder<object>();
            other.Property = new object();
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.Equal(equalityResponse, result);
            // Teardown
        }

        [Fact]
        public void EqualsWhenOverriddenSourcePropertyEqualsReturnsFalseWillReturnFalse()
        {
            // Fixture setup
            var equalityResponse = false;
            var value = new PropertyHolder<EqualityResponder>();
            value.Property = new EqualityResponder(equalityResponse);
            var sut = new Likeness<PropertyHolder<EqualityResponder>, PropertyHolder<object>>(value);

            var other = new PropertyHolder<object>();
            other.Property = new object();
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.Equal(equalityResponse, result);
            // Teardown
        }

        [Fact]
        public void EqualsOfIdenticalObjectsReturnsTrue()
        {
            // Fixture setup
            var value = new QuadrupleParameterType<string, string, string, string>("Lorem", "ipsum", "dolor", "sit");
            var other = new QuadrupleParameterType<string, string, string, string>("Lorem", "ipsum", "dolor", "sit");
            // The rest of the test
            LikenessTest.CompareLikenessToObject(value, other, true);
        }

        [Fact]
        public void EqualsOfDifferentObjectsReturnFalse()
        {
            // Fixture setup
            var value = new QuadrupleParameterType<string, string, string, string>("Lorem", "ipsum", "dolor", "sit");
            var other = new QuadrupleParameterType<string, string, string, string>("amet", "consectetur", "adipisicing", "elit");
            // The rest of the test
            LikenessTest.CompareLikenessToObject(value, other, false);
        }

        [Fact]
        public void OmitAutoComparisonWillCauseDifferentObjectsToLookEqual()
        {
            // Fixture setup
            var value = new QuadrupleParameterType<string, string, string, string>("Lorem", "ipsum", "dolor", "sit");
            var other = new QuadrupleParameterType<string, string, string, string>("amet", "consectetur", "adipisicing", "elit");

            var sut = new Likeness<QuadrupleParameterType<string, string, string, string>, QuadrupleParameterType<string, string, string, string>>(value)
                .OmitAutoComparison();
            // Exercise system
            bool result = sut.Equals(other);
            // Verify outcome
            Assert.True(result, "OmitAutoComparison");
            // Teardown
        }

        [Fact]
        public void WithDefaultEqualityOfNullPropertyThrows()
        {
            // Fixture setup
            var sut = new Likeness<object, object>(new object());
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.WithDefaultEquality<object>(null));
            // Teardown
        }

        [Fact]
        public void OmitAutoComparisonFollowedByWithDefaultEqualityWillOptInOnThosePropertiesOnlyAndReturnTrueWhenTheyMatch()
        {
            // Fixture setup
            var value = new QuadrupleParameterType<string, string, string, string>("Lorem", "ipsum", "dolor", "sit");
            var other = new QuadrupleParameterType<string, string, string, string>("Lorem", "ploeh", "dolor", "fnaah");

            var sut = new Likeness<QuadrupleParameterType<string, string, string, string>, QuadrupleParameterType<string, string, string, string>>(value)
                .OmitAutoComparison()
                .WithDefaultEquality(d => d.Parameter1)
                .WithDefaultEquality(d => d.Parameter3);
            // Exercise system
            bool result = sut.Equals(other);
            // Verify outcome
            Assert.True(result, "DefaultEquality");
            // Teardown
        }

        [Fact]
        public void OmitAutoComparisonFollowedByWithDefaultEqualityWillOptInOnThosePropertiesOnlyAndReturnFalseWhenTheyDoNotMatch()
        {
            // Fixture setup
            var value = new QuadrupleParameterType<string, string, string, string>("Lorem", "ipsum", "dolor", "sit");
            var other = new QuadrupleParameterType<string, string, string, string>("Lorem", "ploeh", "dolor", "fnaah");

            var sut = new Likeness<QuadrupleParameterType<string, string, string, string>, QuadrupleParameterType<string, string, string, string>>(value)
                .OmitAutoComparison()
                .WithDefaultEquality(d => d.Parameter1)
                .WithDefaultEquality(d => d.Parameter4);
            // Exercise system
            bool result = sut.Equals(other);
            // Verify outcome
            Assert.False(result, "DefaultEquality");
            // Teardown
        }

        [Fact]
        public void OmitAutoComparisonFollowedByCorrectComboOfDefaultEqualityAndExplictyWithReturnsTrue()
        {
            // Fixture setup
            var value = new QuadrupleParameterType<string, string, string, string>("Lorem", "ipsum", "dolor", "sit");
            var other = new QuadrupleParameterType<string, string, string, string>("Lorem", "IPSUM", "dolor", "fnaah");

            var sut = new Likeness<QuadrupleParameterType<string, string, string, string>, QuadrupleParameterType<string, string, string, string>>(value)
                .OmitAutoComparison()
                .WithDefaultEquality(d => d.Parameter1)
                .With(d => d.Parameter2).EqualsWhen((s, d) => s.Parameter2.ToUpper() == d.Parameter2);
            // Exercise system
            bool result = sut.Equals(other);
            // Verify outcome
            Assert.True(result, "OmitAutoComparison...explicit With");
            // Teardown
        }

        [Fact]
        public void ShouldEqualDoesNotThrowWhenSourceAndDestinationMatch()
        {
            // Fixture setup
            var source = new object();
            var destination = new object();
            var sut = new Likeness<object, object>(source);
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() =>
                sut.ShouldEqual(destination));
            // Teardown
        }

        [Fact]
        public void ShouldEqualOfNullAgainstNullDoesNotThrow()
        {
            // Fixture setup
            var sut = new Likeness<object, object>(null);
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() =>
                sut.ShouldEqual(null));
            // Teardown
        }

        [Fact]
        public void ShouldEqualOfInstanceAgainstNullThrows()
        {
            // Fixture setup
            var dummy = new object();
            var sut = new Likeness<object, object>(dummy);
            // Exercise system and verify outcome
            Assert.Throws<LikenessException>(() =>
                sut.ShouldEqual(null));
            // Teardown
        }

        [Fact]
        public void ShouldEqualOfDifferentValuesThrows()
        {
            // Fixture setup
            var source = new { Property = 2 };
            var destination = new PropertyHolder<int> { Property = 1 };
            var sut = source.AsSource().OfLikeness<PropertyHolder<int>>();
            // Exercise system and verify outcome
            Assert.Throws<LikenessException>(() =>
                sut.ShouldEqual(destination));
            // Teardown
        }

        [Fact]
        public void ProxyIsNotNull()
        {
            // Fixture setup
            var source = new ConcreteType();
            var sut = source.AsSource().OfLikeness<AbstractType>();
            // Exercise system
            var result = sut.CreateProxy();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void ProxyIsCorrectType()
        {
            // Fixture setup
            var source = new ConcreteType();
            var sut = source.AsSource().OfLikeness<AbstractType>();
            // Exercise system
            var result = sut.CreateProxy();
            // Verify outcome
            Assert.IsAssignableFrom<AbstractType>(result);
            // Teardown
        }

        [Fact]
        public void ProxyReturnsDifferentInstanceWhenAccessedMultipleTimes()
        {
            // Fixture setup
            var source = new ConcreteType();
            var sut = source.AsSource().OfLikeness<AbstractType>();
            var expectedProxies = new[] { sut.CreateProxy(), sut.CreateProxy(), sut.CreateProxy() };
            // Exercise system
            var result = Enumerable.Range(1, expectedProxies.Length)
                .Select(x => sut.CreateProxy())
                .ToArray();
            // Verify outcome
            Assert.False(expectedProxies.SequenceEqual(result, new ReferenceEqualityComparer()));
            // Teardown
        }

        [Fact]
        public void ProxyInstanceIsDifferentThanSourceInstance()
        {
            // Fixture setup
            var source = new ConcreteType();
            var sut = source.AsSource().OfLikeness<AbstractType>();
            // Exercise system
            var result = sut.CreateProxy();
            // Verify outcome
            Assert.NotSame(source, result);
            // Teardown
        }

        [Fact]
        public void ProxyDoesNotEqualNullObject()
        {
            // Fixture setup
            var source = new ConcreteType();
            var sut = source.AsSource().OfLikeness<AbstractType>();
            // Exercise system
            var result = sut.CreateProxy();
            // Verify outcome
            Assert.False(result.Equals(null));
            // Teardown
        }

        [Fact]
        public void ProxyDoesNotEqualNullSource()
        {
            // Fixture setup
            var source = new ConcreteType();
            var sut = source.AsSource().OfLikeness<AbstractType>();
            // Exercise system
            var result = sut.CreateProxy();
            // Verify outcome
            Assert.False(result.Equals((ConcreteType)null));
            // Teardown
        }

        [Fact]
        public void ProxyWhenCalledMultipleTimesForSemanticallyEqualObjectsReturnsTrue()
        {
            // Fixture setup
            var source = new ConcreteType();
            var sut = source.AsSource().OfLikeness<AbstractType>().CreateProxy();
            var expectedResult = Enumerable.Range(1, 3).Select(x => true);
            // Exercise system
            var result = Enumerable.Range(1, 3)
                .Select(x => sut.Equals(new ConcreteType()));
            // Verify outcome
            Assert.True(expectedResult.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void ProxyWhenCalledMultipleTimesForSemanticallyUnequalObjectsReturnsFalse()
        {
            // Fixture setup
            var source = new ConcreteType("Lorem");
            var sut = source.AsSource().OfLikeness<AbstractType>().CreateProxy();
            sut.Property1 = "Fnaah";
            var expectedResult = Enumerable.Range(1, 3).Select(x => false);
            // Exercise system
            var result = Enumerable.Range(1, 3)
                .Select(x => sut.Equals(new ConcreteType()));
            // Verify outcome
            Assert.True(expectedResult.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void ProxyOfAbstractTypeEqualsConcreteInstancesThatDifferOnlyOnMemberNotDefinedByAbstraction()
        {
            // Fixture setup
            var other = new ConcreteType("Lorem", "ipsum", "dolor", "sit");
            other.Property5 = "ploeh";

            var sut = other.AsSource().OfLikeness<AbstractType>().CreateProxy();

            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void ProxyOfAbstractTypeDoesNotEqualConcreteInstanceWhenPropertyDiffers()
        {
            // Fixture setup
            var other = new ConcreteType("Lorem", "ipsum", "dolor", "sit");
            other.Property4 = "ploeh";

            var sut = other.AsSource().OfLikeness<AbstractType>().CreateProxy();
            sut.Property4 = "Fnaah";

            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void ProxyGetHashCodeDoesNotEqualRealGetHashCode()
        {
            // Fixture setup
            var source = new TypeOverridingGetHashCode();
            int expected = source.GetHashCode();
            var sut = source.AsSource().OfLikeness<TypeOverridingGetHashCode>();
            // Exercise system
            var result = sut.CreateProxy();
            // Verify outcome
            Assert.NotEqual(expected, result.GetHashCode());
            // Teardown
        }

        [Fact]
        public void ProxyThrowsWhenRealTypeIsSealed()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ProxyCreationException>(
                () => new ConcreteType().AsSource().OfLikeness<PublicSealedType>().CreateProxy());
            // Teardown
        }

        [Fact]
        public void ProxyOfTypeWithAccessibleConstructorDoesNotThrowWhenSourceTypeIsAnonymousType()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.DoesNotThrow(
                () => new { }.AsSource().OfLikeness<AbstractType>().CreateProxy());
            // Teardown
        }

        [Fact]
        public void ProxyOfTypeWithNonDefaultConstructorThrowsWhenSourceConstructorValuesDoNotMapToDestinationConstructor()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ProxyCreationException>(
                () => new { }.AsSource().OfLikeness<AbstractTypeWithNonDefaultConstructor<string>>().CreateProxy());
            // Teardown
        }

        [Fact]
        public void ProxyOfAbstractTypeWithNonDefaultConstructorDoesNotThrow()
        {
            // Fixture setup
            var value = new PropertyHolder<string>();
            value.Property = "Foo";
            var sut = value.AsSource().OfLikeness<AbstractTypeWithNonDefaultConstructor<string>>();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.CreateProxy());
            // Teardown
        }

        [Fact]
        public void ProxyOfTypeWithPrivateAndOtherCtorDoesNotThrow()
        {
            // Fixture setup
            var value = new PropertyHolder<string>();
            value.Property = "Foo";
            var sut = value.AsSource().OfLikeness<TypeWithPrivateDefaultCtorAndOtherCtor<string>>();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.CreateProxy());
            // Teardown
        }
        [Fact]
        public void ProxyOfQuadrupleParameterTypeEqualsTripleParameterType()
        {
            // Fixture setup
            var value = new QuadrupleParameterType<int, double, long, string>(1, 2.0, 3, "4");
            var sut = value.AsSource().OfLikeness<TripleParameterType<int, double, long>>().CreateProxy();
            // Exercise system
            var result = sut.Equals(value);
            // Verify outcome
            Assert.True(result);
        }

        [Fact]
        public void ProxyOfQuadrupleParameterTypeEqualsDoubleParameterType()
        {
            // Fixture setup
            var value = new QuadrupleParameterType<int, double, long, string>(1, 2.0, 3, "4");
            var sut = value.AsSource().OfLikeness<DoubleParameterType<int, double>>().CreateProxy();
            // Exercise system
            var result = sut.Equals(value);
            // Verify outcome
            Assert.True(result);
        }

        [Fact]
        public void ProxyOfQuadrupleParameterTypeEqualsSingleParameterType()
        {
            // Fixture setup
            var value = new QuadrupleParameterType<int, double, long, string>(1, 2.0, 3, "4");
            var sut = value.AsSource().OfLikeness<SingleParameterType<int>>()
                .With(d => d.Parameter).EqualsWhen((s, d) => s.Parameter1 == d.Parameter)
                .CreateProxy();
            // Exercise system
            var result = sut.Equals(value);
            // Verify outcome
            Assert.True(result);
        }

        [Fact]
        public void ProxyEqualsIsTrueWhenCorrectMappingHasBeenDefined()
        {
            // Fixture setup
            var other = new PropertyHolder<string>();
            other.Property = "Foo";

            var sut = new Likeness<PropertyHolder<string>, FieldHolder<string>>(other)
                .With(d => d.Field).EqualsWhen((s, d) => s.Property == d.Field)
                .CreateProxy();
            sut.Field = other.Property;

            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void ProxyWithoutPropertyEqualsInstanceEvenIfItDiffersOnThatProperty()
        {
            // Fixture setup
            var other = new ConcreteType("Lorem", "ipsum", "dolor", "sit");
            other.Property5 = "amet";

            var sut = other.AsSource().OfLikeness<ConcreteType>()
                .Without(x => x.Property5)
                .CreateProxy();
            sut.Property5 = "Fnaah";
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void ProxyWithoutTwoPropertiesEqualsInstanceEvenThoughThosePropertyValuesDiffer()
        {
            // Fixture setup
            var value = new ConcreteType("Lorem", "ipsum", "dolor", "sit");
            value.Property5 = "amet";

            var sut = value.AsSource().OfLikeness<ConcreteType>()
                .Without(x => x.Property5).Without(x => x.Property1)
                .CreateProxy();
            sut.Property1 = "Ndøh";
            sut.Property5 = "Sqryt";
            // Exercise system
            var result = sut.Equals(value);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void ProxyWithSemanticEqualityWillReturnTrue()
        {
            // Fixture setup
            var value = new ConcreteType("Lorem", "ipsum", "dolor", "sit");

            var sut = value.AsSource().OfLikeness<DoublePropertyHolder<object, object>>()
                .CreateProxy();
            // Exercise system
            var result = sut.Equals(value);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void ProxyOmitAutoComparisonWillCauseDifferentObjectsToLookEqual()
        {
            // Fixture setup
            var value = new ConcreteType("Lorem", "ipsum", "dolor", "sit");

            var sut = value.AsSource().OfLikeness<ConcreteType>()
                .OmitAutoComparison()
                .CreateProxy();
            sut.Property1 = "amet";
            sut.Property2 = "consectetur";
            sut.Property3 = "adipisicing";
            sut.Property4 = "elit";
            // Exercise system
            bool result = sut.Equals(value);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void ProxyOmitAutoComparisonFollowedByWithDefaultEqualityWillOptInOnThosePropertiesOnlyAndReturnTrueWhenTheyMatch()
        {
            // Fixture setup
            var value = new ConcreteType("Lorem", "ipsum", "dolor", "sit");

            var sut = value.AsSource().OfLikeness<ConcreteType>()
                .OmitAutoComparison()
                .WithDefaultEquality(d => d.Property1)
                .WithDefaultEquality(d => d.Property3)
                .CreateProxy();
            sut.Property1 = "Lorem";
            sut.Property2 = "ploeh";
            sut.Property3 = "dolor";
            sut.Property4 = "fnaah";
            // Exercise system
            bool result = sut.Equals(value);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void ProxyOmitAutoComparisonFollowedByWithDefaultEqualityWillOptInOnThosePropertiesOnlyAndReturnFalseWhenTheyDoNotMatch()
        {
            // Fixture setup
            var value = new ConcreteType("Lorem", "ipsum", "dolor", "sit");

            var sut = value.AsSource().OfLikeness<AbstractType>()
                .OmitAutoComparison()
                .WithDefaultEquality(d => d.Property1)
                .WithDefaultEquality(d => d.Property4)
                .CreateProxy();
            sut.Property1 = "Lorem";
            sut.Property2 = "ploeh";
            sut.Property3 = "dolor";
            sut.Property4 = "fnaah";
            // Exercise system
            bool result = sut.Equals(value);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void ProxyOmitAutoComparisonFollowedByCorrectComboOfDefaultEqualityAndExplictyWithReturnsTrue()
        {
            // Fixture setup
            var value = new ConcreteType("Lorem", "ipsum", "dolor", "sit");

            var sut = value.AsSource().OfLikeness<AbstractType>()
                .OmitAutoComparison()
                .WithDefaultEquality(d => d.Property1)
                .With(d => d.Property2).EqualsWhen((s, d) => s.Property2.ToString().ToUpper() == d.Property2.ToString())
                .CreateProxy();
            sut.Property1 = "Lorem";
            sut.Property2 = "IPSUM";
            sut.Property3 = "dolor";
            sut.Property4 = "fnaah";
            // Exercise system
            bool result = sut.Equals(value);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void ProxyOfIdenticalParameterTypesCanBeCreated()
        {
            // Fixture setup
            var sut = new TripleParameterType<long, long, long>(1, 2, 3)
                .AsSource().OfLikeness<TripleParameterType<long, long, long>>();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.CreateProxy());
            // Teardown
        }

        [Fact]
        public void ProxyOfIdenticalParameterTypesWithPropertiesCanBeCreated()
        {
            // Fixture setup
            var sut = new TypeWithIdenticalParameterTypesAndProperties(1, 2, 3)
                .AsSource().OfLikeness<TypeWithIdenticalParameterTypesAndProperties>();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.CreateProxy());
            // Teardown
        }

        [Fact]
        public void ProxyOfTypeWithDifferentParameterTypesAndPropertiesCanBeCreated()
        {
            // Fixture setup
            var sut = new TypeWithDifferentParameterTypesAndProperties(1, "2", 3)
                .AsSource().OfLikeness<TypeWithDifferentParameterTypesAndProperties>();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.CreateProxy());
            // Teardown
        }

        [Fact]
        public void ProxyOfTypeWithIncompatibleAndCompatibleConstructorCanBeCreated()
        {
            // Fixture setup
            var value = new TypeWithIncompatibleAndCompatibleConstructor(
               new ConcreteType());

            var sut = value
                .AsSource()
                .OfLikeness<TypeWithIncompatibleAndCompatibleConstructor>();
            // Exercise system
            var result = sut.CreateProxy();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void ProxyOfTypeWithIncompatibleAndCompatibleConstructorCanBeCreatedSecondOverload()
        {
            // Fixture setup
            var value = new TypeWithIncompatibleAndCompatibleConstructor(
                new ConcreteType(),
                new byte());

            var sut = value
                .AsSource()
                .OfLikeness<TypeWithIncompatibleAndCompatibleConstructor>();
            // Exercise system
            var result = sut.CreateProxy();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void ProxyOfTypeWithIncompatibleAndCompatibleConstructorCanBeCreatedThirdOverload()
        {
            // Fixture setup
            var value = new TypeWithIncompatibleAndCompatibleConstructor(
                new ConcreteType(),
                new ConcreteType(),
                new byte());

            var sut = value
                .AsSource()
                .OfLikeness<TypeWithIncompatibleAndCompatibleConstructor>();
            // Exercise system
            var result = sut.CreateProxy();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void ProxyOfTypeWithUnorderedPropertiesCanBeCreated()
        {
            // Fixture setup
            var value = new TypeWithUnorderedProperties(
                new ConcreteType(),
                new ConcreteType(),
                new byte());

            var sut = value
                .AsSource()
                .OfLikeness<TypeWithUnorderedProperties>();
            // Exercise system
            var result = sut.CreateProxy();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void ProxyAndLikenessHaveSameBehavior()
        {
            // Fixture setup
            var original = new ConcreteType
            {
                Property1 = "value1",
                Property2 = "value2"
            };

            var likeness = original.AsSource().OfLikeness<ConcreteType>()
                .Without(x => x.Property1);
            var expected = likeness.Equals(original);
            var sut = likeness.CreateProxy();
            // Exercise system
            var result = sut.Equals(original);
            // Verify outcome
            Assert.Equal(expected, result);
            // Teardown
        }

        [Fact]
        public void ProxyCanCorrectlyAssignsFieldValues()
        {
            // Fixture setup
            var expected = 3;
            var value = new FieldHolder<int>();
            value.Field = expected;

            var sut = value.AsSource().OfLikeness<FieldHolder<int>>()
                .CreateProxy();
            // Exercise system
            var result = sut.Field;
            // Verify outcome
            Assert.Equal(expected, result);
            // Teardown
        }

        [Fact]
        public void ProxyCanCorrectlyAssignsFieldValuesToTypeWithPublicFieldsAndProperties()
        {
            // Fixture setup
            var value = new TypeWithPublicFieldsAndProperties();
            value.AutomaticProperty = 1m;
            value.Field = "2";
            value.Number = 3;

            var sut = value.AsSource()
                .OfLikeness<TypeWithPublicFieldsAndProperties>()
                .CreateProxy();
            // Exercise system
            var result =
                   value.AutomaticProperty == sut.AutomaticProperty
                && value.Field == sut.Field
                && value.Number == sut.Number;
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void ProxyCanCorrectlyAssignFieldValuesFromAnonymousType()
        {
            // Fixture setup
            var expected = 3.0;
            var value = new { Field = expected };
            var sut = value.AsSource().OfLikeness<FieldHolder<double>>()
                .CreateProxy();
            // Exercise system
            var result = sut.Field;
            // Verify outcome
            Assert.Equal(expected, result);
            // Teardown
        }

        [Fact]
        public void ProxyCanCorrectlyAssignFieldValuesFromAnonymousTypeToTypeWithPublicFieldsAndProperties()
        {
            // Fixture setup
            var value = new
            {
                AutomaticProperty = 1m,
                Field = "2",
                Number = long.MaxValue
            };
            var sut = value.AsSource()
                .OfLikeness<TypeWithPublicFieldsAndProperties>()
                .CreateProxy();
            // Exercise system
            var result =
                   value.AutomaticProperty == sut.AutomaticProperty
                && value.Field == sut.Field
                && value.Number == sut.Number;
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void ProxyCanCorrectlyAssignValuesToTypeWithSimilarPropertyNamesAndIdenticalPropertyTypes()
        {
            // Fixture setup
            var expected =
                new TypeWithSimilarPropertyNamesAndIdenticalPropertyTypes
            {
                Property = 1,
                Property2 = 2
            };
            var sut = expected
                .AsSource()
                .OfLikeness<TypeWithSimilarPropertyNamesAndIdenticalPropertyTypes>();
            // Exercise system
            var result = sut.CreateProxy();
            // Verify outcome
            Assert.True(
                expected.Property == result.Property &&
                expected.Property2 == result.Property2);
            // Teardown
        }

        [Fact]
        public void LikenessInitializedWithNullValueWillHoldCorrectValue()
        {
            // Fixture setup
            var sut = new Likeness<PropertyHolder<string>>(null);
            // Exercise system
            PropertyHolder<string> result = sut.Value;
            // Verify outcome
            Assert.Null(result);
            // Teardown
        }

        [Fact]
        public void LikenessValueIsCorrect()
        {
            // Fixture setup
            var expected = new ConcreteType();
            var sut = new Likeness<ConcreteType>(expected);
            // Exercise system
            var result = sut.Value;
            // Verify outcome
            Assert.Equal(expected, result);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullComparerThrows()
        {
            // Fixture setup
            var dummyValue = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new Likeness<object>(
                    dummyValue,
                    (IEqualityComparer<object>)null));
            // Teardown
        }

        [Fact]
        public void LikenessIsEquatable()
        {
            // Fixture setup
            // Exercise system
            var sut = new Likeness<int>(1);
            // Verify outcome
            Assert.IsAssignableFrom<IEquatable<int>>(sut);
            // Teardown
        }

        [Theory]
        [InlineData(null, null, true)]
        [InlineData("az", "az", true)]
        [InlineData("az", null, false)]
        [InlineData(null, "az", false)]
        public void LikenessEqualsReturnsCorrectResult(
            string value,
            string other,
            bool expected)
        {
            // Fixture setup
            var sut = new Likeness<string>(value);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.Equal(expected, result);
            // Teardown

        }

        [Fact]
        public void LikenessDoesNotEqualNull()
        {
            // Fixture setup
            var sut = new Likeness<DateTime>(DateTime.Now);
            // Exercise system
            var actual = BothEquals(sut, (object)null);
            // Verify outcome
            Assert.False(actual.Any(b => b));
            // Teardown
        }

        [Fact]
        public void LikenessEqualsNullValueWhenProvidedValueIsNull()
        {
            // Fixture setup
            var sut = new Likeness<PropertyHolder<string>>(null);
            PropertyHolder<string> nullValue = null;
            // Exercise system
            var actual = BothEquals(sut, nullValue);
            // Verify outcome
            Assert.True(actual.Any(b => b));
            // Teardown
        }

        [Fact]
        public void LikenessWithValueDoesNotEqualNull()
        {
            // Fixture setup
            var sut = new Likeness<ConcreteType>(new ConcreteType());
            ConcreteType nullValue = null;
            // Exercise system
            var actual = BothEquals(sut, nullValue);
            // Verify outcome
            Assert.False(actual.Any(b => b));
            // Teardown
        }

        [Fact]
        public void LikenessEqualsItself()
        {
            // Fixture setup
            var sut = new Likeness<ConcreteType>(new ConcreteType());
            // Exercise system
            var result = sut.Equals(sut);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void LikenessGetHashCodeShouldReturnHashCodeOfContainedObject()
        {
            // Fixture setup
            var anonymousDateTime = new DateTime(2010, 1, 3);
            var sut = new Likeness<DateTime>(anonymousDateTime);
            // Exercise system
            var result = sut.GetHashCode();
            // Verify outcome
            int expected = anonymousDateTime.GetHashCode();
            Assert.Equal(expected, result);
            // Teardown
        }

        [Fact]
        public void LikenessGetHashCodeWhenNullIsContainedWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new Likeness<string>(null);
            // Exercise system
            var result = sut.GetHashCode();
            // Verify outcome
            Assert.Equal(0, result);
            // Teardown
        }

        [Fact]
        public void LikenessToStringReturnsCorrectResult()
        {
            // Fixture setup
            var anonymousTimeSpan = new TimeSpan(7, 4, 2, 1);
            var sut = new Likeness<TimeSpan>(anonymousTimeSpan);
            // Exercise system
            var result = sut.ToString();
            // Verify outcome
            var expectedText = "Likeness of " + anonymousTimeSpan.ToString();
            Assert.Equal(expectedText, result);
            // Teardown
        }

        [Fact]
        public void LikenessToStringOfContainedNullWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new Likeness<string>(null);
            // Exercise system
            var result = sut.ToString();
            // Verify outcome
            Assert.Equal("Likeness of null", result);
            // Teardown
        }

        [Fact]
        public void LikenessEqualsIdenticalStrongType()
        {
            // Fixture setup
            var ticks = 8293247;
            var value = TimeSpan.FromTicks(ticks);
            var sut = new Likeness<TimeSpan>(value);
            var other = TimeSpan.FromTicks(ticks);
            // Exercise system
            var actual = BothEquals(sut, other);
            // Verify outcome
            Assert.True(actual.Any(b => b));
            // Teardown
        }

        [Fact]
        public void LikenessComparingStringPropertyHolderToRealStringPropertyHolderWillIndicateEquality()
        {
            // Fixture setup
            var anonymousText = "Anonymous text";

            var likenObject = new PropertyHolder<string>();
            likenObject.Property = anonymousText;

            var comparee = new PropertyHolder<string>();
            comparee.Property = anonymousText;

            var sut = new Likeness<PropertyHolder<string>>(likenObject);

            // Exercise system
            bool result = sut.Equals(comparee);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void LikenessComparingStringPropertyHoldersWithDifferentValuesWillIndicateDifference()
        {
            // Fixture setup
            var anonymousText1 = "Anonymous text";
            var anonymousText2 = "Some other text";

            var likenObject = new PropertyHolder<string>();
            likenObject.Property = anonymousText1;

            var comparee = new PropertyHolder<string>();
            comparee.Property = anonymousText2;

            var sut = new Likeness<PropertyHolder<string>>(likenObject);

            // Exercise system
            bool result = sut.Equals(comparee);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void LikenessComparingStringFieldHolderToRealStringFieldHolderWillIndicateEquality()
        {
            // Fixture setup
            var anonymousText = "Anonymous text";

            var likenObject = new FieldHolder<string>();
            likenObject.Field = anonymousText;

            var comparee = new FieldHolder<string>();
            comparee.Field = anonymousText;

            var sut = new Likeness<FieldHolder<string>>(likenObject);

            // Exercise system
            bool result = sut.Equals(comparee);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void LikenessComparingStringFieldHoldersWithDifferentValuesWillIndicateDifference()
        {
            // Fixture setup
            var anonymousText1 = "Anonymous text";
            var anonymousText2 = "Some other text";

            var likenObject = new FieldHolder<string>();
            likenObject.Field = anonymousText1;

            var comparee = new FieldHolder<string>();
            comparee.Field = anonymousText2;

            var sut = new Likeness<FieldHolder<string>>(likenObject);

            // Exercise system
            bool result = sut.Equals(comparee);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void ObjectsWithNullPropertiesWillHaveCorrectLikeness()
        {
            // Fixture setup
            var likenObject = new PropertyHolder<object>();
            likenObject.Property = null;

            var comparee = new PropertyHolder<object>();
            comparee.Property = null;

            var sut = new Likeness<PropertyHolder<object>>(likenObject);

            // Exercise system
            bool result = sut.Equals(comparee);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void LikenessOfObjectWithNullPropertyWillNotBeEqualToSameTypeOfObjectWithValuedProperty()
        {
            // Fixture setup
            var likenObject = new PropertyHolder<object>();
            likenObject.Property = null;

            var comparee = new PropertyHolder<object>();
            comparee.Property = new object();

            var sut = new Likeness<PropertyHolder<object>>(likenObject);

            // Exercise system
            bool result = sut.Equals(comparee);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void LikenessObjectWithValuePropertyWillNotBeEqualToSameTypeOfObjectWithNullProperty()
        {
            // Fixture setup
            var likenObject = new PropertyHolder<object>();
            likenObject.Property = new object();

            var comparee = new PropertyHolder<object>();
            comparee.Property = null;

            var sut = new Likeness<PropertyHolder<object>>(likenObject);

            // Exercise system
            bool result = sut.Equals(comparee);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Theory]
        [InlineData(123, 123, true)]
        [InlineData(456, 123, false)]
        public void LikenessEqualsForwardsCorrectCallToComparer(
            int value,
            int other,
            bool expected)
        {
            // Fixture setup
            var comparerStub = new DelegatingEqualityComparer<int>
            {
                OnEquals = (x, y) => x.Equals(y)
            };

            var sut = new Likeness<int>(value, comparerStub);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.Equal(expected, result);
            // Teardown
        }

        [Fact]
        public void ResemblanceIsNotNull()
        {
            // Fixture setup
            var dummyValue = new ConcreteType();
            var sut = new Likeness<ConcreteType>(dummyValue);
            // Exercise system
            var result = sut.ToResemblance();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void ResemblanceIsCorrectType()
        {
            // Fixture setup
            var dummyValue = new ConcreteType();
            var sut = new Likeness<ConcreteType>(dummyValue);
            // Exercise system
            var result = sut.ToResemblance();
            // Verify outcome
            Assert.IsAssignableFrom<ConcreteType>(result);
            // Teardown
        }

        [Fact]
        public void ResemblanceReturnsDifferentInstanceWhenAccessedMultipleTimes()
        {
            // Fixture setup
            var dummyValue = new ConcreteType();
            var sut = new Likeness<ConcreteType>(dummyValue);
            var expectedResemblances = new[] 
            { 
                sut.ToResemblance(), 
                sut.ToResemblance(), 
                sut.ToResemblance() 
            };
            // Exercise system
            var result = Enumerable
                .Range(1, expectedResemblances.Length)
                .Select(x => sut.ToResemblance());
            // Verify outcome
            Assert.False(
                expectedResemblances
                    .SequenceEqual(
                        result,
                        new ReferenceEqualityComparer()));
            // Teardown
        }

        [Fact]
        public void ResemblanceDoesNotEqualNullObject()
        {
            // Fixture setup
            var dummyValue = new ConcreteType();
            var sut = new Likeness<ConcreteType>(dummyValue);
            // Exercise system
            var result = sut.ToResemblance();
            // Verify outcome
            Assert.False(result.Equals(null));
            // Teardown
        }

        [Fact]
        public void ResemblanceWhenCalledMultipleTimesForSemanticallyEqualObjectsReturnsTrue()
        {
            // Fixture setup
            var dummyValue = new ConcreteType();
            var sut = new Likeness<ConcreteType>(dummyValue).ToResemblance();
            // Exercise system
            var result = Enumerable
                .Range(1, 3)
                .Select(x => sut.Equals(new ConcreteType()));
            // Verify outcome
            var expectedResult = Enumerable.Range(1, 3).Select(x => true);
            Assert.True(expectedResult.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void ResemblanceWhenCalledMultipleTimesForSemanticallyUnequalObjectsReturnsFalse()
        {
            // Fixture setup
            var anonymousText = "Lorem";
            var value = new ConcreteType(anonymousText);
            var sut = new Likeness<ConcreteType>(value).ToResemblance();
            // Exercise system
            var result = Enumerable
                .Range(1, 3)
                .Select(x => sut.Equals(new ConcreteType()));
            // Verify outcome
            var expectedResult = Enumerable.Range(1, 3).Select(x => false);
            Assert.True(expectedResult.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void ResemblanceGetHashCodeDoesNotEqualRealGetHashCode()
        {
            // Fixture setup
            var value = new TypeOverridingGetHashCode();
            int expected = value.GetHashCode();
            var sut = new Likeness<TypeOverridingGetHashCode>(value);
            // Exercise system
            var result = sut.ToResemblance();
            // Verify outcome
            Assert.NotEqual(expected, result.GetHashCode());
            // Teardown
        }

        [Fact]
        public void ResemblanceThrowsWhenRealTypeIsSealed()
        {
            // Fixture setup
            var dummyValue = new PublicSealedType();
            // Exercise system and verify outcome
            Assert.Throws<ProxyCreationException>(
                () => new Likeness<PublicSealedType>(dummyValue).ToResemblance());
            // Teardown
        }

        [Fact]
        public void ResemblanceOfTypeWithPrivateAndOtherCtorDoesNotThrow()
        {
            // Fixture setup
            var anonymousText = "Anonymous text";
            var dummyValue = new TypeWithPrivateDefaultCtorAndOtherCtor<string>(anonymousText);
            var sut = new Likeness<TypeWithPrivateDefaultCtorAndOtherCtor<string>>(dummyValue);
            // Exercise system
            var result = sut.ToResemblance();
            // Verify outcome
            Assert.IsAssignableFrom<TypeWithPrivateDefaultCtorAndOtherCtor<string>>(result);
            // Teardown
        }

        [Fact]
        public void ResemblanceOfIdenticalParameterTypesAndPropertiesCanBeCreated()
        {
            // Fixture setup
            var dummyValue = new TypeWithIdenticalParameterTypesAndProperties(1, 2, 3);
            var sut = new Likeness<TypeWithIdenticalParameterTypesAndProperties>(dummyValue);
            // Exercise system
            var result = sut.ToResemblance();
            // Verify outcome
            Assert.IsAssignableFrom<TypeWithIdenticalParameterTypesAndProperties>(result);
            // Teardown
        }

        [Fact]
        public void ResemblanceOfTypeWithDifferentParameterTypesAndPropertiesCanBeCreated()
        {
            // Fixture setup
            var dummyValue = new TypeWithDifferentParameterTypesAndProperties(1, "2", 3);
            var sut = new Likeness<TypeWithDifferentParameterTypesAndProperties>(dummyValue);
            // Exercise system
            var result = sut.ToResemblance();
            // Verify outcome
            Assert.IsAssignableFrom<TypeWithDifferentParameterTypesAndProperties>(result);
            // Teardown
        }

        [Fact]
        public void ResemblanceOfTypeWithIncompatibleAndCompatibleConstructorCanBeCreated()
        {
            // Fixture setup
            var value = new TypeWithIncompatibleAndCompatibleConstructor(
                new ConcreteType());

            var sut = new Likeness<TypeWithIncompatibleAndCompatibleConstructor>(value);
            // Exercise system
            var result = sut.ToResemblance();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void ResemblanceOfTypeWithIncompatibleAndCompatibleConstructorCanBeCreatedSecondOverload()
        {
            // Fixture setup
            var value = new TypeWithIncompatibleAndCompatibleConstructor(
                new ConcreteType(),
                new byte());

            var sut = new Likeness<TypeWithIncompatibleAndCompatibleConstructor>(value);
            // Exercise system
            var result = sut.ToResemblance();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void ResemblanceOfTypeWithIncompatibleAndCompatibleConstructorCanBeCreatedThirdOverload()
        {
            // Fixture setup
            var value = new TypeWithIncompatibleAndCompatibleConstructor(
                new ConcreteType(),
                new ConcreteType(),
                new byte());

            var sut = new Likeness<TypeWithIncompatibleAndCompatibleConstructor>(value);
            // Exercise system
            var result = sut.ToResemblance();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void ResemblanceOfTypeWithUnorderedPropertiesCanBeCreated()
        {
            // Fixture setup
            var value = new TypeWithUnorderedProperties(
                new ConcreteType(),
                new ConcreteType(),
                new byte());

            var sut = new Likeness<TypeWithUnorderedProperties>(value);
            // Exercise system
            var result = sut.ToResemblance();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void CreateProxyReturnsCorrectForSourceTypeWithAllNullProperties()
        {
            // Fixture setup
            var obj = new TypeWithUnorderedProperties(null, null, (byte)4);

            // Exercise system
            var proxy = obj.AsSource().OfLikeness<TypeWithIncompatibleAndCompatibleConstructor>().CreateProxy();
            // Verify outcome

            Assert.NotNull(proxy);
            Assert.Null(proxy.Property1);
            Assert.Null(proxy.Property2);
            Assert.NotNull(proxy.Property3);
            Assert.Equal(obj.Property3, proxy.Property3);
            // Teardown
        }


        [Fact]
        public void CreateProxyReturnsCorrectForSourceTypeWithBothNullAndNonNullProperties()
        {
            // Fixture setup
            var obj = new TypeWithUnorderedProperties(
                                                    new ConcreteType("a string", "another", "last string"), 
                                                    null, (byte)4);

            // Exercise system
            var proxy = obj.AsSource().OfLikeness<TypeWithIncompatibleAndCompatibleConstructor>().CreateProxy();

            // Verify outcome
            Assert.NotNull(proxy);
            Assert.NotNull(proxy.Property1);
            Assert.Null(proxy.Property2);
            Assert.NotNull(proxy.Property3);
            Assert.Equal(obj.Property1, proxy.Property1);
            Assert.Equal(obj.Property3, proxy.Property3);
            // Teardown
        }

        [Theory]
        [ClassData(typeof(PropertiesThatHaveNullValues))]
        public void CreateProxyReturnsCorrectForSourceTypeWithNonNullPropertiesThatHaveNullProperties
                                                    (ConcreteType property1, AbstractType property2, byte property3)
        {
            // Fixture setup
            var obj = new TypeWithUnorderedProperties(property1, property2, property3);

            // Exercise system
            var proxy = obj.AsSource().OfLikeness<TypeWithIncompatibleAndCompatibleConstructor>().CreateProxy();

            // Verify outcome
            Assert.NotNull(proxy);
            Assert.NotNull(proxy.Property1);
            Assert.NotNull(proxy.Property2);
            Assert.NotNull(proxy.Property3);
            Assert.Equal(obj.Property1, proxy.Property1);
            Assert.Equal(obj.Property2, proxy.Property2);
            Assert.Equal(obj.Property3, proxy.Property3);
            // Teardown
        }

        private static void CompareLikenessToObject<TSource, TDestination>(TSource likenObject, TDestination comparee, bool expectedResult)
        {
            // Fixture setup
            var sut = new Likeness<TSource, TDestination>(likenObject);
            // Exercise system
            bool result = sut.Equals(comparee);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        private static IEnumerable<bool> BothEquals<T1, T2>(T1 sut, T2 other)
        {
            yield return sut.Equals((object)other);
            yield return sut.Equals(other);
        }

        private class A
        {
            public string X { get; set; }
        }

        private class B : A
        {
            public new int X { get; set; }
        }

        private class ReferenceEqualityComparer : IEqualityComparer<object>
        {
            bool IEqualityComparer<object>.Equals(object x, object y)
            {
                return object.ReferenceEquals(x, y);
            }

            int IEqualityComparer<object>.GetHashCode(object obj)
            {
                return obj != null ? obj.GetHashCode() : 0;
            }
        }

        private class MemberComparerComparer : IEqualityComparer<MemberComparer>
        {
            public bool Equals(MemberComparer x, MemberComparer y)
            {
                return x.Comparer.GetType() == y.Comparer.GetType();
            }

            public int GetHashCode(MemberComparer obj)
            {
                return obj.GetHashCode();
            }
        }

        private sealed class PropertiesThatHaveNullValues : IEnumerable<object[]>
        {

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] {  
                    new ConcreteType(null,null),  
                    new ConcreteType(null, null, null), (byte)12 };

                yield return new object[] {  
                    new ConcreteType(null, Guid.NewGuid().ToString()),  
                    new ConcreteType(null, 2, RandomString()), (byte)12 };

                yield return new object[] {  
                    new ConcreteType(Guid.NewGuid().ToString(), null),  
                    new ConcreteType(null, 2, RandomString()), (byte)12 };

                yield return new object[] {  
                    new ConcreteType(Guid.NewGuid().ToString(), null),  
                    new CompositeType(null, 
                                        new ConcreteType(null, null), 
                                        new ConcreteType(RandomString(), null),
                                        new ConcreteType(null, null)), (byte)12 };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            private string RandomString()
            {
                return Guid.NewGuid().ToString();
            }
        }
    }
}