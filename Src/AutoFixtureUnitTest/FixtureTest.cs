using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.DataAnnotations;
using AutoFixture.Dsl;
using AutoFixture.Kernel;
using AutoFixtureUnitTest.DataAnnotations;
using AutoFixtureUnitTest.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class FixtureTest
    {
        [Fact]
        public void InitializedWithDefaultConstructorSutHasCorrectEngineParts()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            var result = sut.Engine;
            // Assert
            var expectedParts = from b in new DefaultEngineParts()
                                select b.GetType();
            var composite = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(result);
            Assert.True(expectedParts.SequenceEqual(from b in composite
                                                    select b.GetType()));
        }

        [Fact]
        public void InitializeWithNullRelaysThrows()
        {
            // Arrange
            // Act & assert
            var ex = Assert.Throws<ArgumentNullException>(() =>
                new Fixture(null));
            Assert.Equal("engineParts", ex.ParamName);
        }

        [Fact]
        public void InitializedWithRelaysSutHasCorrectEngineParts()
        {
            // Arrange
            var relays = new DefaultRelays();
            var sut = new Fixture(relays);
            // Act
            var result = sut.Engine;
            // Assert
            var expectedParts = from b in relays
                                select b.GetType();
            var composite = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(result);
            Assert.True(expectedParts.SequenceEqual(from b in composite
                                                    select b.GetType()));
        }

        [Fact]
        public void InitializeWithNullEngineThrows()
        {
            // Arrange
            var dummyMany = new MultipleRelay();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new Fixture(null, dummyMany));
        }

        [Fact]
        public void InitializeWithNullManyThrows()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new Fixture(dummyBuilder, null));
        }

        [Fact]
        public void InitializedWithEngineSutHasCorrectEngine()
        {
            // Arrange
            var expectedEngine = new DelegatingSpecimenBuilder();
            var dummyMany = new MultipleRelay();
            var sut = new Fixture(expectedEngine, dummyMany);
            // Act
            var result = sut.Engine;
            // Assert
            Assert.Equal(expectedEngine, result);
        }

        [Fact]
        public void InitializedWithManySutHasCorrectRepeatCount()
        {
            // Arrange
            var expectedRepeatCount = 187;
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var many = new MultipleRelay { Count = expectedRepeatCount };
            var sut = new Fixture(dummyBuilder, many);
            // Act
            var result = sut.RepeatCount;
            // Assert
            Assert.Equal(expectedRepeatCount, result);
        }

        [Fact]
        public void SettingRepeatCountWillCorrectlyUpdateMany()
        {
            // Arrange
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var many = new MultipleRelay();
            var sut = new Fixture(dummyBuilder, many);
            // Act
            sut.RepeatCount = 26;
            // Assert
            Assert.Equal(sut.RepeatCount, many.Count);
        }

        [Fact]
        public void CustomizationsIsInstance()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            IList<ISpecimenBuilder> result = sut.Customizations;
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void CustomizationsIsStable()
        {
            // Arrange
            var sut = new Fixture();
            var builder = new DelegatingSpecimenBuilder();
            // Act
            sut.Customizations.Add(builder);
            // Assert
            Assert.Contains(builder, sut.Customizations);
        }

        [Fact]
        public void ResidueCollectorsIsInstance()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            IList<ISpecimenBuilder> result = sut.ResidueCollectors;
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void ResidueCollectorsIsStable()
        {
            // Arrange
            var sut = new Fixture();
            var builder = new DelegatingSpecimenBuilder();
            // Act
            sut.ResidueCollectors.Add(builder);
            // Assert
            Assert.Contains(builder, sut.ResidueCollectors);
        }

        [Fact]
        public void BehaviorsIsInstance()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            IList<ISpecimenBuilderTransformation> result = sut.Behaviors;
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void BehaviorsIsStable()
        {
            // Arrange
            var sut = new Fixture();
            var behavior = new DelegatingSpecimenBuilderTransformation();
            // Act
            sut.Behaviors.Add(behavior);
            // Assert
            Assert.Contains(behavior, sut.Behaviors);
        }

        [Fact]
        public void BehaviorsContainsCorrectRecursionBehavior()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            var result = sut.Behaviors;
            // Assert
            Assert.True(result.OfType<ThrowingRecursionBehavior>().Any());
        }

        [Fact]
        public void SutIsCustomizableComposer()
        {
            // Arrange
            // Act
            var sut = new Fixture();
            // Assert
            Assert.IsAssignableFrom<IFixture>(sut);
        }

        [Fact]
        public void CreateAnonymousWillCreateSimpleObject()
        {
            // Arrange
            Fixture sut = new Fixture();
            // Act
            object result = sut.Create<object>();
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        [Obsolete]
        public void CreateAnonymousCompatibilityExtensionWillCreateSimpleObject()
        {
            // Arrange
            Fixture sut = new Fixture();
            // Act
            object result = sut.CreateAnonymous<object>();
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void CreateUnregisteredAbstractTypeWillThrow()
        {
            // Arrange
            Fixture sut = new Fixture();
            // Act & assert
            Assert.ThrowsAny<ObjectCreationException>(() =>
                sut.Create<AbstractType>());
        }

        [Fact]
        public void CreateAnonymousWillCreateSingleParameterType()
        {
            // Arrange
            Fixture sut = new Fixture();
            // Act
            SingleParameterType<object> result = sut.Create<SingleParameterType<object>>();
            // Assert
            Assert.NotNull(result.Parameter);
        }

        [Fact]
        public void CreateAnonymousWillUseRegisteredMapping()
        {
            // Arrange
            Fixture sut = new Fixture();
            sut.Register<AbstractType>(() => new ConcreteType());
            // Act
            SingleParameterType<AbstractType> result = sut.Create<SingleParameterType<AbstractType>>();
            // Assert
            Assert.IsAssignableFrom<ConcreteType>(result.Parameter);
        }

        [Fact]
        public void CreateOnMultipleThreadsConcurrentlyGeneratesPopulatedSpecimens()
        {
            // Arrange
            const int specimenCountPerThread = 25;
            const int threadCount = 8;
            var sut = new Fixture();

            // Act
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

            // Assert
            Assert.Equal(specimenCountPerThread * threadCount, specimensByThread.Sum(t => t.Length));

            var allValuesNotPopulated = specimensByThread
                .SelectMany(t => t.SelectMany(s => s.ValuesNotPopulated));

            Assert.Empty(allValuesNotPopulated);
        }

        [Fact]
        public void CreateAnonymousWillUseRegisteredMappingWithSingleParameter()
        {
            // Arrange
            Fixture sut = new Fixture();
            sut.Register<object, AbstractType>(obj => new ConcreteType(obj));
            // Act
            AbstractType result = sut.Create<AbstractType>();
            // Assert
            Assert.NotNull(result.Property1);
        }

        [Fact]
        public void CreateAnonymousWillUseRegisteredMappingWithDoubleParameters()
        {
            // Arrange
            Fixture sut = new Fixture();
            sut.Register<object, object, AbstractType>((obj1, obj2) => new ConcreteType(obj1, obj2));
            // Act
            AbstractType result = sut.Create<AbstractType>();
            // Assert
            Assert.NotNull(result.Property1);
            Assert.NotNull(result.Property2);
        }

        [Fact]
        public void CreateAnonymousWillUseRegisteredMappingWithTripleParameters()
        {
            // Arrange
            Fixture sut = new Fixture();
            sut.Register<object, object, object, AbstractType>((obj1, obj2, obj3) => new ConcreteType(obj1, obj2, obj3));
            // Act
            AbstractType result = sut.Create<AbstractType>();
            // Assert
            Assert.NotNull(result.Property1);
            Assert.NotNull(result.Property2);
            Assert.NotNull(result.Property3);
        }

        [Fact]
        public void CreateAnonymousWillUseRegisteredMappingWithQuadrupleParameters()
        {
            // Arrange
            Fixture sut = new Fixture();
            sut.Register<object, object, object, object, AbstractType>((obj1, obj2, obj3, obj4) => new ConcreteType(obj1, obj2, obj3, obj4));
            // Act
            AbstractType result = sut.Create<AbstractType>();
            // Assert
            Assert.NotNull(result.Property1);
            Assert.NotNull(result.Property2);
            Assert.NotNull(result.Property3);
            Assert.NotNull(result.Property4);
        }

        [Fact]
        public void CustomizeInstanceWillReturnFactory()
        {
            // Arrange
            Fixture sut = new Fixture();
            // Act
            var result = sut.Build<object>();
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void CreateAnonymousWithStringPropertyWillAssignNonEmptyString()
        {
            // Arrange
            Fixture sut = new Fixture();
            // Act
            PropertyHolder<string> result = sut.Create<PropertyHolder<string>>();
            // Assert
            Assert.False(string.IsNullOrEmpty(result.Property), "Property should be assigned");
        }

        [Fact]
        public void CreateAnonymousWithStringPropertyWillAppendPropertyNameToString()
        {
            // Arrange
            string expectedName = "Property";
            Fixture sut = new Fixture();
            // Act
            PropertyHolder<string> result = sut.Create<PropertyHolder<string>>();
            // Assert
            string propertyValue = result.Property;
            string text = new TextGuidRegex().GetText(propertyValue);
            Assert.Equal(expectedName, text);
        }

        [Fact]
        public void CreateAnonymousWithStringPropertyTwiceWillAssignDifferentValues()
        {
            // Arrange
            Fixture sut = new Fixture();
            PropertyHolder<string> ph = sut.Create<PropertyHolder<string>>();
            // Act
            PropertyHolder<string> result = sut.Create<PropertyHolder<string>>();
            // Assert
            Assert.NotEqual<string>(ph.Property, result.Property);
        }

        [Fact]
        public void CreateAnonymousWithBooleanPropertyWillAssignTrue()
        {
            // Arrange
            bool unexpectedBoolean = default(bool);
            Fixture sut = new Fixture();
            // Act
            PropertyHolder<bool> result = sut.Create<PropertyHolder<bool>>();
            // Assert
            Assert.NotEqual<bool>(unexpectedBoolean, result.Property);
        }

        [Fact]
        public void CreateAnonymousWithCharPropertyTwiceWillAssignDifferentValues()
        {
            // Arrange
            Fixture sut = new Fixture();
            PropertyHolder<char> ph = sut.Create<PropertyHolder<char>>();
            // Act
            PropertyHolder<char> result = sut.Create<PropertyHolder<char>>();
            // Assert
            Assert.NotEqual<char>(ph.Property, result.Property);
        }

        [Fact]
        public void CreateAnonymousWithBooleanPropertyTwiceWillAssignDifferentValues()
        {
            // Arrange
            Fixture sut = new Fixture();
            PropertyHolder<bool> ph = sut.Create<PropertyHolder<bool>>();
            // Act
            PropertyHolder<bool> result = sut.Create<PropertyHolder<bool>>();
            // Assert
            Assert.NotEqual<bool>(ph.Property, result.Property);
        }

        [Fact]
        public void CreateAnonymousWithBytePropertyWillAssignNonDefaultValue()
        {
            // Arrange
            byte unexpectedByte = default(byte);
            Fixture sut = new Fixture();
            // Act
            PropertyHolder<byte> result = sut.Create<PropertyHolder<byte>>();
            // Assert
            Assert.NotEqual<byte>(unexpectedByte, result.Property);
        }

        [Fact]
        public void CreateAnonymousWithBytePropertyTwiceWillAssignDifferentValues()
        {
            // Arrange
            Fixture sut = new Fixture();
            PropertyHolder<byte> ph = sut.Create<PropertyHolder<byte>>();
            // Act
            PropertyHolder<byte> result = sut.Create<PropertyHolder<byte>>();
            // Assert
            Assert.NotEqual<byte>(ph.Property, result.Property);
        }

        [Fact]
        public void CreateAnonymousWithSignedBytePropertyWillAssignNonDefaultValue()
        {
            // Arrange
            sbyte unexpectedSbyte = default(sbyte);
            Fixture sut = new Fixture();
            // Act
            PropertyHolder<sbyte> result = sut.Create<PropertyHolder<sbyte>>();
            // Assert
            Assert.NotEqual<sbyte>(unexpectedSbyte, result.Property);
        }

        [Fact]
        public void CreateAnonymousWithSignedBytePropertyTwiceWillAssignDifferentValues()
        {
            // Arrange
            Fixture sut = new Fixture();
            PropertyHolder<sbyte> ph = sut.Create<PropertyHolder<sbyte>>();
            // Act
            PropertyHolder<sbyte> result = sut.Create<PropertyHolder<sbyte>>();
            // Assert
            Assert.NotEqual<sbyte>(ph.Property, result.Property);
        }

        [Fact]
        public void CreateAnonymousWithUnsignedInt16PropertyWillAssignNonDefaultValue()
        {
            // Arrange
            ushort unexpectedNumber = default(ushort);
            Fixture sut = new Fixture();
            // Act
            PropertyHolder<ushort> result = sut.Create<PropertyHolder<ushort>>();
            // Assert
            Assert.NotEqual<ushort>(unexpectedNumber, result.Property);
        }

        [Fact]
        public void CreateAnonymousWithUnsignedInt16PropertyTwiceWillAssignDifferentValues()
        {
            // Arrange
            Fixture sut = new Fixture();
            PropertyHolder<ushort> ph = sut.Create<PropertyHolder<ushort>>();
            // Act
            PropertyHolder<ushort> result = sut.Create<PropertyHolder<ushort>>();
            // Assert
            Assert.NotEqual<ushort>(ph.Property, result.Property);
        }

        [Fact]
        public void CreateAnonymousWithInt16PropertyWillAssignNonDefaultValue()
        {
            // Arrange
            short unexpectedNumber = default(short);
            Fixture sut = new Fixture();
            // Act
            PropertyHolder<short> result = sut.Create<PropertyHolder<short>>();
            // Assert
            Assert.NotEqual<short>(unexpectedNumber, result.Property);
        }

        [Fact]
        public void CreateAnonymousWithInt16PropertyTwiceWillAssignDifferentValues()
        {
            // Arrange
            Fixture sut = new Fixture();
            PropertyHolder<short> ph = sut.Create<PropertyHolder<short>>();
            // Act
            PropertyHolder<short> result = sut.Create<PropertyHolder<short>>();
            // Assert
            Assert.NotEqual<short>(ph.Property, result.Property);
        }

        [Fact]
        public void CreateAnonymousWithUnsignedInt32PropertyWillAssignNonDefaultValue()
        {
            // Arrange
            uint unexpectedNumber = default(uint);
            Fixture sut = new Fixture();
            // Act
            PropertyHolder<uint> result = sut.Create<PropertyHolder<uint>>();
            // Assert
            Assert.NotEqual<uint>(unexpectedNumber, result.Property);
        }

        [Fact]
        public void CreateAnonymousWithUnsignedInt32PropertyTwiceWillAssignDifferentValues()
        {
            // Arrange
            Fixture sut = new Fixture();
            PropertyHolder<uint> ph = sut.Create<PropertyHolder<uint>>();
            // Act
            PropertyHolder<uint> result = sut.Create<PropertyHolder<uint>>();
            // Assert
            Assert.NotEqual<uint>(ph.Property, result.Property);
        }

        [Fact]
        public void CreateAnonymousWithInt32PropertyWillAssignNonDefaultValue()
        {
            // Arrange
            int unexpectedNumber = default(int);
            Fixture sut = new Fixture();
            // Act
            PropertyHolder<int> result = sut.Create<PropertyHolder<int>>();
            // Assert
            Assert.NotEqual<int>(unexpectedNumber, result.Property);
        }

        [Fact]
        public void CreateAnonymousWithInt32PropertyTwiceWillAssignDifferentValues()
        {
            // Arrange
            Fixture sut = new Fixture();
            PropertyHolder<int> ph = sut.Create<PropertyHolder<int>>();
            // Act
            PropertyHolder<int> result = sut.Create<PropertyHolder<int>>();
            // Assert
            Assert.NotEqual<int>(ph.Property, result.Property);
        }

        [Fact]
        public void CreateAnonymousWithUnsignedInt64PropertyWillAssignNonDefaultValue()
        {
            // Arrange
            ulong unexpectedNumber = default(ulong);
            Fixture sut = new Fixture();
            // Act
            PropertyHolder<ulong> result = sut.Create<PropertyHolder<ulong>>();
            // Assert
            Assert.NotEqual<ulong>(unexpectedNumber, result.Property);
        }

        [Fact]
        public void CreateAnonymousWithUnsignedInt64PropertyTwiceWillAssignDifferentValues()
        {
            // Arrange
            Fixture sut = new Fixture();
            PropertyHolder<ulong> ph = sut.Create<PropertyHolder<ulong>>();
            // Act
            PropertyHolder<ulong> result = sut.Create<PropertyHolder<ulong>>();
            // Assert
            Assert.NotEqual<ulong>(ph.Property, result.Property);
        }

        [Fact]
        public void CreateAnonymousWithInt64PropertyWillAssignNonDefaultValue()
        {
            // Arrange
            long unexpectedNumber = default(long);
            Fixture sut = new Fixture();
            // Act
            PropertyHolder<long> result = sut.Create<PropertyHolder<long>>();
            // Assert
            Assert.NotEqual<long>(unexpectedNumber, result.Property);
        }

        [Fact]
        public void CreateAnonymousWithInt64PropertyTwiceWillAssignDifferentValues()
        {
            // Arrange
            Fixture sut = new Fixture();
            PropertyHolder<long> ph = sut.Create<PropertyHolder<long>>();
            // Act
            PropertyHolder<long> result = sut.Create<PropertyHolder<long>>();
            // Assert
            Assert.NotEqual<long>(ph.Property, result.Property);
        }

        [Fact]
        public void CreateAnonymousWithDecimalPropertyWillAssignNonDefaultValue()
        {
            // Arrange
            decimal unexpectedNumber = default(decimal);
            Fixture sut = new Fixture();
            // Act
            PropertyHolder<decimal> result = sut.Create<PropertyHolder<decimal>>();
            // Assert
            Assert.NotEqual<decimal>(unexpectedNumber, result.Property);
        }

        [Fact]
        public void CreateAnonymousWithDecimalPropertyTwiceWillAssignDifferentValues()
        {
            // Arrange
            Fixture sut = new Fixture();
            PropertyHolder<decimal> ph = sut.Create<PropertyHolder<decimal>>();
            // Act
            PropertyHolder<decimal> result = sut.Create<PropertyHolder<decimal>>();
            // Assert
            Assert.NotEqual<decimal>(ph.Property, result.Property);
        }

        [Fact]
        public void CreateAnonymousWithSinglePropertyWillAssignNonDefaultValue()
        {
            // Arrange
            float unexpectedNumber = default(float);
            Fixture sut = new Fixture();
            // Act
            PropertyHolder<float> result = sut.Create<PropertyHolder<float>>();
            // Assert
            Assert.NotEqual<float>(unexpectedNumber, result.Property);
        }

        [Fact]
        public void CreateAnonymousWithSinglePropertyTwiceWillAssignDifferentValues()
        {
            // Arrange
            Fixture sut = new Fixture();
            PropertyHolder<float> ph = sut.Create<PropertyHolder<float>>();
            // Act
            PropertyHolder<float> result = sut.Create<PropertyHolder<float>>();
            // Assert
            Assert.NotEqual<float>(ph.Property, result.Property);
        }

        [Fact]
        public void CreateAnonymousWithDoublePropertyWillAssignNonDefaultValue()
        {
            // Arrange
            double unexpectedNumber = default(double);
            Fixture sut = new Fixture();
            // Act
            PropertyHolder<double> result = sut.Create<PropertyHolder<double>>();
            // Assert
            Assert.NotEqual<double>(unexpectedNumber, result.Property);
        }

        [Fact]
        public void CreateAnonymousWithDoublePropertyTwiceWillAssignDifferentValues()
        {
            // Arrange
            Fixture sut = new Fixture();
            PropertyHolder<double> ph = sut.Create<PropertyHolder<double>>();
            // Act
            PropertyHolder<double> result = sut.Create<PropertyHolder<double>>();
            // Assert
            Assert.NotEqual<double>(ph.Property, result.Property);
        }

        [Fact]
        public void CreateAnonymousWithDoubleMixedWholeNumericPropertiesWillAssignDifferentValues()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            var result = sut.Create<DoublePropertyHolder<int, long>>();
            // Assert
            Assert.NotEqual(result.Property1, result.Property2);
        }

        [Fact]
        public void CreateAnonymousWithDoubleMixedSmallWholeNumericPropertiesWillAssignDifferentValues()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            var result = sut.Create<DoublePropertyHolder<byte, short>>();
            // Assert
            Assert.NotEqual(result.Property1, result.Property2);
        }

        [Fact]
        public void CreateAnonymousWithDoubleMixedFloatingPointNumericPropertiesWillAssignDifferentValues()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            var result = sut.Create<DoublePropertyHolder<double, float>>();
            // Assert
            Assert.NotEqual(result.Property1, result.Property2);
        }

        [Fact]
        public void CreateAnonymousWithDoubleMixedNumericPropertiesWillAssignDifferentValues()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            var result = sut.Create<DoublePropertyHolder<long, float>>();
            // Assert
            Assert.NotEqual(result.Property1, result.Property2);
        }

        [Fact]
        public void CreateAnonymousWithNumericSequenceCustomizationAndDoubleMixedWholeNumericPropertiesWillAssignSameValue()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            sut.Customize(new NumericSequencePerTypeCustomization());
            var result = sut.Create<DoublePropertyHolder<int, long>>();
            // Assert
            Assert.Equal(result.Property1, result.Property2);
        }

        [Fact]
        public void CreateAnonymousWithNumericSequenceCustomizationAndDoubleMixedSmallWholeNumericPropertiesWillAssignSameValue()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            sut.Customize(new NumericSequencePerTypeCustomization());
            var result = sut.Create<DoublePropertyHolder<byte, short>>();
            // Assert
            Assert.Equal(result.Property1, result.Property2);
        }

        [Fact]
        public void CreateAnonymousWithNumericSequenceCustomizationAndDoubleMixedFloatingPointNumericPropertiesWillAssignSameValue()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            sut.Customize(new NumericSequencePerTypeCustomization());
            var result = sut.Create<DoublePropertyHolder<double, float>>();
            // Assert
            Assert.Equal(result.Property1, result.Property2);
        }

        [Fact]
        public void CreateAnonymousWithNumericSequenceCustomizationAndDoubleMixedNumericPropertiesWillAssignSameValue()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            sut.Customize(new NumericSequencePerTypeCustomization());
            var result = sut.Create<DoublePropertyHolder<long, float>>();
            // Assert
            Assert.Equal(result.Property1, result.Property2);
        }

        [Fact]
        public void CreateAnonymousWithRandomNumericSequenceCustomizationReturnsRandomNumbers()
        {
            // Arrange
            var sut = new Fixture();
            sut.Customizations.Add(new RandomNumericSequenceGenerator(15, 30));
            var definedNumbers = new object[]
            {
                1,
                2U,
                (byte)3,
                (sbyte)4,
                5L,
                6UL,
                (short)7,
                (ushort)8,
                9.0F,
                10.0D,
                11M
            };
            // Act
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
            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void InjectCustomUpperLimitWillCauseSutToReturnNumbersInLimit()
        {
            // Arrange
            int lower = -9;
            int upper = -1;
            var sut = new Fixture();
            sut.Customizations.Add(new RandomNumericSequenceGenerator(lower, upper));
            // Act
            var result = sut.Create<DoublePropertyHolder<int, long>>();
            // Assert
            Assert.True(
                (result.Property1 >= lower && result.Property1 <= upper) &&
                (result.Property2 >= lower && result.Property2 <= upper));
        }

        [Fact]
        public void CreateAnonymousWithGuidPropertyWillAssignNonDefaultValue()
        {
            // Arrange
            Guid unexpectedGuid = default(Guid);
            var sut = new Fixture();
            // Act
            var result = sut.Create<PropertyHolder<Guid>>();
            // Assert
            Assert.NotEqual<Guid>(unexpectedGuid, result.Property);
        }

        [Fact]
        public void CreateAnonymousWithGuidPropertyTwiceWillAssignDifferentValues()
        {
            // Arrange
            var sut = new Fixture();
            var ph = sut.Create<PropertyHolder<Guid>>();
            // Act
            var result = sut.Create<PropertyHolder<Guid>>();
            // Assert
            Assert.NotEqual<Guid>(ph.Property, result.Property);
        }

        [Fact]
        public void CreateAnonymousWithFlagEnumPropertyTwiceWillAssignDifferentValues()
        {
            // Arrange
            var sut = new Fixture();
            var ph = sut.Create<PropertyHolder<ActivityScope>>();
            // Act
            var result = sut.Create<PropertyHolder<ActivityScope>>();
            // Assert
            Assert.NotEqual<ActivityScope>(ph.Property, result.Property);
        }

        [Fact]
        public void CreateAnonymousWithFlagEnumPropertyMultipleTimesWillAssignValidValues()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            var result = sut.Build<PropertyHolder<ActivityScope?>>().CreateMany(100);
            // Assert
            long activityMin = 0;
            long activityMax = (long)ActivityScope.All;
            foreach (var propertyHolder in result)
            {
                long activityScope = (long)propertyHolder.Property;
                Assert.InRange(activityScope, activityMin, activityMax);
            }
        }

        [Fact]
        public void CreateAnonymousWithDoubleDateTimePropertiesWillAssignDifferentDateTimes()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            var result = sut.Create<DoublePropertyHolder<DateTime, DateTime>>();
            // Assert
            Assert.NotEqual(result.Property1, result.Property2);
        }

        [Fact]
        public void CreateAnonymousWithDateTimePropertyAndIncrementingDateTimeCustomizationTwiceWithinMsReturnsDateTimesExactlyOneDayApart()
        {
            // Arrange
            var nowResolution = TimeSpan.FromMilliseconds(10); // see http://msdn.microsoft.com/en-us/library/system.datetime.now.aspx
            var sut = new Fixture();
            sut.Customize(new IncrementingDateTimeCustomization());
            // Act
            var firstResult = sut.Create<PropertyHolder<DateTime>>();
            Thread.Sleep(nowResolution + nowResolution);
            var secondResult = sut.Create<PropertyHolder<DateTime>>();
            // Assert
            Assert.Equal(firstResult.Property.AddDays(1), secondResult.Property);
        }

        [Fact]
        public void CreateAnonymousWithDoubleDateTimePropertiesAndCurrentDateTimeCustomizationWillAssignEqualDates()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            sut.Customize(new CurrentDateTimeCustomization());
            var result = sut.Create<DoublePropertyHolder<DateTime, DateTime>>();
            // Assert
            Assert.Equal(result.Property1.Date, result.Property2.Date);
        }

        [Fact]
        public void CreateAnonymousWithArrayPropertyCorrectlyAssignsArray()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            var result = sut.Create<PropertyHolder<int[]>>();
            // Assert
            Assert.NotEmpty(result.Property);
            Assert.True(result.Property.All(i => i != 0));
        }

        [Fact]
        public void CreateAnonymousWithVoidParameterlessDelegatePropertyWillAssignNonDefaultValue()
        {
            // Arrange
            var unexpectedDelegate = default(Action);
            var sut = new Fixture();
            // Act
            var result = sut.Create<PropertyHolder<Action>>();
            // Assert
            Assert.NotEqual<Action>(unexpectedDelegate, result.Property);
        }

        [Fact]
        public void CreateAnonymousWithVoidSingleObjectParameterDelegatePropertyWillAssignNonDefaultValue()
        {
            // Arrange
            var unexpectedDelegate = default(Action<object>);
            var sut = new Fixture();
            // Act
            var result = sut.Create<PropertyHolder<Action<object>>>();
            // Assert
            Assert.NotEqual<Action<object>>(unexpectedDelegate, result.Property);
        }

        [Fact]
        public void CreateAnonymousWithVoidSingleSpecializedObjectParameterDelegatePropertyWillAssignNonDefaultValue()
        {
            // Arrange
            var unexpectedValue = default(Action<string>);
            var sut = new Fixture();
            // Act
            var result = sut.Create<PropertyHolder<Action<string>>>();
            // Assert
            Assert.NotEqual<Action<string>>(unexpectedValue, result.Property);
        }

        [Fact]
        public void CreateAnonymousWithVoidSingleValueParameterDelegatePropertyWillAssignNonDefaultValue()
        {
            // Arrange
            var unexpectedValue = default(Action<int>);
            var sut = new Fixture();
            // Act
            var result = sut.Create<PropertyHolder<Action<int>>>();
            // Assert
            Assert.NotEqual<Action<int>>(unexpectedValue, result.Property);
        }

        [Fact]
        public void CreateAnonymousWithVoidDoubleObjectParametersDelegatePropertyWillAssignNonDefaultValue()
        {
            // Arrange
            var unexpectedDelegate = default(Action<object, object>);
            var sut = new Fixture();
            // Act
            var result = sut.Create<PropertyHolder<Action<object, object>>>();
            // Assert
            Assert.NotEqual<Action<object, object>>(unexpectedDelegate, result.Property);
        }

        [Fact]
        public void CreateAnonymousWithVoidDoubleSpecializedObjectParametersDelegatePropertyWillAssignNonDefaultValue()
        {
            // Arrange
            var unexpectedDelegate = default(Action<string, string>);
            var sut = new Fixture();
            // Act
            var result = sut.Create<PropertyHolder<Action<string, string>>>();
            // Assert
            Assert.NotEqual<Action<string, string>>(unexpectedDelegate, result.Property);
        }

        [Fact]
        public void CreateAnonymousWithVoidDoubleValueParametersDelegatePropertyWillAssignNonDefaultValue()
        {
            // Arrange
            var unexpectedDelegate = default(Action<int, bool>);
            var sut = new Fixture();
            // Act
            var result = sut.Create<PropertyHolder<Action<int, bool>>>();
            // Assert
            Assert.NotEqual<Action<int, bool>>(unexpectedDelegate, result.Property);
        }

        [Fact]
        public void CreateAnonymousWithVoidParameterlessDelegatePropertyWillAssignDelegateNotThrowing()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            var result = sut.Create<PropertyHolder<Action>>();
            // Assert
            Assert.Null(Record.Exception(() => ((Action)result.Property).Invoke()));
        }

        [Fact]
        public void CreateAnonymousWithReturnObjectParameterlessDelegatePropertyWillAssignNonDefaultValue()
        {
            // Arrange
            var unexpectedDelegate = default(Func<object>);
            var sut = new Fixture();
            // Act
            var result = sut.Create<PropertyHolder<Func<object>>>();
            // Assert
            Assert.NotEqual<Func<object>>(unexpectedDelegate, result.Property);
        }

        [Fact]
        public void CreateAnonymousWithReturnObjectSingleObjectParameterDelegatePropertyWillAssignNonDefaultValue()
        {
            // Arrange
            var unexpectedDelegate = default(Func<object, object>);
            var sut = new Fixture();
            // Act
            var result = sut.Create<PropertyHolder<Func<object, object>>>();
            // Assert
            Assert.NotEqual<Func<object, object>>(unexpectedDelegate, result.Property);
        }

        [Fact]
        public void CreateAnonymousWithReturnObjectSingleSpecializedObjectParameterDelegatePropertyWillAssignNonDefaultValue()
        {
            // Arrange
            var unexpectedDelegate = default(Func<string, object>);
            var sut = new Fixture();
            // Act
            var result = sut.Create<PropertyHolder<Func<string, object>>>();
            // Assert
            Assert.NotEqual<Func<string, object>>(unexpectedDelegate, result.Property);
        }

        [Fact]
        public void CreateAnonymousWithReturnObjectSingleValueParameterDelegatePropertyWillAssignNonDefaultValue()
        {
            // Arrange
            var unexpectedDelegate = default(Func<int, object>);
            var sut = new Fixture();
            // Act
            var result = sut.Create<PropertyHolder<Func<int, object>>>();
            // Assert
            Assert.NotEqual<Func<int, object>>(unexpectedDelegate, result.Property);
        }

        [Fact]
        public void CreateAnonymousWithReturnValueParameterlessDelegatePropertyWillAssignNonDefaultValue()
        {
            // Arrange
            var unexpectedDelegate = default(Func<int>);
            var sut = new Fixture();
            // Act
            var result = sut.Create<PropertyHolder<Func<int>>>();
            // Assert
            Assert.NotEqual<Func<int>>(unexpectedDelegate, result.Property);
        }

        [Fact]
        public void CreateAnonymousWithReturnValueSingleObjectParameterDelegatePropertyWillAssignNonDefaultValue()
        {
            // Arrange
            var unexpectedDelegate = default(Func<int, object>);
            var sut = new Fixture();
            // Act
            var result = sut.Create<PropertyHolder<Func<int, object>>>();
            // Assert
            Assert.NotEqual<Func<int, object>>(unexpectedDelegate, result.Property);
        }

        [Fact]
        public void CreateAnonymousWithReturnValueSingleSpecializedObjectParameterDelegatePropertyWillAssignNonDefaultValue()
        {
            // Arrange
            var unexpectedDelegate = default(Func<int, string>);
            var sut = new Fixture();
            // Act
            var result = sut.Create<PropertyHolder<Func<int, string>>>();
            // Assert
            Assert.NotEqual<Func<int, string>>(unexpectedDelegate, result.Property);
        }

        [Fact]
        public void CreateAnonymousWithReturnObjectParameterlessDelegatePropertyWillAssignDelegateReturningNonDefaultValue()
        {
            // Arrange
            var unexpectedResult = default(string);
            var sut = new Fixture();
            // Act
            var result = sut.Create<PropertyHolder<Func<string>>>();
            // Assert
            var actualResult = ((Func<string>)result.Property).Invoke();
            Assert.NotEqual<string>(unexpectedResult, actualResult);
        }

        [Fact]
        public void CreateAnonymousWithReturnValueParameterlessDelegatePropertyWillAssignDelegateReturningNonDefaultValue()
        {
            // Arrange
            var unexpectedResult = default(int);
            var sut = new Fixture();
            // Act
            var result = sut.Create<PropertyHolder<Func<int>>>();
            // Assert
            var actualResult = ((Func<int>)result.Property).Invoke();
            Assert.NotEqual<int>(unexpectedResult, actualResult);
        }

        [Fact]
        public void CreateAnonymousWithTypeWithFactoryMethodWillInvokeFactoryMethod()
        {
            // Arrange
            var fixture = new Fixture();
            var result = fixture.Create<TypeWithFactoryMethod>();
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void CreateAnonymousWithTypeWithFactoryPropertyWillInvokeFactoryProperty()
        {
            // Arrange
            var fixture = new Fixture();
            var result = fixture.Create<TypeWithFactoryProperty>();
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedDecimalPropertyReturnsCorrectResultForIntegerRange()
        {
            // Arrange
            var fixture = new Fixture();
            var result = fixture.Create<RangeValidatedType>();
            // Assert
            Assert.True(
                result.Property >= RangeValidatedType.Minimum && result.Property <= RangeValidatedType.Maximum,
                string.Format(
                    "Expected result to fall into the interval [{0}, {1}], but was {2}",
                    RangeValidatedType.Minimum,
                    RangeValidatedType.Maximum,
                    result.Property));
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedDecimalFieldReturnsCorrectResultForIntegerRange()
        {
            // Arrange
            var fixture = new Fixture();
            var result = fixture.Create<RangeValidatedType>();
            // Assert
            Assert.True(result.Field >= RangeValidatedType.Minimum && result.Field <= RangeValidatedType.Maximum);
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedDoublePropertyReturnsCorrectResultForDoubleRange()
        {
            // Arrange
            var fixture = new Fixture();
            var result = fixture.Create<RangeValidatedType>();
            // Assert
            Assert.True(result.Property2 >= RangeValidatedType.DoubleMinimum && result.Property2 <= RangeValidatedType.DoubleMaximum);
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedDecimalPropertyReturnsCorrectResultForDoubleRange()
        {
            // Arrange
            var fixture = new Fixture();
            var result = fixture.Create<RangeValidatedType>();
            // Assert
            Assert.True(
                Convert.ToDecimal(RangeValidatedType.DoubleMinimum) <= result.Property3 && result.Property3 <= Convert.ToDecimal(RangeValidatedType.DoubleMaximum),
                string.Format(
                    "Expected result to fall into the interval [{0}, {1}], but was {2}",
                    RangeValidatedType.DoubleMinimum,
                    RangeValidatedType.DoubleMaximum,
                    result.Property3));
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedDecimalPropertyReturnsCorrectResultForStringRange()
        {
            // Arrange
            var fixture = new Fixture();
            var result = fixture.Create<RangeValidatedType>();
            // Assert
            Assert.True(result.Property4 >= Convert.ToDecimal(RangeValidatedType.StringMinimum) && result.Property4 <= Convert.ToDecimal(RangeValidatedType.StringMaximum));
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedIntegerPropertyReturnsCorrectResultForIntegerRange()
        {
            // Arrange
            var fixture = new Fixture();
            var result = fixture.Create<RangeValidatedType>();
            // Assert
            Assert.True(result.Property5 >= RangeValidatedType.Minimum && result.Property5 <= RangeValidatedType.Maximum);
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedBytePropertyReturnsCorrectResultForIntegerRange()
        {
            // Arrange
            var fixture = new Fixture();
            var result = fixture.Create<RangeValidatedType>();
            // Assert
            Assert.True(result.Property6 >= RangeValidatedType.Minimum && result.Property6 <= RangeValidatedType.Maximum);
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedShortPropertyReturnsCorrectResultForIntegerRange()
        {
            // Arrange
            var fixture = new Fixture();
            var result = fixture.Create<RangeValidatedType>();
            // Assert
            Assert.True(result.Property7 >= RangeValidatedType.Minimum && result.Property7 <= RangeValidatedType.Maximum);
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedUnsignedShortPropertyReturnsCorrectResultForIntegerRange()
        {
            // Arrange
            var fixture = new Fixture();
            var result = fixture.Create<RangeValidatedType>();
            // Assert
            Assert.InRange(
                result.UnsignedShortProperty,
                RangeValidatedType.Minimum,
                RangeValidatedType.Maximum);
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedUnsignedIntPropertyReturnsCorrectResultForIntegerRange()
        {
            // Arrange
            var fixture = new Fixture();
            var result = fixture.Create<RangeValidatedType>();
            // Assert
            Assert.InRange(
                (int)result.UnsignedIntProperty,
                RangeValidatedType.Minimum,
                RangeValidatedType.Maximum);
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedUnsignedLongPropertyReturnsCorrectResultForIntegerRange()
        {
            // Arrange
            var fixture = new Fixture();
            var result = fixture.Create<RangeValidatedType>();
            // Assert
            Assert.InRange(
                (int)result.UnsignedLongProperty,
                RangeValidatedType.Minimum,
                RangeValidatedType.Maximum);
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedSignedBytePropertyReturnsCorrectResultForIntegerRange()
        {
            // Arrange
            var fixture = new Fixture();
            var result = fixture.Create<RangeValidatedType>();
            // Assert
            Assert.InRange(
                (int)result.SignedByteProperty,
                RangeValidatedType.Minimum,
                RangeValidatedType.Maximum);
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedDoublePropertyReturnsCorrectResultForDoubleWithMinimumDoubleMinValue()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var result = fixture.Create<RangeValidatedType>();
            // Assert
            Assert.True(result.PropertyWithMinimumDoubleMinValue <= RangeValidatedType.Maximum);
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedDoublePropertyReturnsCorrectResultForDoubleWithMaximumDoubleMaxValue()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var result = fixture.Create<RangeValidatedType>();
            // Assert
            Assert.True(result.PropertyWithMaximumDoubleMaxValue >= RangeValidatedType.Minimum);
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedFloatPropertyReturnsCorrectResultForPropertyWithMinimumFloatMinValue()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var result = fixture.Create<RangeValidatedType>();
            // Assert
            Assert.True(result.PropertyWithMinimumFloatMinValue <= RangeValidatedType.Maximum);
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedFloatPropertyReturnsCorrectResultForPropertyWithMaximumFloatMaxValue()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var result = fixture.Create<RangeValidatedType>();
            // Assert
            Assert.True(result.PropertyWithMaximumFloatMaxValue >= RangeValidatedType.Minimum);
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedDecimalPropertyReturnsCorrectResultForIntegerRangeOnMultipleCall()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var result = from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().Property)
                         where n < RangeValidatedType.Minimum && n > RangeValidatedType.Maximum
                         select n;
            // Assert
            Assert.False(result.Any());
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedDecimalFieldReturnsCorrectResultForIntegerRangeOnMultipleCall()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var result = from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().Field)
                         where n < RangeValidatedType.Minimum && n > RangeValidatedType.Maximum
                         select n;
            // Assert
            Assert.False(result.Any());
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedDoublePropertyReturnsCorrectResultForDoubleRangeOnMultipleCall()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var result = from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().Property2)
                         where n < RangeValidatedType.DoubleMinimum && n > RangeValidatedType.DoubleMaximum
                         select n;
            // Assert
            Assert.False(result.Any());
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedDecimalPropertyReturnsCorrectResultForDoubleRangeOnMultipleCall()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var result = from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().Property3)
                         where n < Convert.ToDecimal(RangeValidatedType.DoubleMinimum) && n > Convert.ToDecimal(RangeValidatedType.DoubleMaximum)
                         select n;
            // Assert
            Assert.False(result.Any());
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedDecimalPropertyReturnsCorrectResultForStringRangeOnMultipleCall()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var result = from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().Property4)
                         where n < Convert.ToDecimal(RangeValidatedType.StringMinimum) && n > Convert.ToDecimal(RangeValidatedType.StringMaximum)
                         select n;
            // Assert
            Assert.False(result.Any());
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedIntegerPropertyReturnsCorrectResultForIntegerRangeOnMultipleCall()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var result = from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().Property5)
                         where n < RangeValidatedType.Minimum && n > RangeValidatedType.Maximum
                         select n;
            // Assert
            Assert.False(result.Any());
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedBytePropertyReturnsCorrectResultForIntegerRangeOnMultipleCall()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var result = from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().Property6)
                         where n < RangeValidatedType.Minimum && n > RangeValidatedType.Maximum
                         select n;
            // Assert
            Assert.False(result.Any());
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedShortPropertyReturnsCorrectResultForIntegerRangeOnMultipleCall()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var result = from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().Property7)
                         where n < RangeValidatedType.Minimum && n > RangeValidatedType.Maximum
                         select n;
            // Assert
            Assert.False(result.Any());
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedUnsignedShortPropertyReturnsCorrectResultForIntegerRangeOnMultipleCall()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var result = from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().UnsignedShortProperty)
                         where n < RangeValidatedType.Minimum && n > RangeValidatedType.Maximum
                         select n;
            // Assert
            Assert.False(result.Any());
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedUnsignedIntPropertyReturnsCorrectResultForIntegerRangeOnMultipleCall()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var result = from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().UnsignedIntProperty)
                         where n < RangeValidatedType.Minimum && n > RangeValidatedType.Maximum
                         select n;
            // Assert
            Assert.False(result.Any());
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedUnsignedLongPropertyReturnsCorrectResultForIntegerRangeOnMultipleCall()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var result = from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().UnsignedLongProperty)
                         where n < RangeValidatedType.Minimum && n > RangeValidatedType.Maximum
                         select n;
            // Assert
            Assert.False(result.Any());
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedSignedBytePropertyReturnsCorrectResultForIntegerRangeOnMultipleCall()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var result = from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().SignedByteProperty)
                         where n < RangeValidatedType.Minimum && n > RangeValidatedType.Maximum
                         select n;
            // Assert
            Assert.False(result.Any());
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedDoublePropertyReturnsCorrectResultForDoubleWithMinimumDoubleMinValueOnMultipleCall()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var result = from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().PropertyWithMinimumDoubleMinValue)
                         where n > Convert.ToDouble(RangeValidatedType.Maximum)
                         select n;
            // Assert
            Assert.False(result.Any());
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedDoublePropertyReturnsCorrectResultForDoubleWithMaximumDoubleMaxValueOnMultipleCall()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var result = from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().PropertyWithMaximumDoubleMaxValue)
                         where n < Convert.ToDouble(RangeValidatedType.Minimum)
                         select n;
            // Assert
            Assert.False(result.Any());
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedFloatPropertyReturnsCorrectResultForPropertyWithMinimumFloatMinValueOnMultipleCall()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var result = from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().PropertyWithMinimumFloatMinValue)
                         where n > Convert.ToSingle(RangeValidatedType.Maximum)
                         select n;
            // Assert
            Assert.False(result.Any());
        }

        [Fact]
        [UseCulture("en-US")]
        public void CreateAnonymousWithRangeValidatedFloatPropertyReturnsCorrectResultForPropertyWithMaximumFloatMaxValueOnMultipleCall()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var result = from n in Enumerable.Range(1, 33).Select(i => fixture.Create<RangeValidatedType>().PropertyWithMaximumFloatMaxValue)
                         where n < Convert.ToSingle(RangeValidatedType.Minimum)
                         select n;
            // Assert
            Assert.False(result.Any());
        }

        [Fact]
        public void CreateAnonymousWithRegularExpressionValidatedTypeReturnsCorrectResult()
        {
            // Arrange
            var fixture = new Fixture();
            var result = fixture.Create<RegularExpressionValidatedType>();
            // Assert
            Assert.Matches(RegularExpressionValidatedType.Pattern, result.Property);
        }

        [Fact]
        public void CreateManyAnonymousWithRegularExpressionValidatedTypeReturnsDifferentResults()
        {
            // This test exposes an issue with Xeger/Random.
            // Xeger(pattern) internally creates an instance of Random with the default seed.
            // This means that the RegularExpressionGenerator might create identical strings
            // if called multiple times within a short time.

            // Arrange
            var fixture = new Fixture();
            var result = fixture.CreateMany<RegularExpressionValidatedType>(10).Select(x => x.Property).ToArray();
            // Assert
            Assert.Equal(result.Distinct(), result);
        }

        [Fact]
        public void CreateAnonymousWithStringLengthValidatedTypeReturnsCorrectResult()
        {
            // Arrange
            var fixture = new Fixture();
            var result = fixture.Create<StringLengthValidatedType>();
            // Assert
            Assert.True(result.Property.Length <= StringLengthValidatedType.MaximumLength);
        }

        [Fact]
        public void CreateAnonymousWithStringLengthValidatedReturnsCorrectResultMultipleCall()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var result = from n in Enumerable.Range(1, 33).Select(i => fixture.Create<StringLengthValidatedType>().Property.Length)
                         where n > StringLengthValidatedType.MaximumLength
                         select n;
            // Assert
            Assert.False(result.Any());
        }

        [Fact]
        public void CreateAnonymousWithUriReturnsValidResult()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            Uri result;
            bool succeed = Uri.TryCreate(
                fixture.Create<Uri>().OriginalString,
                UriKind.Absolute,
                out result);
            // Assert
            Assert.True(succeed && result != null);
        }

        [Fact]
        public void DefaultRepeatCountIsThree()
        {
            // Arrange
            int expectedRepeatCount = 3;
            Fixture sut = new Fixture();
            // Act
            int result = sut.RepeatCount;
            // Assert
            Assert.Equal<int>(expectedRepeatCount, result);
        }
        [Fact]
        public void RepeatWillPerformActionTheDefaultNumberOfTimes()
        {
            // Arrange
            IFixture sut = new Fixture();
            int expectedCount = sut.RepeatCount;
            // Act
            int result = 0;
            sut.Repeat(() => result++).ToList();
            // Assert
            Assert.Equal<int>(expectedCount, result);
        }

        [Fact]
        public void RepeatWillReturnTheDefaultNumberOfItems()
        {
            // Arrange
            IFixture sut = new Fixture();
            int expectedCount = sut.RepeatCount;
            // Act
            IEnumerable<object> result = sut.Repeat(() => new object());
            // Assert
            Assert.Equal<int>(expectedCount, result.Count());
        }

        [Fact]
        public void RepeatWillPerformActionTheSpecifiedNumberOfTimes()
        {
            // Arrange
            int expectedCount = 2;
            IFixture sut = new Fixture();
            sut.RepeatCount = expectedCount;
            // Act
            int result = 0;
            sut.Repeat(() => result++).ToList();
            // Assert
            Assert.Equal<int>(expectedCount, result);
        }

        [Fact]
        public void RepeatWillReturnTheSpecifiedNumberOfItems()
        {
            // Arrange
            int expectedCount = 13;
            IFixture sut = new Fixture();
            sut.RepeatCount = expectedCount;
            // Act
            IEnumerable<object> result = sut.Repeat(() => new object());
            // Assert
            Assert.Equal<int>(expectedCount, result.Count());
        }

        [Fact]
        public void ReplacingStringMappingWillUseNewStringCreationAlgorithm()
        {
            // Arrange
            string expectedText = "Anonymous string";
            Fixture sut = new Fixture();
            // Act
            sut.Customize<string>(c => c.FromSeed(s => expectedText));
            // Assert
            string result = sut.Create<string>();
            Assert.Equal(expectedText, result);
        }

        [Fact]
        public void AddManyWillAddItemsToListUsingCreator()
        {
            // Arrange
            Fixture sut = new Fixture();
            IEnumerable<int> expectedList = Enumerable.Range(1, sut.RepeatCount);
            List<int> list = new List<int>();
            // Act
            int i = 0;
            sut.AddManyTo(list, () => ++i);
            // Assert
            Assert.True(expectedList.SequenceEqual(list));
        }

        [Fact]
        public void AddManyWillAddItemsToListUsingAnonymousCreator()
        {
            // Arrange
            Fixture sut = new Fixture();
            int expectedItemCount = sut.RepeatCount;
            List<string> list = new List<string>();
            // Act
            sut.AddManyTo(list);
            // Assert
            int result = (from s in list
                          where !string.IsNullOrEmpty(s)
                          select s).Count();
            Assert.Equal<int>(expectedItemCount, result);
        }

        [Fact]
        public void AddManyWillAddItemsToCollection()
        {
            // Arrange
            Fixture sut = new Fixture();
            int expectedCount = sut.RepeatCount;
            ICollection<int> collection = new LinkedList<int>();
            // Act
            sut.AddManyTo(collection);
            // Assert
            Assert.Equal<int>(expectedCount, collection.Count);
        }

        [Fact]
        public void AddManyWithRepeatCountWillAddItemsToCollection()
        {
            // Arrange
            var sut = new Fixture();
            int expectedCount = 24;
            ICollection<int> collection = new LinkedList<int>();
            // Act
            sut.AddManyTo(collection, expectedCount);
            // Assert
            Assert.Equal<int>(expectedCount, collection.Count);
        }

        [Fact]
        public void AddManyWithCreatorWillAddItemsToCollection()
        {
            // Arrange
            Fixture sut = new Fixture();
            int expectedCount = sut.RepeatCount;
            ICollection<object> collection = new LinkedList<object>();
            // Act
            sut.AddManyTo(collection, () => new object());
            // Assert
            Assert.Equal<int>(expectedCount, collection.Count);
        }

        [Fact]
        public void CreateManyWillCreateManyAnonymousItems()
        {
            // Arrange
            Fixture sut = new Fixture();
            int expectedItemCount = sut.RepeatCount;
            // Act
            IEnumerable<string> result = sut.CreateMany<string>();
            // Assert
            int nonDefaultCount = (from s in result
                                   where !string.IsNullOrEmpty(s)
                                   select s).Count();
            Assert.Equal<int>(expectedItemCount, nonDefaultCount);
        }

        [Fact]
        public void CreateManyWillCreateCorrectNumberOfAnonymousItems()
        {
            // Arrange
            var sut = new Fixture();
            int expectedItemCount = 248;
            // Act
            IEnumerable<string> result = sut.CreateMany<string>(expectedItemCount);
            // Assert
            int nonDefaultCount = (from s in result
                                   where !string.IsNullOrEmpty(s)
                                   select s).Count();
            Assert.Equal<int>(expectedItemCount, nonDefaultCount);
        }

        [Fact]
        public void CustomizeNullTransformationThrows()
        {
            // Arrange
            var sut = new Fixture();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Customize<object>(null));
        }

        [Fact]
        public void RegisterTypeWithPropertyOverrideWillSetPropertyValueCorrectly()
        {
            // Arrange
            string expectedValue = "Anonymous text";
            Fixture sut = new Fixture();
            // Act
            sut.Customize<PropertyHolder<string>>(f => f.With(ph => ph.Property, expectedValue));
            PropertyHolder<string> result = sut.Create<PropertyHolder<string>>();
            // Assert
            Assert.Equal(expectedValue, result.Property);
        }

        [Fact]
        public void CreateNestedTypeWillPopulateNestedProperty()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            var result = sut.Create<PropertyHolder<PropertyHolder<string>>>();
            // Assert
            Assert.False(string.IsNullOrEmpty(result.Property.Property), "Nested property string should not be null or empty.");
        }

        [Fact]
        public void DoOnCommandWithNullSingleParameterActionThrows()
        {
            // Arrange
            var sut = new Fixture();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Do<object>(null));
        }

        [Fact]
        public void DoOnCommandWithSingleParameterWillInvokeMethod()
        {
            // Arrange
            bool methodInvoked = false;
            var sut = new Fixture();

            var mock = new CommandMock<string>();
            mock.OnCommand = x => methodInvoked = true;
            // Act
            sut.Do((string s) => mock.Command(s));
            // Assert
            Assert.True(methodInvoked, "Command method invoked");
        }

        [Fact]
        public void DoOnCommandWithSingleParameterWillInvokeMethodWithCorrectParameter()
        {
            // Arrange
            int expectedNumber = 94;

            var sut = new Fixture();
            sut.Register<int>(() => expectedNumber);

            var mock = new CommandMock<int>();
            mock.OnCommand = x => Assert.Equal<int>(expectedNumber, x);
            // Act
            sut.Do((int i) => mock.Command(i));
            // Assert (done by mock)
        }

        [Fact]
        public void DoOnCommandWithNullTwoParameterActionThrows()
        {
            // Arrange
            var sut = new Fixture();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Do<object, object>(null));
        }

        [Fact]
        public void DoOnCommandWithTwoParametersWillInvokeMethod()
        {
            // Arrange
            bool methodInvoked = false;
            var sut = new Fixture();

            var mock = new CommandMock<string, long>();
            mock.OnCommand = (x1, x2) => methodInvoked = true;
            // Act
            sut.Do((string x1, long x2) => mock.Command(x1, x2));
            // Assert
            Assert.True(methodInvoked, "Command method invoked");
        }

        [Fact]
        public void DoOnCommandWithTwoParametersWillInvokeMethodWithCorrectFirstParameter()
        {
            // Arrange
            double expectedNumber = 25364.37;

            var sut = new Fixture();
            sut.Register<double>(() => expectedNumber);

            var mock = new CommandMock<double, object>();
            mock.OnCommand = (x1, x2) => Assert.Equal<double>(expectedNumber, x1);
            // Act
            sut.Do((double x1, object x2) => mock.Command(x1, x2));
            // Assert (done by mock)
        }

        [Fact]
        public void DoOnCommandWithTwoParametersWillInvokeMethodWithCorrectSecondParameter()
        {
            // Arrange
            short expectedNumber = 3734;

            var sut = new Fixture();
            sut.Register<short>(() => expectedNumber);

            var mock = new CommandMock<DateTime, short>();
            mock.OnCommand = (x1, x2) => Assert.Equal<short>(expectedNumber, x2);
            // Act
            sut.Do((DateTime x1, short x2) => mock.Command(x1, x2));
            // Assert (done by mock)
        }

        [Fact]
        public void DoOnCommandWithNullThreeParameterActionThrows()
        {
            // Arrange
            var sut = new Fixture();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Do<object, object, object>(null));
        }

        [Fact]
        public void DoOnCommandWithThreeParametersWillInvokeMethod()
        {
            // Arrange
            bool methodInvoked = false;
            var sut = new Fixture();

            var mock = new CommandMock<object, object, object>();
            mock.OnCommand = (x1, x2, x3) => methodInvoked = true;
            // Act
            sut.Do((object x1, object x2, object x3) => mock.Command(x1, x2, x3));
            // Assert
            Assert.True(methodInvoked, "Command method invoked");
        }

        [Fact]
        public void DoOnCommandWithThreeParametersWillInvokeMethodWithCorrectFirstParameter()
        {
            // Arrange
            DateTime expectedDateTime = new DateTime(1004328837);

            var sut = new Fixture();
            sut.Register<DateTime>(() => expectedDateTime);

            var mock = new CommandMock<DateTime, long, short>();
            mock.OnCommand = (x1, x2, x3) => Assert.Equal<DateTime>(expectedDateTime, x1);
            // Act
            sut.Do((DateTime x1, long x2, short x3) => mock.Command(x1, x2, x3));
            // Assert (done by mock)
        }

        [Fact]
        public void DoOnCommandWithThreeParametersWillInvokeMethodWithCorrectSecondParameter()
        {
            // Arrange
            TimeSpan expectedTimeSpan = TimeSpan.FromHours(53);

            var sut = new Fixture();
            sut.Register<TimeSpan>(() => expectedTimeSpan);

            var mock = new CommandMock<uint, TimeSpan, TimeSpan>();
            mock.OnCommand = (x1, x2, x3) => Assert.Equal<TimeSpan>(expectedTimeSpan, x2);
            // Act
            sut.Do((uint x1, TimeSpan x2, TimeSpan x3) => mock.Command(x1, x2, x3));
            // Assert (done by mock)
        }

        [Fact]
        public void DoOnCommandWithThreeParametersWillInvokeMethodWithCorrectThirdParameter()
        {
            // Arrange
            var expectedText = "Anonymous text";

            var sut = new Fixture();
            sut.Register<string>(() => expectedText);

            var mock = new CommandMock<double, uint, string>();
            mock.OnCommand = (x1, x2, x3) => Assert.Equal(expectedText, x3);
            // Act
            sut.Do((double x1, uint x2, string x3) => mock.Command(x1, x2, x3));
            // Assert (done by mock)
        }

        [Fact]
        public void DoOnCommandWithNullFourParameterActionThrows()
        {
            // Arrange
            var sut = new Fixture();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Do<object, object, object, object>(null));
        }

        [Fact]
        public void DoOnCommandWithFourParametersWillInvokeMethod()
        {
            // Arrange
            bool methodInvoked = false;
            var sut = new Fixture();

            var mock = new CommandMock<uint, ushort, int, bool>();
            mock.OnCommand = (x1, x2, x3, x4) => methodInvoked = true;
            // Act
            sut.Do((uint x1, ushort x2, int x3, bool x4) => mock.Command(x1, x2, x3, x4));
            // Assert
            Assert.True(methodInvoked, "Command method invoked");
        }

        [Fact]
        public void DoOnCommandWithFourParametersWillInvokeMethodWithCorrectFirstParameter()
        {
            // Arrange
            uint expectedNumber = 294;

            var sut = new Fixture();
            sut.Register<uint>(() => expectedNumber);

            var mock = new CommandMock<uint, bool, string, Guid>();
            mock.OnCommand = (x1, x2, x3, x4) => Assert.Equal<uint>(expectedNumber, x1);
            // Act
            sut.Do((uint x1, bool x2, string x3, Guid x4) => mock.Command(x1, x2, x3, x4));
            // Assert (done by mock)
        }

        [Fact]
        public void DoOnCommandWithFourParametersWillInvokeMethodWithCorrectSecondParameter()
        {
            // Arrange
            decimal expectedNumber = 92183.28m;

            var sut = new Fixture();
            sut.Register<decimal>(() => expectedNumber);

            var mock = new CommandMock<ushort, decimal, Guid, bool>();
            mock.OnCommand = (x1, x2, x3, x4) => Assert.Equal<decimal>(expectedNumber, x2);
            // Act
            sut.Do((ushort x1, decimal x2, Guid x3, bool x4) => mock.Command(x1, x2, x3, x4));
            // Assert (done by mock)
        }

        [Fact]
        public void DoOnCommandWithFourParametersWillInvokeMethodWithCorrectThirdParameter()
        {
            // Arrange
            Guid expectedGuid = Guid.NewGuid();

            var sut = new Fixture();
            sut.Register<Guid>(() => expectedGuid);

            var mock = new CommandMock<bool, string, Guid, string>();
            mock.OnCommand = (x1, x2, x3, x4) => Assert.Equal<Guid>(expectedGuid, x3);
            // Act
            sut.Do((bool x1, string x2, Guid x3, string x4) => mock.Command(x1, x2, x3, x4));
            // Assert (done by mock)
        }

        [Fact]
        public void DoOnCommandWithFourParametersWillInvokeMethodWithCorrectFourthParameter()
        {
            // Arrange
            var expectedObj = new ConcreteType();

            var sut = new Fixture();
            sut.Register<ConcreteType>(() => expectedObj);

            var mock = new CommandMock<int?, DateTime, TimeSpan, ConcreteType>();
            mock.OnCommand = (x1, x2, x3, x4) => Assert.Equal<ConcreteType>(expectedObj, x4);
            // Act
            sut.Do((int? x1, DateTime x2, TimeSpan x3, ConcreteType x4) => mock.Command(x1, x2, x3, x4));
            // Assert (done by mock)
        }

        [Fact]
        public void GetOnCommandWithNullSingleParameterFunctionThrows()
        {
            // Arrange
            var sut = new Fixture();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Get<object, object>(null));
        }

        [Fact]
        public void GetOnQueryWithSingleParameterWillInvokeMethod()
        {
            // Arrange
            bool methodInvoked = false;
            var sut = new Fixture();

            var mock = new QueryMock<ulong, bool>();
            mock.OnQuery = x =>
            {
                methodInvoked = true;
                return true;
            };
            // Act
            sut.Get((ulong s) => mock.Query(s));
            // Assert
            Assert.True(methodInvoked, "Query method invoked");
        }

        [Fact]
        public void GetOnQueryWithSingleParameterWillInvokeMethodWithCorrectParameter()
        {
            // Arrange
            double? expectedNumber = 23892;

            var sut = new Fixture();
            sut.Register<double?>(() => expectedNumber);

            var mock = new QueryMock<double?, string>();
            mock.OnQuery = x =>
            {
                Assert.Equal<double?>(expectedNumber, x);
                return "Anonymous text";
            };
            // Act
            sut.Get((double? x) => mock.Query(x));
            // Assert (done by mock)
        }

        [Fact]
        public void GetOnQueryWithSingleParameterWillReturnCorrectResult()
        {
            // Arrange
            var expectedVersion = new Version(2, 45);
            var sut = new Fixture();

            var mock = new QueryMock<int?, Version>();
            mock.OnQuery = x => expectedVersion;
            // Act
            var result = sut.Get((int? x) => mock.Query(x));
            // Assert
            Assert.Equal<Version>(expectedVersion, result);
        }

        [Fact]
        public void GetOnCommandWithNullDoubleParameterFunctionThrows()
        {
            // Arrange
            var sut = new Fixture();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Get<object, object, object>(null));
        }

        [Fact]
        public void GetOnQueryWithTwoParametersWillInvokeMethod()
        {
            // Arrange
            bool methodInvoked = false;
            var sut = new Fixture();

            var mock = new QueryMock<string, int, long>();
            mock.OnQuery = (x1, x2) =>
            {
                methodInvoked = true;
                return 148;
            };
            // Act
            sut.Get((string x1, int x2) => mock.Query(x1, x2));
            // Assert
            Assert.True(methodInvoked, "Query method invoked");
        }

        [Fact]
        public void GetOnQueryWithTwoParametersWillInvokeMethodWithCorrectFirstParameter()
        {
            // Arrange
            byte expectedByte = 213;

            var sut = new Fixture();
            sut.Register<byte>(() => expectedByte);

            var mock = new QueryMock<byte, int, double>();
            mock.OnQuery = (x1, x2) =>
            {
                Assert.Equal<byte>(expectedByte, x1);
                return 9823829;
            };
            // Act
            sut.Get((byte x1, int x2) => mock.Query(x1, x2));
            // Assert (done by mock)
        }

        [Fact]
        public void GetOnQueryWithTwoParametersWillInvokeMethodWithCorrectSecondParameter()
        {
            // Arrange
            sbyte expectedByte = -29;

            var sut = new Fixture();
            sut.Register<sbyte>(() => expectedByte);

            var mock = new QueryMock<DateTime, sbyte, bool>();
            mock.OnQuery = (x1, x2) =>
            {
                Assert.Equal<sbyte>(expectedByte, x2);
                return false;
            };
            // Act
            sut.Get((DateTime x1, sbyte x2) => mock.Query(x1, x2));
            // Assert (done by mock)
        }

        [Fact]
        public void GetOnQueryWithTwoParametersWillReturnCorrectResult()
        {
            // Arrange
            byte? expectedByte = 198;
            var sut = new Fixture();

            var mock = new QueryMock<DateTime, TimeSpan, byte?>();
            mock.OnQuery = (x1, x2) => expectedByte;
            // Act
            var result = sut.Get((DateTime x1, TimeSpan x2) => mock.Query(x1, x2));
            // Assert
            Assert.Equal<byte?>(expectedByte, result);
        }

        [Fact]
        public void GetOnCommandWithNullTripleParameterFunctionThrows()
        {
            // Arrange
            var sut = new Fixture();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Get<object, object, object, object>(null));
        }

        [Fact]
        public void GetOnQueryWithThreeParametersWillInvokeMethod()
        {
            // Arrange
            bool methodInvoked = false;
            var sut = new Fixture();

            var mock = new QueryMock<object, object, object, object>();
            mock.OnQuery = (x1, x2, x3) =>
            {
                methodInvoked = true;
                return new object();
            };
            // Act
            sut.Get((object x1, object x2, object x3) => mock.Query(x1, x2, x3));
            // Assert
            Assert.True(methodInvoked, "Query method invoked");
        }

        [Fact]
        public void GetOnQueryWithThreeParametersWillInvokeMethodWithCorrectFirstParameter()
        {
            // Arrange
            sbyte? expectedByte = -56;

            var sut = new Fixture();
            sut.Register<sbyte?>(() => expectedByte);

            var mock = new QueryMock<sbyte?, bool, string, float>();
            mock.OnQuery = (x1, x2, x3) =>
            {
                Assert.Equal<sbyte?>(expectedByte, x1);
                return 3646.77f;
            };
            // Act
            sut.Get((sbyte? x1, bool x2, string x3) => mock.Query(x1, x2, x3));
            // Assert (done by mock)
        }

        [Fact]
        public void GetOnQueryWithThreeParametersWillInvokeMethodWithCorrectSecondParameter()
        {
            // Arrange
            float expectedNumber = -927.2f;

            var sut = new Fixture();
            sut.Register<float>(() => expectedNumber);

            var mock = new QueryMock<bool, float, TimeSpan, object>();
            mock.OnQuery = (x1, x2, x3) =>
            {
                Assert.Equal<float>(expectedNumber, x2);
                return new object();
            };
            // Act
            sut.Get((bool x1, float x2, TimeSpan x3) => mock.Query(x1, x2, x3));
            // Assert (done by mock)
        }

        [Fact]
        public void GetOnQueryWithThreeParametersWillInvokeMethodWithCorrectThirdParameter()
        {
            // Arrange
            var expectedText = "Anonymous text";

            var sut = new Fixture();
            sut.Register<string>(() => expectedText);

            var mock = new QueryMock<long, short, string, decimal?>();
            mock.OnQuery = (x1, x2, x3) =>
            {
                Assert.Equal(expectedText, x3);
                return 111.11m;
            };
            // Act
            sut.Get((long x1, short x2, string x3) => mock.Query(x1, x2, x3));
            // Assert (done by mock)
        }

        [Fact]
        public void GetOnQueryWithThreeParametersWillReturnCorrectResult()
        {
            // Arrange
            var expectedDateTime = new DateTimeOffset(2839327192831219387, TimeSpan.FromHours(-2));
            var sut = new Fixture();

            var mock = new QueryMock<short, long, Guid, DateTimeOffset>();
            mock.OnQuery = (x1, x2, x3) => expectedDateTime;
            // Act
            var result = sut.Get((short x1, long x2, Guid x3) => mock.Query(x1, x2, x3));
            // Assert
            Assert.Equal<DateTimeOffset>(expectedDateTime, result);
        }

        [Fact]
        public void GetOnCommandWithNullQuadrupleParameterFunctionThrows()
        {
            // Arrange
            var sut = new Fixture();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Get<object, object, object, object, object>(null));
        }

        [Fact]
        public void GetOnQueryWithFourParametersWillInvokeMethod()
        {
            // Arrange
            bool methodInvoked = false;
            var sut = new Fixture();

            var mock = new QueryMock<object, object, object, object, object>();
            mock.OnQuery = (x1, x2, x3, x4) =>
            {
                methodInvoked = true;
                return new object();
            };
            // Act
            sut.Get((object x1, object x2, object x3, object x4) => mock.Query(x1, x2, x3, x4));
            // Assert
            Assert.True(methodInvoked, "Query method invoked");
        }

        [Fact]
        public void GetOnQueryWithFourParametersWillInvokeMethodWithCorrectFirstParameter()
        {
            // Arrange
            var expectedTimeSpan = TimeSpan.FromSeconds(23);

            var sut = new Fixture();
            sut.Register<TimeSpan>(() => expectedTimeSpan);

            var mock = new QueryMock<TimeSpan, Version, Random, Guid, EventArgs>();
            mock.OnQuery = (x1, x2, x3, x4) =>
            {
                Assert.Equal<TimeSpan>(expectedTimeSpan, x1);
                return EventArgs.Empty;
            };
            // Act
            sut.Get((TimeSpan x1, Version x2, Random x3, Guid x4) => mock.Query(x1, x2, x3, x4));
            // Assert (done by mock)
        }

        [Fact]
        public void GetOnQueryWithFourParametersWillInvokeMethodWithCorrectSecondParameter()
        {
            // Arrange
            var expectedDateTimeKind = DateTimeKind.Utc;

            var sut = new Fixture();
            sut.Register<DateTimeKind>(() => expectedDateTimeKind);

            var mock = new QueryMock<Random, DateTimeKind, DateTime, string, float>();
            mock.OnQuery = (x1, x2, x3, x4) =>
            {
                Assert.Equal<DateTimeKind>(expectedDateTimeKind, x2);
                return 77f;
            };
            // Act
            sut.Get((Random x1, DateTimeKind x2, DateTime x3, string x4) => mock.Query(x1, x2, x3, x4));
            // Assert (done by mock)
        }

        [Fact]
        public void GetOnQueryWithFourParametersWillInvokeMethodWithCorrectThirdParameter()
        {
            // Arrange
            var expectedDayOfWeek = DayOfWeek.Friday;

            var sut = new Fixture();
            sut.Register<DayOfWeek>(() => expectedDayOfWeek);

            var mock = new QueryMock<int, float, DayOfWeek, string, ConsoleColor>();
            mock.OnQuery = (x1, x2, x3, x4) =>
            {
                Assert.Equal<DayOfWeek>(expectedDayOfWeek, x3);
                return ConsoleColor.Black;
            };
            // Act
            sut.Get((int x1, float x2, DayOfWeek x3, string x4) => mock.Query(x1, x2, x3, x4));
            // Assert (done by mock)
        }

        [Fact]
        public void GetOnQueryWithFourParametersWillInvokeMethodWithCorrectFourthParameter()
        {
            // Arrange
            var expectedNumber = 42;

            var sut = new Fixture();
            sut.Register<int>(() => expectedNumber);

            var mock = new QueryMock<Version, ushort, string, int, ConsoleColor>();
            mock.OnQuery = (x1, x2, x3, x4) =>
            {
                Assert.Equal<int>(expectedNumber, x4);
                return ConsoleColor.Cyan;
            };
            // Act
            sut.Get((Version x1, ushort x2, string x3, int x4) => mock.Query(x1, x2, x3, x4));
            // Assert (done by mock)
        }

        [Fact]
        public void GetOnQueryWithFourParametersWillReturnCorrectResult()
        {
            // Arrange
            var expectedColor = ConsoleColor.DarkGray;
            var sut = new Fixture();

            var mock = new QueryMock<int, int, int, int, ConsoleColor>();
            mock.OnQuery = (x1, x2, x3, x4) => expectedColor;
            // Act
            var result = sut.Get((int x1, int x2, int x3, int x4) => mock.Query(x1, x2, x3, x4));
            // Assert
            Assert.Equal<ConsoleColor>(expectedColor, result);
        }

        [Fact]
        [Obsolete]
        public void FromFactoryWithOneParameterWillRespectPreviousCustomizationsObsolete()
        {
            // Arrange
            string expectedText = Guid.NewGuid().ToString();
            var sut = new Fixture();
            sut.Customize<PropertyHolder<string>>(ob => ob.With(ph => ph.Property, expectedText));
            // Act
            var result = sut.Build<SingleParameterType<PropertyHolder<string>>>()
                .FromFactory((PropertyHolder<string> ph) => new SingleParameterType<PropertyHolder<string>>(ph))
                .CreateAnonymous();
            // Assert
            Assert.Equal(expectedText, result.Parameter.Property);
        }

        [Fact]
        public void FromFactoryWithOneParameterWillRespectPreviousCustomizations()
        {
            // Arrange
            string expectedText = Guid.NewGuid().ToString();
            var sut = new Fixture();
            sut.Customize<PropertyHolder<string>>(ob => ob.With(ph => ph.Property, expectedText));
            // Act
            var result = sut.Build<SingleParameterType<PropertyHolder<string>>>()
                .FromFactory((PropertyHolder<string> ph) => new SingleParameterType<PropertyHolder<string>>(ph))
                .Create();
            // Assert
            Assert.Equal(expectedText, result.Parameter.Property);
        }

        [Fact]
        [Obsolete]
        public void FromFactoryWithTwoParametersWillRespectPreviousCustomizationsObsolete()
        {
            // Arrange
            string expectedText = Guid.NewGuid().ToString();
            var sut = new Fixture();
            sut.Customize<PropertyHolder<string>>(ob => ob.With(ph => ph.Property, expectedText));
            // Act
            var result = sut.Build<SingleParameterType<PropertyHolder<string>>>()
                .FromFactory((PropertyHolder<string> ph, object dummy) => new SingleParameterType<PropertyHolder<string>>(ph))
                .CreateAnonymous();
            // Assert
            Assert.Equal(expectedText, result.Parameter.Property);
        }

        [Fact]
        public void FromFactoryWithTwoParametersWillRespectPreviousCustomizations()
        {
            // Arrange
            string expectedText = Guid.NewGuid().ToString();
            var sut = new Fixture();
            sut.Customize<PropertyHolder<string>>(ob => ob.With(ph => ph.Property, expectedText));
            // Act
            var result = sut.Build<SingleParameterType<PropertyHolder<string>>>()
                .FromFactory((PropertyHolder<string> ph, object dummy) => new SingleParameterType<PropertyHolder<string>>(ph))
                .Create();
            // Assert
            Assert.Equal(expectedText, result.Parameter.Property);
        }

        [Fact]
        [Obsolete]
        public void FromFactoryWithThreeParametersWillRespectPreviousCustomizationsObsolete()
        {
            // Arrange
            string expectedText = Guid.NewGuid().ToString();
            var sut = new Fixture();
            sut.Customize<PropertyHolder<string>>(ob => ob.With(ph => ph.Property, expectedText));
            // Act
            var result = sut.Build<SingleParameterType<PropertyHolder<string>>>()
                .FromFactory((PropertyHolder<string> ph, object dummy1, object dummy2) => new SingleParameterType<PropertyHolder<string>>(ph))
                .CreateAnonymous();
            // Assert
            Assert.Equal(expectedText, result.Parameter.Property);
        }

        [Fact]
        public void FromFactoryWithThreeParametersWillRespectPreviousCustomizations()
        {
            // Arrange
            string expectedText = Guid.NewGuid().ToString();
            var sut = new Fixture();
            sut.Customize<PropertyHolder<string>>(ob => ob.With(ph => ph.Property, expectedText));
            // Act
            var result = sut.Build<SingleParameterType<PropertyHolder<string>>>()
                .FromFactory((PropertyHolder<string> ph, object dummy1, object dummy2) => new SingleParameterType<PropertyHolder<string>>(ph))
                .Create();
            // Assert
            Assert.Equal(expectedText, result.Parameter.Property);
        }

        [Fact]
        [Obsolete]
        public void FromFactoryWithFourParametersWillRespectPreviousCustomizationsObsolete()
        {
            // Arrange
            string expectedText = Guid.NewGuid().ToString();
            var sut = new Fixture();
            sut.Customize<PropertyHolder<string>>(ob => ob.With(ph => ph.Property, expectedText));
            // Act
            var result = sut.Build<SingleParameterType<PropertyHolder<string>>>()
                .FromFactory((PropertyHolder<string> ph, object dummy1, object dummy2, object dummy3) => new SingleParameterType<PropertyHolder<string>>(ph))
                .CreateAnonymous();
            // Assert
            Assert.Equal(expectedText, result.Parameter.Property);
        }

        [Fact]
        public void FromFactoryWithFourParametersWillRespectPreviousCustomizations()
        {
            // Arrange
            string expectedText = Guid.NewGuid().ToString();
            var sut = new Fixture();
            sut.Customize<PropertyHolder<string>>(ob => ob.With(ph => ph.Property, expectedText));
            // Act
            var result = sut.Build<SingleParameterType<PropertyHolder<string>>>()
                .FromFactory((PropertyHolder<string> ph, object dummy1, object dummy2, object dummy3) => new SingleParameterType<PropertyHolder<string>>(ph))
                .Create();
            // Assert
            Assert.Equal(expectedText, result.Parameter.Property);
        }

        [Fact]
        public void CustomizeCanDefineConstructor()
        {
            // Arrange
            var sut = new Fixture();
            string expectedText = Guid.NewGuid().ToString();
            sut.Customize<SingleParameterType<string>>(ob => ob.FromFactory(() => new SingleParameterType<string>(expectedText)));
            // Act
            var result = sut.Create<SingleParameterType<string>>();
            // Assert
            Assert.Equal(expectedText, result.Parameter);
        }

        [Fact]
        public void CreateAnonymousWillNotThrowWhenTypeHasIndexedProperty()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            var result = sut.Create<IndexedPropertyHolder<object>>();
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void BuildWillReturnBuilderThatCreatesTheCorrectNumberOfInstances()
        {
            // Arrange
            int expectedRepeatCount = 242;
            var sut = new Fixture();
            sut.RepeatCount = expectedRepeatCount;
            // Act
            var result = sut.Build<object>().CreateMany();
            // Assert
            Assert.Equal<int>(expectedRepeatCount, result.Count());
        }

        [Fact]
        public void InjectWillCauseSutToReturnInstanceWhenRequested()
        {
            // Arrange
            var expectedResult = new PropertyHolder<object>();
            var sut = new Fixture();
            sut.Inject(expectedResult);
            // Act
            var result = sut.Create<PropertyHolder<object>>();
            // Assert
            Assert.Equal<PropertyHolder<object>>(expectedResult, result);
        }

        [Fact]
        public void InjectWillCauseSutToReturnInstanceWithoutAutoPropertiesWhenRequested()
        {
            // Arrange
            var item = new PropertyHolder<object>();
            item.Property = null;

            var sut = new Fixture();
            sut.Inject(item);
            // Act
            var result = sut.Create<PropertyHolder<object>>();
            // Assert
            Assert.Null(result.Property);
        }

        [Fact]
        public void CreateAnonymousWillInvokeResidueCollector()
        {
            // Arrange
            bool resolveWasInvoked = false;

            var residueCollector = new DelegatingSpecimenBuilder();
            residueCollector.OnCreate = (r, c) =>
            {
                resolveWasInvoked = true;
                return new ConcreteType();
            };

            var sut = new Fixture();
            sut.ResidueCollectors.Add(residueCollector);
            // Act
            sut.Create<PropertyHolder<AbstractType>>();
            // Assert
            Assert.True(resolveWasInvoked, "Resolver");
        }

        [Fact]
        public void CreateAnonymousOnUnregisteredAbstractionWillInvokeResidueCollectorWithCorrectType()
        {
            // Arrange
            var residueCollector = new DelegatingSpecimenBuilder();
            residueCollector.OnCreate = (r, c) =>
            {
                Assert.Equal(typeof(AbstractType), r);
                return new ConcreteType();
            };

            var sut = new Fixture();
            sut.ResidueCollectors.Add(residueCollector);
            // Act
            sut.Create<PropertyHolder<AbstractType>>();
            // Assert (done by callback)
        }

        [Fact]
        public void CreateAnonymousOnUnregisteredAbstractionWillReturnInstanceFromResidueCollector()
        {
            // Arrange
            var expectedValue = new ConcreteType();

            var residueCollector = new DelegatingSpecimenBuilder();
            residueCollector.OnCreate = (r, c) => expectedValue;

            var sut = new Fixture();
            sut.ResidueCollectors.Add(residueCollector);
            // Act
            var result = sut.Create<PropertyHolder<AbstractType>>().Property;
            // Assert
            Assert.Equal<AbstractType>(expectedValue, result);
        }

        [Fact]
        public void FreezeWillCauseCreateAnonymousToKeepReturningTheFrozenInstance()
        {
            // Arrange
            var sut = new Fixture();
            var expectedResult = sut.Freeze<Guid>();
            // Act
            var result = sut.Create<Guid>();
            // Assert
            Assert.Equal<Guid>(expectedResult, result);
        }

        [Fact]
        public void FreezeWillCauseFixtureToKeepReturningTheFrozenInstanceEvenAsPropertyOfOtherType()
        {
            // Arrange
            var sut = new Fixture();
            var expectedResult = sut.Freeze<DateTime>();
            // Act
            var result = sut.Create<PropertyHolder<DateTime>>().Property;
            // Assert
            Assert.Equal<DateTime>(expectedResult, result);
        }

        [Fact]
        public void FreezeWithNullTransformationThrows()
        {
            // Arrange
            var sut = new Fixture();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Freeze((Func<ICustomizationComposer<object>, ISpecimenBuilder>)null));
        }

        [Fact]
        public void FreezeBuiltInstanceWillCauseFixtureToKeepReturningTheFrozenInstance()
        {
            // Arrange
            var sut = new Fixture();
            var frozen = sut.Freeze<DoublePropertyHolder<DateTime, Guid>>(ob => ob.OmitAutoProperties().With(x => x.Property1));
            // Act
            var result = sut.Create<DoublePropertyHolder<DateTime, Guid>>();
            // Assert
            Assert.Equal(frozen.Property1, result.Property1);
            Assert.Equal(frozen.Property2, result.Property2);
        }

        [Fact]
        public void CreateManyWithDoCustomizationWillReturnCorrectResult()
        {
            // Arrange
            var sut = new Fixture();
            sut.Customize<List<string>>(ob => ob.Do(sut.AddManyTo).OmitAutoProperties());
            // Act
            var result = sut.CreateMany<List<string>>();
            // Assert
            Assert.True(result.All(l => l.Count == sut.RepeatCount), "Customize/Do/CreateMany");
        }

        [Fact]
        [Obsolete]
        public void OmitAutoPropertiesFollowedByOptInWillNotSetOtherPropertiesObsolete()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            var result = sut.Build<DoublePropertyHolder<object, object>>()
                .OmitAutoProperties()
                .With(x => x.Property1)
                .CreateAnonymous();
            // Assert
            Assert.Null(result.Property2);
        }

        [Fact]
        public void OmitAutoPropertiesFollowedByOptInWillNotSetOtherProperties()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            var result = sut.Build<DoublePropertyHolder<object, object>>()
                .OmitAutoProperties()
                .With(x => x.Property1)
                .Create();
            // Assert
            Assert.Null(result.Property2);
        }

        [Fact]
        [Obsolete]
        public void OmitAutoPropertiesFollowedByTwoOptInsWillNotSetAnyOtherPropertiesObsolete()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            var result = sut.Build<TriplePropertyHolder<int, int, object>>()
                .OmitAutoProperties()
                .With(x => x.Property1, 42)
                .With(x => x.Property2, 1337)
                .CreateAnonymous();
            // Assert
            Assert.Equal(42, result.Property1);
            Assert.Equal(1337, result.Property2);
            Assert.Null(result.Property3);
        }

        [Fact]
        public void OmitAutoPropertiesFollowedByTwoOptInsWillNotSetAnyOtherProperties()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            var result = sut.Build<TriplePropertyHolder<int, int, object>>()
                .OmitAutoProperties()
                .With(x => x.Property1, 42)
                .With(x => x.Property2, 1337)
                .Create();
            // Assert
            Assert.Equal(42, result.Property1);
            Assert.Equal(1337, result.Property2);
            Assert.Null(result.Property3);
        }

        [Fact]
        [Obsolete]
        public void WithTwoOptInsFollowedByOmitAutoPropertiesWillNotSetAnyOtherPropertiesObsolete()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            var result = sut.Build<TriplePropertyHolder<int, int, object>>()
                .With(x => x.Property1, 42)
                .With(x => x.Property2, 1337)
                .OmitAutoProperties()
                .CreateAnonymous();
            // Assert
            Assert.Equal(42, result.Property1);
            Assert.Equal(1337, result.Property2);
            Assert.Null(result.Property3);
        }

        [Fact]
        public void WithTwoOptInsFollowedByOmitAutoPropertiesWillNotSetAnyOtherProperties()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            var result = sut.Build<TriplePropertyHolder<int, int, object>>()
                .With(x => x.Property1, 42)
                .With(x => x.Property2, 1337)
                .OmitAutoProperties()
                .Create();
            // Assert
            Assert.Equal(42, result.Property1);
            Assert.Equal(1337, result.Property2);
            Assert.Null(result.Property3);
        }

        [Fact]
        public void CreateAnonymousWillThrowOnReferenceRecursionPoint()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            Assert.ThrowsAny<ObjectCreationException>(() =>
                sut.Create<RecursionTestObjectWithReferenceOutA>());
        }

        [Fact]
        public void CreateAnonymousWillThrowOnConstructorRecursionPoint()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            Assert.ThrowsAny<ObjectCreationException>(() =>
                sut.Create<RecursionTestObjectWithConstructorReferenceOutA>());
        }

        [Fact]
        [Obsolete]
        public void BuildWithThrowingRecursionHandlerWillThrowOnReferenceRecursionPointObsolete()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            Assert.ThrowsAny<ObjectCreationException>(() =>
                new SpecimenContext(
                    new ThrowingRecursionGuard(
                        sut.Build<RecursionTestObjectWithReferenceOutA>()))
                .CreateAnonymous<RecursionTestObjectWithReferenceOutA>());
        }

        [Fact]
        public void BuildWithThrowingRecursionHandlerWillThrowOnReferenceRecursionPoint()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            Assert.ThrowsAny<ObjectCreationException>(() =>
                new SpecimenContext(
                    new RecursionGuard(
                        sut.Build<RecursionTestObjectWithReferenceOutA>(),
                        new ThrowingRecursionHandler()))
                .Create<RecursionTestObjectWithReferenceOutA>());
        }

        [Fact]
        [Obsolete]
        public void BuildWithThrowingRecursionHandlerWillThrowOnConstructorRecursionPointObsolete()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            Assert.ThrowsAny<ObjectCreationException>(() =>
                new SpecimenContext(
                    new ThrowingRecursionGuard(
                        sut.Build<RecursionTestObjectWithConstructorReferenceOutA>()))
                .CreateAnonymous<RecursionTestObjectWithConstructorReferenceOutA>());
        }

        [Fact]
        public void BuildWithThrowingRecursionHandlerWillThrowOnConstructorRecursionPoint()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            Assert.ThrowsAny<ObjectCreationException>(() =>
                new SpecimenContext(
                    new RecursionGuard(
                        sut.Build<RecursionTestObjectWithConstructorReferenceOutA>(),
                        new ThrowingRecursionHandler()))
                .Create<RecursionTestObjectWithConstructorReferenceOutA>());
        }

        [Fact]
        [Obsolete]
        public void BuildWithNullRecursionHandlerWillCreateNullOnRecursionPointObsolete()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            var result = new SpecimenContext(
                new NullRecursionGuard(
                    sut.Build<RecursionTestObjectWithConstructorReferenceOutA>()))
            .CreateAnonymous<RecursionTestObjectWithConstructorReferenceOutA>();
            // Assert
            Assert.Null(result.ReferenceToB.ReferenceToA);
        }

        [Fact]
        public void BuildWithNullRecursionHandlerWillCreateNullOnRecursionPoint()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            var result = new SpecimenContext(
                new RecursionGuard(
                    sut.Build<RecursionTestObjectWithConstructorReferenceOutA>(),
                    new NullRecursionHandler()))
            .Create<RecursionTestObjectWithConstructorReferenceOutA>();
            // Assert
            Assert.Null(result.ReferenceToB.ReferenceToA);
        }

        [Fact]
        public void BuildWithOmitRecursionGuardWillOmitPropertyOnRecursionPoint()
        {
            // Arrange
            var sut = new Fixture();
            sut.Behaviors.Clear();
            sut.Behaviors.Add(new OmitOnRecursionBehavior());
            // Act
            var actual = sut.Create<RecursionTestObjectWithReferenceOutA>();
            // Assert
            Assert.Null(actual.ReferenceToB.ReferenceToA);
        }

        [Fact]
        public void CreateAnonymousOnRegisteredInstanceWillReturnInstanceWithoutAutoProperties()
        {
            // Arrange
            var item = new PropertyHolder<string>();
            var sut = new Fixture();
            // Act
            sut.Inject(item);
            // Assert
            var result = sut.Create<PropertyHolder<string>>();
            Assert.Null(result.Property);
        }

        [Fact]
        public void CreateAnonymousOnRegisteredParameterlessFuncWillReturnInstanceWithoutAutoProperties()
        {
            // Arrange
            var item = new PropertyHolder<string>();
            var sut = new Fixture();
            // Act
            sut.Register(() => item);
            // Assert
            var result = sut.Create<PropertyHolder<string>>();
            Assert.Null(result.Property);
        }

        [Fact]
        public void CreateAnonymousOnRegisteredSingleParameterFuncWillReturnInstanceWithoutAutoProperties()
        {
            // Arrange
            var item = new PropertyHolder<string>();
            var sut = new Fixture();
            // Act
            sut.Register((object obj) => item);
            // Assert
            var result = sut.Create<PropertyHolder<string>>();
            Assert.Null(result.Property);
        }

        [Fact]
        public void CreateAnonymousOnRegisteredDoubleParameterFuncWillReturnInstanceWithoutAutoProperties()
        {
            // Arrange
            var item = new PropertyHolder<string>();
            var sut = new Fixture();
            // Act
            sut.Register((object obj1, object obj2) => item);
            // Assert
            var result = sut.Create<PropertyHolder<string>>();
            Assert.Null(result.Property);
        }

        [Fact]
        public void CreateAnonymousOnRegisteredTripleParameterFuncWillReturnInstanceWithoutAutoProperties()
        {
            // Arrange
            var item = new PropertyHolder<string>();
            var sut = new Fixture();
            // Act
            sut.Register((object obj1, object obj2, object obj3) => item);
            // Assert
            var result = sut.Create<PropertyHolder<string>>();
            Assert.Null(result.Property);
        }

        [Fact]
        public void CreateAnonymousOnRegisteredQuadrupleParameterFuncWillReturnInstanceWithoutAutoProperties()
        {
            // Arrange
            var item = new PropertyHolder<string>();
            var sut = new Fixture();
            // Act
            sut.Register((object obj1, object obj2, object obj3, object obj4) => item);
            // Assert
            var result = sut.Create<PropertyHolder<string>>();
            Assert.Null(result.Property);
        }

        [Fact]
        public void CreateAnonymousWithOmitAutoPropertiesWillNotAssignProperty()
        {
            // Arrange
            Fixture sut = new Fixture() { OmitAutoProperties = true };
            // Act
            PropertyHolder<string> result = sut.Create<PropertyHolder<string>>();
            // Assert
            Assert.Null(result.Property);
        }

        [Fact]
        [Obsolete]
        public void CustomizeInstanceWithOmitAutoPropertiesWillReturnFactoryWithOmitAutoPropertiesObsolete()
        {
            // Arrange
            var sut = new Fixture() { OmitAutoProperties = true };
            // Act
            var builder = sut.Build<PropertyHolder<object>>();
            PropertyHolder<object> result = builder.CreateAnonymous();
            // Assert
            Assert.Null(result.Property);
        }

        [Fact]
        public void CustomizeInstanceWithOmitAutoPropertiesWillReturnFactoryWithOmitAutoProperties()
        {
            // Arrange
            var sut = new Fixture() { OmitAutoProperties = true };
            // Act
            var builder = sut.Build<PropertyHolder<object>>();
            PropertyHolder<object> result = builder.Create();
            // Assert
            Assert.Null(result.Property);
        }

        [Fact]
        public void FreezedFirstCallToCreateAnonymousWithOmitAutoPropertiesWillNotAssignProperty()
        {
            // Arrange
            var sut = new Fixture() { OmitAutoProperties = true };
            // Act
            var expectedResult = sut.Freeze<PropertyHolder<string>>();
            // Assert
            Assert.Null(expectedResult.Property);
        }

        [Fact]
        public void CustomizedBuilderCreateAnonymousWithOmitAutoPropertiesWillNotAssignProperty()
        {
            // Arrange
            var sut = new Fixture() { OmitAutoProperties = true };
            // Act
            sut.Customize<PropertyHolder<string>>(x => x);
            var expectedResult = sut.Create<PropertyHolder<string>>();
            // Assert
            Assert.Null(expectedResult.Property);
        }

        [Fact]
        public void CustomizedOverrideOfOmitAutoPropertiesWillAssignProperty()
        {
            // Arrange
            var sut = new Fixture() { OmitAutoProperties = true };
            // Act
            sut.Customize<PropertyHolder<string>>(x => x.WithAutoProperties());
            var expectedResult = sut.Create<PropertyHolder<string>>();
            // Assert
            Assert.NotNull(expectedResult.Property);
        }

        [Fact]
        public void DefaultOmitAutoPropertiesIsFalse()
        {
            // Arrange
            Fixture sut = new Fixture();
            // Act
            bool result = sut.OmitAutoProperties;
            // Assert
            Assert.False(result, "OmitAutoProperties");
        }

        [Fact]
        public void FromSeedWithNullFuncThrows()
        {
            // Arrange
            var sut = new Fixture();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Build<object>().FromSeed(null));
        }

        [Fact]
        [Obsolete]
        public void BuildFromSeedWillReturnCorrectResultObsolete()
        {
            // Arrange
            var sut = new Fixture();
            var expectedResult = new object();
            // Act
            var result = sut.Build<object>().FromSeed(s => expectedResult).CreateAnonymous();
            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void BuildFromSeedWillReturnCorrectResult()
        {
            // Arrange
            var sut = new Fixture();
            var expectedResult = new object();
            // Act
            var result = sut.Build<object>().FromSeed(s => expectedResult).Create();
            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CustomizeFromSeedWithUnmodifiedSeedValueWillPopulatePropertyOfSameType()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            fixture.Customize<Version>(c => c.FromSeed(s => s));
            // Assert
            Assert.Null(fixture.Create<PropertyHolder<Version>>().Property);
        }

        [Fact]
        public void CustomizeFromSeedWithFixedSeedValueWillPopulatePropertyOfSameType()
        {
            // Arrange
            var fixture = new Fixture();
            var seed = new ConcreteType();
            // Act
            fixture.Customize<ConcreteType>(c => c.FromSeed(s => seed));
            // Assert
            Assert.Equal(seed, fixture.Create<PropertyHolder<ConcreteType>>().Property);
        }

        [Fact]
        [Obsolete]
        public void BuildAndCreateAnonymousWillSetInt32Property()
        {
            // Arrange
            int unexpectedNumber = default(int);
            var sut = new Fixture();
            // Act
            PropertyHolder<int> result = sut.Build<PropertyHolder<int>>().CreateAnonymous();
            // Assert
            Assert.NotEqual<int>(unexpectedNumber, result.Property);
        }

        [Fact]
        public void BuildAndCreateWillSetInt32Property()
        {
            // Arrange
            int unexpectedNumber = default(int);
            var sut = new Fixture();
            // Act
            PropertyHolder<int> result = sut.Build<PropertyHolder<int>>().Create();
            // Assert
            Assert.NotEqual<int>(unexpectedNumber, result.Property);
        }

        [Fact]
        [Obsolete]
        public void BuildAndCreateAnonymousWillSetInt32Field()
        {
            // Arrange
            int unexpectedNumber = default(int);
            var sut = new Fixture();
            // Act
            FieldHolder<int> result = sut.Build<FieldHolder<int>>().CreateAnonymous();
            // Assert
            Assert.NotEqual<int>(unexpectedNumber, result.Field);
        }

        [Fact]
        public void BuildAndCreateWillSetInt32Field()
        {
            // Arrange
            int unexpectedNumber = default(int);
            var sut = new Fixture();
            // Act
            FieldHolder<int> result = sut.Build<FieldHolder<int>>().Create();
            // Assert
            Assert.NotEqual<int>(unexpectedNumber, result.Field);
        }

        [Fact]
        [Obsolete]
        public void BuildAndCreateAnonymousWillNotAttemptToSetReadOnlyProperty()
        {
            // Arrange
            int expectedNumber = default(int);
            var sut = new Fixture();
            // Act
            ReadOnlyPropertyHolder<int> result = sut.Build<ReadOnlyPropertyHolder<int>>().CreateAnonymous();
            // Assert
            Assert.Equal<int>(expectedNumber, result.Property);
        }

        [Fact]
        public void BuildAndCreateWillNotAttemptToSetReadOnlyProperty()
        {
            // Arrange
            int expectedNumber = default(int);
            var sut = new Fixture();
            // Act
            ReadOnlyPropertyHolder<int> result = sut.Build<ReadOnlyPropertyHolder<int>>().Create();
            // Assert
            Assert.Equal<int>(expectedNumber, result.Property);
        }

        [Fact]
        [Obsolete]
        public void BuildAndCreateAnonymousWillNotAttemptToSetReadOnlyField()
        {
            // Arrange
            int expectedNumber = default(int);
            var sut = new Fixture();
            // Act
            ReadOnlyFieldHolder<int> result = sut.Build<ReadOnlyFieldHolder<int>>().CreateAnonymous();
            // Assert
            Assert.Equal<int>(expectedNumber, result.Field);
        }

        [Fact]
        public void BuildAndCreateWillNotAttemptToSetReadOnlyField()
        {
            // Arrange
            int expectedNumber = default(int);
            var sut = new Fixture();
            // Act
            ReadOnlyFieldHolder<int> result = sut.Build<ReadOnlyFieldHolder<int>>().Create();
            // Assert
            Assert.Equal<int>(expectedNumber, result.Field);
        }

        [Fact]
        [Obsolete]
        public void BuildWithWillSetPropertyOnCreatedObjectObsolete()
        {
            // Arrange
            string expectedText = "Anonymous text";
            var sut = new Fixture();
            // Act
            PropertyHolder<string> result = sut.Build<PropertyHolder<string>>()
                .With(ph => ph.Property, expectedText)
                .CreateAnonymous();
            // Assert
            Assert.Equal(expectedText, result.Property);
        }

        [Fact]
        public void BuildWithWillSetPropertyOnCreatedObject()
        {
            // Arrange
            string expectedText = "Anonymous text";
            var sut = new Fixture();
            // Act
            PropertyHolder<string> result = sut.Build<PropertyHolder<string>>()
                .With(ph => ph.Property, expectedText)
                .Create();
            // Assert
            Assert.Equal(expectedText, result.Property);
        }

        [Fact]
        [Obsolete]
        public void BuildWithWillSetFieldOnCreatedObjectObsolete()
        {
            // Arrange
            string expectedText = "Anonymous text";
            var fixture = new Fixture();
            // Act
            FieldHolder<string> result = fixture.Build<FieldHolder<string>>()
                .With(fh => fh.Field, expectedText)
                .CreateAnonymous();
            // Assert
            Assert.Equal(expectedText, result.Field);
        }

        [Fact]
        public void BuildWithWillSetFieldOnCreatedObject()
        {
            // Arrange
            string expectedText = "Anonymous text";
            var fixture = new Fixture();
            // Act
            FieldHolder<string> result = fixture
                .Build<FieldHolder<string>>()
                .With(fh => fh.Field, expectedText)
                .Create();
            // Assert
            Assert.Equal(expectedText, result.Field);
        }

        [Fact]
        public void BuildWithFactoryWillSetPropertyOnCreatedObject()
        {
            // Arrange
            var values = new Queue<string>(new[] { "value1", "value2" });
            var fixture = new Fixture();
            // Act
            var builder = fixture
                .Build<PropertyHolder<string>>()
                .With(ph => ph.Property, () => values.Dequeue());
            var result1 = builder.Create();
            var result2 = builder.Create();
            // Assert
            Assert.Equal("value1", result1.Property);
            Assert.Equal("value2", result2.Property);
        }

        [Fact]
        public void BuildWithFactoryWillSetFieldOnCreatedObject()
        {
            // Arrange
            var values = new Queue<string>(new[] { "value1", "value2" });
            var fixture = new Fixture();
            // Act
            var builder = fixture
                .Build<FieldHolder<string>>()
                .With(ph => ph.Field, () => values.Dequeue());
            var result1 = builder.Create();
            var result2 = builder.Create();
            // Assert
            Assert.Equal("value1", result1.Field);
            Assert.Equal("value2", result2.Field);
        }

        [Fact]
        public void BuildWithSingleArgFactoryWillSetPropertyOnCreatedObject()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Inject<Queue<string>>(new Queue<string>(new[] { "value1", "value2" }));
            // Act
            var builder = fixture
                .Build<PropertyHolder<string>>()
                .With(ph => ph.Property, (Queue<string> values) => values.Dequeue());
            var result1 = builder.Create();
            var result2 = builder.Create();
            // Assert
            Assert.Equal("value1", result1.Property);
            Assert.Equal("value2", result2.Property);
        }

        [Fact]
        public void BuildWithSingleArgFactoryWillSetFieldOnCreatedObject()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Inject<Queue<string>>(new Queue<string>(new[] { "value1", "value2" }));
            // Act
            var builder = fixture
                .Build<FieldHolder<string>>()
                .With(ph => ph.Field, (Queue<string> values) => values.Dequeue());
            var result1 = builder.Create();
            var result2 = builder.Create();
            // Assert
            Assert.Equal("value1", result1.Field);
            Assert.Equal("value2", result2.Field);
        }

        [Fact]
        [Obsolete]
        public void BuildAnonymousWithWillAssignPropertyEvenInCombinationWithOmitAutoPropertiesObsolete()
        {
            // Arrange
            long unexpectedNumber = default(long);
            var sut = new Fixture();
            // Act
            var result = sut
                .Build<DoublePropertyHolder<long, long>>()
                .With(ph => ph.Property1)
                .OmitAutoProperties()
                .CreateAnonymous();
            // Assert
            Assert.NotEqual<long>(unexpectedNumber, result.Property1);
        }

        [Fact]
        public void BuildAnonymousWithWillAssignPropertyEvenInCombinationWithOmitAutoProperties()
        {
            // Arrange
            long unexpectedNumber = default(long);
            var sut = new Fixture();
            // Act
            var result = sut
                .Build<DoublePropertyHolder<long, long>>()
                .With(ph => ph.Property1)
                .OmitAutoProperties()
                .Create();
            // Assert
            Assert.NotEqual<long>(unexpectedNumber, result.Property1);
        }

        [Fact]
        [Obsolete]
        public void BuildAnonymousWithWillAssignFieldEvenInCombinationWithOmitAutoProperties()
        {
            // Arrange
            int unexpectedNumber = default(int);
            var sut = new Fixture();
            // Act
            var result = sut
                .Build<DoubleFieldHolder<int, decimal>>()
                .With(fh => fh.Field1)
                .OmitAutoProperties()
                .CreateAnonymous();
            // Assert
            Assert.NotEqual<int>(unexpectedNumber, result.Field1);
        }

        [Fact]
        public void BuildWithWillAssignFieldEvenInCombinationWithOmitAutoProperties()
        {
            // Arrange
            int unexpectedNumber = default(int);
            var sut = new Fixture();
            // Act
            var result = sut
                .Build<DoubleFieldHolder<int, decimal>>()
                .With(fh => fh.Field1)
                .OmitAutoProperties()
                .Create();
            // Assert
            Assert.NotEqual<int>(unexpectedNumber, result.Field1);
        }

        [Fact]
        [Obsolete]
        public void BuildWithoutWillIgnorePropertyOnCreatedObjectObsolete()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            var result = sut.Build<DoublePropertyHolder<string, string>>().Without(ph => ph.Property1).CreateAnonymous();
            // Assert
            Assert.Null(result.Property1);
        }

        [Fact]
        public void BuildWithoutWillIgnorePropertyOnCreatedObject()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            var result = sut.Build<DoublePropertyHolder<string, string>>().Without(ph => ph.Property1).Create();
            // Assert
            Assert.Null(result.Property1);
        }

        [Fact]
        [Obsolete]
        public void BuildWithoutWillIgnorePropertyOnCreatedObjectEvenInCombinationWithWithAutoPropertiesObsolete()
        {
            // Arrange
            var sut = new Fixture() { OmitAutoProperties = true };
            // Act
            var result = sut.Build<DoublePropertyHolder<string, string>>().WithAutoProperties().Without(ph => ph.Property1).CreateAnonymous();
            // Assert
            Assert.Null(result.Property1);
        }

        [Fact]
        public void BuildWithoutWillIgnorePropertyOnCreatedObjectEvenInCombinationWithWithAutoProperties()
        {
            // Arrange
            var sut = new Fixture() { OmitAutoProperties = true };
            // Act
            var result = sut.Build<DoublePropertyHolder<string, string>>().WithAutoProperties().Without(ph => ph.Property1).Create();
            // Assert
            Assert.Null(result.Property1);
        }

        [Fact]
        [Obsolete]
        public void BuildWithoutWillIgnoreFieldOnCreatedObjectObsolete()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            var result = sut.Build<DoubleFieldHolder<string, string>>().Without(fh => fh.Field1).CreateAnonymous();
            // Assert
            Assert.Null(result.Field1);
        }

        [Fact]
        public void BuildWithoutWillIgnoreFieldOnCreatedObject()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            var result = sut.Build<DoubleFieldHolder<string, string>>().Without(fh => fh.Field1).Create();
            // Assert
            Assert.Null(result.Field1);
        }

        [Fact]
        [Obsolete]
        public void BuildWithoutWillNotIgnoreOtherPropertyOnCreatedObjectObsolete()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            var result = sut.Build<DoublePropertyHolder<string, string>>().Without(ph => ph.Property1).CreateAnonymous();
            // Assert
            Assert.NotNull(result.Property2);
        }

        [Fact]
        public void BuildWithoutWillNotIgnoreOtherPropertyOnCreatedObject()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            var result = sut.Build<DoublePropertyHolder<string, string>>().Without(ph => ph.Property1).Create();
            // Assert
            Assert.NotNull(result.Property2);
        }

        [Fact]
        [Obsolete]
        public void BuildWithoutWillNotIgnoreOtherFieldOnCreatedObjectObsolete()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            var result = sut.Build<DoubleFieldHolder<string, string>>().Without(fh => fh.Field1).CreateAnonymous();
            // Assert
            Assert.NotNull(result.Field2);
        }

        [Fact]
        public void BuildWithoutWillNotIgnoreOtherFieldOnCreatedObject()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            var result = sut.Build<DoubleFieldHolder<string, string>>().Without(fh => fh.Field1).Create();
            // Assert
            Assert.NotNull(result.Field2);
        }

        [Fact]
        [Obsolete]
        public void BuildAndOmitAutoPropertiesWillNotAutoPopulatePropertyObsolete()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            PropertyHolder<object> result = sut.Build<PropertyHolder<object>>().OmitAutoProperties().CreateAnonymous();
            // Assert
            Assert.Null(result.Property);
        }

        [Fact]
        public void BuildAndOmitAutoPropertiesWillNotAutoPopulateProperty()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            PropertyHolder<object> result = sut.Build<PropertyHolder<object>>().OmitAutoProperties().Create();
            // Assert
            Assert.Null(result.Property);
        }

        [Fact]
        [Obsolete]
        public void BuildWithAutoPropertiesWillAutoPopulatePropertyObsolete()
        {
            // Arrange
            var sut = new Fixture { OmitAutoProperties = true };
            // Act
            PropertyHolder<object> result = sut.Build<PropertyHolder<object>>().WithAutoProperties().CreateAnonymous();
            // Assert
            Assert.NotNull(result.Property);
        }

        [Fact]
        public void BuildWithAutoPropertiesWillAutoPopulateProperty()
        {
            // Arrange
            var sut = new Fixture { OmitAutoProperties = true };
            // Act
            PropertyHolder<object> result = sut.Build<PropertyHolder<object>>().WithAutoProperties().Create();
            // Assert
            Assert.NotNull(result.Property);
        }

        [Fact]
        [Obsolete]
        public void BuildAndDoWillPerformOperationOnCreatedObjectObsolete()
        {
            // Arrange
            var sut = new Fixture();
            var expectedObject = new object();
            // Act
            var result = sut.Build<CollectionHolder<object>>().Do(x => x.Collection.Add(expectedObject)).CreateAnonymous().Collection.First();
            // Assert
            Assert.Equal<object>(expectedObject, result);
        }

        [Fact]
        public void BuildAndDoWillPerformOperationOnCreatedObject()
        {
            // Arrange
            var sut = new Fixture();
            var expectedObject = new object();
            // Act
            var result = sut.Build<CollectionHolder<object>>().Do(x => x.Collection.Add(expectedObject)).Create().Collection.First();
            // Assert
            Assert.Equal<object>(expectedObject, result);
        }

        [Fact]
        [Obsolete]
        public void BuilderSequenceWillBePreservedObsolete()
        {
            // Arrange
            var sut = new Fixture();
            int expectedValue = 3;
            // Act
            var result = sut.Build<PropertyHolder<int>>()
                .With(x => x.Property, 1)
                .Do(x => x.SetProperty(2))
                .With(x => x.Property, expectedValue)
                .CreateAnonymous();
            // Assert
            Assert.Equal<int>(expectedValue, result.Property);
        }

        [Fact]
        public void BuilderSequenceWillBePreserved()
        {
            // Arrange
            var sut = new Fixture();
            int expectedValue = 3;
            // Act
            var result = sut.Build<PropertyHolder<int>>()
                .With(x => x.Property, 1)
                .Do(x => x.SetProperty(2))
                .With(x => x.Property, expectedValue)
                .Create();
            // Assert
            Assert.Equal<int>(expectedValue, result.Property);
        }

        [Fact]
        [Obsolete]
        public void BuildAndCreateAnonymousWillInvokeResidueCollector()
        {
            // Arrange
            bool resolveWasInvoked = false;

            var residueCollector = new DelegatingSpecimenBuilder();
            residueCollector.OnCreate = (r, c) =>
            {
                resolveWasInvoked = true;
                return new ConcreteType();
            };

            var sut = new Fixture();
            sut.ResidueCollectors.Add(residueCollector);
            // Act
            sut.Build<PropertyHolder<AbstractType>>().CreateAnonymous();
            // Assert
            Assert.True(resolveWasInvoked, "Resolve");
        }

        [Fact]
        public void BuildAndCreateWillInvokeResidueCollector()
        {
            // Arrange
            bool resolveWasInvoked = false;

            var residueCollector = new DelegatingSpecimenBuilder();
            residueCollector.OnCreate = (r, c) =>
            {
                resolveWasInvoked = true;
                return new ConcreteType();
            };

            var sut = new Fixture();
            sut.ResidueCollectors.Add(residueCollector);
            // Act
            sut.Build<PropertyHolder<AbstractType>>().Create();
            // Assert
            Assert.True(resolveWasInvoked, "Resolve");
        }

        [Fact]
        [Obsolete]
        public void BuildAndCreateAnonymousOnUnregisteredAbstractionWillInvokeResidueCollectorWithCorrectType()
        {
            // Arrange
            var residueCollector = new DelegatingSpecimenBuilder();
            residueCollector.OnCreate = (r, c) =>
            {
                Assert.Equal(typeof(AbstractType), r);
                return new ConcreteType();
            };

            var sut = new Fixture();
            sut.ResidueCollectors.Add(residueCollector);
            // Act
            sut.Build<PropertyHolder<AbstractType>>().CreateAnonymous();
            // Assert (done by callback)
        }

        [Fact]
        public void BuildAndCreateOnUnregisteredAbstractionWillInvokeResidueCollectorWithCorrectType()
        {
            // Arrange
            var residueCollector = new DelegatingSpecimenBuilder();
            residueCollector.OnCreate = (r, c) =>
            {
                Assert.Equal(typeof(AbstractType), r);
                return new ConcreteType();
            };

            var sut = new Fixture();
            sut.ResidueCollectors.Add(residueCollector);
            // Act
            sut.Build<PropertyHolder<AbstractType>>().Create();
            // Assert (done by callback)
        }

        [Fact]
        [Obsolete]
        public void BuildAndCreateAnonymousOnUnregisteredAbstractionWillReturnInstanceFromResidueCollector()
        {
            // Arrange
            var expectedValue = new ConcreteType();

            var residueCollector = new DelegatingSpecimenBuilder();
            residueCollector.OnCreate = (r, c) => expectedValue;

            var sut = new Fixture();
            sut.ResidueCollectors.Add(residueCollector);
            // Act
            var result = sut.Build<PropertyHolder<AbstractType>>().CreateAnonymous().Property;
            // Assert
            Assert.Equal<AbstractType>(expectedValue, result);
        }

        [Fact]
        public void BuildAndCreateOnUnregisteredAbstractionWillReturnInstanceFromResidueCollector()
        {
            // Arrange
            var expectedValue = new ConcreteType();

            var residueCollector = new DelegatingSpecimenBuilder();
            residueCollector.OnCreate = (r, c) => expectedValue;

            var sut = new Fixture();
            sut.ResidueCollectors.Add(residueCollector);
            // Act
            var result = sut.Build<PropertyHolder<AbstractType>>().Create().Property;
            // Assert
            Assert.Equal<AbstractType>(expectedValue, result);
        }

        [Fact]
        [Obsolete]
        public void BuildAndOmitAutoPropertiesWillNotMutateSutObsolete()
        {
            // Arrange
            var fixture = new Fixture();
            var sut = fixture.Build<PropertyHolder<string>>();
            // Act
            sut.OmitAutoProperties();
            // Assert
            var instance = sut.CreateAnonymous();
            Assert.NotNull(instance.Property);
        }

        [Fact]
        public void BuildAndOmitAutoPropertiesWillNotMutateSut()
        {
            // Arrange
            var fixture = new Fixture();
            var sut = fixture.Build<PropertyHolder<string>>();
            // Act
            sut.OmitAutoProperties();
            // Assert
            var instance = sut.Create();
            Assert.NotNull(instance.Property);
        }

        [Fact]
        [Obsolete]
        public void BuildWithAutoPropertiesWillNotMutateSutObsolete()
        {
            // Arrange
            var fixture = new Fixture() { OmitAutoProperties = true };
            var sut = fixture.Build<PropertyHolder<string>>();
            // Act
            sut.WithAutoProperties();
            // Assert
            var instance = sut.CreateAnonymous();
            Assert.Null(instance.Property);
        }

        [Fact]
        public void BuildWithAutoPropertiesWillNotMutateSut()
        {
            // Arrange
            var fixture = new Fixture() { OmitAutoProperties = true };
            var sut = fixture.Build<PropertyHolder<string>>();
            // Act
            sut.WithAutoProperties();
            // Assert
            var instance = sut.Create();
            Assert.Null(instance.Property);
        }

        [Fact]
        [Obsolete]
        public void BuildAnonymousWithWillNotMutateSut()
        {
            // Arrange
            var fixture = new Fixture();
            var sut = fixture.Build<PropertyHolder<string>>().OmitAutoProperties();
            // Act
            sut.With(s => s.Property);
            // Assert
            var instance = sut.CreateAnonymous();
            Assert.Null(instance.Property);
        }

        [Fact]
        public void BuildWithWillNotMutateSut()
        {
            // Arrange
            var fixture = new Fixture();
            var sut = fixture.Build<PropertyHolder<string>>().OmitAutoProperties();
            // Act
            sut.With(s => s.Property);
            // Assert
            var instance = sut.Create();
            Assert.Null(instance.Property);
        }

        [Fact]
        [Obsolete]
        public void BuildAnonymousWithUnexpectedWillNotMutateSut()
        {
            // Arrange
            var fixture = new Fixture();
            var unexpectedProperty = "Anonymous value";
            var sut = fixture.Build<PropertyHolder<string>>();
            // Act
            sut.With(s => s.Property, unexpectedProperty);
            // Assert
            var instance = sut.CreateAnonymous();
            Assert.NotEqual(unexpectedProperty, instance.Property);
        }

        [Fact]
        public void BuildWithUnexpectedWillNotMutateSut()
        {
            // Arrange
            var fixture = new Fixture();
            var unexpectedProperty = "Anonymous value";
            var sut = fixture.Build<PropertyHolder<string>>();
            // Act
            sut.With(s => s.Property, unexpectedProperty);
            // Assert
            var instance = sut.Create();
            Assert.NotEqual(unexpectedProperty, instance.Property);
        }

        [Fact]
        [Obsolete]
        public void BuildWithoutWillNotMutateSutObsolete()
        {
            // Arrange
            var fixture = new Fixture();
            var sut = fixture.Build<PropertyHolder<string>>();
            // Act
            sut.Without(s => s.Property);
            // Assert
            var instance = sut.CreateAnonymous();
            Assert.NotNull(instance.Property);
        }

        [Fact]
        public void BuildWithoutWillNotMutateSut()
        {
            // Arrange
            var fixture = new Fixture();
            var sut = fixture.Build<PropertyHolder<string>>();
            // Act
            sut.Without(s => s.Property);
            // Assert
            var instance = sut.Create();
            Assert.NotNull(instance.Property);
        }

        [Fact]
        [Obsolete]
        public void BuildAndCreateAnonymousWillReturnCreatedObject()
        {
            // Arrange
            object expectedObject = new object();
            var sut = new Fixture();
            // Act
            object result = sut.Build<object>().FromSeed(seed => expectedObject).CreateAnonymous();
            // Assert
            Assert.Equal<object>(expectedObject, result);
        }

        [Fact]
        public void BuildAndCreateWillReturnCreatedObject()
        {
            // Arrange
            object expectedObject = new object();
            var sut = new Fixture();
            // Act
            object result = sut.Build<object>().FromSeed(seed => expectedObject).Create();
            // Assert
            Assert.Equal<object>(expectedObject, result);
        }

        [Fact]
        public void BuildAndCreateManyWillCreateManyAnonymousItems()
        {
            // Arrange
            var sut = new Fixture();
            var expectedItemCount = sut.RepeatCount;
            // Act
            IEnumerable<PropertyHolder<int>> result = sut.Build<PropertyHolder<int>>().CreateMany();
            // Assert
            var uniqueItemCount = (from ph in result
                                   select ph.Property).Distinct().Count();
            Assert.Equal<int>(expectedItemCount, uniqueItemCount);
        }

        [Fact]
        public void BuildAndCreateManyWillCreateCorrectNumberOfItems()
        {
            // Arrange
            int expectedCount = 401;
            var sut = new Fixture();
            // Act
            IEnumerable<PropertyHolder<int>> result = sut.Build<PropertyHolder<int>>().CreateMany(expectedCount);
            // Assert
            var uniqueItemCount = (from ph in result
                                   select ph.Property).Distinct().Count();
            Assert.Equal<int>(expectedCount, uniqueItemCount);
        }

        [Fact]
        [Obsolete]
        public void BuildAndCreateAnonymousWillCreateObject()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            object result = sut.Build<object>().CreateAnonymous();
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void BuildAndCreateWillCreateObject()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            object result = sut.Build<object>().Create();
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        [Obsolete]
        public void BuildAndCreateAnonymousAfterDefiningConstructorWithZeroParametersWillReturnDefinedObject()
        {
            // Arrange
            var sut = new Fixture();
            object expectedObject = new object();
            // Act
            var result = sut.Build<object>()
                .FromFactory(() => expectedObject)
                .CreateAnonymous();
            // Assert
            Assert.Equal<object>(expectedObject, result);
        }

        [Fact]
        public void BuildAndCreateAfterDefiningConstructorWithZeroParametersWillReturnDefinedObject()
        {
            // Arrange
            var sut = new Fixture();
            object expectedObject = new object();
            // Act
            var result = sut.Build<object>()
                .FromFactory(() => expectedObject)
                .Create();
            // Assert
            Assert.Equal<object>(expectedObject, result);
        }

        [Fact]
        [Obsolete]
        public void BuildAndCreateAnonymousAfterDefiningConstructorWithOneParameterWillReturnDefinedObject()
        {
            // Arrange
            var sut = new Fixture();
            SingleParameterType<object> expectedObject = new SingleParameterType<object>(new object());
            // Act
            var result = sut.Build<SingleParameterType<object>>()
                .FromFactory<object>(obj => expectedObject)
                .CreateAnonymous();
            // Assert
            Assert.Equal<SingleParameterType<object>>(expectedObject, result);
        }

        [Fact]
        public void BuildAndCreateAfterDefiningConstructorWithOneParameterWillReturnDefinedObject()
        {
            // Arrange
            var sut = new Fixture();
            SingleParameterType<object> expectedObject = new SingleParameterType<object>(new object());
            // Act
            var result = sut.Build<SingleParameterType<object>>()
                .FromFactory<object>(obj => expectedObject)
                .Create();
            // Assert
            Assert.Equal<SingleParameterType<object>>(expectedObject, result);
        }

        [Fact]
        [Obsolete]
        public void BuildAndCreateAnonymousAfterDefiningConstructorWithTwoParametersWillReturnDefinedObject()
        {
            // Arrange
            var sut = new Fixture();
            DoubleParameterType<object, object> expectedObject = new DoubleParameterType<object, object>(new object(), new object());
            // Act
            var result = sut.Build<DoubleParameterType<object, object>>()
                .FromFactory<object, object>((o1, o2) => expectedObject)
                .CreateAnonymous();
            // Assert
            Assert.Equal<DoubleParameterType<object, object>>(expectedObject, result);
        }

        [Fact]
        public void BuildAndCreateAfterDefiningConstructorWithTwoParametersWillReturnDefinedObject()
        {
            // Arrange
            var sut = new Fixture();
            DoubleParameterType<object, object> expectedObject = new DoubleParameterType<object, object>(new object(), new object());
            // Act
            var result = sut.Build<DoubleParameterType<object, object>>()
                .FromFactory<object, object>((o1, o2) => expectedObject)
                .Create();
            // Assert
            Assert.Equal<DoubleParameterType<object, object>>(expectedObject, result);
        }

        [Fact]
        [Obsolete]
        public void BuildAndCreateAnonymousAfterDefiningConstructorWithThreeParametersWillReturnDefinedObject()
        {
            // Arrange
            var sut = new Fixture();
            TripleParameterType<object, object, object> expectedObject = new TripleParameterType<object, object, object>(new object(), new object(), new object());
            // Act
            var result = sut.Build<TripleParameterType<object, object, object>>()
                .FromFactory<object, object, object>((o1, o2, o3) => expectedObject)
                .CreateAnonymous();
            // Assert
            Assert.Equal<TripleParameterType<object, object, object>>(expectedObject, result);
        }

        [Fact]
        public void BuildAndCreateAfterDefiningConstructorWithThreeParametersWillReturnDefinedObject()
        {
            // Arrange
            var sut = new Fixture();
            TripleParameterType<object, object, object> expectedObject = new TripleParameterType<object, object, object>(new object(), new object(), new object());
            // Act
            var result = sut.Build<TripleParameterType<object, object, object>>()
                .FromFactory<object, object, object>((o1, o2, o3) => expectedObject)
                .Create();
            // Assert
            Assert.Equal<TripleParameterType<object, object, object>>(expectedObject, result);
        }

        [Fact]
        [Obsolete]
        public void BuildAndCreateAnonymousAfterDefiningConstructorWithFourParametersWillReturnDefinedObject()
        {
            // Arrange
            var sut = new Fixture();
            QuadrupleParameterType<object, object, object, object> expectedObject = new QuadrupleParameterType<object, object, object, object>(new object(), new object(), new object(), new object());
            // Act
            var result = sut.Build<QuadrupleParameterType<object, object, object, object>>()
                .FromFactory<object, object, object, object>((o1, o2, o3, o4) => expectedObject)
                .CreateAnonymous();
            // Assert
            Assert.Equal<QuadrupleParameterType<object, object, object, object>>(expectedObject, result);
        }

        [Fact]
        public void BuildAndCreateAfterDefiningConstructorWithFourParametersWillReturnDefinedObject()
        {
            // Arrange
            var sut = new Fixture();
            QuadrupleParameterType<object, object, object, object> expectedObject = new QuadrupleParameterType<object, object, object, object>(new object(), new object(), new object(), new object());
            // Act
            var result = sut.Build<QuadrupleParameterType<object, object, object, object>>()
                .FromFactory<object, object, object, object>((o1, o2, o3, o4) => expectedObject)
                .Create();
            // Assert
            Assert.Equal<QuadrupleParameterType<object, object, object, object>>(expectedObject, result);
        }

        [Fact]
        [Obsolete]
        public void BuildFromFactoryStillAppliesAutoPropertiesObsolete()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            var result = sut.Build<PropertyHolder<string>>()
                .FromFactory(() => new PropertyHolder<string>())
                .CreateAnonymous();
            // Assert
            Assert.NotNull(result.Property);
        }

        [Fact]
        public void BuildFromFactoryStillAppliesAutoProperties()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            var result = sut.Build<PropertyHolder<string>>()
                .FromFactory(() => new PropertyHolder<string>())
                .Create();
            // Assert
            Assert.NotNull(result.Property);
        }

        [Fact]
        [Obsolete]
        public void BuildOverwritesPreviousFactoryBasedCustomizationObsolete()
        {
            // Arrange
            var sut = new Fixture();
            sut.Customize<PropertyHolder<object>>(c => c.FromFactory(() => new PropertyHolder<object>()));
            // Act
            var result = sut.Build<PropertyHolder<object>>().OmitAutoProperties().CreateAnonymous();
            // Assert
            Assert.Null(result.Property);
        }

        [Fact]
        public void BuildOverwritesPreviousFactoryBasedCustomization()
        {
            // Arrange
            var sut = new Fixture();
            sut.Customize<PropertyHolder<object>>(c => c.FromFactory(() => new PropertyHolder<object>()));
            // Act
            var result = sut.Build<PropertyHolder<object>>().OmitAutoProperties().Create();
            // Assert
            Assert.Null(result.Property);
        }

        [Fact]
        public void NewestCustomizationWins()
        {
            // Arrange
            var sut = new Fixture();
            sut.Customize<string>(c => c.FromFactory(() => "ploeh"));

            var expectedResult = "fnaah";
            // Act
            sut.Customize<string>(c => c.FromFactory(() => expectedResult));
            var result = sut.Create<string>();
            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void BuildAndComposeWillCarryBehaviorsForward()
        {
            // Arrange
            var sut = new Fixture();
            sut.Behaviors.Clear();

            var expectedBuilder = new DelegatingSpecimenBuilder();
            sut.Behaviors.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode(1, b) });
            // Act
            var result = sut.Build<object>();
            // Assert
            var comparer = new TaggedNodeComparer(new TrueComparer<ISpecimenBuilder>());
            var composite = Assert.IsAssignableFrom<CompositeNodeComposer<object>>(result);
            Assert.Equal(new TaggedNode(1), composite.Node.First(), comparer);
        }

        [Fact]
        [Obsolete]
        public void BuildAbstractClassThrowsObsolete()
        {
            // Arrange
            var sut = new Fixture();
            // Act & assert
            Assert.Throws<ObjectCreationException>(() =>
                sut.Build<AbstractType>().CreateAnonymous());
        }

        [Fact]
        public void BuildAbstractClassThrows()
        {
            // Arrange
            var sut = new Fixture();
            // Act & assert
            Assert.Throws<ObjectCreationException>(() =>
                sut.Build<AbstractType>().Create());
        }

        [Fact]
        [Obsolete]
        public void BuildAbstractTypeUsingStronglyTypedFactoryIsPossibleObsolete()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            var result = sut.Build<AbstractType>().FromFactory(() => new ConcreteType()).CreateAnonymous();
            // Assert
            Assert.IsAssignableFrom<ConcreteType>(result);
        }

        [Fact]
        public void BuildAbstractTypeUsingStronglyTypedFactoryIsPossible()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            var result = sut.Build<AbstractType>().FromFactory(() => new ConcreteType()).Create();
            // Assert
            Assert.IsAssignableFrom<ConcreteType>(result);
        }

        [Fact]
        [Obsolete]
        public void BuildAbstractTypeUsingBuilderIsPossibleObsolete()
        {
            // Arrange
            var sut = new Fixture();
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => new ConcreteType() };
            // Act
            var result = sut.Build<AbstractType>().FromFactory(builder).CreateAnonymous();
            // Assert
            Assert.IsAssignableFrom<ConcreteType>(result);
        }

        [Fact]
        public void BuildAbstractTypeUsingBuilderIsPossible()
        {
            // Arrange
            var sut = new Fixture();
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => new ConcreteType() };
            // Act
            var result = sut.Build<AbstractType>().FromFactory(builder).Create();
            // Assert
            Assert.IsAssignableFrom<ConcreteType>(result);
        }

        [Fact]
        [Obsolete]
        public void BuildAbstractTypeCorrectlyAppliesPropertyObsolete()
        {
            // Arrange
            var expected = new object();
            var sut = new Fixture();
            // Act
            var result = sut.Build<AbstractType>()
                .FromFactory(() => new ConcreteType())
                .With(x => x.Property1, expected)
                .CreateAnonymous();
            // Assert
            Assert.Equal(expected, result.Property1);
        }

        [Fact]
        public void BuildAbstractTypeCorrectlyAppliesProperty()
        {
            // Arrange
            var expected = new object();
            var sut = new Fixture();
            // Act
            var result = sut.Build<AbstractType>()
                .FromFactory(() => new ConcreteType())
                .With(x => x.Property1, expected)
                .Create();
            // Assert
            Assert.Equal(expected, result.Property1);
        }

        [Fact]
        [Obsolete]
        public void RegisterNullWillAssignCorrectPickedPropertyValueObsolete()
        {
            // Arrange
            var sut = new Fixture();
            sut.Register(() => (string)null);
            // Act
            var result = sut.Build<PropertyHolder<string>>().With(p => p.Property).CreateAnonymous();
            // Assert
            Assert.Null(result.Property);
        }

        [Fact]
        public void RegisterNullWillAssignCorrectPickedPropertyValue()
        {
            // Arrange
            var sut = new Fixture();
            sut.Register(() => (string)null);
            // Act
            var result = sut.Build<PropertyHolder<string>>().With(p => p.Property).Create();
            // Assert
            Assert.Null(result.Property);
        }

        [Fact]
        public void AddingTracingBehaviorWillTraceDiagnostics()
        {
            // Arrange
            using (var writer = new StringWriter())
            {
                var sut = new Fixture();
                sut.Behaviors.Add(new TracingBehavior(writer));
                // Act
                sut.Create<int>();
                // Assert
                Assert.False(string.IsNullOrEmpty(writer.ToString()));
            }
        }

        [Fact]
        public void CreateAnonymousEnumReturnsCorrectResult()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            var result = sut.Create<TriState>();
            // Assert
            Assert.Equal(TriState.First, result);
        }

        [Fact]
        public void CreateManyAnonymousFlagEnumsReturnsCorrectResult()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            var result = sut.CreateMany<ActivityScope>(100).ToArray().Last();
            // Assert
            Assert.Equal(ActivityScope.Standalone, result);
        }

        [Fact]
        public void CustomizeNullCustomizationThrows()
        {
            // Arrange
            var sut = new Fixture();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Customize((ICustomization)null));
        }

        [Fact]
        public void CustomizeCorrectlyAppliesCustomization()
        {
            // Arrange
            var sut = new Fixture();

            var verified = false;
            var customization = new DelegatingCustomization { OnCustomize = f => verified = f == sut };
            // Act
            sut.Customize(customization);
            // Assert
            Assert.True(verified, "Mock verified");
        }

        [Fact]
        public void CustomizeReturnsCorrectResult()
        {
            // Arrange
            var sut = new Fixture();
            var dummyCustomization = new DelegatingCustomization();
            // Act
            var result = sut.Customize(dummyCustomization);
            // Assert
            Assert.Equal(sut, result);
        }

        [Fact]
        public void CreateAnonymousIntPtrThrowsCorrectException()
        {
            // Arrange
            var sut = new Fixture();
            // Act & assert
            var creationEx = Assert.ThrowsAny<ObjectCreationException>(() =>
                 sut.Create<IntPtr>());
            Assert.IsAssignableFrom<IllegalRequestException>(creationEx.InnerException);
        }

        [Fact]
        [Obsolete]
        public void BuildWithOverriddenVirtualPropertyCorrectlySetsPropertyObsolete()
        {
            // Arrange
            var sut = new Fixture();
            var expected = Guid.NewGuid();
            // Act
            var result = sut.Build<ConcreteType>()
                .With(x => x.Property4, expected)
                .CreateAnonymous();
            // Assert
            Assert.Equal(expected, result.Property4);
        }

        [Fact]
        public void BuildWithOverriddenVirtualPropertyCorrectlySetsProperty()
        {
            // Arrange
            var sut = new Fixture();
            var expected = Guid.NewGuid();
            // Act
            var result = sut.Build<ConcreteType>()
                .With(x => x.Property4, expected)
                .Create();
            // Assert
            Assert.Equal(expected, result.Property4);
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
            // Arrange
            var sut = new Fixture();
            // Act
            var result = sut.Create<ObservableCollection<decimal>>();
            // Assert
            Assert.NotEmpty(result);
        }

        [Fact, Obsolete]
        public void CreateAnonymousEnumerableWhenEnumerableRelayIsPresentReturnsCorrectResult()
        {
            // Arrange
            var sut = new Fixture();
            sut.ResidueCollectors.Add(new EnumerableRelay());
            // Act
            var result = sut.Create<IEnumerable<decimal>>();
            // Assert
            Assert.True(result.Any());
        }

        [Fact]
        public void CreateAnonymousListDoesFillWithItemsPerDefault()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            var result = sut.Create<List<string>>();
            // Assert
            Assert.True(result.Any());
        }

        [Fact, Obsolete]
        public void CreateAnonymousListWithCustomizationsReturnsCorrectResult()
        {
            // Arrange
            var sut = new Fixture();
            sut.Customizations.Add(new FilteringSpecimenBuilder(new MethodInvoker(new EnumerableFavoringConstructorQuery()), new ListSpecification()));
            sut.ResidueCollectors.Add(new EnumerableRelay());
            // Act
            var result = sut.Create<List<string>>();
            // Assert
            Assert.True(result.Any());
        }

        [Fact, Obsolete]
        public void CreateAnonymousHashSetWithCustomizationsReturnsCorrectResult()
        {
            // Arrange
            var sut = new Fixture();
            sut.Customizations.Add(new FilteringSpecimenBuilder(new MethodInvoker(new EnumerableFavoringConstructorQuery()), new HashSetSpecification()));
            sut.ResidueCollectors.Add(new EnumerableRelay());
            // Act
            var result = sut.Create<HashSet<float>>();
            // Assert
            Assert.True(result.Any());
        }

        [Fact, Obsolete]
        public void CreateAnonymousIListWithCustomizationsReturnsCorrectResult()
        {
            // Arrange
            var sut = new Fixture();
            sut.Customizations.Add(new FilteringSpecimenBuilder(new MethodInvoker(new EnumerableFavoringConstructorQuery()), new ListSpecification()));
            sut.ResidueCollectors.Add(new EnumerableRelay());
            sut.ResidueCollectors.Add(new ListRelay());
            // Act
            var result = sut.Create<IList<int>>();
            // Assert
            Assert.True(result.Any());
        }

        [Fact, Obsolete]
        public void CreateAnonymousICollectionWithCustomizationsReturnsCorrectResult()
        {
            // Arrange
            var sut = new Fixture();
            sut.Customizations.Add(new FilteringSpecimenBuilder(new MethodInvoker(new EnumerableFavoringConstructorQuery()), new ListSpecification()));
            sut.ResidueCollectors.Add(new EnumerableRelay());
            sut.ResidueCollectors.Add(new CollectionRelay());
            // Act
            var result = sut.Create<ICollection<Version>>();
            // Assert
            Assert.True(result.Any());
        }

        [Fact, Obsolete]
        public void CreateAnonymousCollectionWithCustomizationsReturnsCorrectResult()
        {
            // Arrange
            var sut = new Fixture();
            sut.Customizations.Add(new FilteringSpecimenBuilder(new MethodInvoker(new EnumerableFavoringConstructorQuery()), new ListSpecification()));
            sut.Customizations.Add(new FilteringSpecimenBuilder(new MethodInvoker(new ListFavoringConstructorQuery()), new CollectionSpecification()));
            sut.ResidueCollectors.Add(new EnumerableRelay());
            sut.ResidueCollectors.Add(new ListRelay());
            // Act
            var result = sut.Create<Collection<ConcreteType>>();
            // Assert
            Assert.True(result.Any());
        }

        [Fact, Obsolete]
        public void CreateAnonymousDictionaryWithCustomizationsReturnsCorrectResult()
        {
            // Arrange
            var sut = new Fixture();
            sut.Customizations.Add(new FilteringSpecimenBuilder(new Postprocessor(new MethodInvoker(new ModestConstructorQuery()), new DictionaryFiller()), new DictionarySpecification()));
            // Act
            var result = sut.Create<Dictionary<int, string>>();
            // Assert
            Assert.True(result.Any());
        }

        [Fact, Obsolete]
        public void CreateAnonymousIDictionaryWithCustomizationsReturnsCorrectResult()
        {
            // Arrange
            var sut = new Fixture();
            sut.Customizations.Add(new FilteringSpecimenBuilder(new Postprocessor(new MethodInvoker(new ModestConstructorQuery()), new DictionaryFiller()), new DictionarySpecification()));
            sut.ResidueCollectors.Add(new DictionaryRelay());
            // Act
            var result = sut.Create<IDictionary<TimeSpan, Version>>();
            // Assert
            Assert.True(result.Any());
        }

        [Fact, Obsolete]
        public void CreateMultipleHoldersOfEnumsReturnsCorrectResultForLastItem()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            var result = sut.CreateMany<PropertyHolder<TriState>>(4);
            // Assert
            Assert.InRange(result.Last().Property, TriState.First, TriState.Third);
        }

        [Fact]
        public void CreateMultipleHoldersOfNullableEnumsReturnsCorrectResultForLastItem()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            var result = sut.CreateMany<PropertyHolder<TriState?>>(4);
            // Assert
            Assert.InRange(result.Last().Property.GetValueOrDefault(), TriState.First, TriState.Third);
        }

        [Fact]
        public void CreateMultipleSingleParameterTypesWithEnumReturnsCorrectResultForLastItem()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            var result = sut.CreateMany<PropertyHolder<SingleParameterType<TriState>>>(4);
            // Assert
            Assert.InRange(result.Last().Property.Parameter, TriState.First, TriState.Third);
        }

        // Supporting http://autofixture.codeplex.com/discussions/262288 Breaking this test might not be considered a breaking change
        [Fact]
        public void RefreezeHack()
        {
            // Arrange
            var fixture = new Fixture();
            var version2 = fixture.Create<Version>();
            var version1 = fixture.Freeze<Version>();
            // Act
            fixture.Inject(version2);
            var actual = fixture.Create<Version>();
            // Assert
            Assert.Equal(version2, actual);
        }

        // Supporting http://autofixture.codeplex.com/discussions/262288 Breaking this test might not be considered a breaking change
        [Fact]
        public void UnfreezeHack()
        {
            // Arrange
            var fixture = new Fixture();
            var snapshot = fixture.Customizations.ToList();
            var anonymousVersion = fixture.Freeze<Version>();
            var freezer = fixture.Customizations.Except(snapshot).Single();
            // Act
            fixture.Customizations.Remove(freezer);
            var expected = fixture.Freeze<Version>();
            var actual = fixture.Create<Version>();
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void UseGreedyConstructorQueryWithAutoProperties()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Customizations.Add(new Postprocessor(
                new MethodInvoker(new GreedyConstructorQuery()),
                new AutoPropertiesCommand(),
                new AnyTypeSpecification()));
            // Act
            var result = fixture.Create<ConcreteType>();
            // Assert
            Assert.NotNull(result.Property5);
        }

        [Fact]
        public void CreateAnonymousTypeReturnsInstance()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var result = fixture.Create<Type>();
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void CreateHashSetReturnsCorrectResult()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var result = fixture.Create<HashSet<string>>();
            // Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public void CreateSortedSetReturnsCorrectResult()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var result = fixture.Create<SortedSet<string>>();
            // Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public void CreateSortedDictionaryReturnsCorrectResult()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var result = fixture.Create<SortedDictionary<string, object>>();
            // Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public void CreateSortedListReturnsCorrectResult()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var result = fixture.Create<SortedList<int, string>>();
            // Assert
            Assert.NotEmpty(result);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void InjectDoesNotModifyAutoProperties(bool expected)
        {
            // Arrange
            var fixture = new Fixture();
            fixture.OmitAutoProperties = expected;
            // Act
            fixture.Inject("dummy");
            // Assert
            Assert.Equal(expected, fixture.OmitAutoProperties);
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
        public void ResidueCollectorsContainEnumerableRelayByDefault(
            Type relayType)
        {
            var sut = new Fixture();
            Assert.Contains(
                sut.ResidueCollectors,
                b => relayType.IsInstanceOfType(b));
        }

        [Theory]
        [InlineData(typeof(IList<>), typeof(List<>))]
        [InlineData(typeof(IReadOnlyList<>), typeof(ReadOnlyCollection<>))]
        [InlineData(typeof(ICollection<>), typeof(List<>))]
        [InlineData(typeof(IReadOnlyCollection<>), typeof(ReadOnlyCollection<>))]
        [InlineData(typeof(IDictionary<,>), typeof(Dictionary<,>))]
        public void ResidueCollectorsContainForwardsByDefault(Type from, Type to)
        {
            // Arrange
            var sut = new Fixture();
            // Act & assert
            Assert.Contains(
                sut.ResidueCollectors,
                b => b is TypeRelay tr && tr.From == from && tr.To == to);
        }

        [Theory]
        [InlineData(typeof(List<>), typeof(EnumerableFavoringConstructorQuery))]
        [InlineData(typeof(HashSet<>), typeof(EnumerableFavoringConstructorQuery))]
        [InlineData(typeof(Collection<>), typeof(ListFavoringConstructorQuery))]
        [InlineData(typeof(ObservableCollection<>), typeof(EnumerableFavoringConstructorQuery))]
        public void CustomizationsContainBuilderForProperConcreteMultipleTypeByDefault(
            Type matchingType,
            Type queryType)
        {
            var sut = new Fixture();
            Assert.Contains(
                sut.Customizations,
                b => b is FilteringSpecimenBuilder fsb
                     && fsb.Specification is ExactTypeSpecification ets && ets.TargetType == matchingType
                     && fsb.Builder is MethodInvoker mi && mi.Query.GetType() == queryType);
        }

        [Theory]
        [InlineData(typeof(Dictionary<,>), typeof(ModestConstructorQuery))]
        [InlineData(typeof(SortedDictionary<,>), typeof(ModestConstructorQuery))]
        [InlineData(typeof(SortedList<,>), typeof(ModestConstructorQuery))]
        public void CustomizationsContainBuilderForConcreteDictionariesByDefault(
            Type matchingType,
            Type queryType)
        {
            var sut = new Fixture();
            Assert.Contains(
                sut.Customizations,
                b => b is FilteringSpecimenBuilder fsb
                     && fsb.Specification is ExactTypeSpecification ets && ets.TargetType == matchingType
                     && fsb.Builder is Postprocessor pp && pp.Command is DictionaryFiller
                     && pp.Builder is MethodInvoker mi && mi.Query.GetType() == queryType);
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
            // Arrange
            var fixture = new Fixture();
            // Act
            var result = fixture.Create<MutableValueType>();
            // Assert
            Assert.NotNull(result.Property1);
            Assert.NotNull(result.Property2);
            Assert.NotNull(result.Property3);
        }

        [Fact]
        public void CreateAnonymousStructWithoutConstructorThrowsException()
        {
            // Arrange
            var fixture = new Fixture();
            // Act & assert
            Assert.ThrowsAny<ObjectCreationException>(() => fixture.Create<MutableValueTypeWithoutConstructor>());
        }

        /// <summary>
        /// This test is just to make sure that edge cases as decimal which is not primitive type and is a structure will not fall within
        /// struct checking mechanism.
        /// </summary>
        [Fact]
        public void CreateDecimalDoesNotThrowException()
        {
            // Arrange
            var fixture = new Fixture();
            // Act & assert
            Assert.Null(Record.Exception(() => fixture.Create<decimal>()));
        }

        [Fact]
        public void CreateAnonymousStructWithoutConstructorUsingCustomizationReturnsInstance()
        {
            // Arrange
            var fixture = new Fixture();
            var sut = new SupportMutableValueTypesCustomization();
            sut.Customize(fixture);
            // Act & assert
            var result = fixture.Create<MutableValueTypeWithoutConstructor>();
            // Assert
            Assert.NotNull(result.Property1);
            Assert.NotNull(result.Property2);
        }

        [Fact, Obsolete]
        public void CustomizingEnumerableCustomizedCreateManyWhenCreateManyIsMappedToEnumerable()
        {
            // Arrange
            var fixture =
                new Fixture().Customize(new MapCreateManyToEnumerable());
            var expected = new[] { "a", "b", "c", "d" };
            fixture.Register<IEnumerable<string>>(() => expected);
            // Act
            var actual = fixture.CreateMany<string>();
            // Assert
            Assert.Equal(expected, actual.ToArray());
        }

        [Fact]
        public void ItIsPossibleToMapManyRequestToCustomEnumerableInstance()
        {
            // Arrange
            var sut = new Fixture();
            var expected = new[] { "a", "b", "c", "d" };
            sut.Customizations.Add(
                new FilteringSpecimenBuilder(
                    new FixedBuilder(
                        expected),
                    new EqualRequestSpecification(
                        new MultipleRequest(
                            new SeededRequest(
                                typeof(string),
                                default(string))))));

            // Act
            var actual = sut.CreateMany<string>();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CreateAbstractTypeThreeTimesGivesTheSameErrorMessage()
        {
            // Arrange
            var fixture = new Fixture();

            // Act & assert
            var ocex1 = Assert.ThrowsAny<ObjectCreationException>(() =>
                fixture.Create<PropertyHolder<AbstractType>>());

            var ocex2 = Assert.ThrowsAny<ObjectCreationException>(() =>
                fixture.Create<PropertyHolder<AbstractType>>());

            var ocex3 = Assert.ThrowsAny<ObjectCreationException>(() =>
                fixture.Create<PropertyHolder<AbstractType>>());

            Assert.Equal(ocex1.Message, ocex2.Message);
            Assert.Equal(ocex2.Message, ocex3.Message);
        }

        [Fact]
        public void CreateRecursiveTypeExceptionMessageIsStable()
        {
            // Arrange
            var fixture = new Fixture();

            // Act & assert
            var ocex1 = Assert.ThrowsAny<ObjectCreationException>(() =>
                fixture.Create<PropertyHolder<RecursionTestObjectWithReferenceOutA>>());

            var ocex2 = Assert.ThrowsAny<ObjectCreationException>(() =>
                fixture.Create<PropertyHolder<RecursionTestObjectWithReferenceOutA>>());

            var ocex3 = Assert.ThrowsAny<ObjectCreationException>(() =>
                fixture.Create<PropertyHolder<RecursionTestObjectWithReferenceOutA>>());

            Assert.Equal(ocex1.Message, ocex2.Message);
            Assert.Equal(ocex2.Message, ocex3.Message);
        }

        [Fact]
        public void TraceOutputForRecursiveTypeCreationFailureIsStable()
        {
            // Arrange
            var fixture = new Fixture();
            var outputWriter = new StringWriter();
            fixture.Behaviors.Add(new TracingBehavior(outputWriter));

            // Act & assert
            Assert.ThrowsAny<ObjectCreationException>(() =>
                fixture.Create<PropertyHolder<RecursionTestObjectWithReferenceOutA>>());

            var traceOutput1 = outputWriter.ToString();
            outputWriter.GetStringBuilder().Clear();

            Assert.ThrowsAny<ObjectCreationException>(() =>
                fixture.Create<PropertyHolder<RecursionTestObjectWithReferenceOutA>>());

            var traceOutput2 = outputWriter.ToString();
            outputWriter.GetStringBuilder().Clear();

            Assert.Equal(traceOutput1, traceOutput2);
        }

        [Fact]
        public void CreateSmallRecursiveSequenceGraph()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior(2));
            // Act
            var actual = fixture.Create<RecursiveSequenceNode>();
            // Assert
            Assert.NotEmpty(actual);
            Assert.True(actual.All(n => !n.Any()));
        }

        [Fact]
        public void CreateSequenceOfSmallRecursiveSequenceGraphs()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));
            fixture.Customizations.Add(
                new OmitEnumerableParameterRequestRelay());
            // Act
            var actual = fixture.Create<IEnumerable<RecursiveSequenceNode>>();
            // Assert
            Assert.NotEmpty(actual);
            Assert.True(actual.All(n => !n.Any()));
        }

        [Fact]
        public void CreateArrayOfSmallRecursiveSequenceGraphs()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));
            fixture.Customizations.Add(
                new OmitEnumerableParameterRequestRelay());
            // Act
            var actual = fixture.Create<RecursiveSequenceNode[]>();
            // Assert
            Assert.NotEmpty(actual);
            Assert.True(actual.All(n => !n.Any()));
        }

        [Fact]
        public void CreateManySmallRecursiveSequenceGraphs()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));
            fixture.Customizations.Add(
                new OmitEnumerableParameterRequestRelay());
            // Act
            var actual = fixture.CreateMany<RecursiveSequenceNode>();
            // Assert
            Assert.NotEmpty(actual);
            Assert.True(actual.All(n => !n.Any()));
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
            // Arrange
            var fixture = new Fixture();
            fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior(2));
            // Act
            var actual = fixture.Create<RecursiveArrayNode>();
            // Assert
            Assert.NotEmpty(actual);
            Assert.True(actual.All(n => !n.Any()));
        }

        [Fact]
        public void CreateSequenceOfSmallRecursiveArrayGraphs()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));
            fixture.Customizations.Add(
                new OmitArrayParameterRequestRelay());
            // Act
            var actual = fixture.Create<IEnumerable<RecursiveArrayNode>>();
            // Assert
            Assert.NotEmpty(actual);
            Assert.True(actual.All(n => !n.Any()));
        }

        [Fact]
        public void CreateArrayOfSmallRecursiveArrayGraphs()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));
            fixture.Customizations.Add(
                new OmitArrayParameterRequestRelay());
            // Act
            var actual = fixture.Create<RecursiveArrayNode[]>();
            // Assert
            Assert.NotEmpty(actual);
            Assert.True(actual.All(n => !n.Any()));
        }

        [Fact]
        public void CreateManySmallRecursiveArrayGraphs()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));
            fixture.Customizations.Add(
                new OmitArrayParameterRequestRelay());
            // Act
            var actual = fixture.CreateMany<RecursiveArrayNode>();
            // Assert
            Assert.NotEmpty(actual);
            Assert.True(actual.All(n => !n.Any()));
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
            // Arrange
            var fixture = new Fixture();
            fixture.Customize<AcwaacpChildA>(c => c.Without(x => x.Value));
            // Act
            var actual = fixture.Create<AcwaacpChildB>();
            // Assert
            Assert.NotEqual(default(int), actual.Value);
        }

        /// <summary>
        /// Checks the scenario: https://github.com/AutoFixture/AutoFixture/issues/531.
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
        /// Checks the scenario reported in https://github.com/AutoFixture/AutoFixture/issues/772.
        /// </summary>
        [Fact]
        public void CustomizatonOfSamePropertyIsIgnoredDuringTheBuild()
        {
            // arrange
            var sut = new Fixture();
            sut.Customize<DoublePropertyHolder<string, int>>(c => c.With(x => x.Property1, "foo"));

            // act
            var result = sut
                .Build<DoublePropertyHolder<string, int>>()
                .With(x => x.Property2, 42)
                .Create();

            // assert
            Assert.NotEqual("foo", result.Property1);
            Assert.Equal(42, result.Property2);
        }

        /// <summary>
        /// Scenario from https://github.com/AutoFixture/AutoFixture/issues/321.
        /// </summary>
        [Fact]
        public void CustomizationOfIntPropertyDoesntThrowInBuild()
        {
            // arrange
            var sut = new Fixture();
            sut.Customize<PropertyHolder<long>>(c => c.Without(x => x.Property));

            // act
            var result = sut.Build<PropertyHolder<long>>().With(x => x.Property).Create();

            // assert
            Assert.NotEqual(0L, result.Property);
        }

        [Fact]
        public void EnableDisableAutoPropertiesDoesntBreakCustomization()
        {
            // arrange
            var sut = new Fixture();
            sut.Customize<PropertyHolder<string>>(c =>
                c
                    .Without(x => x.Property)
                    .OmitAutoProperties()
                    .WithAutoProperties());

            // act
            var result = sut.Create<PropertyHolder<string>>();

            // assert
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

        [Theory]
        [InlineData(typeof(string[][,,][]))]
        [InlineData(typeof(object[,,][][,]))]
        public void CreateComplexArrayTypeReturnsCorrectResult(Type request)
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
            // Arrange
            Fixture sut = new Fixture();
            // Act
            Task result = sut.Create<Task>();
            // Assert
            Thread thread = new Thread(result.Wait);
            thread.Start();
            bool ranToCompletion = thread.Join(1000);

            Assert.True(ranToCompletion);
        }

        [Fact]
        public void CreateGenericTaskReturnsAwaitableTask()
        {
            // Arrange
            Fixture sut = new Fixture();
            // Act
            Task<int> result = sut.Create<Task<int>>();
            // Assert
            Thread thread = new Thread(result.Wait);
            thread.Start();
            bool ranToCompletion = thread.Join(1000);

            Assert.True(ranToCompletion);
        }

        [Fact]
        public void CreateGenericTaskReturnsTaskWhoseResultWasResolvedByFixture()
        {
            // Arrange
            Fixture sut = new Fixture();
            int frozenInt = sut.Freeze<int>();
            // Act
            Task<int> result = sut.Create<Task<int>>();
            // Assert
            Assert.Equal(frozenInt, result.Result);
        }

        [Fact]
        public void CreateLazyInitializedTypeReturnsCorrectResult()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var result = fixture.Create<Lazy<string>>();
            var actual = result.Value;
            // Assert
            Assert.NotNull(actual);
        }

        [Fact]
        public void CreatesEnumerator()
        {
            // Arrange
            var fixture = new Fixture();
            // Act & assert
            IEnumerator<int> result = null;
            Assert.Null(Record.Exception(() => result = fixture.Create<IEnumerator<int>>()));
            Assert.NotNull(result);
        }

        [Fact]
        public void CreatesEnumeratorWithSpecifiedNumberOfItems()
        {
            // Arrange
            var fixture = new Fixture();
            var expectedCount = fixture.RepeatCount;
            // Act
            var result = fixture.Create<IEnumerator<int>>();
            // Assert
            int count = 0;
            while (result.MoveNext())
            {
                count++;
                Assert.True(count <= expectedCount);
            }
            Assert.Equal(expectedCount, count);
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
            // Arrange
            var fixture = new Fixture();
            // Act
            var actual = fixture.Create<System.Net.IPAddress>();
            // Assert
            Assert.NotNull(actual);
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
        /// https://github.com/AutoFixture/AutoFixture/pull/604.
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

        [Fact]
        public void Issue691_DontUseCastOperatorsToCreateSpecimen()
        {
            // Arrange
            var fixture = new Fixture();

            // Act & assert
            var ex = Assert.ThrowsAny<ObjectCreationException>(() =>
                fixture.Create<TypeWithCastOperatorsWithoutPublicConstructor>());

            Assert.Contains("most likely because it has no public constructor", ex.Message);
        }

        [Fact]
        public void Issue724_ShouldFailWithMeaningfulException_WhenNestedPropertyConfiguredViaBuildWithValue()
        {
            // Arrange
            var fixture = new Fixture();

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                return fixture
                    .Build<PropertyHolder<ConcreteType>>()
                    .With(x => x.Property.Property1, "Dummy");
            });
            Assert.Contains("nested property or field", ex.Message);
        }

        [Fact]
        public void Issue724_ShouldFailWithMeaningfulException_WhenNestedPropertyConfiguredViaBuildWith()
        {
            // Arrange
            var fixture = new Fixture();

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                return fixture
                    .Build<PropertyHolder<ConcreteType>>()
                    .With(x => x.Property.Property1);
            });
            Assert.Contains("nested property or field", ex.Message);
        }

        [Fact]
        public void Issue724_ShouldFailWithMeaningfulException_WhenNestedPropertyConfiguredViaBuildWithout()
        {
            // Arrange
            var fixture = new Fixture();

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                return fixture
                    .Build<PropertyHolder<ConcreteType>>()
                    .Without(x => x.Property.Property1);
            });
            Assert.Contains("nested property or field", ex.Message);
        }

        [Fact]
        public void Issue724_ShouldFailWithMeaningfulException_WhenNestedPropertyConfiguredViaCustomizeWithValue()
        {
            // Arrange
            var fixture = new Fixture();

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                fixture.Customize<PropertyHolder<ConcreteType>>(c => c.With(x => x.Property.Property1, "dummy"));
            });
            Assert.Contains("nested property or field", ex.Message);
        }

        [Fact]
        public void Issue724_ShouldFailWithMeaningfulException_WhenNestedPropertyConfiguredViaCustomizeWith()
        {
            // Arrange
            var fixture = new Fixture();

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                fixture.Customize<PropertyHolder<ConcreteType>>(c => c.With(x => x.Property.Property1));
            });
            Assert.Contains("nested property or field", ex.Message);
        }

        [Fact]
        public void Issue724_ShouldFailWithMeaningfulException_WhenNestedPropertyConfiguredViaCustomizeWithout()
        {
            // Arrange
            var fixture = new Fixture();

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                fixture.Customize<PropertyHolder<ConcreteType>>(c => c.Without(x => x.Property.Property1));
            });
            Assert.Contains("nested property or field", ex.Message);
        }

        [Fact]
        public void Issue871_FailsWithArgumentExceptionForNullRequest()
        {
            // Arrange
            var sut = new Fixture();
            var specimenContext = new DelegatingSpecimenContext();

            // Act & assert
            Assert.Throws<ArgumentNullException>(() => sut.Create(null, specimenContext));
        }

        [Fact]
        public void Issue871_FailsWithArgumentExceptionForNullRequestContext()
        {
            // Arrange
            var sut = new Fixture();
            var request = new object();

            // Act & assert
            Assert.Throws<ArgumentNullException>(() => sut.Create(request, null));
        }

