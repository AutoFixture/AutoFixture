using System;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class EmailAddressLocalPartGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new EmailAddressLocalPartGenerator();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestReturnsCorrectResult()
        {
            // Arrange
            var sut = new EmailAddressLocalPartGenerator();
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(null, dummyContext);
            // Assert
            Assert.Equal(new NoSpecimen(), result);
        }

        [Fact]
        public void CreateWithNullContextThrows()
        {
            // Arrange
            var sut = new EmailAddressLocalPartGenerator();
            var dummyRequest = new object();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => sut.Create(dummyRequest, null));
        }

        [Fact]
        public void CreateWithNonEmailAddressLocalPartRequestReturnsCorrectResult()
        {
            // Arrange
            var sut = new EmailAddressLocalPartGenerator();
            var dummyRequest = new object();
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(dummyRequest, dummyContext);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateWhenLocalPartReceivedFromContextIsNullReturnsCorrectResult()
        {
            // Arrange
            var request = typeof(EmailAddressLocalPart);
            object expectedValue = null;
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r =>
                {
                    Assert.Equal(typeof(string), r);
                    return expectedValue;
                }
            };
            var sut = new EmailAddressLocalPartGenerator();
            // Act & assert
            var result = sut.Create(request, context);
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateReturnsEmailAddressLocalPartUsingLocalPartReceivedFromContext()
        {
            // Arrange
            var request = typeof(EmailAddressLocalPart);
            string expectedLocalPart = Guid.NewGuid().ToString();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r =>
                    {
                        Assert.Equal(typeof(string), r);
                        return expectedLocalPart;
                    }
            };
            var sut = new EmailAddressLocalPartGenerator();
            // Act
            var result = sut.Create(typeof(EmailAddressLocalPart), context) as EmailAddressLocalPart;
            // Assert
            Assert.Equal(expectedLocalPart, result.LocalPart);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void CreateReturnsNoSpecimenWhenContextCreatesInvalidLocalPartString(string invalidLocalPart)
        {
            // Arrange
            var request = typeof(EmailAddressLocalPart);

            var context = new DelegatingSpecimenContext
            {
                OnResolve = r =>
                {
                    Assert.Equal(typeof(string), r);
                    return invalidLocalPart;
                }
            };
            var sut = new EmailAddressLocalPartGenerator();
            // Act
            var result = sut.Create(typeof(EmailAddressLocalPart), context);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }
    }
}
