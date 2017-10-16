using System;
using AutoFixture;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class MutableValueTypeWarningThrowerTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new MutableValueTypeWarningThrower();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateThrowsCorrectException()
        {
            // Fixture setup
            var sut = new MutableValueTypeWarningThrower();
            var dummyRequest = new object();
            var dummyContext = new DelegatingSpecimenContext();
            // Exercise system and verify outcome
            Assert.Throws<ObjectCreationException>(() =>
                sut.Create(dummyRequest, dummyContext));
            // Teardown
        }

        [Fact]
        public void ExceptionContainsInformationAboutRequest()
        {
            // Fixture setup
            var sut = new MutableValueTypeWarningThrower();
            var request = Guid.NewGuid();
            var dummyContext = new DelegatingSpecimenContext();
            // Exercise system
            var e = Assert.Throws<ObjectCreationException>(() =>
                sut.Create(request, dummyContext));
            // Verify outcome
            Assert.Contains(request.ToString(), e.Message);
            // Teardown
        } 
    }
}