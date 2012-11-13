using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Dsl;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest.Dsl
{
    public class NodeComposerTest
    {
        [Fact]
        public void SutIsComposer()
        {
            // Fixture setup
            // Exercise system
            var sut = NodeComposer.Create<object>();
            // Verify outcome
            Assert.IsAssignableFrom<ICustomizationComposer<object>>(sut);
            // Teardown
        }

        [Fact]
        public void SutIsSpecimenBuilderNode()
        {
            // Fixture setup
            // Exercise system
            var sut = NodeComposer.Create<string>();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilderNode>(sut);
            // Teardown
        }

        [Fact]
        public void SutYieldsDecoratedBuilder()
        {
            // Fixture setup
            var expected = new DelegatingSpecimenBuilder();
            // Exercise system
            var sut = new NodeComposer<Guid>(expected);
            // Verify outcome
            Assert.True(new[] { expected }.SequenceEqual(sut));
            Assert.True(new object[] { expected }.SequenceEqual(
                ((System.Collections.IEnumerable)sut).Cast<object>()));
            // Teardown
        }

        [Fact]
        public void BuilderIsCorrect()
        {
            // Fixture setup
            var expected = new DelegatingSpecimenBuilder();
            var sut = new NodeComposer<Guid>(expected);
            // Exercise system
            ISpecimenBuilder actual = sut.Builder;
            // Verify outcome
            Assert.Equal(expected, actual);
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
            var sut = new NodeComposer<object>(builder);
            // Exercise system
            var actual = sut.Create(request, context);
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void SutIsCorrectInitialGraph()
        {
            // Fixture setup
            var sut = NodeComposer.Create<int>();
            // Exercise system
            // Verify outcome
            var factory = new MethodInvoker(new ModestConstructorQuery());
            var expected = new NodeComposer<int>(
                new TypedNode(typeof(int), factory));
            Assert.True(expected.GraphEquals(sut, new NodeComparer()));
            // Teardown
        }

        [Fact]
        public void FromSeedReturnsCorrectResult()
        {
            // Fixture setup
            var sut = NodeComposer.Create<decimal>();
            Func<decimal, decimal> f = d => d;
            // Exercise system
            var actual = sut.FromSeed(f);
            // Verify outcome
            var factory = new SeededFactory<decimal>(f);
            var expected = new NodeComposer<decimal>(
                new TypedNode(typeof(decimal), factory));

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
            // Teardown
        }

        [Fact]
        public void FromSpecimenBuilderFactoryReturnsCorrectResult()
        {
            // Fixture setup
            var sut = NodeComposer.Create<Guid>();
            var builder = new DelegatingSpecimenBuilder();
            // Exercise system
            var actual = sut.FromFactory(builder);
            // Verify outcome
            var expected = new NodeComposer<Guid>(
                new TypedNode(typeof(Guid), builder));

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
            // Teardown
        }

        [Fact]
        public void FromNoArgFuncReturnsCorrectResult()
        {
            // Fixture setup
            var sut = NodeComposer.Create<long>();
            Func<long> f = () => 0;
            // Exercise system
            var actual = sut.FromFactory(f);
            // Verify outcome
            var factory = new SpecimenFactory<long>(f);
            var expected = new NodeComposer<long>(
                new TypedNode(typeof(long), factory));

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
            // Teardown
        }

        [Fact]
        public void FromSingleArgFuncReturnsCorrectResult()
        {
            // Fixture setup
            var sut = NodeComposer.Create<float>();
            Func<int, float> f = i => i;
            // Exercise system
            var actual = sut.FromFactory(f);
            // Verify outcome
            var factory = new SpecimenFactory<int, float>(f);
            var expected = new NodeComposer<float>(
                new TypedNode(typeof(float), factory));

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
            // Teardown
        }

        [Fact]
        public void FromDoubleArgFuncReturnsCorrectResult()
        {
            // Fixture setup
            var sut = NodeComposer.Create<string>();
            Func<int, Version, string> f = (i, _) => i.ToString();
            // Exercise system
            var actual = sut.FromFactory(f);
            // Verify outcome
            var factory = new SpecimenFactory<int, Version, string>(f);
            var expected = new NodeComposer<string>(
                new TypedNode(typeof(string), factory));

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
            // Teardown
        }

        [Fact]
        public void FromTripleArgFuncReturnsCorrectResult()
        {
            // Fixture setup
            var sut = NodeComposer.Create<decimal>();
            Func<int, Guid, Version, decimal> f = (i, g, v) => i;
            // Exercise system
            var actual = sut.FromFactory(f);
            // Verify outcome
            var factory = new SpecimenFactory<int, Guid, Version, decimal>(f);
            var expected = new NodeComposer<decimal>(
                new TypedNode(typeof(decimal), factory));

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
            // Teardown
        }

        [Fact]
        public void FromQuadrupleArgFuncReturnsCorrectResult()
        {
            // Fixture setup
            var sut = NodeComposer.Create<Version>();
            Func<int, int, int, int, Version> f = (mj, mn, b, r) => new Version(mj, mn, b, r);
            // Exercise system
            var actual = sut.FromFactory(f);
            // Verify outcome
            var factory = new SpecimenFactory<int, int, int, int, Version>(f);
            var expected = new NodeComposer<Version>(
                new TypedNode(typeof(Version), factory));

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
            // Teardown
        }

        [Fact]
        public void LegacyComposeReturnsCorrectResult()
        {
            // Fixture setup
            var sut = NodeComposer.Create<UTF8Encoding>();
            // Exercise system
            var actual = sut.Compose();
            // Verify outcome
            Assert.Equal(sut, actual);
            // Teardown
        }

        [Fact]
        public void ComposeReturnsCorrectResult()
        {
            // Fixture setup
            var sut = NodeComposer.Create<GenericUriParser>();
            // Exercise system
            var expectedBuilders = new[]
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            };
            var actual = sut.Compose(expectedBuilders);
            // Verify outcome
            var nc = Assert.IsAssignableFrom<NodeComposer<GenericUriParser>>(actual);
            var composite = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(nc.Builder);
            Assert.True(expectedBuilders.SequenceEqual(composite));
            // Teardown
        }

        [Fact]
        public void ComposeSingleItemReturnsCorrectResult()
        {
            // Fixture setup
            var sut = NodeComposer.Create<GenericUriParser>();
            var expected = new DelegatingSpecimenBuilder();
            // Exercise system
            var actual = sut.Compose(new[] { expected });
            // Verify outcome
            var f = Assert.IsAssignableFrom<NodeComposer<GenericUriParser>>(actual);
            Assert.Equal(expected, f.Builder);
            // Teardown
        }

        [Fact]
        public void DoReturnsCorrectResult()
        {
            // Fixture setup
            var sut = NodeComposer.Create<AppDomainSetup>();
            Action<AppDomainSetup> a = ads => ads.DisallowApplicationBaseProbing = false;
            // Exercise system
            var actual = sut.Do(a);
            // Verify outcome
            var expected = new NodeComposer<AppDomainSetup>(
                new FilteringSpecimenBuilder(
                    new CompositeSpecimenBuilder(
                        new Postprocessor<AppDomainSetup>(
                            new NoSpecimenOutputGuard(
                                new MethodInvoker(
                                    new ModestConstructorQuery()),
                                new InverseRequestSpecification(
                                    new SeedRequestSpecification(
                                        typeof(AppDomainSetup)))),
                            a),
                        new SeedIgnoringRelay()),
                    new OrRequestSpecification(
                        new SeedRequestSpecification(typeof(AppDomainSetup)),
                        new ExactTypeSpecification(typeof(AppDomainSetup)))));

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
            // Teardown
        }

        [Fact]
        public void SecondDoReturnsCorrectResult()
        {
            // Fixture setup
            var sut = NodeComposer.Create<PropertyHolder<string>>();
            Action<PropertyHolder<string>> dummy = _ => { };
            Action<PropertyHolder<string>> a = ph => ph.Property = "";
            // Exercise system
            var actual = sut.Do(dummy).Do(a);
            // Verify outcome
            var expected = new NodeComposer<PropertyHolder<string>>(
                new FilteringSpecimenBuilder(
                    new CompositeSpecimenBuilder(
                        new Postprocessor<PropertyHolder<string>>(
                            new Postprocessor<PropertyHolder<string>>(
                                new NoSpecimenOutputGuard(
                                    new MethodInvoker(
                                        new ModestConstructorQuery()),
                                    new InverseRequestSpecification(
                                        new SeedRequestSpecification(
                                            typeof(PropertyHolder<string>)))),
                                dummy),
                            a),
                        new SeedIgnoringRelay()),
                    new OrRequestSpecification(
                        new SeedRequestSpecification(typeof(PropertyHolder<string>)),
                        new ExactTypeSpecification(typeof(PropertyHolder<string>)))));

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
            // Teardown
        }

        [Fact]
        public void WithAutoPropertiesReturnsCorrectResult()
        {
            // Fixture setup
            var sut = NodeComposer.Create<Version>();
            // Exercise system
            var actual = sut.WithAutoProperties();
            // Verify outcome
            var expected = new NodeComposer<Version>(
                new FilteringSpecimenBuilder(
                    new CompositeSpecimenBuilder(
                        new Postprocessor<Version>(
                            new NoSpecimenOutputGuard(
                                new MethodInvoker(
                                    new ModestConstructorQuery()),
                                new InverseRequestSpecification(
                                    new SeedRequestSpecification(
                                        typeof(Version)))),
                            new AutoPropertiesCommand<Version>().Execute,
                            new OrRequestSpecification(
                                new SeedRequestSpecification(typeof(Version)),
                                new ExactTypeSpecification(typeof(Version)))),
                        new SeedIgnoringRelay()),
                    new OrRequestSpecification(
                        new SeedRequestSpecification(typeof(Version)),
                        new ExactTypeSpecification(typeof(Version)))));

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
            // Teardown
        }

        [Fact]
        public void OmitAutoPropertiesIsInitiallyANoOp()
        {
            // Fixture setup
            var sut = NodeComposer.Create<Version>();
            // Exercise system
            var actual = sut.OmitAutoProperties();
            // Verify outcome
            var expected = sut;

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
            // Teardown
        }

        [Fact]
        public void OmitAutoPropertiesAfterAddingAutoPropertiesReturnsCorrectResult()
        {
            // Fixture setup
            var sut = NodeComposer.Create<Version>();
            // Exercise system
            var actual = sut.WithAutoProperties().OmitAutoProperties();
            // Verify outcome
            var expected = sut;

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
            // Teardown
        }

        [Fact]
        public void WithAnonymouValueReturnsCorrectResult()
        {
            // Fixture setup
            var sut = NodeComposer.Create<PropertyHolder<int>>();
            // Exercise system
            var actual = sut.With(x => x.Property);
            // Verify outcome
            var expected = new NodeComposer<PropertyHolder<int>>(
                new FilteringSpecimenBuilder(
                    new CompositeSpecimenBuilder(
                        new Postprocessor<PropertyHolder<int>>(
                            new NoSpecimenOutputGuard(
                                new MethodInvoker(
                                    new ModestConstructorQuery()),
                                new InverseRequestSpecification(
                                    new SeedRequestSpecification(
                                        typeof(PropertyHolder<int>)))),
                            new BindingCommand<PropertyHolder<int>, int>(x => x.Property).Execute,
                            new OrRequestSpecification(
                                new SeedRequestSpecification(typeof(PropertyHolder<int>)),
                                new ExactTypeSpecification(typeof(PropertyHolder<int>)))),
                        new SeedIgnoringRelay()),
                    new OrRequestSpecification(
                        new SeedRequestSpecification(typeof(PropertyHolder<int>)),
                        new ExactTypeSpecification(typeof(PropertyHolder<int>)))));

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
            var sut = NodeComposer.Create<PropertyHolder<string>>();
            var pi = typeof(PropertyHolder<string>).GetProperty("Property");
            // Exercise system
            var actual = sut.With(x => x.Property, value);
            // Verify outcome
            var expected = new NodeComposer<PropertyHolder<string>>(
                new CompositeSpecimenBuilder(
                    new Omitter(
                        new EqualRequestSpecification(
                            pi,
                            new MemberInfoEqualityComparer())),
                    new FilteringSpecimenBuilder(
                        new CompositeSpecimenBuilder(
                            new Postprocessor<PropertyHolder<string>>(
                                new NoSpecimenOutputGuard(
                                    new MethodInvoker(
                                        new ModestConstructorQuery()),
                                    new InverseRequestSpecification(
                                        new SeedRequestSpecification(
                                            typeof(PropertyHolder<string>)))),
                                new BindingCommand<PropertyHolder<string>, string>(x => x.Property, value).Execute,
                                new OrRequestSpecification(
                                    new SeedRequestSpecification(typeof(PropertyHolder<string>)),
                                    new ExactTypeSpecification(typeof(PropertyHolder<string>)))),
                            new SeedIgnoringRelay()),
                        new OrRequestSpecification(
                            new SeedRequestSpecification(typeof(PropertyHolder<string>)),
                            new ExactTypeSpecification(typeof(PropertyHolder<string>))))));

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
            // Teardown
        }

        [Fact]
        public void WithoutReturnsCorrectResult()
        {
            // Fixture setup
            var sut = NodeComposer.Create<FieldHolder<short>>();
            var fi = typeof(FieldHolder<short>).GetField("Field");
            // Exercise system
            var actual = sut.Without(x => x.Field);
            // Verify outcome
            var expected = new NodeComposer<FieldHolder<short>>(
                new CompositeSpecimenBuilder(
                    new Omitter(
                        new EqualRequestSpecification(
                            fi,
                            new MemberInfoEqualityComparer())),
                    new FilteringSpecimenBuilder(
                        new CompositeSpecimenBuilder(
                            new NoSpecimenOutputGuard(
                                new MethodInvoker(
                                    new ModestConstructorQuery()),
                                new InverseRequestSpecification(
                                    new SeedRequestSpecification(
                                        typeof(FieldHolder<short>)))),
                            new SeedIgnoringRelay()),
                        new OrRequestSpecification(
                            new SeedRequestSpecification(typeof(FieldHolder<short>)),
                            new ExactTypeSpecification(typeof(FieldHolder<short>))))));

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
            // Teardown
        }

        [Fact]
        public void WithAutoPropertiesTrueReturnsCorrectResult()
        {
            // Fixture setup
            var sut = NodeComposer.Create<Version>();
            // Exercise system
            NodeComposer<Version> actual = 
                sut.WithAutoProperties(true);
            // Verify outcome
            var expected = new NodeComposer<Version>(
                new FilteringSpecimenBuilder(
                    new CompositeSpecimenBuilder(
                        new Postprocessor<Version>(
                            new NoSpecimenOutputGuard(
                                new MethodInvoker(
                                    new ModestConstructorQuery()),
                                new InverseRequestSpecification(
                                    new SeedRequestSpecification(
                                        typeof(Version)))),
                            new AutoPropertiesCommand<Version>().Execute,
                            new OrRequestSpecification(
                                new SeedRequestSpecification(typeof(Version)),
                                new ExactTypeSpecification(typeof(Version)))),
                        new SeedIgnoringRelay()),
                    new OrRequestSpecification(
                        new SeedRequestSpecification(typeof(Version)),
                        new ExactTypeSpecification(typeof(Version)))));

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
            // Teardown
        }

        [Fact]
        public void WithAutoPropertiesFalseReturnsCorrectResult()
        {
            // Fixture setup
            var sut = NodeComposer.Create<Version>();
            // Exercise system
            var actual = sut.WithAutoProperties(false);
            // Verify outcome
            var expected = sut;

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
            // Teardown
        }

        [Fact]
        public void TogglingAutoPropertiesOnAndOffReturnsCorrectResult()
        {
            // Fixture setup
            var sut = NodeComposer.Create<Version>();
            // Exercise system
            var actual = sut.WithAutoProperties(true).WithAutoProperties(false);
            // Verify outcome
            var expected = sut;

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
            // Teardown
        }

        [Fact]
        public void WithAutoPropertiesTrueFollowedByFromFactoryReturnsCorrectResult()
        {
            // Fixture setup
            var sut = NodeComposer.Create<PropertyHolder<string>>();
            Func<PropertyHolder<string>> f =
                () => new PropertyHolder<string>();
            // Exercise system
            var actual = sut.WithAutoProperties(true).FromFactory(f);
            // Verify outcome
            var expected = new NodeComposer<PropertyHolder<string>>(
                new FilteringSpecimenBuilder(
                    new CompositeSpecimenBuilder(
                        new Postprocessor<PropertyHolder<string>>(
                            new NoSpecimenOutputGuard(
                                new SpecimenFactory<PropertyHolder<string>>(f),
                                new InverseRequestSpecification(
                                    new SeedRequestSpecification(
                                        typeof(PropertyHolder<string>)))),
                            new AutoPropertiesCommand<PropertyHolder<string>>().Execute,
                            new OrRequestSpecification(
                                new SeedRequestSpecification(typeof(PropertyHolder<string>)),
                                new ExactTypeSpecification(typeof(PropertyHolder<string>)))),
                        new SeedIgnoringRelay()),
                    new OrRequestSpecification(
                        new SeedRequestSpecification(typeof(PropertyHolder<string>)),
                        new ExactTypeSpecification(typeof(PropertyHolder<string>)))));

            var n = Assert.IsAssignableFrom<ISpecimenBuilderNode>(actual);
            Assert.True(expected.GraphEquals(n, new NodeComparer()));
            // Teardown
        }
    }
}
