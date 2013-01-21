using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;

namespace Ploeh.AutoFixtureUnitTest
{
    public class BehaviorRootTest
    {
        [Fact]
        public void SutIsCompositeSpecimenBuilder()
        {
            var dummy = new DelegatingSpecimenBuilder();
            var sut = new BehaviorRoot(dummy);
            Assert.IsAssignableFrom<ISpecimenBuilderNode>(sut);
        }

        [Fact]
        public void BuilderIsCorrect()
        {
            // Fixture setup
            var expected = new DelegatingSpecimenBuilder();
            var sut = new BehaviorRoot(expected);
            // Exercise system
            ISpecimenBuilder actual = sut.Builder;
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void ComposeReturnsCorrectResult()
        {
            // Fixture setup
            var dummy = new DelegatingSpecimenBuilder();
            var sut = new BehaviorRoot(dummy);
            // Exercise system
            var expected = new[]
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            };
            var actual = sut.Compose(expected);
            // Verify outcome
            var br = Assert.IsAssignableFrom<BehaviorRoot>(actual);
            var builders = 
                Assert.IsAssignableFrom<IEnumerable<ISpecimenBuilder>>(
                    br.Builder);
            Assert.True(expected.SequenceEqual(builders));
            // Teardown
        }

        [Fact]
        public void ComposeSingleItemReturnsCorrectResult()
        {
            // Fixture setup
            var dummy = new DelegatingSpecimenBuilder();
            var sut = new BehaviorRoot(dummy);
            var expected = new DelegatingSpecimenBuilder();
            // Exercise system
            var actual = sut.Compose(new[] { expected });
            // Verify outcome
            var br = Assert.IsAssignableFrom<BehaviorRoot>(actual);
            Assert.Equal(expected, br.Builder);
            // Teardown
        }

        [Fact]
        public void CreateReturnsCorrectResult()
        {
            // Fixture setup
            var request = new object();
            var context = new DelegatingSpecimenContext();
            var expected = new object();
            var stub = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) =>
                {
                    Assert.Equal(request, r);
                    Assert.Equal(context, c);
                    return expected;
                }
            };
            var sut = new BehaviorRoot(stub);
            // Exercise system
            var actual = sut.Create(request, context);
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void SutYieldsDecoratedBuilder()
        {
            // Fixture setup
            var expected = new DelegatingSpecimenBuilder();            
            // Exercise system
            var sut = new BehaviorRoot(expected);
            // Verify outcome
            Assert.True(new[] { expected }.SequenceEqual(sut));
            Assert.True(new object[] { expected }.SequenceEqual(
                ((System.Collections.IEnumerable)sut).Cast<object>()));
            // Teardown
        }

        [Fact]
        public void ConstructWithNullBuilderThrows()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new BehaviorRoot(null));
        }
    }
}
