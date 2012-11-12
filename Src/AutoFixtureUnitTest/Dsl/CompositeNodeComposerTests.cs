using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixture.Dsl;

namespace Ploeh.AutoFixtureUnitTest.Dsl
{
    public class CompositeNodeComposerTests
    {
        [Fact]
        public void SutIsComposer()
        {
            // Fixture setup
            var dummyNode = new CompositeSpecimenBuilder();
            // Exercise system
            var sut = new CompositeNodeComposer<object>(dummyNode);
            // Verify outcome
            Assert.IsAssignableFrom<ICustomizationComposer<object>>(sut);
            // Teardown
        }

        [Fact]
        public void SutIsSpecimenBuilderNode()
        {
            // Fixture setup
            var dummyNode = new CompositeSpecimenBuilder();
            // Exercise system
            var sut = new CompositeNodeComposer<float>(dummyNode);
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilderNode>(sut);
            // Teardown
        }

        [Fact]
        public void ComposeReturnsCorrectResult()
        {
            // Fixture setup
            var dummyNode = new CompositeSpecimenBuilder();
            var sut = new CompositeNodeComposer<uint>(dummyNode);
            // Exercise system
            var expectedBuilders = new[]
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            };
            var actual = sut.Compose(expectedBuilders);
            // Verify outcome
            var c =
                Assert.IsAssignableFrom<CompositeNodeComposer<uint>>(actual);
            var composite = 
                Assert.IsAssignableFrom<CompositeSpecimenBuilder>(c.Node);
            Assert.True(expectedBuilders.SequenceEqual(composite));
            // Teardown
        }

        [Fact]
        public void ComposeSingleNodeReturnsCorrectResult()
        {            
            // Fixture setup
            var dummyNode = new CompositeSpecimenBuilder();
            var sut = new CompositeNodeComposer<uint>(dummyNode);
            ISpecimenBuilderNode expected = new CompositeSpecimenBuilder();
            // Exercise system
            var actual = sut.Compose(new[] { expected });
            // Verify outcome
            var c =
                Assert.IsAssignableFrom<CompositeNodeComposer<uint>>(actual);
            Assert.Equal(expected, c.Node);
            // Teardown
        }

