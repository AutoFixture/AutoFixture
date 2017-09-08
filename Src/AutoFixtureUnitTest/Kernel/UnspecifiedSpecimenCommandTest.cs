using System;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
#pragma warning disable 618
    public class UnspecifiedSpecimenCommandTest
    {
        [Fact]
        public void SutIsSpecifiedSpecimenCommand()
        {
            // Fixture setup
            // Exercise system
            var sut = new UnspecifiedSpecimenCommand<object>(obj => { });
            // Verify outcome
            Assert.IsAssignableFrom<ISpecifiedSpecimenCommand<object>>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullActionThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new UnspecifiedSpecimenCommand<object>(null));
            // Teardown
        }

        [Fact]
        public void ActionIsCorrect()
        {
            // Fixture setup
            Action<string> expectedAction = s => { };
            var sut = new UnspecifiedSpecimenCommand<string>(expectedAction);
            // Exercise system
            Action<string> result = sut.Action;
            // Verify outcome
            Assert.Equal(expectedAction, result);
            // Teardown
        }

        [Fact]
        public void ExecuteCorrectlyInvokesAction()
        {
            // Fixture setup
            var specimen = new object();

            var verified = false;
            Action<object> spy = s => verified = specimen.Equals(s);

            var sut = new UnspecifiedSpecimenCommand<object>(spy);
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Execute(specimen, dummyContainer);
            // Verify outcome
            Assert.True(verified);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new UnspecifiedSpecimenCommand<object>(s => { });
            // Exercise system
            var dummyRequest = new object();
            var result = sut.IsSatisfiedBy(dummyRequest);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }
    }
#pragma warning restore 618
}
