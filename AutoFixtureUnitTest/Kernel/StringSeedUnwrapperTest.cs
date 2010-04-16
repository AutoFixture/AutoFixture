using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class StringSeedUnwrapperTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new StringSeedUnwrapper();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestWillReturnNull()
        {
            // Fixture setup
            var sut = new StringSeedUnwrapper();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
            var result = sut.Create(null, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWithNullContainerWillThrow()
        {
            // Fixture setup
            var sut = new StringSeedUnwrapper();
            var dummyRequest = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Create(dummyRequest, null));
            // Teardown
        }

        [Fact]
        public void CreateWithNonSeedWillReturnNull()
        {
            // Fixture setup
            var sut = new StringSeedUnwrapper();
            var nonSeed = new object();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
            var result = sut.Create(nonSeed, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen(nonSeed);
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWithNonStringRequestSeedWillReturnNull()
        {
            // Fixture setup
            var sut = new StringSeedUnwrapper();
            var nonStringRequestSeed = new SeededRequest(typeof(object), "Anonymous value");
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
            var result = sut.Create(nonStringRequestSeed, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen(nonStringRequestSeed);
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWithNonStringSeedWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new StringSeedUnwrapper();
            var nonStringSeed = new SeededRequest(typeof(string), new object());
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
            var result = sut.Create(nonStringSeed, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen(nonStringSeed);
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWithStringSeedWhenContainerCannotCreateStringsWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new StringSeedUnwrapper();
            var stringSeed = new SeededRequest(typeof(string), "Anonymous value");
            var unableContainer = new DelegatingSpecimenContainer { OnCreate = r => new NoSpecimen(stringSeed) };
            // Exercise system
            var result = sut.Create(stringSeed, unableContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen(stringSeed);
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWithStringSeedWhenContainerCanCreateStringsWillReturnCorrectResult()
        {
            // Fixture setup
            var seedString = Guid.NewGuid().ToString();
            var containerString = Guid.NewGuid().ToString();

            var sut = new StringSeedUnwrapper();
            var stringSeed = new SeededRequest(typeof(string), seedString);
            var container = new DelegatingSpecimenContainer { OnCreate = r => containerString };
            // Exercise system
            var result = sut.Create(stringSeed, container);
            // Verify outcome
            var expectedString = seedString + containerString;
            Assert.Equal(expectedString, result);
            // Teardown
        }

        [Fact]
        public void CreateWithStringSeedWillCorrectlyInvokeContainer()
        {
            // Fixture setup
            var sut = new StringSeedUnwrapper();
            var stringSeed = new SeededRequest(typeof(string), "Anonymous value");

            var mockVerified = false;
            var containerMock = new DelegatingSpecimenContainer();
            containerMock.OnCreate = r =>
                {
                    Assert.Equal(typeof(string), r);
                    mockVerified = true;
                    return null;
                };
            // Exercise system
            sut.Create(stringSeed, containerMock);
            // Verify outcome
            Assert.True(mockVerified, "Mock verification");
            // Teardown
        }
    }
}
