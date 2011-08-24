using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Dsl;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Dsl;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class BehaviorComposerTest
    {
        [Fact]
        public void SutIsPostprocessComposer()
        {
            // Fixture setup
            var dummyComposer = new DelegatingComposer<object>();
            // Exercise system
            var sut = new BehaviorComposer<object>(dummyComposer);
            // Verify outcome
            Assert.IsAssignableFrom<ICustomizationComposer<object>>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullComposerThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new BehaviorComposer<object>(null));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullBehaviorArrayThrows()
        {
            // Fixture setup
            var dummyComposer = new DelegatingComposer<object>();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new BehaviorComposer<object>(dummyComposer, (ISpecimenBuilderTransformation[])null));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullComposerAndEnumerableThrows()
        {
            // Fixture setup
            var dummyBehaviors = Enumerable.Empty<ISpecimenBuilderTransformation>();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new BehaviorComposer<object>(null, dummyBehaviors));
            // Teardown
        }

        [Fact]
        public void InitializeWithComposerAndNullEnumerableThrows()
        {
            // Fixture setup
            var dummyComposer = new DelegatingComposer<object>();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new BehaviorComposer<object>(dummyComposer, (IEnumerable<ISpecimenBuilderTransformation>)null));
            // Teardown
        }

        [Fact]
        public void InitializedWithArrayConstructorHasCorrectComposer()
        {
            // Fixture setup
            var expectedComposer = new DelegatingComposer<object>();
            var sut = new BehaviorComposer<object>(expectedComposer);
            // Exercise system
            ICustomizationComposer<object> result = sut.Composer;
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
            var sut = new BehaviorComposer<object>(expectedComposer, dummyBehaviors);
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
            var sut = new BehaviorComposer<object>(dummyComposer, expectedBehaviors);
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
            var sut = new BehaviorComposer<object>(dummyComposer, expectedBehaviors);
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
            var sut = new BehaviorComposer<object>(dummyComposer, behaviors);
            var expectedBehaviors = sut.Behaviors;
            // Exercise system
            var result = sut.Behaviors;
            // Verify outcome
            Assert.True(expectedBehaviors.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void WithNullCustomizationComposerThrows()
        {
            // Fixture setup
            var dummyComposer = new DelegatingComposer<object>();
            var sut = new BehaviorComposer<object>(dummyComposer);
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.With((ICustomizationComposer<object>)null));
            // Teardown
        }

        [Fact]
        public void WithCustomizationComposerReturnsResultWithCorrectComposer()
        {
            // Fixture setup
            var dummyComposer = new DelegatingComposer<object>();
            var sut = new BehaviorComposer<object>(dummyComposer);
            var expectedComposer = new DelegatingComposer<object>();
            // Exercise system
            var result = sut.With(expectedComposer);
            // Verify outcome
            Assert.Equal(expectedComposer, result.Composer);
            // Teardown
        }

        [Fact]
        public void WithCustomizationComposerReturnsResultWithCorrectBehaviors()
        {
            // Fixture setup
            var dummyComposer1 = new DelegatingComposer<object>();
            var expectedBehaviors = Enumerable.Range(1, 3)
                .Select(i => new DelegatingSpecimenBuilderTransformation())
                .ToArray();
            var sut = new BehaviorComposer<object>(dummyComposer1, expectedBehaviors);
            // Exercise system
            var dummyComposer2 = new DelegatingComposer<object>();
            var result = sut.With(dummyComposer2);
            // Verify outcome
            Assert.True(expectedBehaviors.SequenceEqual(result.Behaviors));
            // Teardown
        }

        [Fact]
        public void WithNullPostprocessComposerThrows()
        {
            // Fixture setup
            var dummyComposer = new DelegatingComposer<object>();
            var sut = new BehaviorComposer<object>(dummyComposer);
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.With((IPostprocessComposer<object>)null));
            // Teardown
        }

        [Fact]
        public void WithPostprocessComposerReturnsResultWithCorrectComposer()
        {
            // Fixture setup
            var dummyComposer = new DelegatingComposer<object>();
            var sut = new BehaviorComposer<object>(dummyComposer);
            IPostprocessComposer<object> expectedComposer = new DelegatingComposer<object>();
            // Exercise system
            var result = sut.With(expectedComposer);
            // Verify outcome
            Assert.Equal(expectedComposer, result.Composer);
            // Teardown
        }

        [Fact]
        public void WithPostprocessComposerReturnsResultWithCorrectBehaviors()
        {
            // Fixture setup
            var dummyComposer1 = new DelegatingComposer<object>();
            var expectedBehaviors = Enumerable.Range(1, 3)
                .Select(i => new DelegatingSpecimenBuilderTransformation())
                .ToArray();
            var sut = new BehaviorComposer<object>(dummyComposer1, expectedBehaviors);
            // Exercise system
            IPostprocessComposer<object> dummyComposer2 = new DelegatingComposer<object>();
            var result = sut.With(dummyComposer2);
            // Verify outcome
            Assert.True(expectedBehaviors.SequenceEqual(result.Behaviors));
            // Teardown
        }

        [Fact]
        public void FromSeedReturnsCorrectResult()
        {
            // Fixture setup
            Func<object, object> expectedFactory = s => s;
            var expectedComposer = new DelegatingComposer<object>();
            var composer = new DelegatingComposer { OnFromSeed = f => f == expectedFactory ? expectedComposer : new DelegatingComposer<object>() };

            var sut = new BehaviorComposer<object>(composer);
            // Exercise system
            var result = sut.FromSeed(expectedFactory);
            // Verify outcome
            var behaviorComposer = Assert.IsAssignableFrom<BehaviorPostprocessComposer<object>>(result);
            Assert.Equal(expectedComposer, behaviorComposer.Composer);
            // Teardown
        }

        [Fact]
        public void FromBuilderFactoryReturnsCorrectResult()
        {
            // Fixture setup
            var expectedFactory = new DelegatingSpecimenBuilder();
            var expectedComposer = new DelegatingComposer<Version>();
            var composer = new DelegatingComposer<Version> { OnFromBuilder = f => f == expectedFactory ? expectedComposer : new DelegatingComposer<Version>() };

            var sut = new BehaviorComposer<Version>(composer);
            // Exercise system
            var result = sut.FromFactory(expectedFactory);
            // Verify outcome
            var behaviorComposer = Assert.IsAssignableFrom<BehaviorPostprocessComposer<Version>>(result);
            Assert.Equal(expectedComposer, behaviorComposer.Composer);
            // Teardown
        }

        [Fact]
        public void FromZeroInputFactoryReturnsCorrectResult()
        {
            // Fixture setup
            Func<object> expectedFactory = () => new object();
            var expectedComposer = new DelegatingComposer<object>();
            var composer = new DelegatingComposer { OnFromFactory = f => f == expectedFactory ? expectedComposer : new DelegatingComposer<object>() };

            var sut = new BehaviorComposer<object>(composer);
            // Exercise system
            var result = sut.FromFactory(expectedFactory);
            // Verify outcome
            var behaviorComposer = Assert.IsAssignableFrom<BehaviorPostprocessComposer<object>>(result);
            Assert.Equal(expectedComposer, behaviorComposer.Composer);
            // Teardown
        }

        [Fact]
        public void FromSingleInputFactoryReturnsCorrectResult()
        {
            // Fixture setup
            Func<object, object> expectedFactory = s => s;
            var expectedComposer = new DelegatingComposer<object>();
            var composer = new DelegatingComposer { OnFromOverloadeFactory = f => f.Equals(expectedFactory) ? expectedComposer : new DelegatingComposer<object>() };

            var sut = new BehaviorComposer<object>(composer);
            // Exercise system
            var result = sut.FromFactory(expectedFactory);
            // Verify outcome
            var behaviorComposer = Assert.IsAssignableFrom<BehaviorPostprocessComposer<object>>(result);
            Assert.Equal(expectedComposer, behaviorComposer.Composer);
            // Teardown
        }

        [Fact]
        public void FromDoubleInputFactoryReturnsCorrectResult()
        {
            // Fixture setup
            Func<object, object, object> expectedFactory = (x, y) => x;
            var expectedComposer = new DelegatingComposer<object>();
            var composer = new DelegatingComposer { OnFromOverloadeFactory = f => f.Equals(expectedFactory) ? expectedComposer : new DelegatingComposer<object>() };

            var sut = new BehaviorComposer<object>(composer);
            // Exercise system
            var result = sut.FromFactory(expectedFactory);
            // Verify outcome
            var behaviorComposer = Assert.IsAssignableFrom<BehaviorPostprocessComposer<object>>(result);
            Assert.Equal(expectedComposer, behaviorComposer.Composer);
            // Teardown
        }

        [Fact]
        public void FromTripleInputFactoryReturnsCorrectResult()
        {
            // Fixture setup
            Func<object, object, object, object> expectedFactory = (x, y, z) => x;
            var expectedComposer = new DelegatingComposer<object>();
            var composer = new DelegatingComposer { OnFromOverloadeFactory = f => f.Equals(expectedFactory) ? expectedComposer : new DelegatingComposer<object>() };

            var sut = new BehaviorComposer<object>(composer);
            // Exercise system
            var result = sut.FromFactory(expectedFactory);
            // Verify outcome
            var behaviorComposer = Assert.IsAssignableFrom<BehaviorPostprocessComposer<object>>(result);
            Assert.Equal(expectedComposer, behaviorComposer.Composer);
            // Teardown
        }

        [Fact]
        public void FromQuadrupleInputFactoryReturnsCorrectResult()
        {
            // Fixture setup
            Func<object, object, object, object, object> expectedFactory = (x, y, z, æ) => x;
            var expectedComposer = new DelegatingComposer<object>();
            var composer = new DelegatingComposer { OnFromOverloadeFactory = f => f.Equals(expectedFactory) ? expectedComposer : new DelegatingComposer<object>() };

            var sut = new BehaviorComposer<object>(composer);
            // Exercise system
            var result = sut.FromFactory(expectedFactory);
            // Verify outcome
            var behaviorComposer = Assert.IsAssignableFrom<BehaviorPostprocessComposer<object>>(result);
            Assert.Equal(expectedComposer, behaviorComposer.Composer);
            // Teardown
        }

        [Fact]
        public void DoReturnsCorrectResult()
        {
            // Fixture setup
            Action<object> expectedAction = s => { };
            var expectedComposer = new DelegatingComposer<object>();
            var composer = new DelegatingComposer<object> { OnDo = a => a == expectedAction ? expectedComposer : new DelegatingComposer<object>() };

            var sut = new BehaviorComposer<object>(composer);
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

            var sut = new BehaviorComposer<object>(composer);
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

            var sut = new BehaviorComposer<PropertyHolder<object>>(composer);
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

            var sut = new BehaviorComposer<PropertyHolder<object>>(composer);
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

            var sut = new BehaviorComposer<object>(composer);
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

            var sut = new BehaviorComposer<PropertyHolder<object>>(composer);
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

            var sut = new BehaviorComposer<object>(composer, behaviors);
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
            var sut = new BehaviorComposer<object>(composer);
            // Exercise system
            var result = sut.Compose();
            // Verify outcome
            Assert.Equal(expectedBuilder, result);
            // Teardown
        }
    }
}
