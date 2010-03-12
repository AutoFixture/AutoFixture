using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    [TestClass]
    public class NoSpecimenTest
    {
        [TestMethod]
        public void DefaultConstructorWillSetRequestToNull()
        {
            // Fixture setup
            var sut = new NoSpecimen();
            // Exercise system
            var result = sut.Request;
            // Verify outcome
            Assert.IsNull(result, "Request");
            // Teardown
        }

        [TestMethod]
        public void CreateWithNullRequestWillSetCorrectRequest()
        {
            // Fixture setup
            // Exercise system
            var sut = new NoSpecimen(null);
            // Verify outcome
            Assert.IsNull(sut.Request, "Request");
            // Teardown
        }

        [TestMethod]
        public void RequestWillMatchContructorArgument()
        {
            // Fixture setup
            var expectedRequest = new object();
            var sut = new NoSpecimen(expectedRequest);
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
            var sut = new NoSpecimen();
            // Verify outcome
            Assert.IsInstanceOfType(sut, typeof(IEquatable<NoSpecimen>));
            // Teardown
        }

        [TestMethod]
        public void SutDoesNotEqualNullObject()
        {
            // Fixture setup
            var sut = new NoSpecimen();
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
            var sut = new NoSpecimen();
            NoSpecimen other = null;
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
            var sut = new NoSpecimen();
            var anonymousObject = new object();
            // Exercise system
            var result = sut.Equals(anonymousObject);
            // Verify outcome
            Assert.IsFalse(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutDoesNotEqualOtherObjectWhenSutRequestIsNull()
        {
            // Fixture setup
            var sut = new NoSpecimen();
            object other = new NoSpecimen(new object());
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsFalse(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutDoesNotEqualOtherSutWhenSutRequestIsNull()
        {
            // Fixture setup
            var sut = new NoSpecimen();
            var other = new NoSpecimen(new object());
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsFalse(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutDoesNotEqualOtherObjectWhenOtherRequestIsNull()
        {
            // Fixture setup
            var sut = new NoSpecimen(new object());
            object other = new NoSpecimen();
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsFalse(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutDoesNotEqualOtherSutWhenOtherRequestIsNull()
        {
            // Fixture setup
            var sut = new NoSpecimen(new object());
            var other = new NoSpecimen();
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsFalse(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutDoesNotEqualOtherObjectWhenRequestsDiffer()
        {
            // Fixture setup
            var sut = new NoSpecimen(new object());
            object other = new NoSpecimen(new object());
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
            var sut = new NoSpecimen(new object());
            var other = new NoSpecimen(new object());
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsFalse(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutEqualsOtherObjectWhenBothRequestsAreNull()
        {
            // Fixture setup
            var sut = new NoSpecimen();
            object other = new NoSpecimen();
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsTrue(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutEqualsOtherSutWhenBothRequestsAreNull()
        {
            // Fixture setup
            var sut = new NoSpecimen();
            var other = new NoSpecimen();
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsTrue(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutEqualsOtherObjectWhenRequestsAreEqual()
        {
            // Fixture setup
            var request = new object();
            var sut = new NoSpecimen(request);
            object other = new NoSpecimen(request);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsTrue(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutEqualsOtherSutWhenRequestsAreEqual()
        {
            // Fixture setup
            var request = new object();
            var sut = new NoSpecimen(request);
            var other = new NoSpecimen(request);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsTrue(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void GetHashCodeWhenRequestIsNullWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new NoSpecimen();
            // Exercise system
            var result = sut.GetHashCode();
            // Verify outcome
            Assert.AreEqual(0, result, "GetHashCode");
            // Teardown
        }

        [TestMethod]
        public void GetHashCodeWhenRequestIsNotNullWillReturnCorrectResult()
        {
            // Fixture setup
            var request = new object();
            var sut = new NoSpecimen(request);
            // Exercise system
            var result = sut.GetHashCode();
            // Verify outcome
            var expectedHashCode = request.GetHashCode();
            Assert.AreEqual(expectedHashCode, result, "GetHashCode");
            // Teardown
        }
    }
}
