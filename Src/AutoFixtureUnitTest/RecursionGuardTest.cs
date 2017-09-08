using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;
using System.Collections;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest
{
    public class RecursionGuardTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Exercise system
            var sut = new DelegatingRecursionGuard(dummyBuilder);
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void SutIsNode()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Exercise system
            var sut = new DelegatingRecursionGuard(dummyBuilder);
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilderNode>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullBuilderThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new DelegatingRecursionGuard(null));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullEqualityComparerThrows()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new DelegatingRecursionGuard(dummyBuilder, null));
            // Teardown
        }

        [Fact]
        public void SutYieldsInjectedBuilder()
        {
            // Fixture setup
            var expected = new DelegatingSpecimenBuilder();
            var sut = new DelegatingRecursionGuard(expected);
            // Exercise system
            // Verify outcome
            Assert.Equal(expected, sut.Single());
            Assert.Equal(expected, ((System.Collections.IEnumerable)sut).Cast<object>().Single());
            // Teardown
        }

        [Fact]
        public void ComparerIsCorrect()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var expected = new DelegatingEqualityComparer();
            var sut = new DelegatingRecursionGuard(dummyBuilder, expected);
            // Exercise system
            IEqualityComparer actual = sut.Comparer;
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void CreateWillUseEqualityComparer()
        {
            // Fixture setup
            var builder = new DelegatingSpecimenBuilder();
            builder.OnCreate = (r, c) => c.Resolve(r);
            bool comparerUsed = false;
            var comparer = new DelegatingEqualityComparer { OnEquals = (x, y) => comparerUsed = true };
            var sut = new DelegatingRecursionGuard(builder, comparer);
            sut.OnHandleRecursiveRequest = (obj) => { return null; };
            var container = new DelegatingSpecimenContext();
            container.OnResolve = (r) => sut.Create(r, container);

            // Exercise system
            sut.Create(Guid.NewGuid(), container);

            // Verify outcome
            Assert.True(comparerUsed);
        }

        [Fact]
        public void CreateWillNotTriggerHandlingOnFirstRequest()
        {
            // Fixture setup
            var sut = new DelegatingRecursionGuard(new DelegatingSpecimenBuilder());
            bool handlingTriggered = false;
            sut.OnHandleRecursiveRequest = obj => handlingTriggered = true;

            // Exercise system
            sut.Create(Guid.NewGuid(), new DelegatingSpecimenContext());

            // Verify outcome
            Assert.False(handlingTriggered);
        }

        [Fact]
        public void CreateWillNotTriggerHandlingOnSubsequentSimilarRequests()
        {
            // Fixture setup
            var sut = new DelegatingRecursionGuard(new DelegatingSpecimenBuilder());
            bool handlingTriggered = false;
            object request = Guid.NewGuid();
            sut.OnHandleRecursiveRequest = obj => handlingTriggered = true;

            // Exercise system
            sut.Create(request, new DelegatingSpecimenContext());
            sut.Create(request, new DelegatingSpecimenContext());

            // Verify outcome
            Assert.False(handlingTriggered);
        }

        [Fact]
        public void CreateWillTriggerHandlingOnRecursiveRequests()
        {
            // Fixture setup
            var builder = new DelegatingSpecimenBuilder();
            builder.OnCreate = (r, c) => c.Resolve(r);
            var sut = new DelegatingRecursionGuard(builder);
            bool handlingTriggered = false;
            sut.OnHandleRecursiveRequest = obj => handlingTriggered = true;
            var container = new DelegatingSpecimenContext();
            container.OnResolve = (r) => sut.Create(r, container);

            // Exercise system
            sut.Create(Guid.NewGuid(), container);

            // Verify outcome
            Assert.True(handlingTriggered);
        }

        [Fact]
        public void CreateWillNotTriggerHandlingOnSecondarySameRequestWhenDecoratedBuilderThrows()
        {
            // Fixture setup
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) =>
            {
                throw new PrivateExpectedException("The decorated builder always throws.");
            }};

            var sut = new DelegatingRecursionGuard(builder) { OnHandleRecursiveRequest = o =>
            {
                throw new PrivateUnexpectedException("Recursive handling should not be triggered.");
            }};

            var dummyContext = new DelegatingSpecimenContext();
            var sameRequest = new object();

            // Exercise system and verify outcome
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
            // Fixture setup
            object subRequest1 = Guid.NewGuid();
            object subRequest2 = Guid.NewGuid();
            var requestScenario = new Stack<object>(new [] { subRequest1, subRequest2, subRequest1 });
            var builder = new DelegatingSpecimenBuilder();
            builder.OnCreate = (r, c) => c.Resolve(requestScenario.Pop());

            var sut = new DelegatingRecursionGuard(builder);
            object recursiveRequest = null;
            sut.OnHandleRecursiveRequest = obj => recursiveRequest = obj;

            var container = new DelegatingSpecimenContext();
            container.OnResolve = (r) => sut.Create(r, container);

            // Exercise system
            sut.Create(Guid.NewGuid(), container);

            // Verify outcome
            Assert.Same(subRequest1, recursiveRequest);
        }

        [Fact]
        public void CreateWillNotTriggerHandlingUntilDeeperThanRecursionDepth()
        {
            // Fixture setup
            var requestScenario = new Queue<int>(new[] { 1, 2, 1, 3, 1, 4 });
            var builder = new DelegatingSpecimenBuilder();
            builder.OnCreate = (r, c) => c.Resolve(requestScenario.Dequeue());

            // By setting the depth to two we expect the handle to be triggered at the third "1" occurrence.
            var sut = new DelegatingRecursionGuard(builder, 2);
            object recursiveRequest = null;

            sut.OnHandleRecursiveRequest = obj => recursiveRequest = obj;
            

            var container = new DelegatingSpecimenContext();
            container.OnResolve = r => sut.Create(r, container);

            // Exercise system
            sut.Create(5, container);

            // Verify outcome
            // Check that recursion was actually detected as expected
            Assert.Equal(1, recursiveRequest);
            // Check that we passed the first recursion, but didn't go any further
            Assert.Equal(4, requestScenario.Dequeue());
        }

        [Fact]
        public void ConstructWithBuilderAndRecursionHandlerHasCorrectHandler()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var expected = new DelegatingRecursionHandler();
            var sut = new RecursionGuard(dummyBuilder, expected);
            // Exercise system
            IRecursionHandler actual = sut.RecursionHandler;
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void ConstructWithBuilderAndRecursionHandlerHasCorrectBuilder()
        {
            // Fixture setup
            var expected = new DelegatingSpecimenBuilder();
            var dummyHandler = new DelegatingRecursionHandler();
            var sut = new RecursionGuard(expected, dummyHandler);
            // Exercise system
            var actual = sut.Builder;
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void ConstructWithBuilderAndRecursionHandlerHasCorrectComparer()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyHandler = new DelegatingRecursionHandler();
            var sut = new RecursionGuard(dummyBuilder, dummyHandler);
            // Exercise system
            var actual = sut.Comparer;
            // Verify outcome
            Assert.Equal(EqualityComparer<object>.Default, actual);
            // Teardown
        }

        [Fact]
        public void ConstructWithNullBuilderAndRecursionHandlerThrows()
        {
            // Fixture setup
            var dummyHandler = new DelegatingRecursionHandler();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(
                () => new RecursionGuard(null, dummyHandler));
            // Teardown
        }

        [Fact]
        public void ConstructWithBuilderAndNullRecursionHandlerThrows()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(
                () => new RecursionGuard(dummyBuilder, (IRecursionHandler)null));
            // Teardown
        }

        [Fact]
        public void ConstructWithBuilderAndHandlerAndComparerAndRecursionDepthHasCorrectBuilder()
        {
            // Fixture setup
            var expected = new DelegatingSpecimenBuilder();
            var dummyHandler = new DelegatingRecursionHandler();
            var dummyComparer = new DelegatingEqualityComparer();
            var dummyRecursionDepth = 2;
            var sut = new RecursionGuard(expected, dummyHandler, dummyComparer, dummyRecursionDepth);
            // Exercise system
            var actual = sut.Builder;
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void ConstructWithBuilderAndHandlerAndComparerAndRecursionDepthHasCorrectHandler()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var expected = new DelegatingRecursionHandler();
            var dummyComparer = new DelegatingEqualityComparer();
            var dummyRecursionDepth = 2;
            var sut = new RecursionGuard(dummyBuilder, expected, dummyComparer, dummyRecursionDepth);
            // Exercise system
            var actual = sut.RecursionHandler;
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void ConstructWithBuilderAndHandlerAndComparerAndRecursionDepthHasCorrectComparer()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyHandler = new DelegatingRecursionHandler();
            var expected = new DelegatingEqualityComparer();
            var dummyRecursionDepth = 2;
            var sut = new RecursionGuard(dummyBuilder, dummyHandler, expected, dummyRecursionDepth);
            // Exercise system
            var actual = sut.Comparer;
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }        
        
        [Fact]
        public void ConstructWithBuilderAndHandlerAndComparerAndRecursionDepthHasCorrectRecursionDepth()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyHandler = new DelegatingRecursionHandler();
            var dummyComparer = new DelegatingEqualityComparer();
            var expected = 2;
            var sut = new RecursionGuard(dummyBuilder, dummyHandler, dummyComparer, expected);
            // Exercise system
            var actual = sut.RecursionDepth;
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void ConstructWithNullBuilderAndHandlerAndComparerAndRecursionDepthThrows()
        {
            // Fixture setup
            var dummyHandler = new DelegatingRecursionHandler();
            var dummyComparer = new DelegatingEqualityComparer();
            var dummyRecursionDepth = 2;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(
                () => new RecursionGuard(null, dummyHandler, dummyComparer, dummyRecursionDepth));
            // Teardown
        }

        [Fact]
        public void ConstructWithBuilderAndNullHandlerAndComparerAndRecursionDepthThrows()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyComparer = new DelegatingEqualityComparer();
            var dummyRecursionDepth = 2;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(
                () => new RecursionGuard(dummyBuilder, null, dummyComparer, dummyRecursionDepth));
            // Teardown
        }

        [Fact]
        public void ConstructWithBuilderAndHandlerAndNullComparerAndRecursionDepthThrows()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyHandler = new DelegatingRecursionHandler();
            var dummyRecursionDepth = 2;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(
                () => new RecursionGuard(dummyBuilder, dummyHandler, null, dummyRecursionDepth));
            // Teardown
        }

        [Fact]
        public void ConstructWithBuilderSetsRecursionDepthCorrectly()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyHandler = new DelegatingRecursionHandler();
            // Exercise system
