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
            // Arrange
            var request = typeof(Task);
            var context = new SpecimenContext(new Fixture());
            var sut = new TaskGenerator();
            // Act
            var result = sut.Create(request, context);
            // Assert
            Assert.IsAssignableFrom<Task>(result);
        }

        [Fact]
        public void CreatedNonGenericTasksAreMarkedAsComplete()
        {
            // Arrange
            var request = typeof(Task);
            var context = new SpecimenContext(new Fixture());
            var sut = new TaskGenerator();
            // Act
            var task = (Task)sut.Create(request, context);
            // Assert
            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
        }

        [Fact]
        public void CreatesGenericTasks()
        {
            // Arrange
            var request = typeof(Task<int>);
            var context = new SpecimenContext(new Fixture());
            var sut = new TaskGenerator();
            // Act
            var result = sut.Create(request, context);
            // Assert
            Assert.IsType<Task<int>>(result);
        }

        [Fact]
        public void CreatedGenericTasksAreMarkedAsComplete()
        {
            // Arrange
            var request = typeof(Task<int>);
            var context = new SpecimenContext(new Fixture());
            var sut = new TaskGenerator();
            // Act
            var task = (Task<int>)sut.Create(request, context);
            // Assert
            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
        }

        [Fact]
        public void ResultOfGenericTasksIsResolvedByTheContext()
        {
            // Arrange
            var request = typeof(Task<int>);
            var fixture = new Fixture();
            var context = new SpecimenContext(fixture);
            var sut = new TaskGenerator();

            var frozenInt = fixture.Freeze<int>();
            // Act
            var task = (Task<int>)sut.Create(request, context);
            // Assert
            Assert.Equal(frozenInt, task.Result);
        }

        [Fact]
        public void ThrowsExceptionWhenContextIsNull()
        {
            // Arrange
            const int request = 3;
            var sut = new TaskGenerator();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => sut.Create(request, null));
        }

        [Fact]
        public void ReturnsNoSpecimenWhenRequestIsNull()
        {
            // Arrange
            var context = new SpecimenContext(new Fixture());
            var sut = new TaskGenerator();
            // Act
            var result = sut.Create(null, context);
            // Assert
            Assert.IsType<NoSpecimen>(result);
        }

        [Fact]
        public void ReturnsNoSpecimenForNonTypeRequests()
        {
            // Arrange
            const int request = 3;
            var context = new SpecimenContext(new Fixture());
            var sut = new TaskGenerator();
            // Act
            var result = sut.Create(request, context);
            // Assert
            Assert.IsType<NoSpecimen>(result);
        }

        [Fact]
        public void ReturnsNoSpecimenForNonTaskTypeRequests()
        {
            // Arrange
            var request = typeof(string);
            var context = new SpecimenContext(new Fixture());
            var sut = new TaskGenerator();
            // Act
            var result = sut.Create(request, context);
            // Assert
            Assert.IsType<NoSpecimen>(result);
        }

        [Fact]
        public void ReturnsNoSpecimenForUnboundTaskRequests()
        {
            // Arrange
            var request = typeof(Task<>);
            var context = new SpecimenContext(new Fixture());
            var sut = new TaskGenerator();
            // Act
            var result = sut.Create(request, context);
            // Assert
            Assert.IsType<NoSpecimen>(result);
        }

        [Fact]
        public void ReturnsNoSpecimenForUnknownTaskRequests()
        {
            // Arrange
            var request = typeof(CustomTask);
            var context = new SpecimenContext(new Fixture());
            var sut = new TaskGenerator();
            // Act
            var result = sut.Create(request, context);
            // Assert
            Assert.IsType<NoSpecimen>(result);
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
            // Arrange
            var request = typeof(CustomGenericTask<int>);
            var context = new SpecimenContext(new Fixture());
            var sut = new TaskGenerator();
            // Act
            var result = sut.Create(request, context);
            // Assert
            Assert.IsType<NoSpecimen>(result);
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
