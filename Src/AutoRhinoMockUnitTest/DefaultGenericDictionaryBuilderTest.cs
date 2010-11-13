using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoRhinoMock;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Rhino.Mocks;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.AutoRhinoMock.UnitTest
{
    public class DefaultGenericDictionaryBuilderTest
    {
        [Fact]
        public void SutImplementsISpecimenBuilder()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = fixture.CreateAnonymous<DefaultGenericDictionaryBuilder>();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(result);
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestThrows()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<DefaultGenericDictionaryBuilder>();
            // Exercise system & verify outcome
            Assert.Throws<ArgumentNullException>(() => sut.Create(null, MockRepository.GenerateMock<ISpecimenContext>()));
        }

        [Fact]
        public void CreateWithNullSpecimenContextThrows()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<DefaultGenericDictionaryBuilder>();
            // Exercise system & verify outcome
            Assert.Throws<ArgumentNullException>(() => sut.Create(new object(), null));
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(AbstractType))]
        [InlineData(typeof(ConcreteType))]
        [InlineData(typeof(IInterface))]
        public void CreateReturnsCorrectResultForNonGenericTypes(Type request)
        {
            // Fixture setup
            var fixture = new Fixture();
            var contextStub = MockRepository.GenerateMock<ISpecimenContext>();

            var sut = fixture.CreateAnonymous<DefaultGenericDictionaryBuilder>();
            // Exercise system 
            var result = sut.Create(request, contextStub);
            // Verify outcome
            Assert.IsType<NoSpecimen>(result);
        }

        [Fact]
        public void CreateReturnsCorrectResultForIDictionary()
        {
            // Fixture setup
            var fixture = new Fixture();
            var contextStub = MockRepository.GenerateMock<ISpecimenContext>();

            var sut = fixture.CreateAnonymous<DefaultGenericDictionaryBuilder>();
            // Exercise system 
            var result = sut.Create(typeof(IDictionary<int, object>), contextStub);

            // Verify outcome
            Assert.IsType<Dictionary<int, object>>(result);
        }

        [Fact]
        public void CreateReturnsCorrectResultForGenericTypeNotAssignableFromDictionary()
        {
            // Fixture setup
            var fixture = new Fixture();
            var contextStub = MockRepository.GenerateMock<ISpecimenContext>();

            var sut = fixture.CreateAnonymous<DefaultGenericDictionaryBuilder>();
            // Exercise system 
            var result = sut.Create(typeof(RhinoMockTestTypes.ConcreteGenericType<int>), contextStub);

            // Verify outcome
            Assert.IsType<NoSpecimen>(result);
        }
    }
}
