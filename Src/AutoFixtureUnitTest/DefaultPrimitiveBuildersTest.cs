using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixture;
using System.Collections;

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
                    typeof(StringSeedRelay),
                    typeof(Int32SequenceGenerator),
                    typeof(DateTimeGenerator),
                    typeof(DecimalSequenceGenerator),
                    typeof(BooleanSwitch),
                    typeof(GuidGenerator),
                    typeof(Int64SequenceGenerator),
                    typeof(UInt64SequenceGenerator),
                    typeof(UInt32SequenceGenerator),
                    typeof(Int16SequenceGenerator),
                    typeof(UInt16SequenceGenerator),
                    typeof(ByteSequenceGenerator),
                    typeof(SByteSequenceGenerator),
                    typeof(SingleSequenceGenerator),
                    typeof(DoubleSequenceGenerator),
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
