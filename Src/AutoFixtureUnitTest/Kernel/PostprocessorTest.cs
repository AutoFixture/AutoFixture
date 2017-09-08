using System;
using System.Linq;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class PostprocessorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyCommand = new DelegatingSpecimenCommand();
            // Exercise system
            var sut = new Postprocessor(dummyBuilder, dummyCommand);
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void SutIsPostProcessor()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyCommand = new DelegatingSpecimenCommand();
            // Exercise system
            var sut = new Postprocessor(dummyBuilder, dummyCommand);
            // Verify outcome
            Assert.IsAssignableFrom<Postprocessor<object>>(sut);
            // Teardown
        }

#pragma warning disable 618
        [Fact]
        public void SutCanBeInitializedWithDoubleAction()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            Action<object, ISpecimenContext> dummyAction = (s, c) => { };
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() => new Postprocessor(dummyBuilder, dummyAction)));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullBuilderThrows()
        {
            // Fixture setup
            Action<object> dummyAction = s => { };
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new Postprocessor(null, dummyAction));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullActionThrows()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new Postprocessor(dummyBuilder, (Action<object>)null));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullSpecificationThrows()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            Action<object, ISpecimenContext> dummyAction = (s, c) => { };
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new Postprocessor(dummyBuilder, dummyAction, null));
            // Teardown
        }

        [Fact]
        public void BuilderIsCorrect()
        {
            // Fixture setup
            var expectedBuilder = new DelegatingSpecimenBuilder();
            Action<object> dummyAction = s => { };

            var sut = new Postprocessor<object>(expectedBuilder, dummyAction);
            // Exercise system
            ISpecimenBuilder result = sut.Builder;
            // Verify outcome
            Assert.Equal(expectedBuilder, result);
            // Teardown
        }

        [Fact]
        public void ActionIsCorrect()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            Action<object, ISpecimenContext> expectedAction = (s, c) => { };

            var sut = new Postprocessor<object>(dummyBuilder, expectedAction);
            // Exercise system
            Action<object, ISpecimenContext> result = sut.Action;
            // Verify outcome
            Assert.Equal(expectedAction, result);
            // Teardown
        }

        [Fact]
        public void SpecificationIsCorrect()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            Action<object, ISpecimenContext> dummyAction = (s, c) => { };
            var expectedSpec = new DelegatingRequestSpecification();

            var sut = new Postprocessor<object>(dummyBuilder, dummyAction, expectedSpec);
            // Exercise system
            IRequestSpecification result = sut.Specification;
            // Verify outcome
            Assert.Equal(expectedSpec, result);
            // Teardown
        }
