using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.Dsl;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class Scenario
    {
        [Fact]
        public void CreateSingleStringParameterizedType()
        {
            // Arrange
            var container = Scenario.CreateContainer();
            // Act
            var result = (SingleParameterType<string>)container.Resolve(typeof(SingleParameterType<string>));
            // Assert
            var name = new TextGuidRegex().GetText(result.Parameter);
            string guidString = new TextGuidRegex().GetGuid(result.Parameter);
            Guid g = new Guid(guidString);
            Assert.Equal("parameter", name);
            Assert.NotEqual<Guid>(Guid.Empty, g);
        }

        [Fact]
        public void CreateDoubleStringParameterizedType()
        {
            // Arrange
            var container = Scenario.CreateContainer();
            // Act
            var result = (DoubleParameterType<string, string>)container.Resolve(typeof(DoubleParameterType<string, string>));
            // Assert
            Assert.False(string.IsNullOrEmpty(result.Parameter1), "Parameter1");
            Assert.False(string.IsNullOrEmpty(result.Parameter2), "Parameter2");
        }

        [Fact]
        public void CreateStringAndIntegerParameterizedType()
        {
            // Arrange
            var container = Scenario.CreateContainer();
            // Act
            var result = (DoubleParameterType<string, int>)container.Resolve(typeof(DoubleParameterType<string, int>));
            // Assert
            Assert.False(string.IsNullOrEmpty(result.Parameter1), "Parameter11");
            Assert.NotEqual(0, result.Parameter2);
        }

        [Fact]
        public void CreateDecimalAndBooleanParameterizedType()
        {
            // Arrange
            var container = Scenario.CreateContainer();
            // Act
            var result = (DoubleParameterType<decimal, bool>)container.Resolve(typeof(DoubleParameterType<decimal, bool>));
            // Assert
            Assert.Equal(1m, result.Parameter1);
            Assert.True(result.Parameter2, "Parameter2");
        }

        [Fact]
        public void CreateNestedType()
        {
            // Arrange
            var container = Scenario.CreateContainer();
            // Act
            var result = (DoubleParameterType<DoubleParameterType<int, Guid>, DoubleParameterType<decimal, bool>>)container.Resolve(
                typeof(DoubleParameterType<DoubleParameterType<int, Guid>, DoubleParameterType<decimal, bool>>));
            // Assert
            Assert.Equal(1, result.Parameter1.Parameter1);
            Assert.NotEqual(default(Guid), result.Parameter1.Parameter2);
            Assert.Equal(1m, result.Parameter2.Parameter1);
            Assert.True(result.Parameter2.Parameter2);
        }

        [Fact]
        [Obsolete]
        public void CreateDoubleMixedParameterizedTypeWithNumberBasedStringGeneratorObsolete()
        {
            // Arrange
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
            // Act
            var result = (TripleParameterType<int, string, int>)container.Resolve(typeof(TripleParameterType<int, string, int>));
            // Assert
            Assert.Equal(1, result.Parameter1);
            Assert.Equal("parameter22", result.Parameter2);
            Assert.Equal(3, result.Parameter3);
        }

        [Fact]
        [Obsolete]
        public void CreateDoubleMixedParameterizedTypeWithNumberBasedStringGenerator()
        {
            // Arrange
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
            // Act
            var result = (TripleParameterType<int, string, int>)container.Resolve(typeof(TripleParameterType<int, string, int>));
            // Assert
            Assert.Equal(1, result.Parameter1);
            Assert.Equal("parameter22", result.Parameter2);
            Assert.Equal(3, result.Parameter3);
        }

        [Fact]
        public void CreateAndAddPropertyValues()
        {
            // Arrange
            var ctorInvoker = new MethodInvoker(new ModestConstructorQuery());
            var strCmd = new BindingCommand<DoublePropertyHolder<string, int>, string>(ph => ph.Property1);
            var intCmd = new BindingCommand<DoublePropertyHolder<string, int>, int>(ph => ph.Property2);
            var strPostprocessor = new Postprocessor(ctorInvoker, strCmd);
            var intPostprocessor = new Postprocessor(strPostprocessor, intCmd);

            var builder = new CompositeSpecimenBuilder(
                new FilteringSpecimenBuilder(intPostprocessor, new ExactTypeSpecification(typeof(DoublePropertyHolder<string, int>))),
                Scenario.CreateAutoPropertyBuilder());
            var container = new SpecimenContext(builder);
            // Act
            var result = container.Resolve(typeof(DoublePropertyHolder<string, int>));
            // Assert
            var actual = Assert.IsAssignableFrom<DoublePropertyHolder<string, int>>(result);
            Assert.False(string.IsNullOrEmpty(actual.Property1), "Property1");
            Assert.Equal(1, actual.Property2);
        }

        [Fact]
        public void CreateUsingBasicAutoPropertiesFunctionality()
        {
            // Arrange
            var container = Scenario.CreateContainer();
            // Act
            var result = container.Resolve(typeof(DoublePropertyHolder<int, int>));
            // Assert
            var actual = Assert.IsAssignableFrom<DoublePropertyHolder<int, int>>(result);
            Assert.Equal(1, actual.Property1);
            Assert.Equal(2, actual.Property2);
        }

        [Fact]
        public void CreateNestedStringTypeWithAutoProperties()
        {
            // Arrange
            var container = Scenario.CreateContainer();
            // Act
            var result = container.Resolve(typeof(DoublePropertyHolder<DoublePropertyHolder<string, string>, DoublePropertyHolder<string, string>>));
            // Assert
            var actual = Assert.IsAssignableFrom<DoublePropertyHolder<DoublePropertyHolder<string, string>, DoublePropertyHolder<string, string>>>(result);
            Assert.False(string.IsNullOrEmpty(actual.Property1.Property1));
            Assert.False(string.IsNullOrEmpty(actual.Property1.Property2));
            Assert.False(string.IsNullOrEmpty(actual.Property2.Property1));
            Assert.False(string.IsNullOrEmpty(actual.Property2.Property2));
        }

        [Fact]
        public void CreateNestedIntegerTypeWithAutoProperties()
        {
            // Arrange
            var container = Scenario.CreateContainer();
            // Act
            var result = container.Resolve(typeof(DoublePropertyHolder<DoublePropertyHolder<int, int>, DoublePropertyHolder<int, int>>));
            // Assert
            var actual = Assert.IsAssignableFrom<DoublePropertyHolder<DoublePropertyHolder<int, int>, DoublePropertyHolder<int, int>>>(result);
            Assert.Equal(1, actual.Property1.Property1);
            Assert.Equal(2, actual.Property1.Property2);
            Assert.Equal(3, actual.Property2.Property1);
            Assert.Equal(4, actual.Property2.Property2);
        }

        [Fact]
        public void CombineExplicitPropertyWithAutoProperties()
        {
            // Arrange
            var expectedText = "Fnaah";

            var specifiedCommand = new BindingCommand<DoublePropertyHolder<string, int>, string>(ph => ph.Property1, expectedText);
            var reservedProperty = new InverseRequestSpecification(specifiedCommand);

            var customizedBuilder = new Postprocessor(
                new Postprocessor(
                    new MethodInvoker(new ModestConstructorQuery()),
                    specifiedCommand),
                new AutoPropertiesCommand(reservedProperty),
                new AnyTypeSpecification());

            var builder = new CompositeSpecimenBuilder(
                customizedBuilder,
                Scenario.CreateAutoPropertyBuilder());
            var container = new SpecimenContext(builder);
            // Act
            var result = container.Resolve(typeof(DoublePropertyHolder<string, int>));
            // Assert
            var actual = Assert.IsAssignableFrom<DoublePropertyHolder<string, int>>(result);
            Assert.Equal(expectedText, actual.Property1);
            Assert.Equal(1, actual.Property2);
        }

        [Fact]
        public void RequestFiniteSequenceReturnsCorrectResult()
        {
            // Arrange
            var container = Scenario.CreateContainer();
            // Act
            var result = container.Resolve(new FiniteSequenceRequest(typeof(int), 10));
            // Assert
            var actual = Assert.IsAssignableFrom<IEnumerable<object>>(result);
            Assert.True(Enumerable.Range(1, 10).Cast<object>().SequenceEqual(actual));
        }

        [Fact]
        public void RequestManyReturnsCorrectResult()
        {
            // Arrange
            var container = Scenario.CreateContainer();
            // Act
            var result = container.Resolve(new MultipleRequest(typeof(int)));
            // Assert
            var actual = Assert.IsAssignableFrom<IEnumerable<object>>(result);
            Assert.True(Enumerable.Range(1, 3).Cast<object>().SequenceEqual(actual));
        }

        [Fact]
        [Obsolete]
        public void CreateAnonymousReturnsCorrectResult()
        {
            // Arrange
            var container = Scenario.CreateContainer();
            // Act
            var result = container.CreateAnonymous<int>();
            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void CreateReturnsCorrectResult()
        {
            // Arrange
            var container = Scenario.CreateContainer();
            // Act
            var result = container.Create<int>();
            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void CreateManyReturnsCorrectResult()
        {
            // Arrange
            var container = Scenario.CreateContainer();
            // Act
            var result = container.CreateMany<decimal>();
            // Assert
            Assert.True(Enumerable.Range(1, 3).Select(i => (decimal)i).SequenceEqual(result));
        }

        [Fact]
        public void CreateManyWithCountReturnsCorrectResult()
        {
            // Arrange
            var count = 8;
            var container = Scenario.CreateContainer();
            // Act
            var result = container.CreateMany<long>(count);
            // Assert
            Assert.True(Enumerable.Range(1, count).Select(i => (long)i).SequenceEqual(result));
        }

        [Fact]
        [Obsolete]
        public void ComposeWithValueReturnsCorrectResultObsolete()
        {
            // Arrange
            var expectedValue = 9;
            var customBuilder = SpecimenBuilderNodeFactory.CreateComposer<PropertyHolder<int>>()
                .With(x => x.Property, expectedValue);
            var builder = new CompositeSpecimenBuilder(
                customBuilder,
                Scenario.CreateCoreBuilder());
            // Act
            var result = new SpecimenContext(builder).CreateAnonymous<PropertyHolder<int>>();
            // Assert
            Assert.Equal(expectedValue, result.Property);
        }

        [Fact]
        public void ComposeWithValueReturnsCorrectResult()
        {
            // Arrange
            var expectedValue = 9;
            var customBuilder = SpecimenBuilderNodeFactory.CreateComposer<PropertyHolder<int>>()
                .With(x => x.Property, expectedValue);
            var builder = new CompositeSpecimenBuilder(
                customBuilder,
                Scenario.CreateCoreBuilder());
            // Act
            var result = new SpecimenContext(builder).Create<PropertyHolder<int>>();
            // Assert
            Assert.Equal(expectedValue, result.Property);
        }

        [Fact]
        [Obsolete]
        public void ComposeWithAutoPropertiesAndExplicitPropertyObsolete()
        {
            // Arrange
            var customBuilder = SpecimenBuilderNodeFactory.CreateComposer<DoublePropertyHolder<int, int>>()
                .WithAutoProperties()
                .With(x => x.Property1, 8);
            var builder = new CompositeSpecimenBuilder(
                customBuilder,
                Scenario.CreateCoreBuilder());
            // Act
            var result = new SpecimenContext(builder).CreateAnonymous<DoublePropertyHolder<int, int>>();
            // Assert
            Assert.Equal(8, result.Property1);
            Assert.Equal(1, result.Property2);
        }

        [Fact]
        public void ComposeWithAutoPropertiesAndExplicitProperty()
        {
            // Arrange
            var customBuilder = SpecimenBuilderNodeFactory.CreateComposer<DoublePropertyHolder<int, int>>()
                .WithAutoProperties()
                .With(x => x.Property1, 8);
            var builder = new CompositeSpecimenBuilder(
                customBuilder,
                Scenario.CreateCoreBuilder());
            // Act
            var result = new SpecimenContext(builder).Create<DoublePropertyHolder<int, int>>();
            // Assert
            Assert.Equal(8, result.Property1);
            Assert.Equal(1, result.Property2);
        }

        [Fact]
        [Obsolete]
        public void ComposeWithAutoPropertiesObsolete()
        {
            // Arrange
            var customBuilder = SpecimenBuilderNodeFactory.CreateComposer<DoublePropertyHolder<int, int>>()
                .WithAutoProperties();
            var builder = new CompositeSpecimenBuilder(
                customBuilder,
                Scenario.CreateCoreBuilder());
            // Act
            var result = new SpecimenContext(builder).CreateAnonymous<DoublePropertyHolder<int, int>>();
            // Assert
            Assert.Equal(1, result.Property1);
            Assert.Equal(2, result.Property2);
        }

        [Fact]
        public void ComposeWithAutoProperties()
        {
            // Arrange
            var customBuilder = SpecimenBuilderNodeFactory.CreateComposer<DoublePropertyHolder<int, int>>()
                .WithAutoProperties();
            var builder = new CompositeSpecimenBuilder(
                customBuilder,
                Scenario.CreateCoreBuilder());
            // Act
            var result = new SpecimenContext(builder).Create<DoublePropertyHolder<int, int>>();
            // Assert
            Assert.Equal(1, result.Property1);
            Assert.Equal(2, result.Property2);
        }

        [Fact]
        [Obsolete]
        public void ComposeComplexObjectWithAutoPropertiesAndSomeCustomizationsObsolete()
        {
            // Arrange
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
            // Act
            var result = new SpecimenContext(builder).CreateAnonymous<DoublePropertyHolder<DoublePropertyHolder<long, long>, DoublePropertyHolder<int, string>>>();
            // Assert
            Assert.Equal(1, result.Property1.Property1);
            Assert.Equal(43, result.Property1.Property2);
            Assert.Equal(1, result.Property2.Property1);
            Assert.Null(result.Property2.Property2);
        }

        [Fact]
        public void ComposeComplexObjectWithAutoPropertiesAndSomeCustomizations()
        {
            // Arrange
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
            // Act
            var result = new SpecimenContext(builder).Create<DoublePropertyHolder<DoublePropertyHolder<long, long>, DoublePropertyHolder<int, string>>>();
            // Assert
            Assert.Equal(1, result.Property1.Property1);
            Assert.Equal(43, result.Property1.Property2);
            Assert.Equal(1, result.Property2.Property1);
            Assert.Null(result.Property2.Property2);
        }

        [Fact]
        [Obsolete]
        public void CustomDoSetsCorrectPropertyObsolete()
        {
            // Arrange
            var builder = new CompositeSpecimenBuilder(
                SpecimenBuilderNodeFactory.CreateComposer<PropertyHolder<decimal>>().OmitAutoProperties().Do(x => x.SetProperty(6789)),
                Scenario.CreateCoreBuilder());
            // Act
            var result = new SpecimenContext(builder).CreateAnonymous<SingleParameterType<PropertyHolder<decimal>>>();
            // Assert
            Assert.Equal(6789, result.Parameter.Property);
        }

        [Fact]
        public void CustomDoSetsCorrectProperty()
        {
            // Arrange
            var builder = new CompositeSpecimenBuilder(
                SpecimenBuilderNodeFactory.CreateComposer<PropertyHolder<decimal>>().OmitAutoProperties().Do(x => x.SetProperty(6789)),
                Scenario.CreateCoreBuilder());
            // Act
            var result = new SpecimenContext(builder).Create<SingleParameterType<PropertyHolder<decimal>>>();
            // Assert
            Assert.Equal(6789, result.Parameter.Property);
        }

        [Fact]
        [Obsolete]
        public void ComposeWithoutCorrectlyCreatesSpecimenObsolete()
        {
            // Arrange
            var builder = new CompositeSpecimenBuilder(
                SpecimenBuilderNodeFactory.CreateComposer<DoubleFieldHolder<string, int>>().WithAutoProperties().Without(x => x.Field1),
                Scenario.CreateCoreBuilder());
            // Act
            var result = new SpecimenContext(builder).CreateAnonymous<DoubleFieldHolder<string, int>>();
            // Assert
            Assert.Null(result.Field1);
            Assert.Equal(1, result.Field2);
        }

        [Fact]
        public void ComposeWithoutCorrectlyCreatesSpecimen()
        {
            // Arrange
            var builder = new CompositeSpecimenBuilder(
                SpecimenBuilderNodeFactory.CreateComposer<DoubleFieldHolder<string, int>>().WithAutoProperties().Without(x => x.Field1),
                Scenario.CreateCoreBuilder());
            // Act
            var result = new SpecimenContext(builder).Create<DoubleFieldHolder<string, int>>();
            // Assert
            Assert.Null(result.Field1);
            Assert.Equal(1, result.Field2);
        }

        [Fact]
        [Obsolete]
        public void CustomizeFromFactoryCorrectlyResolvesSpecimenObsolete()
        {
            // Arrange
            var instance = new PropertyHolder<float> { Property = 89 };
            var builder = new CompositeSpecimenBuilder(
                SpecimenBuilderNodeFactory.CreateComposer<PropertyHolder<float>>().FromFactory(() => instance).OmitAutoProperties(),
                Scenario.CreateCoreBuilder());
            // Act
            var result = new SpecimenContext(builder).CreateAnonymous<PropertyHolder<float>>();
            // Assert
            Assert.Equal(instance, result);
            Assert.Equal(89, result.Property);
        }

        [Fact]
        public void CustomizeFromFactoryCorrectlyResolvesSpecimen()
        {
            // Arrange
            var instance = new PropertyHolder<float> { Property = 89 };
            var builder = new CompositeSpecimenBuilder(
                SpecimenBuilderNodeFactory.CreateComposer<PropertyHolder<float>>().FromFactory(() => instance).OmitAutoProperties(),
                Scenario.CreateCoreBuilder());
            // Act
            var result = new SpecimenContext(builder).Create<PropertyHolder<float>>();
            // Assert
            Assert.Equal(instance, result);
            Assert.Equal(89, result.Property);
        }

        [Fact]
        [Obsolete]
        public void CustomizeAndComposeComplexTypeObsolete()
        {
            // Arrange
            // Act
            var result = new CompositeNodeComposer<DoublePropertyHolder<int, decimal>>(
                new CompositeSpecimenBuilder(
                    SpecimenBuilderNodeFactory.CreateComposer<DoublePropertyHolder<int, decimal>>(),
                    Scenario.CreateAutoPropertyBuilder()
                    )
                ).With(x => x.Property2, 8m).WithAutoProperties().CreateAnonymous();
            // Assert
            Assert.Equal(1, result.Property1);
            Assert.Equal(8, result.Property2);
        }

        [Fact]
        public void CustomizeAndComposeComplexType()
        {
            // Arrange
            // Act
            var result = new CompositeNodeComposer<DoublePropertyHolder<int, decimal>>(
                new CompositeSpecimenBuilder(
                    SpecimenBuilderNodeFactory.CreateComposer<DoublePropertyHolder<int, decimal>>(),
                    Scenario.CreateAutoPropertyBuilder()
                    )
                ).With(x => x.Property2, 8m).WithAutoProperties().Create();
            // Assert
            Assert.Equal(1, result.Property1);
            Assert.Equal(8, result.Property2);
        }

        [Fact]
        public void DisposeBehaviorDisposesSpecimen()
        {
            // Arrange
            var behavior = new DisposableTrackingBehavior();
            var fixture = new Fixture();
            fixture.Behaviors.Add(behavior);

            var disp = fixture.Create<DisposableSpy>();
            // Act
            behavior.Dispose();
            // Assert
            Assert.True(disp.Disposed);
        }

        [Fact]
        public void DisposeCustomizationDisposesSpecimen()
        {
            // Arrange
            var customization = new DisposableTrackingCustomization();
            var fixture = new Fixture().Customize(customization);

            var disp = fixture.Create<DisposableSpy>();
            // Act
            customization.Dispose();
            // Assert
            Assert.True(disp.Disposed);
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
