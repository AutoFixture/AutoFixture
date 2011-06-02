using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Idioms;
using System.Reflection;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class ReflectionExceptionUnwrappingCommandTest
    {
        [Fact]
        public void SutIsContextualCommand()
        {
            // Fixture setup
            var dummyCommand = new DelegatingContextualCommand();
            // Exercise system
            var sut = new ReflectionExceptionUnwrappingCommand(dummyCommand);
            // Verify outcome
            Assert.IsAssignableFrom<IContextualCommand>(sut);
            // Teardown
        }

        [Fact]
        public void CommandIsCorrect()
        {
            // Fixture setup
            var expectedCommand = new DelegatingContextualCommand();
            var sut = new ReflectionExceptionUnwrappingCommand(expectedCommand);
            // Exercise system
            IContextualCommand result = sut.Command;
            // Verify outcome
            Assert.Equal(expectedCommand, result);
            // Teardown
        }

        [Fact]
        public void ExecuteExecutesDecoratedCommand()
        {
            // Fixture setup
            var mockVerified = false;
            var cmd = new DelegatingContextualCommand { OnExecute = () => mockVerified = true };
            var sut = new ReflectionExceptionUnwrappingCommand(cmd);
            // Exercise system
            sut.Execute();
            // Verify outcome
            Assert.True(mockVerified, "Mock verified.");
            // Teardown
        }

        [Fact]
        public void ExecuteRethrowsNormalException()
        {
            // Fixture setup
            var cmd = new DelegatingContextualCommand { OnExecute = () => { throw new InvalidOperationException(); } };
            var sut = new ReflectionExceptionUnwrappingCommand(cmd);
            // Exercise system and verify outcome
            Assert.Throws<InvalidOperationException>(() =>
                sut.Execute());
            // Teardown
        }

        [Fact]
        public void ExecuteUnwrapsAndThrowsInnerExceptionFromTargetInvocationException()
        {
            // Fixture setup
            var expectedException = new InvalidOperationException();
            var cmd = new DelegatingContextualCommand { OnExecute = () => { throw new TargetInvocationException(expectedException); } };
            var sut = new ReflectionExceptionUnwrappingCommand(cmd);
            // Exercise system and verify outcome
            var e = Assert.Throws<InvalidOperationException>(() =>
                sut.Execute());
            Assert.Equal(expectedException, e);
            // Teardown
        }
    }
}
