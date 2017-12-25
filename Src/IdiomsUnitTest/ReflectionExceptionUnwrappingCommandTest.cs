using System;
using System.Reflection;
using AutoFixture.Idioms;
using Xunit;

namespace AutoFixture.IdiomsUnitTest
{
    public class ReflectionExceptionUnwrappingCommandTest
    {
        [Fact]
        public void SutIsContextualCommand()
        {
            // Arrange
            var dummyCommand = new DelegatingGuardClauseCommand();
            // Act
            var sut = new ReflectionExceptionUnwrappingCommand(dummyCommand);
            // Assert
            Assert.IsAssignableFrom<IGuardClauseCommand>(sut);
        }

        [Fact]
        public void CommandIsCorrect()
        {
            // Arrange
            var expectedCommand = new DelegatingGuardClauseCommand();
            var sut = new ReflectionExceptionUnwrappingCommand(expectedCommand);
            // Act
            IGuardClauseCommand result = sut.Command;
            // Assert
            Assert.Equal(expectedCommand, result);
        }
        
        [Fact]
        public void RequestedParamNameIsCorrect()
        {
            const string expected = "foo";
            var commandStub = new DelegatingGuardClauseCommand { RequestedParameterName = expected };
            var sut = new ReflectionExceptionUnwrappingCommand(commandStub);

            var actual = sut.RequestedParameterName;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ExecuteExecutesDecoratedCommand()
        {
            // Arrange
            var mockVerified = false;
            var expectedValue = new object();
            var cmd = new DelegatingGuardClauseCommand { OnExecute = v => mockVerified = expectedValue.Equals(v) };
            var sut = new ReflectionExceptionUnwrappingCommand(cmd);
            // Act
            sut.Execute(expectedValue);
            // Assert
            Assert.True(mockVerified, "Mock verified.");
        }

        [Fact]
        public void ExecuteRethrowsNormalException()
        {
            // Arrange
            var cmd = new DelegatingGuardClauseCommand { OnExecute = v => { throw new InvalidOperationException(); } };
            var sut = new ReflectionExceptionUnwrappingCommand(cmd);
            // Act & Assert
            var dummyValue = new object();
            Assert.Throws<InvalidOperationException>(() =>
                sut.Execute(dummyValue));
        }

        [Fact]
        public void ExecuteUnwrapsAndThrowsInnerExceptionFromTargetInvocationException()
        {
            // Arrange
            var expectedException = new InvalidOperationException();
            var cmd = new DelegatingGuardClauseCommand { OnExecute = v => { throw new TargetInvocationException(expectedException); } };
            var sut = new ReflectionExceptionUnwrappingCommand(cmd);
            // Act & Assert
            var dummyValue = new object();
            var e = Assert.Throws<InvalidOperationException>(() =>
                sut.Execute(dummyValue));
            Assert.Equal(expectedException, e);
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(int))]
        [InlineData(typeof(string))]
        [InlineData(typeof(Version))]
        public void ContextTypeIsCorrect(Type type)
        {
            // Arrange
            var cmd = new DelegatingGuardClauseCommand { RequestedType = type };
            var sut = new ReflectionExceptionUnwrappingCommand(cmd);
            // Act
            var result = sut.RequestedType;
            // Assert
            Assert.Equal(type, result);
        }

        [Fact]
        public void CreateExceptionReturnsCorrectResult()
        {
            // Arrange
            var value = Guid.NewGuid().ToString();
            var expected = new Exception();
            var cmd = new DelegatingGuardClauseCommand { OnCreateException = v => value == v ? expected : new Exception() };
            var sut = new ReflectionExceptionUnwrappingCommand(cmd);
            // Act
            var result = sut.CreateException(value);
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CreateExceptionWithInnerReturnsCorrectResult()
        {
            // Arrange
            var value = Guid.NewGuid().ToString();
            var inner = new Exception();
            var expected = new Exception();
            var cmd = new DelegatingGuardClauseCommand { OnCreateExceptionWithInner = (v, e) => value == v && inner.Equals(e) ? expected : new Exception() };
            var sut = new ReflectionExceptionUnwrappingCommand(cmd);
            // Act
            var result = sut.CreateException(value, inner);
            // Assert
            Assert.Equal(expected, result);
        }
    }
}