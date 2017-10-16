using System;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class TaskGeneratorTests
    {
        [Fact]
        public void CreatesNonGenericTasks()
        {
            // Fixture setup
            var request = typeof (Task);
            var context = new SpecimenContext(new Fixture());
            var sut = new TaskGenerator();
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            Assert.IsAssignableFrom<Task>(result);
            // Teardown
        }

        [Fact]
        public void CreatedNonGenericTasksAreMarkedAsComplete()
        {
            // Fixture setup
            var request = typeof (Task);
            var context = new SpecimenContext(new Fixture());
            var sut = new TaskGenerator();
            // Exercise system
            var task = (Task) sut.Create(request, context);
            // Verify outcome
            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            // Teardown
        }

        [Fact]
        public void CreatesGenericTasks()
        {
            // Fixture setup
            var request = typeof (Task<int>);
            var context = new SpecimenContext(new Fixture());
            var sut = new TaskGenerator();
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            Assert.IsType<Task<int>>(result);
            // Teardown
        }

        [Fact]
        public void CreatedGenericTasksAreMarkedAsComplete()
        {
            // Fixture setup
            var request = typeof (Task<int>);
            var context = new SpecimenContext(new Fixture());
            var sut = new TaskGenerator();
            // Exercise system
            var task = (Task<int>) sut.Create(request, context);
            // Verify outcome
            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            // Teardown
        }

        [Fact]
        public void ResultOfGenericTasksIsResolvedByTheContext()
        {
            // Fixture setup
            var request = typeof (Task<int>);
            var fixture = new Fixture();
            var context = new SpecimenContext(fixture);
            var sut = new TaskGenerator();

            var frozenInt = fixture.Freeze<int>();
            // Exercise system
            var task = (Task<int>) sut.Create(request, context);
            // Verify outcome
            Assert.Equal(frozenInt, task.Result);
            // Teardown
        }

        [Fact]
        public void ThrowsExceptionWhenContextIsNull()
        {
            // Fixture setup
            const int request = 3;
            var sut = new TaskGenerator();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => sut.Create(request, null));
            // Teardown
        }

        [Fact]
        public void ReturnsNoSpecimenWhenRequestIsNull()
        {
            // Fixture setup
            var context = new SpecimenContext(new Fixture());
            var sut = new TaskGenerator();
            // Exercise system
            var result = sut.Create(null, context);
            // Verify outcome
            Assert.IsType<NoSpecimen>(result);
            // Teardown
        }

        [Fact]
        public void ReturnsNoSpecimenForNonTypeRequests()
        {
            // Fixture setup
            const int request = 3;
            var context = new SpecimenContext(new Fixture());
            var sut = new TaskGenerator();
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            Assert.IsType<NoSpecimen>(result);
            // Teardown
        }

        [Fact]
        public void ReturnsNoSpecimenForNonTaskTypeRequests()
        {
            // Fixture setup
            var request = typeof (string);
            var context = new SpecimenContext(new Fixture());
            var sut = new TaskGenerator();
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            Assert.IsType<NoSpecimen>(result);
            // Teardown
        }

        [Fact]
        public void ReturnsNoSpecimenForUnboundTaskRequests()
        {
            // Fixture setup
            var request = typeof (Task<>);
            var context = new SpecimenContext(new Fixture());
            var sut = new TaskGenerator();
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            Assert.IsType<NoSpecimen>(result);
            // Teardown
        }

        [Fact]
        public void ReturnsNoSpecimenForUnknownTaskRequests()
        {
            // Fixture setup
            var request = typeof (CustomTask);
            var context = new SpecimenContext(new Fixture());
            var sut = new TaskGenerator();
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            Assert.IsType<NoSpecimen>(result);
            // Teardown
        }

        private class CustomTask : Task
        {
            public CustomTask(Action action)
                : base(action)
            {
            }
        }

        [Fact]
        public void ReturnsNoSpecimenForUnknownGenericTaskRequests()
        {
            // Fixture setup
            var request = typeof (CustomGenericTask<int>);
            var context = new SpecimenContext(new Fixture());
            var sut = new TaskGenerator();
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            Assert.IsType<NoSpecimen>(result);
            // Teardown
        }

        private class CustomGenericTask<T> : Task<T>
        {
            public CustomGenericTask(Func<T> function)
                : base(function)
            {
            }
        }
    }
}
