using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Dsl;
using System.Linq.Expressions;
using Ploeh.TestTypeFoundation;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest.Dsl
{
    public class CompositeComposerTest
    {
        [Fact]
        public void SutIsPostprocessComposer()
        {
            // Fixture setup
            // Exercise system
            var sut = new CompositeComposer<object>();
            // Verify outcome
            Assert.IsAssignableFrom<ICustomizationComposer<object>>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullArrayThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new CompositeComposer<DateTime>((ICustomizationComposer<DateTime>[])null));
            // Teardown
        }

        [Fact]
        public void IntializeWithNullEnumerableThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new CompositeComposer<TimeSpan>((IEnumerable<ICustomizationComposer<TimeSpan>>)null));
            // Teardown
        }

        [Fact]
        public void ComposersIsCorrectWhenInitializedWithArray()
        {
            // Fixture setup
            var expectedComposers = Enumerable.Range(1, 3).Select(i => new DelegatingComposer<string>()).ToArray();
            var sut = new CompositeComposer<string>(expectedComposers);
            // Exercise system
            IEnumerable<ICustomizationComposer<string>> result = sut.Composers;
            // Verify outcome
            Assert.True(expectedComposers.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void ComposersIsCorrectWhenInitializedWithEnumerable()
        {
            // Fixture setup
            var expectedComposers = Enumerable.Range(1, 3).Select(i => new DelegatingComposer<int>()).Cast<ICustomizationComposer<int>>().ToList();
            var sut = new CompositeComposer<int>(expectedComposers);
            // Exercise system
            var result = sut.Composers;
            // Verify outcome
            Assert.True(expectedComposers.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void DoReturnsCorrectResult()
        {
            // Fixture setup
            Action<object> expectedAction = s => { };
            var expectedComposers = Enumerable.Range(1, 3).Select(i => new DelegatingComposer<object>()).ToArray();
            var initialComposers = (from c in expectedComposers
                                    select new DelegatingComposer
                                    {
                                        OnDo = a => a == expectedAction ? c : new DelegatingComposer<object>()
                                    }).ToArray();
            var sut = new CompositeComposer<object>(initialComposers);
            // Exercise system
            var result = sut.Do(expectedAction);
            // Verify outcome
            var composite = Assert.IsAssignableFrom<CompositePostprocessComposer<object>>(result);
            Assert.True(expectedComposers.SequenceEqual(composite.Composers));
            // Teardown
        }

        [Fact]
        public void OmitAutoPropertiesReturnsCorrectResult()
        {
            // Fixture setup
            var expectedComposers = Enumerable.Range(1, 3).Select(i => new DelegatingComposer<object>()).ToArray();
            var initialComposers = (from c in expectedComposers
                                    select new DelegatingComposer { OnOmitAutoProperties = () => c }).ToArray();
            var sut = new CompositeComposer<object>(initialComposers);
            // Exercise system
            var result = sut.OmitAutoProperties();
            // Verify outcome
            var composite = Assert.IsAssignableFrom<CompositePostprocessComposer<object>>(result);
            Assert.True(expectedComposers.SequenceEqual(composite.Composers));
            // Teardown
        }

        [Fact]
        public void AnonymousWithReturnsCorrectResult()
        {
            // Fixture setup
            Expression<Func<PropertyHolder<object>, object>> expectedExpression = x => x.Property;
            var expectedComposers = Enumerable.Range(1, 3).Select(i => new DelegatingComposer<PropertyHolder<object>>()).ToArray();
            var initialComposers = (from c in expectedComposers
                                    select new DelegatingComposer<PropertyHolder<object>>
                                    {
                                        OnAnonymousWith = f => f == expectedExpression ? c : new DelegatingComposer<PropertyHolder<object>>()
                                    }).ToArray();
            var sut = new CompositeComposer<PropertyHolder<object>>(initialComposers);
            // Exercise system
            var result = sut.With(expectedExpression);
            // Verify outcome
            var composite = Assert.IsAssignableFrom<CompositePostprocessComposer<PropertyHolder<object>>>(result);
            Assert.True(expectedComposers.SequenceEqual(composite.Composers));
            // Teardown
        }

        [Fact]
        public void WithReturnsCorrectResult()
        {
            // Fixture setup
            Expression<Func<PropertyHolder<object>, object>> expectedExpression = x => x.Property;
            var value = new object();

            var expectedComposers = Enumerable.Range(1, 3).Select(i => new DelegatingComposer<PropertyHolder<object>>()).ToArray();
            var initialComposers = (from c in expectedComposers
                                    select new DelegatingComposer<PropertyHolder<object>>
                                    {
                                        OnWith = (f, v) => f == expectedExpression && v == value ? c : new DelegatingComposer<PropertyHolder<object>>()
                                    }).ToArray();
            var sut = new CompositeComposer<PropertyHolder<object>>(initialComposers);
            // Exercise system
            var result = sut.With(expectedExpression, value);
            // Verify outcome
            var composite = Assert.IsAssignableFrom<CompositePostprocessComposer<PropertyHolder<object>>>(result);
            Assert.True(expectedComposers.SequenceEqual(composite.Composers));
            // Teardown
        }

        [Fact]
        public void WithAutoPropertiesReturnsCorrectResult()
        {
            // Fixture setup
            var expectedComposers = Enumerable.Range(1, 3).Select(i => new DelegatingComposer<object>()).ToArray();
            var initialComposers = (from c in expectedComposers
                                    select new DelegatingComposer { OnWithAutoProperties = () => c }).ToArray();
            var sut = new CompositeComposer<object>(initialComposers);
            // Exercise system
            var result = sut.WithAutoProperties();
            // Verify outcome
            var composite = Assert.IsAssignableFrom<CompositePostprocessComposer<object>>(result);
            Assert.True(expectedComposers.SequenceEqual(composite.Composers));
            // Teardown
        }

        [Fact]
        public void WithoutReturnsCorrectResult()
        {
            // Fixture setup
            Expression<Func<PropertyHolder<object>, object>> expectedExpression = x => x.Property;
            var expectedComposers = Enumerable.Range(1, 3).Select(i => new DelegatingComposer<PropertyHolder<object>>()).ToArray();
            var initialComposers = (from c in expectedComposers
                                    select new DelegatingComposer<PropertyHolder<object>>
                                    {
                                        OnWithout = f => f == expectedExpression ? c : new DelegatingComposer<PropertyHolder<object>>()
                                    }).ToArray();
            var sut = new CompositeComposer<PropertyHolder<object>>(initialComposers);
            // Exercise system
            var result = sut.Without(expectedExpression);
            // Verify outcome
            var composite = Assert.IsAssignableFrom<CompositePostprocessComposer<PropertyHolder<object>>>(result);
            Assert.True(expectedComposers.SequenceEqual(composite.Composers));
            // Teardown
        }

        [Fact]
        public void FromSeedReturnsCorrectResult()
        {
            // Fixture setup
            Func<string, string> expectedFactory = s => s;
            var expectedComposers = Enumerable.Range(1, 3).Select(i => new DelegatingComposer<string>()).ToArray();
            var initialComposers = (from c in expectedComposers
                                    select new DelegatingComposer<string>
                                    {
                                        OnFromSeed = f => f == expectedFactory ? c : new DelegatingComposer<string>()
                                    }).ToArray();
            var sut = new CompositeComposer<string>(initialComposers);
            // Exercise system
            var result = sut.FromSeed(expectedFactory);
            // Verify outcome
            var composite = Assert.IsAssignableFrom<CompositePostprocessComposer<string>>(result);
            Assert.True(expectedComposers.SequenceEqual(composite.Composers));
            // Teardown
        }

        [Fact]
        public void FromFactoryReturnsCorrectResult()
        {
            // Fixture setup
            Func<object> expectedFactory = () => new object();
            var expectedComposers = Enumerable.Range(1, 3).Select(i => new DelegatingComposer()).ToArray();
            var initialComposers = (from c in expectedComposers
                                    select new DelegatingComposer
                                    {
                                        OnFromFactory = f => f == expectedFactory ? c : new DelegatingComposer()
                                    }).ToArray();
            var sut = new CompositeComposer<object>(initialComposers);
            // Exercise system
            var result = sut.FromFactory(expectedFactory);
            // Verify outcome
            var composite = Assert.IsAssignableFrom<CompositePostprocessComposer<object>>(result);
            Assert.True(expectedComposers.SequenceEqual(composite.Composers));
            // Teardown
        }

        [Fact]
        public void FromSingleParameterFactoryReturnsCorrectResult()
        {
            // Fixture setup
            Func<object, object> expectedFactory = s => new object();
            var expectedComposers = Enumerable.Range(1, 3).Select(i => new DelegatingComposer()).ToArray();
            var initialComposers = (from c in expectedComposers
                                    select new DelegatingComposer
                                    {
                                        OnFromOverloadeFactory = f => f.Equals(expectedFactory) ? c : new DelegatingComposer()
                                    }).ToArray();
            var sut = new CompositeComposer<object>(initialComposers);
            // Exercise system
            var result = sut.FromFactory(expectedFactory);
            // Verify outcome
            var composite = Assert.IsAssignableFrom<CompositePostprocessComposer<object>>(result);
            Assert.True(expectedComposers.SequenceEqual(composite.Composers));
            // Teardown
        }

        [Fact]
        public void FromDoubleParameterFactoryReturnsCorrectResult()
        {
            // Fixture setup
            Func<object, object, object> expectedFactory = (x, y) => new object();
            var expectedComposers = Enumerable.Range(1, 3).Select(i => new DelegatingComposer()).ToArray();
            var initialComposers = (from c in expectedComposers
                                    select new DelegatingComposer
                                    {
                                        OnFromOverloadeFactory = f => f.Equals(expectedFactory) ? c : new DelegatingComposer()
                                    }).ToArray();
            var sut = new CompositeComposer<object>(initialComposers);
            // Exercise system
            var result = sut.FromFactory(expectedFactory);
            // Verify outcome
            var composite = Assert.IsAssignableFrom<CompositePostprocessComposer<object>>(result);
            Assert.True(expectedComposers.SequenceEqual(composite.Composers));
            // Teardown
        }

        [Fact]
        public void FromTripleParameterFactoryReturnsCorrectResult()
        {
            // Fixture setup
            Func<object, object, object, object> expectedFactory = (x, y, z) => new object();
            var expectedComposers = Enumerable.Range(1, 3).Select(i => new DelegatingComposer()).ToArray();
            var initialComposers = (from c in expectedComposers
                                    select new DelegatingComposer
                                    {
                                        OnFromOverloadeFactory = f => f.Equals(expectedFactory) ? c : new DelegatingComposer()
                                    }).ToArray();
            var sut = new CompositeComposer<object>(initialComposers);
            // Exercise system
            var result = sut.FromFactory(expectedFactory);
            // Verify outcome
            var composite = Assert.IsAssignableFrom<CompositePostprocessComposer<object>>(result);
            Assert.True(expectedComposers.SequenceEqual(composite.Composers));
            // Teardown
        }

        [Fact]
        public void FromQuadrupleParameterFactoryReturnsCorrectResult()
        {
            // Fixture setup
            Func<object, object, object, object, object> expectedFactory = (x, y, z, æ) => new object();
            var expectedComposers = Enumerable.Range(1, 3).Select(i => new DelegatingComposer()).ToArray();
            var initialComposers = (from c in expectedComposers
                                    select new DelegatingComposer
                                    {
                                        OnFromOverloadeFactory = f => f.Equals(expectedFactory) ? c : new DelegatingComposer()
                                    }).ToArray();
            var sut = new CompositeComposer<object>(initialComposers);
            // Exercise system
            var result = sut.FromFactory(expectedFactory);
            // Verify outcome
            var composite = Assert.IsAssignableFrom<CompositePostprocessComposer<object>>(result);
            Assert.True(expectedComposers.SequenceEqual(composite.Composers));
            // Teardown
        }

        [Fact]
        public void ComposeReturnsCorrectResult()
        {
            // Fixture setup
            var expectedBuilders = Enumerable.Range(1, 3).Select(i => new DelegatingSpecimenBuilder()).ToArray();
            var composers = (from b in expectedBuilders
                             select new DelegatingComposer { OnCompose = () => b }).ToArray();
            var sut = new CompositeComposer<object>(composers);
            // Exercise system
            var result = sut.Compose();
            // Verify outcome
            var composite = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(result);
            Assert.True(expectedBuilders.SequenceEqual(composite.Builders));
            // Teardown
        }
    }
}
