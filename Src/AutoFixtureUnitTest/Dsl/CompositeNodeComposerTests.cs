using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixture.Dsl;
using Ploeh.TestTypeFoundation;
using Xunit.Extensions;

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
#pragma warning disable 618
                    new NoSpecimen(r)
#pragma warning restore 618
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
                SpecimenBuilderNodeFactory.CreateComposer<int>(),
                SpecimenBuilderNodeFactory.CreateComposer<string>(),
                SpecimenBuilderNodeFactory.CreateComposer<int>(),
                new DelegatingSpecimenBuilder());
            var sut = new CompositeNodeComposer<int>(node);
            Func<int, int> f = i => i;
            // Exercise system
            var actual = sut.FromSeed(f);
            // Verify outcome
            var expected = new CompositeNodeComposer<int>(
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<int>().FromSeed(f),
                    SpecimenBuilderNodeFactory.CreateComposer<string>(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<int>().FromSeed(f),
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
                SpecimenBuilderNodeFactory.CreateComposer<decimal>(),
                SpecimenBuilderNodeFactory.CreateComposer<int>(),
                SpecimenBuilderNodeFactory.CreateComposer<decimal>(),
                new DelegatingSpecimenBuilder());
            var sut = new CompositeNodeComposer<decimal>(node);
            var factory = new DelegatingSpecimenBuilder();
            // Exercise system
            var actual = sut.FromFactory(factory);
            // Verify outcome
            var expected = new CompositeNodeComposer<decimal>(
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<decimal>().FromFactory(factory),
                    SpecimenBuilderNodeFactory.CreateComposer<int>(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<decimal>().FromFactory(factory),
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
                SpecimenBuilderNodeFactory.CreateComposer<Guid>(),
                SpecimenBuilderNodeFactory.CreateComposer<float>(),
                SpecimenBuilderNodeFactory.CreateComposer<Guid>(),
                new DelegatingSpecimenBuilder());
            var sut = new CompositeNodeComposer<Guid>(node);
            Func<Guid> f = () => Guid.Empty;
            // Exercise system
            var actual = sut.FromFactory(f);
            // Verify outcome
            var expected = new CompositeNodeComposer<Guid>(
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<Guid>().FromFactory(f),
                    SpecimenBuilderNodeFactory.CreateComposer<float>(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<Guid>().FromFactory(f),
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
                SpecimenBuilderNodeFactory.CreateComposer<string>(),
                SpecimenBuilderNodeFactory.CreateComposer<ulong>(),
                SpecimenBuilderNodeFactory.CreateComposer<string>(),
                new DelegatingSpecimenBuilder());
            var sut = new CompositeNodeComposer<string>(node);
            Func<int, string> f = i => i.ToString();
            // Exercise system
            var actual = sut.FromFactory(f);
            // Verify outcome
            var expected = new CompositeNodeComposer<string>(
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<string>().FromFactory(f),
                    SpecimenBuilderNodeFactory.CreateComposer<ulong>(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<string>().FromFactory(f),
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
                SpecimenBuilderNodeFactory.CreateComposer<Version>(),
                SpecimenBuilderNodeFactory.CreateComposer<string>(),
                SpecimenBuilderNodeFactory.CreateComposer<Version>(),
                new DelegatingSpecimenBuilder());
            var sut = new CompositeNodeComposer<Version>(node);
            Func<int, int, Version> f = (mj, mn) => new Version(mj, mn);
            // Exercise system
            var actual = sut.FromFactory(f);
            // Verify outcome
            var expected = new CompositeNodeComposer<Version>(
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<Version>().FromFactory(f),
                    SpecimenBuilderNodeFactory.CreateComposer<string>(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<Version>().FromFactory(f),
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
                SpecimenBuilderNodeFactory.CreateComposer<long>(),
                SpecimenBuilderNodeFactory.CreateComposer<Version>(),
                SpecimenBuilderNodeFactory.CreateComposer<long>(),
                new DelegatingSpecimenBuilder());
            var sut = new CompositeNodeComposer<long>(node);
            Func<long, int, short, long> f = (x, y, z) => x;
            // Exercise system
            var actual = sut.FromFactory(f);
            // Verify outcome
            var expected = new CompositeNodeComposer<long>(
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<long>().FromFactory(f),
                    SpecimenBuilderNodeFactory.CreateComposer<Version>(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<long>().FromFactory(f),
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
                SpecimenBuilderNodeFactory.CreateComposer<Version>(),
                SpecimenBuilderNodeFactory.CreateComposer<Guid>(),
                SpecimenBuilderNodeFactory.CreateComposer<Version>(),
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
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<Version>().FromFactory(f),
                    SpecimenBuilderNodeFactory.CreateComposer<Guid>(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<Version>().FromFactory(f),
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
                SpecimenBuilderNodeFactory.CreateComposer<AppDomainSetup>(),
                SpecimenBuilderNodeFactory.CreateComposer<bool>(),
                SpecimenBuilderNodeFactory.CreateComposer<AppDomainSetup>(),
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
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<AppDomainSetup>().Do(a),
                    SpecimenBuilderNodeFactory.CreateComposer<bool>(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<AppDomainSetup>().Do(a),
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
                SpecimenBuilderNodeFactory.CreateComposer<Version>(),
                SpecimenBuilderNodeFactory.CreateComposer<Guid>(),
                SpecimenBuilderNodeFactory.CreateComposer<Version>(),
                new DelegatingSpecimenBuilder());
            var sut = new CompositeNodeComposer<Version>(node);
            // Exercise system
            var actual = sut.WithAutoProperties();
            // Verify outcome
            var expected = new CompositeNodeComposer<Version>(
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<Version>().WithAutoProperties(),
                    SpecimenBuilderNodeFactory.CreateComposer<Guid>(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<Version>().WithAutoProperties(),
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
                SpecimenBuilderNodeFactory.CreateComposer<Version>(),
                SpecimenBuilderNodeFactory.CreateComposer<UTF8Encoding>(),
                SpecimenBuilderNodeFactory.CreateComposer<Version>(),
                new DelegatingSpecimenBuilder());
            var sut = new CompositeNodeComposer<Version>(node);
            // Exercise system
            var actual = sut.OmitAutoProperties();
            // Verify outcome
            var expected = new CompositeNodeComposer<Version>(
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<Version>().OmitAutoProperties(),
                    SpecimenBuilderNodeFactory.CreateComposer<UTF8Encoding>(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<Version>().OmitAutoProperties(),
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
                SpecimenBuilderNodeFactory.CreateComposer<Version>(),
                SpecimenBuilderNodeFactory.CreateComposer<string>(),
                SpecimenBuilderNodeFactory.CreateComposer<Version>(),
                new DelegatingSpecimenBuilder());
            var sut = new CompositeNodeComposer<Version>(node);
            // Exercise system
            var actual = sut.WithAutoProperties().OmitAutoProperties();
            // Verify outcome
            var expected = new CompositeNodeComposer<Version>(
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<Version>().WithAutoProperties().OmitAutoProperties(),
                    SpecimenBuilderNodeFactory.CreateComposer<string>(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<Version>().WithAutoProperties().OmitAutoProperties(),
                    new DelegatingSpecimenBuilder()));
            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
            // Teardown
        }

        [Fact]
        public void WithAnonymousValueReturnsCorrectResult()
        {
            // Fixture setup
            var node = new CompositeSpecimenBuilder(
                new DelegatingSpecimenBuilder(),
                SpecimenBuilderNodeFactory.CreateComposer<PropertyHolder<int>>(),
                SpecimenBuilderNodeFactory.CreateComposer<int>(),
                SpecimenBuilderNodeFactory.CreateComposer<PropertyHolder<int>>(),
                new DelegatingSpecimenBuilder());
            var sut = new CompositeNodeComposer<PropertyHolder<int>>(node);
            // Exercise system
            var actual = sut.With(x => x.Property);
            // Verify outcome
            var expected = new CompositeNodeComposer<PropertyHolder<int>>(
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<PropertyHolder<int>>().With(x => x.Property),
                    SpecimenBuilderNodeFactory.CreateComposer<int>(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<PropertyHolder<int>>().With(x => x.Property),
                    new DelegatingSpecimenBuilder()));
            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
            // Teardown
        }

        [Theory]
        [InlineData("")]
        [InlineData("foo")]
        [InlineData("bar")]
        public void WithExplicitValueReturnsCorrectResult(string value)
        {
            // Fixture setup
            var node = new CompositeSpecimenBuilder(
                new DelegatingSpecimenBuilder(),
                SpecimenBuilderNodeFactory.CreateComposer<PropertyHolder<string>>(),
                SpecimenBuilderNodeFactory.CreateComposer<Version>(),
                SpecimenBuilderNodeFactory.CreateComposer<PropertyHolder<string>>(),
                new DelegatingSpecimenBuilder());
            var sut = new CompositeNodeComposer<PropertyHolder<string>>(node);
            // Exercise system
            var actual = sut.With(x => x.Property, value);
            // Verify outcome
            var expected = new CompositeNodeComposer<PropertyHolder<string>>(
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<PropertyHolder<string>>().With(x => x.Property, value),
                    SpecimenBuilderNodeFactory.CreateComposer<Version>(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<PropertyHolder<string>>().With(x => x.Property, value),
                    new DelegatingSpecimenBuilder()));
            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
            // Teardown
        }

        [Theory]
        [InlineData("")]
        [InlineData("foo")]
        [InlineData("bar")]
        public void WithFactoryReturnsCorrectResult(string value)
        {
            // Fixture setup
            var node = new CompositeSpecimenBuilder(
                new DelegatingSpecimenBuilder(),
                SpecimenBuilderNodeFactory.CreateComposer<PropertyHolder<string>>(),
                SpecimenBuilderNodeFactory.CreateComposer<Version>(),
                SpecimenBuilderNodeFactory.CreateComposer<PropertyHolder<string>>(),
                new DelegatingSpecimenBuilder());
            Func<string> factory = () => value;
            var sut = new CompositeNodeComposer<PropertyHolder<string>>(node);
            // Exercise system
            var actual = sut.With(x => x.Property, factory);
            // Verify outcome
            var expected = new CompositeNodeComposer<PropertyHolder<string>>(
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    SpecimenBuilderNodeFactory.CreateComposer<PropertyHolder<string>>().With(x => x.Property, factory),
                    SpecimenBuilderNodeFactory.CreateComposer<Version>(),
                    SpecimenBuilderNodeFactory.CreateComposer<PropertyHolder<string>>().With(x => x.Property, factory),
                    new DelegatingSpecimenBuilder()));
            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
            // Teardown
        }

        [Fact]
        public void WithoutReturnsCorrectResult()
        {
            // Fixture setup
            var node = new CompositeSpecimenBuilder(
                new DelegatingSpecimenBuilder(),
                SpecimenBuilderNodeFactory.CreateComposer<FieldHolder<short>>(),
                SpecimenBuilderNodeFactory.CreateComposer<PropertyHolder<int>>(),
                SpecimenBuilderNodeFactory.CreateComposer<FieldHolder<short>>(),
                new DelegatingSpecimenBuilder());
            var sut = new CompositeNodeComposer<FieldHolder<short>>(node);
            // Exercise system
            var actual = sut.Without(x => x.Field);
            // Verify outcome
            var expected = new CompositeNodeComposer<FieldHolder<short>>(
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<FieldHolder<short>>().Without(x => x.Field),
                    SpecimenBuilderNodeFactory.CreateComposer<PropertyHolder<int>>(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<FieldHolder<short>>().Without(x => x.Field),
                    new DelegatingSpecimenBuilder()));
            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
            // Teardown
        }
    }
}
