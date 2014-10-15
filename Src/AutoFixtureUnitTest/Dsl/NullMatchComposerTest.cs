using Ploeh.AutoFixture.Dsl;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Dsl
{
    public class NullMatchComposerTest
    {
        [Fact]
        public void SutIsMatchComposer()
        {
            // Fixture setup
            // Exercise system
            var sut = new NullMatchComposer<object>();
            // Verify outcome
            Assert.IsAssignableFrom<IMatchComposer<object>>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithSpecimenBuilderCreatesUsingThatSpecimenBuilder()
        {
            // Fixture setup
            var specimen = new object();
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (o, c) => specimen
            };
            var sut = new NullMatchComposer<object>(builder);
            // Exercise system
            var result = sut.Create(new object(), new DelegatingSpecimenContext());
            // Verify outcome
            Assert.Same(specimen, result);
            // Teardown
        }

        [Fact]
        public void InitializeWithDefaultConstructorUsesEmptyCompositeSpecimenBuilder()
        {
            // Fixture setup
            var sut = new NullMatchComposer<object>();
            // Exercise system
            var result = sut.Builder;
            // Verify outcome
            var composite = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(result);
            Assert.Empty(composite.Builders);
            // Teardown
        }

        [Fact]
        public void OrReturnsTheMatchComposerItself()
        {
            // Fixture setup
            var sut = new NullMatchComposer<object>();
            // Exercise system
            // Verify outcome
            Assert.Same(sut, sut.Or);
            // Teardown
        }

        [Fact]
        public void ByBaseTypeReturnsTheMatchComposerItself()
        {
            // Fixture setup
            var sut = new NullMatchComposer<object>();
            // Exercise system
            var result = sut.ByBaseType();
            // Verify outcome
            Assert.Same(sut, result);
            // Teardown
        }

        [Fact]
        public void ByInterfacesReturnsTheMatchComposerItself()
        {
            // Fixture setup
            var sut = new NullMatchComposer<object>();
            // Exercise system
            var result = sut.ByInterfaces();
            // Verify outcome
            Assert.Same(sut, result);
            // Teardown
        }

        [Fact]
        public void ByExactTypeReturnsTheMatchComposerItself()
        {
            // Fixture setup
            var sut = new NullMatchComposer<object>();
            // Exercise system
            var result = sut.ByExactType();
            // Verify outcome
            Assert.Same(sut, result);
            // Teardown
        }

        [Fact]
        public void ByParameterNameReturnsTheMatchComposerItself()
        {
            // Fixture setup
            var sut = new NullMatchComposer<object>();
            // Exercise system
            var result = sut.ByParameterName("parameter");
            // Verify outcome
            Assert.Same(sut, result);
            // Teardown
        }

        [Fact]
        public void ByPropertyNameReturnsTheMatchComposerItself()
        {
            // Fixture setup
            var sut = new NullMatchComposer<object>();
            // Exercise system
            var result = sut.ByPropertyName("Property");
            // Verify outcome
            Assert.Same(sut, result);
            // Teardown
        }

        [Fact]
        public void ByFieldNameReturnsTheMatchComposerItself()
        {
            // Fixture setup
            var sut = new NullMatchComposer<object>();
            // Exercise system
            var result = sut.ByFieldName("Field");
            // Verify outcome
            Assert.Same(sut, result);
            // Teardown
        }
    }
}
