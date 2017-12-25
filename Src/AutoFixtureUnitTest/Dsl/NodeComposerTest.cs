using System;
using System.Linq;
using System.Reflection;
using System.Text;
using AutoFixture.Dsl;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Dsl
{
    public class NodeComposerTest
    {
        [Fact]
        public void SutIsComposer()
        {
            // Arrange
            // Act
            var sut = SpecimenBuilderNodeFactory.CreateComposer<object>();
            // Assert
            Assert.IsAssignableFrom<ICustomizationComposer<object>>(sut);
        }

        [Fact]
        public void SutIsSpecimenBuilderNode()
        {
            // Arrange
            // Act
            var sut = SpecimenBuilderNodeFactory.CreateComposer<string>();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilderNode>(sut);
        }

        [Fact]
        public void SutYieldsDecoratedBuilder()
        {
            // Arrange
            var expected = new DelegatingSpecimenBuilder();
            // Act
            var sut = new NodeComposer<Guid>(expected);
            // Assert
            Assert.True(new[] { expected }.SequenceEqual(sut));
            Assert.True(new object[] { expected }.SequenceEqual(
                ((System.Collections.IEnumerable)sut).Cast<object>()));
        }

        [Fact]
        public void BuilderIsCorrect()
        {
            // Arrange
            var expected = new DelegatingSpecimenBuilder();
            var sut = new NodeComposer<Guid>(expected);
            // Act
            ISpecimenBuilder actual = sut.Builder;
            // Assert
            Assert.Equal(expected, actual);
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
            var sut = new NodeComposer<object>(builder);
            // Act
            var actual = sut.Create(request, context);
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SutIsCorrectInitialGraph()
        {
            // Arrange
            var sut = SpecimenBuilderNodeFactory.CreateComposer<int>();
            // Act
            // Assert
            var factory = new MethodInvoker(new ModestConstructorQuery());
            var expected = new NodeComposer<int>(
                SpecimenBuilderNodeFactory.CreateTypedNode(typeof(int), factory));
            Assert.True(expected.GraphEquals(sut, new NodeComparer()));
        }

        [Fact]
        public void FromSeedReturnsCorrectResult()
        {
            // Arrange
            var sut = SpecimenBuilderNodeFactory.CreateComposer<decimal>();
            Func<decimal, decimal> f = d => d;
            // Act
            var actual = sut.FromSeed(f);
            // Assert
            var factory = new SeededFactory<decimal>(f);
            var expected = new NodeComposer<decimal>(
                SpecimenBuilderNodeFactory.CreateTypedNode(typeof(decimal), factory));

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
        }

        [Fact]
        public void FromSpecimenBuilderFactoryReturnsCorrectResult()
        {
            // Arrange
            var sut = SpecimenBuilderNodeFactory.CreateComposer<Guid>();
            var builder = new DelegatingSpecimenBuilder();
            // Act
            var actual = sut.FromFactory(builder);
            // Assert
            var expected = new NodeComposer<Guid>(
                SpecimenBuilderNodeFactory.CreateTypedNode(typeof(Guid), builder));

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
        }

        [Fact]
        public void FromNoArgFuncReturnsCorrectResult()
        {
            // Arrange
            var sut = SpecimenBuilderNodeFactory.CreateComposer<long>();
            Func<long> f = () => 0;
            // Act
            var actual = sut.FromFactory(f);
            // Assert
            var factory = new SpecimenFactory<long>(f);
            var expected = new NodeComposer<long>(
                SpecimenBuilderNodeFactory.CreateTypedNode(typeof(long), factory));

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
        }

        [Fact]
        public void FromSingleArgFuncReturnsCorrectResult()
        {
            // Arrange
            var sut = SpecimenBuilderNodeFactory.CreateComposer<float>();
            Func<int, float> f = i => i;
            // Act
            var actual = sut.FromFactory(f);
            // Assert
            var factory = new SpecimenFactory<int, float>(f);
            var expected = new NodeComposer<float>(
                SpecimenBuilderNodeFactory.CreateTypedNode(typeof(float), factory));

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
        }

        [Fact]
        public void FromDoubleArgFuncReturnsCorrectResult()
        {
            // Arrange
            var sut = SpecimenBuilderNodeFactory.CreateComposer<string>();
            Func<int, Version, string> f = (i, _) => i.ToString();
            // Act
            var actual = sut.FromFactory(f);
            // Assert
            var factory = new SpecimenFactory<int, Version, string>(f);
            var expected = new NodeComposer<string>(
                SpecimenBuilderNodeFactory.CreateTypedNode(typeof(string), factory));

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
        }

        [Fact]
        public void FromTripleArgFuncReturnsCorrectResult()
        {
            // Arrange
            var sut = SpecimenBuilderNodeFactory.CreateComposer<decimal>();
            Func<int, Guid, Version, decimal> f = (i, g, v) => i;
            // Act
            var actual = sut.FromFactory(f);
            // Assert
            var factory = new SpecimenFactory<int, Guid, Version, decimal>(f);
            var expected = new NodeComposer<decimal>(
                SpecimenBuilderNodeFactory.CreateTypedNode(typeof(decimal), factory));

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
        }

        [Fact]
        public void FromQuadrupleArgFuncReturnsCorrectResult()
        {
            // Arrange
            var sut = SpecimenBuilderNodeFactory.CreateComposer<Version>();
            Func<int, int, int, int, Version> f = (mj, mn, b, r) => new Version(mj, mn, b, r);
            // Act
            var actual = sut.FromFactory(f);
            // Assert
            var factory = new SpecimenFactory<int, int, int, int, Version>(f);
            var expected = new NodeComposer<Version>(
                SpecimenBuilderNodeFactory.CreateTypedNode(typeof(Version), factory));

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
        }

        [Fact]
        public void LegacyComposeReturnsCorrectResult()
        {
            // Arrange
            var sut = SpecimenBuilderNodeFactory.CreateComposer<UTF8Encoding>();
            // Act
            var actual = sut.Compose();
            // Assert
            Assert.Equal(sut, actual);
        }

        [Fact]
        public void ComposeReturnsCorrectResult()
        {
            // Arrange
            var sut = SpecimenBuilderNodeFactory.CreateComposer<ConcreteType>();
            // Act
            var expectedBuilders = new[]
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            };
            var actual = sut.Compose(expectedBuilders);
            // Assert
            var nc = Assert.IsAssignableFrom<NodeComposer<ConcreteType>>(actual);
            var composite = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(nc.Builder);
            Assert.True(expectedBuilders.SequenceEqual(composite));
        }

        [Fact]
        public void ComposeSingleItemReturnsCorrectResult()
        {
            // Arrange
            var sut = SpecimenBuilderNodeFactory.CreateComposer<ConcreteType>();
            var expected = new DelegatingSpecimenBuilder();
            // Act
            var actual = sut.Compose(new[] { expected });
            // Assert
            var f = Assert.IsAssignableFrom<NodeComposer<ConcreteType>>(actual);
            Assert.Equal(expected, f.Builder);
        }

        [Fact]
        public void DoReturnsCorrectResult()
        {
            // Arrange
            var sut = SpecimenBuilderNodeFactory.CreateComposer<ConcreteType>();
            Action<ConcreteType> a = ads => ads.Property1 = 42;
            // Act
            var actual = sut.Do(a);
            // Assert
            var expected = new NodeComposer<ConcreteType>(
                new FilteringSpecimenBuilder(
                    new CompositeSpecimenBuilder(
                        new Postprocessor(
                            new NoSpecimenOutputGuard(
                                new MethodInvoker(
                                    new ModestConstructorQuery()),
                                new InverseRequestSpecification(
                                    new SeedRequestSpecification(
                                        typeof(ConcreteType)))),
                            new ActionSpecimenCommand<ConcreteType>(a)),
                        new SeedIgnoringRelay()),
                    new OrRequestSpecification(
                        new SeedRequestSpecification(typeof(ConcreteType)),
                        new ExactTypeSpecification(typeof(ConcreteType)))));

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
        }

        [Fact]
        public void SecondDoReturnsCorrectResult()
        {
            // Arrange
            var sut = SpecimenBuilderNodeFactory.CreateComposer<PropertyHolder<string>>();
            Action<PropertyHolder<string>> dummy = _ => { };
            Action<PropertyHolder<string>> a = ph => ph.Property = "";
            // Act
            var actual = sut.Do(dummy).Do(a);
            // Assert
            var expected = new NodeComposer<PropertyHolder<string>>(
                new FilteringSpecimenBuilder(
                    new CompositeSpecimenBuilder(
                        new Postprocessor(
                            new Postprocessor(
                                new NoSpecimenOutputGuard(
                                    new MethodInvoker(
                                        new ModestConstructorQuery()),
                                    new InverseRequestSpecification(
                                        new SeedRequestSpecification(
                                            typeof(PropertyHolder<string>)))),
                                new ActionSpecimenCommand<PropertyHolder<string>>(dummy)),
                            new ActionSpecimenCommand<PropertyHolder<string>>(a)),
                        new SeedIgnoringRelay()),
                    new OrRequestSpecification(
                        new SeedRequestSpecification(typeof(PropertyHolder<string>)),
                        new ExactTypeSpecification(typeof(PropertyHolder<string>)))));

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
        }

        [Fact]
        public void WithAutoPropertiesReturnsCorrectResult()
        {
            // Arrange
            var sut = SpecimenBuilderNodeFactory.CreateComposer<Version>();
            // Act
            var actual = sut.WithAutoProperties();
            // Assert
            var expected = new NodeComposer<Version>(
                new FilteringSpecimenBuilder(
                    new CompositeSpecimenBuilder(
                        new Postprocessor(
                            new NoSpecimenOutputGuard(
                                new MethodInvoker(
                                    new ModestConstructorQuery()),
                                new InverseRequestSpecification(
                                    new SeedRequestSpecification(
                                        typeof(Version)))),
                            new AutoPropertiesCommand(typeof(Version)),
                            new TrueRequestSpecification()),
                        new SeedIgnoringRelay()),
                    new OrRequestSpecification(
                        new SeedRequestSpecification(typeof(Version)),
                        new ExactTypeSpecification(typeof(Version)))));

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
        }

        [Fact]
        public void OmitAutoPropertiesIsInitiallyANoOp()
        {
            // Arrange
            var sut = SpecimenBuilderNodeFactory.CreateComposer<Version>();
            // Act
            var actual = sut.OmitAutoProperties();
            // Assert
            var expected = sut;

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
        }

        [Fact]
        public void OmitAutoPropertiesAfterAddingAutoPropertiesReturnsCorrectResult()
        {
            // Arrange
            var sut = SpecimenBuilderNodeFactory.CreateComposer<Version>();
            // Act
            var actual = sut.WithAutoProperties().OmitAutoProperties();
            // Assert
            var expected = new NodeComposer<Version>(
                new FilteringSpecimenBuilder(
                    new CompositeSpecimenBuilder(
                        new Postprocessor(
                            new NoSpecimenOutputGuard(
                                new MethodInvoker(
                                    new ModestConstructorQuery()),
                                new InverseRequestSpecification(
                                    new SeedRequestSpecification(
                                        typeof(Version)))),
                            new AutoPropertiesCommand(typeof(Version)),
                            new FalseRequestSpecification()),
                        new SeedIgnoringRelay()),
                    new OrRequestSpecification(
                        new SeedRequestSpecification(typeof(Version)),
                        new ExactTypeSpecification(typeof(Version)))));

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
        }

        [Fact]
        public void WithAnonymousValueReturnsCorrectResult()
        {
            // Arrange
            var sut = SpecimenBuilderNodeFactory.CreateComposer<PropertyHolder<int>>();
            // Act
            var actual = sut.With(x => x.Property);
            // Assert
            var expected = new NodeComposer<PropertyHolder<int>>(
                new FilteringSpecimenBuilder(
                    new CompositeSpecimenBuilder(
                        new Postprocessor(
                            new NoSpecimenOutputGuard(
                                new MethodInvoker(
                                    new ModestConstructorQuery()),
                                new InverseRequestSpecification(
                                    new SeedRequestSpecification(
                                        typeof(PropertyHolder<int>)))),
                            new BindingCommand<PropertyHolder<int>, int>(x => x.Property),
                            new OrRequestSpecification(
                                new SeedRequestSpecification(typeof(PropertyHolder<int>)),
                                new ExactTypeSpecification(typeof(PropertyHolder<int>)))),
                        new SeedIgnoringRelay()),
                    new OrRequestSpecification(
                        new SeedRequestSpecification(typeof(PropertyHolder<int>)),
                        new ExactTypeSpecification(typeof(PropertyHolder<int>)))));

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
            var sut = SpecimenBuilderNodeFactory.CreateComposer<PropertyHolder<string>>();
            var pi = typeof(PropertyHolder<string>).GetProperty("Property");
            // Act
            var actual = sut.With(x => x.Property, value);
            // Assert
            var expected = new NodeComposer<PropertyHolder<string>>(
                new FilteringSpecimenBuilder(
                    new CompositeSpecimenBuilder(
                        new Postprocessor(
                            new Postprocessor(
                                new NoSpecimenOutputGuard(
                                    new MethodInvoker(
                                        new ModestConstructorQuery()),
                                    new InverseRequestSpecification(
                                        new SeedRequestSpecification(
                                            typeof(PropertyHolder<string>)))),
                                new AutoPropertiesCommand(
                                    typeof(PropertyHolder<string>),
                                    new InverseRequestSpecification(
                                        new EqualRequestSpecification(
                                            pi,
                                            new MemberInfoEqualityComparer()))),
                                new FalseRequestSpecification()
                            ),
                            new BindingCommand<PropertyHolder<string>, string>(x => x.Property, value),
                            new OrRequestSpecification(
                                new SeedRequestSpecification(typeof(PropertyHolder<string>)),
                                new ExactTypeSpecification(typeof(PropertyHolder<string>)))),
                        new SeedIgnoringRelay()),
                    new OrRequestSpecification(
                        new SeedRequestSpecification(typeof(PropertyHolder<string>)),
                        new ExactTypeSpecification(typeof(PropertyHolder<string>)))));

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
        }

        [Theory]
        [InlineData("", 1)]
        [InlineData("bar", 0)]
        [InlineData("foo", 42)]
        [InlineData("bar", -1)]
        public void SuccessiveWithValueReturnsCorrectResult(
            string value1,
            int value2)
        {
            // Arrange
            var sut =
                SpecimenBuilderNodeFactory.CreateComposer<DoublePropertyHolder<string, int>>();
            var pi1 = typeof(DoublePropertyHolder<string, int>).GetProperty("Property1");
            var pi2 = typeof(DoublePropertyHolder<string, int>).GetProperty("Property2");
            // Act
            var actual = sut
                .With(x => x.Property1, value1)
                .With(x => x.Property2, value2);
            // Assert
            var expected = new NodeComposer<DoublePropertyHolder<string, int>>(
                new FilteringSpecimenBuilder(
                    new CompositeSpecimenBuilder(
                        new Postprocessor(
                            new Postprocessor(
                                new Postprocessor(
                                    new NoSpecimenOutputGuard(
                                        new MethodInvoker(
                                            new ModestConstructorQuery()),
                                        new InverseRequestSpecification(
                                            new SeedRequestSpecification(
                                                typeof(DoublePropertyHolder<string, int>)))),
                                    new AutoPropertiesCommand(
                                        typeof(DoublePropertyHolder<string, int>),
                                        new AndRequestSpecification(
                                            new InverseRequestSpecification(
                                                new EqualRequestSpecification(
                                                    pi1,
                                                    new MemberInfoEqualityComparer())),
                                            new InverseRequestSpecification(
                                                new EqualRequestSpecification(
                                                    pi2,
                                                    new MemberInfoEqualityComparer())))),
                                    new FalseRequestSpecification()),
                                new BindingCommand<DoublePropertyHolder<string, int>, string>(x => x.Property1, value1),
                                new OrRequestSpecification(
                                    new SeedRequestSpecification(typeof(DoublePropertyHolder<string, int>)),
                                    new ExactTypeSpecification(typeof(DoublePropertyHolder<string, int>)))),
                            new BindingCommand<DoublePropertyHolder<string, int>, int>(x => x.Property2, value2),
                            new OrRequestSpecification(
                                new SeedRequestSpecification(typeof(DoublePropertyHolder<string, int>)),
                                new ExactTypeSpecification(typeof(DoublePropertyHolder<string, int>)))),
                        new SeedIgnoringRelay()),
                    new OrRequestSpecification(
                        new SeedRequestSpecification(typeof(DoublePropertyHolder<string, int>)),
                        new ExactTypeSpecification(typeof(DoublePropertyHolder<string, int>)))));

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
        }

        [Fact]
        public void WithoutReturnsCorrectResult()
        {
            // Arrange
            var sut = SpecimenBuilderNodeFactory.CreateComposer<FieldHolder<short>>();
            var fi = typeof(FieldHolder<short>).GetField("Field");
            // Act
            var actual = sut.Without(x => x.Field);
            // Assert
            var expected = new NodeComposer<FieldHolder<short>>(
                new FilteringSpecimenBuilder(
                    new CompositeSpecimenBuilder(
                        new Postprocessor(
                            new NoSpecimenOutputGuard(
                                new MethodInvoker(
                                    new ModestConstructorQuery()),
                                new InverseRequestSpecification(
                                    new SeedRequestSpecification(
                                        typeof(FieldHolder<short>)))),
                            new AutoPropertiesCommand(
                                typeof(FieldHolder<short>),
                                new InverseRequestSpecification(
                                    new EqualRequestSpecification(
                                        fi,
                                        new MemberInfoEqualityComparer()))),
                            new FalseRequestSpecification()),
                        new SeedIgnoringRelay()),
                    new OrRequestSpecification(
                        new SeedRequestSpecification(typeof(FieldHolder<short>)),
                        new ExactTypeSpecification(typeof(FieldHolder<short>)))));

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
        }

        [Fact]
        public void WithAutoPropertiesTrueReturnsCorrectResult()
        {
            // Arrange
            var sut = SpecimenBuilderNodeFactory.CreateComposer<Version>();
            // Act
            NodeComposer<Version> actual =
                sut.WithAutoProperties(true);
            // Assert
            var expected = new NodeComposer<Version>(
                new FilteringSpecimenBuilder(
                    new CompositeSpecimenBuilder(
                        new Postprocessor(
                            new NoSpecimenOutputGuard(
                                new MethodInvoker(
                                    new ModestConstructorQuery()),
                                new InverseRequestSpecification(
                                    new SeedRequestSpecification(
                                        typeof(Version)))),
                            new AutoPropertiesCommand(typeof(Version)),
                            new TrueRequestSpecification()),
                        new SeedIgnoringRelay()),
                    new OrRequestSpecification(
                        new SeedRequestSpecification(typeof(Version)),
                        new ExactTypeSpecification(typeof(Version)))));

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
        }

        [Fact]
        public void WithAutoPropertiesFalseReturnsCorrectResult()
        {
            // Arrange
            var sut = SpecimenBuilderNodeFactory.CreateComposer<Version>();
            // Act
            var actual = sut.WithAutoProperties(false);
            // Assert
            var expected = sut;

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
        }

        [Fact]
        public void TogglingAutoPropertiesOnAndOffReturnsCorrectResult()
        {
            // Arrange
            var sut = SpecimenBuilderNodeFactory.CreateComposer<Version>();
            // Act
            var actual = sut.WithAutoProperties(true).WithAutoProperties(false);
            // Assert
            var expected = new NodeComposer<Version>(
                new FilteringSpecimenBuilder(
                    new CompositeSpecimenBuilder(
                        new Postprocessor(
                            new NoSpecimenOutputGuard(
                                new MethodInvoker(
                                    new ModestConstructorQuery()),
                                new InverseRequestSpecification(
                                    new SeedRequestSpecification(
                                        typeof(Version)))),
                            new AutoPropertiesCommand(typeof(Version)),
                            new FalseRequestSpecification()),
                        new SeedIgnoringRelay()),
                    new OrRequestSpecification(
                        new SeedRequestSpecification(typeof(Version)),
                        new ExactTypeSpecification(typeof(Version)))));

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
        }

        [Fact]
        public void WithAutoPropertiesTrueFollowedByFromFactoryReturnsCorrectResult()
        {
            // Arrange
            var sut = SpecimenBuilderNodeFactory.CreateComposer<PropertyHolder<string>>();
            Func<PropertyHolder<string>> f =
                () => new PropertyHolder<string>();
            // Act
            var actual = sut.WithAutoProperties(true).FromFactory(f);
            // Assert
            var expected = new NodeComposer<PropertyHolder<string>>(
                new FilteringSpecimenBuilder(
                    new CompositeSpecimenBuilder(
                        new Postprocessor(
                            new NoSpecimenOutputGuard(
                                new SpecimenFactory<PropertyHolder<string>>(f),
                                new InverseRequestSpecification(
                                    new SeedRequestSpecification(
                                        typeof(PropertyHolder<string>)))),
                            new AutoPropertiesCommand(typeof(PropertyHolder<string>)),
                            new TrueRequestSpecification()),
                        new SeedIgnoringRelay()),
                    new OrRequestSpecification(
                        new SeedRequestSpecification(typeof(PropertyHolder<string>)),
                        new ExactTypeSpecification(typeof(PropertyHolder<string>)))));

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
        }

        [Fact]
        public void WithAutoPropertiesTrueFollowedByDoReturnsCorrectResult()
        {
            // Arrange
            var sut = SpecimenBuilderNodeFactory.CreateComposer<PropertyHolder<int>>();
            Action<PropertyHolder<int>> a = ph => ph.Property = 42;
            // Act
            var actual = sut.WithAutoProperties(true).Do(a);
            // Assert
            var expected = new NodeComposer<PropertyHolder<int>>(
                new FilteringSpecimenBuilder(
                    new CompositeSpecimenBuilder(
                        new Postprocessor(
                            new Postprocessor(
                                new NoSpecimenOutputGuard(
                                    new MethodInvoker(
                                        new ModestConstructorQuery()),
                                    new InverseRequestSpecification(
                                        new SeedRequestSpecification(
                                            typeof(PropertyHolder<int>)))),
                                new ActionSpecimenCommand<PropertyHolder<int>>(a)),
                            new AutoPropertiesCommand(typeof(PropertyHolder<int>)),
                            new TrueRequestSpecification()),
                        new SeedIgnoringRelay()),
                    new OrRequestSpecification(
                        new SeedRequestSpecification(typeof(PropertyHolder<int>)),
                        new ExactTypeSpecification(typeof(PropertyHolder<int>)))));

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
        }
    }
}
