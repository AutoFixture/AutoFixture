using System;
using System.Reflection;
using AutoFixture.AutoMoq.UnitTest.TestTypes;
using AutoFixture.Kernel;
using Moq;
using Xunit;

namespace AutoFixture.AutoMoq.UnitTest
{
    public class AutoMockPropertiesCommandTest
    {
        [Theory]
        [ClassData(typeof (ValidNonMockSpecimens))]
        public void ExecuteDoesNotThrowsWhenSpecimenIsValidNonMockSpecimen(object validNonMockSpecimen)
        {
            // Arrange
            var context = new Mock<ISpecimenContext>().Object;
            var sut = new AutoMockPropertiesCommand();
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Execute(validNonMockSpecimen, context)));
        }

        [Fact]
        public void ExecuteThrowsWhenContextIsNull()
        {
            // Arrange
            var specimen = new Mock<object>();
            var sut = new AutoMockPropertiesCommand();
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => sut.Execute(specimen, null));
        }

        [Fact]
        public void IgnoresNonMockSpecimens()
        {
            // Arrange
            var specimen = new object();
            var context = new Mock<ISpecimenContext>().Object;
            var sut = new AutoMockPropertiesCommand();
            // Act & Assert
            Assert.Null(Record.Exception(() => sut.Execute(specimen, context)));
        }

        [Fact]
        public void PopulatesPropertiesUsingContext()
        {
            // Arrange
            var specimen = new Mock<IInterfaceWithProperty>();

            const string frozenString = "a string";
            var contextStub = new Mock<ISpecimenContext>();
            contextStub.Setup(ctx => ctx.Resolve(specimen.Object.GetType().GetProperty("Property"))).Returns(frozenString);

            var sut = new AutoMockPropertiesCommand();
            // Act
            sut.Execute(specimen, contextStub.Object);
            // Assert
            specimen.VerifySet(s => s.Property = frozenString, Times.Once());
        }

        [Fact]
        public void PopulatesFields()
        {
            // Arrange
            var specimen = new Mock<TypeWithPublicField>();

            const string frozenString = "a string";
            var contextStub = new Mock<ISpecimenContext>();
            contextStub.Setup(ctx => ctx.Resolve(specimen.Object.GetType().GetField("Field"))).Returns(frozenString);

            var sut = new AutoMockPropertiesCommand();
            // Act
            sut.Execute(specimen, contextStub.Object);
            // Assert
            Assert.Equal(frozenString, specimen.Object.Field);
        }

        [Theory]
        [InlineData("__interceptors")]
        [InlineData("__target")]
        public void IgnoresProxyMembers(string proxyFieldName)
        {
            // Arrange
            var specimen = new Mock<IInterfaceWithoutMembers>();
            var proxyField = specimen.Object.GetType().GetField(proxyFieldName);
            var initialProxyFieldValue = proxyField?.GetValue(specimen.Object);

            var contextStub = new Mock<ISpecimenContext>();
            contextStub.Setup(ctx => ctx.Resolve(It.IsAny<FieldInfo>()))
                .Returns((FieldInfo fi) => fi.FieldType.IsArray ?
                    Array.CreateInstance(fi.FieldType.GetElementType(), 0) :
                    Activator.CreateInstance(fi.FieldType));

            var sut = new AutoMockPropertiesCommand();
            // Act
            sut.Execute(specimen, contextStub.Object);
            // Assert
            var finalProxyFieldValue = proxyField?.GetValue(specimen.Object);
            Assert.Same(initialProxyFieldValue, finalProxyFieldValue);
        }
    }
}
