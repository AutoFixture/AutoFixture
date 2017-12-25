using System;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    [Obsolete]
    public class UnspecifiedSpecimenCommandTest
    {
        [Fact]
        public void SutIsSpecifiedSpecimenCommand()
        {
            // Arrange
            // Act
            var sut = new UnspecifiedSpecimenCommand<object>(obj => { });
            // Assert
            Assert.IsAssignableFrom<ISpecifiedSpecimenCommand<object>>(sut);
        }

        [Fact]
        public void InitializeWithNullActionThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new UnspecifiedSpecimenCommand<object>(null));
        }

        [Fact]
        public void ActionIsCorrect()
        {
            // Arrange
            Action<string> expectedAction = s => { };
            var sut = new UnspecifiedSpecimenCommand<string>(expectedAction);
            // Act
            Action<string> result = sut.Action;
            // Assert
            Assert.Equal(expectedAction, result);
        }

        [Fact]
        public void ExecuteCorrectlyInvokesAction()
        {
            // Arrange
            var specimen = new object();

            var verified = false;
            Action<object> spy = s => verified = specimen.Equals(s);

            var sut = new UnspecifiedSpecimenCommand<object>(spy);
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Execute(specimen, dummyContainer);
            // Assert
            Assert.True(verified);
        }

        [Fact]
        public void IsSatisfiedByReturnsCorrectResult()
        {
            // Arrange
            var sut = new UnspecifiedSpecimenCommand<object>(s => { });
            // Act
            var dummyRequest = new object();
            var result = sut.IsSatisfiedBy(dummyRequest);
            // Assert
            Assert.False(result);
        }
    }
}
