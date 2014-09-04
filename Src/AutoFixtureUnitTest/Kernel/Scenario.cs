using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Dsl;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class Scenario
    {
        [Fact]
        public void CreateSingleStringParameterizedType()
        {
            // Fixture setup
            var container = Scenario.CreateContainer();
            // Exercise system
            var result = (SingleParameterType<string>)container.Resolve(typeof(SingleParameterType<string>));
            // Verify outcome
            var name = new TextGuidRegex().GetText(result.Parameter);
            string guidString = new TextGuidRegex().GetGuid(result.Parameter);
            Guid g = new Guid(guidString);
            Assert.Equal("parameter", name);
            Assert.NotEqual<Guid>(Guid.Empty, g);
            // Teardown
        }

        [Fact]
        public void CreateDoubleStringParameterizedType()
        {
            // Fixture setup
            var container = Scenario.CreateContainer();
            // Exercise system
            var result = (DoubleParameterType<string, string>)container.Resolve(typeof(DoubleParameterType<string, string>));
            // Verify outcome
            Assert.False(string.IsNullOrEmpty(result.Parameter1), "Parameter1");
            Assert.False(string.IsNullOrEmpty(result.Parameter2), "Parameter2");
            // Teardown
        }

        [Fact]
        public void CreateStringAndIntegerParameterizedType()
        {
            // Fixture setup
            var container = Scenario.CreateContainer();
            // Exercise system
            var result = (DoubleParameterType<string, int>)container.Resolve(typeof(DoubleParameterType<string, int>));
            // Verify outcome
            Assert.False(string.IsNullOrEmpty(result.Parameter1), "Parameter11");
            Assert.NotEqual(0, result.Parameter2);
            // Teardown
        }

        [Fact]
        public void CreateDecimalAndBooleanParameterizedType()
        {
            // Fixture setup
            var container = Scenario.CreateContainer();
            // Exercise system
            var result = (DoubleParameterType<decimal, bool>)container.Resolve(typeof(DoubleParameterType<decimal, bool>));
            // Verify outcome
            Assert.Equal(1m, result.Parameter1);
            Assert.True(result.Parameter2, "Parameter2");
            // Teardown
        }

        [Fact]
        public void CreateNestedType()
        {
            // Fixture setup
            var container = Scenario.CreateContainer();
            // Exercise system
            var result = (DoubleParameterType<DoubleParameterType<int, Guid>, DoubleParameterType<decimal, bool>>)container.Resolve(
                typeof(DoubleParameterType<DoubleParameterType<int, Guid>, DoubleParameterType<decimal, bool>>));
            // Verify outcome
            Assert.Equal(1, result.Parameter1.Parameter1);
            Assert.NotEqual(default(Guid), result.Parameter1.Parameter2);
            Assert.Equal(1m, result.Parameter2.Parameter1);
            Assert.Equal(true, result.Parameter2.Parameter2);
            // Teardown
        }

        [Fact][Obsolete]
        public void CreateDoubleMixedParameterizedTypeWithNumberBasedStringGeneratorObsolete()
        {
            // Fixture setup
            var intGenerator = new Int32SequenceGenerator();
            var builder = new CompositeSpecimenBuilder(
                intGenerator,
                new StringGenerator(() => intGenerator.CreateAnonymous()),
                new Int64SequenceGenerator(),
                new DecimalSequenceGenerator(),
                new BooleanSwitch(),
                new GuidGenerator(),
                new MethodInvoker(new ModestConstructorQuery()),
                new ParameterRequestRelay(),
                new StringSeedRelay(),
                new SeedIgnoringRelay());
            var container = new SpecimenContext(builder);
            // Exercise system
            var result = (TripleParameterType<int, string, int>)container.Resolve(typeof(TripleParameterType<int, string, int>));
            // Verify outcome
            Assert.Equal(1, result.Parameter1);
            Assert.Equal("parameter22", result.Parameter2);
            Assert.Equal(3, result.Parameter3);
            // Teardown
        }

        [Fact]
        public void CreateDoubleMixedParameterizedTypeWithNumberBasedStringGenerator()
        {
            // Fixture setup
            var intGenerator = new Int32SequenceGenerator();
            var builder = new CompositeSpecimenBuilder(
                intGenerator,
                new StringGenerator(() => intGenerator.Create()),
                new Int64SequenceGenerator(),
                new DecimalSequenceGenerator(),
                new BooleanSwitch(),
                new GuidGenerator(),
                new MethodInvoker(new ModestConstructorQuery()),
                new ParameterRequestRelay(),
                new StringSeedRelay(),
                new SeedIgnoringRelay());
            var container = new SpecimenContext(builder);
            // Exercise system
            var result = (TripleParameterType<int, string, int>)container.Resolve(typeof(TripleParameterType<int, string, int>));
            // Verify outcome
            Assert.Equal(1, result.Parameter1);
            Assert.Equal("parameter22", result.Parameter2);
            Assert.Equal(3, result.Parameter3);
            // Teardown
        }

        [Fact]
        public void CreateAndAddProperyValues()
        {
            // Fixture setup
            var ctorInvoker = new MethodInvoker(new ModestConstructorQuery());
            var strCmd = new BindingCommand<DoublePropertyHolder<string, int>, string>(ph => ph.Property1);
            var intCmd = new BindingCommand<DoublePropertyHolder<string, int>, int>(ph => ph.Property2);
            var strPostprocessor = new Postprocessor<DoublePropertyHolder<string, int>>(ctorInvoker, strCmd);
            var intPostprocessor = new Postprocessor<DoublePropertyHolder<string, int>>(strPostprocessor, intCmd);

            var builder = new CompositeSpecimenBuilder(
                new FilteringSpecimenBuilder(intPostprocessor, new ExactTypeSpecification(typeof(DoublePropertyHolder<string, int>))),
                Scenario.CreateAutoPropertyBuilder());
            var container = new SpecimenContext(builder);
            // Exercise system
            var result = container.Resolve(typeof(DoublePropertyHolder<string, int>));
            // Verify outcome
            var actual = Assert.IsAssignableFrom<DoublePropertyHolder<string, int>>(result);
            Assert.False(string.IsNullOrEmpty(actual.Property1), "Property1");
            Assert.Equal(1, actual.Property2);
            // Teardown
        }

        [Fact]
        public void CreateUsingBasicAutoPropertiesFunctionality()
        {
            // Fixture setup
            var container = Scenario.CreateContainer();
            // Exercise system
            var result = container.Resolve(typeof(DoublePropertyHolder<int, int>));
            // Verify outcome
            var actual = Assert.IsAssignableFrom<DoublePropertyHolder<int, int>>(result);
            Assert.Equal(1, actual.Property1);
            Assert.Equal(2, actual.Property2);
            // Teardown
        }

        [Fact]
        public void CreateNestedStringTypeWithAutoProperties()
        {
            // Fixture setup
            var container = Scenario.CreateContainer();
            // Exercise system
            var result = container.Resolve(typeof(DoublePropertyHolder<DoublePropertyHolder<string, string>, DoublePropertyHolder<string, string>>));
            // Verify outcome
            var actual = Assert.IsAssignableFrom<DoublePropertyHolder<DoublePropertyHolder<string, string>, DoublePropertyHolder<string, string>>>(result);
            Assert.False(string.IsNullOrEmpty(actual.Property1.Property1));
            Assert.False(string.IsNullOrEmpty(actual.Property1.Property2));
            Assert.False(string.IsNullOrEmpty(actual.Property2.Property1));
            Assert.False(string.IsNullOrEmpty(actual.Property2.Property2));
            // Teardown
        }

        [Fact]
        public void CreateNestedIntegerTypeWithAutoProperties()
        {
            // Fixture setup
            var container = Scenario.CreateContainer();
            // Exercise system
            var result = container.Resolve(typeof(DoublePropertyHolder<DoublePropertyHolder<int, int>, DoublePropertyHolder<int, int>>));
            // Verify outcome
            var actual = Assert.IsAssignableFrom<DoublePropertyHolder<DoublePropertyHolder<int, int>, DoublePropertyHolder<int, int>>>(result);
            Assert.Equal(1, actual.Property1.Property1);
            Assert.Equal(2, actual.Property1.Property2);
            Assert.Equal(3, actual.Property2.Property1);
            Assert.Equal(4, actual.Property2.Property2);
            // Teardown
        }

        [Fact]
        public void CombineExplictPropertyWithAutoProperties()
        {
            // Fixture setup
            var expectedText = "Fnaah";

            var specifiedCommand = new BindingCommand<DoublePropertyHolder<string, int>, string>(ph => ph.Property1, expectedText);
            var reservedProperty = new InverseRequestSpecification(specifiedCommand);

            var customizedBuilder = new Postprocessor<DoublePropertyHolder<string, int>>(
                new Postprocessor<DoublePropertyHolder<string, int>>(
                    new MethodInvoker(new ModestConstructorQuery()),
                    specifiedCommand),
                new AutoPropertiesCommand<DoublePropertyHolder<string, int>>(reservedProperty),
                new AnyTypeSpecification());

            var builder = new CompositeSpecimenBuilder(
                customizedBuilder,
                Scenario.CreateAutoPropertyBuilder());
            var container = new SpecimenContext(builder);
            // Exercise system
            var result = container.Resolve(typeof(DoublePropertyHolder<string, int>));
            // Verify outcome
            var actual = Assert.IsAssignableFrom<DoublePropertyHolder<string, int>>(result);
            Assert.Equal(expectedText, actual.Property1);
            Assert.Equal(1, actual.Property2);
            // Teardown
        }

        [Fact]
        public void RequestFiniteSequenceReturnsCorrectResult()
        {
            // Fixture setup
            var container = Scenario.CreateContainer();
            // Exercise system
            var result = container.Resolve(new FiniteSequenceRequest(typeof(int), 10));
            // Verify outcome
            var actual = Assert.IsAssignableFrom<IEnumerable<object>>(result);
            Assert.True(Enumerable.Range(1, 10).Cast<object>().SequenceEqual(actual));
            // Teardown
        }

        [Fact]
        public void RequestManyReturnsCorrectResult()
        {
            // Fixture setup
            var container = Scenario.CreateContainer();
            // Exercise system
            var result = container.Resolve(new MultipleRequest(typeof(int)));
            // Verify outcome
            var actual = Assert.IsAssignableFrom<IEnumerable<object>>(result);
            Assert.True(Enumerable.Range(1, 3).Cast<object>().SequenceEqual(actual));
            // Teardown
        }

        [Fact][Obsolete]
        public void CreateAnonymousReturnsCorrectResult()
        {
            // Fixture setup
            var container = Scenario.CreateContainer();
            // Exercise system
            var result = container.CreateAnonymous<int>();
            // Verify outcome
            Assert.Equal(1, result);
            // Teardown
        }

        [Fact]
        public void CreateReturnsCorrectResult()
        {
            // Fixture setup
            var container = Scenario.CreateContainer();
            // Exercise system
            var result = container.Create<int>();
            // Verify outcome
            Assert.Equal(1, result);
            // Teardown
        }

        [Fact][Obsolete]
        public void CreateAnonymousWithSeedReturnsCorrectResult()
        {
            // Fixture setup
            var container = Scenario.CreateContainer();
            // Exercise system
            var result = container.CreateAnonymous("Seed");
            // Verify outcome
            Assert.Contains("Seed", result);
            // Teardown
        }

        [Fact]
        public void CreateWithSeedReturnsCorrectResult()
        {
            // Fixture setup
            var container = Scenario.CreateContainer();
            // Exercise system
            var result = container.Create("Seed");
            // Verify outcome
            Assert.Contains("Seed", result);
            // Teardown
        }

        [Fact]
        public void CreateManyReturnsCorrectResult()
        {
            // Fixture setup
            var container = Scenario.CreateContainer();
            // Exercise system
            var result = container.CreateMany<decimal>();
            // Verify outcome
            Assert.True(Enumerable.Range(1, 3).Select(i => (decimal)i).SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void CreateManyWithSeedReturnsCorrectResult()
        {
            // Fixture setup
            var container = Scenario.CreateContainer();
            // Exercise system
            var result = container.CreateMany("Seed").ToList();
            // Verify outcome
            Assert.NotEmpty(result);
            Assert.True(result.All(s => s.Contains("Seed")));
            // Teardown
        }

        [Fact]
        public void CreateManyWithCountReturnsCorrectResult()
        {
            // Fixture setup
            var count = 8;
            var container = Scenario.CreateContainer();
            // Exercise system
            var result = container.CreateMany<long>(count);
            // Verify outcome
            Assert.True(Enumerable.Range(1, count).Select(i => (long)i).SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void CreateManyWithSeedAndCountReturnsCorrectResult()
        {
            // Fixture setup
            var seed = "Seed";
            var count = 2;
            var container = Scenario.CreateContainer();
            // Exercise system
            var result = container.CreateMany(seed, count).ToList();
            // Verify outcome
            Assert.Equal(count, result.Count);
            Assert.True(result.All(s => s.Contains(seed)));
            // Teardown
        }

        [Fact][Obsolete]
        public void ComposeWithValueReturnsCorrectResultObsolete()
        {
            // Fixture setup
            var expectedValue = 9;
            var customBuilder = SpecimenBuilderNodeFactory.CreateComposer<PropertyHolder<int>>()
                .With(x => x.Property, expectedValue);
            var builder = new CompositeSpecimenBuilder(
                customBuilder,
                Scenario.CreateCoreBuilder());
            // Exercise system
            var result = new SpecimenContext(builder).CreateAnonymous<PropertyHolder<int>>();
            // Verify outcome
            Assert.Equal(expectedValue, result.Property);
            // Teardown
        }

        [Fact]
        public void ComposeWithValueReturnsCorrectResult()
        {
            // Fixture setup
            var expectedValue = 9;
            var customBuilder = SpecimenBuilderNodeFactory.CreateComposer<PropertyHolder<int>>()
                .With(x => x.Property, expectedValue);
            var builder = new CompositeSpecimenBuilder(
                customBuilder,
                Scenario.CreateCoreBuilder());
            // Exercise system
            var result = new SpecimenContext(builder).Create<PropertyHolder<int>>();
            // Verify outcome
            Assert.Equal(expectedValue, result.Property);
            // Teardown
        }

        [Fact][Obsolete]
        public void ComposeWithAutoPropertiesAndExplicitPropertyObsolete()
        {
            // Fixture setup
            var customBuilder = SpecimenBuilderNodeFactory.CreateComposer<DoublePropertyHolder<int, int>>()
                .WithAutoProperties()
                .With(x => x.Property1, 8);
            var builder = new CompositeSpecimenBuilder(
                customBuilder,
                Scenario.CreateCoreBuilder());
            // Exercise system
            var result = new SpecimenContext(builder).CreateAnonymous<DoublePropertyHolder<int, int>>();
            // Verify outcome
            Assert.Equal(8, result.Property1);
            Assert.Equal(1, result.Property2);
            // Teardown
        }

        [Fact]
        public void ComposeWithAutoPropertiesAndExplicitProperty()
        {
            // Fixture setup
            var customBuilder = SpecimenBuilderNodeFactory.CreateComposer<DoublePropertyHolder<int, int>>()
                .WithAutoProperties()
                .With(x => x.Property1, 8);
            var builder = new CompositeSpecimenBuilder(
                customBuilder,
                Scenario.CreateCoreBuilder());
            // Exercise system
            var result = new SpecimenContext(builder).Create<DoublePropertyHolder<int, int>>();
            // Verify outcome
            Assert.Equal(8, result.Property1);
            Assert.Equal(1, result.Property2);
            // Teardown
        }

        [Fact][Obsolete]
        public void ComposeWithAutoPropertiesObsolete()
        {
            // Fixture setup
            var customBuilder = SpecimenBuilderNodeFactory.CreateComposer<DoublePropertyHolder<int, int>>()
                .WithAutoProperties();
            var builder = new CompositeSpecimenBuilder(
                customBuilder,
                Scenario.CreateCoreBuilder());
            // Exercise system
            var result = new SpecimenContext(builder).CreateAnonymous<DoublePropertyHolder<int, int>>();
            // Verify outcome
            Assert.Equal(1, result.Property1);
            Assert.Equal(2, result.Property2);
            // Teardown
        }

        [Fact]
        public void ComposeWithAutoProperties()
        {
            // Fixture setup
            var customBuilder = SpecimenBuilderNodeFactory.CreateComposer<DoublePropertyHolder<int, int>>()
                .WithAutoProperties();
            var builder = new CompositeSpecimenBuilder(
                customBuilder,
                Scenario.CreateCoreBuilder());
            // Exercise system
            var result = new SpecimenContext(builder).Create<DoublePropertyHolder<int, int>>();
            // Verify outcome
            Assert.Equal(1, result.Property1);
            Assert.Equal(2, result.Property2);
            // Teardown
        }

        [Fact][Obsolete]
        public void ComposeComplexObjectWithAutoPropertiesAndSomeCustomizationsObsolete()
        {
            // Fixture setup
            var builder = new CompositeSpecimenBuilder(
                SpecimenBuilderNodeFactory.CreateComposer<DoublePropertyHolder<long, long>>()
                    .With(x => x.Property2, 43)
                    .WithAutoProperties(),
                SpecimenBuilderNodeFactory.CreateComposer<DoublePropertyHolder<int, string>>()
                    .OmitAutoProperties()
                    .With(x => x.Property1),
                SpecimenBuilderNodeFactory.CreateComposer<DoublePropertyHolder<DoublePropertyHolder<long, long>, DoublePropertyHolder<int, string>>>()
                    .WithAutoProperties(),
                Scenario.CreateCoreBuilder());
            // Exercise system
            var result = new SpecimenContext(builder).CreateAnonymous<DoublePropertyHolder<DoublePropertyHolder<long, long>, DoublePropertyHolder<int, string>>>();
            // Verify outcome
            Assert.Equal(1, result.Property1.Property1);
            Assert.Equal(43, result.Property1.Property2);
            Assert.Equal(1, result.Property2.Property1);
            Assert.Null(result.Property2.Property2);
            // Teardown
        }

        [Fact]
        public void ComposeComplexObjectWithAutoPropertiesAndSomeCustomizations()
        {
            // Fixture setup
            var builder = new CompositeSpecimenBuilder(
                SpecimenBuilderNodeFactory.CreateComposer<DoublePropertyHolder<long, long>>()
                    .With(x => x.Property2, 43)
                    .WithAutoProperties(),
                SpecimenBuilderNodeFactory.CreateComposer<DoublePropertyHolder<int, string>>()
                    .OmitAutoProperties()
                    .With(x => x.Property1),
                SpecimenBuilderNodeFactory.CreateComposer<DoublePropertyHolder<DoublePropertyHolder<long, long>, DoublePropertyHolder<int, string>>>()
                    .WithAutoProperties(),
                Scenario.CreateCoreBuilder());
            // Exercise system
            var result = new SpecimenContext(builder).Create<DoublePropertyHolder<DoublePropertyHolder<long, long>, DoublePropertyHolder<int, string>>>();
            // Verify outcome
            Assert.Equal(1, result.Property1.Property1);
            Assert.Equal(43, result.Property1.Property2);
            Assert.Equal(1, result.Property2.Property1);
            Assert.Null(result.Property2.Property2);
            // Teardown
        }

        [Fact][Obsolete]
        public void CustomDoSetsCorrectPropertyObsolete()
        {
            // Fixture setup
            var builder = new CompositeSpecimenBuilder(
                SpecimenBuilderNodeFactory.CreateComposer<PropertyHolder<decimal>>().OmitAutoProperties().Do(x => x.SetProperty(6789)),
                Scenario.CreateCoreBuilder());
            // Exercise system
            var result = new SpecimenContext(builder).CreateAnonymous<SingleParameterType<PropertyHolder<decimal>>>();
            // Verify outcome
            Assert.Equal(6789, result.Parameter.Property);
            // Teardown
        }

        [Fact]
        public void CustomDoSetsCorrectProperty()
        {
            // Fixture setup
            var builder = new CompositeSpecimenBuilder(
                SpecimenBuilderNodeFactory.CreateComposer<PropertyHolder<decimal>>().OmitAutoProperties().Do(x => x.SetProperty(6789)),
                Scenario.CreateCoreBuilder());
            // Exercise system
            var result = new SpecimenContext(builder).Create<SingleParameterType<PropertyHolder<decimal>>>();
            // Verify outcome
            Assert.Equal(6789, result.Parameter.Property);
            // Teardown
        }

        [Fact][Obsolete]
        public void ComposeWithoutCorrectlyCreatesSpecimenObsolete()
        {
            // Fixture setup
            var builder = new CompositeSpecimenBuilder(
                SpecimenBuilderNodeFactory.CreateComposer<DoubleFieldHolder<string, int>>().WithAutoProperties().Without(x => x.Field1),
                Scenario.CreateCoreBuilder());
            // Exercise system
            var result = new SpecimenContext(builder).CreateAnonymous<DoubleFieldHolder<string, int>>();
            // Verify outcome
            Assert.Null(result.Field1);
            Assert.Equal(1, result.Field2);
            // Teardown
        }

        [Fact]
        public void ComposeWithoutCorrectlyCreatesSpecimen()
        {
            // Fixture setup
            var builder = new CompositeSpecimenBuilder(
                SpecimenBuilderNodeFactory.CreateComposer<DoubleFieldHolder<string, int>>().WithAutoProperties().Without(x => x.Field1),
                Scenario.CreateCoreBuilder());
            // Exercise system
            var result = new SpecimenContext(builder).Create<DoubleFieldHolder<string, int>>();
            // Verify outcome
            Assert.Null(result.Field1);
            Assert.Equal(1, result.Field2);
            // Teardown
        }

        [Fact][Obsolete]
        public void CustomizeFromFactoryCorrectlyResolvesSpecimenObsolete()
        {
            // Fixture setup
            var instance = new PropertyHolder<float> { Property = 89 };
            var builder = new CompositeSpecimenBuilder(
                SpecimenBuilderNodeFactory.CreateComposer<PropertyHolder<float>>().FromFactory(() => instance).OmitAutoProperties(),
                Scenario.CreateCoreBuilder());
            // Exercise system
            var result = new SpecimenContext(builder).CreateAnonymous<PropertyHolder<float>>();
            // Verify outcome
            Assert.Equal(instance, result);
            Assert.Equal(89, result.Property);
            // Teardown
        }

        [Fact]
        public void CustomizeFromFactoryCorrectlyResolvesSpecimen()
        {
            // Fixture setup
            var instance = new PropertyHolder<float> { Property = 89 };
            var builder = new CompositeSpecimenBuilder(
                SpecimenBuilderNodeFactory.CreateComposer<PropertyHolder<float>>().FromFactory(() => instance).OmitAutoProperties(),
                Scenario.CreateCoreBuilder());
            // Exercise system
            var result = new SpecimenContext(builder).Create<PropertyHolder<float>>();
            // Verify outcome
            Assert.Equal(instance, result);
            Assert.Equal(89, result.Property);
            // Teardown
        }

        [Fact][Obsolete]
        public void CustomizeAndComposeComplexTypeObsolete()
        {
            // Fixture setup
            // Exercise system
            var result = new CompositeNodeComposer<DoublePropertyHolder<int, decimal>>(
                new CompositeSpecimenBuilder(
                    SpecimenBuilderNodeFactory.CreateComposer<DoublePropertyHolder<int, decimal>>(),
                    Scenario.CreateAutoPropertyBuilder()
                    )
                ).With(x => x.Property2, 8m).WithAutoProperties().CreateAnonymous();
            // Verify outcome
            Assert.Equal(1, result.Property1);
            Assert.Equal(8, result.Property2);
            // Teardown
        }

        [Fact]
        public void CustomizeAndComposeComplexType()
        {
            // Fixture setup
            // Exercise system
            var result = new CompositeNodeComposer<DoublePropertyHolder<int, decimal>>(
                new CompositeSpecimenBuilder(
                    SpecimenBuilderNodeFactory.CreateComposer<DoublePropertyHolder<int, decimal>>(),
                    Scenario.CreateAutoPropertyBuilder()
                    )
                ).With(x => x.Property2, 8m).WithAutoProperties().Create();
            // Verify outcome
            Assert.Equal(1, result.Property1);
            Assert.Equal(8, result.Property2);
            // Teardown
        }

        [Fact]
        public void DisposeBehaviorDisposesSpecimen()
        {
            // Fixture setup
            var behavior = new DisposableTrackingBehavior();
            var fixture = new Fixture();
            fixture.Behaviors.Add(behavior);

            var disp = fixture.Create<DisposableSpy>();
            // Exercise system
            behavior.Dispose();
            // Verify outcome
            Assert.True(disp.Disposed);
            // Teardown
        }

        [Fact]
        public void DisposeCustomizationDisposesSpecimen()
        {
            // Fixture setup
            var customization = new DisposableTrackingCustomization();
            var fixture = new Fixture().Customize(customization);

            var disp = fixture.Create<DisposableSpy>();
            // Exercise system
            customization.Dispose();
            // Verify outcome
            Assert.True(disp.Disposed);
            // Teardown
        }

        [Fact]
        public void CustomizeFactoryWithMatchingByBaseType()
        {
            // Fixture setup
            var fixture = new Fixture();
            var expected = new ConcreteType();
            fixture.Customize<ConcreteType>(c => c
                   .FromFactory(() => expected)
                   .Match().ByBaseType());
            // Exercise system
            var actual = fixture.Create<AbstractType>();
            // Verify outcome
            Assert.Same(expected, actual);
            // Teardown
        }

        [Fact]
        public void CustomizeFactoryWithMatchingByExactType()
        {
            // Fixture setup
            var fixture = new Fixture();
            var expected = new ConcreteType();
            fixture.Customize<ConcreteType>(c => c
                   .FromFactory(() => expected)
                   .Match().ByExactType());
            // Exercise system
            var actual = fixture.Create<ConcreteType>();
            // Verify outcome
            Assert.Same(expected, actual);
            // Teardown
        }

        [Fact]
        public void CustomizeFactoryWithMatchingByBaseTypeOrExactType()
        {
            // Fixture setup
            var fixture = new Fixture();
            var expected = new ConcreteType();
            fixture.Customize<ConcreteType>(c => c
                .FromFactory(() => expected)
                .Match().ByBaseType().Or.ByExactType());
            // Exercise system
            // Verify outcome
            Assert.Same(expected, fixture.Create<AbstractType>());
            Assert.Same(expected, fixture.Create<ConcreteType>());
            // Teardown
        }

        [Fact]
        public void CustomizeFactoryWithMatchingByInterfaceOrExactType()
        {
            // Fixture setup
            var fixture = new Fixture();
            var expected = new NoopInterfaceImplementer();
            fixture.Customize<NoopInterfaceImplementer>(c => c
                .FromFactory(() => expected)
                .Match().ByInterfaces().Or.ByExactType());
            // Exercise system
            // Verify outcome
            Assert.Same(expected, fixture.Create<IInterface>());
            Assert.Same(expected, fixture.Create<NoopInterfaceImplementer>());
            // Teardown
        }

        [Fact]
        public void CustomizeFactoryWithMatchingByParameterName()
        {
            // Fixture setup
            var fixture = new Fixture();
            var expected = new object();
            fixture.Customize<ConcreteType>(c => c
                   .FromFactory(new MethodInvoker(new GreedyConstructorQuery()))
                   .OmitAutoProperties());
            fixture.Customize<object>(c => c
                   .FromFactory(() => expected)
                   .Match().ByParameterName("obj1"));
            // Exercise system
            var actual = fixture.Create<ConcreteType>().Property1;
            // Verify outcome
            Assert.Same(expected, actual);
            // Teardown
        }

        [Fact]
        public void CustomizeFactoryWithMatchingByPropertyName()
        {
            // Fixture setup
            var fixture = new Fixture();
            var expected = new object();
            fixture.Customize<object>(c => c
                   .FromFactory(() => expected)
                   .Match().ByPropertyName("Property"));
            // Exercise system
            var actual = fixture.Create<PropertyHolder<object>>().Property;
            // Verify outcome
            Assert.Same(expected, actual);
            // Teardown
        }

        [Fact]
        public void CustomizeFactoryWithMatchingByFieldName()
        {
            // Fixture setup
            var fixture = new Fixture();
            var expected = new object();
            fixture.Customize<object>(c => c
                   .FromFactory(() => expected)
                   .Match().ByFieldName("Field"));
            // Exercise system
            var actual = fixture.Create<FieldHolder<object>>().Field;
            // Verify outcome
            Assert.Same(expected, actual);
            // Teardown
        }

        [Fact]
        public void CustomizeFactoryWithMatchingByMultipleMemberNames()
        {
            // Fixture setup
            var fixture = new Fixture();
            var expected = "frozen";
            fixture.Customize<ConcreteType>(c => c
                   .FromFactory(new MethodInvoker(new GreedyConstructorQuery()))
                   .Without(s => s.Property1)
                   .Without(s => s.Property2));
            fixture.Customize<object>(c => c
                   .FromFactory(() => expected)
                   .Match()
                   .ByParameterName("obj1")
                   .Or.ByParameterName("obj2")
                   .Or.ByPropertyName("Property3"));
            // Exercise system
            var actual = fixture.Create<ConcreteType>();
            // Verify outcome
            Assert.Same(expected, actual.Property1);
            Assert.Same(expected, actual.Property2);
            Assert.Same(expected, actual.Property3);
            // Teardown
        }

        private static SpecimenContext CreateContainer()
        {
            var builder = Scenario.CreateAutoPropertyBuilder();
            var tracer = new TraceWriter(TestConsole.Out, new TracingBuilder(builder));
            return new SpecimenContext(tracer);
        }

        private static ISpecimenBuilder CreateAutoPropertyBuilder()
        {
            var builder = Scenario.CreateCoreBuilder();
            return new Postprocessor(builder, new AutoPropertiesCommand(), new AnyTypeSpecification());
        }

        private static CompositeSpecimenBuilder CreateCoreBuilder()
        {
            return new CompositeSpecimenBuilder(
                new Int32SequenceGenerator(),
                new Int64SequenceGenerator(),
                new StringGenerator(() => Guid.NewGuid()),
                new DecimalSequenceGenerator(),
                new BooleanSwitch(),
                new GuidGenerator(),
                new MethodInvoker(new ModestConstructorQuery()),
                new ParameterRequestRelay(),
                new PropertyRequestRelay(),
                new FieldRequestRelay(),
                new MultipleRelay { Count = 3 },
                new FiniteSequenceRelay(),
                new StringSeedRelay(),
                new SeedIgnoringRelay());
        }
    }
}