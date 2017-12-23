using System;
using System.Reflection;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class AutoPropertiesCommandTest : IDisposable
    {
        [Fact]
        [Obsolete]
        public void SutIsSpecifiedSpecimenCommand()
        {
            // Arrange
            // Act
            var sut = new AutoPropertiesCommand<string>();
            // Assert
            Assert.IsAssignableFrom<ISpecifiedSpecimenCommand<string>>(sut);
        }

        [Fact]
        [Obsolete]
        public void ExecuteWithNullSpecimenThrows()
        {
            // Arrange
            var sut = new AutoPropertiesCommand<object>();
            var dummyContainer = new DelegatingSpecimenContext();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => sut.Execute(null, dummyContainer));
        }

        [Fact]
        [Obsolete]
        public void ExecuteWithNullContainerThrows()
        {
            // Arrange
            var sut = new AutoPropertiesCommand<object>();
            var dummySpecimen = new object();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => sut.Execute(dummySpecimen, null));
        }

        [Fact]
        [Obsolete]
        public void ExecuteWillAssignCorrectFieldValue()
        {
            // Arrange
            var expectedFieldValue = new object();
            var container = new DelegatingSpecimenContext { OnResolve = r => expectedFieldValue };

            var sut = new AutoPropertiesCommand<FieldHolder<object>>();

            var specimen = new FieldHolder<object>();
            // Act
            sut.Execute((object)specimen, container);
            // Assert
            Assert.Equal(expectedFieldValue, specimen.Field);
        }

        [Fact]
        [Obsolete]
        public void ExecuteWillAssignCorrectPropertyValue()
        {
            // Arrange
            var expectedPropertyValue = new object();
            var container = new DelegatingSpecimenContext { OnResolve = r => expectedPropertyValue };

            var sut = new AutoPropertiesCommand<PropertyHolder<object>>();

            var specimen = new PropertyHolder<object>();
            // Act
            sut.Execute((object)specimen, container);
            // Assert
            Assert.Equal(expectedPropertyValue, specimen.Property);
        }

        [Fact]
        [Obsolete]
        public void ExecuteDoesNotSetReadOnlyProperty()
        {
            // Arrange
            var sut = new AutoPropertiesCommand<SingleParameterType<object>>();
            var specimen = new SingleParameterType<object>(new object());
            var unexpectedValue = new object();
            var container = new DelegatingSpecimenContext { OnResolve = r => unexpectedValue };
            // Act
            sut.Execute((object)specimen, container);
            // Assert
            Assert.NotEqual(unexpectedValue, specimen.Parameter);
        }

        [Fact]
        [Obsolete]
        public void ExecuteDoesNotThrowOnIndexedProperty()
        {
            // Arrange
            var sut = new AutoPropertiesCommand<IndexedPropertyHolder<object>>();
            var specimen = new IndexedPropertyHolder<object>();
            var container = new DelegatingSpecimenContext { OnResolve = r => new object() };
            // Act & assert
            Assert.Null(Record.Exception(() =>
                sut.Execute((object)specimen, container)));
        }

        [Fact]
        [Obsolete]
        public void ExecuteDoesNotSetStaticProperty()
        {
            // Arrange
            var sut = new AutoPropertiesCommand<StaticPropertyHolder<object>>();
            var specimen = new StaticPropertyHolder<object>();
            var container = new DelegatingSpecimenContext { OnResolve = r => new object() };
            // Act
            sut.Execute((object)specimen, container);
            // Assert
            Assert.Null(StaticPropertyHolder<object>.Property);
        }

        [Fact]
        [Obsolete]
        void ExecuteDoesNotSetStaticField()
        {
            // Arrange
            var sut = new AutoPropertiesCommand<StaticFieldHolder<object>>();
            var specimen = new StaticFieldHolder<object>();
            var container = new DelegatingSpecimenContext { OnResolve = r => new object() };
            // Act
            sut.Execute((object)specimen, container);
            // Assert
            Assert.Null(StaticFieldHolder<object>.Field);
        }

        [Fact]
        [Obsolete]
        public void ExecuteDoesNotSetReadonlyField()
        {
            // Arrange
            var sut = new AutoPropertiesCommand<ReadOnlyFieldHolder<object>>();
            var specimen = new ReadOnlyFieldHolder<object>();
            var unexpectedValue = new object();
            var container = new DelegatingSpecimenContext { OnResolve = r => unexpectedValue };
            // Act
            sut.Execute((object)specimen, container);
            // Assert
            Assert.NotEqual(unexpectedValue, specimen.Field);
        }

        [Fact]
        [Obsolete]
        public void InitializeWithNullSpecificationThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => new AutoPropertiesCommand<object>(null));
        }

        [Fact]
        [Obsolete]
        public void ExecuteDoesNotAssignPropertyWhenSpecificationIsNotSatisfied()
        {
            // Arrange
            var spec = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => false };
            var sut = new AutoPropertiesCommand<PropertyHolder<object>>(spec);
            var specimen = new PropertyHolder<object>();
            var container = new DelegatingSpecimenContext { OnResolve = r => new object() };
            // Act
            sut.Execute((object)specimen, container);
            // Assert
            Assert.Null(specimen.Property);
        }

        [Fact]
        [Obsolete]
        public void ExecuteWillQuerySpecificationWithCorrectPropertyInfo()
        {
            // Arrange
            var expectedPropertyInfo = typeof(PropertyHolder<string>).GetProperty("Property");
            var verified = false;
            var specMock = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => verified = expectedPropertyInfo.Equals(r) };
            var sut = new AutoPropertiesCommand<PropertyHolder<string>>(specMock);
            // Act
            var specimen = new PropertyHolder<string>();
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Execute((object)specimen, dummyContainer);
            // Assert
            Assert.True(verified, "Mock verified");
        }

        [Fact]
        [Obsolete]
        public void ExecuteWillNotAssignPropertyWhenContextReturnsOmitSpecimen()
        {
            // Arrange
            var sut = new AutoPropertiesCommand<PropertyHolder<object>>();
            var specimen = new PropertyHolder<object>();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => new OmitSpecimen()
            };
            // Act
            sut.Execute((object)specimen, context);
            // Assert
            Assert.Null(specimen.Property);
        }

        [Fact]
        [Obsolete]
        public void ExecuteDoesNotAssignFieldWhenSpecificationIsNotSatisfied()
        {
            // Arrange
            var spec = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => false };
            var sut = new AutoPropertiesCommand<FieldHolder<object>>(spec);
            var specimen = new FieldHolder<object>();
            var container = new DelegatingSpecimenContext { OnResolve = r => new object() };
            // Act
            sut.Execute((object)specimen, container);
            // Assert
            Assert.Null(specimen.Field);
        }

        [Fact]
        [Obsolete]
        public void ExecuteWillQuerySpecificationWithCorrectFieldInfo()
        {
            // Arrange
            var expectedFieldInfo = typeof(FieldHolder<string>).GetField("Field");
            var verified = false;
            var specMock = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => verified = expectedFieldInfo.Equals(r) };
            var sut = new AutoPropertiesCommand<FieldHolder<string>>(specMock);
            // Act
            var specimen = new FieldHolder<string>();
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Execute((object)specimen, dummyContainer);
            // Assert
            Assert.True(verified, "Mock verified");
        }

        [Fact]
        [Obsolete]
        public void ExecuteWillNotAssignFieldWhenContextReturnsOmitSpecimen()
        {
            // Arrange
            var sut = new AutoPropertiesCommand<FieldHolder<object>>();
            var specimen = new FieldHolder<object>();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r => new OmitSpecimen()
            };
            // Act
            sut.Execute((object)specimen, context);
            // Assert
            Assert.Null(specimen.Field);
        }

        [Fact]
        [Obsolete]
        public void IsSatisfiedByNullThrows()
        {
            // Arrange
            var sut = new AutoPropertiesCommand<object>();
            // Act & assert
#pragma warning disable 618
            Assert.Throws<ArgumentNullException>(() => sut.IsSatisfiedBy(null));
#pragma warning restore 618
        }

        [Fact]
        [Obsolete]
        public void IsSatisfiedByAnonymousRequestReturnsCorrectResult()
        {
            // Arrange
            var sut = new AutoPropertiesCommand<PropertyHolder<object>>();
            var anonymousRequest = new object();
            // Act
#pragma warning disable 618
            var result = sut.IsSatisfiedBy(anonymousRequest);
#pragma warning restore 618
            // Assert
            Assert.False(result);
        }

        [Fact]
        [Obsolete]
        public void IsSatisfiedByUnfilteredPropertyInfoReturnsCorrectResult()
        {
            // Arrange
            var request = typeof(PropertyHolder<object>).GetProperty("Property");
            var sut = new AutoPropertiesCommand<PropertyHolder<object>>();
            // Act
#pragma warning disable 618
            var result = sut.IsSatisfiedBy(request);
#pragma warning restore 618
            // Assert
            Assert.True(result);
        }

        [Fact]
        [Obsolete]
        public void IsSatisfiedByReadOnlyPropertyReturnsCorrectResult()
        {
            // Arrange
            var request = typeof(SingleParameterType<object>).GetProperty("Parameter");
            var sut = new AutoPropertiesCommand<SingleParameterType<object>>();
            // Act
#pragma warning disable 618
            var result = sut.IsSatisfiedBy(request);
#pragma warning restore 618
            // Assert
            Assert.False(result);
        }

        [Fact]
        [Obsolete]
        public void IsSatisfiedByIndexedPropertyReturnsCorrectResult()
        {
            // Arrange
            var request = typeof(IndexedPropertyHolder<string>).GetProperty("Item");
            var sut = new AutoPropertiesCommand<IndexedPropertyHolder<string>>();
            // Act
#pragma warning disable 618
            var result = sut.IsSatisfiedBy(request);
#pragma warning restore 618
            // Assert
            Assert.False(result);
        }

        [Fact]
        [Obsolete]
        public void IsSatisfiedByFilteredPropertyInfoReturnsCorrectResult()
        {
            // Arrange
            var spec = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => false };
            var sut = new AutoPropertiesCommand<PropertyHolder<object>>(spec);
            var request = typeof(PropertyHolder<object>).GetProperty("Property");
            // Act
