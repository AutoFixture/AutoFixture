using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Dsl;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;

namespace Ploeh.AutoFixtureUnitTest.Dsl
{
    public class NodeComposerTest
    {
        [Fact]
        public void SutIsComposer()
        {
            // Fixture setup
            // Exercise system
            var sut = new NodeComposer<object>();
            // Verify outcome
            Assert.IsAssignableFrom<ICustomizationComposer<object>>(sut);
            // Teardown
        }

        [Fact]
        public void SutIsFilter()
        {
            // Fixture setup
            // Exercise system
            var sut = new NodeComposer<string>();
            // Verify outcome
            Assert.IsAssignableFrom<FilteringSpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void SutIsCorrectInitialGraph()
        {
            // Fixture setup
            var sut = new NodeComposer<int>();
            // Exercise system
            // Verify outcome
            var factory = new MethodInvoker(new ModestConstructorQuery());
            var expected = new TypedNode(typeof(int), factory);
            Assert.True(expected.GraphEquals(sut, new NodeComparer()));
            // Teardown
        }

        [Fact]
        public void FromSeedReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new NodeComposer<decimal>();
            Func<decimal, decimal> f = d => d;
            // Exercise system
            var actual = sut.FromSeed(f);
            // Verify outcome
            var factory = new SeededFactory<decimal>(f);
            var expected = new TypedNode(typeof(decimal), factory);

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
            // Teardown
        }

        [Fact]
        public void FromSpecimenBuilderFactoryReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new NodeComposer<Guid>();
            var builder = new DelegatingSpecimenBuilder();
            // Exercise system
            var actual = sut.FromFactory(builder);
            // Verify outcome
            var expected = new TypedNode(typeof(Guid), builder);

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
            // Teardown
        }

        [Fact]
        public void FromNoArgFuncReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new NodeComposer<long>();
            Func<long> f = () => 0;
            // Exercise system
            var actual = sut.FromFactory(f);
            // Verify outcome
            var factory = new SpecimenFactory<long>(f);
            var expected = new TypedNode(typeof(long), factory);

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
            // Teardown
        }

        [Fact]
        public void FromSingleArgFuncReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new NodeComposer<float>();
            Func<int, float> f = i => i;
            // Exercise system
            var actual = sut.FromFactory(f);
            // Verify outcome
            var factory = new SpecimenFactory<int, float>(f);
            var expected = new TypedNode(typeof(float), factory);

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
            // Teardown
        }

        [Fact]
        public void FromDoubleArgFuncReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new NodeComposer<string>();
            Func<int, Version, string> f = (i, _) => i.ToString();
            // Exercise system
            var actual = sut.FromFactory(f);
            // Verify outcome
            var factory = new SpecimenFactory<int, Version, string>(f);
            var expected = new TypedNode(typeof(string), factory);

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
            // Teardown
        }

        [Fact]
        public void FromTripleArgFuncReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new NodeComposer<decimal>();
            Func<int, Guid, Version, decimal> f = (i, g, v) => i;
            // Exercise system
            var actual = sut.FromFactory(f);
            // Verify outcome
            var factory = new SpecimenFactory<int, Guid, Version, decimal>(f);
            var expected = new TypedNode(typeof(decimal), factory);

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
            // Teardown
        }

        [Fact]
        public void FromQuadrupleArgFuncReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new NodeComposer<Version>();
            Func<int, int, int, int, Version> f = (mj, mn, b, r) => new Version(mj, mn, b, r);
            // Exercise system
            var actual = sut.FromFactory(f);
            // Verify outcome
            var factory = new SpecimenFactory<int, int, int, int, Version>(f);
            var expected = new TypedNode(typeof(Version), factory);

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
            // Teardown
        }
    }
}
