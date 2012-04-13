using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
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
            Assert.True(expectedParts.SequenceEqual(from b in composite.Builders
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
            Assert.True(expectedParts.SequenceEqual(from b in composite.Builders
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
        public void SutIsSpecimenBuilderComposer()
        {
            // Fixture setup
            // Exercise system
            var sut = new Fixture();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilderComposer>(sut);
            // Teardown
        }

        [Fact]
        public void ComposeReturnsInstance()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.Compose();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void ComposeCorrectlyAppliesBehaviors()
        {
            // Fixture setup
            var sut = new Fixture();

            var comparer = new TaggedNodeComparer(new TrueComparer<ISpecimenBuilder>());

            sut.Behaviors.Clear();
            sut.Behaviors.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new TaggedNode(1, b) });
            sut.Behaviors.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => comparer.Equals(new TaggedNode(1), b) ? new TaggedNode(2, b) : new TaggedNode(3, b) });
            // Exercise system
            var result = sut.Compose();
            // Verify outcome
            Assert.Equal(new TaggedNode(2), result, comparer);
            // Teardown
        }

        [Fact]
        public void ComposeWhenAutoPropertiesAreOmittedReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new Fixture { OmitAutoProperties = true };
            // Exercise system
            var result = sut.Compose();
            // Verify outcome
            var guard = Assert.IsAssignableFrom<RecursionGuard>(result);

            var composedBuilders = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(guard.Builder).Builders.ToList();

            var customizer = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(composedBuilders[0]);
            Assert.True(sut.Customizations.SequenceEqual(customizer.Builders, new TypeBasedComparer<ISpecimenBuilder>()));

            var engineAndMultiple = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(composedBuilders[1]);
            Assert.Same(sut.Engine, engineAndMultiple.Builders[0]);

            var residueCollector = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(composedBuilders[2]);
            Assert.True(sut.ResidueCollectors.SequenceEqual(residueCollector.Builders));

            Assert.IsAssignableFrom<TerminatingSpecimenBuilder>(composedBuilders[3]);
            // Teardown
        }

        [Fact]
        public void ComposeWhenAutoPropertiesAreEnabledReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.Compose();
            // Verify outcome
            var guard = Assert.IsAssignableFrom<RecursionGuard>(result);

            var composedBuilders = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(guard.Builder).Builders.ToList();

            var customizer = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(composedBuilders[0]);
            Assert.True(sut.Customizations.SequenceEqual(customizer.Builders));

            var postprocessor = Assert.IsAssignableFrom<Postprocessor>(composedBuilders[1]);
            var engineAndMultiple = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(postprocessor.Builder);
            Assert.Same(sut.Engine, engineAndMultiple.Builders[0]);
            Assert.IsAssignableFrom<AnyTypeSpecification>(postprocessor.Specification);

            var residueCollector = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(composedBuilders[2]);
            Assert.True(sut.ResidueCollectors.SequenceEqual(residueCollector.Builders));

            Assert.IsAssignableFrom<TerminatingSpecimenBuilder>(composedBuilders[3]);
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
                sut.CreateAnonymous<AbstractType>());
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWillCreateSingleParameterType()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            // Exercise system
            SingleParameterType<object> result = sut.CreateAnonymous<SingleParameterType<object>>();
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
            SingleParameterType<AbstractType> result = sut.CreateAnonymous<SingleParameterType<AbstractType>>();
            // Verify outcome
            Assert.IsAssignableFrom<ConcreteType>(result.Parameter);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWillUseRegisteredMappingWithSingleParameter()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            sut.Register<object, AbstractType>(obj => new ConcreteType(obj));
            // Exercise system
            AbstractType result = sut.CreateAnonymous<AbstractType>();
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
            AbstractType result = sut.CreateAnonymous<AbstractType>();
            // Verify outcome
            Assert.NotNull(result.Property1);
            Assert.NotNull(result.Property2);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWillUseRegisterdMappingWithTripleParameters()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            sut.Register<object, object, object, AbstractType>((obj1, obj2, obj3) => new ConcreteType(obj1, obj2, obj3));
            // Exercise system
            AbstractType result = sut.CreateAnonymous<AbstractType>();
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
            AbstractType result = sut.CreateAnonymous<AbstractType>();
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
            string result = sut.CreateAnonymous(expectedText);
            // Verify outcome
            string actualText = new TextGuidRegex().GetText(result);
            Assert.Equal<string>(expectedText, actualText);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousStringWillAppendGuid()
        {
            // Fixture setup
            string anonymousText = "Anonymous text";
            Fixture sut = new Fixture();
            // Exercise system
            string result = sut.CreateAnonymous(anonymousText);
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
            PropertyHolder<string> result = sut.CreateAnonymous<PropertyHolder<string>>();
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
            PropertyHolder<string> result = sut.CreateAnonymous<PropertyHolder<string>>();
            // Verify outcome
            string propertyValue = result.Property;
            string text = new TextGuidRegex().GetText(propertyValue);
            Assert.Equal<string>(expectedName, text);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithStringPropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<string> ph = sut.CreateAnonymous<PropertyHolder<string>>();
            // Exercise system
            PropertyHolder<string> result = sut.CreateAnonymous<PropertyHolder<string>>();
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
            PropertyHolder<bool> result = sut.CreateAnonymous<PropertyHolder<bool>>();
            // Verify outcome
            Assert.NotEqual<bool>(unexpectedBoolean, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithCharPropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<char> ph = sut.CreateAnonymous<PropertyHolder<char>>();
            // Exercise system
            PropertyHolder<char> result = sut.CreateAnonymous<PropertyHolder<char>>();
            // Verify outcome
            Assert.NotEqual<char>(ph.Property, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithBooleanPropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<bool> ph = sut.CreateAnonymous<PropertyHolder<bool>>();
            // Exercise system
            PropertyHolder<bool> result = sut.CreateAnonymous<PropertyHolder<bool>>();
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
            PropertyHolder<byte> result = sut.CreateAnonymous<PropertyHolder<byte>>();
            // Verify outcome
            Assert.NotEqual<byte>(unexpectedByte, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithBytePropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<byte> ph = sut.CreateAnonymous<PropertyHolder<byte>>();
            // Exercise system
            PropertyHolder<byte> result = sut.CreateAnonymous<PropertyHolder<byte>>();
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
            PropertyHolder<sbyte> result = sut.CreateAnonymous<PropertyHolder<sbyte>>();
            // Verify outcome
            Assert.NotEqual<sbyte>(unexpectedSbyte, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithSignedBytePropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setpu
            Fixture sut = new Fixture();
            PropertyHolder<sbyte> ph = sut.CreateAnonymous<PropertyHolder<sbyte>>();
            // Exercise system
            PropertyHolder<sbyte> result = sut.CreateAnonymous<PropertyHolder<sbyte>>();
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
            PropertyHolder<ushort> result = sut.CreateAnonymous<PropertyHolder<ushort>>();
            // Verify outcome
            Assert.NotEqual<ushort>(unexpectedNumber, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithUnsignedInt16PropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<ushort> ph = sut.CreateAnonymous<PropertyHolder<ushort>>();
            // Exercise system
            PropertyHolder<ushort> result = sut.CreateAnonymous<PropertyHolder<ushort>>();
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
            PropertyHolder<short> result = sut.CreateAnonymous<PropertyHolder<short>>();
            // Verify outcome
            Assert.NotEqual<short>(unexpectedNumber, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithInt16PropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<short> ph = sut.CreateAnonymous<PropertyHolder<short>>();
            // Exercise system
            PropertyHolder<short> result = sut.CreateAnonymous<PropertyHolder<short>>();
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
            PropertyHolder<uint> result = sut.CreateAnonymous<PropertyHolder<uint>>();
            // Verify outcome
            Assert.NotEqual<uint>(unexpectedNumber, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithUnsignedInt32PropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<uint> ph = sut.CreateAnonymous<PropertyHolder<uint>>();
            // Exercise system
            PropertyHolder<uint> result = sut.CreateAnonymous<PropertyHolder<uint>>();
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
            PropertyHolder<int> result = sut.CreateAnonymous<PropertyHolder<int>>();
            // Verify outcome
            Assert.NotEqual<int>(unexpectedNumber, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithInt32PropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<int> ph = sut.CreateAnonymous<PropertyHolder<int>>();
            // Exercise system
            PropertyHolder<int> result = sut.CreateAnonymous<PropertyHolder<int>>();
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
            PropertyHolder<ulong> result = sut.CreateAnonymous<PropertyHolder<ulong>>();
            // Verify outcome
            Assert.NotEqual<ulong>(unexpectedNumber, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithUnsignedInt64PropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<ulong> ph = sut.CreateAnonymous<PropertyHolder<ulong>>();
            // Exercise system
            PropertyHolder<ulong> result = sut.CreateAnonymous<PropertyHolder<ulong>>();
            // Verify outcome
            Assert.NotEqual<ulong>(ph.Property, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithInt64PropertyWillAssignNonDefaulValue()
        {
            // Fixture setup
            long unexpectedNumber = default(long);
            Fixture sut = new Fixture();
            // Exercise system
            PropertyHolder<long> result = sut.CreateAnonymous<PropertyHolder<long>>();
            // Verify outcome
            Assert.NotEqual<long>(unexpectedNumber, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithInt64PropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<long> ph = sut.CreateAnonymous<PropertyHolder<long>>();
            // Exercise system
            PropertyHolder<long> result = sut.CreateAnonymous<PropertyHolder<long>>();
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
            PropertyHolder<decimal> result = sut.CreateAnonymous<PropertyHolder<decimal>>();
            // Verify outcome
            Assert.NotEqual<decimal>(unexpectedNumber, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithDecimalPropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<decimal> ph = sut.CreateAnonymous<PropertyHolder<decimal>>();
            // Exercise system
            PropertyHolder<decimal> result = sut.CreateAnonymous<PropertyHolder<decimal>>();
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
            PropertyHolder<float> result = sut.CreateAnonymous<PropertyHolder<float>>();
            // Verify outcome
            Assert.NotEqual<float>(unexpectedNumber, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithSinglePropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<float> ph = sut.CreateAnonymous<PropertyHolder<float>>();
            // Exercise system
            PropertyHolder<float> result = sut.CreateAnonymous<PropertyHolder<float>>();
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
            PropertyHolder<double> result = sut.CreateAnonymous<PropertyHolder<double>>();
            // Verify outcome
            Assert.NotEqual<double>(unexpectedNumber, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithDoublePropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            Fixture sut = new Fixture();
            PropertyHolder<double> ph = sut.CreateAnonymous<PropertyHolder<double>>();
            // Exercise system
            PropertyHolder<double> result = sut.CreateAnonymous<PropertyHolder<double>>();
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
            var result = sut.CreateAnonymous<DoublePropertyHolder<int, long>>();
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
            var result = sut.CreateAnonymous<DoublePropertyHolder<byte, short>>();
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
            var result = sut.CreateAnonymous<DoublePropertyHolder<double, float>>();
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
            var result = sut.CreateAnonymous<DoublePropertyHolder<long, float>>();
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
            var result = sut.CreateAnonymous<DoublePropertyHolder<int, long>>();
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
            var result = sut.CreateAnonymous<DoublePropertyHolder<byte, short>>();
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
            var result = sut.CreateAnonymous<DoublePropertyHolder<double, float>>();
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
            var result = sut.CreateAnonymous<DoublePropertyHolder<long, float>>();
            // Verify outcome
            Assert.Equal(result.Property1, result.Property2);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithGuidProperyWillAssignNonDefaultValue()
        {
            // Fixture setup
            Guid unexpectedGuid = default(Guid);
            var sut = new Fixture();
            // Exercise system
            var result = sut.CreateAnonymous<PropertyHolder<Guid>>();
            // Verify outcome
            Assert.NotEqual<Guid>(unexpectedGuid, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithGuidPropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            var sut = new Fixture();
            var ph = sut.CreateAnonymous<PropertyHolder<Guid>>();
            // Exercise system
            var result = sut.CreateAnonymous<PropertyHolder<Guid>>();
            // Verify outcome
            Assert.NotEqual<Guid>(ph.Property, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithFlagEnumPropertyTwiceWillAssignDifferentValues()
        {
            // Fixture setup
            var sut = new Fixture();
            var ph = sut.CreateAnonymous<PropertyHolder<ActivityScope>>();
            // Exercise system
            var result = sut.CreateAnonymous<PropertyHolder<ActivityScope>>();
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
        public void CreateAnonymousWithDateTimePropertyWillAssignDifferentDateThanToday()
        {
            // Fixture setup
            var today = DateTime.Today;
            var sut = new Fixture();
            // Exercise system
            var result = sut.CreateAnonymous<PropertyHolder<DateTime>>();
            // Verify outcome
            Assert.NotEqual(today, result.Property.Date);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithDoubleDateTimePropertiesWillAssignDifferentDates()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.CreateAnonymous<DoublePropertyHolder<DateTime, DateTime>>();
            // Verify outcome
            Assert.NotEqual<DateTime>(result.Property1.Date, result.Property2.Date);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithDateTimePropertyTwiceWithinMillisecondsReturnsDatesExactlyOneDayApart()
        {
            // Fixture setup
            var nowResolution = TimeSpan.FromMilliseconds(10); // see http://msdn.microsoft.com/en-us/library/system.datetime.now.aspx
            var sut = new Fixture();
            // Exercise system
            var firstResult = sut.CreateAnonymous<PropertyHolder<DateTime>>();
            Thread.Sleep(nowResolution + nowResolution);
            var secondResult = sut.CreateAnonymous<PropertyHolder<DateTime>>();
            // Verify outcome
            Assert.Equal(firstResult.Property.AddDays(1), secondResult.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithDoubleDateTimePropertiesAndFreezedInt32ValueWillAssignDifferentDates()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            sut.Freeze<int>();
            var result = sut.CreateAnonymous<DoublePropertyHolder<DateTime, DateTime>>();
            // Verify outcome
            Assert.NotEqual<DateTime>(result.Property1.Date, result.Property2.Date);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithDoubleDateTimePropertiesAndCurrentDateTimeCustomizationWillAssignSameDate()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            sut.Customize(new CurrentDateTimeCustomization());
            var result = sut.CreateAnonymous<DoublePropertyHolder<DateTime, DateTime>>();
            // Verify outcome
            Assert.Equal<DateTime>(result.Property1.Date, result.Property2.Date);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithArrayPropertyCorrectlyAssignsArray()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.CreateAnonymous<PropertyHolder<int[]>>();
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
            var result = sut.CreateAnonymous<PropertyHolder<Action>>();
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
            var result = sut.CreateAnonymous<PropertyHolder<Action<object>>>();
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
            var result = sut.CreateAnonymous<PropertyHolder<Action<string>>>();
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
            var result = sut.CreateAnonymous<PropertyHolder<Action<int>>>();
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
            var result = sut.CreateAnonymous<PropertyHolder<Action<object, object>>>();
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
            var result = sut.CreateAnonymous<PropertyHolder<Action<string, string>>>();
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
            var result = sut.CreateAnonymous<PropertyHolder<Action<int, bool>>>();
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
            var result = sut.CreateAnonymous<PropertyHolder<Action>>();
            // Verify outcome
            Assert.DoesNotThrow(() => ((Action)result.Property).Invoke());
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithReturnObjectParameterlessDelegatePropertyWillAssignNonDefaultValue()
        {
            // Fixture setup
            var unexpectedDelegate = default(Func<object>);
            var sut = new Fixture();
            // Exercise system
            var result = sut.CreateAnonymous<PropertyHolder<Func<object>>>();
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
            var result = sut.CreateAnonymous<PropertyHolder<Func<object, object>>>();
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
            var result = sut.CreateAnonymous<PropertyHolder<Func<string, object>>>();
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
            var result = sut.CreateAnonymous<PropertyHolder<Func<int, object>>>();
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
            var result = sut.CreateAnonymous<PropertyHolder<Func<int>>>();
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
            var result = sut.CreateAnonymous<PropertyHolder<Func<int, object>>>();
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
            var result = sut.CreateAnonymous<PropertyHolder<Func<int, string>>>();
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
            var result = sut.CreateAnonymous<PropertyHolder<Func<string>>>();
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
            var result = sut.CreateAnonymous<PropertyHolder<Func<int>>>();
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
            var result = fixture.CreateAnonymous<TypeWithFactoryMethod>();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithTypeWithFactoryPropertyWillInvokeFactoryProperty()
        {
            // Fixture setup
            var fixture = new Fixture();
            var result = fixture.CreateAnonymous<TypeWithFactoryProperty>();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithRangeValidatedTypeReturnsCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture();
            var result = fixture.CreateAnonymous<RangeValidatedType>();
            // Verify outcome
            Assert.True(result.Property >= RangeValidatedType.Minimum && result.Property <= RangeValidatedType.Maximum);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithRangeValidatedTypeReturnsCorrectResultMultipleCall()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = (from n in Enumerable.Range(1, 33).Select(i => fixture.CreateAnonymous<RangeValidatedType>().Property)
                          where (n < RangeValidatedType.Minimum && n > RangeValidatedType.Maximum)
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
            var result = fixture.CreateAnonymous<RegularExpressionValidatedType>();
            // Verify outcome
            Assert.True(Regex.IsMatch(result.Property, RegularExpressionValidatedType.Pattern));
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithStringLengthValidatedTypeReturnsCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture();
            var result = fixture.CreateAnonymous<StringLengthValidatedType>();
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
            var result = (from n in Enumerable.Range(1, 33).Select(i => fixture.CreateAnonymous<StringLengthValidatedType>().Property.Length)
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
                fixture.CreateAnonymous<Uri>().OriginalString,
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
            Fixture sut = new Fixture();
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
            Fixture sut = new Fixture();
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
            Fixture sut = new Fixture();
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
            Fixture sut = new Fixture();
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
            string result = sut.CreateAnonymous<string>();
            Assert.Equal<string>(expectedText, result);
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
            PropertyHolder<string> result = sut.CreateAnonymous<PropertyHolder<string>>();
            // Verify outcome
            Assert.Equal<string>(expectedValue, result.Property);
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
            int result = sut.CreateAnonymous(expectedValue);
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
            var result = sut.CreateAnonymous<PropertyHolder<PropertyHolder<string>>>();
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
            mock.OnCommand = (x1, x2, x3) => Assert.Equal<string>(expectedText, x3);
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
            var expectedOS = new OperatingSystem(PlatformID.Win32NT, new Version(5, 0));

            var sut = new Fixture();
            sut.Register<OperatingSystem>(() => expectedOS);

            var mock = new CommandMock<int?, DateTime, TimeSpan, OperatingSystem>();
            mock.OnCommand = (x1, x2, x3, x4) => Assert.Equal<OperatingSystem>(expectedOS, x4);
            // Exercise system
            sut.Do((int? x1, DateTime x2, TimeSpan x3, OperatingSystem x4) => mock.Command(x1, x2, x3, x4));
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
            mock.OnQuery = (x1, x2, x3) => { Assert.Equal<string>(expectedText, x3); return 111.11m; };
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

            var mock = new QueryMock<int, float, DayOfWeek, string, LoaderOptimization>();
            mock.OnQuery = (x1, x2, x3, x4) => { Assert.Equal<DayOfWeek>(expectedDayOfWeek, x3); return LoaderOptimization.MultiDomain; };
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

            var mock = new QueryMock<Version, ushort, string, int, PlatformID>();
            mock.OnQuery = (x1, x2, x3, x4) => { Assert.Equal<int>(expectedNumber, x4); return PlatformID.WinCE; };
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
        public void FromFactoryWithOneParameterWillRespectPreviousCustomizations()
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
            Assert.Equal<string>(expectedText, result.Parameter.Property);
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
                .CreateAnonymous();
            // Verify outcome
            Assert.Equal<string>(expectedText, result.Parameter.Property);
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
                .CreateAnonymous();
            // Verify outcome
            Assert.Equal<string>(expectedText, result.Parameter.Property);
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
                .CreateAnonymous();
            // Verify outcome
            Assert.Equal<string>(expectedText, result.Parameter.Property);
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
            var result = sut.CreateAnonymous<SingleParameterType<string>>();
            // Verify outcome
            Assert.Equal<string>(expectedText, result.Parameter);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWillNotThrowWhenTypeHasIndexedProperty()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.CreateAnonymous<IndexedPropertyHolder<object>>();
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
            var result = sut.CreateAnonymous<PropertyHolder<object>>();
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
            var result = sut.CreateAnonymous<PropertyHolder<object>>();
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
            sut.CreateAnonymous<PropertyHolder<AbstractType>>();
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
            sut.CreateAnonymous<PropertyHolder<AbstractType>>();
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
            var result = sut.CreateAnonymous<PropertyHolder<AbstractType>>().Property;
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
            var result = sut.CreateAnonymous<Guid>();
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
            var result = sut.CreateAnonymous<PropertyHolder<DateTime>>().Property;
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
            var result = sut.CreateAnonymous("Something else");
            // Verify outcome
            Assert.Equal<string>(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void FreezeWithNullTranformationThrows()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Freeze((Func<ICustomizationComposer<object>, ISpecimenBuilderComposer>)null));
            // Teardown
        }

        [Fact]
        public void FreezeBuiltInstanceWillCauseFixtureToKeepReturningTheFrozenInstance()
        {
            // Fixture setup
            var sut = new Fixture();
            var frozen = sut.Freeze<DoublePropertyHolder<DateTime, Guid>>(ob => ob.OmitAutoProperties().With(x => x.Property1));
            // Exercise system
            var result = sut.CreateAnonymous<DoublePropertyHolder<DateTime, Guid>>();
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
        public void OmitAutoPropertiesFollowedByOptInWillNotSetOtherProperties()
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
        public void CreateAnonymousWillThrowOnReferenceRecursionPoint()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            Assert.Throws<ObjectCreationException>(() =>
                sut.CreateAnonymous<RecursionTestObjectWithReferenceOutA>());
        }

        [Fact]
        public void CreateAnonymousWillThrowOnConstructorRecursionPoint()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            Assert.Throws<ObjectCreationException>(() =>
                sut.CreateAnonymous<RecursionTestObjectWithConstructorReferenceOutA>());
        }

        [Fact]
        public void BuildWithThrowingRecursionHandlerWillThrowOnReferenceRecursionPoint()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            Assert.Throws<ObjectCreationException>(() =>
                new SpecimenContext(
                    new ThrowingRecursionGuard(
                        sut.Build<RecursionTestObjectWithReferenceOutA>().Compose()
                    )
                ).CreateAnonymous<RecursionTestObjectWithReferenceOutA>());
        }

        [Fact]
        public void BuildWithThrowingRecursionHandlerWillThrowOnConstructorRecursionPoint()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            Assert.Throws<ObjectCreationException>(() =>
                new SpecimenContext(
                    new ThrowingRecursionGuard(
                        sut.Build<RecursionTestObjectWithConstructorReferenceOutA>().Compose()
                    )
                ).CreateAnonymous<RecursionTestObjectWithConstructorReferenceOutA>());
        }

        [Fact]
        public void BuildWithNullRecursionHandlerWillCreateNullOnRecursionPoint()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = new SpecimenContext(
                new NullRecursionGuard(
                    sut.Build<RecursionTestObjectWithConstructorReferenceOutA>().Compose()
                )
            ).CreateAnonymous<RecursionTestObjectWithConstructorReferenceOutA>();
            // Verify outcome
            Assert.Null(result.ReferenceToB.ReferenceToA);
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
            var result = sut.CreateAnonymous<PropertyHolder<string>>();
            Assert.Null(result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousOnReqisteredParameterlessFuncWillReturnInstanceWithoutAutoProperties()
        {
            // Fixture setup
            var item = new PropertyHolder<string>();
            var sut = new Fixture();
            // Exercise system
            sut.Register(() => item);
            // Verify outcome
            var result = sut.CreateAnonymous<PropertyHolder<string>>();
            Assert.Null(result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousOnReqisteredSingleParameterFuncWillReturnInstanceWithoutAutoProperties()
        {
            // Fixture setup
            var item = new PropertyHolder<string>();
            var sut = new Fixture();
            // Exercise system
            sut.Register((object obj) => item);
            // Verify outcome
            var result = sut.CreateAnonymous<PropertyHolder<string>>();
            Assert.Null(result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousOnReqisteredDoubleParameterFuncWillReturnInstanceWithoutAutoProperties()
        {
            // Fixture setup
            var item = new PropertyHolder<string>();
            var sut = new Fixture();
            // Exercise system
            sut.Register((object obj1, object obj2) => item);
            // Verify outcome
            var result = sut.CreateAnonymous<PropertyHolder<string>>();
            Assert.Null(result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousOnReqisteredTripleParameterFuncWillReturnInstanceWithoutAutoProperties()
        {
            // Fixture setup
            var item = new PropertyHolder<string>();
            var sut = new Fixture();
            // Exercise system
            sut.Register((object obj1, object obj2, object obj3) => item);
            // Verify outcome
            var result = sut.CreateAnonymous<PropertyHolder<string>>();
            Assert.Null(result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousOnReqisteredQuadrupleParameterFuncWillReturnInstanceWithoutAutoProperties()
        {
            // Fixture setup
            var item = new PropertyHolder<string>();
            var sut = new Fixture();
            // Exercise system
            sut.Register((object obj1, object obj2, object obj3, object obj4) => item);
            // Verify outcome
            var result = sut.CreateAnonymous<PropertyHolder<string>>();
            Assert.Null(result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWithOmitAutoPropertiesWillNotAssignProperty()
        {
            // Fixture setup
            Fixture sut = new Fixture() { OmitAutoProperties = true };
            // Exercise system
            PropertyHolder<string> result = sut.CreateAnonymous<PropertyHolder<string>>();
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
            PropertyHolder<object> result = builder.CreateAnonymous();
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
            var expectedResult = sut.CreateAnonymous<PropertyHolder<string>>();
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
            var expectedResult = sut.CreateAnonymous<PropertyHolder<string>>();
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
        public void BuildFromSeedWillReturnCorrectResult()
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
        public void BuildFromSeedWillCreateUsingCorrectSeed()
        {
            // Fixture setup
            var sut = new Fixture();
            var seed = new object();

            var verified = false;
            Func<object, object> mock = s => verified = seed.Equals(s);
            // Exercise system
            sut.Build<object>().FromSeed(mock).CreateAnonymous(seed);
            // Verify outcome
            Assert.True(verified, "Mock verified");
            // Teardown
        }

        [Fact]
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
        public void BuildWithWillSetPropertyOnCreatedObject()
        {
            // Fixture setup
            string expectedText = "Anonymous text";
            var sut = new Fixture();
            // Exercise system
            PropertyHolder<string> result = sut.Build<PropertyHolder<string>>().With(ph => ph.Property, expectedText).CreateAnonymous();
            // Verify outcome
            Assert.Equal<string>(expectedText, result.Property);
            // Teardown
        }

        [Fact]
        public void BuildWithWillSetFieldOnCreatedObject()
        {
            // Fixture setup
            string expectedText = "Anonymous text";
            var fixture = new Fixture();
            // Exercise system
            FieldHolder<string> result = fixture.Build<FieldHolder<string>>().With(fh => fh.Field, expectedText).CreateAnonymous();
            // Verify outcome
            Assert.Equal<string>(expectedText, result.Field);
            // Teardown
        }

        [Fact]
        public void BuildAnonymousWithWillAssignPropertyEvenInCombinationWithOmitAutoProperties()
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
        public void BuildWithoutWillIgnorePropertyOnCreatedObject()
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
        public void BuildWithoutWillIgnorePropertyOnCreatedObjectEvenInCombinationWithWithAutoProperties()
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
        public void BuildWithoutWillIgnoreFieldOnCreatedObject()
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
        public void BuildWithoutWillNotIgnoreOtherPropertyOnCreatedObject()
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
        public void BuildWithoutWillNotIgnoreOtherFieldOnCreatedObject()
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
        public void BuildAndOmitAutoPropertiesWillNotAutoPopulateProperty()
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
        public void BuildWithAutoPropertiesWillAutoPopulateProperty()
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
        public void BuildAndDoWillPerformOperationOnCreatedObject()
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
                .CreateAnonymous();
            // Verify outcome
            Assert.Equal<int>(expectedValue, result.Property);
            // Teardown
        }

        [Fact]
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
        public void BuildAndOmitAutoPropetiesWillNotMutateSut()
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
        public void BuildWithAutoPropetiesWillNotMutateSut()
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
        public void BuildWithoutWillNotMutateSut()
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
        public void BuildFromFactoryStillAppliesAutoProperties()
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
        public void BuildOverwritesPreviousFactoryBasedCustomization()
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
        public void NewestCustomizationWins()
        {
            // Fixture setup
            var sut = new Fixture();
            sut.Customize<string>(c => c.FromFactory(() => "ploeh"));

            var expectedResult = "fnaah";
            // Exercise system
            sut.Customize<string>(c => c.FromFactory(() => expectedResult));
            var result = sut.CreateAnonymous<string>();
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
            var result = sut.Build<object>().Compose();
            // Verify outcome
            var comparer = new TaggedNodeComparer(new TrueComparer<ISpecimenBuilder>());
            var composite = Assert.IsAssignableFrom<CompositeSpecimenBuilder>(result);
            Assert.Equal(new TaggedNode(1), composite.Builders.First(), comparer);
            // Teardown
        }

        [Fact]
        public void BuildAbstractClassThrows()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system and verify outcome
            Assert.Throws<ObjectCreationException>(() =>
                sut.Build<AbstractType>().CreateAnonymous());
            // Teardown
        }

        [Fact]
        public void BuildAbstractTypeUsingStronglyTypedFactoryIsPossible()
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
        public void BuildAbstractTypeUsingBuilderIsPossible()
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
        public void BuildAbstractTypeCorrectlyAppliesProperty()
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
        public void RegisterNullWillAssignCorrectPickedPropertyValue()
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
        public void AddingTracingBehaviorWillTraceDiagnostics()
        {
            // Fixture setup
            using (var writer = new StringWriter())
            {
                var sut = new Fixture();
                sut.Behaviors.Add(new TracingBehavior(writer));
                // Exercise system
                sut.CreateAnonymous<int>();
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
            var result = sut.CreateAnonymous<TriState>();
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
            Assert.Throws<IllegalRequestException>(() =>
                sut.CreateAnonymous<IntPtr>());
            // Teardown
        }

        [Fact]
        public void BuildWithOverriddenVirtualPropertyCorrectlySetsProperty()
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
        public void CreateAnonymousEnumerableWithoutCustomizationThrows()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system and verify outcome
            Assert.Throws<ObjectCreationException>(() =>
                sut.CreateAnonymous<IEnumerable<decimal>>());
            // Teardown
        }

        [Fact]
        public void CreateAnonymousEnumerableWhenEnumerableRelayIsPresentReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new Fixture();
            sut.ResidueCollectors.Add(new EnumerableRelay());
            // Exercise system
            var result = sut.CreateAnonymous<IEnumerable<decimal>>();
            // Verify outcome
            Assert.True(result.Any());
            // Teardown
        }

        [Fact]
        public void CreateAnonymousListDoesNotFillWithItemsPerDefault()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.CreateAnonymous<List<string>>();
            // Verify outcome
            Assert.False(result.Any());
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
            var result = sut.CreateAnonymous<List<string>>();
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
            var result = sut.CreateAnonymous<HashSet<float>>();
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
            var result = sut.CreateAnonymous<IList<int>>();
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
            var result = sut.CreateAnonymous<ICollection<Version>>();
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
            var result = sut.CreateAnonymous<Collection<OperatingSystem>>();
            // Verify outcome
            Assert.True(result.Any());
            // Teardown
        }

        [Fact]
        public void CreateAnonymousDictionaryWithCustomizationsReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new Fixture();
            sut.Customizations.Add(new FilteringSpecimenBuilder(new Postprocessor(new MethodInvoker(new ModestConstructorQuery()), DictionaryFiller.AddMany), new DictionarySpecification()));
            // Exercise system
            var result = sut.CreateAnonymous<Dictionary<int, string>>();
            // Verify outcome
            Assert.True(result.Any());
            // Teardown
        }

        [Fact]
        public void CreateAnonymousIDictionaryWithCustomizationsReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new Fixture();
            sut.Customizations.Add(new FilteringSpecimenBuilder(new Postprocessor(new MethodInvoker(new ModestConstructorQuery()), DictionaryFiller.AddMany), new DictionarySpecification()));
            sut.ResidueCollectors.Add(new DictionaryRelay());
            // Exercise system
            var result = sut.CreateAnonymous<IDictionary<TimeSpan, Version>>();
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
            var version2 = fixture.CreateAnonymous<Version>();
            var version1 = fixture.Freeze<Version>();
            // Exercise system
            fixture.Inject(version2);
            var actual = fixture.CreateAnonymous<Version>();
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
            var actual = fixture.CreateAnonymous<Version>();
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
                new AutoPropertiesCommand().Execute,
                new AnyTypeSpecification()));
            // Exercise system
            var result = fixture.CreateAnonymous<ConcreteType>();
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
            var result = fixture.CreateAnonymous<Type>();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
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
    }
}