#pragma warning disable 618
            var sut = new RecursionGuard(dummyBuilder);
#pragma warning restore 618
            // Verify outcome
            Assert.Equal(1, sut.RecursionDepth);
        }

        [Fact]
        public void ConstructWithBuilderAndHandlerSetsRecursionDepthCorrectly()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyHandler = new DelegatingRecursionHandler();
            // Exercise system
            var sut = new RecursionGuard(dummyBuilder, dummyHandler);
            // Verify outcome
            Assert.Equal(1, sut.RecursionDepth);
        }

        [Fact]
        public void ConstructWithBuilderAndHandlerAndComparerHasCorrectBuilder()
        {
            // Fixture setup
            var expected = new DelegatingSpecimenBuilder();
            var dummyHandler = new DelegatingRecursionHandler();
            var dummyComparer = new DelegatingEqualityComparer();
#pragma warning disable 618
            var sut = new RecursionGuard(expected, dummyHandler, dummyComparer);
#pragma warning restore 618
            // Exercise system
            var actual = sut.Builder;
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void ConstructWithBuilderAndHandlerAndComparerHasCorrectHandler()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var expected = new DelegatingRecursionHandler();
            var dummyComparer = new DelegatingEqualityComparer();
#pragma warning disable 618
            var sut = new RecursionGuard(dummyBuilder, expected, dummyComparer);
#pragma warning restore 618
            // Exercise system
            var actual = sut.RecursionHandler;
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void ConstructWithBuilderAndHandlerAndComparerHasCorrectComparer()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyHandler = new DelegatingRecursionHandler();
            var expected = new DelegatingEqualityComparer();
#pragma warning disable 618
            var sut = new RecursionGuard(dummyBuilder, dummyHandler, expected);
#pragma warning restore 618
            // Exercise system
            var actual = sut.Comparer;
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }
        
        [Fact]
        public void ConstructWithBuilderAndHandlerAndRecursionDepthSetsRecursionDepthCorrectly()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyHandler = new DelegatingRecursionHandler();
            const int explicitRecursionDepth = 2;
            // Exercise system
            var sut = new RecursionGuard(dummyBuilder, dummyHandler, explicitRecursionDepth);
            // Verify outcome
            Assert.Equal(explicitRecursionDepth, sut.RecursionDepth);
        }

        [Fact]
        public void CreateReturnsResultFromInjectedHandlerWhenRequestIsMatched()
        {
            // Fixture setup
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
            // Exercise system
            var actual = sut.Create(request, context);
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void ComposeReturnsCorrectResult()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyHandler = new DelegatingRecursionHandler();
            var dummyComparer = new DelegatingEqualityComparer();
            const int dummyRecursionDepth = 2;
            var sut = new RecursionGuard(dummyBuilder, dummyHandler, dummyComparer, dummyRecursionDepth);
            // Exercise system
            var expectedBuilders = new[]
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            };
            var actual = sut.Compose(expectedBuilders);
            // Verify outcome
            var rg = Assert.IsAssignableFrom<RecursionGuard>(actual);
            var composite = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(rg.Builder);
            Assert.True(expectedBuilders.SequenceEqual(composite));
            // Teardown
        }

        [Fact]
        public void ComposeSingleItemReturnsCorrectResult()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyHandler = new DelegatingRecursionHandler();
            var dummyComparer = new DelegatingEqualityComparer();
            int dummyRecursionDepth = 2;
            var sut = new RecursionGuard(dummyBuilder, dummyHandler, dummyComparer, dummyRecursionDepth);
            // Exercise system
            var expected = new DelegatingSpecimenBuilder();
            var actual = sut.Compose(new[] { expected });
            // Verify outcome
            var rg = Assert.IsAssignableFrom<RecursionGuard>(actual);
            Assert.Equal(expected, rg.Builder);
            // Teardown
        }

        [Fact]
        public void ComposeRetainsHandler()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var expected = new DelegatingRecursionHandler();
            var dummyComparer = new DelegatingEqualityComparer();
            int dummyRecursionDepth = 2;
            var sut = new RecursionGuard(dummyBuilder, expected, dummyComparer, dummyRecursionDepth);
            // Exercise system
            var actual = sut.Compose(new ISpecimenBuilder[0]);
            // Verify outcome
            var rg = Assert.IsAssignableFrom<RecursionGuard>(actual);
            Assert.Equal(expected, rg.RecursionHandler);
            // Teardown
        }

        [Fact]
        public void ComposeRetainsComparer()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyHandler = new DelegatingRecursionHandler();
            var expected = new DelegatingEqualityComparer();
            int dummyRecursionDepth = 2;
            var sut = new RecursionGuard(dummyBuilder, dummyHandler, expected, dummyRecursionDepth);
            // Exercise system
            var actual = sut.Compose(new ISpecimenBuilder[0]);
            // Verify outcome
            var rg = Assert.IsAssignableFrom<RecursionGuard>(actual);
            Assert.Equal(expected, rg.Comparer);
            // Teardown
        }

        [Fact]
        public void ComposeRetainsRecursionDepth()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyHandler = new DelegatingRecursionHandler();
            var dummyComparer = new DelegatingEqualityComparer();
            int expected = 2;
            var sut = new RecursionGuard(dummyBuilder, dummyHandler, dummyComparer, expected);
            // Exercise system
            var actual = sut.Compose(new ISpecimenBuilder[0]);
            // Verify outcome
            var rg = Assert.IsAssignableFrom<RecursionGuard>(actual);
            Assert.Equal(expected, rg.RecursionDepth);
            // Teardown
        }

        [Fact]
        public void CreateOnMultipleThreadsConcurrentlyWorks()
        {
            // Fixture setup
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
            // Exercise system
            int[] specimens = Enumerable.Range(0, 1000)
                .AsParallel()
                    .WithDegreeOfParallelism(8)
                    .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                .Select(x => (int)sut.Create(typeof(int), dummyContext))
                .ToArray();
            // Verify outcome
            Assert.Equal(1000, specimens.Length);
            Assert.True(specimens.All(s => s == 99));
            // Teardown
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-7)]
        [InlineData(-42)]
        public void ConstructorWithRecursionDepthLowerThanOneThrows(int recursionDepth)
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var dummyHandler = new DelegatingRecursionHandler();
            var dummyComparer = new DelegatingEqualityComparer();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new RecursionGuard(dummyBuilder, dummyHandler, dummyComparer, recursionDepth));
            // Teardown
        }
    }
}