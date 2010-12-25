using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Idioms;
using System.Reflection;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class ReflectionBoundaryBehaviorTest
    {
        [Fact]
        public void SutIsBoundaryBehavior()
        {
            // Fixture setup
            var dummyBehavior = new DelegatingBoundaryBehavior();
            // Exercise system
            var sut = new ReflectionBoundaryBehavior(dummyBehavior);
            // Verify outcome
            Assert.IsAssignableFrom<ExceptionBoundaryBehavior>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullBehaviorThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new ReflectionBoundaryBehavior(null));
            // Teardown
        }

        [Fact]
        public void ExerciseExercisesDecoratedBehavior()
        {
            // Fixture setup
            Action<object> action = x => { };
            var verified = false;
            var mock = new DelegatingBoundaryBehavior { OnExercise = a => verified = a == action };

            var sut = new ReflectionBoundaryBehavior(mock);
            // Exercise system
            sut.Exercise(action);
            // Verify outcome
            Assert.True(verified, "Mock verified.");
            // Teardown
        }

        [Fact]
        public void DescriptionReturnsDecoratedDescription()
        {
            // Fixture setup
            var expectedDescription = "Anonymous description.";
            var stub = new DelegatingBoundaryBehavior { WritableDescription = expectedDescription };

            var sut = new ReflectionBoundaryBehavior(stub);
            // Exercise system
            var result = sut.Description;
            // Verify outcome
            Assert.Equal(expectedDescription, result);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByNonReflectionExceptionReturnsCorrectResult()
        {
            // Fixture setup
            var nonReflectionException = new Exception();

            var dummyBehavior = new DelegatingBoundaryBehavior();
            var sut = new ReflectionBoundaryBehavior(dummyBehavior);
            // Exercise system
            var result = sut.IsSatisfiedBy(nonReflectionException);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByReflectionExceptionReturnsCorrectResult()
        {
            // Fixture setup
            var innerException = new Exception();
            var reflectionException = new TargetInvocationException(innerException);
            var stub = new DelegatingBoundaryBehavior { OnIsSatisfiedBy = e => e == innerException };
            var sut = new ReflectionBoundaryBehavior(stub);
            // Exercise system
            var result = sut.IsSatisfiedBy(reflectionException);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }
    }
}
