using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class RecursionGuardTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Act
            var sut = new DelegatingRecursionGuard(dummyBuilder);
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void SutIsNode()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Act
            var sut = new DelegatingRecursionGuard(dummyBuilder);
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilderNode>(sut);
        }

        [Fact]
        public void InitializeWithNullBuilderThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => new DelegatingRecursionGuard(null));
        }

        [Fact]
        public void InitializeWithNullEqualityComparerThrows()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => new DelegatingRecursionGuard(dummyBuilder, null));
        }

        [Fact]
        public void SutYieldsInjectedBuilder()
        {
            // Arrange
            var expected = new DelegatingSpecimenBuilder();
            var sut = new DelegatingRecursionGuard(expected);
            // Act
            // Assert
            Assert.Equal(expected, sut.Single());
            Assert.Equal(expected, ((System.Collections.IEnumerable)sut).Cast<object>().Single());
        }

        [Fact]
        public void ComparerIsCorrect()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var expected = new DelegatingEqualityComparer();
            var sut = new DelegatingRecursionGuard(dummyBuilder, expected);
            // Act
            IEqualityComparer actual = sut.Comparer;
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CreateWillUseEqualityComparer()
        {
            // Arrange
            var builder = new DelegatingSpecimenBuilder();
            builder.OnCreate = (r, c) => c.Resolve(r);
            bool comparerUsed = false;
            var comparer = new DelegatingEqualityComparer { OnEquals = (x, y) => comparerUsed = true };
            var sut = new DelegatingRecursionGuard(builder, comparer);
            sut.OnHandleRecursiveRequest = (obj) => { return null; };
            var container = new DelegatingSpecimenContext();
            container.OnResolve = (r) => sut.Create(r, container);

            // Act
            sut.Create(Guid.NewGuid(), container);

            // Assert
            Assert.True(comparerUsed);
        }

        [Fact]
        public void CreateWillNotTriggerHandlingOnFirstRequest()
        {
            // Arrange
            var sut = new DelegatingRecursionGuard(new DelegatingSpecimenBuilder());
            bool handlingTriggered = false;
            sut.OnHandleRecursiveRequest = obj => handlingTriggered = true;

            // Act
            sut.Create(Guid.NewGuid(), new DelegatingSpecimenContext());

            // Assert
            Assert.False(handlingTriggered);
        }

        [Fact]
        public void CreateWillNotTriggerHandlingOnSubsequentSimilarRequests()
        {
            // Arrange
            var sut = new DelegatingRecursionGuard(new DelegatingSpecimenBuilder());
            bool handlingTriggered = false;
            object request = Guid.NewGuid();
            sut.OnHandleRecursiveRequest = obj => handlingTriggered = true;

            // Act
            sut.Create(request, new DelegatingSpecimenContext());
            sut.Create(request, new DelegatingSpecimenContext());

            // Assert
            Assert.False(handlingTriggered);
        }

        [Fact]
        public void CreateWillTriggerHandlingOnRecursiveRequests()
        {
            // Arrange
            var builder = new DelegatingSpecimenBuilder();
            builder.OnCreate = (r, c) => c.Resolve(r);
            var sut = new DelegatingRecursionGuard(builder);
            bool handlingTriggered = false;
            sut.OnHandleRecursiveRequest = obj => handlingTriggered = true;
            var container = new DelegatingSpecimenContext();
            container.OnResolve = (r) => sut.Create(r, container);

            // Act
            sut.Create(Guid.NewGuid(), container);

            // Assert
            Assert.True(handlingTriggered);
        }

        [Fact]
        public void CreateWillNotTriggerHandlingOnSecondarySameRequestWhenDecoratedBuilderThrows()
        {
            // Arrange
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) =>
{
throw new PrivateExpectedException("The decorated builder always throws.");
}
            };

            var sut = new DelegatingRecursionGuard(builder)
            {
                OnHandleRecursiveRequest = o =>
{
throw new PrivateUnexpectedException("Recursive handling should not be triggered.");
}
            };

            var dummyContext = new DelegatingSpecimenContext();
            var sameRequest = new object();

            // Act & assert
            Assert.Throws<PrivateExpectedException>(() => sut.Create(sameRequest, dummyContext));
            Assert.Empty(sut.UnprotectedRecordedRequests);
            Assert.Throws<PrivateExpectedException>(() => sut.Create(sameRequest, dummyContext));
            Assert.Empty(sut.UnprotectedRecordedRequests);
        }

        class PrivateExpectedException : Exception
        {
            public PrivateExpectedException(string message)
                : base(message)
            {
            }
        }

        class PrivateUnexpectedException : Exception
        {
            public PrivateUnexpectedException(string message)
                : base(message)
            {
            }
        }

        [Fact]
        public void CreateWillTriggerHandlingOnSecondLevelRecursiveRequest()
        {
            // Arrange
            object subRequest1 = Guid.NewGuid();
            object subRequest2 = Guid.NewGuid();
            var requestScenario = new Stack<object>(new[] { subRequest1, subRequest2, subRequest1 });
            var builder = new DelegatingSpecimenBuilder();
            builder.OnCreate = (r, c) => c.Resolve(requestScenario.Pop());

            var sut = new DelegatingRecursionGuard(builder);
            object recursiveRequest = null;
            sut.OnHandleRecursiveRequest = obj => recursiveRequest = obj;

            var container = new DelegatingSpecimenContext();
            container.OnResolve = (r) => sut.Create(r, container);

            // Act
            sut.Create(Guid.NewGuid(), container);

            // Assert
            Assert.Same(subRequest1, recursiveRequest);
        }

        [Fact]
        public void CreateWillNotTriggerHandlingUntilDeeperThanRecursionDepth()
        {
            // Arrange
            var requestScenario = new Queue<int>(new[] { 1, 2, 1, 3, 1, 4 });
            var builder = new DelegatingSpecimenBuilder();
            builder.OnCreate = (r, c) => c.Resolve(requestScenario.Dequeue());

            // By setting the depth to two we expect the handle to be triggered at the third "1" occurrence.
            var sut = new DelegatingRecursionGuard(builder, 2);
            object recursiveRequest = null;

            sut.OnHandleRecursiveRequest = obj => recursiveRequest = obj;

            var container = new DelegatingSpecimenContext();
            container.OnResolve = r => sut.Create(r, container);

            // Act
            sut.Create(5, container);

            // Assert
            // Check that recursion was actually detected as expected
            Assert.Equal(1, recursiveRequest);
            // Check that we passed the first recursion, but didn't go any further
            Assert.Equal(4, requestScenario.Dequeue());
        }

        [Fact]
        public void ConstructWithBuilderAndRecursionHandlerHasCorrectHandler()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var expected = new DelegatingRecursionHandler();
            var sut = new RecursionGuard(dummyBuilder, expected);
            // Act
            IRecursionHandler actual = sut.RecursionHandler;
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConstructWithBuilderAndRecursionHandlerHasCorrectBuilder()
        {
            // Arrange
            var expected = new DelegatingSpecimenBuilder();
            var dummyHandler = new DelegatingRecursionHandler();
            var sut = new RecursionGuard(expected, dummyHandler);
            // Act
            var actual = sut.Builder;
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConstructWithBuilderAndRecursionHandlerHasCorrectComparer()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyHandler = new DelegatingRecursionHandler();
            var sut = new RecursionGuard(dummyBuilder, dummyHandler);
            // Act
            var actual = sut.Comparer;
            // Assert
            Assert.Equal(EqualityComparer<object>.Default, actual);
        }

        [Fact]
        public void ConstructWithNullBuilderAndRecursionHandlerThrows()
        {
            // Arrange
            var dummyHandler = new DelegatingRecursionHandler();
            // Act & assert
            Assert.Throws<ArgumentNullException>(
                () => new RecursionGuard(null, dummyHandler));
        }

        [Fact]
        public void ConstructWithBuilderAndNullRecursionHandlerThrows()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Act & assert
            Assert.Throws<ArgumentNullException>(
                () => new RecursionGuard(dummyBuilder, (IRecursionHandler)null));
        }

        [Fact]
        public void ConstructWithBuilderAndHandlerAndComparerAndRecursionDepthHasCorrectBuilder()
        {
            // Arrange
            var expected = new DelegatingSpecimenBuilder();
            var dummyHandler = new DelegatingRecursionHandler();
            var dummyComparer = new DelegatingEqualityComparer();
            var dummyRecursionDepth = 2;
            var sut = new RecursionGuard(expected, dummyHandler, dummyComparer, dummyRecursionDepth);
            // Act
            var actual = sut.Builder;
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConstructWithBuilderAndHandlerAndComparerAndRecursionDepthHasCorrectHandler()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var expected = new DelegatingRecursionHandler();
            var dummyComparer = new DelegatingEqualityComparer();
            var dummyRecursionDepth = 2;
            var sut = new RecursionGuard(dummyBuilder, expected, dummyComparer, dummyRecursionDepth);
            // Act
            var actual = sut.RecursionHandler;
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConstructWithBuilderAndHandlerAndComparerAndRecursionDepthHasCorrectComparer()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyHandler = new DelegatingRecursionHandler();
            var expected = new DelegatingEqualityComparer();
            var dummyRecursionDepth = 2;
            var sut = new RecursionGuard(dummyBuilder, dummyHandler, expected, dummyRecursionDepth);
            // Act
            var actual = sut.Comparer;
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConstructWithBuilderAndHandlerAndComparerAndRecursionDepthHasCorrectRecursionDepth()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyHandler = new DelegatingRecursionHandler();
            var dummyComparer = new DelegatingEqualityComparer();
            var expected = 2;
            var sut = new RecursionGuard(dummyBuilder, dummyHandler, dummyComparer, expected);
            // Act
            var actual = sut.RecursionDepth;
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConstructWithNullBuilderAndHandlerAndComparerAndRecursionDepthThrows()
        {
            // Arrange
            var dummyHandler = new DelegatingRecursionHandler();
            var dummyComparer = new DelegatingEqualityComparer();
            var dummyRecursionDepth = 2;
            // Act & assert
            Assert.Throws<ArgumentNullException>(
                () => new RecursionGuard(null, dummyHandler, dummyComparer, dummyRecursionDepth));
        }

        [Fact]
        public void ConstructWithBuilderAndNullHandlerAndComparerAndRecursionDepthThrows()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyComparer = new DelegatingEqualityComparer();
            var dummyRecursionDepth = 2;
            // Act & assert
            Assert.Throws<ArgumentNullException>(
                () => new RecursionGuard(dummyBuilder, null, dummyComparer, dummyRecursionDepth));
        }

        [Fact]
        public void ConstructWithBuilderAndHandlerAndNullComparerAndRecursionDepthThrows()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyHandler = new DelegatingRecursionHandler();
            var dummyRecursionDepth = 2;
            // Act & assert
            Assert.Throws<ArgumentNullException>(
                () => new RecursionGuard(dummyBuilder, dummyHandler, null, dummyRecursionDepth));
        }

        [Fact]
        [Obsolete]
        public void ConstructWithBuilderSetsRecursionDepthCorrectly()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyHandler = new DelegatingRecursionHandler();
            // Act
