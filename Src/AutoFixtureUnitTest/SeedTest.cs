using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest
{
    [TestClass]
    public class SeedTest
    {
        [TestMethod]
        public void SutIsCustomAttributeProvider()
        {
            // Fixture setup
            // Exercise system
            var sut = new Seed(typeof(object), new object());
            // Verify outcome
            Assert.IsInstanceOfType(sut, typeof(ICustomAttributeProvider));
            // Teardown
        }

        [TestMethod]
        public void GetCustomAttributesWillReturnInstance()
        {
            // Fixture setup
            var sut = new Seed(typeof(string), string.Empty);
            // Exercise system
            var result = sut.GetCustomAttributes(true);
            // Verify outcome
            Assert.IsNotNull(result, "GetCustomAttributes");
            // Teardown
        }

        [TestMethod]
        public void GetSpecificCustomAttributesWillReturnInstance()
        {
            // Fixture setup
            var sut = new Seed(typeof(int), 1);
            // Exercise system
            var result = sut.GetCustomAttributes(typeof(DescriptionAttribute), false);
            // Verify outcome
            Assert.IsNotNull(result, "GetCustomAttributes");
            // Teardown
        }

        [TestMethod]
        public void IsDefinedWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new Seed(typeof(decimal), -1);
            // Exercise system
            var result = sut.IsDefined(typeof(FlagsAttribute), true);
            // Verify outcome
            Assert.IsFalse(result, "IsDefined");
            // Teardown
        }

        [TestMethod]
        public void ValueIsCorrect()
        {
            // Fixture setup
            var expectedValue = "Anonymous value";
            var sut = new Seed(typeof(string), expectedValue);
            // Exercise system
            var result = sut.Value;
            // Verify outcome
            Assert.AreEqual(expectedValue, result, "Value");
            // Teardown
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void CreateWithNullTypeWillThrow()
        {
            // Fixture setup
            // Exercise system
            new Seed(null, new object());
            // Verify outcome (expected exception)
            // Teardown
        }

        [TestMethod]
        public void TypeIsCorrect()
        {
            // Fixture setup
            var expectedType = typeof(string);
            var sut = new Seed(expectedType, "Anonymous value");
            // Exercise system
            var result = sut.TargetType;
            // Verify outcome
            Assert.AreEqual(expectedType, result, "Type");
            // Teardown
        }

        [TestMethod]
        public void SutIsEquatable()
        {
            // Fixture setup
            // Exercise system
            var sut = new Seed(typeof(decimal), 1);
            // Verify outcome
            Assert.IsInstanceOfType(sut, typeof(IEquatable<Seed>));
            // Teardown
        }

        [TestMethod]
        public void SutDoesNotEqualNullObject()
        {
            // Fixture setup
            var sut = new Seed(typeof(DateTime), new DateTime(103029));
            object other = null;
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsFalse(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutDoesNotEqualNullSut()
        {
            // Fixture setup
            var sut = new Seed(typeof(TimeSpan), new object());
            Seed other = null;
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsFalse(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutDoesNotEqualAnonymousObject()
        {
            // Fixture setup
            var sut = new Seed(typeof(object), "Anonymous value");
            object anonymousObject = new FileStyleUriParser();
            // Exercise system
            var result = sut.Equals(anonymousObject);
            // Verify outcome
            Assert.IsFalse(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutDoesNotEqualOtherObjectWhenTypesDiffer()
        {
            // Fixture setup
            var anonymousValue = 1;
            var sut = new Seed(typeof(string), anonymousValue);
            object other = new Seed(typeof(TimeSpan), anonymousValue);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsFalse(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutDoesNotEqualOtherSutWhenTypesDiffer()
        {
            // Fixture setup
            var anonymousValue = 1;
            var sut = new Seed(typeof(string), anonymousValue);
            var other = new Seed(typeof(TimeSpan), anonymousValue);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsFalse(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutDoesNotEqualOtherObjectWhenValuesDiffer()
        {
            // Fixture setup
            var anonymousType = typeof(object);
            var sut = new Seed(anonymousType, 98);
            object other = new Seed(anonymousType, "Anonymous value");
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsFalse(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutDoesNotEqualOtherSutWhenValuesDiffer()
        {
            // Fixture setup
            var anonymousType = typeof(object);
            var sut = new Seed(anonymousType, 98);
            var other = new Seed(anonymousType, "Anonymous value");
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsFalse(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutDoesNotEqualOtherObjectWhenSutValueIsNull()
        {
            // Fixture setup
            var anonymousType = typeof(float);
            var sut = new Seed(anonymousType, null);
            object other = new Seed(anonymousType, 2.9f);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsFalse(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutDoesNotEqualOtherSutWhenSutValueIsNull()
        {
            // Fixture setup
            var anonymousType = typeof(float);
            var sut = new Seed(anonymousType, null);
            var other = new Seed(anonymousType, 2.9f);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsFalse(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutDoesNotEqualOtherObjectWhenOtherValueIsNull()
        {
            // Fixture setup
            var anonymousType = typeof(Buffer);
            var sut = new Seed(anonymousType, new GopherStyleUriParser());
            object other = new Seed(anonymousType, null);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsFalse(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutDoesNotEqualOtherSutWhenOtherValueIsNull()
        {
            // Fixture setup
            var anonymousType = typeof(Buffer);
            var sut = new Seed(anonymousType, new GopherStyleUriParser());
            var other = new Seed(anonymousType, null);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsFalse(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutEqualsOtherObjectWhenTypeAndValueEquals()
        {
            // Fixture setup
            var type = typeof(HttpStyleUriParser);
            var value = new OperatingSystem(PlatformID.WinCE, new Version());
            var sut = new Seed(type, value);
            object other = new Seed(type, value);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsTrue(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutEqualsOtherSutWhenTypeAndValueEquals()
        {
            // Fixture setup
            var type = typeof(HttpStyleUriParser);
            var value = new OperatingSystem(PlatformID.WinCE, new Version());
            var sut = new Seed(type, value);
            var other = new Seed(type, value);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsTrue(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutEqualsOtherObjectWhenTypesAreEqualAndValuesAreNull()
        {
            // Fixture setup
            var type = typeof(WeakReference);
            var sut = new Seed(type, null);
            object other = new Seed(type, null);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsTrue(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutEqualsOtherSutWhenTypesAreEqualAndValuesAreNull()
        {
            // Fixture setup
            var type = typeof(WeakReference);
            var sut = new Seed(type, null);
            var other = new Seed(type, null);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsTrue(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void GetHashCodeWillReturnCorrectResultWhenValueIsNull()
        {
            // Fixture setup
            var type = typeof(Version);
            var sut = new Seed(type, null);
            var expectedHashCode = type.GetHashCode();
            // Exercise system
            var result = sut.GetHashCode();
            // Verify outcome
            Assert.AreEqual(expectedHashCode, result, "GetHashCode");
            // Teardown
        }

        [TestMethod]
        public void GetHashCodeWillReturnCorrectResult()
        {
            // Fixture setup
            var type = typeof(NetPipeStyleUriParser);
            var value = Missing.Value;
            var sut = new Seed(type, value);
            var expectedHashCode = type.GetHashCode() ^ value.GetHashCode();
            // Exercise system
            var result = sut.GetHashCode();
            // Verify outcome
            Assert.AreEqual(expectedHashCode, result, "GetHashCode");
            // Teardown
        }
    }
}
