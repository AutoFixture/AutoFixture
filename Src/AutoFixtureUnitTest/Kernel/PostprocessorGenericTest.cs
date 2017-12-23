using System;
using System.Linq;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    [Obsolete]
    public class PostprocessorGenericTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyCommand = new DelegatingSpecimenCommand();
            // Act
            var sut = new Postprocessor<string>(dummyBuilder, dummyCommand);
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void SutIsNode()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyCommand = new DelegatingSpecimenCommand();
            // Act
            var sut = new Postprocessor<Version>(dummyBuilder, dummyCommand);
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilderNode>(sut);
        }

        [Fact]
        [Obsolete]
        public void InitializeSingleActionWithNullBuilderThrows()
        {
            // Arrange
            Action<int> dummyAction = s => { };
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => new Postprocessor<int>(null, dummyAction));
        }

        [Fact]
        [Obsolete]
        public void InitializeSingleActionWithNullActionThrows()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => new Postprocessor<Guid>(dummyBuilder, (Action<Guid>)null));
        }

        [Fact]
        [Obsolete]
        public void InitializeDoubleActionWithNullBuilderThrows()
        {
            // Arrange
            Action<int, ISpecimenContext> dummyAction = (s, c) => { };
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => new Postprocessor<int>(null, dummyAction));
        }

        [Fact]
        [Obsolete]
        public void InitializeDoubleActionWithNullActionThrows()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => new Postprocessor<string>(dummyBuilder, (Action<string, ISpecimenContext>)null));
        }

        [Fact]
        [Obsolete]
        public void InitializeDoubleActionAndSpecificationWithNullBuilderThrows()
        {
            // Arrange
            Action<object, ISpecimenContext> dummyAction = (s, c) => { };
            var dummySpec = new DelegatingRequestSpecification();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => new Postprocessor<object>(null, dummyAction, dummySpec));
        }

        [Fact]
        [Obsolete]
        public void InitializeDoubleActionAndSpecificationWithNullActionThrows()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummySpec = new DelegatingRequestSpecification();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new Postprocessor<object>(
                    dummyBuilder,
                    (Action<object, ISpecimenContext>)null,
                    dummySpec));
        }

        [Fact]
        [Obsolete]
        public void InitializeDoubleActionAndSpecificationWithNullSpecificationThrows()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            Action<object, ISpecimenContext> dummyAction = (s, c) => { };
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => new Postprocessor<object>(dummyBuilder, dummyAction, null));
        }

        [Fact]
        public void SutYieldsInjectedBuilder()
        {
            // Arrange
            var expected = new DelegatingSpecimenBuilder();
            var dummyCommand = new DelegatingSpecimenCommand();
            var sut = new Postprocessor<object>(expected, dummyCommand);
            // Act
            // Assert
            Assert.Equal(expected, sut.Single());
            Assert.Equal(expected, ((System.Collections.IEnumerable)sut).Cast<object>().Single());
        }

        [Fact]
        public void ComposeReturnsCorrectResult()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyCommand = new DelegatingSpecimenCommand();
            var sut = new Postprocessor<object>(dummyBuilder, dummyCommand);
            // Act
            var expectedBuilders = new[]
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            };
            var actual = sut.Compose(expectedBuilders);
            // Assert
            var pp = Assert.IsAssignableFrom<Postprocessor<object>>(actual);
            var composite = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(pp.Builder);
            Assert.True(expectedBuilders.SequenceEqual(composite));
        }

        [Fact]
        public void ComposeSingleItemReturnsCorrectResult()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyCommand = new DelegatingSpecimenCommand();
            var sut = new Postprocessor<object>(dummyBuilder, dummyCommand);
            // Act
            var expected = new DelegatingSpecimenBuilder();
            var actual = sut.Compose(new[] { expected });
            // Assert
            var pp = Assert.IsAssignableFrom<Postprocessor<object>>(actual);
            Assert.Equal(expected, pp.Builder);
        }

        [Fact]
        [Obsolete]
        public void ComposePreservesAction()
        {
            // Arrange
            Action<Version, ISpecimenContext> expected = (x, y) => { };
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new Postprocessor<Version>(dummyBuilder, expected);
            // Act
            var actual = sut.Compose(new ISpecimenBuilder[0]);
            // Assert
            var pp = Assert.IsAssignableFrom<Postprocessor<Version>>(actual);
            Assert.Equal(expected, pp.Action);
        }

        [Fact]
        public void ComposePreservesCommand()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var expected = new DelegatingSpecimenCommand();
            var sut = new Postprocessor<object>(dummyBuilder, expected);
            // Act
            var actual = sut.Compose(new ISpecimenBuilder[0]);
            // Assert
            var pp = Assert.IsAssignableFrom<Postprocessor<object>>(actual);
            Assert.Equal(expected, pp.Command);
        }

        [Fact]
        public void ComposePreservesSpecification()
        {
            // Arrange
            var expected = new DelegatingRequestSpecification();
            var dummyCommand = new DelegatingSpecimenCommand();
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new Postprocessor<Version>(dummyBuilder, dummyCommand, expected);
            // Act
            var actual = sut.Compose(new ISpecimenBuilder[0]);
            // Assert
            var pp = Assert.IsAssignableFrom<Postprocessor<Version>>(actual);
            Assert.Equal(expected, pp.Specification);
        }

        [Fact]
        public void CreateInvokesDecoratedBuilderWithCorrectParametersOnSutWithCommand()
        {
            // Arrange
            var expectedRequest = new object();
            var expectedContainer = new DelegatingSpecimenContext();

            var verified = false;
            var builderMock = new DelegatingSpecimenBuilder { OnCreate = (r, c) => verified = r == expectedRequest && c == expectedContainer };

            var dummyCommand = new DelegatingSpecimenCommand();
            var sut = new Postprocessor<bool>(builderMock, dummyCommand);
            // Act
            sut.Create(expectedRequest, expectedContainer);
            // Assert
            Assert.True(verified, "Mock verified");
        }

        [Fact]
        public void CreateReturnsCorrectResultOnSutWithCommand()
        {
            // Arrange
            var expectedResult = 1m;
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => expectedResult };
            var dummyCommand = new DelegatingSpecimenCommand();
            var sut = new Postprocessor<decimal>(builder, dummyCommand);
            // Act
            var dummyRequest = new object();
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(dummyRequest, dummyContainer);
            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateThrowsWhenBuilderReturnsIncompatibleTypeOnSutWithCommand()
        {
            // Arrange
            var nonInt = "Anonymous variable";
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => nonInt };

            var dummyCommand = new DelegatingSpecimenCommand();
            var sut = new Postprocessor<int>(builder, dummyCommand);
            // Act & assert
            var dummyRequest = new object();
            var dummyContainer = new DelegatingSpecimenContext();
            Assert.Throws<InvalidOperationException>(() => sut.Create(dummyRequest, dummyContainer));
        }

        [Fact]
        public void CreateReturnsCorrectResultWhenBuilderReturnsNoSpecimen()
        {
            // Arrange
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => new NoSpecimen() };

            var dummyCommand = new DelegatingSpecimenCommand();
            var sut = new Postprocessor<int>(builder, dummyCommand);
            // Act
            var dummyRequest = new object();
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(dummyRequest, dummyContainer);
            // Assert
            Assert.IsAssignableFrom<NoSpecimen>(result);
        }

        [Fact]
        public void CreateInvokesActionWithCreatedSpecimenOnSutWithSingleAction()
        {
            // Arrange
            var expectedSpecimen = new DateTime(2010, 4, 26);
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => expectedSpecimen };

            var verified = false;
            var mock = new DelegatingSpecimenCommand
            {
                OnExecute = (s, c) => verified = expectedSpecimen.Equals(s)
            };

            var sut = new Postprocessor<DateTime>(builder, mock);
            // Act
            var dummyRequest = new object();
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Create(dummyRequest, dummyContainer);
            // Assert
            Assert.True(verified, "Mock verified");
        }

        [Fact]
        public void CreateInvokesDecoratedBuilderWithCorrectParametersOnSutWithDoubleAction()
        {
            // Arrange
            var expectedRequest = new object();
            var expectedContainer = new DelegatingSpecimenContext();

            var verified = false;
            var builderMock = new DelegatingSpecimenBuilder { OnCreate = (r, c) => verified = r == expectedRequest && c == expectedContainer };

            var dummyCommand = new DelegatingSpecimenCommand();
            var sut = new Postprocessor<bool>(builderMock, dummyCommand);
            // Act
            sut.Create(expectedRequest, expectedContainer);
            // Assert
            Assert.True(verified, "Mock verified");
        }

        [Fact]
        public void CreateReturnsCorrectResultOnSutWithDoubleAction()
        {
            // Arrange
            var expectedResult = 1m;
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => expectedResult };
            var dummyCommand = new DelegatingSpecimenCommand();
            var sut = new Postprocessor<decimal>(builder, dummyCommand);
            // Act
            var dummyRequest = new object();
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(dummyRequest, dummyContainer);
            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateThrowsWhenBuilderReturnsIncompatibleTypeOnSutWithDoubleAction()
        {
            // Arrange
            var nonInt = "Anonymous variable";
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => nonInt };

            var dummyCommand = new DelegatingSpecimenCommand();
            var sut = new Postprocessor<int>(builder, dummyCommand);
            // Act & assert
            var dummyRequest = new object();
            var dummyContainer = new DelegatingSpecimenContext();
            Assert.Throws<InvalidOperationException>(() => sut.Create(dummyRequest, dummyContainer));
        }

        [Fact]
        public void CreateInvokesActionWithCreatedSpecimenOnSutWithCommand()
        {
            // Arrange
            var expectedSpecimen = new DateTime(2010, 4, 26);
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => expectedSpecimen };

            var expectedContext = new DelegatingSpecimenContext();

            var verified = false;
            var mock = new DelegatingSpecimenCommand
            {
                OnExecute = (s, c) =>
                    verified = expectedSpecimen.Equals(s) && c == expectedContext
            };

            var sut = new Postprocessor<DateTime>(builder, mock);
            // Act
            var dummyRequest = new object();
            sut.Create(dummyRequest, expectedContext);
            // Assert
            Assert.True(verified, "Mock verified");
        }

        [Fact]
        public void CreateDoesNotInvokeActionWhenSpecificationIsFalse()
        {
            // Arrange
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => new object() };

            var verified = false;
            var mock = new DelegatingSpecimenCommand
            {
                OnExecute = (s, c) => verified = true
            };

            var spec = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => false };

            var sut = new Postprocessor<object>(builder, mock, spec);
            // Act
            var dummyRequest = new object();
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Create(dummyRequest, dummyContainer);
            // Assert
            Assert.False(verified, "Mock invoked");
        }

        [Fact]
        public void CommandIsCorrect()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var expected = new DelegatingSpecimenCommand();

            var sut = new Postprocessor<object>(dummyBuilder, expected);
            // Act
            ISpecimenCommand actual = sut.Command;
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BuilderIsCorrectWhenConstructedWithMinimalConstructor()
        {
            // Arrange
            var expected = new DelegatingSpecimenBuilder();
            var dummyCommand = new DelegatingSpecimenCommand();

            var sut = new Postprocessor<object>(expected, dummyCommand);
            // Act
            var actual = sut.Builder;
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        [Obsolete]
        public void ActionIsNotNullWhenConstructedWithMinimalConstructor()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyCommand = new DelegatingSpecimenCommand();

            var sut = new Postprocessor<object>(dummyBuilder, dummyCommand);
            // Act
            var actual = sut.Action;
            // Assert
            Assert.NotNull(actual);
        }

        [Fact]
        public void SpecificationIsCorrectWhenConstructedWithMinimalConstructor()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyCommand = new DelegatingSpecimenCommand();

            var sut = new Postprocessor<object>(dummyBuilder, dummyCommand);
            // Act
            var actual = sut.Specification;
            // Assert
            Assert.IsAssignableFrom<TrueRequestSpecification>(actual);
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
            // Arrange
            var expected = new DelegatingSpecimenBuilder();
            var dummyCommand = new DelegatingSpecimenCommand();
            var dummySpecification = new DelegatingRequestSpecification();

            var sut = new Postprocessor<object>(
                expected,
                dummyCommand,
                dummySpecification);
            // Act
            var actual = sut.Builder;
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CommandIsCorrectWhenConstructedWithFullConstructor()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var expected = new DelegatingSpecimenCommand();
            var dummySpecification = new DelegatingRequestSpecification();

            var sut = new Postprocessor<object>(
                dummyBuilder,
                expected,
                dummySpecification);
            // Act
            var actual = sut.Command;
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SpecificationIsCorrectWhenConstructedWithFullConstructor()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyCommand = new DelegatingSpecimenCommand();
            var expected = new DelegatingRequestSpecification();

            var sut = new Postprocessor<object>(
                dummyBuilder,
                dummyCommand,
                expected);
            // Act
            var actual = sut.Specification;
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        [Obsolete]
        public void ActionIsNotNullWhenConstructedWithFullConstructor()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyCommand = new DelegatingSpecimenCommand();
            var dummySpecification = new DelegatingRequestSpecification();

            var sut = new Postprocessor<object>(
                dummyBuilder,
                dummyCommand,
                dummySpecification);
            // Act
            var actual = sut.Action;
            // Assert
            Assert.NotNull(actual);
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