        [Fact]
        public void CreateReturnsCorrectResult()
        {
            // Fixture setup
            var request = new object();
            var context = new DelegatingSpecimenContext();
            var expected = new object();
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => r == request && c == context ?
                    expected :
                    new NoSpecimen(r)
            };
            var sut = new CompositeNodeComposer<ushort>(
                new CompositeSpecimenBuilder(
                    builder));
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
            var expected = new CompositeSpecimenBuilder();            
            // Exercise system
            var sut = new CompositeNodeComposer<uint>(expected);
            // Verify outcome
            Assert.True(new[] { expected }.SequenceEqual(sut));
            Assert.True(new object[] { expected }.SequenceEqual(
                ((System.Collections.IEnumerable)sut).Cast<object>()));
            // Teardown
        }

        [Fact]
        public void NodeIsCorrect()
        {
            // Fixture setup
            var expected = new CompositeSpecimenBuilder();
            var sut = new CompositeNodeComposer<string>(expected);
            // Exercise system
            ISpecimenBuilderNode actual = sut.Node;
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void ConstructWithNullNodeThrows()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new CompositeNodeComposer<Guid>(null));
        }

        [Fact]
        public void FromSeedReturnsCorrectResult()
        {
            // Fixture setup
            var node = new CompositeSpecimenBuilder(
                new DelegatingSpecimenBuilder(),
                NodeComposer.Create<int>(),
                NodeComposer.Create<string>(),
                NodeComposer.Create<int>(),
                new DelegatingSpecimenBuilder());
            var sut = new CompositeNodeComposer<int>(node);
            Func<int, int> f = i => i;
            // Exercise system
            var actual = sut.FromSeed(f);
            // Verify outcome
            var expected = new CompositeNodeComposer<int>(
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    (ISpecimenBuilder)NodeComposer.Create<int>().FromSeed(f),
                    NodeComposer.Create<string>(),
                    (ISpecimenBuilder)NodeComposer.Create<int>().FromSeed(f),
                    new DelegatingSpecimenBuilder()));
            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
            // Teardown
        }

        [Fact]
        public void FromSpecimenBuilderFactoryReturnsCorrectResult()
        {
            // Fixture setup
            var node = new CompositeSpecimenBuilder(
                new DelegatingSpecimenBuilder(),
                NodeComposer.Create<decimal>(),
                NodeComposer.Create<int>(),
                NodeComposer.Create<decimal>(),
                new DelegatingSpecimenBuilder());
            var sut = new CompositeNodeComposer<decimal>(node);
            var factory = new DelegatingSpecimenBuilder();
            // Exercise system
            var actual = sut.FromFactory(factory);
            // Verify outcome
            var expected = new CompositeNodeComposer<decimal>(
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    (ISpecimenBuilder)NodeComposer.Create<decimal>().FromFactory(factory),
                    NodeComposer.Create<int>(),
                    (ISpecimenBuilder)NodeComposer.Create<decimal>().FromFactory(factory),
                    new DelegatingSpecimenBuilder()));
            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
            // Teardown
        }

        [Fact]
        public void FromNoArgFuncReturnsCorrectResult()
        {
            // Fixture setup
            var node = new CompositeSpecimenBuilder(
                new DelegatingSpecimenBuilder(),
                NodeComposer.Create<Guid>(),
                NodeComposer.Create<float>(),
                NodeComposer.Create<Guid>(),
                new DelegatingSpecimenBuilder());
            var sut = new CompositeNodeComposer<Guid>(node);
            Func<Guid> f = () => Guid.Empty;
            // Exercise system
            var actual = sut.FromFactory(f);
            // Verify outcome
            var expected = new CompositeNodeComposer<Guid>(
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    (ISpecimenBuilder)NodeComposer.Create<Guid>().FromFactory(f),
                    NodeComposer.Create<float>(),
                    (ISpecimenBuilder)NodeComposer.Create<Guid>().FromFactory(f),
                    new DelegatingSpecimenBuilder()));
            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
            // Teardown
        }

        [Fact]
        public void FromSingleArgFuncReturnsCorrectResult()
        {
            // Fixture setup
            var node = new CompositeSpecimenBuilder(
                new DelegatingSpecimenBuilder(),
                NodeComposer.Create<string>(),
                NodeComposer.Create<ulong>(),
                NodeComposer.Create<string>(),
                new DelegatingSpecimenBuilder());
            var sut = new CompositeNodeComposer<string>(node);
            Func<int, string> f = i => i.ToString();
            // Exercise system
            var actual = sut.FromFactory(f);
            // Verify outcome
            var expected = new CompositeNodeComposer<string>(
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    (ISpecimenBuilder)NodeComposer.Create<string>().FromFactory(f),
                    NodeComposer.Create<ulong>(),
                    (ISpecimenBuilder)NodeComposer.Create<string>().FromFactory(f),
                    new DelegatingSpecimenBuilder()));
            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
            // Teardown
        }

        [Fact]
        public void FromDoubleArgFuncReturnsCorrectResult()
        {
            // Fixture setup
            var node = new CompositeSpecimenBuilder(
                new DelegatingSpecimenBuilder(),
                NodeComposer.Create<Version>(),
                NodeComposer.Create<string>(),
                NodeComposer.Create<Version>(),
                new DelegatingSpecimenBuilder());
            var sut = new CompositeNodeComposer<Version>(node);
            Func<int, int, Version> f = (mj, mn) => new Version(mj, mn);
            // Exercise system
            var actual = sut.FromFactory(f);
            // Verify outcome
            var expected = new CompositeNodeComposer<Version>(
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    (ISpecimenBuilder)NodeComposer.Create<Version>().FromFactory(f),
                    NodeComposer.Create<string>(),
                    (ISpecimenBuilder)NodeComposer.Create<Version>().FromFactory(f),
                    new DelegatingSpecimenBuilder()));
            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
            // Teardown
        }

        [Fact]
        public void FromTripleArgFuncReturnsCorrectResult()
        {
            // Fixture setup
            var node = new CompositeSpecimenBuilder(
                new DelegatingSpecimenBuilder(),
                NodeComposer.Create<long>(),
                NodeComposer.Create<Version>(),
                NodeComposer.Create<long>(),
                new DelegatingSpecimenBuilder());
            var sut = new CompositeNodeComposer<long>(node);
            Func<long, int, short, long> f = (x, y, z) => x;
            // Exercise system
            var actual = sut.FromFactory(f);
            // Verify outcome
            var expected = new CompositeNodeComposer<long>(
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    (ISpecimenBuilder)NodeComposer.Create<long>().FromFactory(f),
                    NodeComposer.Create<Version>(),
                    (ISpecimenBuilder)NodeComposer.Create<long>().FromFactory(f),
                    new DelegatingSpecimenBuilder()));
            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
            // Teardown
        }

        [Fact]
        public void FromQuadrupleArgFuncReturnsCorrectResult()
        {
            // Fixture setup
            var node = new CompositeSpecimenBuilder(
                new DelegatingSpecimenBuilder(),
                NodeComposer.Create<Version>(),
                NodeComposer.Create<Guid>(),
                NodeComposer.Create<Version>(),
                new DelegatingSpecimenBuilder());
            var sut = new CompositeNodeComposer<Version>(node);
            Func<int, int, int , int, Version> f = 
                (mj, mn, b, r) => new Version(mj, mn, b, r);
            // Exercise system
            var actual = sut.FromFactory(f);
            // Verify outcome
            var expected = new CompositeNodeComposer<Version>(
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    (ISpecimenBuilder)NodeComposer.Create<Version>().FromFactory(f),
                    NodeComposer.Create<Guid>(),
                    (ISpecimenBuilder)NodeComposer.Create<Version>().FromFactory(f),
                    new DelegatingSpecimenBuilder()));
            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
            // Teardown
        }

        [Fact]
        public void LegacyComposeReturnsCorrectResult()
        {
            // Fixture setup
            var dummyNode = new CompositeSpecimenBuilder();
            var sut = new CompositeNodeComposer<UTF8Encoding>(dummyNode);
            // Exercise system
            var actual = sut.Compose();
            // Verify outcome
            Assert.Equal(sut, actual);
            // Teardown
        }

        [Fact]
        public void DoReturnsCorrectResult()
        {
            // Fixture setup
            var node = new CompositeSpecimenBuilder(
                new DelegatingSpecimenBuilder(),
                NodeComposer.Create<AppDomainSetup>(),
                NodeComposer.Create<bool>(),
                NodeComposer.Create<AppDomainSetup>(),
                new DelegatingSpecimenBuilder());
            var sut = new CompositeNodeComposer<AppDomainSetup>(node);
            Action<AppDomainSetup> a =
                ads => ads.DisallowApplicationBaseProbing = false;
            // Exercise system
            var actual = sut.Do(a);
            // Verify outcome
            var expected = new CompositeNodeComposer<AppDomainSetup>(
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    (ISpecimenBuilder)NodeComposer.Create<AppDomainSetup>().Do(a),
                    NodeComposer.Create<bool>(),
                    (ISpecimenBuilder)NodeComposer.Create<AppDomainSetup>().Do(a),
                    new DelegatingSpecimenBuilder()));
            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
            // Teardown
        }

        [Fact]
        public void WithAutoPropertiesReturnsCorrectResult()
        {
            // Fixture setup
            var node = new CompositeSpecimenBuilder(
                new DelegatingSpecimenBuilder(),
                NodeComposer.Create<Version>(),
                NodeComposer.Create<Guid>(),
                NodeComposer.Create<Version>(),
                new DelegatingSpecimenBuilder());
            var sut = new CompositeNodeComposer<Version>(node);
            // Exercise system
            var actual = sut.WithAutoProperties();
            // Verify outcome
            var expected = new CompositeNodeComposer<Version>(
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    (ISpecimenBuilder)NodeComposer.Create<Version>().WithAutoProperties(),
                    NodeComposer.Create<Guid>(),
                    (ISpecimenBuilder)NodeComposer.Create<Version>().WithAutoProperties(),
                    new DelegatingSpecimenBuilder()));
            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
            // Teardown
        }

        [Fact]
        public void OmitAutoPropertiesReturnsCorrectResult()
        {
            // Fixture setup
            var node = new CompositeSpecimenBuilder(
                new DelegatingSpecimenBuilder(),
                NodeComposer.Create<Version>(),
                NodeComposer.Create<UTF8Encoding>(),
                NodeComposer.Create<Version>(),
                new DelegatingSpecimenBuilder());
            var sut = new CompositeNodeComposer<Version>(node);
            // Exercise system
            var actual = sut.OmitAutoProperties();
            // Verify outcome
            var expected = new CompositeNodeComposer<Version>(
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    (ISpecimenBuilder)NodeComposer.Create<Version>().OmitAutoProperties(),
                    NodeComposer.Create<UTF8Encoding>(),
                    (ISpecimenBuilder)NodeComposer.Create<Version>().OmitAutoProperties(),
                    new DelegatingSpecimenBuilder()));
            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
            // Teardown
        }

        [Fact]
        public void OmitAutoPropertiesAfterAddingAutoPropertiesReturnsCorrectResult()
        {
            // Fixture setup
            var node = new CompositeSpecimenBuilder(
                new DelegatingSpecimenBuilder(),
                NodeComposer.Create<Version>(),
                NodeComposer.Create<string>(),
                NodeComposer.Create<Version>(),
                new DelegatingSpecimenBuilder());
            var sut = new CompositeNodeComposer<Version>(node);
            // Exercise system
            var actual = sut.WithAutoProperties().OmitAutoProperties();
            // Verify outcome
            var expected = new CompositeNodeComposer<Version>(
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    (ISpecimenBuilder)NodeComposer.Create<Version>().WithAutoProperties().OmitAutoProperties(),
                    NodeComposer.Create<string>(),
                    (ISpecimenBuilder)NodeComposer.Create<Version>().WithAutoProperties().OmitAutoProperties(),
                    new DelegatingSpecimenBuilder()));
            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
            // Teardown
        }
    }
}
