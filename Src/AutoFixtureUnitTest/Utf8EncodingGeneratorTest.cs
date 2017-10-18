using System;
using System.Text;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
        [Obsolete]
    public class Utf8EncodingGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
#pragma warning disable 618
            var sut = new Utf8EncodingGenerator();
#pragma warning restore 618
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithEncodingRequestWillReturnUtf8Encoding()
        {
            // Arrange
#pragma warning disable 618
            var sut = new Utf8EncodingGenerator();
#pragma warning restore 618

            // Act
            var result = sut.Create(typeof(Encoding), new DelegatingSpecimenContext());

            // Assert
            Assert.Equal(Encoding.UTF8, result);
        }

        [Fact]
        public void CreateWithNullRequestWillReturnNoSpecimen()
        {
            // Arrange
#pragma warning disable 618
            var sut = new Utf8EncodingGenerator();
#pragma warning restore 618

            // Act
            var result = sut.Create(null, new DelegatingSpecimenContext());

            // Assert
            Assert.Equal(new NoSpecimen(), result);
        }

        [Fact]
        public void CreateWithNonTypeRequestWillReturnNoSpecimen()
        {
            // Arrange
#pragma warning disable 618
            var sut = new Utf8EncodingGenerator();
#pragma warning restore 618

            // Act
            var result = sut.Create(new object(), new DelegatingSpecimenContext());
            // Assert
            Assert.Equal(new NoSpecimen(), result);
        }
    }
}
