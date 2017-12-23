using System;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class UInt16SequenceGeneratorTest
    {
        [Fact]
        [Obsolete]
        public void CreateWillReturnOneOnFirstCall()
        {
            new LoopTest<UInt16SequenceGenerator, ushort>(sut => sut.CreateAnonymous()).Execute(1);
        }

        [Fact]
        [Obsolete]
        public void CreateWillReturnTwoOnSecondCall()
        {
            new LoopTest<UInt16SequenceGenerator, ushort>(sut => sut.CreateAnonymous()).Execute(2);
        }

        [Fact]
        [Obsolete]
        public void CreateWillReturnTenOnTenthCall()
        {
            new LoopTest<UInt16SequenceGenerator, ushort>(sut => sut.CreateAnonymous()).Execute(10);
        }

        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new UInt16SequenceGenerator();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestWillReturnCorrectResult()
        {
            // Arrange
            var sut = new UInt16SequenceGenerator();
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
            var sut = new UInt16SequenceGenerator();
            // Act
            var dummyRequest = new object();
            sut.Create(dummyRequest, null);
            // Assert (no exception indicates success)
        }

        [Fact]
        public void CreateWithNonUInt16RequestWillReturnCorrectResult()
        {
            // Arrange
            var nonUInt16Request = new object();
            var sut = new UInt16SequenceGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(nonUInt16Request, dummyContainer);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateWithUInt16RequestWillReturnCorrectResult()
        {
            // Arrange
            var uInt16Request = typeof(ushort);
            var sut = new UInt16SequenceGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(uInt16Request, dummyContainer);
            // Assert
            Assert.Equal((ushort)1, result);
        }

        [Fact]
        public void CreateWithUInt16RequestWillReturnCorrectResultOnSecondCall()
        {
            // Arrange
            var uInt16Request = typeof(ushort);
            var dummyContainer = new DelegatingSpecimenContext();
            var loopTest = new LoopTest<UInt16SequenceGenerator, ushort>(sut => (ushort)sut.Create(uInt16Request, dummyContainer));
            // Act & assert
            loopTest.Execute(2);
        }

        [Fact]
        public void CreateWithUInt16RequestWillReturnCorrectResultOnTenthCall()
        {
            // Arrange
            var uInt16Request = typeof(ushort);
            var dummyContainer = new DelegatingSpecimenContext();
            var loopTest = new LoopTest<UInt16SequenceGenerator, ushort>(sut => (ushort)sut.Create(uInt16Request, dummyContainer));
            // Act & assert
            loopTest.Execute(10);
        }
    }
}
