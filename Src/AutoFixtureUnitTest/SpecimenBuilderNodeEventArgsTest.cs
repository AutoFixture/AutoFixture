using System;
using AutoFixture;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class SpecimenBuilderNodeEventArgsTest
    {
        [Fact]
        public void SutIsEventArgs()
        {
            // Arrange
            var dummyNode = new CompositeSpecimenBuilder();
            // Act
            var sut = new SpecimenBuilderNodeEventArgs(dummyNode);
            // Assert
            Assert.IsAssignableFrom<EventArgs>(sut);
        }

        [Fact]
        public void GraphIsCorrect()
        {
            // Arrange
            var expected = new CompositeSpecimenBuilder();
            var sut = new SpecimenBuilderNodeEventArgs(expected);
            // Act
            ISpecimenBuilderNode actual = sut.Graph;
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConstructWithNullGraphThrows()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new SpecimenBuilderNodeEventArgs(null));
        }
    }
}
