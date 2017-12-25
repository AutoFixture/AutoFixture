using System;
using System.Collections.Generic;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class CompositeSpecimenCommandTest
    {
        [Fact]
        public void ExecutesEveryChildCommandWithCorrectContext()
        {
            // Arrange
            var specimen = new object();
            var dummyContext = new DelegatingSpecimenContext();

            var command1Verified = false;
            var command1 = new DelegatingSpecimenCommand
            {
                OnExecute = (req, ctx) =>
                {
                    command1Verified = true;
                    Assert.Same(dummyContext, ctx);
                }
            };

            var command2Verified = false;
            var command2 = new DelegatingSpecimenCommand
            {
                OnExecute = (req, ctx) =>
                {
                    command2Verified = true;
                    Assert.Same(dummyContext, ctx);
                }
            };

            var sut = new CompositeSpecimenCommand(command1, command2);
            // Act
            sut.Execute(specimen, dummyContext);
            // Assert
            Assert.True(command1Verified);
            Assert.True(command2Verified);
        }

        [Fact]
        public void ExecutesEveryChildCommandWithCorrectSpecimen()
        {
            // Arrange
            var specimen = new object();
            var dummyContext = new DelegatingSpecimenContext();

            var command1Verified = false;
            var command1 = new DelegatingSpecimenCommand
            {
                OnExecute = (req, ctx) =>
                    {
                        command1Verified = true;
                        Assert.Same(specimen, req);
                    }
            };

            var command2Verified = false;
            var command2 = new DelegatingSpecimenCommand
            {
                OnExecute = (req, ctx) =>
                    {
                        command2Verified = true;
                        Assert.Same(specimen, req);
                    }
            };

            var sut = new CompositeSpecimenCommand(command1, command2);
            // Act
            sut.Execute(specimen, dummyContext);
            // Assert
            Assert.True(command1Verified);
            Assert.True(command2Verified);
        }

        [Fact]
        public void CtorThrowsWhenCommandsIsNull()
        {
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => new CompositeSpecimenCommand(null as ISpecimenCommand[]));
            Assert.Throws<ArgumentNullException>(() => new CompositeSpecimenCommand(null as IEnumerable<ISpecimenCommand>));
        }

        [Fact]
        public void CommandsPropertyIsWiredUpThroughConstructor()
        {
            // Arrange
            ISpecimenCommand[] expectedCommands = { new DelegatingSpecimenCommand() };
            var sut = new CompositeSpecimenCommand(expectedCommands);
            // Act
            var commands = sut.Commands;
            // Assert
            Assert.Same(expectedCommands, commands);
        }

        [Fact]
        public void CommandsIsNotNullWhenSutIsCreatedWithDefaultConstructor()
        {
            // Arrange
            var sut = new CompositeSpecimenCommand();
            // Act
            var commands = sut.Commands;
            // Assert
            Assert.NotNull(commands);
        }
    }
}
