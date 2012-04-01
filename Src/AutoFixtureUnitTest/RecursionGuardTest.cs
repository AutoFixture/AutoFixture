using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;
using System.Collections;

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
    }
}