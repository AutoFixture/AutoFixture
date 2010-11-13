using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixture.AutoRhinoMock;
using Rhino.Mocks;
using Xunit.Extensions;
using Ploeh.TestTypeFoundation;

namespace Ploeh.AutoFixture.AutoRhinoMock.UnitTest
{
    public class DefaultGenericListBuilderTest
    {
        [Fact]
        public void SutImplementsISpecimenBuilder()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = fixture.CreateAnonymous<DefaultGenericListBuilder>();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(result);
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestThrows()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<DefaultGenericListBuilder>();
            // Exercise system & verify outcome
            Assert.Throws<ArgumentNullException>(() => sut.Create(null, MockRepository.GenerateMock<ISpecimenContext>()));
        }

        [Fact]
        public void CreateWithNullSpecimenContextThrows()
        {
            // Fixture setup
            var fixture = new Fixture();

            var sut = fixture.CreateAnonymous<DefaultGenericListBuilder>();
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

            var sut = fixture.CreateAnonymous<DefaultGenericListBuilder>();
            // Exercise system 
            var result = sut.Create(request, contextStub);
            // Verify outcome
            Assert.IsType<NoSpecimen>(result);
        }

        [Fact]
        public void CreateReturnsCorrectResultForIEnumerable()
        {
            // Fixture setup
            var fixture = new Fixture();
            var contextStub = MockRepository.GenerateMock<ISpecimenContext>();

            var sut = fixture.CreateAnonymous<DefaultGenericListBuilder>();
            // Exercise system 
            var result = sut.Create(typeof(IEnumerable<int>), contextStub);

            // Verify outcome
            Assert.IsType<List<int>>(result);
        }

        [Fact]
        public void CreateReturnsCorrectResultForIList()
        {
            // Fixture setup
            var fixture = new Fixture();
            var contextStub = MockRepository.GenerateMock<ISpecimenContext>();

            var sut = fixture.CreateAnonymous<DefaultGenericListBuilder>();
            // Exercise system 
            var result = sut.Create(typeof(IList<int>), contextStub);

            // Verify outcome
            Assert.IsType<List<int>>(result);
        }

        [Fact]
        public void CreateReturnsCorrectResultForGenericTypeNotAssignableFromList()
        {
            // Fixture setup
            var fixture = new Fixture();
            var contextStub = MockRepository.GenerateMock<ISpecimenContext>();

            var sut = fixture.CreateAnonymous<DefaultGenericListBuilder>();
            // Exercise system 
            var result = sut.Create(typeof(RhinoMockTestTypes.ConcreteGenericType<int>), contextStub);

            // Verify outcome
            Assert.IsType<NoSpecimen>(result);
        }
    }
}
