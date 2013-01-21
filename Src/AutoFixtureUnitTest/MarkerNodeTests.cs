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
    public abstract class MarkerNodeTests<T> where T : ISpecimenBuilderNode
    {

        [Fact]
        public void BuilderIsCorrect()
        {
            // Fixture setup
            var expected = new DelegatingSpecimenBuilder();
            var sut = this.CreateSut(expected);
            // Exercise system
            ISpecimenBuilder actual = this.GetBuilder(sut);
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void ComposeReturnsCorrectResult()
        {
            // Fixture setup
            var dummy = new DelegatingSpecimenBuilder();
            var sut = this.CreateSut(dummy);
            // Exercise system
            var expected = new[]
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            };
            var actual = sut.Compose(expected);
            // Verify outcome
            var mn = Assert.IsAssignableFrom<T>(actual);
            var builders = 
                Assert.IsAssignableFrom<IEnumerable<ISpecimenBuilder>>(
                    this.GetBuilder(mn));
            Assert.True(expected.SequenceEqual(builders));
            // Teardown
        }

        [Fact]
        public void ComposeSingleItemReturnsCorrectResult()
        {
            // Fixture setup
            var dummy = new DelegatingSpecimenBuilder();
            var sut = this.CreateSut(dummy);
            var expected = new DelegatingSpecimenBuilder();
            // Exercise system
            var actual = sut.Compose(new[] { expected });
            // Verify outcome
            var mn = Assert.IsAssignableFrom<T>(actual);
            Assert.Equal(expected, this.GetBuilder(mn));
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
            var sut = this.CreateSut(stub);
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
            var sut = this.CreateSut(expected);
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
                this.CreateSut(null));
        }

        public abstract T CreateSut(ISpecimenBuilder builder);

        public abstract ISpecimenBuilder GetBuilder(T sut);
    }

    public class BehaviorRootTests : MarkerNodeTests<BehaviorRoot>
    {
        public override BehaviorRoot CreateSut(ISpecimenBuilder builder)
        {
            return new BehaviorRoot(builder);
        }

        public override ISpecimenBuilder GetBuilder(BehaviorRoot sut)
        {
            return sut.Builder;
        }
    }

    public class CustomizationNodeTests : MarkerNodeTests<CustomizationNode>
    {
        public override CustomizationNode CreateSut(ISpecimenBuilder builder)
        {
            return new CustomizationNode(builder);
        }

        public override ISpecimenBuilder GetBuilder(CustomizationNode sut)
        {
            return sut.Builder;
        }
    }

    public class ResidueCollectorNodeTests : MarkerNodeTests<ResidueCollectorNode>
    {
        public override ResidueCollectorNode CreateSut(ISpecimenBuilder builder)
        {
            return new ResidueCollectorNode(builder);
        }

        public override ISpecimenBuilder GetBuilder(ResidueCollectorNode sut)
        {
            return sut.Builder;
        }
    }

    public class AutoPropertiesTargetTests : MarkerNodeTests<AutoPropertiesTarget>
    {
        public override AutoPropertiesTarget CreateSut(ISpecimenBuilder builder)
        {
            return new AutoPropertiesTarget(builder);
        }

        public override ISpecimenBuilder GetBuilder(AutoPropertiesTarget sut)
        {
            return sut.Builder;
        }
    }
}
