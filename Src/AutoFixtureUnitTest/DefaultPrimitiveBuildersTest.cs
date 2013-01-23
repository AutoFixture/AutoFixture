﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class DefaultPrimitiveBuildersTest
    {
        [Fact]
        public void SutIsSpecimenBuilders()
        {
            // Fixture setup
            // Exercise system
            var sut = new DefaultPrimitiveBuilders();
            // Verify outcome
            Assert.IsAssignableFrom<IEnumerable<ISpecimenBuilder>>(sut);
            // Teardown
        }

        [Fact]
        public void SutHasCorrectContents()
        {
            // Fixture setup
            var expectedBuilderTypes = new[]
                {
                    typeof(StringGenerator),
                    typeof(ConstrainedStringGenerator),
                    typeof(StringSeedRelay),
                    typeof(RandomNumericSequenceGenerator),
                    typeof(CharSequenceGenerator),
                    typeof(UriGenerator),
                    typeof(UriSchemeGenerator),
                    typeof(RangedNumberGenerator),
                    typeof(RegularExpressionGenerator),
                    typeof(StrictlyMonotonicallyIncreasingDateTimeGenerator),
                    typeof(BooleanSwitch),
                    typeof(GuidGenerator),
                    typeof(TypeGenerator),
                    typeof(DelegateGenerator),
                    typeof(IntPtrGuard)
                };
            // Exercise system
            var sut = new DefaultPrimitiveBuilders();
            // Verify outcome
            Assert.True(expectedBuilderTypes.SequenceEqual(sut.Select(b => b.GetType())));
            // Teardown
        }

        [Fact]
        public void NonGenericEnumeratorMatchesGenericEnumerator()
        {
            // Fixture setup
            var sut = new DefaultPrimitiveBuilders();
            // Exercise system
            IEnumerable result = sut;
            // Verify outcome
            Assert.True(sut.Select(b => b.GetType()).SequenceEqual(result.Cast<object>().Select(o => o.GetType())));
            // Teardown
        }

        [Fact]
        public void StringGeneratorHasFactoryThatCreatesCorrectType()
        {
            // Fixture setup
            var sut = new DefaultPrimitiveBuilders();
            // Exercise system
            var result = sut.OfType<StringGenerator>().Single();
            // Verify outcome
            Assert.IsAssignableFrom<Guid>(result.Factory());
            // Teardown
        }

        [Fact]
        public void StringGeneratorFactoryReturnsNewInstancesForEachCall()
        {
            // Fixture setup
            var sut = new DefaultPrimitiveBuilders();
            // Exercise system
            var result = sut.OfType<StringGenerator>().Single();
            // Verify outcome
            Assert.NotEqual(result.Factory(), result.Factory());
            // Teardown
        }
    }
}
