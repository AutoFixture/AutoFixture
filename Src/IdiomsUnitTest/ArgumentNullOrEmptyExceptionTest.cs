using System;
using AutoFixture.Idioms;
using Xunit;

namespace AutoFixture.IdiomsUnitTest
{
    public class ArgumentNullOrEmptyExceptionTest
    {
        [Fact]
        public void SutIsArgumentException()
        {
            //Arrange
            string paramName;

            //Act
            var sut = new ArgumentNullOrEmptyException(nameof(paramName));

            //Assert
            Assert.IsAssignableFrom<ArgumentException>(sut);
        }

        [Fact]
        public void MessageIsNotNull()
        {
            //Arrange
            string paramName;

            //Act
            var sut = new ArgumentNullOrEmptyException(nameof(paramName));
            var result = sut.Message;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void MessageIsCorrect()
        {
            //Arrange
            string parmName;
            var expectedMessage = "Value should not be null or empty." + Environment.NewLine + "Parameter name: " + nameof(parmName);

            //Act
            var sut = new ArgumentNullOrEmptyException(nameof(parmName));

            //Assert
            Assert.Equal(expectedMessage, sut.Message);
        }
    }
}