#pragma warning disable 618
            var result = sut.IsSatisfiedBy(request);
#pragma warning restore 618
            // Assert
            Assert.False(result);
        }

        [Fact]
        [Obsolete]
        public void IsSatisfiedByWillInvokeSpecificationWithCorrectPropertyInfo()
        {
            // Arrange
            var expectedRequest = typeof(PropertyHolder<object>).GetProperty("Property");
            var verified = false;
            var specMock = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => verified = expectedRequest.Equals(r) };
            var sut = new AutoPropertiesCommand<PropertyHolder<object>>(specMock);
            // Act
#pragma warning disable 618
            sut.IsSatisfiedBy(expectedRequest);
#pragma warning restore 618
            // Assert
            Assert.True(verified, "Mock verified");
        }

        [Fact]
        [Obsolete]
        public void IsSatisfiedByUnfilteredFieldInfoReturnsCorrectResult()
        {
            // Arrange
            var request = typeof(FieldHolder<object>).GetField("Field");
            var sut = new AutoPropertiesCommand<FieldHolder<object>>();
            // Act
#pragma warning disable 618
            var result = sut.IsSatisfiedBy(request);
#pragma warning restore 618
            // Assert
            Assert.True(result);
        }

        [Fact]
        [Obsolete]
        public void IsSatisfiedByFilteredFieldInfoReturnsCorrectResult()
        {
            // Arrange
            var spec = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => false };
            var sut = new AutoPropertiesCommand<FieldHolder<object>>(spec);
            var request = typeof(FieldHolder<object>).GetField("Field");
            // Act
