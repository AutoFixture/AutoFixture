using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class Utf8EncodingGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            var sut = new Utf8EncodingGenerator();
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithEncodingRequestWillReturnUtf8Encoding()
        {
            // Arrange
            var sut = new Utf8EncodingGenerator();

            // Act
            var result = sut.Create(typeof(Encoding), new DelegatingSpecimenContext());

            // Assert
            Assert.Equal(Encoding.UTF8, result);
        }

        [Fact]
        public void CreateWithNullRequestWillReturnNoSpecimen()
        {
            // Arrange
            var sut = new Utf8EncodingGenerator();

            // Act
            var result = sut.Create(null, new DelegatingSpecimenContext());

            // Assert
            Assert.Equal(new NoSpecimen(), result);
        }

        [Fact]
        public void CreateWithNullContextDoesNotThrow()
        {
            // Arrange
            var sut = new Utf8EncodingGenerator();

            // Act
            Assert.DoesNotThrow(() => sut.Create(typeof(object), null));
        }

        [Fact]
        public void CreateWithNonTypeRequestWillReturnNoSpecimen()
        {
            // Arrange
            var sut = new Utf8EncodingGenerator();

            // Act
            var result = sut.Create(new object(), new DelegatingSpecimenContext());
            // Assert
            Assert.Equal(new NoSpecimen(), result);
        }
    }
}
