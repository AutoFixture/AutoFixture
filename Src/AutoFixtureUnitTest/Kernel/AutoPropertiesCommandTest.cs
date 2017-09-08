using System;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class AutoPropertiesCommandTest : IDisposable
    {
#pragma warning disable 618
        [Fact]
        public void SutIsSpecifiedSpecimenCommand()
        {
            // Fixture setup
            // Exercise system
            var sut = new AutoPropertiesCommand<string>();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecifiedSpecimenCommand<string>>(sut);
            // Teardown
        }
#pragma warning restore 618

        [Fact]
        public void ExecuteWithNullSpecimenThrows()
        {
            // Fixture setup
            var sut = new AutoPropertiesCommand<object>();
            var dummyContainer = new DelegatingSpecimenContext();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => sut.Execute(null, dummyContainer));
            // Teardown
        }

        [Fact]
        public void ExecuteWithNullContainerThrows()
        {
            // Fixture setup
            var sut = new AutoPropertiesCommand<object>();
            var dummySpecimen = new object();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => sut.Execute(dummySpecimen, null));
            // Teardown
        }

        [Fact]
        public void ExecuteWillAssignCorrectFieldValue()
        {
            // Fixture setup
            var expectedFieldValue = new object();
            var container = new DelegatingSpecimenContext { OnResolve = r => expectedFieldValue };

            var sut = new AutoPropertiesCommand<FieldHolder<object>>();

            var specimen = new FieldHolder<object>();
            // Exercise system
            sut.Execute(specimen, container);
            // Verify outcome
            Assert.Equal(expectedFieldValue, specimen.Field);
            // Teardown
        }

        [Fact]
        public void ExecuteWillAssignCorrectPropertyValue()
        {
            // Fixture setup
            var expectedPropertyValue = new object();
            var container = new DelegatingSpecimenContext { OnResolve = r => expectedPropertyValue };

            var sut = new AutoPropertiesCommand<PropertyHolder<object>>();

            var specimen = new PropertyHolder<object>();
            // Exercise system
            sut.Execute(specimen, container);
            // Verify outcome
            Assert.Equal(expectedPropertyValue, specimen.Property);
            // Teardown
        }

        [Fact]
        public void ExecuteDoesNotSetReadOnlyProperty()
        {
            // Fixture setup
            var sut = new AutoPropertiesCommand<SingleParameterType<object>>();
            var specimen = new SingleParameterType<object>(new object());
            var unexpectedValue = new object();
            var container = new DelegatingSpecimenContext { OnResolve = r => unexpectedValue };
            // Exercise system
            sut.Execute(specimen, container);
            // Verify outcome
            Assert.NotEqual(unexpectedValue, specimen.Parameter);
            // Teardown
        }

        [Fact]
        public void ExecuteDoesNotThrowOnIndexedProperty()
        {
            // Fixture setup
            var sut = new AutoPropertiesCommand<IndexedPropertyHolder<object>>();
            var specimen = new IndexedPropertyHolder<object>();
            var container = new DelegatingSpecimenContext { OnResolve = r => new object() };
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() =>
                sut.Execute(specimen, container)));
            // Teardown
        }

        [Fact]
        public void ExecuteDoesNotSetStaticProperty()
        {
            // Fixture setup
            var sut = new AutoPropertiesCommand<StaticPropertyHolder<object>>();
            var specimen = new StaticPropertyHolder<object>();
            var container = new DelegatingSpecimenContext { OnResolve = r => new object() };
            // Exercise system
            sut.Execute(specimen, container);
            // Verify outcome
            Assert.Null(StaticPropertyHolder<object>.Property);
            // Teardown
        }

        [Fact]
        public void ExecuteDoesNotSetStaticField()
        {
            // Fixture setup
            var sut = new AutoPropertiesCommand<StaticFieldHolder<object>>();
            var specimen = new StaticFieldHolder<object>();
            var container = new DelegatingSpecimenContext { OnResolve = r => new object() };
            // Exercise system
            sut.Execute(specimen, container);
            // Verify outcome
            Assert.Null(StaticFieldHolder<object>.Field);
            // Teardown
        }

        [Fact]
        public void ExecuteDoesNotSetReadonlyField()
        {
            // Fixture setup
            var sut = new AutoPropertiesCommand<ReadOnlyFieldHolder<object>>();
            var specimen = new ReadOnlyFieldHolder<object>();
            var unexpectedValue = new object();
            var container = new DelegatingSpecimenContext { OnResolve = r => unexpectedValue };
            // Exercise system
            sut.Execute(specimen, container);
            // Verify outcome
            Assert.NotEqual(unexpectedValue, specimen.Field);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullSpecificationThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new AutoPropertiesCommand<object>(null));
            // Teardown
        }

        [Fact]
        public void ExecuteDoesNotAssignPropertyWhenSpecificationIsNotSatisfied()
        {
            // Fixture setup
            var spec = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => false };
            var sut = new AutoPropertiesCommand<PropertyHolder<object>>(spec);
            var specimen = new PropertyHolder<object>();
            var container = new DelegatingSpecimenContext { OnResolve = r => new object() };
            // Exercise system
            sut.Execute(specimen, container);
            // Verify outcome
            Assert.Null(specimen.Property);
            // Teardown
        }

        [Fact]
        public void ExecuteWillQuerySpecificationWithCorrectPropertyInfo()
        {
            // Fixture setup
            var expectedPropertyInfo = typeof(PropertyHolder<string>).GetProperty("Property");
            var verified = false;
            var specMock = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => verified = expectedPropertyInfo.Equals(r) };
            var sut = new AutoPropertiesCommand<PropertyHolder<string>>(specMock);
            // Exercise system
            var specimen = new PropertyHolder<string>();
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Execute(specimen, dummyContainer);
            // Verify outcome
            Assert.True(verified, "Mock verified");
            // Teardown
        }

        [Fact]
        public void ExecuteWillNotAssignPropertyWhenContextReturnsOmitSpecimen()
        {
            // Fixture setup
            var sut = new AutoPropertiesCommand<PropertyHolder<object>>();
            var specimen = new PropertyHolder<object>();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => new OmitSpecimen()
            };
            // Exercise system
            sut.Execute(specimen, context);
            // Verify outcome
            Assert.Null(specimen.Property);
            // Teardown
        }

        [Fact]
        public void ExecuteDoesNotAssignFieldWhenSpecificationIsNotSatisfied()
        {
            // Fixture setup
            var spec = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => false };
            var sut = new AutoPropertiesCommand<FieldHolder<object>>(spec);
            var specimen = new FieldHolder<object>();
            var container = new DelegatingSpecimenContext { OnResolve = r => new object() };
            // Exercise system
            sut.Execute(specimen, container);
            // Verify outcome
            Assert.Null(specimen.Field);
            // Teardown
        }

        [Fact]
        public void ExecuteWillQuerySpecificationWithCorrectFieldInfo()
        {
            // Fixture setup
            var expectedFieldInfo = typeof(FieldHolder<string>).GetField("Field");
            var verified = false;
            var specMock = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => verified = expectedFieldInfo.Equals(r) };
            var sut = new AutoPropertiesCommand<FieldHolder<string>>(specMock);
            // Exercise system
            var specimen = new FieldHolder<string>();
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Execute(specimen, dummyContainer);
            // Verify outcome
            Assert.True(verified, "Mock verified");
            // Teardown
        }

        [Fact]
        public void ExecuteWillNotAssignFieldWhenContextReturnsOmitSpecimen()
        {
            // Fixture setup
            var sut = new AutoPropertiesCommand<FieldHolder<object>>();
            var specimen = new FieldHolder<object>();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => new OmitSpecimen()
            };
            // Exercise system
            sut.Execute(specimen, context);
            // Verify outcome
            Assert.Null(specimen.Field);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByNullThrows()
        {
            // Fixture setup
            var sut = new AutoPropertiesCommand<object>();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => sut.IsSatisfiedBy(null));
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByAnonymousRequestReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new AutoPropertiesCommand<PropertyHolder<object>>();
            var anonymousRequest = new object();
            // Exercise system
            var result = sut.IsSatisfiedBy(anonymousRequest);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByUnfilteredPropertyInfoReturnsCorrectResult()
        {
            // Fixture setup
            var request = typeof(PropertyHolder<object>).GetProperty("Property");
            var sut = new AutoPropertiesCommand<PropertyHolder<object>>();
            // Exercise system
            var result = sut.IsSatisfiedBy(request);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByReadOnlyPropertyReturnsCorrectResult()
        {
            // Fixture setup
            var request = typeof(SingleParameterType<object>).GetProperty("Parameter");
            var sut = new AutoPropertiesCommand<SingleParameterType<object>>();
            // Exercise system
            var result = sut.IsSatisfiedBy(request);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByIndexedPropertyReturnsCorrectResult()
        {
            // Fixture setup
            var request = typeof(IndexedPropertyHolder<string>).GetProperty("Item");
            var sut = new AutoPropertiesCommand<IndexedPropertyHolder<string>>();
            // Exercise system
            var result = sut.IsSatisfiedBy(request);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByFilteredPropertyInfoReturnsCorrectResult()
        {
            // Fixture setup
            var spec = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => false };
            var sut = new AutoPropertiesCommand<PropertyHolder<object>>(spec);
            var request = typeof(PropertyHolder<object>).GetProperty("Property");
            // Exercise system
            var result = sut.IsSatisfiedBy(request);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByWillInvokeSpecificationWithCorrectPropertyInfo()
        {
            // Fixture setup
            var expectedRequest = typeof(PropertyHolder<object>).GetProperty("Property");
            var verified = false;
            var specMock = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => verified = expectedRequest.Equals(r) };
            var sut = new AutoPropertiesCommand<PropertyHolder<object>>(specMock);
            // Exercise system
            sut.IsSatisfiedBy(expectedRequest);
            // Verify outcome
            Assert.True(verified, "Mock verified");
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByUnfilteredFieldInfoReturnsCorrectResult()
        {
            // Fixture setup
            var request = typeof(FieldHolder<object>).GetField("Field");
            var sut = new AutoPropertiesCommand<FieldHolder<object>>();
            // Exercise system
            var result = sut.IsSatisfiedBy(request);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByFilteredFieldInfoReturnsCorrectResult()
        {
            // Fixture setup
            var spec = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => false };
            var sut = new AutoPropertiesCommand<FieldHolder<object>>(spec);
            var request = typeof(FieldHolder<object>).GetField("Field");
            // Exercise system
            var result = sut.IsSatisfiedBy(request);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByWillInvokeSpecificationWithCorrectFieldInfo()
        {
            // Fixture setup
            var expectedRequest = typeof(FieldHolder<object>).GetField("Field");
            var verified = false;
            var specMock = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => verified = expectedRequest.Equals(r) };
            var sut = new AutoPropertiesCommand<FieldHolder<object>>(specMock);
            // Exercise system
            sut.IsSatisfiedBy(expectedRequest);
            // Verify outcome
            Assert.True(verified, "Mock verified");
            // Teardown
        }

        [Fact]
        public void NonGenericSutIsCorrectGenericSut()
        {
            // Fixture setup
            var dummyType = typeof(string);
            // Exercise system
            var sut = new AutoPropertiesCommand(dummyType);
            // Verify outcome
            Assert.IsAssignableFrom<AutoPropertiesCommand<object>>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeNonGenericSutWithNullTypeThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new AutoPropertiesCommand((Type)null));
            // Teardown
        }

        [Fact]
        public void InitializeNonGenericSutWithNullSpecificationThrows()
        {
            // Fixture setup
            var dummyType = typeof(object);
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new AutoPropertiesCommand(dummyType, null));
            // Teardown
        }

        [Fact]
        public void InitializeNonGenericSutWithOnlyNullSpecificationThrow()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(
                () => new AutoPropertiesCommand((IRequestSpecification)null));
            // Teardown
        }

        [Fact]
        public void ExecuteOnNonGenericWillAssignProperty()
        {
            // Fixture setup
            var specimen = new PropertyHolder<object>();
            var sut = new AutoPropertiesCommand(specimen.GetType());

            var expectedPropertyValue = new object();
            var container = new DelegatingSpecimenContext { OnResolve = r => expectedPropertyValue };
            // Exercise system
            sut.Execute(specimen, container);
            // Verify outcome
            Assert.Equal(expectedPropertyValue, specimen.Property);
            // Teardown
        }

        [Fact]
        public void ExecuteOnUnTypedNonGenericWillAssignProperty()
        {
            // Fixture setup
            var sut = new AutoPropertiesCommand();

            var expectedPropertyValue = new object();
            var container = new DelegatingSpecimenContext { OnResolve = r => expectedPropertyValue };

            var specimen = new PropertyHolder<object>();
            // Exercise system
            sut.Execute(specimen, container);
            // Verify outcome
            Assert.Equal(expectedPropertyValue, specimen.Property);
            // Teardown
        }

        [Fact]
        public void ExecuteOnNonGenericTrueSpecifiedAssignsProperty()
        {
            // Fixture setup
            var trueSpecification = new DelegatingRequestSpecification
            {
                OnIsSatisfiedBy = x => true 
            };
            var sut = new AutoPropertiesCommand(trueSpecification);

            var expectedPropertyValue = new object();
            var context = new DelegatingSpecimenContext { OnResolve = r => expectedPropertyValue };

            var specimen = new PropertyHolder<object>();
            // Exercise system
            sut.Execute(specimen, context);
            // Verify outcome
            Assert.Equal(expectedPropertyValue, specimen.Property);
            // Teardown
        }

        [Fact]
        public void ExecuteOnNonGenericFalseSpecifiedDoesNotAssignProperty()
        {
            // Fixture setup
            var falseSpecification = new DelegatingRequestSpecification
            {
                OnIsSatisfiedBy = x => false
            };
            var sut = new AutoPropertiesCommand(falseSpecification);

            var dummyPropertyValue = new object();
            var context = new DelegatingSpecimenContext { OnResolve = r => dummyPropertyValue };

            var specimen = new PropertyHolder<object>();
            // Exercise system
            sut.Execute(specimen, context);
            // Verify outcome
            Assert.NotEqual(dummyPropertyValue, specimen.Property);
            // Teardown
        }

        [Fact]
        public void SutIsSpecimenCommand()
        {
            var sut = new AutoPropertiesCommand<object>();
            Assert.IsAssignableFrom<ISpecimenCommand>(sut);
        }

        public void Dispose()
        {
            StaticPropertyHolder<object>.Property = null;
            StaticFieldHolder<object>.Field = null;
        }
    }
}
