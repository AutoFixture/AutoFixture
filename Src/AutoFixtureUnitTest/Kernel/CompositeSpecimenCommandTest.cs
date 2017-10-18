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
            // Fixture setup
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
            // Exercise system
            sut.Execute(specimen, dummyContext);
            // Verify outcome
            Assert.True(command1Verified);
            Assert.True(command2Verified);
            // Teardown
        }

        [Fact]
        public void ExecutesEveryChildCommandWithCorrectSpecimen()
        {
            // Fixture setup
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
            // Exercise system
            sut.Execute(specimen, dummyContext);
            // Verify outcome
            Assert.True(command1Verified);
            Assert.True(command2Verified);
            // Teardown
        }

        [Fact]
        public void CtorThrowsWhenCommandsIsNull()
        {
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new CompositeSpecimenCommand(null as ISpecimenCommand[]));
            Assert.Throws<ArgumentNullException>(() => new CompositeSpecimenCommand(null as IEnumerable<ISpecimenCommand>));
        }

        [Fact]
        public void CommandsPropertyIsWiredUpThroughConstructor()
        {
            // Fixture setup
            ISpecimenCommand[] expectedCommands = {new DelegatingSpecimenCommand()};
            var sut = new CompositeSpecimenCommand(expectedCommands);
            // Exercise system
            var commands = sut.Commands;
            // Verify outcome
            Assert.Same(expectedCommands, commands);
            // Teardown
        }

        [Fact]
        public void CommandsIsNotNullWhenSutIsCreatedWithDefaultConstructor()
        {
            // Fixture setup
            var sut = new CompositeSpecimenCommand();
            // Exercise system
            var commands = sut.Commands;
            // Verify outcome
            Assert.NotNull(commands);
            // Teardown
        }
    }
}
