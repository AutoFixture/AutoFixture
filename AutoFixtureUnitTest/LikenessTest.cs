using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.TestTypeFoundation;

namespace Ploeh.AutoFixtureUnitTest
{
    // Regression tests of obsolete API until it is removed completely
#pragma warning disable 618

    [TestClass]
    public class LikenessTest
    {
        public LikenessTest()
        {
        }

        [TestMethod]
        public void SutEncapsulatingNullEqualsNull()
        {
            // Fixture setup
            var sut = new Likeness(null);
            // Exercise system
            var result = sut.Equals(null);
            // Verify outcome
            Assert.IsTrue(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutEncapsulatingNullDoesNotEqualsSomeObject()
        {
            // Fixture setup
            var sut = new Likeness(null);
            // Exercise system
            var result = sut.Equals(new object());
            // Verify outcome
            Assert.IsFalse(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutWithValueDoesNotEqualNull()
        {
            // Fixture setup
            var sut = new Likeness(new object());
            // Exercise system
            var result = sut.Equals(null);
            // Verify outcome
            Assert.IsFalse(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void GetHashCodeShouldReturnHashCodeOfContainedObject()
        {
            // Fixture setup
            var anonymousDateTime = new DateTime(2009, 6, 25);
            int expectedHashCode = anonymousDateTime.GetHashCode();

            var sut = new Likeness(anonymousDateTime);
            // Exercise system
            int result = sut.GetHashCode();
            // Verify outcome
            Assert.AreEqual<int>(expectedHashCode, result, "GetHashCode");
            // Teardown
        }

        [TestMethod]
        public void ToStringReturnsCorrectResult()
        {
            // Fixture setup
            var anonymousTimeSpan = new TimeSpan(7, 4, 2, 1);
            string expectedText = "Likeness of " + anonymousTimeSpan.ToString();

            var sut = new Likeness(anonymousTimeSpan);
            // Exercise system
            var result = sut.ToString();
            // Verify outcome
            Assert.AreEqual<string>(expectedText, result, "ToString");
            // Teardown
        }

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
        public void LikenessOfObjectWithPropertyWillNotBeEqualToPropertyWithDifferentProperty()
        {
            // Fixture setup
            PropertyHolder<object> likenObject = new PropertyHolder<object>();
            likenObject.Property = new object();

            var comparee = new { SomeOtherProperty = new object() };

            // The rest of the test
            LikenessTest.CompareLikenessToObject(likenObject, comparee, false);
        }

        [TestMethod]
        public void LikenessAgainstObjectWithOverloadedMembersWillNotThrow()
        {
            // Fixture setup
            var likenObject = new object();

            var comparee = new TypeWithOverloadedMembers();

            // The rest of the test
            LikenessTest.CompareLikenessToObject(likenObject, comparee, true);
        }

        [TestMethod]
        public void LikenessAgainstObjectWithIndexerWillNotThrow()
        {
            // Fixture setup
            var likenObject = new object();

            var comparee = new TypeWithIndexer();

            // The rest of the test
            LikenessTest.CompareLikenessToObject(likenObject, comparee, true);
        }

        [TestMethod]
        public void LikenessAgainstObjectWithHidingPropertyWillNotThrow()
        {
            // Fixture setup
            var likenObject = new A();

            var comparee = new B();

            // The rest of the test
            LikenessTest.CompareLikenessToObject(likenObject, comparee, false);
        }

        [TestMethod]
        public void LikenessOfObjectWithHidingPropertyWillNotThrow()
        {
            // Fixture setup
            var likenObject = new B();

            var comparee = new A();

            // The rest of the test
            LikenessTest.CompareLikenessToObject(likenObject, comparee, false);
        }

        private static void CompareLikenessToObject(object likenObject, object comparee, bool expectedResult)
        {
            // Fixture setup
            Likeness sut = new Likeness(likenObject);
            // Exercise system
            bool result = sut.Equals(comparee);
            // Verify outcome
            Assert.AreEqual<bool>(expectedResult, result, "If all public properties and fields are equal, Likeness should indicate equality.");
            // Teardown
        }

        public class A
        {
            public string X { get; set; }
        }

        public class B : A
        {
            public new int X { get; set; }
        }
    }

#pragma warning restore 618
}
