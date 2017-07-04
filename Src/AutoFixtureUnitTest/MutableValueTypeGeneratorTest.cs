using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class MutableValueTypeGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new MutableValueTypeGenerator();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new MutableValueTypeGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(null, dummyContainer);
            // Verify outcome
            Assert.Equal(new NoSpecimen(), result);
            // Teardown
        }

        [Fact]
        public void CreateWithNullContainerDoesNotThrow()
        {
            // Fixture setup
            var sut = new MutableValueTypeGenerator();
            // Exercise system
            var dummyRequest = new object();
            
            // Verify outcome (no exception indicates success)
            AssertEx.DoesNotThrow(() => sut.Create(dummyRequest, null));
            // Teardown
        }

        [Fact]
        public void CreateWithNonValueTypeRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var nonValueTypeRequest = typeof(NoSpecimen);
            var sut = new MutableValueTypeGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(nonValueTypeRequest, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWithNotTypeRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var nonValueTypeRequest = new object();
            var sut = new MutableValueTypeGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(nonValueTypeRequest, dummyContainer);
            // Verify outcome
            var expectedResult = new NoSpecimen();
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWithValueTypeWithoutConstructorRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var valueTypeRequest = typeof(MutableValueTypeWithoutConstructor);
            var sut = new MutableValueTypeGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(valueTypeRequest, dummyContainer);
            // Verify outcome
            Assert.IsType<MutableValueTypeWithoutConstructor>(result);
            // Teardown
        }

        [Fact]
        public void CreateWithValueTypeRequestWillReturnCorrectResult()
        {
            // Fixture setup
            var valueTypeRequest = typeof(MutableValueType);
            var sut = new MutableValueTypeGenerator();
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(valueTypeRequest, dummyContainer);
            // Verify outcome
            Assert.IsType<NoSpecimen>(result);

            // Teardown
        } 
    }
}
