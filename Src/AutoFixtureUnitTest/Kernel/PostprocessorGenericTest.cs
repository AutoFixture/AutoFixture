using System;
using System.Linq;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class PostprocessorGenericTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            Action<string> dummyAction = s => { };
            // Exercise system
            var sut = new Postprocessor<string>(dummyBuilder, dummyAction);
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void SutIsNode()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            Action<Version> dummyAction = s => { };
            // Exercise system
            var sut = new Postprocessor<Version>(dummyBuilder, dummyAction);
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilderNode>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeSingleActionWithNullBuilderThrows()
        {
            // Fixture setup
            Action<int> dummyAction = s => { };
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new Postprocessor<int>(null, dummyAction));
            // Teardown
        }

        [Fact]
        public void InitializeSingleActionWithNullActionThrows()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new Postprocessor<Guid>(dummyBuilder, (Action<Guid>)null));
            // Teardown
        }

        [Fact]
        public void InitializeDoubleActionWithNullBuilderThrows()
        {
            // Fixture setup
            Action<int, ISpecimenContext> dummyAction = (s, c) => { };
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new Postprocessor<int>(null, dummyAction));
            // Teardown
        }

        [Fact]
        public void InitializeDoubleActionWithNullActionThrows()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new Postprocessor<string>(dummyBuilder, (Action<string, ISpecimenContext>)null));
            // Teardown
        }

        [Fact]
        public void InitializeDoubleActionAndSpecificationWithNullBuilderThrows()
        {
            // Fixture setup
            Action<object, ISpecimenContext> dummyAction = (s, c) => { };
            var dummySpec = new DelegatingRequestSpecification();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new Postprocessor<object>(null, dummyAction, dummySpec));
            // Teardown
        }

        [Fact]
        public void InitializeDoubleActionAndSpecificationWithNullActionThrows()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummySpec = new DelegatingRequestSpecification();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => 
                new Postprocessor<object>(
                    dummyBuilder,
                    (Action<object, ISpecimenContext>)null,
                    dummySpec));
            // Teardown
        }

        [Fact]
        public void InitializeDoubleActionAndSpecificationWithNullSpecificationThrows()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            Action<object, ISpecimenContext> dummyAction = (s, c) => { };
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new Postprocessor<object>(dummyBuilder, dummyAction, null));
            // Teardown
        }

        [Fact]
        public void SutYieldsInjectedBuilder()
        {
            // Fixture setup
            var expected = new DelegatingSpecimenBuilder();
            var sut = new Postprocessor<object>(expected, _ => { });
            // Exercise system
            // Verify outcome
            Assert.Equal(expected, sut.Single());
            Assert.Equal(expected, ((System.Collections.IEnumerable)sut).Cast<object>().Single());
            // Teardown
        }

        [Fact]
        public void ComposeReturnsCorrectResult()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new Postprocessor<object>(dummyBuilder, _ => { });
            // Exercise system
            var expectedBuilders = new[]
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            };
            var actual = sut.Compose(expectedBuilders);
            // Verify outcome
            var pp = Assert.IsAssignableFrom<Postprocessor<object>>(actual);
            var composite = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(pp.Builder);
            Assert.True(expectedBuilders.SequenceEqual(composite));
            // Teardown
        }

        [Fact]
        public void ComposeSingleItemReturnsCorrectResult()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new Postprocessor<object>(dummyBuilder, _ => { });
            // Exercise system
            var expected = new DelegatingSpecimenBuilder();
            var actual = sut.Compose(new[] { expected });
            // Verify outcome
            var pp = Assert.IsAssignableFrom<Postprocessor<object>>(actual);
            Assert.Equal(expected, pp.Builder);
            // Teardown
        }

        [Fact]
        public void ComposePreservesAction()
        {
            // Fixture setup
            Action<Version, ISpecimenContext> expected = (x, y) => { };
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new Postprocessor<Version>(dummyBuilder, expected);
            // Exercise system
            var actual = sut.Compose(new ISpecimenBuilder[0]);
            // Verify outcome
            var pp = Assert.IsAssignableFrom<Postprocessor<Version>>(actual);
            Assert.Equal(expected, pp.Action);
            // Teardown
        }

        [Fact]
        public void ComposePreservesSpecification()
        {
            // Fixture setup
            var expected = new DelegatingRequestSpecification();
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new Postprocessor<Version>(dummyBuilder, (v, ctx) => { }, expected);
            // Exercise system
            var actual = sut.Compose(new ISpecimenBuilder[0]);
            // Verify outcome
            var pp = Assert.IsAssignableFrom<Postprocessor<Version>>(actual);
            Assert.Equal(expected, pp.Specification);
            // Teardown
        }

        [Fact]
        public void CreateInvokesDecoratedBuilderWithCorrectParametersOnSutWithSingleAction()
        {
            // Fixture setup
            var expectedRequest = new object();
            var expectedContainer = new DelegatingSpecimenContext();

            var verified = false;
            var builderMock = new DelegatingSpecimenBuilder { OnCreate = (r, c) => verified = r == expectedRequest && c == expectedContainer };

            Action<bool> dummyAction = s => { };
            var sut = new Postprocessor<bool>(builderMock, dummyAction);
            // Exercise system
            sut.Create(expectedRequest, expectedContainer);
            // Verify outcome
            Assert.True(verified, "Mock verified");
            // Teardown
        }

        [Fact]
        public void CreateReturnsCorrectResultOnSutWithSingleAction()
        {
            // Fixture setup
            var expectedResult = 1m;
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => expectedResult };
            Action<decimal> dummyAction = s => { };
            var sut = new Postprocessor<decimal>(builder, dummyAction);
            // Exercise system
            var dummyRequest = new object();
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(dummyRequest, dummyContainer);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateThrowsWhenBuilderReturnsIncompatibleTypeOnSutWithSingleAction()
        {
            // Fixture setup
            var nonInt = "Anonymous variable";
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => nonInt };

            Action<int> dummyAction = s => { };
            var sut = new Postprocessor<int>(builder, dummyAction);
            // Exercise system and verify outcome
            var dummyRequest = new object();
            var dummyContainer = new DelegatingSpecimenContext();
            Assert.Throws<InvalidOperationException>(() => sut.Create(dummyRequest, dummyContainer));
            // Teardown
        }

        [Fact]
        public void CreateReturnsCorrectResultWhenBuilderReturnsNoSpecimen()
        {
            // Fixture setup
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => new NoSpecimen() };

            Action<int> dummyAction = s => { };
            var sut = new Postprocessor<int>(builder, dummyAction);
            // Exercise system
            var dummyRequest = new object();
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(dummyRequest, dummyContainer);
            // Verify outcome
            Assert.IsAssignableFrom<NoSpecimen>(result);
            // Teardown
        }

        [Fact]
        public void CreateInvokesActionWithCreatedSpecimenOnSutWithSingleAction()
        {
            // Fixture setup
            var expectedSpecimen = new DateTime(2010, 4, 26);
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => expectedSpecimen };

            var verified = false;
            Action<DateTime> mock = s => verified = s == expectedSpecimen;

            var sut = new Postprocessor<DateTime>(builder, mock);
            // Exercise system
            var dummyRequest = new object();
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Create(dummyRequest, dummyContainer);
            // Verify outcome
            Assert.True(verified, "Mock verified");
            // Teardown
        }

        [Fact]
        public void CreateInvokesDecoratedBuilderWithCorrectParametersOnSutWithDoubleAction()
        {
            // Fixture setup
            var expectedRequest = new object();
            var expectedContainer = new DelegatingSpecimenContext();

            var verified = false;
            var builderMock = new DelegatingSpecimenBuilder { OnCreate = (r, c) => verified = r == expectedRequest && c == expectedContainer };

            Action<bool, ISpecimenContext> dummyAction = (s, c) => { };
            var sut = new Postprocessor<bool>(builderMock, dummyAction);
            // Exercise system
            sut.Create(expectedRequest, expectedContainer);
            // Verify outcome
            Assert.True(verified, "Mock verified");
            // Teardown
        }

        [Fact]
        public void CreateReturnsCorrectResultOnSutWithDoubleAction()
        {
            // Fixture setup
            var expectedResult = 1m;
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => expectedResult };
            Action<decimal, ISpecimenContext> dummyAction = (s, c) => { };
            var sut = new Postprocessor<decimal>(builder, dummyAction);
            // Exercise system
            var dummyRequest = new object();
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(dummyRequest, dummyContainer);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateThrowsWhenBuilderReturnsIncompatibleTypeOnSutWithDoubleAction()
        {
            // Fixture setup
            var nonInt = "Anonymous variable";
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => nonInt };

            Action<int, ISpecimenContext> dummyAction = (s, c) => { };
            var sut = new Postprocessor<int>(builder, dummyAction);
            // Exercise system and verify outcome
            var dummyRequest = new object();
            var dummyContainer = new DelegatingSpecimenContext();
            Assert.Throws<InvalidOperationException>(() => sut.Create(dummyRequest, dummyContainer));
            // Teardown
        }

        [Fact]
        public void CreateInvokesActionWithCreatedSpecimenOnSutWithDoubleAction()
        {
            // Fixture setup
            var expectedSpecimen = new DateTime(2010, 4, 26);
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => expectedSpecimen };

            var expectedContainer = new DelegatingSpecimenContext();

            var verified = false;
            Action<DateTime, ISpecimenContext> mock = (s, c) => verified = s == expectedSpecimen && c == expectedContainer;

            var sut = new Postprocessor<DateTime>(builder, mock);
            // Exercise system
            var dummyRequest = new object();            
            sut.Create(dummyRequest, expectedContainer);
            // Verify outcome
            Assert.True(verified, "Mock verified");
            // Teardown
        }

        [Fact]
        public void CreateDoesNotInvokeActionWhenSpecificationIsFalse()
        {
            // Fixture setup
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => new object() };

            var mockInvoked = false;
            Action<object, ISpecimenContext> mock = (s, c) => mockInvoked = true;

            var spec = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => false };

            var sut = new Postprocessor<object>(builder, mock, spec);
            // Exercise system
            var dummyRequest = new object();
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Create(dummyRequest, dummyContainer);
            // Verify outcome
            Assert.False(mockInvoked, "Mock invoked");
            // Teardown
        }

        [Fact]
        public void CommandIsCorrect()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var expected = new DelegatingSpecimenCommand();

            var sut = new Postprocessor<object>(dummyBuilder, expected);
            // Exercise system
            ISpecimenCommand actual = sut.Command;
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void BuilderIsCorrectWhenConstructedWithMinimalConstructor()
        {
            // Fixture setup
            var expected = new DelegatingSpecimenBuilder();
            var dummyCommand = new DelegatingSpecimenCommand();

            var sut = new Postprocessor<object>(expected, dummyCommand);
            // Exercise system
            var actual = sut.Builder;
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void ActionIsNotNullWhenConstructedWithMinimalConstructor()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyCommand = new DelegatingSpecimenCommand();

            var sut = new Postprocessor<object>(dummyBuilder, dummyCommand);
            // Exercise system
            var actual = sut.Action;
            // Verify outcome
            Assert.NotNull(actual);
            // Teardown
        }

        [Fact]
        public void SpecificationIsCorrectWhenConstructedWithMinimalConstructor()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyCommand = new DelegatingSpecimenCommand();

            var sut = new Postprocessor<object>(dummyBuilder, dummyCommand);
            // Exercise system
            var actual = sut.Specification;
            // Verify outcome
            Assert.IsAssignableFrom<TrueRequestSpecification>(actual);
            // Teardown
        }

        [Fact]
        public void ConstructMinimalWithNullBuilderThrows()
        {
            var dummyCommand = new DelegatingSpecimenCommand();
            Assert.Throws<ArgumentNullException>(() =>
                new Postprocessor<object>(null, dummyCommand));
        }

        [Fact]
        public void ConstructMinimalWithNullCommandThrows()
        {
            var dummyBuilder = new DelegatingSpecimenBuilder();
            Assert.Throws<ArgumentNullException>(() =>
                new Postprocessor<object>(dummyBuilder, (ISpecimenCommand)null));
        }

        [Fact]
        public void BuilderIsCorrectWhenConstructedWithFullConstructor()
        {
            // Fixture setup
            var expected = new DelegatingSpecimenBuilder();
            var dummyCommand = new DelegatingSpecimenCommand();
            var dummySpecification = new DelegatingRequestSpecification();

            var sut = new Postprocessor<object>(
                expected,
                dummyCommand,
                dummySpecification);
            // Exercise system
            var actual = sut.Builder;
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void CommandIsCorrectWhenConstructedWithFullConstructor()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var expected = new DelegatingSpecimenCommand();
            var dummySpecification = new DelegatingRequestSpecification();

            var sut = new Postprocessor<object>(
                dummyBuilder,
                expected,
                dummySpecification);
            // Exercise system
            var actual = sut.Command;
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void SpecificationIsCorrectWhenConstructedWithFullConstructor()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyCommand = new DelegatingSpecimenCommand();
            var expected = new DelegatingRequestSpecification();

            var sut = new Postprocessor<object>(
                dummyBuilder,
                dummyCommand,
                expected);
            // Exercise system
            var actual = sut.Specification;
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void ActionIsNotNullWhenConstructedWithFullConstructor()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyCommand = new DelegatingSpecimenCommand();
            var dummySpecification = new DelegatingRequestSpecification();

            var sut = new Postprocessor<object>(
                dummyBuilder,
                dummyCommand,
                dummySpecification);
            // Exercise system
            var actual = sut.Action;
            // Verify outcome
            Assert.NotNull(actual);
            // Teardown
        }

        [Fact]
        public void ConstructFullWithNullBuilderThrows()
        {
            var dummyCommand = new DelegatingSpecimenCommand();
            var dummySpecification = new DelegatingRequestSpecification();
            Assert.Throws<ArgumentNullException>(() =>
                new Postprocessor<object>(
                    null,
                    dummyCommand,
                    dummySpecification));
        }

        [Fact]
        public void ConstructFullWithNullCommandThrows()
        {
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummySpecification = new DelegatingRequestSpecification();
            Assert.Throws<ArgumentNullException>(() =>
                new Postprocessor<object>(
                    dummyBuilder,
                    (ISpecimenCommand)null,
                    dummySpecification));
        }

        [Fact]
        public void ConstructFullWithNullSpecificationThrows()
        {
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyCommand = new DelegatingSpecimenCommand();
            Assert.Throws<ArgumentNullException>(() =>
                new Postprocessor<object>(
                    dummyBuilder,
                    dummyCommand,
                    null));
        }
    }
}
