using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ploeh.AutoFixture.Dsl;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Dsl
{
    public class CompositePostprocessComposerTest
    {
        [Fact]
        public void SutIsPostprocessComposer()
        {
            // Fixture setup
            // Exercise system
            var sut = new CompositePostprocessComposer<object>();
            // Verify outcome
            Assert.IsAssignableFrom<IPostprocessComposer<object>>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullArrayThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new CompositePostprocessComposer<DateTime>((IPostprocessComposer<DateTime>[])null));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullEnumerableThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new CompositePostprocessComposer<TimeSpan>((IEnumerable<IPostprocessComposer<TimeSpan>>)null));
            // Teardown
        }

        [Fact]
        public void ComposersIsCorrectWhenInitializedWithArray()
        {
            // Fixture setup
            var expectedComposers = Enumerable.Range(1, 3).Select(i => new DelegatingComposer<string>()).ToArray();
            var sut = new CompositePostprocessComposer<string>(expectedComposers);
            // Exercise system
            IEnumerable<IPostprocessComposer<string>> result = sut.Composers;
            // Verify outcome
            Assert.True(expectedComposers.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void ComposersIsCorrectWhenInitializedWithEnumerable()
        {
            // Fixture setup
            var expectedComposers = Enumerable.Range(1, 3).Select(i => new DelegatingComposer<int>()).Cast<IPostprocessComposer<int>>().ToList();
            var sut = new CompositePostprocessComposer<int>(expectedComposers);
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
            var sut = new CompositePostprocessComposer<object>(initialComposers);
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
            var sut = new CompositePostprocessComposer<object>(initialComposers);
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
            var sut = new CompositePostprocessComposer<PropertyHolder<object>>(initialComposers);
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
            var sut = new CompositePostprocessComposer<PropertyHolder<object>>(initialComposers);
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
            var sut = new CompositePostprocessComposer<object>(initialComposers);
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
            var sut = new CompositePostprocessComposer<PropertyHolder<object>>(initialComposers);
            // Exercise system
            var result = sut.Without(expectedExpression);
            // Verify outcome
            var composite = Assert.IsAssignableFrom<CompositePostprocessComposer<PropertyHolder<object>>>(result);
            Assert.True(expectedComposers.SequenceEqual(composite.Composers));
            // Teardown
        }
    }
}
