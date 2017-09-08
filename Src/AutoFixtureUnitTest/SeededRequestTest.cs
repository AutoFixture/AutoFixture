using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest
{
    [TestClass]
    public class SeededRequestTest
    {
        [TestMethod]
        public void SeedIsCorrect()
        {
            // Fixture setup
            var expectedSeed = "Anonymous value";
            var sut = new SeededRequest(typeof(string), expectedSeed);
            // Exercise system
            var result = sut.Seed;
            // Verify outcome
            Assert.AreEqual(expectedSeed, result, "Seed");
            // Teardown
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void CreateWithNullRequestWillThrow()
        {
            // Fixture setup
            // Exercise system
            new SeededRequest(null, new object());
            // Verify outcome (expected exception)
            // Teardown
        }

        [TestMethod]
        public void RequestIsCorrect()
        {
            // Fixture setup
            var expectedRequest = new object();
            var sut = new SeededRequest(expectedRequest, "Anonymous value");
            // Exercise system
            var result = sut.Request;
            // Verify outcome
            Assert.AreEqual(expectedRequest, result, "Request");
            // Teardown
        }

        [TestMethod]
        public void SutIsEquatable()
        {
            // Fixture setup
            // Exercise system
            var sut = new SeededRequest(typeof(decimal), 1);
            // Verify outcome
            Assert.IsInstanceOfType(sut, typeof(IEquatable<SeededRequest>));
            // Teardown
        }

        [TestMethod]
        public void SutDoesNotEqualNullObject()
        {
            // Fixture setup
            var sut = new SeededRequest(typeof(DateTime), new DateTime(103029));
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
            var sut = new SeededRequest(typeof(TimeSpan), new object());
            SeededRequest other = null;
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
            var sut = new SeededRequest(new object(), "Anonymous value");
            object anonymousObject = new FileStyleUriParser();
            // Exercise system
            var result = sut.Equals(anonymousObject);
            // Verify outcome
            Assert.IsFalse(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutDoesNotEqualOtherObjectWhenRequestsDiffer()
        {
            // Fixture setup
            var anonymousValue = 1;
            var sut = new SeededRequest(new object(), anonymousValue);
            object other = new SeededRequest(typeof(TimeSpan), anonymousValue);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsFalse(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutDoesNotEqualOtherSutWhenRequestsDiffer()
        {
            // Fixture setup
            var anonymousValue = 1;
            var sut = new SeededRequest(new object(), anonymousValue);
            var other = new SeededRequest(typeof(TimeSpan), anonymousValue);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsFalse(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutDoesNotEqualOtherObjectWhenSeedsDiffer()
        {
            // Fixture setup
            var anonymousRequest = new object();
            var sut = new SeededRequest(anonymousRequest, 98);
            object other = new SeededRequest(anonymousRequest, "Anonymous value");
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsFalse(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutDoesNotEqualOtherSutWhenSeedsDiffer()
        {
            // Fixture setup
            var anonymousRequest = 1;
            var sut = new SeededRequest(anonymousRequest, 98);
            var other = new SeededRequest(anonymousRequest, "Anonymous value");
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsFalse(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutDoesNotEqualOtherObjectWhenSutSeedIsNull()
        {
            // Fixture setup
            var anonymousRequest = string.Empty;
            var sut = new SeededRequest(anonymousRequest, null);
            object other = new SeededRequest(anonymousRequest, 2.9f);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsFalse(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutDoesNotEqualOtherSutWhenSutSeedIsNull()
        {
            // Fixture setup
            var anonymousRequest = typeof(float);
            var sut = new SeededRequest(anonymousRequest, null);
            var other = new SeededRequest(anonymousRequest, 2.9f);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsFalse(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutDoesNotEqualOtherObjectWhenOtherSeedIsNull()
        {
            // Fixture setup
            var anonymousRequest = typeof(Buffer);
            var sut = new SeededRequest(anonymousRequest, new GopherStyleUriParser());
            object other = new SeededRequest(anonymousRequest, null);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsFalse(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutDoesNotEqualOtherSutWhenOtherSeedIsNull()
        {
            // Fixture setup
            var anonymousRequest = typeof(Buffer);
            var sut = new SeededRequest(anonymousRequest, new GopherStyleUriParser());
            var other = new SeededRequest(anonymousRequest, null);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsFalse(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutEqualsOtherObjectWhenRequestAndSeedEquals()
        {
            // Fixture setup
            var request = typeof(HttpStyleUriParser);
            var seed = new OperatingSystem(PlatformID.WinCE, new Version());
            var sut = new SeededRequest(request, seed);
            object other = new SeededRequest(request, seed);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsTrue(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutEqualsOtherSutWhenRequestsAndSeedEquals()
        {
            // Fixture setup
            var request = typeof(HttpStyleUriParser);
            var seed = new OperatingSystem(PlatformID.WinCE, new Version());
            var sut = new SeededRequest(request, seed);
            var other = new SeededRequest(request, seed);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsTrue(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutEqualsOtherObjectWhenRequestsAreEqualAndSeedsAreNull()
        {
            // Fixture setup
            var request = typeof(WeakReference);
            var sut = new SeededRequest(request, null);
            object other = new SeededRequest(request, null);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsTrue(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutEqualsOtherSutWhenRequestsAreEqualAndSeedsAreNull()
        {
            // Fixture setup
            var request = typeof(WeakReference);
            var sut = new SeededRequest(request, null);
            var other = new SeededRequest(request, null);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsTrue(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void GetHashCodeWillReturnCorrectResultWhenSeedIsNull()
        {
            // Fixture setup
            var request = typeof(Version);
            var sut = new SeededRequest(request, null);
            var expectedHashCode = request.GetHashCode();
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
            var request = typeof(NetPipeStyleUriParser);
            var value = Missing.Value;
            var sut = new SeededRequest(request, value);
            var expectedHashCode = request.GetHashCode() ^ value.GetHashCode();
            // Exercise system
            var result = sut.GetHashCode();
            // Verify outcome
            Assert.AreEqual(expectedHashCode, result, "GetHashCode");
            // Teardown
        }
    }
}
