using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Dsl;
using Ploeh.AutoFixture;
using Ploeh.AutoFixtureUnitTest.Dsl;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using System.Linq.Expressions;
using Ploeh.TestTypeFoundation;

namespace Ploeh.AutoFixtureUnitTest
{
    public class BehaviorPostprocessComposerTest
    {
        [Fact]
        public void SutIsPostprocessComposer()
        {
            // Fixture setup
            var dummyComposer = new DelegatingComposer<object>();
            // Exercise system
            var sut = new BehaviorPostprocessComposer<object>(dummyComposer);
            // Verify outcome
            Assert.IsAssignableFrom<IPostprocessComposer<object>>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullComposerThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new BehaviorPostprocessComposer<object>(null));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullBehaviorArrayThrows()
        {
            // Fixture setup
            var dummyComposer = new DelegatingComposer<object>();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new BehaviorPostprocessComposer<object>(dummyComposer, (ISpecimenBuilderTransformation[])null));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullComposerAndEnumerableThrows()
        {
            // Fixture setup
            var dummyBehaviors = Enumerable.Empty<ISpecimenBuilderTransformation>();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new BehaviorPostprocessComposer<object>(null, dummyBehaviors));
            // Teardown
        }

        [Fact]
        public void InitializeWithComposerAndNullEnumerableThrows()
        {
            // Fixture setup
            var dummyComposer = new DelegatingComposer<object>();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new BehaviorPostprocessComposer<object>(dummyComposer, (IEnumerable<ISpecimenBuilderTransformation>)null));
            // Teardown
        }

        [Fact]
        public void InitializedWithArrayConstructorHasCorrectComposer()
        {
            // Fixture setup
            var expectedComposer = new DelegatingComposer<object>();
            var sut = new BehaviorPostprocessComposer<object>(expectedComposer);
            // Exercise system
            IPostprocessComposer<object> result = sut.Composer;
            // Verify outcome
            Assert.Equal(expectedComposer, result);
            // Teardown
        }

        [Fact]
        public void InitializedWithEnumerableConstructorHasCorrectComposer()
        {
            // Fixture setup
            var expectedComposer = new DelegatingComposer<object>();
            var dummyBehaviors = Enumerable.Empty<ISpecimenBuilderTransformation>();
            var sut = new BehaviorPostprocessComposer<object>(expectedComposer, dummyBehaviors);
            // Exercise system
            var result = sut.Composer;
            // Verify outcome
            Assert.Equal(expectedComposer, result);
            // Teardown
        }

        [Fact]
        public void InitializedWithArrayConstructorHasCorrectBehaviors()
        {
            // Fixture setup
            var dummyComposer = new DelegatingComposer<object>();
            var expectedBehaviors = Enumerable.Range(1, 3)
                .Select(i => new DelegatingSpecimenBuilderTransformation())
                .Cast<ISpecimenBuilderTransformation>()
                .ToArray();
            var sut = new BehaviorPostprocessComposer<object>(dummyComposer, expectedBehaviors);
            // Exercise system
            IEnumerable<ISpecimenBuilderTransformation> result = sut.Behaviors;
            // Verify outcome
            Assert.True(expectedBehaviors.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void InitializedWithEnumerableConstructorHasCorrectBehaviors()
        {
            // Fixture setup
            var dummyComposer = new DelegatingComposer<object>();
            var expectedBehaviors = Enumerable.Range(1, 3)
                .Select(i => new DelegatingSpecimenBuilderTransformation())
                .Cast<ISpecimenBuilderTransformation>()
                .ToList();
            var sut = new BehaviorPostprocessComposer<object>(dummyComposer, expectedBehaviors);
            // Exercise system
            var result = sut.Behaviors;
            // Verify outcome
            Assert.True(expectedBehaviors.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void BehaviorsIsStable()
        {
            // Fixture setup
            var dummyComposer = new DelegatingComposer<object>();
            var behaviors = Enumerable.Range(1, 3)
                .Select(i => new DelegatingSpecimenBuilderTransformation())
                .Cast<ISpecimenBuilderTransformation>();
            var sut = new BehaviorPostprocessComposer<object>(dummyComposer, behaviors);
            var expectedBehaviors = sut.Behaviors;
            // Exercise system
            var result = sut.Behaviors;
            // Verify outcome
            Assert.True(expectedBehaviors.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void WithNullComposerThrows()
        {
            // Fixture setup
            var dummyComposer = new DelegatingComposer<object>();
            var sut = new BehaviorPostprocessComposer<object>(dummyComposer);
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.With(null));
            // Teardown
        }

        [Fact]
        public void WithReturnsResultWithCorrectComposer()
        {
            // Fixture setup
            var dummyComposer = new DelegatingComposer<object>();
            var sut = new BehaviorPostprocessComposer<object>(dummyComposer);
            var expectedComposer = new DelegatingComposer<object>();
            // Exercise system
            var result = sut.With(expectedComposer);
            // Verify outcome
            Assert.Equal(expectedComposer, result.Composer);
            // Teardown
        }

        [Fact]
        public void WithReturnsResultWithCorrectBehaviors()
        {
            // Fixture setup
            var dummyComposer1 = new DelegatingComposer<object>();
            var expectedBehaviors = Enumerable.Range(1, 3)
                .Select(i => new DelegatingSpecimenBuilderTransformation())
                .ToArray();
            var sut = new BehaviorPostprocessComposer<object>(dummyComposer1, expectedBehaviors);
            // Exercise system
            var dummyComposer2 = new DelegatingComposer<object>();
            var result = sut.With(dummyComposer2);
            // Verify outcome
            Assert.True(expectedBehaviors.SequenceEqual(result.Behaviors));
            // Teardown
        }

        [Fact]
        public void DoReturnsCorrectResult()
        {
            // Fixture setup
            Action<object> expectedAction = s => { };
            var expectedComposer = new DelegatingComposer<object>();
            var composer = new DelegatingComposer<object> { OnDo = a => a == expectedAction ? expectedComposer : new DelegatingComposer<object>() };

            var sut = new BehaviorPostprocessComposer<object>(composer);
            // Exercise system
            var result = sut.Do(expectedAction);
            // Verify outcome
            var behaviorComposer = Assert.IsAssignableFrom<BehaviorPostprocessComposer<object>>(result);
            Assert.Equal(expectedComposer, behaviorComposer.Composer);
            // Teardown
        }

        [Fact]
        public void OmitAutoPropertiesReturnsCorrectResult()
        {
            // Fixture setup
            var expectedComposer = new DelegatingComposer<object>();
            var composer = new DelegatingComposer<object> { OnOmitAutoProperties = () => expectedComposer };

            var sut = new BehaviorPostprocessComposer<object>(composer);
            // Exercise system
            var result = sut.OmitAutoProperties();
            // Verify outcome
            var behaviorComposer = Assert.IsAssignableFrom<BehaviorPostprocessComposer<object>>(result);
            Assert.Equal(expectedComposer, behaviorComposer.Composer);
            // Teardown
        }

        [Fact]
        public void AnonymousWithReturnsCorrectResult()
        {
            // Fixture setup
            Expression<Func<PropertyHolder<object>, object>> expectedExpression = x => x.Property;
            var expectedComposer = new DelegatingComposer<PropertyHolder<object>>();
            var composer = new DelegatingComposer<PropertyHolder<object>>
            {
                OnAnonymousWith = f => f == expectedExpression ? expectedComposer : new DelegatingComposer<PropertyHolder<object>>()
            };

            var sut = new BehaviorPostprocessComposer<PropertyHolder<object>>(composer);
            // Exercise system
            var result = sut.With(expectedExpression);
            // Verify outcome
            var behaviorComposer = Assert.IsAssignableFrom<BehaviorPostprocessComposer<PropertyHolder<object>>>(result);
            Assert.Equal(expectedComposer, behaviorComposer.Composer);
            // Teardown
        }

        [Fact]
        public void WithReturnsCorrectResult()
        {
            // Fixture setup
            Expression<Func<PropertyHolder<object>, object>> expectedExpression = x => x.Property;
            var value = new object();

            var expectedComposer = new DelegatingComposer<PropertyHolder<object>>();
            var composer = new DelegatingComposer<PropertyHolder<object>>
            {
                OnWith = (f, v) => f == expectedExpression && v == value ? expectedComposer : new DelegatingComposer<PropertyHolder<object>>()
            };

            var sut = new BehaviorPostprocessComposer<PropertyHolder<object>>(composer);
            // Exercise system
            var result = sut.With(expectedExpression, value);
            // Verify outcome
            var behaviorComposer = Assert.IsAssignableFrom<BehaviorPostprocessComposer<PropertyHolder<object>>>(result);
            Assert.Equal(expectedComposer, behaviorComposer.Composer);
            // Teardown
        }

        [Fact]
        public void WithAutoPropertiesReturnsCorrectResult()
        {
            // Fixture setup
            var expectedComposer = new DelegatingComposer<object>();
            var composer = new DelegatingComposer<object> { OnWithAutoProperties = () => expectedComposer };

            var sut = new BehaviorPostprocessComposer<object>(composer);
            // Exercise system
            var result = sut.WithAutoProperties();
            // Verify outcome
            var behaviorComposer = Assert.IsAssignableFrom<BehaviorPostprocessComposer<object>>(result);
            Assert.Equal(expectedComposer, behaviorComposer.Composer);
            // Teardown
        }

        [Fact]
        public void WithoutReturnsCorrectResult()
        {
            // Fixture setup
            Expression<Func<PropertyHolder<object>, object>> expectedExpression = x => x.Property;
            var expectedComposer = new DelegatingComposer<PropertyHolder<object>>();
            var composer = new DelegatingComposer<PropertyHolder<object>>
            {
                OnWithout = f => f == expectedExpression ? expectedComposer : new DelegatingComposer<PropertyHolder<object>>()
            };

            var sut = new BehaviorPostprocessComposer<PropertyHolder<object>>(composer);
            // Exercise system
            var result = sut.Without(expectedExpression);
            // Verify outcome
            var behaviorComposer = Assert.IsAssignableFrom<BehaviorPostprocessComposer<PropertyHolder<object>>>(result);
            Assert.Equal(expectedComposer, behaviorComposer.Composer);
            // Teardown
        }

        [Fact]
        public void ComposeReturnsCorrectResult()
        {
            // Fixture setup
            var builder1 = new DelegatingSpecimenBuilder();
            var builder2 = new DelegatingSpecimenBuilder();
            var builder3 = new DelegatingSpecimenBuilder();

            var composer = new DelegatingComposer<object> { OnCompose = () => builder1 };
            var behaviors = new[]
                {
                    new DelegatingSpecimenBuilderTransformation { OnTransform = b => b == builder1 ? builder2 : new DelegatingSpecimenBuilder() },
                    new DelegatingSpecimenBuilderTransformation { OnTransform = b => b == builder2 ? builder3 : new DelegatingSpecimenBuilder() }
                };

            var sut = new BehaviorPostprocessComposer<object>(composer, behaviors);
            // Exercise system
            var result = sut.Compose();
            // Verify outcome
            Assert.Equal(builder3, result);
            // Teardown
        }

        [Fact]
        public void ComposeWithNoBehaviorsReturnsCorrectResult()
        {
            // Fixture setup
            var expectedBuilder = new DelegatingSpecimenBuilder();
            var composer = new DelegatingComposer<object> { OnCompose = () => expectedBuilder };
            var sut = new BehaviorPostprocessComposer<object>(composer);
            // Exercise system
            var result = sut.Compose();
            // Verify outcome
            Assert.Equal(expectedBuilder, result);
            // Teardown
        }
    }
}
