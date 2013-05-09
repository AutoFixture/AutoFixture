using Ploeh.SemanticComparison.Fluent;
using Ploeh.TestTypeFoundation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

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
        public void ResemblanceIsNotNull()
        {
            // Fixture setup
            var source = new ConcreteType();
            var sut = source.AsSource().OfLikeness<AbstractType>();
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
            var source = new ConcreteType();
            var sut = source.AsSource().OfLikeness<AbstractType>();
            // Exercise system
            var result = sut.ToResemblance();
            // Verify outcome
            Assert.IsAssignableFrom<AbstractType>(result);
            // Teardown
        }

        [Fact]
        public void ResemblanceReturnsDifferentInstanceWhenAccessedMultipleTimes()
        {
            // Fixture setup
            var source = new ConcreteType();
            var sut = source.AsSource().OfLikeness<AbstractType>();
            var expectedProxies = new[] { sut.ToResemblance(), sut.ToResemblance(), sut.ToResemblance() };
            // Exercise system
            var result = Enumerable.Range(1, expectedProxies.Length)
                .Select(x => sut.ToResemblance())
                .ToArray();
            // Verify outcome
            Assert.False(expectedProxies.SequenceEqual(result, new ReferenceEqualityComparer()));
            // Teardown
        }

        [Fact]
        public void ResemblanceInstanceIsDifferentThanSourceInstance()
        {
            // Fixture setup
            var source = new ConcreteType();
            var sut = source.AsSource().OfLikeness<AbstractType>();
            // Exercise system
            var result = sut.ToResemblance();
            // Verify outcome
            Assert.NotSame(source, result);
            // Teardown
        }

        [Fact]
        public void ResemblanceDoesNotEqualNullObject()
        {
            // Fixture setup
            var source = new ConcreteType();
            var sut = source.AsSource().OfLikeness<AbstractType>();
            // Exercise system
            var result = sut.ToResemblance();
            // Verify outcome
            Assert.False(result.Equals(null));
            // Teardown
        }

        [Fact]
        public void ResemblanceDoesNotEqualNullSource()
        {
            // Fixture setup
            var source = new ConcreteType();
            var sut = source.AsSource().OfLikeness<AbstractType>();
            // Exercise system
            var result = sut.ToResemblance();
            // Verify outcome
            Assert.False(result.Equals((ConcreteType)null));
            // Teardown
        }

        [Fact]
        public void MutableProxyWhenCalledMultipleTimesForSemanticallyEqualObjectsReturnsTrue()
        {
            // Fixture setup
            var source = new ConcreteType();
            var sut = source.AsSource().OfLikeness<AbstractType>().ToMutableProxy();
            var expectedResult = Enumerable.Range(1, 3).Select(x => true);
            // Exercise system
            var result = Enumerable.Range(1, 3)
                .Select(x => sut.Equals(new ConcreteType()));
            // Verify outcome
            Assert.True(expectedResult.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void ResemblanceWhenCalledMultipleTimesForSemanticallyEqualObjectsReturnsTrue()
        {
            // Fixture setup
            var source = new ConcreteType();
            var sut = source.AsSource().OfLikeness<AbstractType>().ToResemblance();
            var expectedResult = Enumerable.Range(1, 3).Select(x => true);
            // Exercise system
            var result = Enumerable.Range(1, 3)
                .Select(x => sut.Equals(new ConcreteType()));
            // Verify outcome
            Assert.True(expectedResult.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void ResemblanceWhenCalledMultipleTimesForSemanticallyUnequalObjectsReturnsFalse()
        {
            // Fixture setup
            var source = new ConcreteType("Lorem");
            var sut = source.AsSource().OfLikeness<AbstractType>().ToResemblance();

            var expectedResult = Enumerable.Range(1, 3).Select(x => false);
            // Exercise system
            var result = Enumerable.Range(1, 3)
                .Select(x => sut.Equals(new ConcreteType("ipsum")));
            // Verify outcome
            Assert.True(expectedResult.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void ResemblanceOfAbstractTypeEqualsConcreteInstancesThatDifferOnlyOnMemberNotDefinedByAbstraction()
        {
            // Fixture setup
            var other = new ConcreteType("Lorem", "ipsum", "dolor", "sit");
            other.Property5 = "ploeh";

            var sut = other.AsSource().OfLikeness<AbstractType>().ToResemblance();
            
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void MutableProxyOfAbstractTypeDoesNotEqualConcreteInstanceWhenPropertyDiffers()
        {
            // Fixture setup
            var other = new ConcreteType("Lorem", "ipsum", "dolor", "sit");
            other.Property4 = "ploeh";

            var sut = other.AsSource().OfLikeness<AbstractType>().ToMutableProxy();
            sut.Property4 = "Fnaah";

            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void ResemblanceGetHashCodeDoesNotEqualRealGetHashCode()
        {
            // Fixture setup
            var source = new TypeOverridingGetHashCode();
            int expected = source.GetHashCode();
            var sut = source.AsSource().OfLikeness<TypeOverridingGetHashCode>();
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
            // Exercise system and verify outcome
            Assert.Throws<ProxyCreationException>(
                () => new ConcreteType().AsSource().OfLikeness<PublicSealedType>().ToResemblance());
            // Teardown
        }

        [Fact]
        public void ResemblanceOfTypeWithAccessibleConstructorDoesNotThrowWhenSourceTypeIsAnonymousType()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.DoesNotThrow(
                () => new { }.AsSource().OfLikeness<AbstractType>().ToResemblance());
            // Teardown
        }

        [Fact]
        public void ResemblanceOfTypeWithNonDefaultConstructorThrowsWhenSourceConstructorValuesDoNotMapToDestinationConstructor()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ProxyCreationException>(
                () => new { }.AsSource().OfLikeness<AbstractTypeWithNonDefaultConstructor<string>>().ToResemblance());
            // Teardown
        }

        [Fact]
        public void ResemblanceOfAbstractTypeWithNonDefaultConstructorDoesNotThrow()
        {
            // Fixture setup
            var value = new PropertyHolder<string>();
            value.Property = "Foo";
            var sut = value.AsSource().OfLikeness<AbstractTypeWithNonDefaultConstructor<string>>();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.ToResemblance());
            // Teardown
        }

        [Fact]
        public void ResemblanceOfQuadrupleParameterTypeEqualsTripleParameterType()
        {
            // Fixture setup
            var original = new QuadrupleParameterType<int, double, long, string>(1, 2.0, 3, "4");
            var other = new TripleParameterType<int, double, long>(1, 2.0, 3);

            var sut = original.AsSource().OfLikeness<TripleParameterType<int, double, long>>().ToResemblance();
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result);
        }

        [Fact]
        public void ResemblanceOfQuadrupleParameterTypeEqualsDoubleParameterType()
        {
            // Fixture setup
            var original = new QuadrupleParameterType<int, double, long, string>(1, 2.0, 3, "4");
            var other = new DoubleParameterType<int, double>(1, 2.0);

            var sut = original.AsSource().OfLikeness<DoubleParameterType<int, double>>().ToResemblance();
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result);
        }

        [Fact]
        public void ResemblanceOfQuadrupleParameterTypeEqualsSingleParameterType()
        {
            // Fixture setup
            var original = new QuadrupleParameterType<int, double, long, string>(1, 2.0, 3, "4");
            var other = new SingleParameterType<int>(1);

            var sut = original.AsSource().OfLikeness<SingleParameterType<int>>()
                .With(d => d.Parameter).EqualsWhen((s, d) => s.Parameter1 == d.Parameter)
                .ToResemblance();
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result);
        }

        [Fact]
        public void ResemblanceEqualsIsTrueWhenCorrectMappingHasBeenDefined()
        {
            // Fixture setup
            var original = new PropertyHolder<string> {Property = "Foo"};
            var other = new FieldHolder<string> {Field = "Foo"};

            var sut = new Likeness<PropertyHolder<string>, FieldHolder<string>>(original)
                .With(d => d.Field).EqualsWhen((s, d) => s.Property == d.Field)
                .ToResemblance();
            sut.Field = original.Property;

            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void ResemblanceWithoutPropertyEqualsInstanceEvenIfItDiffersOnThatProperty()
        {
            // Fixture setup
            var other = new ConcreteType("Lorem", "ipsum", "dolor", "sit");
            other.Property5 = "amet";

            var sut = other.AsSource().OfLikeness<ConcreteType>()
                .Without(x => x.Property5)
                .ToResemblance();
            sut.Property5 = "Fnaah";
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void ResemblanceWithoutTwoPropertiesEqualsInstanceEvenThoughThosePropertyValuesDiffer()
        {
            // Fixture setup
            var value = new ConcreteType("Lorem", "ipsum", "dolor", "sit");
            value.Property5 = "amet";

            var sut = value.AsSource().OfLikeness<ConcreteType>()
                .Without(x => x.Property5).Without(x => x.Property1)
                .ToResemblance();
            sut.Property1 = "Ndøh";
            sut.Property5 = "Sqryt";
            // Exercise system
            var result = sut.Equals(value);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void ResemblanceWithSemanticEqualityWillReturnTrue()
        {
            // Fixture setup
            var original = new ConcreteType("Lorem", "ipsum", "dolor", "sit");
            var other = new DoublePropertyHolder<object, object>
                {
                    Property1 = "Lorem",
                    Property2 = "ipsum"
                };

            var sut = original.AsSource().OfLikeness<DoublePropertyHolder<object, object>>()
                .ToResemblance();
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void MutableProxyOmitAutoComparisonWillCauseDifferentObjectsToLookEqual()
        {
            // Fixture setup
            var value = new ConcreteType("Lorem", "ipsum", "dolor", "sit");

            var sut = value.AsSource().OfLikeness<ConcreteType>()
                .OmitAutoComparison()
                .ToMutableProxy();
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
        public void MutableProxyOmitAutoComparisonFollowedByWithDefaultEqualityWillOptInOnThosePropertiesOnlyAndReturnTrueWhenTheyMatch()
        {
            // Fixture setup
            var value = new ConcreteType("Lorem", "ipsum", "dolor", "sit");

            var sut = value.AsSource().OfLikeness<ConcreteType>()
                .OmitAutoComparison()
                .WithDefaultEquality(d => d.Property1)
                .WithDefaultEquality(d => d.Property3)
                .ToMutableProxy();
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
        public void MutableProxyOmitAutoComparisonFollowedByWithDefaultEqualityWillOptInOnThosePropertiesOnlyAndReturnFalseWhenTheyDoNotMatch()
        {
            // Fixture setup
            var value = new ConcreteType("Lorem", "ipsum", "dolor", "sit");

            var sut = value.AsSource().OfLikeness<AbstractType>()
                .OmitAutoComparison()
                .WithDefaultEquality(d => d.Property1)
                .WithDefaultEquality(d => d.Property4)
                .ToMutableProxy();
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
        public void MutableProxyOmitAutoComparisonFollowedByCorrectComboOfDefaultEqualityAndExplictyWithReturnsTrue()
        {
            // Fixture setup
            var value = new ConcreteType("Lorem", "ipsum", "dolor", "sit");

            var sut = value.AsSource().OfLikeness<AbstractType>()
                .OmitAutoComparison()
                .WithDefaultEquality(d => d.Property1)
                .With(d => d.Property2).EqualsWhen((s, d) => s.Property2.ToString().ToUpper() == d.Property2.ToString())
                .ToMutableProxy();
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
        public void ResemblanceOfIdenticalParameterTypesCanBeCreated()
        {
            // Fixture setup
            var sut = new TripleParameterType<long, long, long>(1, 2, 3)
                .AsSource().OfLikeness<TripleParameterType<long, long, long>>();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.ToResemblance());
            // Teardown
        }

        [Fact]
        public void ResemblanceOfIdenticalParameterTypesWithPropertiesCanBeCreated()
        {
            // Fixture setup
            var sut = new TypeWithIdenticalParameterTypesAndProperties(1, 2, 3)
                .AsSource().OfLikeness<TypeWithIdenticalParameterTypesAndProperties>();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.ToResemblance());
            // Teardown
        }

        [Fact]
        public void ResemblanceOfTypeWithDifferentParameterTypesAndPropertiesCanBeCreated()
        {
            // Fixture setup
            var sut = new TypeWithDifferentParameterTypesAndProperties(1, "2", 3)
                .AsSource().OfLikeness<TypeWithDifferentParameterTypesAndProperties>();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.ToResemblance());
            // Teardown
        }

        [Fact]
        public void ResemblanceOfTypeWithIncompatibleAndCompatibleConstructorCanBeCreated()
        {
            // Fixture setup
            var value = new TypeWithIncompatibleAndCompatibleConstructor(
               new ConcreteType());

            var sut = value
                .AsSource()
                .OfLikeness<TypeWithIncompatibleAndCompatibleConstructor>();
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

            var sut = value
                .AsSource()
                .OfLikeness<TypeWithIncompatibleAndCompatibleConstructor>();
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

            var sut = value
                .AsSource()
                .OfLikeness<TypeWithIncompatibleAndCompatibleConstructor>();
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

            var sut = value
                .AsSource()
                .OfLikeness<TypeWithUnorderedProperties>();
            // Exercise system
            var result = sut.ToResemblance();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void ResemblanceAndLikenessHaveSameBehavior1()
        {
            // Fixture setup
            var original = new ConcreteType
            {
                Property1 = "value1",
                Property2 = "value2"
            };
            var other = new ConcreteType
            {
                Property1 = "abc",
                Property2 = "value2"
            };

            var likeness = original.AsSource().OfLikeness<ConcreteType>()
                .Without(x => x.Property1);

            var expected = likeness.Equals(other);

            var sut = likeness.ToResemblance();
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result);
            Assert.Equal(expected, result);
            // Teardown
        }

        [Fact]
        public void ResemblanceAndLikenessHaveSameBehavior2()
        {
            // Fixture setup
            var original = new
            {
                Property1 = "value1",
                Property2 = new[] {"A", "B", "C"},
                Property3 = (object) null,
                Property4 = (object) null,
                Property5 = (object) null
            };

            var other = new ConcreteType
            {
                Property1 = "value1",
                Property2 = "A,B,C",
                Property3 = null,
                Property4 = null,
                Property5 = null
            };

            var likeness = original
                .AsSource().OfLikeness<ConcreteType>()
                .With(x => x.Property2).EqualsWhen((a, b) => string.Join(",", a.Property2) == (string)b.Property2);

            var sut = likeness.ToResemblance();

            var expected = likeness.Equals(other);

            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(expected);
            Assert.Equal(expected, result);
            // Teardown
        }

        [Fact]
        public void ResemblanceAndLikenessHaveSameBehavior3()
        {
            // Fixture setup
            var original = new ConcreteType
            {
                Property1 = "value1",
                Property2 = "abc",
            };

            var other = new ConcreteType
            {
                Property1 = "value1",
                Property2 = "cba"
            };

            var likeness = original
                .AsSource().OfLikeness<ConcreteType>()
                .With(x => x.Property2).EqualsWhen((a, b) =>
                    {
                        return ((string) b.Property2).Reverse().SequenceEqual((IEnumerable<char>) a.Property2);
                    });

            var sut = likeness.ToResemblance();

            var expected = likeness.Equals(other);

            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(expected);
            Assert.Equal(expected, result);
            // Teardown
        }

        [Fact]
        public void ResemblanceAndLikenessHaveSameBehavior4()
        {
            var obj = new
            {
                A = 1,
                B = "hello",
                C = new Dictionary<string, string>
                    {
                        {"A", "D"},
                        {"B", "E"},
                        {"C", "F"},
                    }
            };

            var likeness = obj
                .AsSource().OfLikeness<Test>()
                .With(x => x.C).EqualsWhen((a, b) => ToQueryString(a.C) == b.C);

            var other = new Test
            {
                A = 1,
                B = "hello",
                C = "A=D&B=E&C=F"
            };

            var result1 = likeness.Equals(other);
            var result2 = likeness.ToResemblance().Equals(other);

            // Verify outcome
            Assert.True(result1);
            Assert.Equal(result1, result2);
            // Teardown
        }

        public class Test
        {
            public int A { get; set; }
            public string B { get; set; }
            public string C { get; set; }
        }

        public static string ToQueryString(IDictionary<string, string> source)
        {
            return string.Join("&", source.Select(x => x.Key + "=" + x.Value));
        }

        [Fact]
        public void MutableProxyCanCorrectlyAssignsFieldValues()
        {
            // Fixture setup
            var expected = 3;
            var value = new FieldHolder<int>();
            value.Field = expected;

            var sut = value.AsSource().OfLikeness<FieldHolder<int>>()
                .ToMutableProxy();
            // Exercise system
            var result = sut.Field;
            // Verify outcome
            Assert.Equal(expected, result);
            // Teardown
        }

        [Fact]
        public void MutableProxyCanCorrectlyAssignsFieldValuesToTypeWithPublicFieldsAndProperties()
        {
            // Fixture setup
            var value = new TypeWithPublicFieldsAndProperties();
            value.AutomaticProperty = 1m;
            value.Field = "2";
            value.Number = 3;

            var sut = value.AsSource().OfLikeness<TypeWithPublicFieldsAndProperties>();

            // Exercise system
            var proxy = sut.ToMutableProxy();
            var result =
                   value.AutomaticProperty == proxy.AutomaticProperty
                && value.Field == proxy.Field
                && value.Number == proxy.Number;
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void MutableProxyCanCorrectlyAssignFieldValuesFromAnonymousType()
        {
            // Fixture setup
            var expected = 3.0;
            var value = new { Field = expected };
            var sut = value.AsSource().OfLikeness<FieldHolder<double>>()
                .ToMutableProxy();
            // Exercise system
            var result = sut.Field;
            // Verify outcome
            Assert.Equal(expected, result);
            // Teardown
        }

        [Fact]
        public void MutableProxyCanCorrectlyAssignFieldValuesFromAnonymousTypeToTypeWithPublicFieldsAndProperties()
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
                .ToMutableProxy();
            // Exercise system
            var result = 
                   value.AutomaticProperty == sut.AutomaticProperty 
                && value.Field == sut.Field 
                && value.Number == sut.Number;
            // Verify outcome
            Assert.True(result);
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

        public sealed class PublicSealedType 
        {
        }

        public class TypeOverridingGetHashCode
        {
            public override int GetHashCode()
            {
                return 14;
            }
        }

        public class TypeWithIdenticalParameterTypesAndProperties
        {
            private readonly long field1;
            private readonly long field2;
            private readonly long field3;
            private readonly long field4;

            public TypeWithIdenticalParameterTypesAndProperties(
                long parameter1,
                long parameter2,
                long parameter3)
            {
                this.field1 = parameter1;
                this.field2 = parameter2;
                this.field3 = parameter3;
                this.field4 = 400;
            }

            protected TypeWithIdenticalParameterTypesAndProperties(object source)
            {
                if (source == null)
                {
                    throw new ArgumentNullException("source");
                }
            }

            public long Property1
            {
                get { return this.field1; }
            }

            public long Property2
            {
                get { return this.field2; }
            }

            public long Property3
            {
                get { return this.field3; }
            }

            public long Property4
            {
                get { return this.field4; }
            }
        }

        public class TypeWithDifferentParameterTypesAndProperties
        {
            private readonly double field1;
            private readonly string field2;
            private readonly int field3;
            private readonly Guid field4;

            public TypeWithDifferentParameterTypesAndProperties(
                double field1, 
                string field2,
                int field3)
            {
                this.field1 = field1;
                this.field2 = field2;
                this.field3 = field3;
                this.field4 = Guid.NewGuid();
            }

            protected TypeWithDifferentParameterTypesAndProperties(object source)
            {
                if (source == null)
                {
                    throw new ArgumentNullException("source");
                }
            }

            public double Property1
            {
                get { return this.field1; }
            }

            public string Property2
            {
                get { return this.field2; }
            }

            public int Property3
            {
                get { return this.field3; }
            }

            public Guid Property4
            {
                get { return this.field4; }
            }
        }

        public class TypeWithIncompatibleAndCompatibleConstructor
        {
            private readonly AbstractType value1;
            private readonly AbstractType value2;
            private readonly byte value3;

            public TypeWithIncompatibleAndCompatibleConstructor(ConcreteType a)
                : this(new ConcreteType(), new CompositeType(a), new byte())
            {
            }

            public TypeWithIncompatibleAndCompatibleConstructor(ConcreteType a, byte b)
                : this(new ConcreteType(), new CompositeType(a), b)
            {
            }

            public TypeWithIncompatibleAndCompatibleConstructor(ConcreteType a, AbstractType b, byte c)
            {
                this.value1 = a;
                this.value2 = b;
                this.value3 = c;
            }

            public AbstractType Property1
            {
                get { return this.value1; }
            }

            public AbstractType Property2
            {
                get { return this.value2; }
            }

            public byte Property3
            {
                get { return this.value3; }
            }
        }

        public class TypeWithUnorderedProperties
        {
            private readonly AbstractType value1;
            private readonly AbstractType value2;
            private readonly byte value3;

            public TypeWithUnorderedProperties(ConcreteType a, AbstractType b, byte c)
            {
                this.value1 = a;
                this.value2 = b;
                this.value3 = c;
            }

            public byte Property3
            {
                get { return this.value3; }
            }

            public AbstractType Property1
            {
                get { return this.value1; }
            }

            public AbstractType Property2
            {
                get { return this.value2; }
            }
        }

        public class TypeWithPublicFieldsAndProperties
        {
            public string Field;

            private long number;

            public long Number
            {
                get { return this.number; }
                set { this.number = value; }
            }

            public decimal AutomaticProperty { get; set; }
        }
    }
}