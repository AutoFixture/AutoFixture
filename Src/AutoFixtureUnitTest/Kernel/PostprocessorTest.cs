using System;
using System.Linq;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class PostprocessorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyCommand = new DelegatingSpecimenCommand();
            // Act
            var sut = new Postprocessor(dummyBuilder, dummyCommand);
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        [Obsolete]
        public void SutIsPostProcessor()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyCommand = new DelegatingSpecimenCommand();
            // Act
            var sut = new Postprocessor(dummyBuilder, dummyCommand);
            // Assert
            Assert.IsAssignableFrom<Postprocessor<object>>(sut);
        }

        [Fact]
        [Obsolete]
        public void SutCanBeInitializedWithDoubleAction()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            Action<object, ISpecimenContext> dummyAction = (s, c) => { };
            // Act & assert
            Assert.Null(Record.Exception(() => new Postprocessor(dummyBuilder, dummyAction)));
        }

        [Fact]
        [Obsolete]
        public void InitializeWithNullBuilderThrows()
        {
            // Arrange
            Action<object> dummyAction = s => { };
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => new Postprocessor(null, dummyAction));
        }

        [Fact]
        [Obsolete]
        public void InitializeWithNullActionThrows()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => new Postprocessor(dummyBuilder, (Action<object>)null));
        }

        [Fact]
        [Obsolete]
        public void InitializeWithNullSpecificationThrows()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            Action<object, ISpecimenContext> dummyAction = (s, c) => { };
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => new Postprocessor(dummyBuilder, dummyAction, null));
        }

        [Fact]
        [Obsolete]
        public void BuilderIsCorrect()
        {
            // Arrange
            var expectedBuilder = new DelegatingSpecimenBuilder();
            Action<object> dummyAction = s => { };

            var sut = new Postprocessor<object>(expectedBuilder, dummyAction);
            // Act
            ISpecimenBuilder result = sut.Builder;
            // Assert
            Assert.Equal(expectedBuilder, result);
        }

        [Fact]
        [Obsolete]
        public void ActionIsCorrect()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            Action<object, ISpecimenContext> expectedAction = (s, c) => { };

            var sut = new Postprocessor<object>(dummyBuilder, expectedAction);
            // Act
            Action<object, ISpecimenContext> result = sut.Action;
            // Assert
            Assert.Equal(expectedAction, result);
        }

        [Fact]
        [Obsolete]
        public void SpecificationIsCorrect()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            Action<object, ISpecimenContext> dummyAction = (s, c) => { };
            var expectedSpec = new DelegatingRequestSpecification();

            var sut = new Postprocessor<object>(dummyBuilder, dummyAction, expectedSpec);
            // Act
            IRequestSpecification result = sut.Specification;
            // Assert
            Assert.Equal(expectedSpec, result);
        }

        [Fact]
        public void BuilderIsCorrectWhenConstructedMinimallyNonGenerically()
        {
            // Arrange
            var expected = new DelegatingSpecimenBuilder();
            var dummyCommand = new DelegatingSpecimenCommand();

            var sut = new Postprocessor(expected, dummyCommand);
            // Act
            var actual = sut.Builder;
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CommandIsCorrectWhenConstructedMinimallyNonGenerically()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var expected = new DelegatingSpecimenCommand();

            var sut = new Postprocessor(dummyBuilder, expected);
            // Act
            var actual = sut.Command;
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BuilderIsCorrectWhenConstructedFullyNonGenerically()
        {
            // Arrange
            var expected = new DelegatingSpecimenBuilder();
            var dummyCommand = new DelegatingSpecimenCommand();
            var dummySpecification = new DelegatingRequestSpecification();

            var sut = new Postprocessor(expected, dummyCommand, dummySpecification);
            // Act
            var actual = sut.Builder;
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CommandIsCorrectWhenConstructedFullyNonGenerically()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var expected = new DelegatingSpecimenCommand();
            var dummySpecification = new DelegatingRequestSpecification();

            var sut = new Postprocessor(dummyBuilder, expected, dummySpecification);
            // Act
            var actual = sut.Command;
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SpecificationIsCorrectWhenConstructedFullyNonGenerically()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyCommand = new DelegatingSpecimenCommand();
            var expected = new DelegatingRequestSpecification();

            var sut = new Postprocessor(dummyBuilder, dummyCommand, expected);
            // Act
            var actual = sut.Specification;
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CreateInvokesDecoratedBuilderWithCorrectParameters()
        {
            // Arrange
            var expectedRequest = new object();
            var expectedContainer = new DelegatingSpecimenContext();

            var verified = false;
            var builderMock = new DelegatingSpecimenBuilder { OnCreate = (r, c) => verified = r == expectedRequest && c == expectedContainer };

            var dummyCommand = new DelegatingSpecimenCommand();
            var sut = new Postprocessor(builderMock, dummyCommand);
            // Act
            sut.Create(expectedRequest, expectedContainer);
            // Assert
            Assert.True(verified, "Mock verified");
        }

        [Fact]
        public void CreateReturnsCorrectResult()
        {
            // Arrange
            var expectedResult = new object();
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => expectedResult };
            var dummyCommand = new DelegatingSpecimenCommand();
            var sut = new Postprocessor(builder, dummyCommand);
            // Act
            var dummyRequest = new object();
            var dummyContainer = new DelegatingSpecimenContext();
            var result = sut.Create(dummyRequest, dummyContainer);
            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CreateInvokesActionWithCreatedSpecimen()
        {
            // Arrange
            var expectedSpecimen = new object();
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => expectedSpecimen };

            var verified = false;
            var mock = new DelegatingSpecimenCommand
            {
                OnExecute = (s, c) => verified = expectedSpecimen.Equals(s)
            };

            var sut = new Postprocessor(builder, mock);
            // Act
            var dummyRequest = new object();
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Create(dummyRequest, dummyContainer);
            // Assert
            Assert.True(verified, "Mock verified");
        }

        [Fact]
        public void ComposeReturnsCorrectResult()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyCommand = new DelegatingSpecimenCommand();
            var sut = new Postprocessor(dummyBuilder, dummyCommand);
            // Act
            var expectedBuilders = new[]
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            };
            var actual = sut.Compose(expectedBuilders);
            // Assert
            var pp = Assert.IsAssignableFrom<Postprocessor>(actual);
            var composite = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(pp.Builder);
            Assert.True(expectedBuilders.SequenceEqual(composite));
        }

        [Fact]
        public void ComposeSingleItemReturnsCorrectResult()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyCommand = new DelegatingSpecimenCommand();
            var sut = new Postprocessor(dummyBuilder, dummyCommand);
            // Act
            var expected = new DelegatingSpecimenBuilder();
            var actual = sut.Compose(new[] { expected });
            // Assert
            var pp = Assert.IsAssignableFrom<Postprocessor>(actual);
            Assert.Equal(expected, pp.Builder);
        }

        [Fact]
        [Obsolete]
        public void ComposePreservesAction()
        {
            // Arrange
            Action<object, ISpecimenContext> expected = (x, y) => { };
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new Postprocessor(dummyBuilder, expected);
            // Act
            var actual = sut.Compose(new ISpecimenBuilder[0]);
            // Assert
            var pp = Assert.IsAssignableFrom<Postprocessor>(actual);
            Assert.Equal(expected, pp.Action);
        }

        [Fact]
        public void ComposePreservesCommand()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var expected = new DelegatingSpecimenCommand();
            var sut = new Postprocessor(dummyBuilder, expected);
            // Act
            var actual = sut.Compose(new ISpecimenBuilder[0]);
            // Assert
            var pp = Assert.IsAssignableFrom<Postprocessor>(actual);
            Assert.Equal(expected, pp.Command);
        }

        [Fact]
        public void ComposePreservesSpecification()
        {
            // Arrange
            var expected = new DelegatingRequestSpecification();
            var dummyCommand = new DelegatingSpecimenCommand();
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new Postprocessor(dummyBuilder, dummyCommand, expected);
            // Act
            var actual = sut.Compose(new ISpecimenBuilder[0]);
            // Assert
            var pp = Assert.IsAssignableFrom<Postprocessor>(actual);
            Assert.Equal(expected, pp.Specification);
        }
    }
}