#pragma warning disable 618
            var sut = new RecursionGuard(dummyBuilder);
#pragma warning restore 618
            // Assert
            Assert.Equal(1, sut.RecursionDepth);
        }

        [Fact]
        public void ConstructWithBuilderAndHandlerSetsRecursionDepthCorrectly()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyHandler = new DelegatingRecursionHandler();
            // Act
            var sut = new RecursionGuard(dummyBuilder, dummyHandler);
            // Assert
            Assert.Equal(1, sut.RecursionDepth);
        }

        [Fact]
        [Obsolete]
        public void ConstructWithBuilderAndHandlerAndComparerHasCorrectBuilder()
        {
            // Arrange
            var expected = new DelegatingSpecimenBuilder();
            var dummyHandler = new DelegatingRecursionHandler();
            var dummyComparer = new DelegatingEqualityComparer();
#pragma warning disable 618
            var sut = new RecursionGuard(expected, dummyHandler, dummyComparer);
#pragma warning restore 618
            // Act
            var actual = sut.Builder;
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        [Obsolete]
        public void ConstructWithBuilderAndHandlerAndComparerHasCorrectHandler()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var expected = new DelegatingRecursionHandler();
            var dummyComparer = new DelegatingEqualityComparer();
#pragma warning disable 618
            var sut = new RecursionGuard(dummyBuilder, expected, dummyComparer);
#pragma warning restore 618
            // Act
            var actual = sut.RecursionHandler;
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        [Obsolete]
        public void ConstructWithBuilderAndHandlerAndComparerHasCorrectComparer()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyHandler = new DelegatingRecursionHandler();
            var expected = new DelegatingEqualityComparer();
#pragma warning disable 618
            var sut = new RecursionGuard(dummyBuilder, dummyHandler, expected);
#pragma warning restore 618
            // Act
            var actual = sut.Comparer;
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConstructWithBuilderAndHandlerAndRecursionDepthSetsRecursionDepthCorrectly()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyHandler = new DelegatingRecursionHandler();
            const int explicitRecursionDepth = 2;
            // Act
            var sut = new RecursionGuard(dummyBuilder, dummyHandler, explicitRecursionDepth);
            // Assert
            Assert.Equal(explicitRecursionDepth, sut.RecursionDepth);
        }

        [Fact]
        public void CreateReturnsResultFromInjectedHandlerWhenRequestIsMatched()
        {
            // Arrange
            var builder = new DelegatingSpecimenBuilder()
            {
                OnCreate = (r, ctx) => ctx.Resolve(r)
            };

            var request = new object();
            var expected = new object();
            var handlerStub = new DelegatingRecursionHandler
            {
                OnHandleRecursiveRequest = (r, rs) =>
                    {
                        Assert.Equal(request, r);
                        Assert.NotNull(rs);
                        return expected;
                    }
            };

            var comparer = new DelegatingEqualityComparer
            {
                OnEquals = (x, y) => true
            };

            var sut = new RecursionGuard(builder, handlerStub, comparer, 1);

            var context = new DelegatingSpecimenContext();
            context.OnResolve = r => sut.Create(r, context);
            // Act
            var actual = sut.Create(request, context);
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ComposeReturnsCorrectResult()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyHandler = new DelegatingRecursionHandler();
            var dummyComparer = new DelegatingEqualityComparer();
            const int dummyRecursionDepth = 2;
            var sut = new RecursionGuard(dummyBuilder, dummyHandler, dummyComparer, dummyRecursionDepth);
            // Act
            var expectedBuilders = new[]
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            };
            var actual = sut.Compose(expectedBuilders);
            // Assert
            var rg = Assert.IsAssignableFrom<RecursionGuard>(actual);
            var composite = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(rg.Builder);
            Assert.True(expectedBuilders.SequenceEqual(composite));
        }

        [Fact]
        public void ComposeSingleItemReturnsCorrectResult()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyHandler = new DelegatingRecursionHandler();
            var dummyComparer = new DelegatingEqualityComparer();
            int dummyRecursionDepth = 2;
            var sut = new RecursionGuard(dummyBuilder, dummyHandler, dummyComparer, dummyRecursionDepth);
            // Act
            var expected = new DelegatingSpecimenBuilder();
            var actual = sut.Compose(new[] { expected });
            // Assert
            var rg = Assert.IsAssignableFrom<RecursionGuard>(actual);
            Assert.Equal(expected, rg.Builder);
        }

        [Fact]
        public void ComposeRetainsHandler()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var expected = new DelegatingRecursionHandler();
            var dummyComparer = new DelegatingEqualityComparer();
            int dummyRecursionDepth = 2;
            var sut = new RecursionGuard(dummyBuilder, expected, dummyComparer, dummyRecursionDepth);
            // Act
            var actual = sut.Compose(new ISpecimenBuilder[0]);
            // Assert
            var rg = Assert.IsAssignableFrom<RecursionGuard>(actual);
            Assert.Equal(expected, rg.RecursionHandler);
        }

        [Fact]
        public void ComposeRetainsComparer()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyHandler = new DelegatingRecursionHandler();
            var expected = new DelegatingEqualityComparer();
            int dummyRecursionDepth = 2;
            var sut = new RecursionGuard(dummyBuilder, dummyHandler, expected, dummyRecursionDepth);
            // Act
            var actual = sut.Compose(new ISpecimenBuilder[0]);
            // Assert
            var rg = Assert.IsAssignableFrom<RecursionGuard>(actual);
            Assert.Equal(expected, rg.Comparer);
        }

        [Fact]
        public void ComposeRetainsRecursionDepth()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyHandler = new DelegatingRecursionHandler();
            var dummyComparer = new DelegatingEqualityComparer();
            int expected = 2;
            var sut = new RecursionGuard(dummyBuilder, dummyHandler, dummyComparer, expected);
            // Act
            var actual = sut.Compose(new ISpecimenBuilder[0]);
            // Assert
            var rg = Assert.IsAssignableFrom<RecursionGuard>(actual);
            Assert.Equal(expected, rg.RecursionDepth);
        }

        [Fact]
        public void CreateOnMultipleThreadsConcurrentlyWorks()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, ctx) => ctx.Resolve(r)
            };
            var dummyHandler = new DelegatingRecursionHandler();
            var sut = new RecursionGuard(dummyBuilder, dummyHandler);
            var dummyContext = new DelegatingSpecimenContext()
            {
                OnResolve = (r) => 99
            };
            // Act
            int[] specimens = Enumerable.Range(0, 1000)
                .AsParallel()
                    .WithDegreeOfParallelism(8)
                    .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                .Select(x => (int)sut.Create(typeof(int), dummyContext))
                .ToArray();
            // Assert
            Assert.Equal(1000, specimens.Length);
            Assert.True(specimens.All(s => s == 99));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-7)]
        [InlineData(-42)]
        public void ConstructorWithRecursionDepthLowerThanOneThrows(int recursionDepth)
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyHandler = new DelegatingRecursionHandler();
            var dummyComparer = new DelegatingEqualityComparer();
            // Act & assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new RecursionGuard(dummyBuilder, dummyHandler, dummyComparer, recursionDepth));
        }
    }
}