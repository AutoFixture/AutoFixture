using System;
using AutoFixture;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class ReadonlyCollectionPropertiesCommandTest
    {
        [Fact]
        public void SutIsCommand()
        {
            // Arrange
            // Act
            var sut = new ReadonlyCollectionPropertiesCommand();

            // Assert
            Assert.IsAssignableFrom<ISpecimenCommand>(sut);
        }

        [Fact]
        public void ExecuteNullSpecimenThrows()
        {
            // Arrange
            var sut = new ReadonlyCollectionPropertiesCommand();
            var dummyContainer = new DelegatingSpecimenContext();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => sut.Execute(null, dummyContainer));
        }

        [Fact]
        public void ExecuteNullContextThrows()
        {
            // Arrange
            var sut = new ReadonlyCollectionPropertiesCommand();
            var dummySpecimen = new object();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => sut.Execute(dummySpecimen, null));
        }

        [Fact]
        public void ExecuteFillsReadonlyCollectionProperty()
        {
            // Arrange
            var sut = new ReadonlyCollectionPropertiesCommand();
            var specimen = new CollectionHolder<string>();
            var container = new DelegatingSpecimenContext
            {
                OnResolve = r => new Fixture().CreateMany<string>()
            };

            // Act
            sut.Execute(specimen, container);

            // Assert
            Assert.NotEmpty(specimen.Collection);
        }

        [Fact]
        public void ExecuteDoesNotFillNonCompliantCollectionProperty()
        {
            // Arrange
            var sut = new ReadonlyCollectionPropertiesCommand();
            var specimen = new NonCompliantCollectionHolder<string>();
            var container = new DelegatingSpecimenContext
            {
                OnResolve = r => new Fixture().CreateMany<string>()
            };

            // Act
            sut.Execute(specimen, container);

            // Assert
            Assert.Empty(specimen.Collection);
        }
    }
}