using System;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class SByteSequenceGeneratorTest
    {
        [Fact]
        [Obsolete]
        public void CreateAnonymousWillReturnOneOnFirstCall()
        {
            new LoopTest<SByteSequenceGenerator, sbyte>(sut => sut.CreateAnonymous()).Execute(1);
        }

        [Fact]
        [Obsolete]
        public void CreateAnonymousWillReturnTwoOnSecondCall()
        {
            new LoopTest<SByteSequenceGenerator, sbyte>(sut => sut.CreateAnonymous()).Execute(2);
        }

        [Fact]
        [Obsolete]
        public void CreateAnonymousWillReturnTenOnTenthCall()
        {
            new LoopTest<SByteSequenceGenerator, sbyte>(sut => sut.CreateAnonymous()).Execute(10);
        }

        [Fact]
        [Obsolete]
        public void CreateWillReturnOneOnFirstCall()
        {
            new LoopTest<SByteSequenceGenerator, sbyte>(sut => sut.Create()).Execute(1);
        }

        [Fact]
        [Obsolete]
        public void CreateWillReturnTwoOnSecondCall()
        {
            new LoopTest<SByteSequenceGenerator, sbyte>(sut => sut.Create()).Execute(2);
        }

        [Fact]
        [Obsolete]
        public void CreateWillReturnTenOnTenthCall()
        {
            new LoopTest<SByteSequenceGenerator, sbyte>(sut => sut.Create()).Execute(10);
        }

        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new SByteSequenceGenerator();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestWillReturnCorrectResult()
        {
            // Arrange
            var sut = new SByteSequenceGenerator();
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
            var sut = new SByteSequenceGenerator();
            // Act
            var dummyRequest = new object();
            sut.Create(dummyRequest, null);
            // Assert (no exception indicates success)
        }

        [Fact]
        public void CreateWithNonSByteRequestWillReturnCorrectResult()
        {
            // Arrange
            var nonSByteRequest = new object();
            var sut = new SByteSequenceGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(nonSByteRequest, dummyContainer);
            // Assert
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateWithSByteRequestWillReturnCorrectResult()
        {
            // Arrange
            var sbyteRequest = typeof(sbyte);
            var sut = new SByteSequenceGenerator();
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(sbyteRequest, dummyContainer);
            // Assert
            Assert.Equal((sbyte)1, result);
        }

        [Fact]
        public void CreateWithSByteRequestWillReturnCorrectResultOnSecondCall()
        {
            // Arrange
            var sbyteRequest = typeof(sbyte);
            var dummyContainer = new DelegatingSpecimenContext();
            var loopTest = new LoopTest<SByteSequenceGenerator, sbyte>(sut => (sbyte)sut.Create(sbyteRequest, dummyContainer));
            // Act & assert
            loopTest.Execute(2);
        }

        [Fact]
        public void CreateWithSByteRequestWillReturnCorrectResultOnTenthCall()
        {
            // Arrange
            var sbyteRequest = typeof(sbyte);
            var dummyContainer = new DelegatingSpecimenContext();
            var loopTest = new LoopTest<SByteSequenceGenerator, sbyte>(sut => (sbyte)sut.Create(sbyteRequest, dummyContainer));
            // Act & assert
            loopTest.Execute(10);
        }
    }
}