#if SYSTEM_NET_MAIL
        [Fact]
        public void CreateAnonymousWithMailAddressReturnsValidResult()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var mailAddress = fixture.Create<System.Net.Mail.MailAddress>();
            // Assert
            Assert.True(mailAddress != null);
        }
#endif

        [Fact]
        public void Issue453_RangeAttributeShouldFailWithMeaningfulException()
        {
            // Arrange
            var sut = new Fixture();

            // Act & assert
            var actualEx = Assert.ThrowsAny<ObjectCreationException>(
                () => sut.Create<Issue453_AnnotationWithOverflow>());

            Assert.IsType<OverflowException>(actualEx.InnerException);
            Assert.Contains("To solve the issue", actualEx.InnerException.Message);
        }

        private class Issue453_AnnotationWithOverflow
        {
            [Range(short.MinValue, long.MaxValue, ErrorMessage = "Id is not in range")]
            public long CustomerId { get; set; }
        }

        private class RangeAnnotationWithLargeStringBoundaries
        {
            [Range(typeof(long), "1", /* long.MaxValue */ "9223372036854775807")]
            public long PropertyWithStringValueRange1 { get; set; }

            [Range(typeof(long), /* long.MinValue */ "-9223372036854775808", "-1")]
            public long PropertyWithStringValueRange2 { get; set; }
        }

        [Fact]
        public void RangeAttributeWithLargeStringRangeShouldWork()
        {
            // Arrange
            var sut = new Fixture();

            // Act
            var result = sut.Create<RangeAnnotationWithLargeStringBoundaries>();

            // Assert
            Assert.NotEqual(0, result.PropertyWithStringValueRange1);
            Assert.NotEqual(0, result.PropertyWithStringValueRange2);
        }

        [Fact]
        public void ShouldNotDuplicateRequestPathTwiceInCaseOfRecursionGuardException()
        {
            // Arrange
            var sut = new Fixture();
            var requestToLookFor = typeof(RecursiveArrayNode).GetConstructors().Single().GetParameters().Single();

            // Act & assert
            var actualEx = Assert.ThrowsAny<ObjectCreationException>(() => sut.Create<RecursiveArrayNode>());
            int numberOfRequestOccurence =
                new Regex(Regex.Escape(requestToLookFor.ToString())).Matches(actualEx.Message).Count;

            Assert.Equal(1, numberOfRequestOccurence);
        }

        [Theory]
        [InlineData(typeof(byte))]
        [InlineData(typeof(sbyte))]
        [InlineData(typeof(short))]
        [InlineData(typeof(ushort))]
        [InlineData(typeof(int))]
        [InlineData(typeof(uint))]
        [InlineData(typeof(long))]
        [InlineData(typeof(ulong))]
        [InlineData(typeof(decimal))]
        [InlineData(typeof(float))]
        [InlineData(typeof(double))]
        public void Issue897_ShouldCorrectlyHandleRequestsWithSameMinimumAndMaximumValue(Type type)
        {
            // Arrange
            var expectedValue = Convert.ChangeType(42, type, CultureInfo.InvariantCulture);
            var fakeMember = new FakeMemberInfo(
                new ProvidedAttribute(
                    new RangeAttribute(type, "42", "42"),
                    inherited: false));

            var sut = new Fixture();

            // Act
            var result = sut.Create(fakeMember, new SpecimenContext(sut));

            // Assert
            Assert.Equal(expectedValue, result);
        }

        [Fact]
        public void ShouldResolveReadOnlyCollectionByDefault()
        {
            // Arrange
            var sut = new Fixture();

            // Act
            var result = sut.Create<IReadOnlyCollection<string>>();

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void ResolvedIReadOnlyCollectionShouldBeReadOnly()
        {
            // Arrange
            var sut = new Fixture();

            // Act
            var result = sut.Create<IReadOnlyCollection<string>>();

            // Assert
            Assert.False(result is Collection<string>);
            Assert.False(result is List<string>);
        }

        [Fact]
        public void ShouldResolveReadOnlyListByDefault()
        {
            // Arrange
            var sut = new Fixture();

            // Act
            var result = sut.Create<IReadOnlyList<string>>();

            // Assert
            Assert.IsAssignableFrom<IReadOnlyList<string>>(result);
        }

        [Fact]
        public void ResolvedIReadOnlyListShouldBeReadOnly()
        {
            // Arrange
            var sut = new Fixture();

            // Act
            var result = sut.Create<IReadOnlyList<string>>();

            // Assert
            Assert.False(result is List<string>);
        }

        [Fact]
        public void ShouldResolveReadOnlyDictionaryByDefault()
        {
            // Arrange
            var sut = new Fixture();

            // Act
            var result = sut.Create<IReadOnlyDictionary<int, string>>();

            // Assert
            Assert.IsAssignableFrom<IReadOnlyDictionary<int, string>>(result);
        }

        [Fact]
        public void ResolvedIReadOnlyDictionaryShouldBeReadOnly()
        {
            // Arrange
            var sut = new Fixture();

            // Act
            var result = sut.Create<IReadOnlyDictionary<int, string>>();

            // Assert
            Assert.False(result is Dictionary<int, string>);
        }

        [Fact]
        public void ShouldResolveISetByDefault()
        {
            // Arrange
            var sut = new Fixture();

            // Act
            var result = sut.Create<ISet<string>>();

            // Assert
            Assert.IsAssignableFrom<ISet<string>>(result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void OmitAutoProperitesIsChangedAfterAssignment(bool value)
        {
            // Arrange
            var sut = new Fixture
            {
                OmitAutoProperties = !value
            };

            // Act
            sut.OmitAutoProperties = value;

            // Assert
            Assert.Equal(value, sut.OmitAutoProperties);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Optimization_GraphIsNotMutatedWhenActualOmitAutoPropertiesValueIsNotChanged(bool initialValue)
        {
            // Arrange
            var sut = new Fixture();
            sut.OmitAutoProperties = initialValue;

            var oldBehaviors = sut.Behaviors;
            var oldCustomizations = sut.Customizations;
            var oldResidueCollectors = sut.ResidueCollectors;

            // Act
            sut.OmitAutoProperties = initialValue;

            // Assert
            Assert.Same(oldBehaviors, sut.Behaviors);
            Assert.Same(oldCustomizations, sut.Customizations);
            Assert.Same(oldResidueCollectors, sut.ResidueCollectors);
        }

        [Fact]
        public void RangedRequestIsPresentInFailurePath()
        {
            // Arrange
            var sut = new Fixture();
            var request = new RangedRequest(typeof(ConcreteType), typeof(ConcreteType), 10, 42);
            var context = new SpecimenContext(sut);

            // Act & assert
            var ex = Assert.ThrowsAny<ObjectCreationException>(() =>
                sut.Create(request, context));

            var requestPath = ex.Message.Substring(ex.Message.IndexOf("Request path:"));

            Assert.Contains("RangedRequest", requestPath);
        }

        [Fact]
        public void RangedNumberRequestIsPresentInFailurePath()
        {
            // Arrange
            var sut = new Fixture();
            var request = new RangedNumberRequest(typeof(ConcreteType), 10, 42);
            var context = new SpecimenContext(sut);

            // Act & assert
            var ex = Assert.ThrowsAny<ObjectCreationException>(() =>
                sut.Create(request, context));

            var requestPath = ex.Message.Substring(ex.Message.IndexOf("Request path:"));

            Assert.Contains("RangedNumberRequest", requestPath);
        }

        private class TypeWithRangedEnumProperties
        {
            [Range(1, 3)]
            public EnumType RangedEnumProperty { get; set; }

            [Range(typeof(EnumType), nameof(EnumType.First), nameof(EnumType.Third))]
            public EnumType LiteralRangedEnumProperty { get; set; }

            [Range(typeof(EnumType), "1", "3")]
            public EnumType NumericRangedEnumProperty { get; set; }

            [Range(2, 2)]
            public EnumType Equal2EnumProperty { get; set; }

            [Range(10, 20)]
            public EnumType OutOfRangeEnumProperty { get; set; }
        }

        /// <summary>
        /// Scenario for: https://github.com/AutoFixture/AutoFixture/issues/722.
        /// </summary>
        [Fact]
        public void ShouldCorrectlyResolveEnumPropertiesDecoratedWithRange()
        {
            // Arrange
            var sut = new Fixture();

            // Act
            var result = sut.Create<TypeWithRangedEnumProperties>();
            // Assert
            Assert.InRange(result.RangedEnumProperty, EnumType.First, EnumType.Third);
            Assert.InRange(result.LiteralRangedEnumProperty, EnumType.First, EnumType.Third);
            Assert.InRange(result.NumericRangedEnumProperty, EnumType.First, EnumType.Third);
            Assert.Equal(EnumType.Second, result.Equal2EnumProperty);
        }

        [Fact]
        public void ShouldGenerateOutOfRangeValueIfRangeAttributeIsOutOfEnumRange()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            var result = sut.Create<TypeWithRangedEnumProperties>();
            // Assert
            var numericValue = (int)result.OutOfRangeEnumProperty;
            Assert.InRange(numericValue, 10, 20);
        }

        public class TypeWithDataAnnotationUsage
        {
            [Range(-42, -42)]
            public int FixedValue { get; set; }
        }

        [Fact]
        public void NoDataAnnotationCustomizationShouldDisableAnnotationSupport()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            sut.Customize(new NoDataAnnotationsCustomization());
            var result = sut.Create<TypeWithDataAnnotationUsage>();
            // Assert
            Assert.NotEqual(-42, result.FixedValue);
        }

        public interface IIssue970_ValueHolder
        {
            string Value { get; set; }
        }

        public class Issue970_ValueHolderImpl : IIssue970_ValueHolder
        {
            public string Value { get; set; }
        }

        [Fact]
        public void Issue970_DoesNotFailWhenHelperExtensionMethodIsUsed()
        {
            // Arrange
            var sut = new Fixture();
            // Act
            sut.Customize<Issue970_ValueHolderImpl>(c => ConfigurePropertyField(c));
            var result = sut.Create<Issue970_ValueHolderImpl>();
            // Assert
            Assert.Equal("42", result.Value);

            IPostprocessComposer<T> ConfigurePropertyField<T>(IPostprocessComposer<T> composer)
                where T : IIssue970_ValueHolder
            {
                return composer.With(x => x.Value, "42");
            }
        }

        private class TypeWithRangedTimeSpanProperty
        {
            [Range(typeof(TimeSpan), "02:00:00", "12:00:00")]
            public TimeSpan StringRangedTimeSpanProperty { get; set; }
        }

        [Fact]
        public void ShouldCorrectlyResolveTimeSpanPropertiesDecoratedWithRange()
        {
            // Arrange
            var sut = new Fixture();

            // Act
            var result = sut.Create<TypeWithRangedTimeSpanProperty>();

            // Assert
            Assert.InRange(result.StringRangedTimeSpanProperty, TimeSpan.FromHours(2), TimeSpan.FromHours(12));
        }

#if !VALIDATE_VALIDATEOBJECT_IS_FLACKY
        [Fact]
        public void TimeSpanDecoratedWithRangeCreatedByFixtureShouldPassValidation()
        {
            // Arrange
            var sut = new Fixture();

            // Act
            var timeSpan = sut.Create<TypeWithRangedTimeSpanProperty>();

            // Assert
            Validator.ValidateObject(timeSpan, new ValidationContext(timeSpan), true);
        }
#endif

        private class TypeWithStringPropertyWithMinLength
        {
            [MinLength(100)]
            public string PropertyWithMinLength { get; set; }
        }

        [Fact]
        public void ShouldCorrectlyResolveStringPropertyDecoratedWithMinLength()
        {
            // Arrange
            var sut = new Fixture();

            // Act
            var item = sut.Create<TypeWithStringPropertyWithMinLength>();

            // Assert
            Assert.True(item.PropertyWithMinLength.Length >= 100);
        }

        [Fact]
        public void StringDecoratedWithMinLengthAttributeShouldPassValidation()
        {
            // Arrange
            var sut = new Fixture();

            // Act
            var item = sut.Create<TypeWithStringPropertyWithMinLength>();

            // Assert
            Validator.ValidateObject(item, new ValidationContext(item), true);
        }

        private class TypeWithStringPropertyWithMaxLength
        {
            [MaxLength(5)]
            public string PropertyWithMaxLength { get; set; }
        }

        [Fact]
        public void ShouldCorrectlyResolveStringPropertyDecoratedWithMaxLength()
        {
            // Arrange
            var sut = new Fixture();

            // Act
            var item = sut.Create<TypeWithStringPropertyWithMaxLength>();

            // Assert
            Assert.True(item.PropertyWithMaxLength.Length <= 5);
        }

        [Fact]
        public void StringDecoratedWithMaxLengthAttributeShouldPassValidation()
        {
            // Arrange
            var sut = new Fixture();

            // Act
            var item = sut.Create<TypeWithStringPropertyWithMaxLength>();

            // Assert
            Validator.ValidateObject(item, new ValidationContext(item), true);
        }

        private class TypeWithStringPropertyWithMinAndMaxLength
        {
            [MinLength(5)]
            [MaxLength(10)]
            public string PropertyWithMinAndMaxLength { get; set; }
        }

        [Fact]
        public void ShouldCorrectlyResolveStringPropertyDecoratedWithMinAndMaxLength()
        {
            // Arrange
            var sut = new Fixture();

            // Act
            var item = sut.Create<TypeWithStringPropertyWithMinAndMaxLength>();

            // Assert
            Assert.True(item.PropertyWithMinAndMaxLength.Length >= 5);
            Assert.True(item.PropertyWithMinAndMaxLength.Length <= 10);
        }

        [Fact]
        public void StringDecoratedWithMinAndMaxLengthAttributeShouldPassValidation()
        {
            // Arrange
            var sut = new Fixture();

            // Act
            var item = sut.Create<TypeWithStringPropertyWithMinAndMaxLength>();

            // Assert
            Validator.ValidateObject(item, new ValidationContext(item), true);
        }

        private class TypeWithArrayPropertyWithMinAndMaxLength
        {
            [MinLength(5)]
            [MaxLength(10)]
            public string[] PropertyWithMinAndMaxLength { get; set; }
        }

        [Fact]
        public void ShouldCorrectlyResolveArrayPropertyDecoratedWithMinAndMaxLength()
        {
            // Arrange
            var sut = new Fixture();

            // Act
            var item = sut.Create<TypeWithArrayPropertyWithMinAndMaxLength>();

            // Assert
            Assert.True(item.PropertyWithMinAndMaxLength.Length >= 5);
            Assert.True(item.PropertyWithMinAndMaxLength.Length <= 10);
        }

        [Fact]
        public void ArrayDecoratedWithMinAndMaxLengthAttributeShouldPassValidation()
        {
            // Arrange
            var sut = new Fixture();

            // Act
            var item = sut.Create<TypeWithArrayPropertyWithMinAndMaxLength>();

            // Assert
            Validator.ValidateObject(item, new ValidationContext(item), true);
        }

        private class TypeWithArrayPropertyWithMinLength
        {
            [MinLength(5)]
            public string[] PropertyWithMinLength { get; set; }
        }

        [Fact]
        public void ArrayDecoratedWithMinLengthAttributeShouldPassValidation()
        {
            // Arrange
            var sut = new Fixture();

            // Act
            var item = sut.Create<TypeWithArrayPropertyWithMinLength>();

            // Assert
            Validator.ValidateObject(item, new ValidationContext(item), true);
        }

        private class TypeWithArrayPropertyWithMaxLength
        {
            [MaxLength(5)]
            public string[] PropertyWithMaxLength { get; set; }
        }

        [Fact]
        public void ArrayDecoratedWithMaxLengthAttributeShouldPassValidation()
        {
            // Arrange
            var sut = new Fixture();

            // Act
            var item = sut.Create<TypeWithArrayPropertyWithMaxLength>();

            // Assert
            Validator.ValidateObject(item, new ValidationContext(item), true);
        }

        [Fact]
        public void ShouldResolveRangedSequenceRequest()
        {
            // Arrange
            var sut = new Fixture();
            var minLength = 5;
            var maxLength = 10;
            var rsr = new RangedSequenceRequest(typeof(string), minLength, maxLength);

            // Act
            var result = sut.Create(rsr, new SpecimenContext(sut));

            // Assert
            Assert.IsNotType<NoSpecimen>(result);
            var resultSeq = ((IEnumerable<object>)result).Cast<string>();
            Assert.InRange(resultSeq.Count(), minLength, maxLength);
        }

        [Fact]
        public void ShouldResolveFixedNumberOfItemsForRangedSequenceRequest()
        {
            // Arrange
            var sut = new Fixture();
            var expectedLength = 5;
            var request = new RangedSequenceRequest(typeof(string), expectedLength, expectedLength);

            // Act
            var result = sut.Create(request, new SpecimenContext(sut));

            // Assert
            Assert.IsNotType<NoSpecimen>(result);
            var resultSeq = ((IEnumerable<object>)result).Cast<string>();
            Assert.Equal(expectedLength, resultSeq.Count());
        }
    }
}