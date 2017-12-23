using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoFixture.Dsl;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Dsl
{
    public class CompositePostprocessComposerTest
    {
        [Fact]
        public void SutIsPostprocessComposer()
        {
            // Arrange
            // Act
            var sut = new CompositePostprocessComposer<object>();
            // Assert
            Assert.IsAssignableFrom<IPostprocessComposer<object>>(sut);
        }

        [Fact]
        public void InitializeWithNullArrayThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new CompositePostprocessComposer<DateTime>((IPostprocessComposer<DateTime>[])null));
        }

        [Fact]
        public void InitializeWithNullEnumerableThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new CompositePostprocessComposer<TimeSpan>((IEnumerable<IPostprocessComposer<TimeSpan>>)null));
        }

        [Fact]
        public void ComposersIsCorrectWhenInitializedWithArray()
        {
            // Arrange
            var expectedComposers = Enumerable.Range(1, 3).Select(i => new DelegatingComposer<string>()).ToArray();
            var sut = new CompositePostprocessComposer<string>(expectedComposers);
            // Act
            IEnumerable<IPostprocessComposer<string>> result = sut.Composers;
            // Assert
            Assert.True(expectedComposers.SequenceEqual(result));
        }

        [Fact]
        public void ComposersIsCorrectWhenInitializedWithEnumerable()
        {
            // Arrange
            var expectedComposers = Enumerable.Range(1, 3).Select(i => new DelegatingComposer<int>()).Cast<IPostprocessComposer<int>>().ToList();
            var sut = new CompositePostprocessComposer<int>(expectedComposers);
            // Act
            var result = sut.Composers;
            // Assert
            Assert.True(expectedComposers.SequenceEqual(result));
        }

        [Fact]
        public void DoReturnsCorrectResult()
        {
            // Arrange
            Action<object> expectedAction = s => { };
            var expectedComposers = Enumerable.Range(1, 3).Select(i => new DelegatingComposer<object>()).ToArray();
            var initialComposers = (from c in expectedComposers
                                    select new DelegatingComposer
                                    {
                                        OnDo = a => a == expectedAction ? c : new DelegatingComposer<object>()
                                    }).ToArray();
            var sut = new CompositePostprocessComposer<object>(initialComposers);
            // Act
            var result = sut.Do(expectedAction);
            // Assert
            var composite = Assert.IsAssignableFrom<CompositePostprocessComposer<object>>(result);
            Assert.True(expectedComposers.SequenceEqual(composite.Composers));
        }

        [Fact]
        public void OmitAutoPropertiesReturnsCorrectResult()
        {
            // Arrange
            var expectedComposers = Enumerable.Range(1, 3).Select(i => new DelegatingComposer<object>()).ToArray();
            var initialComposers = (from c in expectedComposers
                                    select new DelegatingComposer { OnOmitAutoProperties = () => c }).ToArray();
            var sut = new CompositePostprocessComposer<object>(initialComposers);
            // Act
            var result = sut.OmitAutoProperties();
            // Assert
            var composite = Assert.IsAssignableFrom<CompositePostprocessComposer<object>>(result);
            Assert.True(expectedComposers.SequenceEqual(composite.Composers));
        }

        [Fact]
        public void AnonymousWithReturnsCorrectResult()
        {
            // Arrange
            Expression<Func<PropertyHolder<object>, object>> expectedExpression = x => x.Property;
            var expectedComposers = Enumerable.Range(1, 3).Select(i => new DelegatingComposer<PropertyHolder<object>>()).ToArray();
            var initialComposers = (from c in expectedComposers
                                    select new DelegatingComposer<PropertyHolder<object>>
                                    {
                                        OnAnonymousWith = f => f == expectedExpression ? c : new DelegatingComposer<PropertyHolder<object>>()
                                    }).ToArray();
            var sut = new CompositePostprocessComposer<PropertyHolder<object>>(initialComposers);
            // Act
            var result = sut.With(expectedExpression);
            // Assert
            var composite = Assert.IsAssignableFrom<CompositePostprocessComposer<PropertyHolder<object>>>(result);
            Assert.True(expectedComposers.SequenceEqual(composite.Composers));
        }

        [Fact]
        public void WithReturnsCorrectResult()
        {
            // Arrange
            Expression<Func<PropertyHolder<object>, object>> expectedExpression = x => x.Property;
            var value = new object();

            var expectedComposers = Enumerable.Range(1, 3).Select(i => new DelegatingComposer<PropertyHolder<object>>()).ToArray();
            var initialComposers = (from c in expectedComposers
                                    select new DelegatingComposer<PropertyHolder<object>>
                                    {
                                        OnWith = (f, v) => f == expectedExpression && v == value ? c : new DelegatingComposer<PropertyHolder<object>>()
                                    }).ToArray();
            var sut = new CompositePostprocessComposer<PropertyHolder<object>>(initialComposers);
            // Act
            var result = sut.With(expectedExpression, value);
            // Assert
            var composite = Assert.IsAssignableFrom<CompositePostprocessComposer<PropertyHolder<object>>>(result);
            Assert.True(expectedComposers.SequenceEqual(composite.Composers));
        }

        [Fact]
        public void WithAutoPropertiesReturnsCorrectResult()
        {
            // Arrange
            var expectedComposers = Enumerable.Range(1, 3).Select(i => new DelegatingComposer<object>()).ToArray();
            var initialComposers = (from c in expectedComposers
                                    select new DelegatingComposer { OnWithAutoProperties = () => c }).ToArray();
            var sut = new CompositePostprocessComposer<object>(initialComposers);
            // Act
            var result = sut.WithAutoProperties();
            // Assert
            var composite = Assert.IsAssignableFrom<CompositePostprocessComposer<object>>(result);
            Assert.True(expectedComposers.SequenceEqual(composite.Composers));
        }

        [Fact]
        public void WithoutReturnsCorrectResult()
        {
            // Arrange
            Expression<Func<PropertyHolder<object>, object>> expectedExpression = x => x.Property;
            var expectedComposers = Enumerable.Range(1, 3).Select(i => new DelegatingComposer<PropertyHolder<object>>()).ToArray();
            var initialComposers = (from c in expectedComposers
                                    select new DelegatingComposer<PropertyHolder<object>>
                                    {
                                        OnWithout = f => f == expectedExpression ? c : new DelegatingComposer<PropertyHolder<object>>()
                                    }).ToArray();
            var sut = new CompositePostprocessComposer<PropertyHolder<object>>(initialComposers);
            // Act
            var result = sut.Without(expectedExpression);
            // Assert
            var composite = Assert.IsAssignableFrom<CompositePostprocessComposer<PropertyHolder<object>>>(result);
            Assert.True(expectedComposers.SequenceEqual(composite.Composers));
        }
    }
}
