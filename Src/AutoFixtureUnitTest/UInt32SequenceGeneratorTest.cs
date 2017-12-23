using System;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class UInt32SequenceGeneratorTest
    {
        [Fact]
        [Obsolete]
        public void CreateWillReturnOneOnFirstCall()
        {
            new LoopTest<UInt32SequenceGenerator, uint>(sut => sut.CreateAnonymous()).Execute(1);
        }

        [Fact]
        [Obsolete]
        public void CreateWillReturnTwoOnSecondCall()
        {
            new LoopTest<UInt32SequenceGenerator, uint>(sut => sut.CreateAnonymous()).Execute(2);
        }

        [Fact]
        [Obsolete]
        public void CreateWillReturnTenOnTenthCall()
        {
            new LoopTest<UInt32SequenceGenerator, uint>(sut => sut.CreateAnonymous()).Execute(10);
        }

        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new UInt32SequenceGenerator();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestWillReturnCorrectResult()
        {
            // Arrange
            var sut = new UInt32SequenceGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(null, dummyContainer);
            // Assert
            Assert.Equal(new NoSpecimen(), result);
        }

        [Fact]
        public void CreateWithNullContainerDoesNotThrow()
        {
            // Arrange
            var sut = new UInt32SequenceGenerator();
            // Act
            var dummyRequest = new object();
            sut.Create(dummyRequest, null);
            // Assert (no exception indicates success)
        }

        [Fact]
        public void CreateWithNonUInt32RequestWillReturnCorrectResult()
        {
            // Arrange
            var nonUInt32Request = new object();
            var sut = new UInt32SequenceGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(nonUInt32Request, dummyContainer);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateWithUInt32RequestWillReturnCorrectResult()
        {
            // Arrange
            var uInt32Request = typeof(uint);
            var sut = new UInt32SequenceGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(uInt32Request, dummyContainer);
            // Assert
            Assert.Equal(1u, result);
        }

        [Fact]
        public void CreateWithUInt32RequestWillReturnCorrectResultOnSecondCall()
        {
            // Arrange
            var uInt32Request = typeof(uint);
            var dummyContainer = new DelegatingSpecimenContext();
            var loopTest = new LoopTest<UInt32SequenceGenerator, uint>(sut => (uint)sut.Create(uInt32Request, dummyContainer));
            // Act & assert
            loopTest.Execute(2);
        }

        [Fact]
        public void CreateWithUInt32RequestWillReturnCorrectResultOnTenthCall()
        {
            // Arrange
            var uInt32Request = typeof(uint);
            var dummyContainer = new DelegatingSpecimenContext();
            var loopTest = new LoopTest<UInt32SequenceGenerator, uint>(sut => (uint)sut.Create(uInt32Request, dummyContainer));
            // Act & assert
            loopTest.Execute(10);
        }
    }
}
