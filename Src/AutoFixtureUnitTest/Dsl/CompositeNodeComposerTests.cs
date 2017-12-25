using System;
using System.Linq;
using System.Text;
using AutoFixture.Dsl;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Dsl
{
    public class CompositeNodeComposerTests
    {
        [Fact]
        public void SutIsComposer()
        {
            // Arrange
            var dummyNode = new CompositeSpecimenBuilder();
            // Act
            var sut = new CompositeNodeComposer<object>(dummyNode);
            // Assert
            Assert.IsAssignableFrom<ICustomizationComposer<object>>(sut);
        }

        [Fact]
        public void SutIsSpecimenBuilderNode()
        {
            // Arrange
            var dummyNode = new CompositeSpecimenBuilder();
            // Act
            var sut = new CompositeNodeComposer<float>(dummyNode);
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilderNode>(sut);
        }

        [Fact]
        public void ComposeReturnsCorrectResult()
        {
            // Arrange
            var dummyNode = new CompositeSpecimenBuilder();
            var sut = new CompositeNodeComposer<uint>(dummyNode);
            // Act
            var expectedBuilders = new[]
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            };
            var actual = sut.Compose(expectedBuilders);
            // Assert
            var c =
                Assert.IsAssignableFrom<CompositeNodeComposer<uint>>(actual);
            var composite =
                Assert.IsAssignableFrom<CompositeSpecimenBuilder>(c.Node);
            Assert.True(expectedBuilders.SequenceEqual(composite));
        }

        [Fact]
        public void ComposeSingleNodeReturnsCorrectResult()
        {
            // Arrange
            var dummyNode = new CompositeSpecimenBuilder();
            var sut = new CompositeNodeComposer<uint>(dummyNode);
            ISpecimenBuilderNode expected = new CompositeSpecimenBuilder();
            // Act
            var actual = sut.Compose(new[] { expected });
            // Assert
            var c =
                Assert.IsAssignableFrom<CompositeNodeComposer<uint>>(actual);
            Assert.Equal(expected, c.Node);
        }

        [Fact]
        public void CreateReturnsCorrectResult()
        {
            // Arrange
            var request = new object();
            var context = new DelegatingSpecimenContext();
            var expected = new object();
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) => r == request && c == context ?
                    expected :
                    new NoSpecimen()
            };
            var sut = new CompositeNodeComposer<ushort>(
                new CompositeSpecimenBuilder(
                    builder));
            // Act
            var actual = sut.Create(request, context);
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SutYieldsDecoratedBuilder()
        {
            // Arrange
            var expected = new CompositeSpecimenBuilder();
            // Act
            var sut = new CompositeNodeComposer<uint>(expected);
            // Assert
            Assert.True(new[] { expected }.SequenceEqual(sut));
            Assert.True(new object[] { expected }.SequenceEqual(
                ((System.Collections.IEnumerable)sut).Cast<object>()));
        }

        [Fact]
        public void NodeIsCorrect()
        {
            // Arrange
            var expected = new CompositeSpecimenBuilder();
            var sut = new CompositeNodeComposer<string>(expected);
            // Act
            ISpecimenBuilderNode actual = sut.Node;
            // Assert
            Assert.Equal(expected, actual);
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
            // Arrange
            var node = new CompositeSpecimenBuilder(
                new DelegatingSpecimenBuilder(),
                SpecimenBuilderNodeFactory.CreateComposer<int>(),
                SpecimenBuilderNodeFactory.CreateComposer<string>(),
                SpecimenBuilderNodeFactory.CreateComposer<int>(),
                new DelegatingSpecimenBuilder());
            var sut = new CompositeNodeComposer<int>(node);
            Func<int, int> f = i => i;
            // Act
            var actual = sut.FromSeed(f);
            // Assert
            var expected = new CompositeNodeComposer<int>(
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<int>().FromSeed(f),
                    SpecimenBuilderNodeFactory.CreateComposer<string>(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<int>().FromSeed(f),
                    new DelegatingSpecimenBuilder()));
            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
        }

        [Fact]
        public void FromSpecimenBuilderFactoryReturnsCorrectResult()
        {
            // Arrange
            var node = new CompositeSpecimenBuilder(
                new DelegatingSpecimenBuilder(),
                SpecimenBuilderNodeFactory.CreateComposer<decimal>(),
                SpecimenBuilderNodeFactory.CreateComposer<int>(),
                SpecimenBuilderNodeFactory.CreateComposer<decimal>(),
                new DelegatingSpecimenBuilder());
            var sut = new CompositeNodeComposer<decimal>(node);
            var factory = new DelegatingSpecimenBuilder();
            // Act
            var actual = sut.FromFactory(factory);
            // Assert
            var expected = new CompositeNodeComposer<decimal>(
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<decimal>().FromFactory(factory),
                    SpecimenBuilderNodeFactory.CreateComposer<int>(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<decimal>().FromFactory(factory),
                    new DelegatingSpecimenBuilder()));
            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
        }

        [Fact]
        public void FromNoArgFuncReturnsCorrectResult()
        {
            // Arrange
            var node = new CompositeSpecimenBuilder(
                new DelegatingSpecimenBuilder(),
                SpecimenBuilderNodeFactory.CreateComposer<Guid>(),
                SpecimenBuilderNodeFactory.CreateComposer<float>(),
                SpecimenBuilderNodeFactory.CreateComposer<Guid>(),
                new DelegatingSpecimenBuilder());
            var sut = new CompositeNodeComposer<Guid>(node);
            Func<Guid> f = () => Guid.Empty;
            // Act
            var actual = sut.FromFactory(f);
            // Assert
            var expected = new CompositeNodeComposer<Guid>(
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<Guid>().FromFactory(f),
                    SpecimenBuilderNodeFactory.CreateComposer<float>(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<Guid>().FromFactory(f),
                    new DelegatingSpecimenBuilder()));
            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
        }

        [Fact]
        public void FromSingleArgFuncReturnsCorrectResult()
        {
            // Arrange
            var node = new CompositeSpecimenBuilder(
                new DelegatingSpecimenBuilder(),
                SpecimenBuilderNodeFactory.CreateComposer<string>(),
                SpecimenBuilderNodeFactory.CreateComposer<ulong>(),
                SpecimenBuilderNodeFactory.CreateComposer<string>(),
                new DelegatingSpecimenBuilder());
            var sut = new CompositeNodeComposer<string>(node);
            Func<int, string> f = i => i.ToString();
            // Act
            var actual = sut.FromFactory(f);
            // Assert
            var expected = new CompositeNodeComposer<string>(
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<string>().FromFactory(f),
                    SpecimenBuilderNodeFactory.CreateComposer<ulong>(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<string>().FromFactory(f),
                    new DelegatingSpecimenBuilder()));
            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
        }

        [Fact]
        public void FromDoubleArgFuncReturnsCorrectResult()
        {
            // Arrange
            var node = new CompositeSpecimenBuilder(
                new DelegatingSpecimenBuilder(),
                SpecimenBuilderNodeFactory.CreateComposer<Version>(),
                SpecimenBuilderNodeFactory.CreateComposer<string>(),
                SpecimenBuilderNodeFactory.CreateComposer<Version>(),
                new DelegatingSpecimenBuilder());
            var sut = new CompositeNodeComposer<Version>(node);
            Func<int, int, Version> f = (mj, mn) => new Version(mj, mn);
            // Act
            var actual = sut.FromFactory(f);
            // Assert
            var expected = new CompositeNodeComposer<Version>(
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<Version>().FromFactory(f),
                    SpecimenBuilderNodeFactory.CreateComposer<string>(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<Version>().FromFactory(f),
                    new DelegatingSpecimenBuilder()));
            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
        }

        [Fact]
        public void FromTripleArgFuncReturnsCorrectResult()
        {
            // Arrange
            var node = new CompositeSpecimenBuilder(
                new DelegatingSpecimenBuilder(),
                SpecimenBuilderNodeFactory.CreateComposer<long>(),
                SpecimenBuilderNodeFactory.CreateComposer<Version>(),
                SpecimenBuilderNodeFactory.CreateComposer<long>(),
                new DelegatingSpecimenBuilder());
            var sut = new CompositeNodeComposer<long>(node);
            Func<long, int, short, long> f = (x, y, z) => x;
            // Act
            var actual = sut.FromFactory(f);
            // Assert
            var expected = new CompositeNodeComposer<long>(
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<long>().FromFactory(f),
                    SpecimenBuilderNodeFactory.CreateComposer<Version>(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<long>().FromFactory(f),
                    new DelegatingSpecimenBuilder()));
            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
        }

        [Fact]
        public void FromQuadrupleArgFuncReturnsCorrectResult()
        {
            // Arrange
            var node = new CompositeSpecimenBuilder(
                new DelegatingSpecimenBuilder(),
                SpecimenBuilderNodeFactory.CreateComposer<Version>(),
                SpecimenBuilderNodeFactory.CreateComposer<Guid>(),
                SpecimenBuilderNodeFactory.CreateComposer<Version>(),
                new DelegatingSpecimenBuilder());
            var sut = new CompositeNodeComposer<Version>(node);
            Func<int, int, int, int, Version> f =
                (mj, mn, b, r) => new Version(mj, mn, b, r);
            // Act
            var actual = sut.FromFactory(f);
            // Assert
            var expected = new CompositeNodeComposer<Version>(
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<Version>().FromFactory(f),
                    SpecimenBuilderNodeFactory.CreateComposer<Guid>(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<Version>().FromFactory(f),
                    new DelegatingSpecimenBuilder()));
            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
        }

        [Fact]
        public void LegacyComposeReturnsCorrectResult()
        {
            // Arrange
            var dummyNode = new CompositeSpecimenBuilder();
            var sut = new CompositeNodeComposer<UTF8Encoding>(dummyNode);
            // Act
            var actual = sut.Compose();
            // Assert
            Assert.Equal(sut, actual);
        }

        [Fact]
        public void DoReturnsCorrectResult()
        {
            // Arrange
            var node = new CompositeSpecimenBuilder(
                new DelegatingSpecimenBuilder(),
                SpecimenBuilderNodeFactory.CreateComposer<ConcreteType>(),
                SpecimenBuilderNodeFactory.CreateComposer<bool>(),
                SpecimenBuilderNodeFactory.CreateComposer<ConcreteType>(),
                new DelegatingSpecimenBuilder());
            var sut = new CompositeNodeComposer<ConcreteType>(node);
            Action<ConcreteType> a =
                ads => ads.Property1 = null;
            // Act
            var actual = sut.Do(a);
            // Assert
            var expected = new CompositeNodeComposer<ConcreteType>(
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<ConcreteType>().Do(a),
                    SpecimenBuilderNodeFactory.CreateComposer<bool>(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<ConcreteType>().Do(a),
                    new DelegatingSpecimenBuilder()));
            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
        }

        [Fact]
        public void WithAutoPropertiesReturnsCorrectResult()
        {
            // Arrange
            var node = new CompositeSpecimenBuilder(
                new DelegatingSpecimenBuilder(),
                SpecimenBuilderNodeFactory.CreateComposer<Version>(),
                SpecimenBuilderNodeFactory.CreateComposer<Guid>(),
                SpecimenBuilderNodeFactory.CreateComposer<Version>(),
                new DelegatingSpecimenBuilder());
            var sut = new CompositeNodeComposer<Version>(node);
            // Act
            var actual = sut.WithAutoProperties();
            // Assert
            var expected = new CompositeNodeComposer<Version>(
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<Version>().WithAutoProperties(),
                    SpecimenBuilderNodeFactory.CreateComposer<Guid>(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<Version>().WithAutoProperties(),
                    new DelegatingSpecimenBuilder()));
            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
        }

        [Fact]
        public void OmitAutoPropertiesReturnsCorrectResult()
        {
            // Arrange
            var node = new CompositeSpecimenBuilder(
                new DelegatingSpecimenBuilder(),
                SpecimenBuilderNodeFactory.CreateComposer<Version>(),
                SpecimenBuilderNodeFactory.CreateComposer<UTF8Encoding>(),
                SpecimenBuilderNodeFactory.CreateComposer<Version>(),
                new DelegatingSpecimenBuilder());
            var sut = new CompositeNodeComposer<Version>(node);
            // Act
            var actual = sut.OmitAutoProperties();
            // Assert
            var expected = new CompositeNodeComposer<Version>(
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<Version>().OmitAutoProperties(),
                    SpecimenBuilderNodeFactory.CreateComposer<UTF8Encoding>(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<Version>().OmitAutoProperties(),
                    new DelegatingSpecimenBuilder()));
            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
        }

        [Fact]
        public void OmitAutoPropertiesAfterAddingAutoPropertiesReturnsCorrectResult()
        {
            // Arrange
            var node = new CompositeSpecimenBuilder(
                new DelegatingSpecimenBuilder(),
                SpecimenBuilderNodeFactory.CreateComposer<Version>(),
                SpecimenBuilderNodeFactory.CreateComposer<string>(),
                SpecimenBuilderNodeFactory.CreateComposer<Version>(),
                new DelegatingSpecimenBuilder());
            var sut = new CompositeNodeComposer<Version>(node);
            // Act
            var actual = sut.WithAutoProperties().OmitAutoProperties();
            // Assert
            var expected = new CompositeNodeComposer<Version>(
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<Version>().WithAutoProperties().OmitAutoProperties(),
                    SpecimenBuilderNodeFactory.CreateComposer<string>(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<Version>().WithAutoProperties().OmitAutoProperties(),
                    new DelegatingSpecimenBuilder()));
            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
        }

        [Fact]
        public void WithAnonymousValueReturnsCorrectResult()
        {
            // Arrange
            var node = new CompositeSpecimenBuilder(
                new DelegatingSpecimenBuilder(),
                SpecimenBuilderNodeFactory.CreateComposer<PropertyHolder<int>>(),
                SpecimenBuilderNodeFactory.CreateComposer<int>(),
                SpecimenBuilderNodeFactory.CreateComposer<PropertyHolder<int>>(),
                new DelegatingSpecimenBuilder());
            var sut = new CompositeNodeComposer<PropertyHolder<int>>(node);
            // Act
            var actual = sut.With(x => x.Property);
            // Assert
            var expected = new CompositeNodeComposer<PropertyHolder<int>>(
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<PropertyHolder<int>>().With(x => x.Property),
                    SpecimenBuilderNodeFactory.CreateComposer<int>(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<PropertyHolder<int>>().With(x => x.Property),
                    new DelegatingSpecimenBuilder()));
            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
        }

        [Theory]
        [InlineData("")]
        [InlineData("foo")]
        [InlineData("bar")]
        public void WithExplicitValueReturnsCorrectResult(string value)
        {
            // Arrange
            var node = new CompositeSpecimenBuilder(
                new DelegatingSpecimenBuilder(),
                SpecimenBuilderNodeFactory.CreateComposer<PropertyHolder<string>>(),
                SpecimenBuilderNodeFactory.CreateComposer<Version>(),
                SpecimenBuilderNodeFactory.CreateComposer<PropertyHolder<string>>(),
                new DelegatingSpecimenBuilder());
            var sut = new CompositeNodeComposer<PropertyHolder<string>>(node);
            // Act
            var actual = sut.With(x => x.Property, value);
            // Assert
            var expected = new CompositeNodeComposer<PropertyHolder<string>>(
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<PropertyHolder<string>>().With(x => x.Property, value),
                    SpecimenBuilderNodeFactory.CreateComposer<Version>(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<PropertyHolder<string>>().With(x => x.Property, value),
                    new DelegatingSpecimenBuilder()));
            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
        }

        [Fact]
        public void WithoutReturnsCorrectResult()
        {
            // Arrange
            var node = new CompositeSpecimenBuilder(
                new DelegatingSpecimenBuilder(),
                SpecimenBuilderNodeFactory.CreateComposer<FieldHolder<short>>(),
                SpecimenBuilderNodeFactory.CreateComposer<PropertyHolder<int>>(),
                SpecimenBuilderNodeFactory.CreateComposer<FieldHolder<short>>(),
                new DelegatingSpecimenBuilder());
            var sut = new CompositeNodeComposer<FieldHolder<short>>(node);
            // Act
            var actual = sut.Without(x => x.Field);
            // Assert
            var expected = new CompositeNodeComposer<FieldHolder<short>>(
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<FieldHolder<short>>().Without(x => x.Field),
                    SpecimenBuilderNodeFactory.CreateComposer<PropertyHolder<int>>(),
                    (ISpecimenBuilder)SpecimenBuilderNodeFactory.CreateComposer<FieldHolder<short>>().Without(x => x.Field),
                    new DelegatingSpecimenBuilder()));
            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
        }
    }
}