#pragma warning restore 618

        [Fact]
        public void BuilderIsCorrectWhenConstructedMinimallyNonGenerically()
        {
            // Fixture setup
            var expected = new DelegatingSpecimenBuilder();
            var dummyCommand = new DelegatingSpecimenCommand();

            var sut = new Postprocessor(expected, dummyCommand);
            // Exercise system
            var actual = sut.Builder;
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void CommandIsCorrectWhenConstructedMinimallyNonGenerically()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var expected = new DelegatingSpecimenCommand();

            var sut = new Postprocessor(dummyBuilder, expected);
            // Exercise system
            var actual = sut.Command;
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void BuilderIsCorrectWhenConstructedFullyNonGenerically()
        {
            // Fixture setup
            var expected = new DelegatingSpecimenBuilder();
            var dummyCommand = new DelegatingSpecimenCommand();
            var dummySpecification = new DelegatingRequestSpecification();

            var sut = new Postprocessor(expected, dummyCommand, dummySpecification);
            // Exercise system
            var actual = sut.Builder;
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void CommandIsCorrectWhenConstructedFullyNonGenerically()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var expected = new DelegatingSpecimenCommand();
            var dummySpecification = new DelegatingRequestSpecification();

            var sut = new Postprocessor(dummyBuilder, expected, dummySpecification);
            // Exercise system
            var actual = sut.Command;
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void SpecificationIsCorrectWhenConstructedFullyNonGenerically()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyCommand = new DelegatingSpecimenCommand();
            var expected = new DelegatingRequestSpecification();

            var sut = new Postprocessor(dummyBuilder, dummyCommand, expected);
            // Exercise system
            var actual = sut.Specification;
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void CreateInvokesDecoratedBuilderWithCorrectParameters()
        {
            // Fixture setup
            var expectedRequest = new object();
            var expectedContainer = new DelegatingSpecimenContext();

            var verified = false;
            var builderMock = new DelegatingSpecimenBuilder { OnCreate = (r, c) => verified = r == expectedRequest && c == expectedContainer };

            var dummyCommand = new DelegatingSpecimenCommand();
            var sut = new Postprocessor(builderMock, dummyCommand);
            // Exercise system
            sut.Create(expectedRequest, expectedContainer);
            // Verify outcome
            Assert.True(verified, "Mock verified");
            // Teardown
        }

        [Fact]
        public void CreateReturnsCorrectResult()
        {
            // Fixture setup
            var expectedResult = new object();
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => expectedResult };
            var dummyCommand = new DelegatingSpecimenCommand();
            var sut = new Postprocessor(builder, dummyCommand);
            // Exercise system
            var dummyRequest = new object();
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(dummyRequest, dummyContainer);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateInvokesActionWithCreatedSpecimen()
        {
            // Fixture setup
            var expectedSpecimen = new object();
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => expectedSpecimen };

            var verified = false;
            var mock = new DelegatingSpecimenCommand
            {
                OnExecute = (s, c) => verified = expectedSpecimen.Equals(s)
            };

            var sut = new Postprocessor(builder, mock);
            // Exercise system
            var dummyRequest = new object();
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Create(dummyRequest, dummyContainer);
            // Verify outcome
            Assert.True(verified, "Mock verified");
            // Teardown
        }

        [Fact]
        public void ComposeReturnsCorrectResult()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyCommand = new DelegatingSpecimenCommand();
            var sut = new Postprocessor(dummyBuilder, dummyCommand);
            // Exercise system
            var expectedBuilders = new[]
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            };
            var actual = sut.Compose(expectedBuilders);
            // Verify outcome
            var pp = Assert.IsAssignableFrom<Postprocessor>(actual);
            var composite = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(pp.Builder);
            Assert.True(expectedBuilders.SequenceEqual(composite));
            // Teardown
        }

        [Fact]
        public void ComposeSingleItemReturnsCorrectResult()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyCommand = new DelegatingSpecimenCommand();
            var sut = new Postprocessor(dummyBuilder, dummyCommand);
            // Exercise system
            var expected = new DelegatingSpecimenBuilder();
            var actual = sut.Compose(new[] { expected });
            // Verify outcome
            var pp = Assert.IsAssignableFrom<Postprocessor>(actual);
            Assert.Equal(expected, pp.Builder);
            // Teardown
        }

#pragma warning disable 618
        [Fact]
        public void ComposePreservesAction()
        {
            // Fixture setup
            Action<object, ISpecimenContext> expected = (x, y) => { };
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new Postprocessor(dummyBuilder, expected);
            // Exercise system
            var actual = sut.Compose(new ISpecimenBuilder[0]);
            // Verify outcome
            var pp = Assert.IsAssignableFrom<Postprocessor>(actual);
            Assert.Equal(expected, pp.Action);
            // Teardown
        }
#pragma warning restore 618

        [Fact]
        public void ComposePreservesCommand()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var expected = new DelegatingSpecimenCommand();
            var sut = new Postprocessor(dummyBuilder, expected);
            // Exercise system
            var actual = sut.Compose(new ISpecimenBuilder[0]);
            // Verify outcome
            var pp = Assert.IsAssignableFrom<Postprocessor<object>>(actual);
            Assert.Equal(expected, pp.Command);
            // Teardown
        }

        [Fact]
        public void ComposePreservesSpecification()
        {
            // Fixture setup
            var expected = new DelegatingRequestSpecification();
            var dummyCommand = new DelegatingSpecimenCommand();
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new Postprocessor(dummyBuilder, dummyCommand, expected);
            // Exercise system
            var actual = sut.Compose(new ISpecimenBuilder[0]);
            // Verify outcome
            var pp = Assert.IsAssignableFrom<Postprocessor>(actual);
            Assert.Equal(expected, pp.Specification);
            // Teardown
        }
    }
}