#pragma warning disable 618
            var result = sut.IsSatisfiedBy(request);
#pragma warning restore 618
            // Assert
            Assert.False(result);
        }

        [Fact]
        [Obsolete]
        public void IsSatisfiedByWillInvokeSpecificationWithCorrectFieldInfo()
        {
            // Arrange
            var expectedRequest = typeof(FieldHolder<object>).GetField("Field");
            var verified = false;
            var specMock = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => verified = expectedRequest.Equals(r) };
            var sut = new AutoPropertiesCommand<FieldHolder<object>>(specMock);
            // Act
#pragma warning disable 618
            sut.IsSatisfiedBy(expectedRequest);
#pragma warning restore 618
            // Assert
            Assert.True(verified, "Mock verified");
        }

        [Fact]
        [Obsolete]
        public void NonGenericSutIsCorrectGenericSut()
        {
            // Arrange
            var dummyType = typeof(string);
            // Act
            var sut = new AutoPropertiesCommand(dummyType);
            // Assert
            Assert.IsAssignableFrom<AutoPropertiesCommand<object>>(sut);
        }

        [Fact]
        public void InitializeNonGenericSutWithNullTypeThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => new AutoPropertiesCommand((Type)null));
        }

        [Fact]
        public void InitializeNonGenericSutWithNullSpecificationThrows()
        {
            // Arrange
            var dummyType = typeof(object);
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => new AutoPropertiesCommand(dummyType, null));
        }

        [Fact]
        public void InitializeNonGenericSutWithOnlyNullSpecificationThrow()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(
                () => new AutoPropertiesCommand((IRequestSpecification)null));
        }

        [Fact]
        public void ExecuteOnNonGenericWillAssignProperty()
        {
            // Arrange
            var specimen = new PropertyHolder<object>();
            var sut = new AutoPropertiesCommand(specimen.GetType());

            var expectedPropertyValue = new object();
            var container = new DelegatingSpecimenContext { OnResolve = r => expectedPropertyValue };
            // Act
            sut.Execute(specimen, container);
            // Assert
            Assert.Equal(expectedPropertyValue, specimen.Property);
        }

        [Fact]
        public void ExecuteOnUnTypedNonGenericWillAssignProperty()
        {
            // Arrange
            var sut = new AutoPropertiesCommand();

            var expectedPropertyValue = new object();
            var container = new DelegatingSpecimenContext { OnResolve = r => expectedPropertyValue };

            var specimen = new PropertyHolder<object>();
            // Act
            sut.Execute(specimen, container);
            // Assert
            Assert.Equal(expectedPropertyValue, specimen.Property);
        }

        [Fact]
        public void ExecuteOnNonGenericTrueSpecifiedAssignsProperty()
        {
            // Arrange
            var trueSpecification = new DelegatingRequestSpecification
            {
                OnIsSatisfiedBy = x => true
            };
            var sut = new AutoPropertiesCommand(trueSpecification);

            var expectedPropertyValue = new object();
            var context = new DelegatingSpecimenContext { OnResolve = r => expectedPropertyValue };

            var specimen = new PropertyHolder<object>();
            // Act
            sut.Execute(specimen, context);
            // Assert
            Assert.Equal(expectedPropertyValue, specimen.Property);
        }

        [Fact]
        public void ExecuteOnNonGenericFalseSpecifiedDoesNotAssignProperty()
        {
            // Arrange
            var falseSpecification = new DelegatingRequestSpecification
            {
                OnIsSatisfiedBy = x => false
            };
            var sut = new AutoPropertiesCommand(falseSpecification);

            var dummyPropertyValue = new object();
            var context = new DelegatingSpecimenContext { OnResolve = r => dummyPropertyValue };

            var specimen = new PropertyHolder<object>();
            // Act
            sut.Execute(specimen, context);
            // Assert
            Assert.NotEqual(dummyPropertyValue, specimen.Property);
        }

        [Fact]
        [Obsolete]
        public void SutIsSpecimenCommand()
        {
            var sut = new AutoPropertiesCommand<object>();
            Assert.IsAssignableFrom<ISpecimenCommand>(sut);
        }

        [Fact]
        public void NonTypedUsesExplicitlySpecifiedTypeForFieldsAndPropertiesResolve()
        {
            // Arrange
            var sut = new AutoPropertiesCommand(typeof(object));

            var dummyPropertyValue = new object();
            var context = new DelegatingSpecimenContext { OnResolve = r => dummyPropertyValue };

            var specimen = new PropertyHolder<object>();

            // Act
            sut.Execute(specimen, context);

            // Assert
            Assert.NotEqual(dummyPropertyValue, specimen.Property);
        }

        [Fact]
        public void NonTypedWithSpecificationUsesExplicitlySpecifiedTypeForFieldsAndPropertiesResolve()
        {
            // Arrange
            var trueSpec = new TrueRequestSpecification();
            var sut = new AutoPropertiesCommand(typeof(object), trueSpec);

            var dummyPropertyValue = new object();
            var context = new DelegatingSpecimenContext { OnResolve = r => dummyPropertyValue };

            var specimen = new PropertyHolder<object>();

            // Act
            sut.Execute(specimen, context);

            // Assert
            Assert.NotEqual(dummyPropertyValue, specimen.Property);
        }

        [Fact]
        public void NonTypedReturnsSpecimenTypeIfProvidedInCtor()
        {
            // Arrange
            var type = typeof(string);

            // Act
            var sut = new AutoPropertiesCommand(type);

            // Assert
            Assert.Equal(type, sut.ExplicitSpecimenType);
        }

        [Fact]
        public void NonTypedWithSpecificationReturnsSpecimenTypeIfProvidedInCtor()
        {
            // Arrange
            var type = typeof(string);
            var spec = new TrueRequestSpecification();

            // Act
            var sut = new AutoPropertiesCommand(type, spec);

            // Assert
            Assert.Equal(type, sut.ExplicitSpecimenType);
        }

        [Fact]
        public void NonTypedReturnsNullSpecimenTypeIfNotProvided()
        {
            // Arrange
            // Act
            var sut = new AutoPropertiesCommand();

            // Assert
            Assert.Null(sut.ExplicitSpecimenType);
        }

        [Fact]
        public void NonTypedWithSpecificationReturnsNullSpecimenTypeIfNotProvided()
        {
            // Arrange
            // Act
            var sut = new AutoPropertiesCommand();

            // Assert
            Assert.Null(sut.ExplicitSpecimenType);
        }


        public void Dispose()
        {
            StaticPropertyHolder<object>.Property = null;
            StaticFieldHolder<object>.Field = null;
        }
    }
}
