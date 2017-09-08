using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Dsl;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.DataAnnotations;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class FixtureTest
    {
        [Fact]
        public void InitializedWithDefaultConstructorSutHasCorrectEngineParts()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.Engine;
            // Verify outcome
            var expectedParts = from b in new DefaultEngineParts()
                                select b.GetType();
            var composite = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(result);
            Assert.True(expectedParts.SequenceEqual(from b in composite
                                                    select b.GetType()));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullRelaysThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new Fixture(null));
            // Teardown
        }

        [Fact]
        public void InitializedWithRelaysSutHasCorrectEngineParts()
        {
            // Fixture setup
            var relays = new DefaultRelays();
            var sut = new Fixture(relays);
            // Exercise system
            var result = sut.Engine;
            // Verify outcome
            var expectedParts = from b in relays
                                select b.GetType();
            var composite = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(result);
            Assert.True(expectedParts.SequenceEqual(from b in composite
                                                    select b.GetType()));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullEngineThrows()
        {
            // Fixture setup
            var dummyMany = new MultipleRelay();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new Fixture(null, dummyMany));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullManyThrows()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new Fixture(dummyBuilder, null));
            // Teardown
        }

        [Fact]
        public void InitializedWithEngineSutHasCorrectEngine()
        {
            // Fixture setup
            var expectedEngine = new DelegatingSpecimenBuilder();
            var dummyMany = new MultipleRelay();
            var sut = new Fixture(expectedEngine, dummyMany);
            // Exercise system
            var result = sut.Engine;
            // Verify outcome
            Assert.Equal(expectedEngine, result);
            // Teardown
        }

        [Fact]
        public void InitializedWithManySutHasCorrectRepeatCount()
        {
            // Fixture setup
            var expectedRepeatCount = 187;
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var many = new MultipleRelay { Count = expectedRepeatCount };
            var sut = new Fixture(dummyBuilder, many);
            // Exercise system
            var result = sut.RepeatCount;
            // Verify outcome
            Assert.Equal(expectedRepeatCount, result);
            // Teardown
        }

        [Fact]
        public void SettingRepeatCountWillCorrectlyUpdateMany()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var many = new MultipleRelay();
            var sut = new Fixture(dummyBuilder, many);
            // Exercise system
            sut.RepeatCount = 26;
            // Verify outcome
            Assert.Equal(sut.RepeatCount, many.Count);
            // Teardown
        }

        [Fact]
        public void CustomizationsIsInstance()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            IList<ISpecimenBuilder> result = sut.Customizations;
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void CustomizationsIsStable()
        {
            // Fixture setup
            var sut = new Fixture();
            var builder = new DelegatingSpecimenBuilder();
            // Exercise system
            sut.Customizations.Add(builder);
            // Verify outcome
            Assert.Contains(builder, sut.Customizations);
            // Teardown
        }

        [Fact]
        public void ResidueCollectorsIsInstance()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            IList<ISpecimenBuilder> result = sut.ResidueCollectors;
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void ResidueCollectorsIsStable()
        {
            // Fixture setup
            var sut = new Fixture();
            var builder = new DelegatingSpecimenBuilder();
            // Exercise system
            sut.ResidueCollectors.Add(builder);
            // Verify outcome
            Assert.Contains(builder, sut.ResidueCollectors);
            // Teardown
        }

        [Fact]
        public void BehaviorsIsInstance()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            IList<ISpecimenBuilderTransformation> result = sut.Behaviors;
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void BehaviorsIsStable()
        {
            // Fixture setup
            var sut = new Fixture();
            var behavior = new DelegatingSpecimenBuilderTransformation();
            // Exercise system
            sut.Behaviors.Add(behavior);
            // Verify outcome
            Assert.Contains(behavior, sut.Behaviors);
            // Teardown
        }

        [Fact]
        public void BehaviorsContainsCorrectRecursionBehavior()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.Behaviors;
            // Verify outcome
            Assert.True(result.OfType<ThrowingRecursionBehavior>().Any());
            // Teardown
        }

        [Fact]
        public void SutIsCustomizableComposer()
        {
            // Fixture setup
            // Exercise system
            var sut = new Fixture();
            // Verify outcome
            Assert.IsAssignableFrom<IFixture>(sut);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWillCreateSimpleObject()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            // Exercise system
            object result = sut.Create<object>();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void CreateAnonymousCompatibilityExtensionWillCreateSimpleObject()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            // Exercise system
            object result = sut.CreateAnonymous<object>();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void CreateUnregisteredAbstractTypeWillThrow()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            // Exercise system and verify outcome
            Assert.Throws<ObjectCreationException>(() =>
                sut.Create<AbstractType>());
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWillCreateSingleParameterType()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            // Exercise system
            SingleParameterType<object> result = sut.Create<SingleParameterType<object>>();
            // Verify outcome
            Assert.NotNull(result.Parameter);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWillUseRegisteredMapping()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            sut.Register<AbstractType>(() => new ConcreteType());
            // Exercise system
            SingleParameterType<AbstractType> result = sut.Create<SingleParameterType<AbstractType>>();
            // Verify outcome
            Assert.IsAssignableFrom<ConcreteType>(result.Parameter);
            // Teardown
        }

        [Fact]
        public void CreateOnMultipleThreadsConcurrentlyGeneratesPopulatedSpecimens()
        {
            // Fixture setup
            const int specimenCountPerThread = 25;
            const int threadCount = 8;
            var sut = new Fixture();

            // Exercise system
            IEnumerable<object> GetPropertyAndFieldValues(object obj, BindingFlags flags)
            {
                var type = obj.GetType();
                foreach (var fieldInfo in type.GetFields(flags))
                {
                    yield return fieldInfo.GetValue(obj);
                }

                foreach (var propertyInfo in type.GetProperties(flags))
                {
                    yield return propertyInfo.GetValue(obj);
                }
            }

            var specimensByThread = Enumerable.Range(0, threadCount)
                .AsParallel()
                    .WithDegreeOfParallelism(threadCount)
                    .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                .Select(threadNumber => Enumerable
                    .Range(0, specimenCountPerThread)
                    .Select(_ => sut.Create<SpecimenWithEverything>())
                    .Select(s => new
                    {
                        Specimen = s,
                        threadNumber,
                        ValuesNotPopulated = GetPropertyAndFieldValues(s, BindingFlags.Public | BindingFlags.Instance)
                            .Where(v => v == null || 0.Equals(v))
                            .ToArray()
                    })
                    .ToArray())
                .ToArray();

            // Verify outcome
            Assert.Equal(specimenCountPerThread * threadCount, specimensByThread.Sum(t => t.Length));

            var allValuesNotPopulated = specimensByThread
                .SelectMany(t => t.SelectMany(s => s.ValuesNotPopulated));

            Assert.Empty(allValuesNotPopulated);

            // Teardown
        }

        [Fact]
        public void CreateAnonymousWillUseRegisteredMappingWithSingleParameter()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            sut.Register<object, AbstractType>(obj => new ConcreteType(obj));
            // Exercise system
            AbstractType result = sut.Create<AbstractType>();
            // Verify outcome
            Assert.NotNull(result.Property1);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWillUseRegisteredMappingWithDoubleParameters()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            sut.Register<object, object, AbstractType>((obj1, obj2) => new ConcreteType(obj1, obj2));
            // Exercise system
            AbstractType result = sut.Create<AbstractType>();
            // Verify outcome
            Assert.NotNull(result.Property1);
            Assert.NotNull(result.Property2);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWillUseRegisteredMappingWithTripleParameters()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            sut.Register<object, object, object, AbstractType>((obj1, obj2, obj3) => new ConcreteType(obj1, obj2, obj3));
            // Exercise system
            AbstractType result = sut.Create<AbstractType>();
            // Verify outcome
            Assert.NotNull(result.Property1);
            Assert.NotNull(result.Property2);
            Assert.NotNull(result.Property3);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWillUseRegisteredMappingWithQuadrupleParameters()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            sut.Register<object, object, object, object, AbstractType>((obj1, obj2, obj3, obj4) => new ConcreteType(obj1, obj2, obj3, obj4));
            // Exercise system
            AbstractType result = sut.Create<AbstractType>();
            // Verify outcome
            Assert.NotNull(result.Property1);
            Assert.NotNull(result.Property2);
            Assert.NotNull(result.Property3);
            Assert.NotNull(result.Property4);
            // Teardown
        }

        [Fact]
        public void CustomizeInstanceWillReturnFactory()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            // Exercise system
            var result = sut.Build<object>();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousStringWillPrefixName()
        {
            // Fixture setup
            string expectedText = "Anonymous text";
            Fixture sut = new Fixture();
            // Exercise system
            string result = sut.Create(expectedText);
            // Verify outcome
            string actualText = new TextGuidRegex().GetText(result);
            Assert.Equal(expectedText, actualText);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousStringWillAppendGuid()
        {
            // Fixture setup
            string anonymousText = "Anonymous text";
            Fixture sut = new Fixture();
            // Exercise system
            string result = sut.Create(anonymousText);
            // Verify outcome
            string guidString = new TextGuidRegex().GetGuid(result);
            Guid g = new Guid(guidString);
            Assert.NotEqual<Guid>(Guid.Empty, g);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithStringPropertyWillAssignNonEmptyString()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            // Exercise system
            PropertyHolder<string> result = sut.Create<PropertyHolder<string>>();
            // Verify outcome
            Assert.False(string.IsNullOrEmpty(result.Property), "Property should be assigned");
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithStringPropertyWillAppendPropertyNameToString()
        {
            // Fixture setup
            string expectedName = "Property";
            Fixture sut = new Fixture();
            // Exercise system
            PropertyHolder<string> result = sut.Create<PropertyHolder<string>>();
            // Verify outcome
            string propertyValue = result.Property;
            string text = new TextGuidRegex().GetText(propertyValue);
            Assert.Equal(expectedName, text);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithStringPropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<string> ph = sut.Create<PropertyHolder<string>>();
            // Exercise system
            PropertyHolder<string> result = sut.Create<PropertyHolder<string>>();
            // Verify outcome
            Assert.NotEqual<string>(ph.Property, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithBooleanPropertyWillAssignTrue()
        {
            // Fixture setup
            bool unexpectedBoolean = default(bool);
            Fixture sut = new Fixture();
            // Exercise system
            PropertyHolder<bool> result = sut.Create<PropertyHolder<bool>>();
            // Verify outcome
            Assert.NotEqual<bool>(unexpectedBoolean, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithCharPropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<char> ph = sut.Create<PropertyHolder<char>>();
            // Exercise system
            PropertyHolder<char> result = sut.Create<PropertyHolder<char>>();
            // Verify outcome
            Assert.NotEqual<char>(ph.Property, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithBooleanPropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<bool> ph = sut.Create<PropertyHolder<bool>>();
            // Exercise system
            PropertyHolder<bool> result = sut.Create<PropertyHolder<bool>>();
            // Verify outcome
            Assert.NotEqual<bool>(ph.Property, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithBytePropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            byte unexpectedByte = default(byte);
            Fixture sut = new Fixture();
            // Exercise system
            PropertyHolder<byte> result = sut.Create<PropertyHolder<byte>>();
            // Verify outcome
            Assert.NotEqual<byte>(unexpectedByte, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithBytePropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<byte> ph = sut.Create<PropertyHolder<byte>>();
            // Exercise system
            PropertyHolder<byte> result = sut.Create<PropertyHolder<byte>>();
            // Verify outcome
            Assert.NotEqual<byte>(ph.Property, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithSignedBytePropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            sbyte unexpectedSbyte = default(sbyte);
            Fixture sut = new Fixture();
            // Exercise system
            PropertyHolder<sbyte> result = sut.Create<PropertyHolder<sbyte>>();
            // Verify outcome
            Assert.NotEqual<sbyte>(unexpectedSbyte, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithSignedBytePropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<sbyte> ph = sut.Create<PropertyHolder<sbyte>>();
            // Exercise system
            PropertyHolder<sbyte> result = sut.Create<PropertyHolder<sbyte>>();
            // Verify outcome
            Assert.NotEqual<sbyte>(ph.Property, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithUnsignedInt16PropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            ushort unexpectedNumber = default(ushort);
            Fixture sut = new Fixture();
            // Exercise system
            PropertyHolder<ushort> result = sut.Create<PropertyHolder<ushort>>();
            // Verify outcome
            Assert.NotEqual<ushort>(unexpectedNumber, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithUnsignedInt16PropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<ushort> ph = sut.Create<PropertyHolder<ushort>>();
            // Exercise system
            PropertyHolder<ushort> result = sut.Create<PropertyHolder<ushort>>();
            // Verify outcome
            Assert.NotEqual<ushort>(ph.Property, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithInt16PropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            short unexpectedNumber = default(short);
            Fixture sut = new Fixture();
            // Exercise system
            PropertyHolder<short> result = sut.Create<PropertyHolder<short>>();
            // Verify outcome
            Assert.NotEqual<short>(unexpectedNumber, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithInt16PropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<short> ph = sut.Create<PropertyHolder<short>>();
            // Exercise system
            PropertyHolder<short> result = sut.Create<PropertyHolder<short>>();
            // Verify outcome
            Assert.NotEqual<short>(ph.Property, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithUnsignedInt32PropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            uint unexpectedNumber = default(uint);
            Fixture sut = new Fixture();
            // Exercise system
            PropertyHolder<uint> result = sut.Create<PropertyHolder<uint>>();
            // Verify outcome
            Assert.NotEqual<uint>(unexpectedNumber, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithUnsignedInt32PropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<uint> ph = sut.Create<PropertyHolder<uint>>();
            // Exercise system
            PropertyHolder<uint> result = sut.Create<PropertyHolder<uint>>();
            // Verify outcome
            Assert.NotEqual<uint>(ph.Property, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithInt32PropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            int unexpectedNumber = default(int);
            Fixture sut = new Fixture();
            // Exercise system
            PropertyHolder<int> result = sut.Create<PropertyHolder<int>>();
            // Verify outcome
            Assert.NotEqual<int>(unexpectedNumber, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithInt32PropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<int> ph = sut.Create<PropertyHolder<int>>();
            // Exercise system
            PropertyHolder<int> result = sut.Create<PropertyHolder<int>>();
            // Verify outcome
            Assert.NotEqual<int>(ph.Property, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithUnsignedInt64PropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            ulong unexpectedNumber = default(ulong);
            Fixture sut = new Fixture();
            // Exercise system
            PropertyHolder<ulong> result = sut.Create<PropertyHolder<ulong>>();
            // Verify outcome
            Assert.NotEqual<ulong>(unexpectedNumber, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithUnsignedInt64PropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<ulong> ph = sut.Create<PropertyHolder<ulong>>();
            // Exercise system
            PropertyHolder<ulong> result = sut.Create<PropertyHolder<ulong>>();
            // Verify outcome
            Assert.NotEqual<ulong>(ph.Property, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithInt64PropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            long unexpectedNumber = default(long);
            Fixture sut = new Fixture();
            // Exercise system
            PropertyHolder<long> result = sut.Create<PropertyHolder<long>>();
            // Verify outcome
            Assert.NotEqual<long>(unexpectedNumber, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithInt64PropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<long> ph = sut.Create<PropertyHolder<long>>();
            // Exercise system
            PropertyHolder<long> result = sut.Create<PropertyHolder<long>>();
            // Verify outcome
            Assert.NotEqual<long>(ph.Property, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithDecimalPropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            decimal unexpectedNumber = default(decimal);
            Fixture sut = new Fixture();
            // Exercise system
            PropertyHolder<decimal> result = sut.Create<PropertyHolder<decimal>>();
            // Verify outcome
            Assert.NotEqual<decimal>(unexpectedNumber, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithDecimalPropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<decimal> ph = sut.Create<PropertyHolder<decimal>>();
            // Exercise system
            PropertyHolder<decimal> result = sut.Create<PropertyHolder<decimal>>();
            // Verify outcome
            Assert.NotEqual<decimal>(ph.Property, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithSinglePropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            float unexpectedNumber = default(float);
            Fixture sut = new Fixture();
            // Exercise system
            PropertyHolder<float> result = sut.Create<PropertyHolder<float>>();
            // Verify outcome
            Assert.NotEqual<float>(unexpectedNumber, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithSinglePropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<float> ph = sut.Create<PropertyHolder<float>>();
            // Exercise system
            PropertyHolder<float> result = sut.Create<PropertyHolder<float>>();
            // Verify outcome
            Assert.NotEqual<float>(ph.Property, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithDoublePropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            double unexpectedNumber = default(double);
            Fixture sut = new Fixture();
            // Exercise system
            PropertyHolder<double> result = sut.Create<PropertyHolder<double>>();
            // Verify outcome
            Assert.NotEqual<double>(unexpectedNumber, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithDoublePropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<double> ph = sut.Create<PropertyHolder<double>>();
            // Exercise system
            PropertyHolder<double> result = sut.Create<PropertyHolder<double>>();
            // Verify outcome
            Assert.NotEqual<double>(ph.Property, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithDoubleMixedWholeNumericPropertiesWillAssignDifferentValues()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.Create<DoublePropertyHolder<int, long>>();
            // Verify outcome
            Assert.NotEqual(result.Property1, result.Property2);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithDoubleMixedSmallWholeNumericPropertiesWillAssignDifferentValues()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.Create<DoublePropertyHolder<byte, short>>();
            // Verify outcome
            Assert.NotEqual(result.Property1, result.Property2);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithDoubleMixedFloatingPointNumericPropertiesWillAssignDifferentValues()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.Create<DoublePropertyHolder<double, float>>();
            // Verify outcome
            Assert.NotEqual(result.Property1, result.Property2);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithDoubleMixedNumericPropertiesWillAssignDifferentValues()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.Create<DoublePropertyHolder<long, float>>();
            // Verify outcome
            Assert.NotEqual(result.Property1, result.Property2);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithNumericSequenceCustomizationAndDoubleMixedWholeNumericPropertiesWillAssignSameValue()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            sut.Customize(new NumericSequencePerTypeCustomization());
            var result = sut.Create<DoublePropertyHolder<int, long>>();
            // Verify outcome
            Assert.Equal(result.Property1, result.Property2);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithNumericSequenceCustomizationAndDoubleMixedSmallWholeNumericPropertiesWillAssignSameValue()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            sut.Customize(new NumericSequencePerTypeCustomization());
            var result = sut.Create<DoublePropertyHolder<byte, short>>();
            // Verify outcome
            Assert.Equal(result.Property1, result.Property2);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithNumericSequenceCustomizationAndDoubleMixedFloatingPointNumericPropertiesWillAssignSameValue()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            sut.Customize(new NumericSequencePerTypeCustomization());
            var result = sut.Create<DoublePropertyHolder<double, float>>();
            // Verify outcome
            Assert.Equal(result.Property1, result.Property2);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithNumericSequenceCustomizationAndDoubleMixedNumericPropertiesWillAssignSameValue()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            sut.Customize(new NumericSequencePerTypeCustomization());
            var result = sut.Create<DoublePropertyHolder<long, float>>();
            // Verify outcome
            Assert.Equal(result.Property1, result.Property2);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithRandomNumericSequenceCustomizationReturnsRandomNumbers()
        {
            // Fixture setup
            var sut = new Fixture();
            sut.Customizations.Add(new RandomNumericSequenceGenerator(15, 30));
            var definedNumbers = new object[]
            {
                1,
                (uint)2,
                (byte)3,
                (sbyte)4,
                (long)5,
                (ulong)6,
                (short)7,
                (ushort)8,
                9.0F,
                10.0D,
                11M
            };
            // Exercise system
            var randomNumbers = new object[]
            {
                sut.Create<int>(),
                sut.Create<uint>(),
                sut.Create<byte>(),
                sut.Create<sbyte>(),
                sut.Create<long>(),
                sut.Create<ulong>(),
                sut.Create<short>(),
                sut.Create<ushort>(),
                sut.Create<float>(),
                sut.Create<double>(),
                sut.Create<decimal>()
            };
            var result = randomNumbers.Intersect(definedNumbers);
            // Verify outcome
            Assert.Empty(result);
            // Teardown
        }

        [Fact]
        public void InjectCustomUpperLimitWillCauseSutToReturnNumbersInLimit()
        {
            // Fixture setup
            int lower = -9;
            int upper = -1;
            var sut = new Fixture();
            sut.Customizations.Add(new RandomNumericSequenceGenerator(lower, upper));
            // Exercise system
            var result = sut.Create<DoublePropertyHolder<int, long>>();
            // Verify outcome
            Assert.True(
                (result.Property1 >= lower && result.Property1 <= upper) &&
                (result.Property2 >= lower && result.Property2 <= upper)
                );
        }

        [Fact]
        public void CreateAnonymousWithGuidPropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            Guid unexpectedGuid = default(Guid);
            var sut = new Fixture();
            // Exercise system
            var result = sut.Create<PropertyHolder<Guid>>();
            // Verify outcome
            Assert.NotEqual<Guid>(unexpectedGuid, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithGuidPropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            var sut = new Fixture();
            var ph = sut.Create<PropertyHolder<Guid>>();
            // Exercise system
            var result = sut.Create<PropertyHolder<Guid>>();
            // Verify outcome
            Assert.NotEqual<Guid>(ph.Property, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithFlagEnumPropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            var sut = new Fixture();
            var ph = sut.Create<PropertyHolder<ActivityScope>>();
            // Exercise system
            var result = sut.Create<PropertyHolder<ActivityScope>>();
            // Verify outcome
            Assert.NotEqual<ActivityScope>(ph.Property, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithFlagEnumPropertyMultipleTimesWillAssignValidValues()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.Build<PropertyHolder<ActivityScope?>>().CreateMany(100);
            // Verify outcome
            long activityMin = 0;
            long activityMax = (long)ActivityScope.All;
            foreach (var propertyHolder in result)
            {
                long activityScope = (long)propertyHolder.Property;
                Assert.InRange(activityScope, activityMin, activityMax);
            }
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithDoubleDateTimePropertiesWillAssignDifferentDateTimes()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.Create<DoublePropertyHolder<DateTime, DateTime>>();
            // Verify outcome
            Assert.NotEqual(result.Property1, result.Property2);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithDateTimePropertyAndIncrementingDateTimeCustomizationTwiceWithinMsReturnsDateTimesExactlyOneDayApart()
        {
            // Fixture setup
            var nowResolution = TimeSpan.FromMilliseconds(10); // see http://msdn.microsoft.com/en-us/library/system.datetime.now.aspx
            var sut = new Fixture();
            sut.Customize(new IncrementingDateTimeCustomization());
            // Exercise system
            var firstResult = sut.Create<PropertyHolder<DateTime>>();
            Thread.Sleep(nowResolution + nowResolution);
            var secondResult = sut.Create<PropertyHolder<DateTime>>();
            // Verify outcome
            Assert.Equal(firstResult.Property.AddDays(1), secondResult.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithDoubleDateTimePropertiesAndCurrentDateTimeCustomizationWillAssignEqualDates()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            sut.Customize(new CurrentDateTimeCustomization());
            var result = sut.Create<DoublePropertyHolder<DateTime, DateTime>>();
            // Verify outcome
            Assert.Equal(result.Property1.Date, result.Property2.Date);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithArrayPropertyCorrectlyAssignsArray()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.Create<PropertyHolder<int[]>>();
            // Verify outcome
            Assert.NotEmpty(result.Property);
            Assert.True(result.Property.All(i => i != 0));
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithVoidParameterlessDelegatePropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            var unexpectedDelegate = default(Action);
            var sut = new Fixture();
            // Exercise system
            var result = sut.Create<PropertyHolder<Action>>();
            // Verify outcome
            Assert.NotEqual<Action>(unexpectedDelegate, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithVoidSingleObjectParameterDelegatePropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            var unexpectedDelegate = default(Action<object>);
            var sut = new Fixture();
            // Exercise system
            var result = sut.Create<PropertyHolder<Action<object>>>();
            // Verify outcome
            Assert.NotEqual<Action<object>>(unexpectedDelegate, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithVoidSingleSpecializedObjectParameterDelegatePropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            var unexpectedValue = default(Action<string>);
            var sut = new Fixture();
            // Exercise system
            var result = sut.Create<PropertyHolder<Action<string>>>();
            // Verify outcome
            Assert.NotEqual<Action<string>>(unexpectedValue, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithVoidSingleValueParameterDelegatePropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            var unexpectedValue = default(Action<int>);
            var sut = new Fixture();
            // Exercise system
            var result = sut.Create<PropertyHolder<Action<int>>>();
            // Verify outcome
            Assert.NotEqual<Action<int>>(unexpectedValue, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithVoidDoubleObjectParametersDelegatePropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            var unexpectedDelegate = default(Action<object, object>);
            var sut = new Fixture();
            // Exercise system
            var result = sut.Create<PropertyHolder<Action<object, object>>>();
            // Verify outcome
            Assert.NotEqual<Action<object, object>>(unexpectedDelegate, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithVoidDoubleSpecializedObjectParametersDelegatePropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            var unexpectedDelegate = default(Action<string, string>);
            var sut = new Fixture();
            // Exercise system
            var result = sut.Create<PropertyHolder<Action<string, string>>>();
            // Verify outcome
            Assert.NotEqual<Action<string, string>>(unexpectedDelegate, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithVoidDoubleValueParametersDelegatePropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            var unexpectedDelegate = default(Action<int, bool>);
            var sut = new Fixture();
            // Exercise system
            var result = sut.Create<PropertyHolder<Action<int, bool>>>();
            // Verify outcome
            Assert.NotEqual<Action<int, bool>>(unexpectedDelegate, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithVoidParameterlessDelegatePropertyWillAssignDelegateNotThrowing()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.Create<PropertyHolder<Action>>();
            // Verify outcome
            Assert.Null(Record.Exception(() => ((Action)result.Property).Invoke()));
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithReturnObjectParameterlessDelegatePropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            var unexpectedDelegate = default(Func<object>);
            var sut = new Fixture();
            // Exercise system
            var result = sut.Create<PropertyHolder<Func<object>>>();
            // Verify outcome
            Assert.NotEqual<Func<object>>(unexpectedDelegate, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithReturnObjectSingleObjectParameterDelegatePropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            var unexpectedDelegate = default(Func<object, object>);
            var sut = new Fixture();
            // Exercise system
            var result = sut.Create<PropertyHolder<Func<object, object>>>();
            // Verify outcome
            Assert.NotEqual<Func<object, object>>(unexpectedDelegate, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithReturnObjectSingleSpecializedObjectParameterDelegatePropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            var unexpectedDelegate = default(Func<string, object>);
            var sut = new Fixture();
            // Exercise system
            var result = sut.Create<PropertyHolder<Func<string, object>>>();
            // Verify outcome
            Assert.NotEqual<Func<string, object>>(unexpectedDelegate, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithReturnObjectSingleValueParameterDelegatePropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            var unexpectedDelegate = default(Func<int, object>);
            var sut = new Fixture();
            // Exercise system
            var result = sut.Create<PropertyHolder<Func<int, object>>>();
            // Verify outcome
            Assert.NotEqual<Func<int, object>>(unexpectedDelegate, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithReturnValueParameterlessDelegatePropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            var unexpectedDelegate = default(Func<int>);
            var sut = new Fixture();
            // Exercise system
            var result = sut.Create<PropertyHolder<Func<int>>>();
            // Verify outcome
            Assert.NotEqual<Func<int>>(unexpectedDelegate, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithReturnValueSingleObjectParameterDelegatePropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            var unexpectedDelegate = default(Func<int, object>);
            var sut = new Fixture();
            // Exercise system
            var result = sut.Create<PropertyHolder<Func<int, object>>>();
            // Verify outcome
            Assert.NotEqual<Func<int, object>>(unexpectedDelegate, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithReturnValueSingleSpecializedObjectParameterDelegatePropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            var unexpectedDelegate = default(Func<int, string>);
            var sut = new Fixture();
            // Exercise system
            var result = sut.Create<PropertyHolder<Func<int, string>>>();
            // Verify outcome
            Assert.NotEqual<Func<int, string>>(unexpectedDelegate, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithReturnObjectParameterlessDelegatePropertyWillAssignDelegateReturningNonDefaultValue()
        {
            // Fixture setup
            var unexpectedResult = default(string);
            var sut = new Fixture();
            // Exercise system
            var result = sut.Create<PropertyHolder<Func<string>>>();
            // Verify outcome
            var actualResult = ((Func<string>)result.Property).Invoke();
            Assert.NotEqual<string>(unexpectedResult, actualResult);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithReturnValueParameterlessDelegatePropertyWillAssignDelegateReturningNonDefaultValue()
        {
            // Fixture setup
            var unexpectedResult = default(int);
            var sut = new Fixture();
            // Exercise system
            var result = sut.Create<PropertyHolder<Func<int>>>();
            // Verify outcome
            var actualResult = ((Func<int>)result.Property).Invoke();
            Assert.NotEqual<int>(unexpectedResult, actualResult);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithTypeWithFactoryMethodWillInvokeFactoryMethod()
        {
            // Fixture setup
            var fixture = new Fixture();
            var result = fixture.Create<TypeWithFactoryMethod>();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithTypeWithFactoryPropertyWillInvokeFactoryProperty()
        {
            // Fixture setup
            var fixture = new Fixture();
            var result = fixture.Create<TypeWithFactoryProperty>();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedDecimalPropertyReturnsCorrectResultForIntegerRange()
        {
            // Fixture setup
            var fixture = new Fixture();
            var result = fixture.Create<RangeValidatedType>();
            // Verify outcome
            Assert.True(
                RangeValidatedType.Minimum <= result.Property && result.Property <= RangeValidatedType.Maximum,
                string.Format(
                    "Expected result to fall into the interval [{0}, {1}], but was {2}",
                    RangeValidatedType.Minimum,
                    RangeValidatedType.Maximum,
                    result.Property));
            // Teardown
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedDecimalFieldReturnsCorrectResultForIntegerRange()
        {
            // Fixture setup
            var fixture = new Fixture();
            var result = fixture.Create<RangeValidatedType>();
            // Verify outcome
            Assert.True(result.Field >= RangeValidatedType.Minimum && result.Field <= RangeValidatedType.Maximum);
            // Teardown
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedDoublePropertyReturnsCorrectResultForDoubleRange()
        {
            // Fixture setup
            var fixture = new Fixture();
            var result = fixture.Create<RangeValidatedType>();
            // Verify outcome
            Assert.True(result.Property2 >= RangeValidatedType.DoubleMinimum && result.Property2 <= RangeValidatedType.DoubleMaximum);
            // Teardown
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedDecimalPropertyReturnsCorrectResultForDoubleRange()
        {
            // Fixture setup
            var fixture = new Fixture();
            var result = fixture.Create<RangeValidatedType>();
            // Verify outcome
            Assert.True(
                Convert.ToDecimal(RangeValidatedType.DoubleMinimum) <= result.Property3 && result.Property3 <= Convert.ToDecimal(RangeValidatedType.DoubleMaximum),
                string.Format(
                    "Expected result to fall into the interval [{0}, {1}], but was {2}",
                    RangeValidatedType.DoubleMinimum,
                    RangeValidatedType.DoubleMaximum,
                    result.Property3));
            // Teardown
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedDecimalPropertyReturnsCorrectResultForStringRange()
        {
            // Fixture setup
            var fixture = new Fixture();
            var result = fixture.Create<RangeValidatedType>();
            // Verify outcome
            Assert.True(result.Property4 >= Convert.ToDecimal(RangeValidatedType.StringMinimum) && result.Property4 <= Convert.ToDecimal(RangeValidatedType.StringMaximum));
            // Teardown
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedIntegerPropertyReturnsCorrectResultForIntegerRange()
        {
            // Fixture setup
            var fixture = new Fixture();
            var result = fixture.Create<RangeValidatedType>();
            // Verify outcome
            Assert.True(result.Property5 >= RangeValidatedType.Minimum && result.Property5 <= RangeValidatedType.Maximum);
            // Teardown
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedBytePropertyReturnsCorrectResultForIntegerRange()
        {
            // Fixture setup
            var fixture = new Fixture();
            var result = fixture.Create<RangeValidatedType>();
            // Verify outcome
            Assert.True(result.Property6 >= RangeValidatedType.Minimum && result.Property6 <= RangeValidatedType.Maximum);
            // Teardown
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedShortPropertyReturnsCorrectResultForIntegerRange()
        {
            // Fixture setup
            var fixture = new Fixture();
            var result = fixture.Create<RangeValidatedType>();
            // Verify outcome
            Assert.True(result.Property7 >= RangeValidatedType.Minimum && result.Property7 <= RangeValidatedType.Maximum);
            // Teardown
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedUnsignedShortPropertyReturnsCorrectResultForIntegerRange()
        {
            // Fixture setup
            var fixture = new Fixture();
            var result = fixture.Create<RangeValidatedType>();
            // Verify outcome
            Assert.InRange(
                result.UnsignedShortProperty,
                RangeValidatedType.Minimum,
                RangeValidatedType.Maximum);
            // Teardown
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedUnsignedIntPropertyReturnsCorrectResultForIntegerRange()
        {
            // Fixture setup
            var fixture = new Fixture();
            var result = fixture.Create<RangeValidatedType>();
            // Verify outcome
            Assert.InRange(
                (int)result.UnsignedIntProperty,
                RangeValidatedType.Minimum,
                RangeValidatedType.Maximum);
            // Teardown
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedUnsignedLongPropertyReturnsCorrectResultForIntegerRange()
        {
            // Fixture setup
            var fixture = new Fixture();
            var result = fixture.Create<RangeValidatedType>();
            // Verify outcome
            Assert.InRange(
                (int)result.UnsignedLongProperty,
                RangeValidatedType.Minimum,
                RangeValidatedType.Maximum);
            // Teardown
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedSignedBytePropertyReturnsCorrectResultForIntegerRange()
        {
            // Fixture setup
            var fixture = new Fixture();
            var result = fixture.Create<RangeValidatedType>();
            // Verify outcome
            Assert.InRange(
                (int)result.SignedByteProperty,
                RangeValidatedType.Minimum,
                RangeValidatedType.Maximum);
            // Teardown
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedDoublePropertyReturnsCorrectResultForDoubleWithMinimumDoubleMinValue()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = fixture.Create<RangeValidatedType>();
            // Verify outcome
            Assert.True(result.PropertyWithMinimumDoubleMinValue <= RangeValidatedType.Maximum);
            // Teardown
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedDoublePropertyReturnsCorrectResultForDoubleWithMaximumDoubleMaxValue()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = fixture.Create<RangeValidatedType>();
            // Verify outcome
            Assert.True(result.PropertyWithMaximumDoubleMaxValue >= RangeValidatedType.Minimum);
            // Teardown
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedFloatPropertyReturnsCorrectResultForPropertyWithMinimumFloatMinValue()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = fixture.Create<RangeValidatedType>();
            // Verify outcome
            Assert.True(result.PropertyWithMinimumFloatMinValue <= RangeValidatedType.Maximum);
            // Teardown
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedFloatPropertyReturnsCorrectResultForPropertyWithMaximumFloatMaxValue()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = fixture.Create<RangeValidatedType>();
            // Verify outcome
            Assert.True(result.PropertyWithMaximumFloatMaxValue >= RangeValidatedType.Minimum);
            // Teardown
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedDecimalPropertyReturnsCorrectResultForIntegerRangeOnMultipleCall()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = (from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().Property)
                          where (n < RangeValidatedType.Minimum && n > RangeValidatedType.Maximum)
                          select n);
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedDecimalFieldReturnsCorrectResultForIntegerRangeOnMultipleCall()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = (from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().Field)
                          where (n < RangeValidatedType.Minimum && n > RangeValidatedType.Maximum)
                          select n);
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedDoublePropertyReturnsCorrectResultForDoubleRangeOnMultipleCall()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = (from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().Property2)
                          where (n < RangeValidatedType.DoubleMinimum && n > RangeValidatedType.DoubleMaximum)
                          select n);
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedDecimalPropertyReturnsCorrectResultForDoubleRangeOnMultipleCall()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = (from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().Property3)
                          where (n < Convert.ToDecimal(RangeValidatedType.DoubleMinimum) && n > Convert.ToDecimal(RangeValidatedType.DoubleMaximum))
                          select n);
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedDecimalPropertyReturnsCorrectResultForStringRangeOnMultipleCall()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = (from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().Property4)
                          where (n < Convert.ToDecimal(RangeValidatedType.StringMinimum) && n > Convert.ToDecimal(RangeValidatedType.StringMaximum))
                          select n);
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedIntegerPropertyReturnsCorrectResultForIntegerRangeOnMultipleCall()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = (from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().Property5)
                          where (n < RangeValidatedType.Minimum && n > RangeValidatedType.Maximum)
                          select n);
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedBytePropertyReturnsCorrectResultForIntegerRangeOnMultipleCall()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = (from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().Property6)
                          where (n < RangeValidatedType.Minimum && n > RangeValidatedType.Maximum)
                          select n);
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedShortPropertyReturnsCorrectResultForIntegerRangeOnMultipleCall()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = (from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().Property7)
                          where (n < RangeValidatedType.Minimum && n > RangeValidatedType.Maximum)
                          select n);
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedUnsignedShortPropertyReturnsCorrectResultForIntegerRangeOnMultipleCall()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = (from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().UnsignedShortProperty)
                          where (n < RangeValidatedType.Minimum && n > RangeValidatedType.Maximum)
                          select n);
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedUnsignedIntPropertyReturnsCorrectResultForIntegerRangeOnMultipleCall()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = (from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().UnsignedIntProperty)
                          where (n < RangeValidatedType.Minimum && n > RangeValidatedType.Maximum)
                          select n);
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedUnsignedLongPropertyReturnsCorrectResultForIntegerRangeOnMultipleCall()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = (from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().UnsignedLongProperty)
                          where (n < RangeValidatedType.Minimum && n > RangeValidatedType.Maximum)
                          select n);
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedSignedBytePropertyReturnsCorrectResultForIntegerRangeOnMultipleCall()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = (from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().SignedByteProperty)
                          where (n < RangeValidatedType.Minimum && n > RangeValidatedType.Maximum)
                          select n);
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedDoublePropertyReturnsCorrectResultForDoubleWithMinimumDoubleMinValueOnMultipleCall()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = (from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().PropertyWithMinimumDoubleMinValue)
                          where (n > Convert.ToDouble(RangeValidatedType.Maximum))
                          select n);
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedDoublePropertyReturnsCorrectResultForDoubleWithMaximumDoubleMaxValueOnMultipleCall()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = (from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().PropertyWithMaximumDoubleMaxValue)
                          where (n < Convert.ToDouble(RangeValidatedType.Minimum))
                          select n);
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedFloatPropertyReturnsCorrectResultForPropertyWithMinimumFloatMinValueOnMultipleCall()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = (from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().PropertyWithMinimumFloatMinValue)
                          where (n > Convert.ToSingle(RangeValidatedType.Maximum))
                          select n);
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedFloatPropertyReturnsCorrectResultForPropertyWithMaximumFloatMaxValueOnMultipleCall()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = (from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().PropertyWithMaximumFloatMaxValue)
                          where (n < Convert.ToSingle(RangeValidatedType.Minimum))
                          select n);
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithRegularExpressionValidatedTypeReturnsCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture();
            var result = fixture.Create<RegularExpressionValidatedType>();
            // Verify outcome
            Assert.Matches(result.Property, RegularExpressionValidatedType.Pattern);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithStringLengthValidatedTypeReturnsCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture();
            var result = fixture.Create<StringLengthValidatedType>();
            // Verify outcome
            Assert.True(result.Property.Length <= StringLengthValidatedType.MaximumLength);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithStringLengthValidatedReturnsCorrectResultMultipleCall()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = (from n in Enumerable.Range(1, 33).Select(i => fixture.Create<StringLengthValidatedType>().Property.Length)
                          where (n > StringLengthValidatedType.MaximumLength)
                          select n);
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithUriReturnsValidResult()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            Uri result;
            bool succeed = Uri.TryCreate(
                fixture.Create<Uri>().OriginalString,
                UriKind.Absolute,
                out result);
            // Verify outcome
            Assert.True(succeed && result != null);
            // Teardown
        }

        [Fact]
        public void DefaultRepeatCountIsThree()
        {
            // Fixture setup
            int expectedRepeatCount = 3;
            Fixture sut = new Fixture();
            // Exercise system
            int result = sut.RepeatCount;
            // Verify outcome
            Assert.Equal<int>(expectedRepeatCount, result);
            // Teardown
        }
        [Fact]
        public void RepeatWillPerformActionTheDefaultNumberOfTimes()
        {
            // Fixture setup
            IFixture sut = new Fixture();
            int expectedCount = sut.RepeatCount;
            // Exercise system
            int result = 0;
            sut.Repeat(() => result++).ToList();
            // Verify outcome
            Assert.Equal<int>(expectedCount, result);
            // Teardown
        }

        [Fact]
        public void RepeatWillReturnTheDefaultNumberOfItems()
        {
            // Fixture setup
            IFixture sut = new Fixture();
            int expectedCount = sut.RepeatCount;
            // Exercise system
            IEnumerable<object> result = sut.Repeat(() => new object());
            // Verify outcome
            Assert.Equal<int>(expectedCount, result.Count());
            // Teardown
        }

        [Fact]
        public void RepeatWillPerformActionTheSpecifiedNumberOfTimes()
        {
            // Fixture setup
            int expectedCount = 2;
            IFixture sut = new Fixture();
            sut.RepeatCount = expectedCount;
            // Exercise system
            int result = 0;
            sut.Repeat(() => result++).ToList();
            // Verify outcome
            Assert.Equal<int>(expectedCount, result);
            // Teardown
        }

        [Fact]
        public void RepeatWillReturnTheSpecifiedNumberOfItems()
        {
            // Fixture setup
            int expectedCount = 13;
            IFixture sut = new Fixture();
            sut.RepeatCount = expectedCount;
            // Exercise system
            IEnumerable<object> result = sut.Repeat(() => new object());
            // Verify outcome
            Assert.Equal<int>(expectedCount, result.Count());
            // Teardown
        }

        [Fact]
        public void ReplacingStringMappingWillUseNewStringCreationAlgorithm()
        {
            // Fixture setup
            string expectedText = "Anonymous string";
            Fixture sut = new Fixture();
            // Exercise system
            sut.Customize<string>(c => c.FromSeed(s => expectedText));
            // Verify outcome
            string result = sut.Create<string>();
            Assert.Equal(expectedText, result);
            // Teardown
        }

        [Fact]
        public void AddManyWillAddItemsToListUsingCreator()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            IEnumerable<int> expectedList = Enumerable.Range(1, sut.RepeatCount);
            List<int> list = new List<int>();
            // Exercise system
            int i = 0;
            sut.AddManyTo(list, () => ++i);
            // Verify outcome
            Assert.True(expectedList.SequenceEqual(list));
            // Teardown
        }

        [Fact]
        public void AddManyWillAddItemsToListUsingAnonymousCreator()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            int expectedItemCount = sut.RepeatCount;
            List<string> list = new List<string>();
            // Exercise system
            sut.AddManyTo(list);
            // Verify outcome
            int result = (from s in list
                          where !string.IsNullOrEmpty(s)
                          select s).Count();
            Assert.Equal<int>(expectedItemCount, result);
            // Teardown
        }

        [Fact]
        public void AddManyWillAddItemsToCollection()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            int expectedCount = sut.RepeatCount;
            ICollection<int> collection = new LinkedList<int>();
            // Exercise system
            sut.AddManyTo(collection);
            // Verify outcome
            Assert.Equal<int>(expectedCount, collection.Count);
            // Teardown
        }

        [Fact]
        public void AddManyWithRepeatCountWillAddItemsToCollection()
        {
            // Fixture setup
            var sut = new Fixture();
            int expectedCount = 24;
            ICollection<int> collection = new LinkedList<int>();
            // Exercise system
            sut.AddManyTo(collection, expectedCount);
            // Verify outcome
            Assert.Equal<int>(expectedCount, collection.Count);
            // Teardown
        }

        [Fact]
        public void AddManyWithCreatorWillAddItemsToCollection()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            int expectedCount = sut.RepeatCount;
            ICollection<object> collection = new LinkedList<object>();
            // Exercise system
            sut.AddManyTo(collection, () => new object());
            // Verify outcome
            Assert.Equal<int>(expectedCount, collection.Count);
            // Teardown
        }

        [Fact]
        public void CreateManyWillCreateManyAnonymousItems()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            int expectedItemCount = sut.RepeatCount;
            // Exercise system
            IEnumerable<string> result = sut.CreateMany<string>();
            // Verify outcome
            int nonDefaultCount = (from s in result
                                   where !string.IsNullOrEmpty(s)
                                   select s).Count();
            Assert.Equal<int>(expectedItemCount, nonDefaultCount);
            // Teardown
        }

        [Fact]
        public void CreateManyWillCreateCorrectNumberOfAnonymousItems()
        {
            // Fixture setup
            var sut = new Fixture();
            int expectedItemCount = 248;
            // Exercise system
            IEnumerable<string> result = sut.CreateMany<string>(expectedItemCount);
            // Verify outcome
            int nonDefaultCount = (from s in result
                                   where !string.IsNullOrEmpty(s)
                                   select s).Count();
            Assert.Equal<int>(expectedItemCount, nonDefaultCount);
            // Teardown
        }

        [Fact]
        public void CreateManyWithSeedWillCreateManyCorrectItems()
        {
            // Fixture setup
            string anonymousPrefix = "AnonymousPrefix";
            var sut = new Fixture();
            int expectedItemCount = sut.RepeatCount;
            // Exercise system
            IEnumerable<string> result = sut.CreateMany(anonymousPrefix);
            // Verify outcome
            int actualCount = (from s in result
                               where s.StartsWith(anonymousPrefix)
                               select s).Count();
            Assert.Equal<int>(expectedItemCount, actualCount);
            // Teardown
        }

        [Fact]
        public void CreateManyWithSeedWillCreateCorrectNumberOfItems()
        {
            // Fixture setup
            string anonymousPrefix = "Prefix";
            int expectedItemCount = 73;
            var sut = new Fixture();
            // Exercise system
            IEnumerable<string> result = sut.CreateMany(anonymousPrefix, expectedItemCount);
            // Verify outcome
            int actualCount = (from s in result
                               where s.StartsWith(anonymousPrefix)
                               select s).Count();
            Assert.Equal<int>(expectedItemCount, actualCount);
            // Teardown
        }

        [Fact]
        public void CustomizeNullTransformationThrows()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Customize<object>(null));
            // Teardown
        }

        [Fact]
        public void RegisterTypeWithPropertyOverrideWillSetPropertyValueCorrectly()
        {
            // Fixture setup
            string expectedValue = "Anonymous text";
            Fixture sut = new Fixture();
            // Exercise system
            sut.Customize<PropertyHolder<string>>(f => f.With(ph => ph.Property, expectedValue));
            PropertyHolder<string> result = sut.Create<PropertyHolder<string>>();
            // Verify outcome
            Assert.Equal(expectedValue, result.Property);
            // Teardown
        }

        [Fact]
        public void CustomizeWithEchoInt32GeneratorWillReturnSeed()
        {
            // Fixture setup
            int expectedValue = 4;
            Fixture sut = new Fixture();
            sut.Customize<int>(c => c.FromSeed(s => s));
            // Exercise system
            int result = sut.Create(expectedValue);
            // Verify outcome
            Assert.Equal<int>(expectedValue, result);
            // Teardown
        }

        [Fact]
        public void CreateNestedTypeWillPopulateNestedProperty()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.Create<PropertyHolder<PropertyHolder<string>>>();
            // Verify outcome
            Assert.False(string.IsNullOrEmpty(result.Property.Property), "Nested property string should not be null or empty.");
            // Teardown
        }

        [Fact]
        public void DoOnCommandWithNullSingleParameterActionThrows()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Do<object>(null));
            // Teardown
        }

        [Fact]
        public void DoOnCommandWithSingleParameterWillInvokeMethod()
        {
            // Fixture setup
            bool methodInvoked = false;
            var sut = new Fixture();

            var mock = new CommandMock<string>();
            mock.OnCommand = x => methodInvoked = true;
            // Exercise system
            sut.Do((string s) => mock.Command(s));
            // Verify outcome
            Assert.True(methodInvoked, "Command method invoked");
            // Teardown
        }

        [Fact]
        public void DoOnCommandWithSingleParameterWillInvokeMethodWithCorrectParameter()
        {
            // Fixture setup
            int expectedNumber = 94;

            var sut = new Fixture();
            sut.Register<int>(() => expectedNumber);

            var mock = new CommandMock<int>();
            mock.OnCommand = x => Assert.Equal<int>(expectedNumber, x);
            // Exercise system
            sut.Do((int i) => mock.Command(i));
            // Verify outcome (done by mock)
            // Teardown
        }

        [Fact]
        public void DoOnCommandWithNullTwoParameterActionThrows()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Do<object, object>(null));
            // Teardown
        }

        [Fact]
        public void DoOnCommandWithTwoParametersWillInvokeMethod()
        {
            // Fixture setup
            bool methodInvoked = false;
            var sut = new Fixture();

            var mock = new CommandMock<string, long>();
            mock.OnCommand = (x1, x2) => methodInvoked = true;
            // Exercise system
            sut.Do((string x1, long x2) => mock.Command(x1, x2));
            // Verify outcome
            Assert.True(methodInvoked, "Command method invoked");
            // Teardown
        }

        [Fact]
        public void DoOnCommandWithTwoParametersWillInvokeMethodWithCorrectFirstParameter()
        {
            // Fixture setup
            double expectedNumber = 25364.37;

            var sut = new Fixture();
            sut.Register<double>(() => expectedNumber);

            var mock = new CommandMock<double, object>();
            mock.OnCommand = (x1, x2) => Assert.Equal<double>(expectedNumber, x1);
            // Exercise system
            sut.Do((double x1, object x2) => mock.Command(x1, x2));
            // Verify outcome (done by mock)
            // Teardown
        }

        [Fact]
        public void DoOnCommandWithTwoParametersWillInvokeMethodWithCorrectSecondParameter()
        {
            // Fixture setup
            short expectedNumber = 3734;

            var sut = new Fixture();
            sut.Register<short>(() => expectedNumber);

            var mock = new CommandMock<DateTime, short>();
            mock.OnCommand = (x1, x2) => Assert.Equal<short>(expectedNumber, x2);
            // Exercise system
            sut.Do((DateTime x1, short x2) => mock.Command(x1, x2));
            // Verify outcome (done by mock)
            // Teardown
        }

        [Fact]
        public void DoOnCommandWithNullThreeParameterActionThrows()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Do<object, object, object>(null));
            // Teardown
        }

        [Fact]
        public void DoOnCommandWithThreeParametersWillInvokeMethod()
        {
            // Fixture setup
            bool methodInvoked = false;
            var sut = new Fixture();

            var mock = new CommandMock<object, object, object>();
            mock.OnCommand = (x1, x2, x3) => methodInvoked = true;
            // Exercise system
            sut.Do((object x1, object x2, object x3) => mock.Command(x1, x2, x3));
            // Verify outcome
            Assert.True(methodInvoked, "Command method invoked");
            // Teardown
        }

        [Fact]
        public void DoOnCommandWithThreeParametersWillInvokeMethodWithCorrectFirstParameter()
        {
            // Fixture setup
            DateTime expectedDateTime = new DateTime(1004328837);

            var sut = new Fixture();
            sut.Register<DateTime>(() => expectedDateTime);

            var mock = new CommandMock<DateTime, long, short>();
            mock.OnCommand = (x1, x2, x3) => Assert.Equal<DateTime>(expectedDateTime, x1);
            // Exercise system
            sut.Do((DateTime x1, long x2, short x3) => mock.Command(x1, x2, x3));
            // Verify outcome (done by mock)
            // Teardown
        }

        [Fact]
        public void DoOnCommandWithThreeParametersWillInvokeMethodWithCorrectSecondParameter()
        {
            // Fixture setup
            TimeSpan expectedTimeSpan = TimeSpan.FromHours(53);

            var sut = new Fixture();
            sut.Register<TimeSpan>(() => expectedTimeSpan);

            var mock = new CommandMock<uint, TimeSpan, TimeSpan>();
            mock.OnCommand = (x1, x2, x3) => Assert.Equal<TimeSpan>(expectedTimeSpan, x2);
            // Exercise system
            sut.Do((uint x1, TimeSpan x2, TimeSpan x3) => mock.Command(x1, x2, x3));
            // Verify outcome (done by mock)
            // Teardown
        }

        [Fact]
        public void DoOnCommandWithThreeParametersWillInvokeMethodWithCorrectThirdParameter()
        {
            // Fixture setup
            var expectedText = "Anonymous text";

            var sut = new Fixture();
            sut.Register<string>(() => expectedText);

            var mock = new CommandMock<double, uint, string>();
            mock.OnCommand = (x1, x2, x3) => Assert.Equal(expectedText, x3);
            // Exercise system
            sut.Do((double x1, uint x2, string x3) => mock.Command(x1, x2, x3));
            // Verify outcome (done by mock)
            // Teardown
        }

        [Fact]
        public void DoOnCommandWithNullFourParameterActionThrows()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Do<object, object, object, object>(null));
            // Teardown
        }

        [Fact]
        public void DoOnCommandWithFourParametersWillInvokeMethod()
        {
            // Fixture setup
            bool methodInvoked = false;
            var sut = new Fixture();

            var mock = new CommandMock<uint, ushort, int, bool>();
            mock.OnCommand = (x1, x2, x3, x4) => methodInvoked = true;
            // Exercise system
            sut.Do((uint x1, ushort x2, int x3, bool x4) => mock.Command(x1, x2, x3, x4));
            // Verify outcome
            Assert.True(methodInvoked, "Command method invoked");
            // Teardown
        }

        [Fact]
        public void DoOnCommandWithFourParametersWillInvokeMethodWithCorrectFirstParameter()
        {
            // Fixture setup
            uint expectedNumber = 294;

            var sut = new Fixture();
            sut.Register<uint>(() => expectedNumber);

            var mock = new CommandMock<uint, bool, string, Guid>();
            mock.OnCommand = (x1, x2, x3, x4) => Assert.Equal<uint>(expectedNumber, x1);
            // Exercise system
            sut.Do((uint x1, bool x2, string x3, Guid x4) => mock.Command(x1, x2, x3, x4));
            // Verify outcome (done by mock)
            // Teardown
        }

        [Fact]
        public void DoOnCommandWithFourParametersWillInvokeMethodWithCorrectSecondParameter()
        {
            // Fixture setup
            decimal expectedNumber = 92183.28m;

            var sut = new Fixture();
            sut.Register<decimal>(() => expectedNumber);

            var mock = new CommandMock<ushort, decimal, Guid, bool>();
            mock.OnCommand = (x1, x2, x3, x4) => Assert.Equal<decimal>(expectedNumber, x2);
            // Exercise system
            sut.Do((ushort x1, decimal x2, Guid x3, bool x4) => mock.Command(x1, x2, x3, x4));
            // Verify outcome (done by mock)
            // Teardown
        }

        [Fact]
        public void DoOnCommandWithFourParametersWillInvokeMethodWithCorrectThirdParameter()
        {
            // Fixture setup
            Guid expectedGuid = Guid.NewGuid();

            var sut = new Fixture();
            sut.Register<Guid>(() => expectedGuid);

            var mock = new CommandMock<bool, string, Guid, string>();
            mock.OnCommand = (x1, x2, x3, x4) => Assert.Equal<Guid>(expectedGuid, x3);
            // Exercise system
            sut.Do((bool x1, string x2, Guid x3, string x4) => mock.Command(x1, x2, x3, x4));
            // Verify outcome (done by mock)
            // Teardown
        }

        [Fact]
        public void DoOnCommandWithFourParametersWillInvokeMethodWithCorrectFourthParameter()
        {
            // Fixture setup
            var expectedObj = new ConcreteType();

            var sut = new Fixture();
            sut.Register<ConcreteType>(() => expectedObj);

            var mock = new CommandMock<int?, DateTime, TimeSpan, ConcreteType>();
            mock.OnCommand = (x1, x2, x3, x4) => Assert.Equal<ConcreteType>(expectedObj, x4);
            // Exercise system
            sut.Do((int? x1, DateTime x2, TimeSpan x3, ConcreteType x4) => mock.Command(x1, x2, x3, x4));
            // Verify outcome (done by mock)
            // Teardown
        }

        [Fact]
        public void GetOnCommandWithNullSingleParameterFunctionThrows()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Get<object, object>(null));
            // Teardown
        }

        [Fact]
        public void GetOnQueryWithSingleParameterWillInvokeMethod()
        {
            // Fixture setup
            bool methodInvoked = false;
            var sut = new Fixture();

            var mock = new QueryMock<ulong, bool>();
            mock.OnQuery = x => { methodInvoked = true; return true; };
            // Exercise system
            sut.Get((ulong s) => mock.Query(s));
            // Verify outcome
            Assert.True(methodInvoked, "Query method invoked");
            // Teardown
        }

        [Fact]
        public void GetOnQueryWithSingleParameterWillInvokeMethodWithCorrectParameter()
        {
            // Fixture setup
            double? expectedNumber = 23892;

            var sut = new Fixture();
            sut.Register<double?>(() => expectedNumber);

            var mock = new QueryMock<double?, string>();
            mock.OnQuery = x => { Assert.Equal<double?>(expectedNumber, x); return "Anonymous text"; };
            // Exercise system
            sut.Get((double? x) => mock.Query(x));
            // Verify outcome (done by mock)
            // Teardown
        }

        [Fact]
        public void GetOnQueryWithSingleParameterWillReturnCorrectResult()
        {
            // Fixture setup
            var expectedVersion = new Version(2, 45);
            var sut = new Fixture();

            var mock = new QueryMock<int?, Version>();
            mock.OnQuery = x => expectedVersion;
            // Exercise system
            var result = sut.Get((int? x) => mock.Query(x));
            // Verify outcome
            Assert.Equal<Version>(expectedVersion, result);
            // Teardown
        }

        [Fact]
        public void GetOnCommandWithNullDoubleParameterFunctionThrows()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Get<object, object, object>(null));
            // Teardown
        }

        [Fact]
        public void GetOnQueryWithTwoParametersWillInvokeMethod()
        {
            // Fixture setup
            bool methodInvoked = false;
            var sut = new Fixture();

            var mock = new QueryMock<string, int, long>();
            mock.OnQuery = (x1, x2) => { methodInvoked = true; return 148; };
            // Exercise system
            sut.Get((string x1, int x2) => mock.Query(x1, x2));
            // Verify outcome
            Assert.True(methodInvoked, "Query method invoked");
            // Teardown
        }

        [Fact]
        public void GetOnQueryWithTwoParametersWillInvokeMethodWithCorrectFirstParameter()
        {
            // Fixture setup
            byte expectedByte = 213;

            var sut = new Fixture();
            sut.Register<byte>(() => expectedByte);

            var mock = new QueryMock<byte, int, double>();
            mock.OnQuery = (x1, x2) => { Assert.Equal<byte>(expectedByte, x1); return 9823829; };
            // Exercise system
            sut.Get((byte x1, int x2) => mock.Query(x1, x2));
            // Verify outcome (done by mock)
            // Teardown
        }

        [Fact]
        public void GetOnQueryWithTwoParametersWillInvokeMethodWithCorrectSecondParameter()
        {
            // Fixture setup
            sbyte expectedByte = -29;

            var sut = new Fixture();
            sut.Register<sbyte>(() => expectedByte);

            var mock = new QueryMock<DateTime, sbyte, bool>();
            mock.OnQuery = (x1, x2) => { Assert.Equal<sbyte>(expectedByte, x2); return false; };
            // Exercise system
            sut.Get((DateTime x1, sbyte x2) => mock.Query(x1, x2));
            // Verify outcome (done by mock)
            // Teardown
        }

        [Fact]
        public void GetOnQueryWithTwoParametersWillReturnCorrectResult()
        {
            // Fixture setup
            byte? expectedByte = 198;
            var sut = new Fixture();

            var mock = new QueryMock<DateTime, TimeSpan, byte?>();
            mock.OnQuery = (x1, x2) => expectedByte;
            // Exercise system
            var result = sut.Get((DateTime x1, TimeSpan x2) => mock.Query(x1, x2));
            // Verify outcome
            Assert.Equal<byte?>(expectedByte, result);
            // Teardown
        }

        [Fact]
        public void GetOnCommandWithNullTripleParameterFunctionThrows()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Get<object, object, object, object>(null));
            // Teardown
        }

        [Fact]
        public void GetOnQueryWithThreeParametersWillInvokeMethod()
        {
            // Fixture setup
            bool methodInvoked = false;
            var sut = new Fixture();

            var mock = new QueryMock<object, object, object, object>();
            mock.OnQuery = (x1, x2, x3) => { methodInvoked = true; return new object(); };
            // Exercise system
            sut.Get((object x1, object x2, object x3) => mock.Query(x1, x2, x3));
            // Verify outcome
            Assert.True(methodInvoked, "Query method invoked");
            // Teardown
        }

        [Fact]
        public void GetOnQueryWithThreeParametersWillInvokeMethodWithCorrectFirstParameter()
        {
            // Fixture setup
            sbyte? expectedByte = -56;

            var sut = new Fixture();
            sut.Register<sbyte?>(() => expectedByte);

            var mock = new QueryMock<sbyte?, bool, string, float>();
            mock.OnQuery = (x1, x2, x3) => { Assert.Equal<sbyte?>(expectedByte, x1); return 3646.77f; };
            // Exercise system
            sut.Get((sbyte? x1, bool x2, string x3) => mock.Query(x1, x2, x3));
            // Verify outcome (done by mock)
            // Teardown
        }

        [Fact]
        public void GetOnQueryWithThreeParametersWillInvokeMethodWithCorrectSecondParameter()
        {
            // Fixture setup
            float expectedNumber = -927.2f;

            var sut = new Fixture();
            sut.Register<float>(() => expectedNumber);

            var mock = new QueryMock<bool, float, TimeSpan, object>();
            mock.OnQuery = (x1, x2, x3) => { Assert.Equal<float>(expectedNumber, x2); return new object(); };
            // Exercise system
            sut.Get((bool x1, float x2, TimeSpan x3) => mock.Query(x1, x2, x3));
            // Verify outcome (done by mock)
            // Teardown
        }

        [Fact]
        public void GetOnQueryWithThreeParametersWillInvokeMethodWithCorrectThirdParameter()
        {
            // Fixture setup
            var expectedText = "Anonymous text";

            var sut = new Fixture();
            sut.Register<string>(() => expectedText);

            var mock = new QueryMock<long, short, string, decimal?>();
            mock.OnQuery = (x1, x2, x3) => { Assert.Equal(expectedText, x3); return 111.11m; };
            // Exercise system
            sut.Get((long x1, short x2, string x3) => mock.Query(x1, x2, x3));
            // Verify outcome (done by mock)
            // Teardown
        }

        [Fact]
        public void GetOnQueryWithThreeParametersWillReturnCorrectResult()
        {
            // Fixture setup
            var expectedDateTime = new DateTimeOffset(2839327192831219387, TimeSpan.FromHours(-2));
            var sut = new Fixture();

            var mock = new QueryMock<short, long, Guid, DateTimeOffset>();
            mock.OnQuery = (x1, x2, x3) => expectedDateTime;
            // Exercise system
            var result = sut.Get((short x1, long x2, Guid x3) => mock.Query(x1, x2, x3));
            // Verify outcome
            Assert.Equal<DateTimeOffset>(expectedDateTime, result);
            // Teardown
        }

        [Fact]
        public void GetOnCommandWithNullQuadrupleParameterFunctionThrows()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Get<object, object, object, object, object>(null));
            // Teardown
        }

        [Fact]
        public void GetOnQueryWithFourParametersWillInvokeMethod()
        {
            // Fixture setup
            bool methodInvoked = false;
            var sut = new Fixture();

            var mock = new QueryMock<object, object, object, object, object>();
            mock.OnQuery = (x1, x2, x3, x4) => { methodInvoked = true; return new object(); };
            // Exercise system
            sut.Get((object x1, object x2, object x3, object x4) => mock.Query(x1, x2, x3, x4));
            // Verify outcome
            Assert.True(methodInvoked, "Query method invoked");
            // Teardown
        }

        [Fact]
        public void GetOnQueryWithFourParametersWillInvokeMethodWithCorrectFirstParameter()
        {
            // Fixture setup
            var expectedTimeSpan = TimeSpan.FromSeconds(23);

            var sut = new Fixture();
            sut.Register<TimeSpan>(() => expectedTimeSpan);

            var mock = new QueryMock<TimeSpan, Version, Random, Guid, EventArgs>();
            mock.OnQuery = (x1, x2, x3, x4) => { Assert.Equal<TimeSpan>(expectedTimeSpan, x1); return EventArgs.Empty; };
            // Exercise system
            sut.Get((TimeSpan x1, Version x2, Random x3, Guid x4) => mock.Query(x1, x2, x3, x4));
            // Verify outcome (done by mock)
            // Teardown
        }

        [Fact]
        public void GetOnQueryWithFourParametersWillInvokeMethodWithCorrectSecondParameter()
        {
            // Fixture setup
            var expectedDateTimeKind = DateTimeKind.Utc;

            var sut = new Fixture();
            sut.Register<DateTimeKind>(() => expectedDateTimeKind);

            var mock = new QueryMock<Random, DateTimeKind, DateTime, string, float>();
            mock.OnQuery = (x1, x2, x3, x4) => { Assert.Equal<DateTimeKind>(expectedDateTimeKind, x2); return 77f; };
            // Exercise system
            sut.Get((Random x1, DateTimeKind x2, DateTime x3, string x4) => mock.Query(x1, x2, x3, x4));
            // Verify outcome (done by mock)
            // Teardown
        }

        [Fact]
        public void GetOnQueryWithFourParametersWillInvokeMethodWithCorrectThirdParameter()
        {
            // Fixture setup
            var expectedDayOfWeek = DayOfWeek.Friday;

            var sut = new Fixture();
            sut.Register<DayOfWeek>(() => expectedDayOfWeek);

            var mock = new QueryMock<int, float, DayOfWeek, string, ConsoleColor>();
            mock.OnQuery = (x1, x2, x3, x4) => { Assert.Equal<DayOfWeek>(expectedDayOfWeek, x3); return ConsoleColor.Black; };
            // Exercise system
            sut.Get((int x1, float x2, DayOfWeek x3, string x4) => mock.Query(x1, x2, x3, x4));
            // Verify outcome (done by mock)
            // Teardown
        }

        [Fact]
        public void GetOnQueryWithFourParametersWillInvokeMethodWithCorrectFourthParameter()
        {
            // Fixture setup
            var expectedNumber = 42;

            var sut = new Fixture();
            sut.Register<int>(() => expectedNumber);

            var mock = new QueryMock<Version, ushort, string, int, ConsoleColor>();
            mock.OnQuery = (x1, x2, x3, x4) => { Assert.Equal<int>(expectedNumber, x4); return ConsoleColor.Cyan; };
            // Exercise system
            sut.Get((Version x1, ushort x2, string x3, int x4) => mock.Query(x1, x2, x3, x4));
            // Verify outcome (done by mock)
            // Teardown
        }

        [Fact]
        public void GetOnQueryWithFourParametersWillReturnCorrectResult()
        {
            // Fixture setup
            var expectedColor = ConsoleColor.DarkGray;
            var sut = new Fixture();

            var mock = new QueryMock<int, int, int, int, ConsoleColor>();
            mock.OnQuery = (x1, x2, x3, x4) => expectedColor;
            // Exercise system
            var result = sut.Get((int x1, int x2, int x3, int x4) => mock.Query(x1, x2, x3, x4));
            // Verify outcome
            Assert.Equal<ConsoleColor>(expectedColor, result);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void FromFactoryWithOneParameterWillRespectPreviousCustomizationsObsolete()
        {
            // Fixture setup
            string expectedText = Guid.NewGuid().ToString();
            var sut = new Fixture();
            sut.Customize<PropertyHolder<string>>(ob => ob.With(ph => ph.Property, expectedText));
            // Exercise system
            var result = sut.Build<SingleParameterType<PropertyHolder<string>>>()
                .FromFactory((PropertyHolder<string> ph) => new SingleParameterType<PropertyHolder<string>>(ph))
                .CreateAnonymous();
            // Verify outcome
            Assert.Equal(expectedText, result.Parameter.Property);
            // Teardown
        }

        [Fact]
        public void FromFactoryWithOneParameterWillRespectPreviousCustomizations()
        {
            // Fixture setup
            string expectedText = Guid.NewGuid().ToString();
            var sut = new Fixture();
            sut.Customize<PropertyHolder<string>>(ob => ob.With(ph => ph.Property, expectedText));
            // Exercise system
            var result = sut.Build<SingleParameterType<PropertyHolder<string>>>()
                .FromFactory((PropertyHolder<string> ph) => new SingleParameterType<PropertyHolder<string>>(ph))
                .Create();
            // Verify outcome
            Assert.Equal(expectedText, result.Parameter.Property);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void FromFactoryWithTwoParametersWillRespectPreviousCustomizationsObsolete()
        {
            // Fixture setup
            string expectedText = Guid.NewGuid().ToString();
            var sut = new Fixture();
            sut.Customize<PropertyHolder<string>>(ob => ob.With(ph => ph.Property, expectedText));
            // Exercise system
            var result = sut.Build<SingleParameterType<PropertyHolder<string>>>()
                .FromFactory((PropertyHolder<string> ph, object dummy) => new SingleParameterType<PropertyHolder<string>>(ph))
                .CreateAnonymous();
            // Verify outcome
            Assert.Equal(expectedText, result.Parameter.Property);
            // Teardown
        }

        [Fact]
        public void FromFactoryWithTwoParametersWillRespectPreviousCustomizations()
        {
            // Fixture setup
            string expectedText = Guid.NewGuid().ToString();
            var sut = new Fixture();
            sut.Customize<PropertyHolder<string>>(ob => ob.With(ph => ph.Property, expectedText));
            // Exercise system
            var result = sut.Build<SingleParameterType<PropertyHolder<string>>>()
                .FromFactory((PropertyHolder<string> ph, object dummy) => new SingleParameterType<PropertyHolder<string>>(ph))
                .Create();
            // Verify outcome
            Assert.Equal(expectedText, result.Parameter.Property);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void FromFactoryWithThreeParametersWillRespectPreviousCustomizationsObsolete()
        {
            // Fixture setup
            string expectedText = Guid.NewGuid().ToString();
            var sut = new Fixture();
            sut.Customize<PropertyHolder<string>>(ob => ob.With(ph => ph.Property, expectedText));
            // Exercise system
            var result = sut.Build<SingleParameterType<PropertyHolder<string>>>()
                .FromFactory((PropertyHolder<string> ph, object dummy1, object dummy2) => new SingleParameterType<PropertyHolder<string>>(ph))
                .CreateAnonymous();
            // Verify outcome
            Assert.Equal(expectedText, result.Parameter.Property);
            // Teardown
        }

        [Fact]
        public void FromFactoryWithThreeParametersWillRespectPreviousCustomizations()
        {
            // Fixture setup
            string expectedText = Guid.NewGuid().ToString();
            var sut = new Fixture();
            sut.Customize<PropertyHolder<string>>(ob => ob.With(ph => ph.Property, expectedText));
            // Exercise system
            var result = sut.Build<SingleParameterType<PropertyHolder<string>>>()
                .FromFactory((PropertyHolder<string> ph, object dummy1, object dummy2) => new SingleParameterType<PropertyHolder<string>>(ph))
                .Create();
            // Verify outcome
            Assert.Equal(expectedText, result.Parameter.Property);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void FromFactoryWithFourParametersWillRespectPreviousCustomizationsObsolete()
        {
            // Fixture setup
            string expectedText = Guid.NewGuid().ToString();
            var sut = new Fixture();
            sut.Customize<PropertyHolder<string>>(ob => ob.With(ph => ph.Property, expectedText));
            // Exercise system
            var result = sut.Build<SingleParameterType<PropertyHolder<string>>>()
                .FromFactory((PropertyHolder<string> ph, object dummy1, object dummy2, object dummy3) => new SingleParameterType<PropertyHolder<string>>(ph))
                .CreateAnonymous();
            // Verify outcome
            Assert.Equal(expectedText, result.Parameter.Property);
            // Teardown
        }

        [Fact]
        public void FromFactoryWithFourParametersWillRespectPreviousCustomizations()
        {
            // Fixture setup
            string expectedText = Guid.NewGuid().ToString();
            var sut = new Fixture();
            sut.Customize<PropertyHolder<string>>(ob => ob.With(ph => ph.Property, expectedText));
            // Exercise system
            var result = sut.Build<SingleParameterType<PropertyHolder<string>>>()
                .FromFactory((PropertyHolder<string> ph, object dummy1, object dummy2, object dummy3) => new SingleParameterType<PropertyHolder<string>>(ph))
                .Create();
            // Verify outcome
            Assert.Equal(expectedText, result.Parameter.Property);
            // Teardown
        }

        [Fact]
        public void CustomizeCanDefineConstructor()
        {
            // Fixture setup
            var sut = new Fixture();
            string expectedText = Guid.NewGuid().ToString();
            sut.Customize<SingleParameterType<string>>(ob => ob.FromFactory(() => new SingleParameterType<string>(expectedText)));
            // Exercise system
            var result = sut.Create<SingleParameterType<string>>();
            // Verify outcome
            Assert.Equal(expectedText, result.Parameter);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWillNotThrowWhenTypeHasIndexedProperty()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.Create<IndexedPropertyHolder<object>>();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void BuildWillReturnBuilderThatCreatesTheCorrectNumberOfInstances()
        {
            // Fixture setup
            int expectedRepeatCount = 242;
            var sut = new Fixture();
            sut.RepeatCount = expectedRepeatCount;
            // Exercise system
            var result = sut.Build<object>().CreateMany();
            // Verify outcome
            Assert.Equal<int>(expectedRepeatCount, result.Count());
            // Teardown
        }

        [Fact]
        public void InjectWillCauseSutToReturnInstanceWhenRequested()
        {
            // Fixture setup
            var expectedResult = new PropertyHolder<object>();
            var sut = new Fixture();
            sut.Inject(expectedResult);
            // Exercise system
            var result = sut.Create<PropertyHolder<object>>();
            // Verify outcome
            Assert.Equal<PropertyHolder<object>>(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void InjectWillCauseSutToReturnInstanceWithoutAutoPropertiesWhenRequested()
        {
            // Fixture setup
            var item = new PropertyHolder<object>();
            item.Property = null;

            var sut = new Fixture();
            sut.Inject(item);
            // Exercise system
            var result = sut.Create<PropertyHolder<object>>();
            // Verify outcome
            Assert.Null(result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWillInvokeResidueCollector()
        {
            // Fixture setup
            bool resolveWasInvoked = false;

            var residueCollector = new DelegatingSpecimenBuilder();
            residueCollector.OnCreate = (r, c) =>
            {
                resolveWasInvoked = true;
                return new ConcreteType();
            };

            var sut = new Fixture();
            sut.ResidueCollectors.Add(residueCollector);
            // Exercise system
            sut.Create<PropertyHolder<AbstractType>>();
            // Verify outcome
            Assert.True(resolveWasInvoked, "Resolver");
            // Teardown
        }

        [Fact]
        public void CreateAnonymousOnUnregisteredAbstractionWillInvokeResidueCollectorWithCorrectType()
        {
            // Fixture setup
            var residueCollector = new DelegatingSpecimenBuilder();
            residueCollector.OnCreate = (r, c) =>
            {
                Assert.Equal(typeof(AbstractType), r);
                return new ConcreteType();
            };

            var sut = new Fixture();
            sut.ResidueCollectors.Add(residueCollector);
            // Exercise system
            sut.Create<PropertyHolder<AbstractType>>();
            // Verify outcome (done by callback)
            // Teardown
        }

        [Fact]
        public void CreateAnonymousOnUnregisteredAbstractionWillReturnInstanceFromResidueCollector()
        {
            // Fixture setup
            var expectedValue = new ConcreteType();

            var residueCollector = new DelegatingSpecimenBuilder();
            residueCollector.OnCreate = (r, c) => expectedValue;

            var sut = new Fixture();
            sut.ResidueCollectors.Add(residueCollector);
            // Exercise system
            var result = sut.Create<PropertyHolder<AbstractType>>().Property;
            // Verify outcome
            Assert.Equal<AbstractType>(expectedValue, result);
            // Teardown
        }

        [Fact]
        public void FreezeWillCauseCreateAnonymousToKeepReturningTheFrozenInstance()
        {
            // Fixture setup
            var sut = new Fixture();
            var expectedResult = sut.Freeze<Guid>();
            // Exercise system
            var result = sut.Create<Guid>();
            // Verify outcome
            Assert.Equal<Guid>(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void FreezeWillCauseFixtureToKeepReturningTheFrozenInstanceEvenAsPropertyOfOtherType()
        {
            // Fixture setup
            var sut = new Fixture();
            var expectedResult = sut.Freeze<DateTime>();
            // Exercise system
            var result = sut.Create<PropertyHolder<DateTime>>().Property;
            // Verify outcome
            Assert.Equal<DateTime>(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void FreezeWithSeedWillCauseFixtureToKeepReturningTheFrozenInstance()
        {
            // Fixture setup
            var sut = new Fixture();
            var expectedResult = sut.Freeze("Frozen");
            // Exercise system
            var result = sut.Create("Something else");
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void FreezeWithNullTransformationThrows()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Freeze((Func<ICustomizationComposer<object>, ISpecimenBuilder>)null));
            // Teardown
        }

        [Fact]
        public void FreezeBuiltInstanceWillCauseFixtureToKeepReturningTheFrozenInstance()
        {
            // Fixture setup
            var sut = new Fixture();
            var frozen = sut.Freeze<DoublePropertyHolder<DateTime, Guid>>(ob => ob.OmitAutoProperties().With(x => x.Property1));
            // Exercise system
            var result = sut.Create<DoublePropertyHolder<DateTime, Guid>>();
            // Verify outcome
            Assert.Equal(frozen.Property1, result.Property1);
            Assert.Equal(frozen.Property2, result.Property2);
            // Teardown
        }

        [Fact]
        public void CreateManyWithDoCustomizationWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new Fixture();
            sut.Customize<List<string>>(ob => ob.Do(sut.AddManyTo).OmitAutoProperties());
            // Exercise system
            var result = sut.CreateMany<List<string>>();
            // Verify outcome
            Assert.True(result.All(l => l.Count == sut.RepeatCount), "Customize/Do/CreateMany");
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void OmitAutoPropertiesFollowedByOptInWillNotSetOtherPropertiesObsolete()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.Build<DoublePropertyHolder<object, object>>()
                .OmitAutoProperties()
                .With(x => x.Property1)
                .CreateAnonymous();
            // Verify outcome
            Assert.Null(result.Property2);
            // Teardown
        }

        [Fact]
        public void OmitAutoPropertiesFollowedByOptInWillNotSetOtherProperties()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.Build<DoublePropertyHolder<object, object>>()
                .OmitAutoProperties()
                .With(x => x.Property1)
                .Create();
            // Verify outcome
            Assert.Null(result.Property2);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void OmitAutoPropertiesFollowedByTwoOptInsWillNotSetAnyOtherPropertiesObsolete()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.Build<TriplePropertyHolder<int, int, object>>()
                .OmitAutoProperties()
                .With(x => x.Property1, 42)
                .With(x => x.Property2, 1337)
                .CreateAnonymous();
            // Verify outcome
            Assert.Equal(42, result.Property1);
            Assert.Equal(1337, result.Property2);
            Assert.Null(result.Property3);
            // Teardown
        }

        [Fact]
        public void OmitAutoPropertiesFollowedByTwoOptInsWillNotSetAnyOtherProperties()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.Build<TriplePropertyHolder<int, int, object>>()
                .OmitAutoProperties()
                .With(x => x.Property1, 42)
                .With(x => x.Property2, 1337)
                .Create();
            // Verify outcome
            Assert.Equal(42, result.Property1);
            Assert.Equal(1337, result.Property2);
            Assert.Null(result.Property3);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void WithTwoOptInsFollowedByOmitAutoPropertiesWillNotSetAnyOtherPropertiesObsolete()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.Build<TriplePropertyHolder<int, int, object>>()
                .With(x => x.Property1, 42)
                .With(x => x.Property2, 1337)
                .OmitAutoProperties()
                .CreateAnonymous();
            // Verify outcome
            Assert.Equal(42, result.Property1);
            Assert.Equal(1337, result.Property2);
            Assert.Null(result.Property3);
            // Teardown
        }

        [Fact]
        public void WithTwoOptInsFollowedByOmitAutoPropertiesWillNotSetAnyOtherProperties()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.Build<TriplePropertyHolder<int, int, object>>()
                .With(x => x.Property1, 42)
                .With(x => x.Property2, 1337)
                .OmitAutoProperties()
                .Create();
            // Verify outcome
            Assert.Equal(42, result.Property1);
            Assert.Equal(1337, result.Property2);
            Assert.Null(result.Property3);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWillThrowOnReferenceRecursionPoint()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            Assert.Throws<ObjectCreationException>(() =>
                sut.Create<RecursionTestObjectWithReferenceOutA>());
        }

        [Fact]
        public void CreateAnonymousWillThrowOnConstructorRecursionPoint()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            Assert.Throws<ObjectCreationException>(() =>
                sut.Create<RecursionTestObjectWithConstructorReferenceOutA>());
        }

        [Fact]
        [Obsolete]
        public void BuildWithThrowingRecursionHandlerWillThrowOnReferenceRecursionPointObsolete()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            Assert.Throws<ObjectCreationException>(() =>
                new SpecimenContext(
                    new ThrowingRecursionGuard(
                        sut.Build<RecursionTestObjectWithReferenceOutA>()
                    )
                ).CreateAnonymous<RecursionTestObjectWithReferenceOutA>());
        }

        [Fact]
        public void BuildWithThrowingRecursionHandlerWillThrowOnReferenceRecursionPoint()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            Assert.Throws<ObjectCreationException>(() =>
                new SpecimenContext(
                    new RecursionGuard(
                        sut.Build<RecursionTestObjectWithReferenceOutA>(),
                        new ThrowingRecursionHandler()
                    )
                ).Create<RecursionTestObjectWithReferenceOutA>());
        }

        [Fact]
        [Obsolete]
        public void BuildWithThrowingRecursionHandlerWillThrowOnConstructorRecursionPointObsolete()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            Assert.Throws<ObjectCreationException>(() =>
                new SpecimenContext(
                    new ThrowingRecursionGuard(
                        sut.Build<RecursionTestObjectWithConstructorReferenceOutA>()
                    )
                ).CreateAnonymous<RecursionTestObjectWithConstructorReferenceOutA>());
        }

        [Fact]
        public void BuildWithThrowingRecursionHandlerWillThrowOnConstructorRecursionPoint()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            Assert.Throws<ObjectCreationException>(() =>
                new SpecimenContext(
                    new RecursionGuard(
                        sut.Build<RecursionTestObjectWithConstructorReferenceOutA>(),
                        new ThrowingRecursionHandler()
                    )
                ).Create<RecursionTestObjectWithConstructorReferenceOutA>());
        }

        [Fact]
        [Obsolete]
        public void BuildWithNullRecursionHandlerWillCreateNullOnRecursionPointObsolete()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = new SpecimenContext(
                new NullRecursionGuard(
                    sut.Build<RecursionTestObjectWithConstructorReferenceOutA>()
                )
            ).CreateAnonymous<RecursionTestObjectWithConstructorReferenceOutA>();
            // Verify outcome
            Assert.Null(result.ReferenceToB.ReferenceToA);
        }

        [Fact]
        public void BuildWithNullRecursionHandlerWillCreateNullOnRecursionPoint()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = new SpecimenContext(
                new RecursionGuard(
                    sut.Build<RecursionTestObjectWithConstructorReferenceOutA>(),
                    new NullRecursionHandler()
                )
            ).Create<RecursionTestObjectWithConstructorReferenceOutA>();
            // Verify outcome
            Assert.Null(result.ReferenceToB.ReferenceToA);
        }

        [Fact]
        public void BuildWithOmitRecursionGuardWillOmitPropertyOnRecursionPoint()
        {
            // Fixture setup
            var sut = new Fixture();
            sut.Behaviors.Clear();
            sut.Behaviors.Add(new OmitOnRecursionBehavior());
            // Exercise system
            var actual = sut.Create<RecursionTestObjectWithReferenceOutA>();
            // Verify outcome
            Assert.Null(actual.ReferenceToB.ReferenceToA);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousOnRegisteredInstanceWillReturnInstanceWithoutAutoProperties()
        {
            // Fixture setup
            var item = new PropertyHolder<string>();
            var sut = new Fixture();
            // Exercise system
            sut.Inject(item);
            // Verify outcome
            var result = sut.Create<PropertyHolder<string>>();
            Assert.Null(result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousOnRegisteredParameterlessFuncWillReturnInstanceWithoutAutoProperties()
        {
            // Fixture setup
            var item = new PropertyHolder<string>();
            var sut = new Fixture();
            // Exercise system
            sut.Register(() => item);
            // Verify outcome
            var result = sut.Create<PropertyHolder<string>>();
            Assert.Null(result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousOnRegisteredSingleParameterFuncWillReturnInstanceWithoutAutoProperties()
        {
            // Fixture setup
            var item = new PropertyHolder<string>();
            var sut = new Fixture();
            // Exercise system
            sut.Register((object obj) => item);
            // Verify outcome
            var result = sut.Create<PropertyHolder<string>>();
            Assert.Null(result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousOnRegisteredDoubleParameterFuncWillReturnInstanceWithoutAutoProperties()
        {
            // Fixture setup
            var item = new PropertyHolder<string>();
            var sut = new Fixture();
            // Exercise system
            sut.Register((object obj1, object obj2) => item);
            // Verify outcome
            var result = sut.Create<PropertyHolder<string>>();
            Assert.Null(result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousOnRegisteredTripleParameterFuncWillReturnInstanceWithoutAutoProperties()
        {
            // Fixture setup
            var item = new PropertyHolder<string>();
            var sut = new Fixture();
            // Exercise system
            sut.Register((object obj1, object obj2, object obj3) => item);
            // Verify outcome
            var result = sut.Create<PropertyHolder<string>>();
            Assert.Null(result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousOnRegisteredQuadrupleParameterFuncWillReturnInstanceWithoutAutoProperties()
        {
            // Fixture setup
            var item = new PropertyHolder<string>();
            var sut = new Fixture();
            // Exercise system
            sut.Register((object obj1, object obj2, object obj3, object obj4) => item);
            // Verify outcome
            var result = sut.Create<PropertyHolder<string>>();
            Assert.Null(result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithOmitAutoPropertiesWillNotAssignProperty()
        {
            // Fixture setup
            Fixture sut = new Fixture() { OmitAutoProperties = true };
            // Exercise system
            PropertyHolder<string> result = sut.Create<PropertyHolder<string>>();
            // Verify outcome
            Assert.Null(result.Property);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void CustomizeInstanceWithOmitAutoPropertiesWillReturnFactoryWithOmitAutoPropertiesObsolete()
        {
            // Fixture setup
            var sut = new Fixture() { OmitAutoProperties = true };
            // Exercise system
            var builder = sut.Build<PropertyHolder<object>>();
            PropertyHolder<object> result = builder.CreateAnonymous();
            // Verify outcome
            Assert.Null(result.Property);
            // Teardown
        }

        [Fact]
        public void CustomizeInstanceWithOmitAutoPropertiesWillReturnFactoryWithOmitAutoProperties()
        {
            // Fixture setup
            var sut = new Fixture() { OmitAutoProperties = true };
            // Exercise system
            var builder = sut.Build<PropertyHolder<object>>();
            PropertyHolder<object> result = builder.Create();
            // Verify outcome
            Assert.Null(result.Property);
            // Teardown
        }

        [Fact]
        public void FreezedFirstCallToCreateAnonymousWithOmitAutoPropertiesWillNotAssignProperty()
        {
            // Fixture setup
            var sut = new Fixture() { OmitAutoProperties = true };
            // Exercise system
            var expectedResult = sut.Freeze<PropertyHolder<string>>();
            // Verify outcome
            Assert.Null(expectedResult.Property);
            // Teardown
        }

        [Fact]
        public void CustomizedBuilderCreateAnonymousWithOmitAutoPropertiesWillNotAssignProperty()
        {
            // Fixture setup
            var sut = new Fixture() { OmitAutoProperties = true };
            // Exercise system
            sut.Customize<PropertyHolder<string>>(x => x);
            var expectedResult = sut.Create<PropertyHolder<string>>();
            // Verify outcome
            Assert.Null(expectedResult.Property);
            // Teardown
        }

        [Fact]
        public void CustomizedOverrideOfOmitAutoPropertiesWillAssignProperty()
        {
            // Fixture setup
            var sut = new Fixture() { OmitAutoProperties = true };
            // Exercise system
            sut.Customize<PropertyHolder<string>>(x => x.WithAutoProperties());
            var expectedResult = sut.Create<PropertyHolder<string>>();
            // Verify outcome
            Assert.NotNull(expectedResult.Property);
            // Teardown
        }

        [Fact]
        public void DefaultOmitAutoPropertiesIsFalse()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            // Exercise system
            bool result = sut.OmitAutoProperties;
            // Verify outcome
            Assert.False(result, "OmitAutoProperties");
            // Teardown
        }

        [Fact]
        public void FromSeedWithNullFuncThrows()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Build<object>().FromSeed(null));
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void BuildFromSeedWillReturnCorrectResultObsolete()
        {
            // Fixture setup
            var sut = new Fixture();
            var expectedResult = new object();
            // Exercise system
            var result = sut.Build<object>().FromSeed(s => expectedResult).CreateAnonymous();
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void BuildFromSeedWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new Fixture();
            var expectedResult = new object();
            // Exercise system
            var result = sut.Build<object>().FromSeed(s => expectedResult).Create();
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void BuildFromSeedWillCreateUsingCorrectSeed()
        {
            // Fixture setup
            var sut = new Fixture();
            var seed = new object();

            var verified = false;
            Func<object, object> mock = s => verified = seed.Equals(s);
            // Exercise system
            sut.Build<object>().FromSeed(mock).Create(seed);
            // Verify outcome
            Assert.True(verified, "Mock verified");
            // Teardown
        }

        [Fact]
        public void CustomizeFromSeedWithUnmodifiedSeedValueWillPopulatePropertyOfSameType()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            fixture.Customize<Version>(c => c.FromSeed(s => s));
            // Verify outcome
            Assert.Null(fixture.Create<PropertyHolder<Version>>().Property);
        }

        [Fact]
        public void CustomizeFromSeedWithFixedSeedValueWillPopulatePropertyOfSameType()
        {
            // Fixture setup
            var fixture = new Fixture();
            var seed = new ConcreteType();
            // Exercise system
            fixture.Customize<ConcreteType>(c => c.FromSeed(s => seed));
            // Verify outcome
            Assert.Equal(seed, fixture.Create<PropertyHolder<ConcreteType>>().Property);
        }

        [Fact]
        [Obsolete]
        public void BuildAndCreateAnonymousWillSetInt32Property()
        {
            // Fixture setup
            int unexpectedNumber = default(int);
            var sut = new Fixture();
            // Exercise system
            PropertyHolder<int> result = sut.Build<PropertyHolder<int>>().CreateAnonymous();
            // Verify outcome
            Assert.NotEqual<int>(unexpectedNumber, result.Property);
            // Teardown
        }

        [Fact]
        public void BuildAndCreateWillSetInt32Property()
        {
            // Fixture setup
            int unexpectedNumber = default(int);
            var sut = new Fixture();
            // Exercise system
            PropertyHolder<int> result = sut.Build<PropertyHolder<int>>().Create();
            // Verify outcome
            Assert.NotEqual<int>(unexpectedNumber, result.Property);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void BuildAndCreateAnonymousWillSetInt32Field()
        {
            // Fixture setup
            int unexpectedNumber = default(int);
            var sut = new Fixture();
            // Exercise system
            FieldHolder<int> result = sut.Build<FieldHolder<int>>().CreateAnonymous();
            // Verify outcome
            Assert.NotEqual<int>(unexpectedNumber, result.Field);
            // Teardown
        }

        [Fact]
        public void BuildAndCreateWillSetInt32Field()
        {
            // Fixture setup
            int unexpectedNumber = default(int);
            var sut = new Fixture();
            // Exercise system
            FieldHolder<int> result = sut.Build<FieldHolder<int>>().Create();
            // Verify outcome
            Assert.NotEqual<int>(unexpectedNumber, result.Field);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void BuildAndCreateAnonymousWillNotAttemptToSetReadOnlyProperty()
        {
            // Fixture setup
            int expectedNumber = default(int);
            var sut = new Fixture();
            // Exercise system
            ReadOnlyPropertyHolder<int> result = sut.Build<ReadOnlyPropertyHolder<int>>().CreateAnonymous();
            // Verify outcome
            Assert.Equal<int>(expectedNumber, result.Property);
            // Teardown
        }

        [Fact]
        public void BuildAndCreateWillNotAttemptToSetReadOnlyProperty()
        {
            // Fixture setup
            int expectedNumber = default(int);
            var sut = new Fixture();
            // Exercise system
            ReadOnlyPropertyHolder<int> result = sut.Build<ReadOnlyPropertyHolder<int>>().Create();
            // Verify outcome
            Assert.Equal<int>(expectedNumber, result.Property);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void BuildAndCreateAnonymousWillNotAttemptToSetReadOnlyField()
        {
            // Fixture setup
            int expectedNumber = default(int);
            var sut = new Fixture();
            // Exercise system
            ReadOnlyFieldHolder<int> result = sut.Build<ReadOnlyFieldHolder<int>>().CreateAnonymous();
            // Verify outcome
            Assert.Equal<int>(expectedNumber, result.Field);
            // Teardown
        }

        [Fact]
        public void BuildAndCreateWillNotAttemptToSetReadOnlyField()
        {
            // Fixture setup
            int expectedNumber = default(int);
            var sut = new Fixture();
            // Exercise system
            ReadOnlyFieldHolder<int> result = sut.Build<ReadOnlyFieldHolder<int>>().Create();
            // Verify outcome
            Assert.Equal<int>(expectedNumber, result.Field);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void BuildWithWillSetPropertyOnCreatedObjectObsolete()
        {
            // Fixture setup
            string expectedText = "Anonymous text";
            var sut = new Fixture();
            // Exercise system
            PropertyHolder<string> result = sut.Build<PropertyHolder<string>>().With(ph => ph.Property, expectedText).CreateAnonymous();
            // Verify outcome
            Assert.Equal(expectedText, result.Property);
            // Teardown
        }

        [Fact]
        public void BuildWithWillSetPropertyOnCreatedObject()
        {
            // Fixture setup
            string expectedText = "Anonymous text";
            var sut = new Fixture();
            // Exercise system
            PropertyHolder<string> result = sut.Build<PropertyHolder<string>>().With(ph => ph.Property, expectedText).Create();
            // Verify outcome
            Assert.Equal(expectedText, result.Property);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void BuildWithWillSetFieldOnCreatedObjectObsolete()
        {
            // Fixture setup
            string expectedText = "Anonymous text";
            var fixture = new Fixture();
            // Exercise system
            FieldHolder<string> result = fixture.Build<FieldHolder<string>>().With(fh => fh.Field, expectedText).CreateAnonymous();
            // Verify outcome
            Assert.Equal(expectedText, result.Field);
            // Teardown
        }

        [Fact]
        public void BuildWithWillSetFieldOnCreatedObject()
        {
            // Fixture setup
            string expectedText = "Anonymous text";
            var fixture = new Fixture();
            // Exercise system
            FieldHolder<string> result = fixture.Build<FieldHolder<string>>().With(fh => fh.Field, expectedText).Create();
            // Verify outcome
            Assert.Equal(expectedText, result.Field);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void BuildAnonymousWithWillAssignPropertyEvenInCombinationWithOmitAutoPropertiesObsolete()
        {
            // Fixture setup
            long unexpectedNumber = default(long);
            var sut = new Fixture();
            // Exercise system
            var result = sut.Build<DoublePropertyHolder<long, long>>().With(ph => ph.Property1).OmitAutoProperties().CreateAnonymous();
            // Verify outcome
            Assert.NotEqual<long>(unexpectedNumber, result.Property1);
            // Teardown
        }

        [Fact]
        public void BuildAnonymousWithWillAssignPropertyEvenInCombinationWithOmitAutoProperties()
        {
            // Fixture setup
            long unexpectedNumber = default(long);
            var sut = new Fixture();
            // Exercise system
            var result = sut.Build<DoublePropertyHolder<long, long>>().With(ph => ph.Property1).OmitAutoProperties().Create();
            // Verify outcome
            Assert.NotEqual<long>(unexpectedNumber, result.Property1);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void BuildAnonymousWithWillAssignFieldEvenInCombinationWithOmitAutoProperties()
        {
            // Fixture setup
            int unexpectedNumber = default(int);
            var sut = new Fixture();
            // Exercise system
            var result = sut.Build<DoubleFieldHolder<int, decimal>>().With(fh => fh.Field1).OmitAutoProperties().CreateAnonymous();
            // Verify outcome
            Assert.NotEqual<int>(unexpectedNumber, result.Field1);
            // Teardown
        }

        [Fact]
        public void BuildWithWillAssignFieldEvenInCombinationWithOmitAutoProperties()
        {
            // Fixture setup
            int unexpectedNumber = default(int);
            var sut = new Fixture();
            // Exercise system
            var result = sut.Build<DoubleFieldHolder<int, decimal>>().With(fh => fh.Field1).OmitAutoProperties().Create();
            // Verify outcome
            Assert.NotEqual<int>(unexpectedNumber, result.Field1);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void BuildWithoutWillIgnorePropertyOnCreatedObjectObsolete()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.Build<DoublePropertyHolder<string, string>>().Without(ph => ph.Property1).CreateAnonymous();
            // Verify outcome
            Assert.Null(result.Property1);
            // Teardown
        }

        [Fact]
        public void BuildWithoutWillIgnorePropertyOnCreatedObject()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.Build<DoublePropertyHolder<string, string>>().Without(ph => ph.Property1).Create();
            // Verify outcome
            Assert.Null(result.Property1);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void BuildWithoutWillIgnorePropertyOnCreatedObjectEvenInCombinationWithWithAutoPropertiesObsolete()
        {
            // Fixture setup
            var sut = new Fixture() { OmitAutoProperties = true };
            // Exercise system
            var result = sut.Build<DoublePropertyHolder<string, string>>().WithAutoProperties().Without(ph => ph.Property1).CreateAnonymous();
            // Verify outcome
            Assert.Null(result.Property1);
            // Teardown
        }

        [Fact]
        public void BuildWithoutWillIgnorePropertyOnCreatedObjectEvenInCombinationWithWithAutoProperties()
        {
            // Fixture setup
            var sut = new Fixture() { OmitAutoProperties = true };
            // Exercise system
            var result = sut.Build<DoublePropertyHolder<string, string>>().WithAutoProperties().Without(ph => ph.Property1).Create();
            // Verify outcome
            Assert.Null(result.Property1);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void BuildWithoutWillIgnoreFieldOnCreatedObjectObsolete()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.Build<DoubleFieldHolder<string, string>>().Without(fh => fh.Field1).CreateAnonymous();
            // Verify outcome
            Assert.Null(result.Field1);
            // Teardown
        }

        [Fact]
        public void BuildWithoutWillIgnoreFieldOnCreatedObject()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.Build<DoubleFieldHolder<string, string>>().Without(fh => fh.Field1).Create();
            // Verify outcome
            Assert.Null(result.Field1);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void BuildWithoutWillNotIgnoreOtherPropertyOnCreatedObjectObsolete()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.Build<DoublePropertyHolder<string, string>>().Without(ph => ph.Property1).CreateAnonymous();
            // Verify outcome
            Assert.NotNull(result.Property2);
            // Teardown
        }

        [Fact]
        public void BuildWithoutWillNotIgnoreOtherPropertyOnCreatedObject()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.Build<DoublePropertyHolder<string, string>>().Without(ph => ph.Property1).Create();
            // Verify outcome
            Assert.NotNull(result.Property2);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void BuildWithoutWillNotIgnoreOtherFieldOnCreatedObjectObsolete()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.Build<DoubleFieldHolder<string, string>>().Without(fh => fh.Field1).CreateAnonymous();
            // Verify outcome
            Assert.NotNull(result.Field2);
            // Teardown
        }

        [Fact]
        public void BuildWithoutWillNotIgnoreOtherFieldOnCreatedObject()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.Build<DoubleFieldHolder<string, string>>().Without(fh => fh.Field1).Create();
            // Verify outcome
            Assert.NotNull(result.Field2);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void BuildAndOmitAutoPropertiesWillNotAutoPopulatePropertyObsolete()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            PropertyHolder<object> result = sut.Build<PropertyHolder<object>>().OmitAutoProperties().CreateAnonymous();
            // Verify outcome
            Assert.Null(result.Property);
            // Teardown
        }

        [Fact]
        public void BuildAndOmitAutoPropertiesWillNotAutoPopulateProperty()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            PropertyHolder<object> result = sut.Build<PropertyHolder<object>>().OmitAutoProperties().Create();
            // Verify outcome
            Assert.Null(result.Property);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void BuildWithAutoPropertiesWillAutoPopulatePropertyObsolete()
        {
            // Fixture setup
            var sut = new Fixture { OmitAutoProperties = true };
            // Exercise system
            PropertyHolder<object> result = sut.Build<PropertyHolder<object>>().WithAutoProperties().CreateAnonymous();
            // Verify outcome
            Assert.NotNull(result.Property);
            // Teardown
        }

        [Fact]
        public void BuildWithAutoPropertiesWillAutoPopulateProperty()
        {
            // Fixture setup
            var sut = new Fixture { OmitAutoProperties = true };
            // Exercise system
            PropertyHolder<object> result = sut.Build<PropertyHolder<object>>().WithAutoProperties().Create();
            // Verify outcome
            Assert.NotNull(result.Property);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void BuildAndDoWillPerformOperationOnCreatedObjectObsolete()
        {
            // Fixture setup
            var sut = new Fixture();
            var expectedObject = new object();
            // Exercise system
            var result = sut.Build<CollectionHolder<object>>().Do(x => x.Collection.Add(expectedObject)).CreateAnonymous().Collection.First();
            // Verify outcome
            Assert.Equal<object>(expectedObject, result);
            // Teardown
        }

        [Fact]
        public void BuildAndDoWillPerformOperationOnCreatedObject()
        {
            // Fixture setup
            var sut = new Fixture();
            var expectedObject = new object();
            // Exercise system
            var result = sut.Build<CollectionHolder<object>>().Do(x => x.Collection.Add(expectedObject)).Create().Collection.First();
            // Verify outcome
            Assert.Equal<object>(expectedObject, result);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void BuilderSequenceWillBePreservedObsolete()
        {
            // Fixture setup
            var sut = new Fixture();
            int expectedValue = 3;
            // Exercise system
            var result = sut.Build<PropertyHolder<int>>()
                .With(x => x.Property, 1)
                .Do(x => x.SetProperty(2))
                .With(x => x.Property, expectedValue)
                .CreateAnonymous();
            // Verify outcome
            Assert.Equal<int>(expectedValue, result.Property);
            // Teardown
        }


        [Fact]
        public void BuilderSequenceWillBePreserved()
        {
            // Fixture setup
            var sut = new Fixture();
            int expectedValue = 3;
            // Exercise system
            var result = sut.Build<PropertyHolder<int>>()
                .With(x => x.Property, 1)
                .Do(x => x.SetProperty(2))
                .With(x => x.Property, expectedValue)
                .Create();
            // Verify outcome
            Assert.Equal<int>(expectedValue, result.Property);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void BuildAndCreateAnonymousWillInvokeResidueCollector()
        {
            // Fixture setup
            bool resolveWasInvoked = false;

            var residueCollector = new DelegatingSpecimenBuilder();
            residueCollector.OnCreate = (r, c) =>
            {
                resolveWasInvoked = true;
                return new ConcreteType();
            };

            var sut = new Fixture();
            sut.ResidueCollectors.Add(residueCollector);
            // Exercise system
            sut.Build<PropertyHolder<AbstractType>>().CreateAnonymous();
            // Verify outcome
            Assert.True(resolveWasInvoked, "Resolve");
            // Teardown
        }

        [Fact]
        public void BuildAndCreateWillInvokeResidueCollector()
        {
            // Fixture setup
            bool resolveWasInvoked = false;

            var residueCollector = new DelegatingSpecimenBuilder();
            residueCollector.OnCreate = (r, c) =>
            {
                resolveWasInvoked = true;
                return new ConcreteType();
            };

            var sut = new Fixture();
            sut.ResidueCollectors.Add(residueCollector);
            // Exercise system
            sut.Build<PropertyHolder<AbstractType>>().Create();
            // Verify outcome
            Assert.True(resolveWasInvoked, "Resolve");
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void BuildAndCreateAnonymousOnUnregisteredAbstractionWillInvokeResidueCollectorWithCorrectType()
        {
            // Fixture setup
            var residueCollector = new DelegatingSpecimenBuilder();
            residueCollector.OnCreate = (r, c) =>
            {
                Assert.Equal(typeof(AbstractType), r);
                return new ConcreteType();
            };

            var sut = new Fixture();
            sut.ResidueCollectors.Add(residueCollector);
            // Exercise system
            sut.Build<PropertyHolder<AbstractType>>().CreateAnonymous();
            // Verify outcome (done by callback)
            // Teardown
        }

        [Fact]
        public void BuildAndCreateOnUnregisteredAbstractionWillInvokeResidueCollectorWithCorrectType()
        {
            // Fixture setup
            var residueCollector = new DelegatingSpecimenBuilder();
            residueCollector.OnCreate = (r, c) =>
            {
                Assert.Equal(typeof(AbstractType), r);
                return new ConcreteType();
            };

            var sut = new Fixture();
            sut.ResidueCollectors.Add(residueCollector);
            // Exercise system
            sut.Build<PropertyHolder<AbstractType>>().Create();
            // Verify outcome (done by callback)
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void BuildAndCreateAnonymousOnUnregisteredAbstractionWillReturnInstanceFromResidueCollector()
        {
            // Fixture setup
            var expectedValue = new ConcreteType();

            var residueCollector = new DelegatingSpecimenBuilder();
            residueCollector.OnCreate = (r, c) => expectedValue;

            var sut = new Fixture();
            sut.ResidueCollectors.Add(residueCollector);
            // Exercise system
            var result = sut.Build<PropertyHolder<AbstractType>>().CreateAnonymous().Property;
            // Verify outcome
            Assert.Equal<AbstractType>(expectedValue, result);
            // Teardown
        }

        [Fact]
        public void BuildAndCreateOnUnregisteredAbstractionWillReturnInstanceFromResidueCollector()
        {
            // Fixture setup
            var expectedValue = new ConcreteType();

            var residueCollector = new DelegatingSpecimenBuilder();
            residueCollector.OnCreate = (r, c) => expectedValue;

            var sut = new Fixture();
            sut.ResidueCollectors.Add(residueCollector);
            // Exercise system
            var result = sut.Build<PropertyHolder<AbstractType>>().Create().Property;
            // Verify outcome
            Assert.Equal<AbstractType>(expectedValue, result);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void BuildAndOmitAutoPropertiesWillNotMutateSutObsolete()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.Build<PropertyHolder<string>>();
            // Exercise system
            sut.OmitAutoProperties();
            // Verify outcome
            var instance = sut.CreateAnonymous();
            Assert.NotNull(instance.Property);
            // Teardown
        }

        [Fact]
        public void BuildAndOmitAutoPropertiesWillNotMutateSut()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.Build<PropertyHolder<string>>();
            // Exercise system
            sut.OmitAutoProperties();
            // Verify outcome
            var instance = sut.Create();
            Assert.NotNull(instance.Property);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void BuildWithAutoPropertiesWillNotMutateSutObsolete()
        {
            // Fixture setup
            var fixture = new Fixture() { OmitAutoProperties = true };
            var sut = fixture.Build<PropertyHolder<string>>();
            // Exercise system
            sut.WithAutoProperties();
            // Verify outcome
            var instance = sut.CreateAnonymous();
            Assert.Null(instance.Property);
            // Teardown
        }

        [Fact]
        public void BuildWithAutoPropertiesWillNotMutateSut()
        {
            // Fixture setup
            var fixture = new Fixture() { OmitAutoProperties = true };
            var sut = fixture.Build<PropertyHolder<string>>();
            // Exercise system
            sut.WithAutoProperties();
            // Verify outcome
            var instance = sut.Create();
            Assert.Null(instance.Property);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void BuildAnonymousWithWillNotMutateSut()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.Build<PropertyHolder<string>>().OmitAutoProperties();
            // Exercise system
            sut.With(s => s.Property);
            // Verify outcome
            var instance = sut.CreateAnonymous();
            Assert.Null(instance.Property);
            // Teardown
        }

        [Fact]
        public void BuildWithWillNotMutateSut()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.Build<PropertyHolder<string>>().OmitAutoProperties();
            // Exercise system
            sut.With(s => s.Property);
            // Verify outcome
            var instance = sut.Create();
            Assert.Null(instance.Property);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void BuildAnonymousWithUnexpectedWillNotMutateSut()
        {
            // Fixture setup
            var fixture = new Fixture();
            var unexpectedProperty = "Anonymous value";
            var sut = fixture.Build<PropertyHolder<string>>();
            // Exercise system
            sut.With(s => s.Property, unexpectedProperty);
            // Verify outcome
            var instance = sut.CreateAnonymous();
            Assert.NotEqual(unexpectedProperty, instance.Property);
            // Teardown
        }

        [Fact]
        public void BuildWithUnexpectedWillNotMutateSut()
        {
            // Fixture setup
            var fixture = new Fixture();
            var unexpectedProperty = "Anonymous value";
            var sut = fixture.Build<PropertyHolder<string>>();
            // Exercise system
            sut.With(s => s.Property, unexpectedProperty);
            // Verify outcome
            var instance = sut.Create();
            Assert.NotEqual(unexpectedProperty, instance.Property);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void BuildWithoutWillNotMutateSutObsolete()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.Build<PropertyHolder<string>>();
            // Exercise system
            sut.Without(s => s.Property);
            // Verify outcome
            var instance = sut.CreateAnonymous();
            Assert.NotNull(instance.Property);
            // Teardown
        }

        [Fact]
        public void BuildWithoutWillNotMutateSut()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.Build<PropertyHolder<string>>();
            // Exercise system
            sut.Without(s => s.Property);
            // Verify outcome
            var instance = sut.Create();
            Assert.NotNull(instance.Property);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void BuildAndCreateAnonymousWillReturnCreatedObject()
        {
            // Fixture setup
            object expectedObject = new object();
            var sut = new Fixture();
            // Exercise system
            object result = sut.Build<object>().FromSeed(seed => expectedObject).CreateAnonymous();
            // Verify outcome
            Assert.Equal<object>(expectedObject, result);
            // Teardown
        }

        [Fact]
        public void BuildAndCreateWillReturnCreatedObject()
        {
            // Fixture setup
            object expectedObject = new object();
            var sut = new Fixture();
            // Exercise system
            object result = sut.Build<object>().FromSeed(seed => expectedObject).Create();
            // Verify outcome
            Assert.Equal<object>(expectedObject, result);
            // Teardown
        }

        [Fact]
        public void BuildAndCreateManyWillCreateManyAnonymousItems()
        {
            // Fixture setup
            var sut = new Fixture();
            var expectedItemCount = sut.RepeatCount;
            // Exercise system
            IEnumerable<PropertyHolder<int>> result = sut.Build<PropertyHolder<int>>().CreateMany();
            // Verify outcome
            var uniqueItemCount = (from ph in result
                                   select ph.Property).Distinct().Count();
            Assert.Equal<int>(expectedItemCount, uniqueItemCount);
            // Teardown
        }

        [Fact]
        public void BuildAndCreateManyWillCreateCorrectNumberOfItems()
        {
            // Fixture setup
            int expectedCount = 401;
            var sut = new Fixture();
            // Exercise system
            IEnumerable<PropertyHolder<int>> result = sut.Build<PropertyHolder<int>>().CreateMany(expectedCount);
            // Verify outcome
            var uniqueItemCount = (from ph in result
                                   select ph.Property).Distinct().Count();
            Assert.Equal<int>(expectedCount, uniqueItemCount);
            // Teardown
        }

        [Fact]
        public void BuildAndCreateManyWithSeedWillCreateManyCorrectItems()
        {
            // Fixture setup
            string anonymousPrefix = "AnonymousPrefix";
            var sut = new Fixture();
            var expectedItemCount = sut.RepeatCount;
            // Exercise system
            IEnumerable<string> result = sut.Build<string>()
                .FromSeed(seed => seed + Guid.NewGuid())
                .CreateMany(anonymousPrefix);
            // Verify outcome
            int actualCount = (from s in result
                               where s.StartsWith(anonymousPrefix)
                               select s).Count();
            Assert.Equal<int>(expectedItemCount, actualCount);
            // Teardown
        }

        [Fact]
        public void BuildAndCreateManyWithSeedWillCreateCorrectNumberOfItems()
        {
            // Fixture setup
            string anonymousPrefix = "Prefix";
            int expectedItemCount = 29;
            var sut = new Fixture();
            sut.RepeatCount = expectedItemCount;
            // Exercise system
            IEnumerable<string> result = sut.Build<string>()
                .FromSeed(seed => seed + Guid.NewGuid())
                .CreateMany(anonymousPrefix, expectedItemCount);
            // Verify outcome
            int actualCount = (from s in result
                               where s.StartsWith(anonymousPrefix)
                               select s).Count();
            Assert.Equal<int>(expectedItemCount, actualCount);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void BuildAndCreateAnonymousWillCreateObject()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            object result = sut.Build<object>().CreateAnonymous();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void BuildAndCreateWillCreateObject()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            object result = sut.Build<object>().Create();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void BuildAndCreateAnonymousAfterDefiningConstructorWithZeroParametersWillReturnDefinedObject()
        {
            // Fixture setup
            var sut = new Fixture();
            object expectedObject = new object();
            // Exercise system
            var result = sut.Build<object>()
                .FromFactory(() => expectedObject)
                .CreateAnonymous();
            // Verify outcome
            Assert.Equal<object>(expectedObject, result);
            // Teardown
        }

        [Fact]
        public void BuildAndCreateAfterDefiningConstructorWithZeroParametersWillReturnDefinedObject()
        {
            // Fixture setup
            var sut = new Fixture();
            object expectedObject = new object();
            // Exercise system
            var result = sut.Build<object>()
                .FromFactory(() => expectedObject)
                .Create();
            // Verify outcome
            Assert.Equal<object>(expectedObject, result);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void BuildAndCreateAnonymousAfterDefiningConstructorWithOneParameterWillReturnDefinedObject()
        {
            // Fixture setup
            var sut = new Fixture();
            SingleParameterType<object> expectedObject = new SingleParameterType<object>(new object());
            // Exercise system
            var result = sut.Build<SingleParameterType<object>>()
                .FromFactory<object>(obj => expectedObject)
                .CreateAnonymous();
            // Verify outcome
            Assert.Equal<SingleParameterType<object>>(expectedObject, result);
            // Teardown
        }

        [Fact]
        public void BuildAndCreateAfterDefiningConstructorWithOneParameterWillReturnDefinedObject()
        {
            // Fixture setup
            var sut = new Fixture();
            SingleParameterType<object> expectedObject = new SingleParameterType<object>(new object());
            // Exercise system
            var result = sut.Build<SingleParameterType<object>>()
                .FromFactory<object>(obj => expectedObject)
                .Create();
            // Verify outcome
            Assert.Equal<SingleParameterType<object>>(expectedObject, result);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void BuildAndCreateAnonymousAfterDefiningConstructorWithTwoParametersWillReturnDefinedObject()
        {
            // Fixture setup
            var sut = new Fixture();
            DoubleParameterType<object, object> expectedObject = new DoubleParameterType<object, object>(new object(), new object());
            // Exercise system
            var result = sut.Build<DoubleParameterType<object, object>>()
                .FromFactory<object, object>((o1, o2) => expectedObject)
                .CreateAnonymous();
            // Verify outcome
            Assert.Equal<DoubleParameterType<object, object>>(expectedObject, result);
            // Teardown
        }

        [Fact]
        public void BuildAndCreateAfterDefiningConstructorWithTwoParametersWillReturnDefinedObject()
        {
            // Fixture setup
            var sut = new Fixture();
            DoubleParameterType<object, object> expectedObject = new DoubleParameterType<object, object>(new object(), new object());
            // Exercise system
            var result = sut.Build<DoubleParameterType<object, object>>()
                .FromFactory<object, object>((o1, o2) => expectedObject)
                .Create();
            // Verify outcome
            Assert.Equal<DoubleParameterType<object, object>>(expectedObject, result);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void BuildAndCreateAnonymousAfterDefiningConstructorWithThreeParametersWillReturnDefinedObject()
        {
            // Fixture setup
            var sut = new Fixture();
            TripleParameterType<object, object, object> expectedObject = new TripleParameterType<object, object, object>(new object(), new object(), new object());
            // Exercise system
            var result = sut.Build<TripleParameterType<object, object, object>>()
                .FromFactory<object, object, object>((o1, o2, o3) => expectedObject)
                .CreateAnonymous();
            // Verify outcome
            Assert.Equal<TripleParameterType<object, object, object>>(expectedObject, result);
            // Teardown
        }

        [Fact]
        public void BuildAndCreateAfterDefiningConstructorWithThreeParametersWillReturnDefinedObject()
        {
            // Fixture setup
            var sut = new Fixture();
            TripleParameterType<object, object, object> expectedObject = new TripleParameterType<object, object, object>(new object(), new object(), new object());
            // Exercise system
            var result = sut.Build<TripleParameterType<object, object, object>>()
                .FromFactory<object, object, object>((o1, o2, o3) => expectedObject)
                .Create();
            // Verify outcome
            Assert.Equal<TripleParameterType<object, object, object>>(expectedObject, result);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void BuildAndCreateAnonymousAfterDefiningConstructorWithFourParametersWillReturnDefinedObject()
        {
            // Fixture setup
            var sut = new Fixture();
            QuadrupleParameterType<object, object, object, object> expectedObject = new QuadrupleParameterType<object, object, object, object>(new object(), new object(), new object(), new object());
            // Exercise system
            var result = sut.Build<QuadrupleParameterType<object, object, object, object>>()
                .FromFactory<object, object, object, object>((o1, o2, o3, o4) => expectedObject)
                .CreateAnonymous();
            // Verify outcome
            Assert.Equal<QuadrupleParameterType<object, object, object, object>>(expectedObject, result);
            // Teardown
        }

        [Fact]
        public void BuildAndCreateAfterDefiningConstructorWithFourParametersWillReturnDefinedObject()
        {
            // Fixture setup
            var sut = new Fixture();
            QuadrupleParameterType<object, object, object, object> expectedObject = new QuadrupleParameterType<object, object, object, object>(new object(), new object(), new object(), new object());
            // Exercise system
            var result = sut.Build<QuadrupleParameterType<object, object, object, object>>()
                .FromFactory<object, object, object, object>((o1, o2, o3, o4) => expectedObject)
                .Create();
            // Verify outcome
            Assert.Equal<QuadrupleParameterType<object, object, object, object>>(expectedObject, result);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void BuildFromFactoryStillAppliesAutoPropertiesObsolete()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.Build<PropertyHolder<string>>()
                .FromFactory(() => new PropertyHolder<string>())
                .CreateAnonymous();
            // Verify outcome
            Assert.NotNull(result.Property);
            // Teardown
        }

        [Fact]
        public void BuildFromFactoryStillAppliesAutoProperties()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.Build<PropertyHolder<string>>()
                .FromFactory(() => new PropertyHolder<string>())
                .Create();
            // Verify outcome
            Assert.NotNull(result.Property);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void BuildOverwritesPreviousFactoryBasedCustomizationObsolete()
        {
            // Fixture setup
            var sut = new Fixture();
            sut.Customize<PropertyHolder<object>>(c => c.FromFactory(() => new PropertyHolder<object>()));
            // Exercise system
            var result = sut.Build<PropertyHolder<object>>().OmitAutoProperties().CreateAnonymous();
            // Verify outcome
            Assert.Null(result.Property);
            // Teardown
        }

        [Fact]
        public void BuildOverwritesPreviousFactoryBasedCustomization()
        {
            // Fixture setup
            var sut = new Fixture();
            sut.Customize<PropertyHolder<object>>(c => c.FromFactory(() => new PropertyHolder<object>()));
            // Exercise system
            var result = sut.Build<PropertyHolder<object>>().OmitAutoProperties().Create();
            // Verify outcome
            Assert.Null(result.Property);
            // Teardown
        }

        [Fact]
        public void NewestCustomizationWins()
        {
            // Fixture setup
            var sut = new Fixture();
            sut.Customize<string>(c => c.FromFactory(() => "ploeh"));

            var expectedResult = "fnaah";
            // Exercise system
            sut.Customize<string>(c => c.FromFactory(() => expectedResult));
            var result = sut.Create<string>();
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void BuildAndComposeWillCarryBehaviorsForward()
        {
            // Fixture setup
            var sut = new Fixture();
            sut.Behaviors.Clear();

            var expectedBuilder = new DelegatingSpecimenBuilder();
            sut.Behaviors.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode(1, b) });
            // Exercise system
            var result = sut.Build<object>();
            // Verify outcome
            var comparer = new TaggedNodeComparer(new TrueComparer<ISpecimenBuilder>());
            var composite = Assert.IsAssignableFrom<CompositeNodeComposer<object>>(result);
            Assert.Equal(new TaggedNode(1), composite.Node.First(), comparer);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void BuildAbstractClassThrowsObsolete()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system and verify outcome
            Assert.Throws<ObjectCreationException>(() =>
                sut.Build<AbstractType>().CreateAnonymous());
            // Teardown
        }

        [Fact]
        public void BuildAbstractClassThrows()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system and verify outcome
            Assert.Throws<ObjectCreationException>(() =>
                sut.Build<AbstractType>().Create());
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void BuildAbstractTypeUsingStronglyTypedFactoryIsPossibleObsolete()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.Build<AbstractType>().FromFactory(() => new ConcreteType()).CreateAnonymous();
            // Verify outcome
            Assert.IsAssignableFrom<ConcreteType>(result);
            // Teardown
        }

        [Fact]
        public void BuildAbstractTypeUsingStronglyTypedFactoryIsPossible()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.Build<AbstractType>().FromFactory(() => new ConcreteType()).Create();
            // Verify outcome
            Assert.IsAssignableFrom<ConcreteType>(result);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void BuildAbstractTypeUsingBuilderIsPossibleObsolete()
        {
            // Fixture setup
            var sut = new Fixture();
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => new ConcreteType() };
            // Exercise system
            var result = sut.Build<AbstractType>().FromFactory(builder).CreateAnonymous();
            // Verify outcome
            Assert.IsAssignableFrom<ConcreteType>(result);
            // Teardown
        }


        [Fact]
        public void BuildAbstractTypeUsingBuilderIsPossible()
        {
            // Fixture setup
            var sut = new Fixture();
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => new ConcreteType() };
            // Exercise system
            var result = sut.Build<AbstractType>().FromFactory(builder).Create();
            // Verify outcome
            Assert.IsAssignableFrom<ConcreteType>(result);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void BuildAbstractTypeCorrectlyAppliesPropertyObsolete()
        {
            // Fixture setup
            var expected = new object();
            var sut = new Fixture();
            // Exercise system
            var result = sut.Build<AbstractType>()
                .FromFactory(() => new ConcreteType())
                .With(x => x.Property1, expected)
                .CreateAnonymous();
            // Verify outcome
            Assert.Equal(expected, result.Property1);
            // Teardown
        }

        [Fact]
        public void BuildAbstractTypeCorrectlyAppliesProperty()
        {
            // Fixture setup
            var expected = new object();
            var sut = new Fixture();
            // Exercise system
            var result = sut.Build<AbstractType>()
                .FromFactory(() => new ConcreteType())
                .With(x => x.Property1, expected)
                .Create();
            // Verify outcome
            Assert.Equal(expected, result.Property1);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void RegisterNullWillAssignCorrectPickedPropertyValueObsolete()
        {
            // Fixture setup
            var sut = new Fixture();
            sut.Register(() => (string)null);
            // Exercise system
            var result = sut.Build<PropertyHolder<string>>().With(p => p.Property).CreateAnonymous();
            // Verify outcome
            Assert.Null(result.Property);
            // Teardown
        }

        [Fact]
        public void RegisterNullWillAssignCorrectPickedPropertyValue()
        {
            // Fixture setup
            var sut = new Fixture();
            sut.Register(() => (string)null);
            // Exercise system
            var result = sut.Build<PropertyHolder<string>>().With(p => p.Property).Create();
            // Verify outcome
            Assert.Null(result.Property);
            // Teardown
        }

        [Fact]
        public void AddingTracingBehaviorWillTraceDiagnostics()
        {
            // Fixture setup
            using (var writer = new StringWriter())
            {
                var sut = new Fixture();
                sut.Behaviors.Add(new TracingBehavior(writer));
                // Exercise system
                sut.Create<int>();
                // Verify outcome
                Assert.False(string.IsNullOrEmpty(writer.ToString()));
                // Teardown
            }
        }

        [Fact]
        public void CreateAnonymousEnumReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.Create<TriState>();
            // Verify outcome
            Assert.Equal(TriState.First, result);
            // Teardown
        }

        [Fact]
        public void CreateManyAnonymousFlagEnumsReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.CreateMany<ActivityScope>(100).ToArray().Last();
            // Verify outcome
            Assert.Equal(ActivityScope.Standalone, result);
            // Teardown
        }

        [Fact]
        public void CustomizeNullCustomizationThrows()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Customize((ICustomization)null));
            // Teardown
        }

        [Fact]
        public void CustomizeCorrectlyAppliesCustomization()
        {
            // Fixture setup
            var sut = new Fixture();

            var verified = false;
            var customization = new DelegatingCustomization { OnCustomize = f => verified = f == sut };
            // Exercise system
            sut.Customize(customization);
            // Verify outcome
            Assert.True(verified, "Mock verified");
            // Teardown
        }

        [Fact]
        public void CustomizeReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new Fixture();
            var dummyCustomization = new DelegatingCustomization();
            // Exercise system
            var result = sut.Customize(dummyCustomization);
            // Verify outcome
            Assert.Equal(sut, result);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousIntPtrThrowsCorrectException()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system and verify outcome
           var creationEx = Assert.Throws<ObjectCreationException>(() =>
                sut.Create<IntPtr>());
            Assert.IsAssignableFrom<IllegalRequestException>(creationEx.InnerException);
            // Teardown
        }

        [Fact]
        [Obsolete]
        public void BuildWithOverriddenVirtualPropertyCorrectlySetsPropertyObsolete()
        {
            // Fixture setup
            var sut = new Fixture();
            var expected = Guid.NewGuid();
            // Exercise system
            var result = sut.Build<ConcreteType>().With(x => x.Property4, expected).CreateAnonymous();
            // Verify outcome
            Assert.Equal(expected, result.Property4);
            // Teardown
        }

        [Fact]
        public void BuildWithOverriddenVirtualPropertyCorrectlySetsProperty()
        {
            // Fixture setup
            var sut = new Fixture();
            var expected = Guid.NewGuid();
            // Exercise system
            var result = sut.Build<ConcreteType>().With(x => x.Property4, expected).Create();
            // Verify outcome
            Assert.Equal(expected, result.Property4);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousEnumerableWithoutCustomizationWorks()
        {
            var sut = new Fixture();
            var actual = sut.Create<IEnumerable<decimal>>();
            Assert.NotEmpty(actual);
        }

        [Fact]
        public void CreateAnonymousObservableCollectionWithoutCustomizationWorks()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.Create<ObservableCollection<decimal>>();
            // Verify outcome
            Assert.NotEmpty(result);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousEnumerableWhenEnumerableRelayIsPresentReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new Fixture();
            sut.ResidueCollectors.Add(new EnumerableRelay());
            // Exercise system
            var result = sut.Create<IEnumerable<decimal>>();
            // Verify outcome
            Assert.True(result.Any());
            // Teardown
        }

        [Fact]
        public void CreateAnonymousListDoesFillWithItemsPerDefault()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.Create<List<string>>();
            // Verify outcome
            Assert.True(result.Any());
            // Teardown
        }

        [Fact]
        public void CreateAnonymousListWithCustomizationsReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new Fixture();
            sut.Customizations.Add(new FilteringSpecimenBuilder(new MethodInvoker(new EnumerableFavoringConstructorQuery()), new ListSpecification()));
            sut.ResidueCollectors.Add(new EnumerableRelay());
            // Exercise system
            var result = sut.Create<List<string>>();
            // Verify outcome
            Assert.True(result.Any());
            // Teardown
        }

        [Fact]
        public void CreateAnonymousHashSetWithCustomizationsReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new Fixture();
            sut.Customizations.Add(new FilteringSpecimenBuilder(new MethodInvoker(new EnumerableFavoringConstructorQuery()), new HashSetSpecification()));
            sut.ResidueCollectors.Add(new EnumerableRelay());
            // Exercise system
            var result = sut.Create<HashSet<float>>();
            // Verify outcome
            Assert.True(result.Any());
            // Teardown
        }

        [Fact]
        public void CreateAnonymousIListWithCustomizationsReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new Fixture();
            sut.Customizations.Add(new FilteringSpecimenBuilder(new MethodInvoker(new EnumerableFavoringConstructorQuery()), new ListSpecification()));
            sut.ResidueCollectors.Add(new EnumerableRelay());
            sut.ResidueCollectors.Add(new ListRelay());
            // Exercise system
            var result = sut.Create<IList<int>>();
            // Verify outcome
            Assert.True(result.Any());
            // Teardown
        }

        [Fact]
        public void CreateAnonymousICollectionWithCustomizationsReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new Fixture();
            sut.Customizations.Add(new FilteringSpecimenBuilder(new MethodInvoker(new EnumerableFavoringConstructorQuery()), new ListSpecification()));
            sut.ResidueCollectors.Add(new EnumerableRelay());
            sut.ResidueCollectors.Add(new CollectionRelay());
            // Exercise system
            var result = sut.Create<ICollection<Version>>();
            // Verify outcome
            Assert.True(result.Any());
            // Teardown
        }

        [Fact]
        public void CreateAnonymousCollectionWithCustomizationsReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new Fixture();
            sut.Customizations.Add(new FilteringSpecimenBuilder(new MethodInvoker(new EnumerableFavoringConstructorQuery()), new ListSpecification()));
            sut.Customizations.Add(new FilteringSpecimenBuilder(new MethodInvoker(new ListFavoringConstructorQuery()), new CollectionSpecification()));
            sut.ResidueCollectors.Add(new EnumerableRelay());
            sut.ResidueCollectors.Add(new ListRelay());
            // Exercise system
            var result = sut.Create<Collection<ConcreteType>>();
            // Verify outcome
            Assert.True(result.Any());
            // Teardown
        }

        [Fact]
        public void CreateAnonymousDictionaryWithCustomizationsReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new Fixture();
            sut.Customizations.Add(new FilteringSpecimenBuilder(new Postprocessor(new MethodInvoker(new ModestConstructorQuery()), new DictionaryFiller()), new DictionarySpecification()));
            // Exercise system
            var result = sut.Create<Dictionary<int, string>>();
            // Verify outcome
            Assert.True(result.Any());
            // Teardown
        }

        [Fact]
        public void CreateAnonymousIDictionaryWithCustomizationsReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new Fixture();
            sut.Customizations.Add(new FilteringSpecimenBuilder(new Postprocessor(new MethodInvoker(new ModestConstructorQuery()), new DictionaryFiller()), new DictionarySpecification()));
            sut.ResidueCollectors.Add(new DictionaryRelay());
            // Exercise system
            var result = sut.Create<IDictionary<TimeSpan, Version>>();
            // Verify outcome
            Assert.True(result.Any());
            // Teardown
        }

        [Fact]
        public void CreateMultipleHoldersOfEnumsReturnsCorrectResultForLastItem()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.CreateMany<PropertyHolder<TriState>>(4);
            // Verify outcome
            Assert.InRange(result.Last().Property, TriState.First, TriState.Third);
            // Teardown
        }

        [Fact]
        public void CreateMultipleHoldersOfNullableEnumsReturnsCorrectResultForLastItem()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.CreateMany<PropertyHolder<TriState?>>(4);
            // Verify outcome
            Assert.InRange(result.Last().Property.GetValueOrDefault(), TriState.First, TriState.Third);
            // Teardown
        }

        [Fact]
        public void CreateMultipleSingleParameterTypesWithEnumReturnsCorrectResultForLastItem()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.CreateMany<PropertyHolder<SingleParameterType<TriState>>>(4);
            // Verify outcome
            Assert.InRange(result.Last().Property.Parameter, TriState.First, TriState.Third);
            // Teardown
        }

        // Supporting http://autofixture.codeplex.com/discussions/262288 Breaking this test might not be considered a breaking change
        [Fact]
        public void RefreezeHack()
        {
            // Fixture setup
            var fixture = new Fixture();
            var version2 = fixture.Create<Version>();
            var version1 = fixture.Freeze<Version>();
            // Exercise system
            fixture.Inject(version2);
            var actual = fixture.Create<Version>();
            // Verify outcome
            Assert.Equal(version2, actual);
            // Teardown
        }

        // Supporting http://autofixture.codeplex.com/discussions/262288 Breaking this test might not be considered a breaking change
        [Fact]
        public void UnfreezeHack()
        {
            // Fixture setup
            var fixture = new Fixture();
            var snapshot = fixture.Customizations.ToList();
            var anonymousVersion = fixture.Freeze<Version>();
            var freezer = fixture.Customizations.Except(snapshot).Single();
            // Exercise system
            fixture.Customizations.Remove(freezer);
            var expected = fixture.Freeze<Version>();
            var actual = fixture.Create<Version>();
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void UseGreedyConstructorQueryWithAutoProperties()
        {
            // Fixture setup
            var fixture = new Fixture();
            fixture.Customizations.Add(new Postprocessor(
                new MethodInvoker(new GreedyConstructorQuery()),
                new AutoPropertiesCommand(),
                new AnyTypeSpecification()));
            // Exercise system
            var result = fixture.Create<ConcreteType>();
            // Verify outcome
            Assert.NotNull(result.Property5);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousTypeReturnsInstance()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = fixture.Create<Type>();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void CreateHashSetReturnsCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = fixture.Create<HashSet<string>>();
            // Verify outcome
            Assert.NotEmpty(result);
            // Teardown
        }

        [Fact]
        public void CreateSortedSetReturnsCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = fixture.Create<SortedSet<string>>();
            // Verify outcome
            Assert.NotEmpty(result);
            // Teardown
        }

        [Fact]
        public void CreateSortedDictionaryReturnsCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = fixture.Create<SortedDictionary<string,object>>();
            // Verify outcome
            Assert.NotEmpty(result);
            // Teardown
        }

        [Fact]
        public void CreateSortedListReturnsCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = fixture.Create<SortedList<int,string>>();
            // Verify outcome
            Assert.NotEmpty(result);
            // Teardown
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void InjectDoesNotModifyAutoProperties(bool expected)
        {
            // Fixture setup
            var fixture = new Fixture();
            fixture.OmitAutoProperties = expected;
            // Exercise system
            fixture.Inject("dummy");
            // Verify outcome
            Assert.Equal(expected, fixture.OmitAutoProperties);
            // Teardown
        }

        [Fact]
        public void CustomizationsContainStableFiniteSequenceRelayByDefault()
        {
            var sut = new Fixture();
            Assert.True(
                sut.Customizations.OfType<StableFiniteSequenceRelay>().Any(),
                "Stable finite sequence relay not found.");
        }

        [Theory]
        [InlineData(typeof(EnumeratorRelay))]
        [InlineData(typeof(EnumerableRelay))]
        [InlineData(typeof(ListRelay))]
        [InlineData(typeof(CollectionRelay))]
        [InlineData(typeof(DictionaryRelay))]
        public void ResidueCollectorsContainMultipleRelaysByDefault(
            Type relayType)
        {
            var sut = new Fixture();
            Assert.True(
                sut.ResidueCollectors.Any(b =>
                    relayType.IsAssignableFrom(b.GetType())),
                relayType.Name + " not found.");
        }

        [Theory]
        [InlineData(typeof(ListSpecification), typeof(EnumerableFavoringConstructorQuery))]
        [InlineData(typeof(HashSetSpecification), typeof(EnumerableFavoringConstructorQuery))]
        [InlineData(typeof(CollectionSpecification), typeof(ListFavoringConstructorQuery))]
        [InlineData(typeof(ObservableCollectionSpecification), typeof(EnumerableFavoringConstructorQuery))]
        public void CustomizationsContainBuilderForProperConcreteMultipleTypeByDefault(
            Type specificationType,
            Type queryType)
        {
            var sut = new Fixture();
            Assert.True(sut.Customizations
                .OfType<FilteringSpecimenBuilder>()
                .Where(b => specificationType.IsAssignableFrom(b.Specification.GetType()))
                .Where(b => typeof(MethodInvoker).IsAssignableFrom(b.Builder.GetType()))
                .Select(b => (MethodInvoker)b.Builder)
                .Where(i => queryType.IsAssignableFrom(i.Query.GetType()))
                .Any());
        }

        [Fact]
        public void CustomizationsContainBuilderForConcreteDictionariesByDefault()
        {
            var sut = new Fixture();
            Assert.True(sut.Customizations
                .OfType<FilteringSpecimenBuilder>()
                .Where(b => typeof(DictionarySpecification).IsAssignableFrom(b.Specification.GetType()))
                .Where(b => typeof(Postprocessor).IsAssignableFrom(b.Builder.GetType()))
                .Select(b => (Postprocessor)b.Builder)
                .Where(p => p.Command is DictionaryFiller)
                .Where(p => typeof(MethodInvoker).IsAssignableFrom(p.Builder.GetType()))
                .Select(p => (MethodInvoker)p.Builder)
                .Where(i => typeof(ModestConstructorQuery).IsAssignableFrom(i.Query.GetType()))
                .Any());
        }

        [Fact]
        public void SutIsSequenceOfSpecimenBuilders()
        {
            var sut = new Fixture();
            Assert.IsAssignableFrom<IEnumerable<ISpecimenBuilder>>(sut);
        }

        [Fact]
        public void SutYieldsSomething()
        {
            var sut = new Fixture();

            Assert.True(sut.Any());
            Assert.NotEmpty(sut);
        }

        [Fact]
        public void CreateAnonymousStructWithConstructorReturnsInstance()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = fixture.Create<MutableValueType>();
            // Verify outcome
            Assert.NotNull(result.Property1);
            Assert.NotNull(result.Property2);
            Assert.NotNull(result.Property3);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousStructWithoutConstructorThrowsException()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system and verify outcome
            Assert.Throws<ObjectCreationException>(() => fixture.Create<MutableValueTypeWithoutConstructor>());
            // Teardown
        }

        /// <summary>
        /// This test is just to make sure that edge cases as decimal which is not primitive type and is a structure will not fall within 
        /// struct checking mechanism.
        /// </summary>
        [Fact]
        public void CreateDecimalDoesNotThrowException()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() => fixture.Create<decimal>()));
            // Teardown
        }

        [Fact]
        public void CreateAnonymousStructWithoutConstructorUsingCustomizationReturnsInstance()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = new SupportMutableValueTypesCustomization();
            sut.Customize(fixture);
            // Exercise system and verify outcome
            var result = fixture.Create<MutableValueTypeWithoutConstructor>();
            // Verify outcome
            Assert.NotNull(result.Property1);
            Assert.NotNull(result.Property2);
            // Teardown
        }

        [Fact]
        public void CustomizingEnumerableCustomizedCreateManyWhenCreateManyIsMappedToEnumerable()
        {
            // Fixture setup
            var fixture =
                new Fixture().Customize(new MapCreateManyToEnumerable());
            var expected = new[] { "a", "b", "c", "d" };
            fixture.Register<IEnumerable<string>>(() => expected);
            // Exercise system
            var actual = fixture.CreateMany<string>();
            // Verify outcome
            Assert.Equal(expected, actual.ToArray());
            // Teardown
        }

        [Fact]
        public void CreateAbstractTypeThreeTimesGivesTheSameErrorMessage()
        {
            // Fixture setup
            var fixture = new Fixture();

            // Exercise system and verify outcome
            var ocex1 = Assert.Throws<ObjectCreationException>(() =>
                fixture.Create<PropertyHolder<AbstractType>>());

            var ocex2 = Assert.Throws<ObjectCreationException>(() =>
                fixture.Create<PropertyHolder<AbstractType>>());

            var ocex3 = Assert.Throws<ObjectCreationException>(() =>
                fixture.Create<PropertyHolder<AbstractType>>());

            Assert.Equal(ocex1.Message, ocex2.Message);
            Assert.Equal(ocex2.Message, ocex3.Message);
            // Teardown
        }

        [Fact]
        public void CreateRecursiveTypeExceptionMessageIsStable()
        {
            // Fixture setup
            var fixture = new Fixture();

            // Exercise system and verify outcome
            var ocex1 = Assert.Throws<ObjectCreationException>(() =>
                fixture.Create<PropertyHolder<RecursionTestObjectWithReferenceOutA>>());

            var ocex2 = Assert.Throws<ObjectCreationException>(() =>
                fixture.Create<PropertyHolder<RecursionTestObjectWithReferenceOutA>>());

            var ocex3 = Assert.Throws<ObjectCreationException>(() =>
                fixture.Create<PropertyHolder<RecursionTestObjectWithReferenceOutA>>());

            Assert.Equal(ocex1.Message, ocex2.Message);
            Assert.Equal(ocex2.Message, ocex3.Message);
            // Teardown
        }

        [Fact]
        public void TraceOutputForRecursiveTypeCreationFailureIsStable()
        {
            // Fixture setup
            var fixture = new Fixture();
            var outputWriter = new StringWriter();
            fixture.Behaviors.Add(new TracingBehavior(outputWriter));

            // Exercise system and verify outcome
            Assert.Throws<ObjectCreationException>(() =>
                fixture.Create<PropertyHolder<RecursionTestObjectWithReferenceOutA>>());

            var traceOutput1 = outputWriter.ToString();
            outputWriter.GetStringBuilder().Clear();

            Assert.Throws<ObjectCreationException>(() =>
                fixture.Create<PropertyHolder<RecursionTestObjectWithReferenceOutA>>());

            var traceOutput2 = outputWriter.ToString();
            outputWriter.GetStringBuilder().Clear();

            Assert.Equal(traceOutput1, traceOutput2);
            // Teardown
        }

        [Fact]
        public void CreateSmallRecursiveSequenceGraph()
        {
            // Fixture setup
            var fixture = new Fixture();
            fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior(2));
            // Exercise system
            var actual = fixture.Create<RecursiveSequenceNode>();
            // Verify outcome
            Assert.NotEmpty(actual);
            Assert.True(actual.All(n => !n.Any()));
            // Teardown
        }

        [Fact]
        public void CreateSequenceOfSmallRecursiveSequenceGraphs()
        {
            // Fixture setup
            var fixture = new Fixture();
            fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));
            fixture.Customizations.Add(
                new OmitEnumerableParameterRequestRelay());
            // Exercise system
            var actual = fixture.Create<IEnumerable<RecursiveSequenceNode>>();
            // Verify outcome
            Assert.NotEmpty(actual);
            Assert.True(actual.All(n => !n.Any()));
            // Teardown
        }

        [Fact]
        public void CreateArrayOfSmallRecursiveSequenceGraphs()
        {
            // Fixture setup
            var fixture = new Fixture();
            fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));
            fixture.Customizations.Add(
                new OmitEnumerableParameterRequestRelay());
            // Exercise system
            var actual = fixture.Create<RecursiveSequenceNode[]>();
            // Verify outcome
            Assert.NotEmpty(actual);
            Assert.True(actual.All(n => !n.Any()));
            // Teardown
        }

        [Fact]
        public void CreateManySmallRecursiveSequenceGraphs()
        {
            // Fixture setup
            var fixture = new Fixture();
            fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));
            fixture.Customizations.Add(
                new OmitEnumerableParameterRequestRelay());
            // Exercise system
            var actual = fixture.CreateMany<RecursiveSequenceNode>();
            // Verify outcome
            Assert.NotEmpty(actual);
            Assert.True(actual.All(n => !n.Any()));
            // Teardown
        }

        private class RecursiveSequenceNode : IEnumerable<RecursiveSequenceNode>
        {
            private readonly IEnumerable<RecursiveSequenceNode> nodes;

            public RecursiveSequenceNode(IEnumerable<RecursiveSequenceNode> nodes)
            {
                this.nodes = nodes;
            }

            public IEnumerator<RecursiveSequenceNode> GetEnumerator()
            {
                return this.nodes.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        [Fact]
        public void CreateSmallRecursiveArrayGraph()
        {
            // Fixture setup
            var fixture = new Fixture();
            fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior(2));
            // Exercise system
            var actual = fixture.Create<RecursiveArrayNode>();
            // Verify outcome
            Assert.NotEmpty(actual);
            Assert.True(actual.All(n => !n.Any()));
            // Teardown
        }

        [Fact]
        public void CreateSequenceOfSmallRecursiveArrayGraphs()
        {
            // Fixture setup
            var fixture = new Fixture();
            fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));
            fixture.Customizations.Add(
                new OmitArrayParameterRequestRelay());
            // Exercise system
            var actual = fixture.Create<IEnumerable<RecursiveArrayNode>>();
            // Verify outcome
            Assert.NotEmpty(actual);
            Assert.True(actual.All(n => !n.Any()));
            // Teardown
        }

        [Fact]
        public void CreateArrayOfSmallRecursiveArrayGraphs()
        {
            // Fixture setup
            var fixture = new Fixture();
            fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));
            fixture.Customizations.Add(
                new OmitArrayParameterRequestRelay());
            // Exercise system
            var actual = fixture.Create<RecursiveArrayNode[]>();
            // Verify outcome
            Assert.NotEmpty(actual);
            Assert.True(actual.All(n => !n.Any()));
            // Teardown
        }

        [Fact]
        public void CreateManySmallRecursiveArrayGraphs()
        {
            // Fixture setup
            var fixture = new Fixture();
            fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));
            fixture.Customizations.Add(
                new OmitArrayParameterRequestRelay());
            // Exercise system
            var actual = fixture.CreateMany<RecursiveArrayNode>();
            // Verify outcome
            Assert.NotEmpty(actual);
            Assert.True(actual.All(n => !n.Any()));
            // Teardown
        }

        private class RecursiveArrayNode : IEnumerable<RecursiveArrayNode>
        {
            private readonly RecursiveArrayNode[] nodes;

            public RecursiveArrayNode(RecursiveArrayNode[] nodes)
            {
                this.nodes = nodes;
            }

            public IEnumerator<RecursiveArrayNode> GetEnumerator()
            {
                return this.nodes.AsEnumerable().GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        [Fact]
        public void CustomizationOfOverriddenPropOfChildADoesNotAffectChildB()
        {
            // Fixture setup
            var fixture = new Fixture();
            fixture.Customize<AcwaacpChildA>(c => c.Without(x => x.Value));
            // Exercise system
            var actual = fixture.Create<AcwaacpChildB>();
            // Verify outcome
            Assert.NotEqual(default(int), actual.Value);
            // Teardown
        }
        
        /// <summary>
        /// Checks the scenario: https://github.com/AutoFixture/AutoFixture/issues/531
        /// </summary>
        [Fact]
        public void CustomizationOfBasePropOfChildADoesNotAffectChildB()
        {
            // arrange
            var sut = new Fixture();
            sut.Customize<AcwaacpChildA>(c => c.With(x => x.Text, "foo"));
            
            // act
            var actual = sut.Create<AcwaacpChildB>();
            
            // assert
            Assert.NotNull(actual.Text);
            Assert.NotEqual("foo", actual.Text);
        }

        /// <summary>
        /// Checks the scenario reported in https://github.com/AutoFixture/AutoFixture/issues/772
        /// </summary>
        [Fact]
        public void CustomizatonOfSamePropertyIsIgnoredDuringTheBuild()
        {
            //arrange
            var sut = new Fixture();
            sut.Customize<DoublePropertyHolder<string, int>>(c => c.With(x => x.Property1, "foo"));
            
            //act
            var result = sut
                .Build<DoublePropertyHolder<string, int>>()
                .With(x => x.Property2, 42)
                .Create();
            
            //assert
            Assert.NotEqual("foo", result.Property1);
            Assert.Equal(42, result.Property2);
        }

        /// <summary>
        /// Scenario from https://github.com/AutoFixture/AutoFixture/issues/321 
        /// </summary>
        [Fact]
        public void CustomizationOfIntPropertyDoesntThrowInBuild()
        {
            //arrange
            var sut = new Fixture();
            sut.Customize<PropertyHolder<long>>(c => c.Without(x => x.Property));
            
            //act
            var result = sut.Build<PropertyHolder<long>>().With(x => x.Property).Create();
            
            //assert
            Assert.NotEqual(0L, result.Property);
        }

        [Fact]
        public void EnableDisableAutoPropertiesDoesntBreakCustomization()
        {
            //arrange
            var sut = new Fixture();
            sut.Customize<PropertyHolder<string>>(c =>
                c
                    .Without(x => x.Property)
                    .OmitAutoProperties()
                    .WithAutoProperties());
            
            //act
            var result = sut.Create<PropertyHolder<string>>();
            
            //assert
            Assert.Null(result.Property);
        }

        private abstract class AbstractClassWithAbstractAndConcreteProperties
        {
            public string Text { get; set; }

            public abstract int Value { get; set; }
        }

        private class AcwaacpChildA : AbstractClassWithAbstractAndConcreteProperties
        {
            public override int Value { get; set; }
        }

        private class AcwaacpChildB : AbstractClassWithAbstractAndConcreteProperties
        {
            public override int Value { get; set; }
        }

        private class RecursionTestObjectWithReferenceOutA
        {
            public RecursionTestObjectWithReferenceOutB ReferenceToB
            {
                get;
                set;
            }
        }

        private class RecursionTestObjectWithReferenceOutB
        {
            public RecursionTestObjectWithReferenceOutA ReferenceToA
            {
                get;
                set;
            }
        }

        private class RecursionTestObjectWithConstructorReferenceOutA
        {
            public RecursionTestObjectWithConstructorReferenceOutB ReferenceToB
            {
                get;
                private set;
            }

            public RecursionTestObjectWithConstructorReferenceOutA(RecursionTestObjectWithConstructorReferenceOutB b)
            {
                this.ReferenceToB = b;
            }
        }

        private class RecursionTestObjectWithConstructorReferenceOutB
        {
            public RecursionTestObjectWithConstructorReferenceOutA ReferenceToA
            {
                get;
                private set;
            }

            public RecursionTestObjectWithConstructorReferenceOutB(RecursionTestObjectWithConstructorReferenceOutA a)
            {
                this.ReferenceToA = a;
            }
        }

        public static IEnumerable<object[]> CreateComplexArrayTypeReturnsCorrectResult_Data =>
            new[]
                {
                    typeof(string[][,,][]),
                    typeof(object[,,][][,])
                }
                .Select(x => new object[] {x});

        [Theory, MemberData(nameof(CreateComplexArrayTypeReturnsCorrectResult_Data))]
        public void CreateComplexArrayTypeReturnsCorrectResult(object request)
        {
            var sut = new Fixture();
            var context = new SpecimenContext(sut);
            Assert.NotNull(context.Resolve(request));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(10)]
        public void CreateComplexArrayTypeReturnsArrayReflectingCorrectRepeatCount(int repeatCount)
        {
            var sut = new Fixture { RepeatCount = repeatCount };

            var actual = sut.Create<int[,][]>();

            Assert.Equal(repeatCount, actual.GetLength(0));
            Assert.Equal(repeatCount, actual.GetLength(1));
            Assert.Equal(repeatCount, actual[0, 0].Length);
        }

        [Fact]
        public void CreateNonGenericTaskReturnsAwaitableTask()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            // Exercise system
            Task result = sut.Create<Task>();
            // Verify outcome
            Thread thread = new Thread(result.Wait);
            thread.Start();
            bool ranToCompletion = thread.Join(1000);

            Assert.True(ranToCompletion);
            // Teardown
        }

        [Fact]
        public void CreateGenericTaskReturnsAwaitableTask()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            // Exercise system
            Task<int> result = sut.Create<Task<int>>();
            // Verify outcome
            Thread thread = new Thread(result.Wait);
            thread.Start();
            bool ranToCompletion = thread.Join(1000);

            Assert.True(ranToCompletion);
            // Teardown
        }

        [Fact]
        public void CreateGenericTaskReturnsTaskWhoseResultWasResolvedByFixture()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            int frozenInt = sut.Freeze<int>();
            // Exercise system
            Task<int> result = sut.Create<Task<int>>();
            // Verify outcome
            Assert.Equal(frozenInt, result.Result);
            // Teardown
        }

        [Fact]
        public void CreateLazyInitializedTypeReturnsCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = fixture.Create<Lazy<string>>();
            var actual = result.Value;
            // Verify outcome
            Assert.NotNull(actual);
            // Teardown
        }

        [Fact]
        public void CreatesEnumerator()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system and verify outcome
            IEnumerator<int> result = null;
            Assert.Null(Record.Exception(() => result = fixture.Create<IEnumerator<int>>()));
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void CreatesEnumeratorWithSpecifiedNumberOfItems()
        {
            // Fixture setup
            var fixture = new Fixture();
            var expectedCount = fixture.RepeatCount;
            // Exercise system
            var result = fixture.Create<IEnumerator<int>>();
            // Verify outcome
            int count = 0;
            while (result.MoveNext())
            {
                count++;
                Assert.True(count <= expectedCount);
            }
            Assert.Equal(expectedCount, count);
            // Teardown
        }

        [Fact]
        public void FixtureCanCreateCultureInfo()
        {
            var fixture = new Fixture();
            var actual = fixture.Create<System.Globalization.CultureInfo>();
            Assert.NotNull(actual);
        }

        [Fact]
        public void FixtureCanCreateEncoding()
        {
            var fixture = new Fixture();
            var actual = fixture.Create<System.Text.Encoding>();
            Assert.NotNull(actual);
        }

        [Fact]
        public void FixtureCanCreateIPAddress()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var actual = fixture.Create<System.Net.IPAddress>();
            // Verify outcome
            Assert.NotNull(actual);
            // Teardown 
        }

        [Fact]
        public void ReturningNullFromFactoryIsPossible()
        {
            var fixture = new Fixture();
            fixture.Customize<string>(x => x.FromFactory(() => null));

            Assert.Null(Record.Exception(() => fixture.Create<string>()));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(10)]
        public void CustomizeBytePropertyReturnsCorrectResult(int expected)
        {
            var fixture = new Fixture();
            fixture.Customize<PropertyHolder<byte>>(c => c
                .With(x => x.Property, expected));

            var actual = fixture.Create<PropertyHolder<byte>>();

            Assert.Equal(expected, actual.Property);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(10)]
        public void CustomizeByteFieldReturnsCorrectResult(int expected)
        {
            var fixture = new Fixture();
            fixture.Customize<FieldHolder<byte>>(c => c
                .With(x => x.Field, expected));

            var actual = fixture.Create<FieldHolder<byte>>();

            Assert.Equal(expected, actual.Field);
        }

        [Fact]
        public void WithImplicitConversionToNullablePropertyReturnsCorrectResult()
        {
            var fixture = new Fixture();
            var expected = fixture.Create<int>();

            var actual = fixture.Build<PropertyHolder<int?>>()
                .With(x => x.Property, expected)
                .Create();

            Assert.Equal(expected, actual.Property);
        }

        [Fact]
        public void WithImplicitConversionToNullableFieldReturnsCorrectResult()
        {
            var fixture = new Fixture();
            var expected = fixture.Create<int>();

            var actual = fixture.Build<FieldHolder<int?>>()
                .With(x => x.Field, expected)
                .Create();

            Assert.Equal(expected, actual.Field);
        }


        /// <summary>
        /// This test reproduces the issue as reported in pull request:
        /// https://github.com/AutoFixture/AutoFixture/pull/604
        /// </summary>
        [Fact]
        public void WithoutOnFieldInBaseClassThrowsNullPointerException()
        {
            Fixture f = new Fixture();
            f.Build<ConcreteType>().Without(x => x.Field1).Create();

            /*
                Success when no ArgumentNullException is thrown:

                When reported, the call to Without on Field1 residing 
                in ConcreteType's base class (AbstractType) will cause 
                a null pointer exception to bubble. 
            */
        }

        [Fact]
        public void UseActionExpression()
        {
            var fixture = new Fixture();
            var actual = fixture.Create<Expression<Action<string, int>>>();
            Assert.Null(Record.Exception(() => actual.Compile()("foo", 42)));
        }

        [Fact]
        public void CreateFuncExpression()
        {
            var fixture = new Fixture();
            var actual = fixture.Create<Expression<Func<object>>>();
            Assert.NotNull(actual.Compile()());
        }

#if SYSTEM_NET_MAIL
        [Fact]
        public void CreateAnonymousWithMailAddressReturnsValidResult()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var mailAddress = fixture.Create<System.Net.Mail.MailAddress>();
            // Verify outcome
            Assert.True(mailAddress != null);
            // Teardown
        }
#endif
    }
}